Shader "Custom/Scroll" {
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

			float4 frag(v2f i) : COLOR
			{
				//当前进度(0-1)
				float progress = currentT;
				float2 nuv;
				float rate;
				//左
				if (_Direction == 0)
				{
					rate = i.uv.x;
					nuv = float2(1 - progress + i.uv.x, i.uv.y);
				}
				//右
				if (_Direction == 1)
				{
					rate = 1 - i.uv.x;
					nuv = float2(i.uv.x - 1 + progress, i.uv.y);
				}
				//上
				if (_Direction == 2)
				{
					rate = 1 - i.uv.y;
					nuv = float2(i.uv.x, i.uv.y - 1 + progress);
				}
				//下
				if (_Direction == 3)
				{
					rate = i.uv.y;
					nuv = float2(i.uv.x, 1 - progress + i.uv.y);
				}				
				if (progress < rate)
					return tex2D(_MainTex, i.uv);
				else
					return tex2D(_NewTex, nuv);

			}
			ENDCG
		}
	}

	Fallback off
}
