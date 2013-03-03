//====================================================================================================================================
//
// Copyright Julian Oliden "Jocyf" 2010 - 01-12-2010
// CloudsToy v1.2
// Intensive particle system personalizable for Volumetric Clouds generation.
//
//====================================================================================================================================

using UnityEngine;
using System.Collections;
using System.IO; // For save splatmap as PNG file.


public class CloudsToy : MonoBehaviour {

	
//=================================================================================
//=================================================================================
public int MaximunClouds = 300;  // The maximun Clouds the system will manage in total.
//=================================================================================
//=================================================================================
public int ToolbarOptions = 0;
public enum TypePreset { None = 0, Stormy = 1, Sunrise = 2, Fantasy = 3 }
public TypePreset CloudPreset = TypePreset.None;
public enum TypeRender { Bright = 0, Realistic = 1 }
public TypeRender CloudRender = TypeRender.Bright;
public enum TypeDetail { Low = 0, Normal = 1, High = 2 }
public TypeDetail CloudDetail = TypeDetail.Low;
public enum Type { Nimbus1=0, Nimbus2=1, Nimbus3=2, Nimbus4=3, Cirrus1=4, Cirrus2=5, MixNimbus=6, MixCirrus=7, MixAll=8, PT1 = 9 }
public Type TypeClouds = Type.Nimbus1;
public float SizeFactorPart = 1;
public float EmissionMult = 1;
public bool  SoftClouds = false;
public Vector3 SpreadDir = new Vector3(-1, 0, 0);
public float LengthSpread = 1;
public int NumberClouds = 100;
public Vector3 Side = new Vector3(1000, 500, 1000);
public Vector3 MaximunVelocity = new Vector3(-10, 0, 0);
public float VelocityMultipier = 1;
public float DisappearMultiplier = 1.5f;
public enum TypePaintDistr { Random = 0, Below = 1 }
public TypePaintDistr PaintType = TypePaintDistr.Below;
public Color CloudColor = new Color(1, 1, 1, 1);
public Color MainColor = new Color(1, 1, 1, 1);
public Color SecondColor = new Color(0.5f, 0.5f, 0.5f, 1);
public int TintStrength = 50;
public float offset = 0.5f;
public int MaxWithCloud = 100;
public int MaxTallCloud = 40;
public int MaxDepthCloud = 100;
public bool  FixedSize = true;
public enum TypeShadow { All = 0, Most = 1, Half = 2, Some = 3, None = 4 }
public TypeShadow NumberOfShadows = TypeShadow.Some;
public Texture2D[] CloudsTextAdd = new Texture2D[6];
public Texture2D[] CloudsTextBlended = new Texture2D[6];
public bool IsAnimate = true;
public float AnimationVelocity = 0;
public ProceduralCloudTexture ProceduralTexture;
//=================================================================================
//=================================================================================
// Parameters needed for manage procedural textures.
public int    PT1TextureWidth = 128;
public int    PT1TextureHeight= 128;
public int    PT1Seed = 132;
public float PT1ScaleWidth = 1;
public float PT1ScaleHeight = 1;
public float PT1ScaleFactor = 1;
public float PT1Lacunarity = 3;
public float PT1FractalIncrement = 0.5f;
public float PT1Octaves = 7;
public float PT1Offset = 1;
/*public bool PT1IsHalo = true;*/
public enum NoisePresetPT1 { Cloud = 0, PerlinCloud = 1 }
public NoisePresetPT1  PT1TypeNoise = NoisePresetPT1.Cloud;
// Turbulence vars.
public int PT1TurbSize = 16;
public float PT1TurbLacun = 0.01f;
public float PT1TurbGain = 0.5f;
// Spherical paramss cloud
public float PT1turbPower = 5.0f; //makes noise
public float PT1xyPeriod = 0.8f; //radius of the circle
public float PT1HaloEffect = 1.7f;
public float PT1HaloInsideRadius = 0.1f;
/*public Color PT1BackgroundColor = new Color(0, 0, 0, 1);
public Color PT1FinalColor = new Color(1, 1, 1, 1);*/
public bool PT1InvertColors = false;
public float PT1ContrastMult = 0;
public bool PT1UseAlphaTexture = true;
public float PT1AlphaIndex = 0.1f;
//=================================================================================
//=================================================================================
// Private vars that are used in the normal execution of the class.
private Material[] CloudsMatAdditive = new Material[6];
private Material[] CloudsMatBlended = new Material[6];
private Material CloudsPTMatAdditive;
private Material CloudsPTMatBlended;
private enum Axis { X = 0, Y = 1, Z = 2, XNeg = 3, YNeg = 4, ZNeg = 5 }
private Axis CloudsGenerateAxis =Axis.X;
private Transform MyTransform;
private Vector3 MyPosition;
private ArrayList MyCloudsParticles = new ArrayList();

// Private vars for detect changing parameters in inspector and execute only a piece of code.inthe Update.
private TypePreset CloudPresetAnt = TypePreset.None;
private TypeRender CloudRenderAnt = TypeRender.Bright;
private TypeDetail CloudDetailAnt = TypeDetail.Low;
private Type TypeCloudsAnt = Type.Nimbus1;
private float EmissionMultAnt = 1;
private float SizeFactorPartAnt = 1;
private bool  SoftCloudsAnt = false;
private Vector3 SpreadDirAnt = new Vector3(-1, 0, 0);
private float LengthSpreadAnt = 1;
private int NumberCloudsAnt = 10;
private Vector3 MaximunVelocityAnt;
private float VelocityMultipierAnt;
private TypePaintDistr PaintTypeAnt = TypePaintDistr.Below;
private Color CloudColorAnt = new Color(1, 1, 1, 1);
private Color MainColorAnt = new Color(1, 1, 1, 1);
private Color SecondColorAnt = new Color(0.5f, 0.5f, 0.5f, 1);
private int TintStrengthAnt = 5;
private float offsetAnt = 0;
private TypeShadow NumberOfShadowsAnt = TypeShadow.All;
private int MaxWithCloudAnt = 200;
private int MaxTallCloudAnt = 50;
private int MaxDepthCloudAnt = 200;
private bool bAssignProcTexture = false;


public void  SetPresetStormy (){
	if(CloudPresetAnt == CloudPreset) 
		return;
	CloudRender = TypeRender.Realistic;
	CloudDetail = TypeDetail.Normal;
	SetCloudDetailParams();
	TypeClouds = Type.Nimbus2;
	SoftClouds = false;
	SpreadDir = new Vector3(-1, 0, 0);
	LengthSpread = 1;
	NumberClouds = 100;
	//Side = new Vector3(2000, 500, 2000);
	DisappearMultiplier = 2;
	MaximunVelocity = new Vector3(-10, 0, 0);
	VelocityMultipier = 0.85f;
	PaintType = TypePaintDistr.Below;
	CloudColor = new Color(1, 1, 1, 0.5f);
	MainColor = new Color(0.62f, 0.62f, 0.62f, 0.3f);
	SecondColor = new Color(0.31f, 0.31f, 0.31f, 1);
	TintStrength = 80;
	offset = 0.8f;
	MaxWithCloud = 200;
	MaxTallCloud = 50;
	MaxDepthCloud = 200;
	FixedSize = false;
	NumberOfShadows = TypeShadow.Some;
	CloudPresetAnt = CloudPreset;
}
 
