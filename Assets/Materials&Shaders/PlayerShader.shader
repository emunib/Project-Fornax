Shader "Unlit/PlayerShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "transparent" {}

		// Additional textures
		_ShirtTex("ShirtTex (RGB)", 2D) = "transparent" {}
		_ColourTex("ColourTex (RGB)", 2D) = "transparent" {} // texture containing original shirt colour.
		_ShirtColour1("ShirtColour1", Color) = (1,1,1,0)
		_ShirtColour2("ShirtColour2", Color) = (1,1,1,0)

	}
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque" 
			"Queue" = "Transparent"
		}
		LOD 100

		// Required so background has alpha.
		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

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

			sampler2D _ColourTex;
			float4 _ColourTex_ST;

			float4 _ShirtColour1;
			float4 _ShirtColour2;
			
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
				// NOTE: tex2D returns a vector (0,0,0,0) with each value ranging from 0.0f - 1.0f
				fixed4 colour1 = tex2D(_MainTex, i.uv);		// does nothing/not used.
				fixed4 colour2 = tex2D(_ShirtTex, i.uv);	// Gets color of texture at coordinate.
			
				fixed4 colour3 = tex2D(_ColourTex, float2(0, 0));	// light red of shirt.
				fixed4 colour4 = tex2D(_ColourTex, float2(1, 0));	// dark red of shirt.

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, colour1);

				
				fixed4 result = colour2;
				// Use step(x, y) if we need to, more efficient than if statement. If y >= x return 1, else return 0.

				if (colour2.r == colour3.r) {
					result = lerp(colour2, _ShirtColour1, colour2.a);  // change area in light red
				}
				if (colour2.r == colour4.r) {
					result = lerp(colour2, _ShirtColour2, colour2.a);  // change area in dark red
				}
				result.a = colour2.a;	// Keep alpha channel the same.
		 
				return result;
			}
			ENDCG
		}
	}
}
