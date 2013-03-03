#pragma strict

private var m_Camera : Camera;
var playerTransform : Transform;

function Start () {
	m_Camera = Camera.mainCamera;
}

function Update ()
{
	transform.forward = m_Camera.transform.forward;
	transform.position = Vector3.Lerp(transform.position, m_Camera.transform.position + transform.forward * 2.0, Time.deltaTime * 20.0);
	transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.up, m_Camera.transform.rotation * Vector3.back);	
}
