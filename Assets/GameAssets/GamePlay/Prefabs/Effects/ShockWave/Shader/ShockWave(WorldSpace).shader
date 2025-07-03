// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShockWave(WorldSpace)" 
{
	Properties 
	{
		_Radius ("Radius", Range(-0.20,0.5)) = 0.2
		_Amplitude ("Amplitude", Range(-10,10)) = 0.05
		_WaveSize  ("WaveSize", Range(0,5)) = 0.2

		_ScreenRatio ("ScreenRatio", Float) = 1 //Width/Height
	}
	 
	SubShader 
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

        GrabPass {"_GrabTexture"}
//		ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings
	 	Blend SrcAlpha OneMinusSrcAlpha


	 	Pass
	 	{
            Cull Off
            ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
//			#pragma surface surf SimpleSpecular alpha
			#include "UnityCG.cginc"

			float _Radius;
			float _Amplitude;
			float _ScreenRatio;
			float _WaveSize;

			sampler2D _GrabTexture;
            int _UseDepth;

            sampler2D _CameraDepthTexture;

			struct v2f 
			{
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
				float4 GrabUV : TEXCOORD2;
			};
	   
			//Our Vertex Shader
			v2f vert (appdata_img v){
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				o.GrabUV = ComputeGrabScreenPos(o.pos);
				return o;
			}

			//Our Fragment Shader
			fixed4 frag (v2f i) : COLOR
			{

				float2 diff=float2(i.uv.x-0.5, (i.uv.y-0.5) ); 

				float dist=sqrt(diff.x * diff.x + diff.y * diff.y);

				float2 uv_displaced = float2(0,0);

				int transparent = 0;

				if (dist>_Radius) 
				{
					if (dist<_Radius+_WaveSize) 
					{
						float angle=(dist-_Radius)*2*3.141592654/_WaveSize;
						float cossin=(1-cos(angle))*0.5;
						uv_displaced.x-=cossin*diff.x*_Amplitude/dist;
						uv_displaced.y-=cossin*diff.y*_Amplitude/dist;

						transparent = 1;
					}
				}

				fixed4 GUV = i.GrabUV;

				GUV.x += uv_displaced.x;
				GUV.y += uv_displaced.y;

				fixed4 orgCol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(GUV));

				if (transparent == 0)
				{
					orgCol.a = 0;
				}

				return orgCol;
			}
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
