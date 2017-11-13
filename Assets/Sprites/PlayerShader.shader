Shader "Unlit/PlayerShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}

		// Additional textures
		_ShirtTex("ShirtTex(RGB)", 2D) = "transparent" {}
		_ShirtColour("ShirtColour", Color) = (1,1,1,1)

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			// 2D texture sampler
			sampler2D _ShirtTex;
			float4 _ShirtTex_ST;
			float4 _ShirtColour;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 colour1 = tex2D(_MainTex, i.uv);
				fixed4 colour2 = tex2D(_ShirtTex, i.uv);
			
				//fixed4 colour2 = fixed4(154, 18, 18, 255);

				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, colour1);

				// colour of red in sprite is in RGBA: 154, 18, 18, 255			Hex colour = 9A1212FF
				
				
				/*
				if (colour1.r == colour2.r && colour1.g == colour2.g && colour1.b == colour2.b) {
					return (_ShirtColour);
				}
				*/
				fixed4 final = colour2;
				//final = lerp(colour1, _ShirtColour, colour2.a);  //change area in red
				//colour1 = lerp(colour1, (0, 0, 0, 0), );

				return final;
			}
			ENDCG
		}
	}
}
