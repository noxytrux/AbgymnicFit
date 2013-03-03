
//====================================================================================================================================
//
// Copyright Julian Oliden "Jocyf" 2010 - 18-11-2010
// Procedural Cloud Texture v1.1
// Intensive particle system personalizable for Volumetric Clouds generation.
//
//====================================================================================================================================
using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(CloudsToy))]
public class CloudsToyEditor : Editor {
	private int i = 0;
	private int MyWidth = 100;
	private bool showAdvancedSettings = false;
	private bool showMaximunClouds = false;
	private CloudsToy CloudSystem;
	private ProceduralCloudTexture ProcText = null;

    public override void OnInspectorGUI() {
		//EditorGUIUtility.LookLikeInspector();
		EditorGUIUtility.LookLikeControls();
		CloudSystem = (CloudsToy) target;
		if (!CloudSystem.gameObject) {
			return;
		}
		
		ProcText = (ProceduralCloudTexture) CloudSystem.ProceduralTexture;

		EditorGUILayout.BeginVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		string[] MenuOptions = new string[2];
		MenuOptions[0] = "    Clouds    ";
		MenuOptions[1] = "Proc Texture";
		CloudSystem.ToolbarOptions = GUILayout.Toolbar(CloudSystem.ToolbarOptions, MenuOptions);
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical();
		switch (CloudSystem.ToolbarOptions) {
			
			
		case 0: 
			// Is the CloudsToy being executed in Unity? If not, show the Maximun Clouds parameter.
			if(!ProcText){
				showMaximunClouds = EditorGUILayout.Foldout(showMaximunClouds, " Maximun Clouds (DO NOT change while executing)");
				if(showMaximunClouds)
					CloudSystem.MaximunClouds = EditorGUILayout.IntField("  ", CloudSystem.MaximunClouds);
					if (GUI.changed)
						EditorUtility.SetDirty(CloudSystem);
				GUI.changed = false;
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
			}
			CloudSystem.CloudPreset = (CloudsToy.TypePreset)EditorGUILayout.EnumPopup("  Cloud Presets: ", CloudSystem.CloudPreset);
			if (GUI.changed) {
				if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Stormy)
					CloudSystem.SetPresetStormy();
				else
				if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Sunrise)
					CloudSystem.SetPresetSunrise();
				else
				if(CloudSystem.CloudPreset == CloudsToy.TypePreset.Fantasy)
					CloudSystem.SetPresetFantasy();
				EditorUtility.SetDirty(CloudSystem);
			}
			GUI.changed = false;
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			CloudSystem.CloudRender = (CloudsToy.TypeRender)EditorGUILayout.EnumPopup("  Cloud Render: ", CloudSystem.CloudRender);
			CloudSystem.TypeClouds = (CloudsToy.Type)EditorGUILayout.EnumPopup("  Cloud Type: ", CloudSystem.TypeClouds);
			CloudSystem.CloudDetail = (CloudsToy.TypeDetail)EditorGUILayout.EnumPopup("  Cloud Detail: ", CloudSystem.CloudDetail);
			if (GUI.changed) {
				CloudSystem.SetCloudDetailParams();
				EditorUtility.SetDirty(CloudSystem);
			}
			GUI.changed = false;
				
