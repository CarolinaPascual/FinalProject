// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//http://developer.download.nvidia.com/cg/index_stdlib.html

Shader "Clase/10-ToonDiffuse"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		pass
		{
			//tags
			Tags { "LightMode" = "ForwardBase" "RenderType" = "Opaque" "RenderQueue" = "Opaque"}

			CGPROGRAM

			//PRAGMAS
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				//POSITION, NORMAL, TANGENT, COLOR, TEXCOORD0 ~ TEXCOORD8
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 position : SV_POSITION; // SI O SI
				float2 uv : TEXCOORD0;
				float3 worldNormal : COLOR;
			};

			float4 _Color;
			sampler2D _MainTex;
			uniform fixed4 _LightColor0; //color de la luz 0 (sol)
			//uniform = valor se carga de afuera del shader

			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;
				OUT.worldNormal = normalize(mul(unity_ObjectToWorld,IN.normal));
		

//				OUT.diffuse = NdotL;
				return OUT;
			}

			fixed Posterize(fixed NdotL, int steps)
			{
				return ceil(NdotL*steps)/steps;
			}

			float4 frag (v2f v) : COLOR
			{
				float4 col = tex2D(_MainTex, v.uv);
				fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 worldNormal = normalize(v.worldNormal);

				//---Diffuse---
				fixed NdotL = saturate(dot(worldNormal, lightDir));
				NdotL = Posterize(NdotL,1);
																	//solo luz solida de ambiente
//				fixed3 diffuseReflection = NdotL *_LightColor0.rgb + UNITY_LIGHTMODEL_AMBIENT;

																	//luces de skybox de ambiente
				fixed3 diffuseReflection = NdotL *_LightColor0.rgb + ShadeSH9(half4(worldNormal,1));
				//-------------

				return col * float4(diffuseReflection,1);
			}

			ENDCG
		}

	}

	Fallback "Diffuse"
}