 public void  SetPresetSunrise (){
 	if(CloudPresetAnt == CloudPreset) 
		return;
	CloudRender = TypeRender.Bright;
	CloudDetail = TypeDetail.Low;
	SetCloudDetailParams();
	EmissionMult = 1.6f;
	SizeFactorPart = 1.5f;
	TypeClouds = Type.Cirrus1;
	SoftClouds = true;
	SpreadDir = new Vector3(-1, 0, 0);
	LengthSpread = 4;
	NumberClouds = 135;
	//Side = new Vector3(2000, 500, 2000);
	DisappearMultiplier = 2;
	MaximunVelocity = new Vector3(-10, 0, 0);
	VelocityMultipier = 6.2f;
	PaintType = TypePaintDistr.Below;
	CloudColor = new Color(1, 1, 1, 1);
	MainColor = new Color(1, 1, 0.66f, 0.5f);
	SecondColor = new Color(1, 0.74f, 0, 1);
	TintStrength = 100;
	offset = 1;
	MaxWithCloud = 500;
	MaxTallCloud = 20;
	MaxDepthCloud = 500;
	FixedSize = true;
	NumberOfShadows = TypeShadow.None;
	CloudPresetAnt = CloudPreset;
}
 
 public void  SetPresetFantasy (){ 
 	if(CloudPresetAnt == CloudPreset) 
		return;
	CloudRender = TypeRender.Bright;
	CloudDetail = TypeDetail.Low;
	EmissionMult = 0.3f;
	SetCloudDetailParams();
	TypeClouds = Type.Nimbus4;
	SoftClouds = false;
	SpreadDir = new Vector3(-1, 0, 0);
	LengthSpread = 1;
	NumberClouds = 200;
	//Side = new Vector3(2000, 500, 2000);
	DisappearMultiplier = 2;
	MaximunVelocity = new Vector3(-10, 0, 0);
	VelocityMultipier = 0.50f;
	PaintType = TypePaintDistr.Random;
	CloudColor = new Color(1, 1, 1, 0.5f);
	MainColor = new Color(1, 0.62f, 0, 1);
	SecondColor = new Color(0.5f, 0.5f, 0.5f, 1);
	TintStrength = 50;
	offset = 0.2f;
	MaxWithCloud = 200;
	MaxTallCloud = 50;
	MaxDepthCloud = 200;
	FixedSize = true;
	NumberOfShadows = TypeShadow.Some;
	CloudPresetAnt = CloudPreset;
}

 public void  SetCloudDetailParams (){ 
	if(CloudDetailAnt == CloudDetail)
		return;
	if(CloudDetail == TypeDetail.Low){
		EmissionMult = 1;
		SizeFactorPart = 1;
	}
	else
	if(CloudDetail == TypeDetail.Normal){
		EmissionMult = 1.5f;
		SizeFactorPart = 1.2f;
	}
	else
	if(CloudDetail == TypeDetail.High){
		EmissionMult = 2.0f;
		SizeFactorPart = 1.3f;
	}
	CloudDetailAnt = CloudDetail;
}


public void PaintTheParticlesShadows(CloudParticle MyCloudParticle){
	if(PaintType == TypePaintDistr.Random)
		MyCloudParticle.PaintParticlesBelow(MainColor, SecondColor, TintStrength, offset, 0);
	else
	if(PaintType == TypePaintDistr.Below)
		MyCloudParticle.PaintParticlesBelow(MainColor, SecondColor, TintStrength, offset, 1);
}


 public void EditorRepaintClouds (){
	int i = 0;
	CloudParticle MyCloudParticle;
	 
	if(MyCloudsParticles.Count == 0)
		return;
	for(i = 0; i < MaximunClouds; i++){
		MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
		if(MyCloudParticle.IsActive()){
			// Define some main particle properties
			if( TypeClouds == Type.Nimbus1 || TypeClouds == Type.Nimbus2 || 
				TypeClouds == Type.Nimbus3 || TypeClouds == Type.Nimbus4 || 
				TypeClouds == Type.MixNimbus || TypeClouds == Type.MixAll)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 0, FixedSize, true ,  true);
			else
			if(TypeClouds == Type.Cirrus1 || TypeClouds == Type.Cirrus2 || TypeClouds == Type.MixCirrus)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 1, FixedSize, true ,  true);
			
			MyCloudParticle.UpdateCloudsPosition();
			if(CloudRender == TypeRender.Realistic)
				MyCloudParticle.SetMainColor(CloudColor);
			PaintTheParticlesShadows(MyCloudParticle);
		}
	}
}


