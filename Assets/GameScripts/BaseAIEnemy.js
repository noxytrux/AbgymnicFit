#pragma strict

var nav : NavMeshAgent;
var target : Transform;
var maxChaseDistance : float = 10.0f;
var maxVisiblity : float = 200.0f; //not used yet (ray cast with degree limitation needed here!)

private var distance : float = 100000.0;
private var chaseTime : float = 2.0f;
private var lastChasePosition : Vector3;
private var updateLastPos : boolean;

function Start () {
	lastChasePosition = transform.position;
	updateLastPos = true;
}

function Update () {
		
	distance = Vector3.Distance(transform.position,target.position);

	//if(distance < 

	nav.destination = target.position;
}