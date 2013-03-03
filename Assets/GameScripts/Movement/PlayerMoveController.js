#pragma strict

// Objects to drag in
public var motor : FreeMovementMotor;
public var character : Transform;

// Private memeber data
private var mainCamera : Camera;
private var secondCamera : Camera;

private var mainCameraTransform : Transform;

private var screenMovementSpace : Quaternion;
private var screenMovementForward : Vector3;
private var screenMovementRight : Vector3;

var xSpeed = 150.0;
var ySpeed = 120.0;

var yMinLimit = -60;
var yMaxLimit = 60;

private var x = 0.0;
private var y = 0.0;

/////////////////////////////////////////////

enum CharacterState {
	Idle = 0,
	Walking = 1,
	Trotting = 2,
	Running = 3,
	Jumping = 4,
	StrafingLeft = 5,
	StrafingRight = 6,
}

private var _characterState : CharacterState;

public var idleAnimation : AnimationClip;
public var walkAnimation : AnimationClip;
public var runAnimation : AnimationClip;
public var jumpPoseAnimation : AnimationClip;
public var strafeLeftAnimation : AnimationClip;
public var strafeRightAnimation : AnimationClip;

public var walkMaxAnimationSpeed : float = 1.0;
public var trotMaxAnimationSpeed : float = 1.0;
public var runMaxAnimationSpeed : float = 1.0;

public var jumpAnimationSpeed : float = 2.0;
public var landAnimationSpeed : float = 1.0;

private var _animation : Animation;

///////////////////////////////////////////

private var moveSpeed = 0.0;
var walkSpeed = 2.0;
var trotSpeed = 4.0;
var runSpeed = 6.0;
var inAirControlAcceleration = 3.0;

var speedSmoothing = 10.0;
var trotAfterSeconds = 3.0;
private var walkTimeStart = 0.0;
private var isMoving = false;
private var inAirVelocity = 0.0;

private var movingBack = false;

private var jumping = false;
private var verticalSpeed = 0.0;
private var speedMultipler = 1.0;

private var isZoomed : boolean = false;
var zoom : int = 20;
var normal : int = 60;
var smooth : float = 5;

var canFire : boolean = false;
var onlyOnce : boolean = false;

////////////////////////////////////////////////

var maximumHitPoints = 100.0;
var hitPoints = 50.0;
var hurtTexture : GUITexture;
var regeneration : float = 1.0;

function Awake () {		
	motor.movementDirection = Vector2.zero;
	motor.facingDirection = Vector2.zero;
	
	// Set main camera
	mainCamera = Camera.main;
	secondCamera = GameObject.Find("SecondCamera").camera;
	mainCameraTransform = mainCamera.transform;
	
	// Ensure we have character set
	// Default to using the transform this component is on
	if (!character)
		character = transform;
	
	_animation = GetComponent(Animation);
}

function Start () {
	// it's fine to calculate this on Start () as the camera is static in rotation
	var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
    
	screenMovementSpace = Quaternion.Euler (0, mainCameraTransform.eulerAngles.y, 0);
	screenMovementForward = screenMovementSpace * Vector3.forward;
	screenMovementRight = screenMovementSpace * Vector3.right;	
}