void  Start (){
	MyTransform = this.transform;
	MyPosition = transform.position;
	CloudParticle MyCloudParticle;
	Vector3 MyPos;
	Vector3 SideAux;
	int i;
	
	//CloudPrefab = GameObject.Find("VolCloud Basic");
	//CloudPrefab = Resources.LoadAssetAtPath("Assets/Volumetric Clouds/Prefabs/VolCloud Basic.prefab", typeof(GameObject));
	CloudPresetAnt = CloudPreset;
	CloudRenderAnt = CloudRender;
	CloudDetailAnt = CloudDetail;
	TypeCloudsAnt = TypeClouds;
	EmissionMultAnt = EmissionMult;
	SizeFactorPartAnt = SizeFactorPart;
	SoftCloudsAnt = SoftClouds;
	SpreadDirAnt = SpreadDir;
	LengthSpreadAnt = LengthSpread;
	NumberCloudsAnt = NumberClouds;
	MaximunVelocityAnt = MaximunVelocity;
	VelocityMultipierAnt = VelocityMultipier;
	PaintTypeAnt = PaintType;
	CloudColorAnt = CloudColor;
	MainColorAnt = MainColor;
	SecondColorAnt = SecondColor;
	TintStrengthAnt = TintStrength;
	offsetAnt = offset;
	NumberOfShadowsAnt = NumberOfShadows;
	MaxWithCloudAnt = MaxWithCloud;
	MaxTallCloudAnt = MaxTallCloud;
	MaxDepthCloudAnt = MaxDepthCloud;

	// Define the axis the clouds are moving on. (Only one value X or Y or Z, must be not equal Zero).
	Vector3 MyVelocity = MaximunVelocity;
	if(MyVelocity.x > 0)
		CloudsGenerateAxis = Axis.X;
	else
	if(MyVelocity.x < 0)
		CloudsGenerateAxis = Axis.XNeg;
	else
	if(MyVelocity.y > 0)
		CloudsGenerateAxis = Axis.Y;
	else
	if(MyVelocity.y < 0)
		CloudsGenerateAxis = Axis.YNeg;
	else
	if(MyVelocity.z > 0)
		CloudsGenerateAxis = Axis.Z;
	else
	if(MyVelocity.z < 0)
		CloudsGenerateAxis = Axis.ZNeg;
	
	// Create the procedural Texture Object only if it's selected in the clouds option in the editor.
	if(TypeClouds == Type.PT1){
		GameObject PText1 = new GameObject();
		PText1.name = "CloudsToyPT1";
		PText1.transform.position =  Vector3.zero;
		PText1.transform.rotation =  Quaternion.identity;
		PText1.transform.parent = MyTransform;
		ProceduralTexture = (ProceduralCloudTexture)PText1.AddComponent ("ProceduralCloudTexture");
		PT1CopyInitialParameters();
	}
	
	// Create the materials based in the textures provided by the user. maximun textures . 6
	// There are two types of materials Additive Soft for bright Clouds & Blend for more realistic ones.
	// First type of clouds. Additive - Bright Ones.
	for(i = 0; i < 6; i++){
		CloudsMatAdditive[i] = new Material(Shader.Find("FX/CloudBright"));
		CloudsMatAdditive[i].mainTexture = CloudsTextAdd[i];
	}
	// Second type of Clouds. Realistic Ones.
	for(i = 0; i < 6; i++){
		CloudsMatBlended[i] = new Material(Shader.Find("FX/CloudRealistic"));
		CloudsMatBlended[i].SetColor("_TintColor", CloudColor);
		CloudsMatBlended[i].mainTexture = CloudsTextBlended[i];
	}
	
	// Tirdth type of Cloud. Procedural Additive texture, Created only if procedural texture had been selected
	if(ProceduralTexture){
		CloudsPTMatAdditive = new Material(Shader.Find("FX/CloudBright"));
		if(ProceduralTexture.IsInicialized())
			CloudsPTMatAdditive.mainTexture = ProceduralTexture.MyTexture;
		// Fourth type of Cloud. Procedural Blended texture
		CloudsPTMatBlended = new Material(Shader.Find("FX/CloudRealistic"));
		CloudsPTMatBlended.SetColor("_TintColor", CloudColor);
		if(ProceduralTexture.IsInicialized())
			CloudsPTMatBlended.mainTexture = ProceduralTexture.MyAlphaTexture;
	}
	
	// Generate the clouds for first time, never well be destroyed during the scene.
	// Using a cubic shape to bounds the limits of coords. creation
	SideAux =  Side/2;
	for(i = 0; i < MaximunClouds; i++){
		MyPos = MyPosition;
		MyPos.x = Random.Range (MyPos.x-SideAux.x, MyPos.x+SideAux.x);
		MyPos.y = Random.Range (MyPos.y-SideAux.y, MyPos.y+SideAux.y);
		MyPos.z = Random.Range (MyPos.z-SideAux.z, MyPos.z+SideAux.z);
		MyCloudParticle = new CloudParticle(MyPos, Quaternion.identity);
		MyCloudParticle.SetCloudParent(MyTransform);
		MyCloudsParticles.Add(MyCloudParticle);
		
		// Define some main particle properties
		if( TypeClouds == Type.Nimbus1 || TypeClouds == Type.Nimbus2 || 
			TypeClouds == Type.Nimbus3 || TypeClouds == Type.Nimbus4 || 
			TypeClouds == Type.MixNimbus || TypeClouds == Type.MixAll || TypeClouds == Type.PT1)
				MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 0, FixedSize, true ,  true);
		else
		if(TypeClouds == Type.Cirrus1 || TypeClouds == Type.Cirrus2 || TypeClouds == Type.MixCirrus)
				MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 1, FixedSize, true ,  true);
		
		AssignCloudMaterial (MyCloudParticle,  CloudRender ,  TypeClouds);
		MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
		MyCloudParticle.SetCloudVelocity (MaximunVelocity, VelocityMultipier);
		MyCloudParticle.SetLengthScale(LengthSpread);
		MyCloudParticle.SetWorldVelocity(SpreadDir);
		MyCloudParticle.SoftCloud(SoftClouds);
		ManageCloudShadow(MyCloudParticle);
		// If the cloud will be active, Paint the cloud otherwise deactivate it (they are initially active, but dont emit anything)
		if(i < NumberClouds){
			if(TypeClouds != Type.PT1){
				MyCloudParticle.SetActive(true); // Emit the particles, because this cloud is visible
				MyCloudParticle.UpdateCloudsPosition(); // Updating the positions of particles once the Particle Emmitter emit them.
				if(CloudRender == TypeRender.Realistic)
					MyCloudParticle.SetMainColor(CloudColor);  // Set the main color of the cloud
				PaintTheParticlesShadows(MyCloudParticle); // Colorize the cloud with the Cloud Color and the Secondary Color
			}
		}
		else
			MyCloudParticle.DesactivateRecursively();
	}

}

