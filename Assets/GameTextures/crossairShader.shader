Shader "GUI/CrossAir Shader" { 
Properties { 
   _MainTex ("Texture", 2D) = "white" {} 
   _Color ("Color", Color) = (1,1,1,1) 
} 

SubShader { 
   Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" } 
   Lighting Off Cull Off ZWrite Off Fog { Mode Off } 
   Blend SrcAlpha OneMinusSrcAlpha 
   
   Pass { 
      Color [_Color] 
      SetTexture [_MainTex] { 
         combine primary, texture * primary 
      } 
   } 
} 
}