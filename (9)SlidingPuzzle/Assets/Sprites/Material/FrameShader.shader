Shader "Unlit/FrameShader" {
	Properties {
		_MainTex  ("Texture", 2D) = "white" {}
		_FrameTex ("Image Frame", 2D) = "white" {}
		_count("Slice Counter", Range(1, 6)) = 4
	}

	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
		ZTest Off
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _FrameTex;
			int _count;

			float4 _MainTex_ST;
			
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_FrameTex, i.uv * _count);

				if (_count > 0) {
					return col * col2;
				} else {
					return col;
				}
			}
			ENDCG
		}
	}
}
