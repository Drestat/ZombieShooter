//	Copyright 2015 Unluck Software	
//	www.chemicalbliss.com
Shader "Unluck Software/Vertex Color/Toon Simple"{
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_UnlitColor ("Diffuse Color", Color) = (0.5,0.5,0.5,1) 
		_DiffuseThreshold ("Diffuse Threshold", Range(0,1)) = 0.1
		_ColorMultiplier ("Color Multiplier", float) = 2
	}
	SubShader {
		Tags {"RenderType"="Opaque" "IgnoreProjector"="True"}
		Cull Off
		ZWrite On
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
				float4 posWorld : TEXCOORD2;
	            float3 normalDir : TEXCOORD1;
			};
			
			v2f vert (appdata v) {
				v2f o;
				o.posWorld = mul(_Object2World, v.vertex);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	            float4x4 modelMatrixInverse = _World2Object; 
	            o.normalDir = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz));
				o.lightDir = normalize(_WorldSpaceCameraPos.xyz - o.posWorld.xyz); 
	            o.color = v.color;
				return o;
			}
			
			half4 frag(v2f i) : COLOR {
				float3 normalDirection = normalize(i.normalDir).xyz;
	 			fixed3 vertexColor = fixed3(i.color.rgb);
	            float3 viewDirection = i.lightDir;
	            float3 lightDirection;
	            float attenuation = 1.0;
				lightDirection = i.lightDir;
	            fixed3 fragmentColor = fixed3(_UnlitColor.rgb); 
	            if (attenuation * max(0.0, dot(normalDirection, lightDirection)) >= _DiffuseThreshold) {
	               fragmentColor = fixed3(_Color.rgb); 
	            }
	            return float4(fragmentColor*vertexColor*_ColorMultiplier, 1.0);
			}
			ENDCG
		}
	}
	Fallback "Unluck Software/Vertex Color/Standard"
}