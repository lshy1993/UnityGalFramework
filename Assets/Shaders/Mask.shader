Shader "Custom/Mask" {
	Properties {
		_MainTex ("Old Texture", 2D) = "white" {}
		_TexSize("Texture Size", vector) = (1920, 1080, 0, 0)
	}
	SubShader{
		LOD 200

		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		Pass{
			ZTest Always Cull Off ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Fog{ Mode off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _NewTex;
			sampler2D _MaskTex;
			float4 _TexSize;

			//阈值
			float vague = 0.2;
			//进度
			float currentT;

			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			float changeval(float x)
			{
				if (x > 1) x = 1;
				if (x < 0) x = 0;
				return x;
			}

			float4 frag(v2f i) : COLOR
			{
				//当前进度(0-1)
				float progress = currentT * (1 + 0.2);
				//该点的原始混合度获取
				float4 col = tex2D(_MaskTex, i.uv);
				float gray = dot(col.rgb, fixed3(0.3, 0.59, 0.11));
				if (progress > gray + 0.2)
				{
					return tex2D(_NewTex, i.uv);
				}
				else if (progress > gray)
				{
					//贴图混合
					float per = (progress - gray) / 0.2;
					float4 oldp = tex2D(_MainTex, i.uv);
					float4 newp = tex2D(_NewTex, i.uv);
					float3 outp = oldp.rgb*(1 - per) + newp.rgb* per;
					return float4(outp, 1);
				}
				else
				{
					return tex2D(_MainTex, i.uv);

				}

			}

			ENDCG
		}
	}

	Fallback off
}