			showAdvancedSettings = EditorGUILayout.Foldout(showAdvancedSettings, "Particles Advanced Settings");
			if(showAdvancedSettings){
				EditorGUILayout.Separator();
				CloudSystem.SizeFactorPart = EditorGUILayout.Slider("  Size Factor: ", CloudSystem.SizeFactorPart, 0.1f, 4.0f);
				CloudSystem.EmissionMult = EditorGUILayout.Slider("  Emitter Mult: ", CloudSystem.EmissionMult, 0.1f, 4.0f);
				EditorGUILayout.Separator();
			}
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			Rect buttonRect = EditorGUILayout.BeginHorizontal();
			buttonRect.x = buttonRect.width / 2 - 100;
			buttonRect.width = 200;
			buttonRect.height = 30;
			//GUI.skin??
			if(GUI.Button(buttonRect, "Repaint Clouds")){
				CloudSystem.EditorRepaintClouds();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			CloudSystem.SoftClouds = EditorGUILayout.Toggle("  Soft Clouds", CloudSystem.SoftClouds);
			if(CloudSystem.SoftClouds){
				CloudSystem.SpreadDir = EditorGUILayout.Vector3Field("  Spread Direction: ", CloudSystem.SpreadDir);
				CloudSystem.LengthSpread = EditorGUILayout.Slider("  Length Spread: ", CloudSystem.LengthSpread, 1, 30);
			}
			EditorGUILayout.Separator();
			CloudSystem.NumberClouds = EditorGUILayout.IntSlider("  Clouds Num: ", CloudSystem.NumberClouds, 1, CloudSystem.MaximunClouds);
			EditorGUILayout.Separator();
			CloudSystem.Side = EditorGUILayout.Vector3Field("  Cloud Creation Size: ", CloudSystem.Side);
			CloudSystem.DisappearMultiplier = EditorGUILayout.Slider("  Dissapear Mult: ", CloudSystem.DisappearMultiplier, 1, 10);
			EditorGUILayout.Separator();
			CloudSystem.MaximunVelocity = EditorGUILayout.Vector3Field("  Maximun Velocity: ", CloudSystem.MaximunVelocity);
			CloudSystem.VelocityMultipier = EditorGUILayout.Slider("  Velocity Mult: ", CloudSystem.VelocityMultipier, 0, 20);
			EditorGUILayout.Separator();
			CloudSystem.PaintType = (CloudsToy.TypePaintDistr)EditorGUILayout.EnumPopup("  Paint Type: ", CloudSystem.PaintType);
			if(CloudSystem.CloudRender == CloudsToy.TypeRender.Realistic)
				CloudSystem.CloudColor = EditorGUILayout.ColorField("  Cloud Color: ", CloudSystem.CloudColor);
			CloudSystem.MainColor = EditorGUILayout.ColorField("  Main Color: ", CloudSystem.MainColor);
			CloudSystem.SecondColor = EditorGUILayout.ColorField("  Secondary Color: ", CloudSystem.SecondColor);
			CloudSystem.TintStrength = EditorGUILayout.IntSlider("  Tint Strength: ", CloudSystem.TintStrength, 1, 100);
			if(CloudSystem.PaintType == CloudsToy.TypePaintDistr.Below)
				CloudSystem.offset = EditorGUILayout.Slider("  Offset: ", CloudSystem.offset, 0, 1);
			EditorGUILayout.Separator();
			CloudSystem.MaxWithCloud = EditorGUILayout.IntSlider("  Width: ", CloudSystem.MaxWithCloud, 10, 1000);
			CloudSystem.MaxTallCloud = EditorGUILayout.IntSlider("  Height: ", CloudSystem.MaxTallCloud, 5, 500);
			CloudSystem.MaxDepthCloud = EditorGUILayout.IntSlider("  Depth: ", CloudSystem.MaxDepthCloud, 5, 1000);
			CloudSystem.FixedSize = EditorGUILayout.Toggle("  Fixed Size", CloudSystem.FixedSize);
			EditorGUILayout.Separator();
			CloudSystem.IsAnimate = EditorGUILayout.Toggle("  Animate Cloud", CloudSystem.IsAnimate);
			if(CloudSystem.IsAnimate) 
				CloudSystem.AnimationVelocity = EditorGUILayout.Slider("  Animation Velocity: ", CloudSystem.AnimationVelocity, 0, 1);
			CloudSystem.NumberOfShadows = (CloudsToy.TypeShadow)EditorGUILayout.EnumPopup("  Shadows: ", CloudSystem.NumberOfShadows);
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			EditorGUILayout.LabelField("Texture Add", "Used for Bright Clouds");
			EditorGUILayout.BeginHorizontal();
			for(i = 0; i < CloudSystem.CloudsTextAdd.Length; i++){
				if(i == CloudSystem.CloudsTextAdd.Length/2){
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
				}
				else
				if(i != 0 && i != 3)
					EditorGUILayout.Separator();
				CloudSystem.CloudsTextAdd[i] = (Texture2D)EditorGUILayout.ObjectField(CloudSystem.CloudsTextAdd[i], typeof(Texture2D), GUILayout.Width(70), GUILayout.Height(70));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			EditorGUILayout.LabelField("Text Blended", "Used for Realistic Clouds");
			EditorGUILayout.BeginHorizontal();
			for(i = 0; i < CloudSystem.CloudsTextBlended.Length; i++){
				if(i == CloudSystem.CloudsTextBlended.Length/2){
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
				}
				else
				if(i != 0 && i != 3)
					EditorGUILayout.Separator();
				CloudSystem.CloudsTextBlended[i] = (Texture2D)EditorGUILayout.ObjectField(CloudSystem.CloudsTextBlended[i], typeof(Texture2D), GUILayout.Width(70), GUILayout.Height(70));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			if (GUI.changed)
				EditorUtility.SetDirty (CloudSystem);
			GUI.changed = false;
			break;
			
			
		case 1:
			if(!ProcText){
				CloudSystem.PT1TextureWidth = EditorGUILayout.IntField("  Texture Width: ", CloudSystem.PT1TextureWidth);
				CloudSystem.PT1TextureHeight = EditorGUILayout.IntField("  Texture Height: ", CloudSystem.PT1TextureHeight);
				if (GUI.changed && ProcText) {
					EditorUtility.SetDirty(CloudSystem);
				}
			}
			GUI.changed = false;
			EditorGUILayout.Separator();
			CloudSystem.PT1TypeNoise = (CloudsToy.NoisePresetPT1)EditorGUILayout.EnumPopup("  Type Noise: ", CloudSystem.PT1TypeNoise);
			EditorGUILayout.Separator();
			CloudSystem.PT1Seed = EditorGUILayout.IntSlider("  Seed: ", CloudSystem.PT1Seed, 1, 10000);
			EditorGUILayout.Separator();
			if (GUI.changed && ProcText) {
				CloudSystem.PT1NewRandomSeed();
				EditorUtility.SetDirty(CloudSystem);
			}
			GUI.changed = false;
			CloudSystem.PT1ScaleWidth = EditorGUILayout.Slider("  Scale Width: ", CloudSystem.PT1ScaleWidth, 0.1f, 50.0f);
			CloudSystem.PT1ScaleHeight = EditorGUILayout.Slider("  Scale Height: ", CloudSystem.PT1ScaleHeight, 0.1f, 50.0f);
			CloudSystem.PT1ScaleFactor = EditorGUILayout.Slider("  Scale Factor: ", CloudSystem.PT1ScaleFactor, 0.1f, 2.0f);
			EditorGUILayout.Separator();
			if(CloudSystem.PT1TypeNoise == CloudsToy.NoisePresetPT1.PerlinCloud){
				CloudSystem.PT1Lacunarity = EditorGUILayout.Slider("  Lacunarity: ", CloudSystem.PT1Lacunarity, 0.0f, 10.0f);
				CloudSystem.PT1FractalIncrement = EditorGUILayout.Slider("  FractalIncrement: ", CloudSystem.PT1FractalIncrement, 0.0f, 2.0f);
				CloudSystem.PT1Octaves = EditorGUILayout.Slider("  Octaves: ", CloudSystem.PT1Octaves, 0.0f, 10.0f);
				CloudSystem.PT1Offset = EditorGUILayout.Slider("  Offset: ", CloudSystem.PT1Offset, 0.1f, 3.0f);
			}else
			if(CloudSystem.PT1TypeNoise == CloudsToy.NoisePresetPT1.Cloud){
				CloudSystem.PT1TurbSize = EditorGUILayout.IntSlider("  Turb Size: ", CloudSystem.PT1TurbSize, 1, 256);
				CloudSystem.PT1TurbLacun = EditorGUILayout.Slider("  Turb Lacun: ", CloudSystem.PT1TurbLacun, 0.01f, 0.99f);
				CloudSystem.PT1TurbGain = EditorGUILayout.Slider("  Turb Gain: ", CloudSystem.PT1TurbGain, 0.01f, 2.99f);
				CloudSystem.PT1xyPeriod = EditorGUILayout.Slider("  Radius: ", CloudSystem.PT1xyPeriod, 0.1f, 2.0f);
				CloudSystem.PT1turbPower = EditorGUILayout.Slider("  Turb Power: ", CloudSystem.PT1turbPower, 1, 60);
			}
			EditorGUILayout.Separator();
			/*CloudSystem.PT1IsHalo = EditorGUILayout.Toggle("  Halo Active:", CloudSystem.PT1IsHalo);
			if(CloudSystem.PT1IsHalo)*/
			CloudSystem.PT1HaloEffect = EditorGUILayout.Slider("  HaloEffect: ", CloudSystem.PT1HaloEffect, 0.1f, 1.7f);
			CloudSystem.PT1HaloInsideRadius = EditorGUILayout.Slider("  Inside Radius: ", CloudSystem.PT1HaloInsideRadius, 0.1f, 3.5f);
			EditorGUILayout.Separator();
			/*CloudSystem.PT1BackgroundColor = EditorGUILayout.ColorField("  Back Color: ", CloudSystem.PT1BackgroundColor);
			CloudSystem.PT1FinalColor = EditorGUILayout.ColorField("  Front Color: ", CloudSystem.PT1FinalColor);*/
			CloudSystem.PT1InvertColors = EditorGUILayout.Toggle("  Invert Colors:", CloudSystem.PT1InvertColors);
			CloudSystem.PT1ContrastMult = EditorGUILayout.Slider("  Contrast Mult: ", CloudSystem.PT1ContrastMult, 0.0f, 2.0f);
			EditorGUILayout.Separator();
			CloudSystem.PT1UseAlphaTexture = EditorGUILayout.Toggle("  Alpha Texture:", CloudSystem.PT1UseAlphaTexture);
			if(CloudSystem.PT1UseAlphaTexture)
				CloudSystem.PT1AlphaIndex = EditorGUILayout.Slider("  Alpha Index: ", CloudSystem.PT1AlphaIndex, 0.0f, 1.0f);
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			// Be sure that the Texture1 class exists before try to paint the textures.
			if(ProcText){
				MyWidth = EditorGUILayout.IntSlider("  BoxDraw Text: ", MyWidth, 50, 200);
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				ProcText.MyTexture = (Texture2D)EditorGUILayout.ObjectField(ProcText.MyTexture, typeof(Texture2D), GUILayout.Width(MyWidth), GUILayout.Height(MyWidth));
				EditorGUILayout.Separator();
				EditorGUILayout.Space();
				if(CloudSystem.PT1UseAlphaTexture)
					ProcText.MyAlphaDrawTexture = (Texture2D)EditorGUILayout.ObjectField(ProcText.MyAlphaDrawTexture, typeof(Texture2D), GUILayout.Width(MyWidth), GUILayout.Height(MyWidth));
				EditorGUILayout.Separator();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
			}
			
			Rect buttonRectPT1 = EditorGUILayout.BeginHorizontal();
			buttonRectPT1.x = buttonRectPT1.width / 2 - 100;
			buttonRectPT1.width = 200;
			buttonRectPT1.height = 30;
			//GUI.skin??
			if(GUI.Button(buttonRectPT1, "Reset Parameters")){
				CloudSystem.ResetCloudParameters();
				if(ProcText){
					CloudSystem.PT1CopyParameters();
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			// Is the program being executed? If so, show the 'Save Params' button.
			if(ProcText){
				Rect buttonRectPrint = EditorGUILayout.BeginHorizontal();
				buttonRectPrint.x = buttonRectPrint.width / 2 - 100;
				buttonRectPrint.width = 200;
				buttonRectPrint.height = 30;
				//GUI.skin??
				if(GUI.Button(buttonRectPrint, "Save Texture")){
					CloudSystem.SaveProceduralTexture();
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			if (GUI.changed){
				if(ProcText){
					CloudSystem.PT1CopyParameters();
					CloudSystem.ModifyPTMaterials();
				}
				EditorUtility.SetDirty (CloudSystem);
			}
			GUI.changed = false;
			
			break;
		
		}
		EditorGUILayout.EndVertical();
    }
}
