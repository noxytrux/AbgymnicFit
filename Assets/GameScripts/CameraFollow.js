#pragma strict

public var ToFollow : GameObject;
public var CameraDistance : float = 1.31;
public var k_v3LookFromPos : Vector3 = Vector3( 0.0, 0.63 /*THIS IS CAM HEIGHT!!!*/, 0.0 );
public var k_v3fLookAtPos : Vector3 = Vector3( 0.0, 0.75, 0.0 );

@HideInInspector var s_v3LastBaseLookFromPos : Vector3;
@HideInInspector var s_fFacing : float;
@HideInInspector var v3Position : Vector3;
@HideInInspector var v3LookDirection : Vector3;
    
private var controller : CharacterController;    
    
function Start () {
	s_v3LastBaseLookFromPos = Vector3( 0, 0, 0 );
	v3Position = Vector3(0,0,0);
    v3LookDirection = Vector3(0,0,0);
	s_fFacing = 1.0;
	controller = ToFollow.GetComponent(CharacterController); 
}

function Update () {
	v3Position = ToFollow.transform.position;
	
	var v3LookFromPos : Vector3;
	
    var fNudge : float = Vector3.Magnitude(s_v3LastBaseLookFromPos - v3Position);
            
    if (fNudge < CameraDistance)
    {
    	fNudge = CameraDistance - fNudge;
        s_v3LastBaseLookFromPos -= Vector3(0,0,0) * fNudge * 1.2f;
        s_v3LastBaseLookFromPos.y = (s_v3LastBaseLookFromPos.y + fNudge * 0.2f);
    }
            
          
    s_v3LastBaseLookFromPos -= controller.velocity * 0.04f;
            
	var v3FacingNudge : Vector3 = Vector3( 0, 0, 0 ) * 0.2f * s_fFacing;
    v3FacingNudge.y = 0.0f;
    s_v3LastBaseLookFromPos -= v3FacingNudge;
            
    var tmp : Vector3 = v3Position - s_v3LastBaseLookFromPos;
    var fDot : float = Vector3.Dot(tmp,Vector3(0,0,0));
    s_fFacing = fDot * 0.1f;
            
    if (s_fFacing > 1.0f)
    	s_fFacing = 1.0f;
    else if (s_fFacing < -1.0f)
    	s_fFacing = -1.0f;
            
    v3LookDirection = v3Position - s_v3LastBaseLookFromPos;
    var fMag : float = v3LookDirection.magnitude;
    if (fMag > 0.0f)
    	v3LookDirection /= fMag;
            
    if(v3LookDirection.y > 0.3f) v3LookDirection.y = 0.3f;
            
    v3LookFromPos = v3Position - v3LookDirection * CameraDistance;
    s_v3LastBaseLookFromPos = v3LookFromPos;
            
    if ( Mathf.Abs(v3LookDirection.y) <= 0.99f )
   	{			
    	v3LookFromPos += k_v3LookFromPos;
       	var v3LookAtPos : Vector3 = (v3Position + k_v3fLookAtPos);        
       	var mRay : Vector3 = v3LookFromPos - v3LookAtPos;
        var d : Vector3 = mRay;
        d.Normalize();
        transform.forward = -d;
        transform.forward.Normalize();
	}
            
	transform.position = v3LookFromPos;	   
}

            
            