// Only we manage the changes of variables in the inspector of Unity, not be used in gametime when
// everything is setup.
void  Update (){
	CloudParticle MyCloudParticle;
	int i;
	
	// Create the procedural Texture Object of PT1 texture is selected in runtime.
	if(TypeClouds == Type.PT1 && !ProceduralTexture){
		GameObject PText1 = new GameObject();
		PText1.name = "CloudsToyPT1";
		PText1.transform.position =  Vector3.zero;
		PText1.transform.rotation =  Quaternion.identity;
		PText1.transform.parent = MyTransform;
		ProceduralTexture = (ProceduralCloudTexture)PText1.AddComponent ("ProceduralCloudTexture");
		PT1CopyInitialParameters();
		
		// Create the procedural materials to use in the clouds if PT1 had been selected.
		CloudsPTMatAdditive = new Material(Shader.Find("FX/CloudBright"));
		if(ProceduralTexture.IsInicialized())
			CloudsPTMatAdditive.mainTexture = ProceduralTexture.MyTexture;
		// Fourth type of Clouds. Procedural Blended textures
		CloudsPTMatBlended = new Material(Shader.Find("FX/CloudRealistic"));
		CloudsPTMatBlended.SetColor("_TintColor", CloudColor);
		if(ProceduralTexture.IsInicialized())
			CloudsPTMatBlended.mainTexture = ProceduralTexture.MyAlphaTexture;
	}
	
	// Trying to Assign a procedural txeture, once the texture is already created, not earlyer
	// PT1 needs time to be created and inicialized, that's why this code exits
	if(ProceduralTexture){
		if(ProceduralTexture.IsInicialized() && !bAssignProcTexture){
			// Procedural Additive textures
			CloudsPTMatAdditive.mainTexture = ProceduralTexture.MyTexture;
			// Procedural Blended textures
			CloudsPTMatBlended.SetColor("_TintColor", CloudColor);
			CloudsPTMatBlended.mainTexture = ProceduralTexture.MyAlphaTexture;
			
			if(TypeClouds == Type.PT1){
				for(i = 0; i < MaximunClouds; i++){
					MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
					if(i < NumberClouds){
						MyCloudParticle.SetActive(true); 
						MyCloudParticle.UpdateCloudsPosition();
						if(CloudRender == TypeRender.Realistic)
							MyCloudParticle.SetMainColor(CloudColor);
						PaintTheParticlesShadows(MyCloudParticle);
					}
				}
			}
			bAssignProcTexture = true;
		}
	}
	
	// Change the number of visible clouds. Must activate the new particles and Update de position of the particles in the ellipse
	if(NumberCloudsAnt != NumberClouds){
		for(i = 0; i < MaximunClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			if(i < NumberClouds && !MyCloudParticle.IsActive()){
				MyCloudParticle.SetActive(true);
				MyCloudParticle.UpdateCloudsPosition();
				if(SoftClouds)
					SoftCloudsAnt = !SoftClouds;
			}
			else
			if(i >= NumberClouds && MyCloudParticle.IsActive())
				MyCloudParticle.DesactivateRecursively();
		}
		NumberCloudsAnt = NumberClouds;
	}
	// Actualize the particle emmitter if the density of particles emmited has changed by user
	if(CloudDetailAnt != CloudDetail){
		if(CloudDetail == TypeDetail.Low){
			EmissionMult = 1;
			SizeFactorPart = 1;
		}
		else
		if(CloudDetail == TypeDetail.Normal){
			EmissionMult = 1.5f;
			SizeFactorPart = 1.2f;
		}
		else
		if(CloudDetail == TypeDetail.High){
			EmissionMult = 2.0f;
			SizeFactorPart = 1.3f;
		}
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			MyCloudParticle.SetActive(true);
			MyCloudParticle.UpdateCloudsPosition();
			if(CloudRender == TypeRender.Realistic)
				MyCloudParticle.SetMainColor(CloudColor);
			PaintTheParticlesShadows(MyCloudParticle);
		}
		CloudDetailAnt = CloudDetail;
	}
	// if change the Size or amount of particles emmitted by any Cloud, must update the partice emmitter and emit again.
	// after that, we ensure the particles are in the assigned ellipsoid of the cloud
	if(SizeFactorPartAnt != SizeFactorPart || EmissionMultAnt != EmissionMult){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			MyCloudParticle.SetActive(true);
			MyCloudParticle.UpdateCloudsPosition();
		}
		SizeFactorPartAnt = SizeFactorPart;
		EmissionMultAnt = EmissionMult;
	}
	// Are soft clouds? Update the particle emmitter and renderer to take care of the change
	if(SoftCloudsAnt != SoftClouds){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			MyCloudParticle.SoftCloud (SoftClouds);
			MyCloudParticle.SetActive(true);
			MyCloudParticle.UpdateCloudsPosition();
		}
		SoftCloudsAnt = SoftClouds;
	}
	//  this two vars, only are visibles if softClouds are true, otherwise any change will not be advised
	if(SpreadDirAnt != SpreadDir || LengthSpreadAnt != LengthSpread){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.SetLengthScale(LengthSpread);
			if(SpreadDirAnt != SpreadDir){
				MyCloudParticle.SetWorldVelocity(SpreadDir);
				MyCloudParticle.SetActive(true);
				MyCloudParticle.UpdateCloudsPosition();
			}
		}
		SpreadDirAnt = SpreadDir;
		LengthSpreadAnt = LengthSpread;
	}
	// Changin the clouds width or tall. Must redefine all the cloud parameters, including his name
	if(MaxWithCloud != MaxWithCloudAnt || MaxTallCloud != MaxTallCloudAnt || MaxDepthCloud != MaxDepthCloudAnt){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			// Define some main particle properties
			if( TypeClouds == Type.Nimbus1 || TypeClouds == Type.Nimbus2 || 
				TypeClouds == Type.Nimbus3 || TypeClouds == Type.Nimbus4 || 
				TypeClouds == Type.MixNimbus || TypeClouds == Type.MixAll || TypeClouds == Type.PT1)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 0, FixedSize, true ,  true);
			else
			if(TypeClouds == Type.Cirrus1 || TypeClouds == Type.Cirrus2 || TypeClouds == Type.MixCirrus)
					MyCloudParticle.DefineCloudProperties (i, MaxWithCloud, MaxTallCloud, MaxDepthCloud, 1, FixedSize, true ,  true);
			// Change the emitter params of the cloud to adjust the new size.
			MyCloudParticle.SetCloudEmitter (i, SpreadDir, SoftClouds, SizeFactorPart, EmissionMult, MaximunVelocity, VelocityMultipier);
			// Start emit again, my friend.
			MyCloudParticle.SetActive(true); 
			//  Update the position of the particles emmitted inside the ellipsoid
			MyCloudParticle.UpdateCloudsPosition();
			// Colorize the cloud
			if(CloudRender == TypeRender.Realistic)
				MyCloudParticle.SetMainColor(CloudColor);
			PaintTheParticlesShadows(MyCloudParticle);
		}
		MaxWithCloudAnt = MaxWithCloud;
		MaxTallCloudAnt = MaxTallCloud;
		MaxDepthCloudAnt = MaxDepthCloud;
	}
	// If change the type of cloud just meaning i must change his material or render mode
	// also assign again the new texture if the procedural texture has changed.
	if(TypeCloudsAnt != TypeClouds || CloudRenderAnt != CloudRender /*|| ProceduralTexture.IsTextureUpdated()*/){
		for(i = 0; i < MaximunClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			// Change the Material depending on the type defined by user
			AssignCloudMaterial(MyCloudParticle, CloudRender, TypeClouds);
		}
		TypeCloudsAnt = TypeClouds;
		CloudRenderAnt = CloudRender;
	}
	// Actualize the velocity of the cloud and take care of the direccion of the mov for the LateUpdate proccess.
	if(MaximunVelocityAnt != MaximunVelocity || VelocityMultipierAnt != VelocityMultipier){
		// Define the axis the clouds are moving on. (Only one value X or Y or Z, must be not equal Zero).
		// Used to determine the way the coulds are goig to dissapear when they move far away from the Box.
		if(MaximunVelocity.x > 0)
			CloudsGenerateAxis = Axis.X;
		else
		if(MaximunVelocity.x < 0)
			CloudsGenerateAxis = Axis.XNeg;
		else
		if(MaximunVelocity.y > 0)
			CloudsGenerateAxis = Axis.Y;
		else
		if(MaximunVelocity.y < 0)
			CloudsGenerateAxis = Axis.YNeg;
		else
		if(MaximunVelocity.z > 0)
			CloudsGenerateAxis = Axis.Z;
		else
		if(MaximunVelocity.z < 0)
			CloudsGenerateAxis = Axis.ZNeg;
		
		for(i = 0; i < MaximunClouds; i++)
			((CloudParticle)MyCloudsParticles[i]).SetCloudVelocity(MaximunVelocity, VelocityMultipier);

		MaximunVelocityAnt = MaximunVelocity;
		VelocityMultipierAnt = VelocityMultipier;
	}
	// All this just change one color or the system to colorize the cloud, just that.
	if(CloudColorAnt != CloudColor){
		for(i = 0; i < NumberClouds; i++)
			((CloudParticle)MyCloudsParticles[i]).SetMainColor(CloudColor);
		CloudColorAnt = CloudColor;
	}
	
	if(MainColorAnt != MainColor){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		MainColorAnt = MainColor;
	}
	
	if(SecondColorAnt != SecondColor || TintStrengthAnt !=TintStrength){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		SecondColorAnt = SecondColor;
		TintStrengthAnt = TintStrength;
	}
	
	if(offsetAnt != offset){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		offsetAnt = offset;
	}
	
	if(PaintTypeAnt != PaintType){
		for(i = 0; i < NumberClouds; i++)
			PaintTheParticlesShadows(((CloudParticle)MyCloudsParticles[i]));
		PaintTypeAnt = PaintType;
	}
	
	// Determine if cloud shadow must be active or not, depending on user choice
	if(NumberOfShadowsAnt != NumberOfShadows){
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			ManageCloudShadow(MyCloudParticle);
		}
		NumberOfShadowsAnt = NumberOfShadows;
	}
	
	if(IsAnimate)
		for(i = 0; i < NumberClouds; i++){
			MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
			MyCloudParticle.AnimateCloud (AnimationVelocity);
		}
}