function Update () {

	hurtTexture.color.a = (1 - (hitPoints/maximumHitPoints));
	
	if(hitPoints < maximumHitPoints){
		hitPoints += Time.deltaTime * regeneration;
		if(hitPoints > maximumHitPoints) hitPoints = maximumHitPoints;
	}
	
	screenMovementSpace = Quaternion.Euler (0, mainCameraTransform.eulerAngles.y, 0);
	screenMovementForward = screenMovementSpace * Vector3.forward;
	screenMovementRight = screenMovementSpace * Vector3.right;	
	
 	x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
 	
 	y = ClampAngle(y, yMinLimit, yMaxLimit);
 		       
   	var rotation = Quaternion.Euler(0, x, 0);
    
    var v = Input.GetAxis("Vertical");
	var h = Input.GetAxis("Horizontal");
	
    isMoving = Mathf.Abs (h) > 0.1 || Mathf.Abs (v) > 0.1;
    
	motor.movementDirection = h * screenMovementRight + v * screenMovementForward; 
	
	if (motor.movementDirection.sqrMagnitude > 1)
		motor.movementDirection.Normalize();
    
	motor.facingDirection = rotation * Vector3(1.0,0.0,0.0);
	motor.facingDirection.y = 0;			
			
	// HANDLE CAMERA POSITION
		
	mainCameraTransform.position = transform.position - motor.facingDirection*1.0f;
	mainCameraTransform.position.y += 0.5;//0.01;
	
	if(Input.GetMouseButton(0)){
		canFire = true;	
		onlyOnce = true;
	}
	else{
		canFire = false;
		onlyOnce = true;
	}
	
	if(onlyOnce){
		onlyOnce = false;
	
		if(canFire){
			SendMessage("OnStartFire",SendMessageOptions.DontRequireReceiver);		
		}
		else {
			SendMessage("OnStopFire",SendMessageOptions.DontRequireReceiver);		
		}
	}
	
	 if(Input.GetMouseButton(1)){
          isZoomed = true; 
     }
     else isZoomed = false;
 
     if(isZoomed == true){
          mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView,zoom,Time.deltaTime*smooth);
          secondCamera.fieldOfView = Mathf.Lerp(secondCamera.fieldOfView,zoom,Time.deltaTime*smooth);
     }
     else{
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView,normal,Time.deltaTime*smooth);
        secondCamera.fieldOfView = Mathf.Lerp(secondCamera.fieldOfView,normal,Time.deltaTime*smooth);
     }
	
	mainCameraTransform.forward = motor.facingDirection;
	mainCameraTransform.Rotate(Vector3(1,0,0),y);
	
	var isOnGround : boolean = isGrounded();
	
	if (v < -0.2)
		movingBack = true;
	else
		movingBack = false;
	
	// Grounded controls
	if (isOnGround && (jumping==false))
	{
		var curSmooth = speedSmoothing * Time.deltaTime;
		var targetSpeed = Mathf.Min(motor.movementDirection.magnitude, 1.0);
	
		_characterState = CharacterState.Idle;
		
		var ShiftPressed : boolean = Input.GetKey (KeyCode.LeftShift);
		
		if(Mathf.Abs (h) > 0.1){
			if(h < -0.2) {
				_characterState = CharacterState.StrafingLeft;
			}
			else if(h > 0.2){
				_characterState = CharacterState.StrafingRight;
			}		
			
			if(ShiftPressed){
				speedMultipler = 1.5;
				targetSpeed *= trotSpeed;
			}
			else {
				speedMultipler = 1.0;
				targetSpeed *= walkSpeed;
			}
		}
		else{
			// Pick speed modifier
			if ( ShiftPressed && !movingBack)
			{
				targetSpeed *= runSpeed;
				_characterState = CharacterState.Running;
			}
			else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
				_characterState = CharacterState.Trotting;
			}
			else
			{
				targetSpeed *= walkSpeed;
				_characterState = CharacterState.Walking;
			}
		
		}
		
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		
		// Reset walk time start when we slow down
		if (moveSpeed < walkSpeed * 0.3)
			walkTimeStart = Time.time;
	}
	// In air controls
	else
	{
		if (isMoving)
			inAirVelocity += Time.deltaTime * inAirControlAcceleration;
	}
	
	//HANDLE JUMPING 
	if (isOnGround)
	{
		if (jumping)
		{
			inAirVelocity = 0.0;
			jumping = false;
		}
	}
	
	if (Input.GetButtonUp ("Jump") && (jumping==false))
	{
		verticalSpeed = 3.0;
		jumping  = true;
		_characterState = CharacterState.Jumping;
	}
		
	motor.walkingSpeed = moveSpeed + inAirVelocity;
	motor.jumpValue = verticalSpeed;
	verticalSpeed = 0.0f;
	//ANIMATION PART
	
	if(_animation) {
		if(_characterState == CharacterState.Jumping) 
		{
			//Debug.Log("ANIMUJE SKOK");
			_animation[jumpPoseAnimation.name].speed = jumpAnimationSpeed;
			_animation[jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
			_animation.CrossFade(jumpPoseAnimation.name);
		} 
		else 
		{
			if(rigidbody.velocity.sqrMagnitude < 0.1) {
				_animation.CrossFade(idleAnimation.name);
			}
			else 
			{
				if(_characterState == CharacterState.Running) {
					_animation[runAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude, 0.0, runMaxAnimationSpeed);
					_animation.CrossFade(runAnimation.name);	
				}
				else if(_characterState == CharacterState.Trotting) {
					
					if(movingBack){	
						_animation[walkAnimation.name].speed = -Mathf.Clamp(rigidbody.velocity.magnitude, 0.0, trotMaxAnimationSpeed);
					}
					else {
						_animation[walkAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude, 0.0, trotMaxAnimationSpeed);
					}
					_animation.CrossFade(walkAnimation.name);	
				}
				else if(_characterState == CharacterState.Walking) {
					if(movingBack){
						_animation[walkAnimation.name].speed = -Mathf.Clamp(rigidbody.velocity.magnitude, 0.0, walkMaxAnimationSpeed);
					}
					else{
						_animation[walkAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude, 0.0, walkMaxAnimationSpeed);
					}
					_animation.CrossFade(walkAnimation.name);	
				}//strafing animations
				else if(_characterState == CharacterState.StrafingLeft) {
					_animation[strafeLeftAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude, 0.0, walkMaxAnimationSpeed * speedMultipler);
					_animation.CrossFade(strafeLeftAnimation.name);	
				}
				else if(_characterState == CharacterState.StrafingRight) {
					_animation[strafeRightAnimation.name].speed = Mathf.Clamp(rigidbody.velocity.magnitude, 0.0, walkMaxAnimationSpeed * speedMultipler);
					_animation.CrossFade(strafeRightAnimation.name);	
				}
			}
		}
	}
	
	
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}

function isGrounded() : boolean {
	var grounded : boolean = false;
	//raycasting here to determine if we touch the ground or not

	var hit: RaycastHit;
	
    if (Physics.Raycast(transform.position, -transform.up, hit)){
    	if(hit.distance < 0.1) grounded = true;
    }

	return grounded;
}

function playerGetHit(damage: float){
	hitPoints -= damage;
	if(hitPoints < 0) hitPoints = 0;
}

