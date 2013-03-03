#pragma strict


var attackRepeatTime : float = 1.0f;
private var lastAttack : float;

function Start () {
	lastAttack = 0;
}

function Update () {

}

function OnCollisionEnter( colision : Collision){

	if( Time.time-lastAttack > attackRepeatTime){
		lastAttack=Time.time; 
				
		if(colision.gameObject.name == "baseMale"){
			colision.gameObject.SendMessage("playerGetHit",5.0);
		}
	
	}
}

//function OnTriggerEnter( other : Collider){
//    if(other.gameObject.name == "baseMale"){
//		Debug.Log("TRIGGER");
//	}
//}