// Manage the dissapearing of the partiles at the end of en Cubic Shape and move them to the begining again.
void  LateUpdate (){
	CloudParticle MyCloudParticle;
	Vector3 MyPos;
	int i;
	bool  DestroyIt = false;
	Vector3 SideAux;

	// Test if some cloud is outside the dissapear shape (that's the original shape plus a Dissapear multiplier)
	SideAux =  (Side * DisappearMultiplier)/2;
	for(i = 0; i < MyCloudsParticles.Count; i++){
		MyCloudParticle = (CloudParticle)MyCloudsParticles[i];
		MyPos = MyCloudParticle.GetCloudPosition();
		if(MyPos.x < MyPosition.x-SideAux.x || MyPos.x > MyPosition.x+SideAux.x)
			DestroyIt = true;
		else
		if(MyPos.y < MyPosition.y-SideAux.y || MyPos.y > MyPosition.y+SideAux.y)
			DestroyIt = true;
		else
		if(MyPos.z < MyPosition.z-SideAux.z || MyPos.z > MyPosition.z+SideAux.z)
			DestroyIt = true;
	
		// If a cloud it's outside, just brig it back in the other side (the axis the clouds are moving along is needed)
		// do all that stuff depending on the shape cuadratic or spherical.
		if(DestroyIt){
			DestroyIt = false;
			SideAux =  (Side * DisappearMultiplier)/2;
			MyPos = MyPosition;
			MyPos.x = Random.Range (MyPos.x-Side.x/2, MyPos.x+Side.x/2);
			MyPos.y = Random.Range (MyPos.y-Side.y/2, MyPos.y+Side.y/2);
			MyPos.z = Random.Range (MyPos.z-Side.z/2, MyPos.z+Side.z/2);
			if(CloudsGenerateAxis == Axis.X)
				MyPos.x = MyPosition.x-SideAux.x;
			else
			if(CloudsGenerateAxis == Axis.XNeg)
				MyPos.x = MyPosition.x+SideAux.x;
			else
			if(CloudsGenerateAxis == Axis.Y)
				MyPos.y = MyPosition.y-SideAux.y;
			else
			if(CloudsGenerateAxis == Axis.YNeg)
				MyPos.y = MyPosition.y+SideAux.y;
			else
			if(CloudsGenerateAxis == Axis.Z)
				MyPos.z = MyPosition.z-SideAux.z;
			else
			if(CloudsGenerateAxis == Axis.ZNeg)
				MyPos.z = MyPosition.z+SideAux.z;
			
			MyCloudParticle.SetCloudPosition(MyPos);
		}
	}
}


