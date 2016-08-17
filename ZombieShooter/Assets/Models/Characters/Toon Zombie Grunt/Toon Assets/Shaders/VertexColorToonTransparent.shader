//	Copyright 2015 Unluck Software	
//	www.chemicalbliss.com
Shader "Unluck Software/Vertex Color/Toon Transparent"
{
	Properties{
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_UnlitColor ("Diffuse Color", Color) = (0.5,0.5,0.5,1) 
		_DiffuseThreshold ("Diffuse Threshold", Range(0,1)) = 0.1 
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_LitOutlineThickness ("Lit Outline Thickness", Range(0,1)) = 0.1
		_UnlitOutlineThickness ("Unlit Outline Thickness", Range(0,1)) = 0.4
		_SpecColor ("Specular Color", Color) = (1,1,1,1) 
		_Shininess ("Shininess", float) = 10
		_ColorMultiplier ("Color Multiplier", float) = 2
	}
	
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		Fog { Mode Off }
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			fixed4 _Color;		 
			uniform fixed4 _LightColor0;       
			uniform fixed4 _UnlitColor;
			uniform fixed _DiffuseThreshold;
			uniform fixed4 _OutlineColor;
			uniform fixed _LitOutlineThickness;
			uniform fixed _UnlitOutlineThickness;
			uniform fixed4 _SpecColor; 
			uniform fixed _Shininess;	
			uniform fixed _ColorMultiplier;
			
			struct appdata {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float3 normal : NORMAL;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float3 lightDir : TEXCOORD0;
	            float3 normalDir : TEXCOORD1;
	            float4 posWorld : TEXCOORD2;
			};
			
			v2f vert (appdata v) {
				v2f o;
				o.posWorld = mul(_Object2World, v.vertex);
	            float4x4 modelMatrixInverse = _World2Object; 
	            o.normalDir = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz));
	            o.lightDir = normalize(float3(_WorldSpaceCameraPos.x,_WorldSpaceCameraPos.y,_WorldSpaceCameraPos.z)- o.posWorld.xyz);
	            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	            o.color = v.color;	
				return o;
			}
			
			
			half4 frag(v2f i) : COLOR {
				float3 normalDirection = normalize(i.normalDir).xyz;
	 			fixed3 vertexColor = fixed3(i.color.rgb);
	 			float3 lightDirection= i.lightDir;
	            float3 viewDirection = i.lightDir;          
	            float attenuation = 1.0;			
	            fixed3 fragmentColor = fixed3(_UnlitColor.rgb); 
	            if (attenuation * max(0.0, dot(normalDirection, lightDirection)) >= _DiffuseThreshold) {
	               fragmentColor = fixed3(_Color.rgb); 
	            }
	            if (dot(viewDirection, normalDirection) < lerp(_UnlitOutlineThickness, _LitOutlineThickness, max(0.0, dot(normalDirection, lightDirection)))) {
	               fragmentColor = fixed3(_OutlineColor.rgb); 
	            }
	            if (dot(normalDirection, lightDirection) > 0.0 && attenuation *  pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess) > 0.5) {
	               fragmentColor = _SpecColor.a 
	                  * fixed3(_SpecColor.rgb)             
	                  + (1.0 - _SpecColor.a)                
	                  * fragmentColor;
	            }
	            return float4(fragmentColor*vertexColor*_ColorMultiplier, _Color.a);
			}	
			ENDCG
		}
	}
	Fallback "Unluck Software/Vertex Color/Standard"
}