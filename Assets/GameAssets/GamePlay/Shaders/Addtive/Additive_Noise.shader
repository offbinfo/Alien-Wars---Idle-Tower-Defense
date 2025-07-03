Shader "GCenter/Additive/Noise"
{
    Properties
	{	
		_MainTex("MainTex", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_Emission("Emission", Float) = 2
		_Color("Color", Color) = (0.5,0.5,0.5,1)

		_StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
	}

	Category 
	{
		SubShader
		{
			Tags
			{
				"Queue"="Transparent"
				"IgnoreProjector"="True"
				"RenderType"="Transparent"
				"PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
			}

			Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

			Blend One One
			ColorMask [_ColorMask]
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest [unity_GUIZTestMode]
			
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;	
				};	
				
				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform float4 _SpeedMainTexUVNoiseZW;
				uniform sampler2D _Noise;
				uniform float4 _Noise_ST;
				uniform float4 _Color;
				uniform float _Emission;

				v2f vert ( appdata_t v  )
				{
					v2f o;
					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = v.texcoord;
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					float2 appendResult21 = float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y);
					float2 uv0_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					float2 panner107 = 1.0 * _Time.y * appendResult21 + uv0_MainTex;
					float4 tex2DNode13 = tex2D( _MainTex, panner107 );
					float2 appendResult22 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
					float2 uv0_Noise = i.texcoord.xy * _Noise_ST.xy + _Noise_ST.zw;
					float2 panner108 = ( 1.0 * _Time.y * appendResult22 + uv0_Noise);
					float4 tex2DNode14 = tex2D( _Noise, panner108 );
					return tex2DNode13 * tex2DNode14 * _Color * i.color * tex2DNode13.a * tex2DNode14.a * _Color.a * i.color.a * _Emission;
				}
				ENDCG 
			}
		}	
	}
}