// Change the Material depending on the type of cloud defined by user and the renderer (Bright/Realistic)
// So assign one of the two sets of materials available (Additive or Blended).
void  AssignCloudMaterial ( CloudParticle MyCloudParticle ,   TypeRender CloudRender ,   Type TypeClouds){
	ModifyPTMaterials();
	if(CloudRender == TypeRender.Bright){
		if(TypeClouds == Type.Nimbus1)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[0]);
		else
		if(TypeClouds == Type.Nimbus2)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[1]);
		else
		if(TypeClouds == Type.Nimbus3)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[2]);
		else
		if(TypeClouds == Type.Nimbus4)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[3]);
		else
		if(TypeClouds == Type.Cirrus1)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[4]);
		else
		if(TypeClouds == Type.Cirrus2)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[5]);
		else
		if(TypeClouds == Type.MixNimbus)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[Random.Range(0, 4)]);
		else
		if(TypeClouds == Type.MixCirrus)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[Random.Range(4, 6)]);
		else
		if(TypeClouds == Type.MixAll)
			MyCloudParticle.SetMaterial(CloudsMatAdditive[Random.Range(0, 6)]);
		else
		if(TypeClouds == Type.PT1)
			//if(ProceduralTexture.IsInicialized())
				MyCloudParticle.SetMaterial(CloudsPTMatAdditive);
	}
	else{
		if(TypeClouds == Type.Nimbus1)
			MyCloudParticle.SetMaterial(CloudsMatBlended[0]);
		else
		if(TypeClouds == Type.Nimbus2)
			MyCloudParticle.SetMaterial(CloudsMatBlended[1]);
		else
		if(TypeClouds == Type.Nimbus3)
			MyCloudParticle.SetMaterial(CloudsMatBlended[2]);
		else
		if(TypeClouds == Type.Nimbus4)
			MyCloudParticle.SetMaterial(CloudsMatBlended[3]);
		else
		if(TypeClouds == Type.Cirrus1)
			MyCloudParticle.SetMaterial(CloudsMatBlended[4]);
		else
		if(TypeClouds == Type.Cirrus2)
			MyCloudParticle.SetMaterial(CloudsMatBlended[5]);
		else
		if(TypeClouds == Type.MixNimbus)
			MyCloudParticle.SetMaterial(CloudsMatBlended[Random.Range(0, 4)]);
		else
		if(TypeClouds == Type.MixCirrus)
			MyCloudParticle.SetMaterial(CloudsMatBlended[Random.Range(4, 6)]);
		else
		if(TypeClouds == Type.MixAll)
			MyCloudParticle.SetMaterial(CloudsMatBlended[Random.Range(0, 6)]);
		else
		if(TypeClouds == Type.PT1)
			//if(ProceduralTexture.IsInicialized())
				MyCloudParticle.SetMaterial(CloudsPTMatBlended);
	}
}

