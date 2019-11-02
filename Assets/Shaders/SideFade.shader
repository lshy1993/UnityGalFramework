Shader "Custom/SideFade" {
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
			float4 _TexSize;

			//方向
			int _Direction;
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
				float progress = currentT;
				//混合度
				float gray;
				//左
				if (_Direction == 0)
				{
					float x = changeval(i.uv.x + 1 - 2 * progress);
					gray = 1 - x;
				}
				//右
				if (_Direction == 1)
				{
					float x = changeval(i.uv.x - 1 + 2 * progress);
					gray = x;
				}
				//上
				if (_Direction == 2)
				{
					float y = changeval(i.uv.y - 1 + 2 * progress);
					gray = y;
				}
				//下
				if (_Direction == 3)
				{
					float y = changeval(i.uv.y + 1 - 2 * progress);
					gray = 1 - y;
				}
				//贴图混合
				float4 oldp = tex2D(_MainTex, i.uv);
				float4 newp = tex2D(_NewTex, i.uv);
				float3 outp = oldp.rgb*(1 - gray) + newp.rgb* gray;
				return float4(outp, 1);

			}
			ENDCG
		}
	}

	Fallback off
}
