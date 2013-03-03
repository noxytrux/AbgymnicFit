#pragma strict

var bulletPrefab : GameObject;
var frequency : float = 2;
var coneAngle : float = 1.5;
var fireSound : AudioClip;
var firing : boolean = false;

var damagePerSecond : float = 20.0;
var forcePerSecond : float = 20.0;
var hitSoundVolume : float = 0.5;

private var lastFireTime : float = -1;

private var hitInfo : RaycastHit;

function Update () {
	
	var layerMask = 1 << 9;
	//layerMask = ~layerMask;

	hitInfo = RaycastHit ();
	Physics.Raycast (transform.position + transform.forward*0.3 +Vector3(0,0.3,0), Camera.mainCamera.transform.forward, hitInfo, Mathf.Infinity, layerMask);
	
	if (firing) {
		if (Time.time > lastFireTime + 1 / frequency) {
			Fire ();
		}
	}
}

function Fire () {
	// Spawn bullet
	var coneRandomRotation = Quaternion.Euler (Random.Range (-coneAngle, coneAngle), Random.Range (-coneAngle, coneAngle), 0);
	var go : GameObject = Instantiate(bulletPrefab, transform.position + transform.forward*0.3 +Vector3(0,0.3,0), coneRandomRotation * Camera.mainCamera.transform.rotation);
	var bullet : SimpleBullet = go.GetComponent.<SimpleBullet> ();
			
	if (hitInfo.transform) {
			
		hitInfo.transform.gameObject.SendMessage("applyDamage", damagePerSecond / frequency ,SendMessageOptions.DontRequireReceiver);	
						
		// Get the rigidbody if any
		if (hitInfo.rigidbody) {
			// Apply force to the target object at the position of the hit point
			var force : Vector3 = transform.forward * (forcePerSecond / frequency);
			hitInfo.rigidbody.AddForceAtPosition (force, hitInfo.point, ForceMode.Impulse);
		}
				
//				// Ricochet sound
//		var sound : AudioClip = MaterialImpactManager.GetBulletHitSound (hitInfo.collider.sharedMaterial);
//		AudioSource.PlayClipAtPoint (sound, hitInfo.point, hitSoundVolume);
				
		bullet.dist = hitInfo.distance;
	}
	else {
		bullet.dist = 1000;
	}
	
	if (audio && fireSound ) { //&& !audio.isPlaying
		audio.clip = fireSound;
		audio.Play ();
	}
	
	lastFireTime = Time.time;
}

function OnStartFire () {
	firing = true;
}

function OnStopFire () {
	firing = false;
	audio.Stop();
}