void  ManageCloudShadow (CloudParticle MyCloudParticle){
	int RandomNumber = Random.Range(0, 10);
	bool  ShowShadow = true;
	
	if(NumberOfShadows != TypeShadow.All){
		if(NumberOfShadows == TypeShadow.Most && RandomNumber > 7)
			ShowShadow = false;
		else
		if(NumberOfShadows == TypeShadow.Half && RandomNumber > 5)
			ShowShadow = false;
		else
		if(NumberOfShadows == TypeShadow.Some && RandomNumber <= 7)
			ShowShadow = false;
		else
		if(NumberOfShadows == TypeShadow.None)
			ShowShadow = false;
			
		if(!ShowShadow && MyCloudParticle.IsShadowActive())
			MyCloudParticle.SetShadowActive(false);
		else
		if(ShowShadow && !MyCloudParticle.IsShadowActive())
			MyCloudParticle.SetShadowActive(true);
	}
	else
		if(!MyCloudParticle.IsShadowActive())
			MyCloudParticle.SetShadowActive(true);
}

// Clear the array of particle objects i built  at Start
void OnDestroy (){
	MyCloudsParticles.Clear();
}

// Not used. Dont wanna draw the gizmos all the time, only when selecting the cloudManager.
/*void  OnDrawGizmos (){
	// Draw a blue sphere creation shape at the transform's position
	Gizmos.color = Color.blue;
	Gizmos.DrawWireCube (transform.position, Side);
	// Draw a yellow sphere at dissapear distance at the transform's position
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireCube (transform.position, Side * DisappearMultiplier);
}*/

// Draw the two gizmos, blue one is the first instantiated area of the clouds.
// 2nd is the limits of their movement (dissapear from one side, appear in the opposite side).
void  OnDrawGizmosSelected (){
	// Draw a blue sphere creation shape at the transform's position
	Gizmos.color = Color.blue;
	Gizmos.DrawWireCube (transform.position, Side);
	// Draw a yellow sphere at dissapear distance at the transform's position
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireCube (transform.position, Side * DisappearMultiplier);
}


public void PT1CopyInitialParameters(){
	ProceduralTexture.TextureWidth = PT1TextureWidth;
	ProceduralTexture.TextureHeight = PT1TextureHeight;

	ProceduralTexture.ScaleWidth = PT1ScaleWidth;
	ProceduralTexture.ScaleHeight = PT1ScaleHeight;
	ProceduralTexture.ScaleFactor = PT1ScaleFactor;
		
	ProceduralTexture.Seed = PT1Seed;
	ProceduralTexture.Lacunarity = PT1Lacunarity;
	ProceduralTexture.FractalIncrement = PT1FractalIncrement;
	ProceduralTexture.Octaves = PT1Octaves;
	ProceduralTexture.Offset = PT1Offset;
	
	/*ProceduralTexture.BackgroundColor = PT1BackgroundColor;
	ProceduralTexture.FinalColor = PT1FinalColor;*/
	ProceduralTexture.InvertColors = PT1InvertColors;
	ProceduralTexture.ContrastMult = PT1ContrastMult;
	
	/*ProceduralTexture.IsHalo = PT1IsHalo;*/
	ProceduralTexture.TypeNoise = (ProceduralCloudTexture.NoisePreset)PT1TypeNoise;
	
	ProceduralTexture.TurbSize = PT1TurbSize;
	ProceduralTexture.TurbLacun = PT1TurbLacun;
	ProceduralTexture.TurbGain = PT1TurbGain;
	ProceduralTexture.turbPower = PT1turbPower;
	ProceduralTexture.xyPeriod = PT1xyPeriod;
	
	ProceduralTexture.HaloEffect =  PT1HaloEffect;
	ProceduralTexture.HaloInsideRadius = PT1HaloInsideRadius;
		
	ProceduralTexture.UseAlphaTexture = PT1UseAlphaTexture;
	ProceduralTexture.AlphaIndex = PT1AlphaIndex;
	ProceduralTexture.HasChanged = true;
}

public void  ResetCloudParameters (){
	/*PT1TextureWidth = 64;
	PT1TextureHeight = 64;*/
	if(PT1TypeNoise == NoisePresetPT1.PerlinCloud){ PT1ScaleWidth = 1; PT1ScaleHeight = 1; PT1HaloInsideRadius = 1.7f;}
	else{ PT1ScaleWidth = 50; PT1ScaleHeight = 50; PT1HaloInsideRadius = 0.1f;}
		
	PT1ScaleFactor = 1;
	
	PT1Seed = 132;
	PT1Lacunarity = 3;
	PT1FractalIncrement = 0.5f;
	PT1Octaves = 7;
	PT1Offset = 1;
	
	/*PT1BackgroundColor = new Color(0, 0, 0, 1);
	PT1FinalColor = new Color(1, 1, 1, 1);*/
	PT1InvertColors = false;
	PT1ContrastMult = 0;
	
	//PT1TypeNoise = NoisePresetPT1.Cloud;
	
	PT1TurbSize = 16;
	PT1TurbLacun = 0.01f;
	PT1TurbGain = 0.5f;
	PT1turbPower = 5.0f;
	PT1xyPeriod = 0.6f;
	
	/*PT1IsHalo = true;*/
	PT1HaloEffect = 1.7f;
	//PT1HaloInsideRadius = 0.1f;
	
	PT1UseAlphaTexture = true;
	PT1AlphaIndex = 0.1f;
}

