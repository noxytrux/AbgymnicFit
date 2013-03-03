#pragma strict

private var m_Camera : Camera;
var playerTransform : Transform;

function Start () {
	m_Camera = Camera.mainCamera;
}

function Update ()
{
	transform.forward = m_Camera.transform.forward;
	transform.position = playerTransform.position + transform.forward;
	transform.position.y += 0.5;
	transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.up, m_Camera.transform.rotation * Vector3.back);	
}
