#pragma strict

var nav : NavMeshAgent;
var target : Transform;
var maxChaseDistance : float = 10.0f;
var maxVisiblity : float = 200.0f; 
var attackRepeatTime : float = 1.0f;
var maxVisibleAngle : float = 80.0f;

private var distance : float = 100000.0;
private var chaseTime : float = 2.0f;
private var lastChasePosition : Vector3;
private var updateLastPos : boolean;
private var returnToChasePosition : boolean;
private var hit: RaycastHit;
private var EyeRotatorVal : float;

//////////////////////////////////////////

public var idleAnimation : AnimationClip;
public var runAnimation : AnimationClip;
public var attackPoseAnimation : AnimationClip;
public var getHitAnimation : AnimationClip;
public var dieAnimation : AnimationClip;

public var walkMaxAnimationSpeed : float = 0.5;
public var runMaxAnimationSpeed : float = 1.0;

public var otherAnimationSpeed : float = 1.0;

private var _animation : Animation;
private var lastAttack : float;
private var canAttack : boolean = false;
	
var healt : float = 100;
var killed : boolean = false;
var meshObject : GameObject;

var FadeOut : boolean = false;
var alphaVal : float = 1.0f;

private var damaged : float = 0.0f;

enum EnemyState {
	Idle = 0,
	Running = 1,
	Attacking = 2,
	Dead = 3,
	GetHit = 4,
	None = 5,
}

private var _enemyState : EnemyState;

function Start () {
	lastChasePosition = transform.position;
	updateLastPos = true;
	returnToChasePosition = false;
	EyeRotatorVal = 0.0f;
	nav.updatePosition = false;
	_animation = GetComponent(Animation);
	lastAttack = 0.0f;
}

function Update () {
		
	distance = Vector3.Distance(transform.position,target.position);

	_enemyState = EnemyState.Idle;
	
	EyeRotatorVal += Time.deltaTime;
	if(EyeRotatorVal > 1000) EyeRotatorVal = 0;
	var rayAngle : float = Mathf.Sin(EyeRotatorVal) * maxVisibleAngle;
	var rotation : Quaternion = Quaternion.Euler(0, rayAngle, 0);
	var newForward : Vector3 = rotation * transform.forward;
	var rayStart : Vector3 = transform.position + Vector3(0,0.4,0);
	
	var playerVisible : boolean = false;
	
	if (Physics.Raycast(rayStart, newForward, hit)){
    	playerVisible = (hit.transform.gameObject.name == "baseMale") && (distance < maxVisiblity);
    }
    
    if(FadeOut){
    	gameObject.collider.isTrigger = true;
	    alphaVal -= Time.deltaTime * 0.5;
	    
	    if(alphaVal > 0.0){	
    		var color : Color = Color(1.0,1.0,1.0,alphaVal);
	    	meshObject.renderer.material.SetColor ("_Color", color);
	    }
	    else{
	    		alphaVal = 0.0f;
	    		Destroy(gameObject);
	    	//	return;
	    }
    }

	if(killed){
		nav.updatePosition = false;
		nav.updateRotation = false;
		
		_enemyState = EnemyState.Dead;
	}
	else{
		if( ((distance < maxChaseDistance) || playerVisible)  ){ //OR player saw by enemy
			//keep chase time alive
			chaseTime += Time.deltaTime;
			
			if( updateLastPos == false){
				updateLastPos = true;
				returnToChasePosition = false;
				lastChasePosition = transform.position;
				chaseTime = 2.0f;
				_enemyState = EnemyState.Running;
			}
		}
	
		//dont chase all the time
		chaseTime -= Time.deltaTime;
		
		if(chaseTime > 0){
			_enemyState = EnemyState.Running;
			nav.updatePosition = true;
			nav.destination = target.position;
			nav.speed = 3.0f;
		}
		else if(chaseTime < 0){
			chaseTime = 0;
			updateLastPos = false;
			returnToChasePosition = true;
		}
		
		if(returnToChasePosition){
			_enemyState = EnemyState.Running;
			nav.updatePosition = true;
			nav.destination = lastChasePosition;
			nav.speed = 1.0f;
			
			if(Vector3.Distance(transform.position,lastChasePosition) < 0.1){
				returnToChasePosition = false;
				_enemyState = EnemyState.Idle;
				nav.updatePosition = false;
			}
		}
		
		if(nav.velocity.sqrMagnitude < 0.1) {
			_enemyState = EnemyState.Idle;
		//	Debug.DrawRay(rayStart,newForward,Color.red,3);
		}
			
		if(distance < 0.3) {
			nav.updatePosition = false;
			//about to stop
			
			if( Time.time-lastAttack > attackRepeatTime){
				lastAttack=Time.time; 
			
				if ( Physics.Raycast(rayStart, transform.forward, hit) ){
	    			canAttack = (hit.transform.gameObject.name == "baseMale");
	    		}
			}
			
			if(canAttack ){    
				_enemyState = EnemyState.Attacking;
			}	
			else {
				nav.destination = target.position;			
			}
		} 		
	
	}
		
	if(damaged > 0){
    	_enemyState = EnemyState.GetHit;
    	damaged -= Time.deltaTime;
    }	
		
	if(_animation) {
		
		    if(_enemyState == EnemyState.Idle){ 
				_animation.CrossFade(idleAnimation.name);
			}
			else 
			{
				if(_enemyState == EnemyState.Running) {
					
					if(returnToChasePosition){
						_animation[runAnimation.name].speed = Mathf.Clamp(nav.velocity.magnitude, 0.0, walkMaxAnimationSpeed);				
					}
					else {
						_animation[runAnimation.name].speed = Mathf.Clamp(nav.velocity.magnitude, 0.0, runMaxAnimationSpeed);				
					}
						
					_animation.CrossFade(runAnimation.name);		
				}
				else if(_enemyState == EnemyState.Attacking) {
					_animation[attackPoseAnimation.name].speed = Mathf.Clamp(1.0, 0.0, otherAnimationSpeed);
					_animation.CrossFade(attackPoseAnimation.name);	
				}
				else if(_enemyState == EnemyState.GetHit) {
					_animation[getHitAnimation.name].speed = Mathf.Clamp(1.0, 0.0, otherAnimationSpeed);
					_animation.CrossFade(getHitAnimation.name);	
				}
				else if(_enemyState == EnemyState.Dead) {
					_animation[dieAnimation.name].speed = Mathf.Clamp(1.0, 0.0, otherAnimationSpeed);
					_animation[dieAnimation.name].wrapMode = WrapMode.ClampForever;
					_animation.CrossFade(dieAnimation.name);	
					//_enemyState = EnemyState.None;
				}
		    }		
	}
	
}

function applyDamage(amount : float){
	if(healt < 0) return;
	
	healt -= amount;
	damaged += Time.deltaTime * 2.0;
	
	if(healt < 0){
		killed = true;
		yield WaitForSeconds(4.0);
		enableFadeOut();
	}
}

function enableFadeOut() {
	FadeOut = true;
}