/*public void PT1ResetParameters(){
	ProceduralTexture.ResetParameters();
}*/

public void PT1CreateNewTexture(){
	ProceduralTexture.CreateNewTexture();
}

public void PT1NewRandomSeed(){
	ProceduralTexture.Seed = PT1Seed;
	ProceduralTexture.NewRandomSeed();
}

public void PT1CopyParameters(){
	//ProceduralTexture.TextureWidth = TextureWidth;
	//ProceduralTexture.TextureHeight = TextureHeight;

	ProceduralTexture.ScaleWidth = PT1ScaleWidth;
	ProceduralTexture.ScaleHeight = PT1ScaleHeight;
	ProceduralTexture.ScaleFactor = PT1ScaleFactor;
		
	ProceduralTexture.Seed = PT1Seed;
	ProceduralTexture.Lacunarity = PT1Lacunarity;
	ProceduralTexture.FractalIncrement = PT1FractalIncrement;
	ProceduralTexture.Octaves = PT1Octaves;
	ProceduralTexture.Offset = PT1Offset;
	
	/*ProceduralTexture.BackgroundColor = PT1BackgroundColor;
	ProceduralTexture.FinalColor = PT1FinalColor;*/
	ProceduralTexture.InvertColors = PT1InvertColors;
	ProceduralTexture.ContrastMult = PT1ContrastMult;
	
	ProceduralTexture.TypeNoise = (ProceduralCloudTexture.NoisePreset)PT1TypeNoise;
	
	ProceduralTexture.TurbSize = PT1TurbSize;
	ProceduralTexture.TurbLacun = PT1TurbLacun;
	ProceduralTexture.TurbGain = PT1TurbGain;
	ProceduralTexture.turbPower = PT1turbPower;
	ProceduralTexture.xyPeriod = PT1xyPeriod;
	
	/*ProceduralTexture.IsHalo = PT1IsHalo;*/
	ProceduralTexture.HaloEffect =  PT1HaloEffect;
	ProceduralTexture.HaloInsideRadius = PT1HaloInsideRadius;
		
	ProceduralTexture.UseAlphaTexture = PT1UseAlphaTexture;
	ProceduralTexture.AlphaIndex = PT1AlphaIndex;
	ProceduralTexture.HasChanged = true;
}


public void ModifyPTMaterials(){
	if(!ProceduralTexture)
		return;
	if(!ProceduralTexture.IsInicialized())
		return;
	
	int i = 0;
	// Tirdth type of Clouds. Procedural Additive textures
	CloudsPTMatAdditive.mainTexture = ProceduralTexture.MyTexture;
	// Fourth type of Clouds. Procedural Blended textures
	CloudsPTMatBlended.SetColor("_TintColor", CloudColor);
	CloudsPTMatBlended.mainTexture = ProceduralTexture.MyAlphaTexture;
}

public void  PrintPT1Paramaters (){
	Debug.Log("PT1TextureWidth : " + PT1TextureWidth.ToString());
	Debug.Log("PT1TextureHeight : " + PT1TextureHeight.ToString());
	
	Debug.Log("PT1Seed : " + PT1Seed.ToString());
	
	Debug.Log("PT1ScaleWidth : " + PT1ScaleWidth.ToString());
	Debug.Log("PT1ScaleHeight : " + PT1ScaleHeight.ToString());
	Debug.Log("PT1ScaleFactor : " + PT1ScaleFactor.ToString());
	
	Debug.Log("PT1Lacunarity : " + PT1Lacunarity.ToString());
	Debug.Log("PT1FractalIncrement : " + PT1FractalIncrement.ToString());
	Debug.Log("PT1Octaves : " + PT1Octaves.ToString());
	Debug.Log("PT1Offset : " + PT1Offset.ToString());

	Debug.Log("PT1HaloEffect : " + PT1HaloEffect.ToString());
	Debug.Log("PT1HaloInsideRadius : " + PT1HaloInsideRadius.ToString());
	
	/*Debug.Log("PT1BackgroundColor : " + PT1BackgroundColor.ToString());
	Debug.Log("PT1FinalColor : " + PT1FinalColor.ToString());*/
	Debug.Log("PT1InvertColors : " + PT1InvertColors.ToString());
	Debug.Log("PT1ContrastMult : " + PT1ContrastMult.ToString());
		
	Debug.Log("PT1UseAlphaTexture : " + PT1UseAlphaTexture.ToString());
	Debug.Log("PT1AlphaIndex : " + PT1AlphaIndex.ToString());
}


public void  SaveProceduralTexture (){
	int screenshotCount = 0;
   	string PTAddFilename;
	string PTBlendedFilename;
	byte[] bytes;
	
	do{
		screenshotCount++;
		PTAddFilename = "Assets/Volumetric Clouds/Textures/Procedural/PTAdd" + screenshotCount + ".png";
		PTBlendedFilename = "Assets/Volumetric Clouds/Textures/Procedural/PTBlended" + screenshotCount + ".png";
	}while (System.IO.File.Exists(PTAddFilename));
   
	// You can comment this if you're targeting to Web Player, because this causes an compilation error
	// Just comment from this #ifUNITY_EDITOR (included) until the #endif (included too).
	// and i will go perfect, but will not be able to save your procedural textures.
	// Otherwise change the platform to Windows /Mac Standalone in File --> Build Settings.
	#if UNITY_EDITOR
		bytes = ProceduralTexture.MyTexture.EncodeToPNG();
		System.IO.File.WriteAllBytes(PTAddFilename, bytes);
		
		bytes = ProceduralTexture.MyAlphaTexture.EncodeToPNG();
		System.IO.File.WriteAllBytes(PTBlendedFilename, bytes);
	#endif

}

}