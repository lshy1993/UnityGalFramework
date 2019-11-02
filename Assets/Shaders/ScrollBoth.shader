Shader "Custom/ScrollBoth" {
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
				float2 ouv, nuv;
				float rate;
				//左
				if (_Direction == 0)
				{
					rate = i.uv.x;
					ouv = float2(i.uv.x - progress, i.uv.y);
					nuv = float2(1 - progress + i.uv.x, i.uv.y);
				}
				//右
				if (_Direction == 1)
				{
					rate = 1 - i.uv.x;
					ouv = float2(progress + i.uv.x, i.uv.y);
					nuv = float2(i.uv.x - 1 + progress, i.uv.y);
				}
				//上
				if (_Direction == 2)
				{
					rate = 1 - i.uv.y;
					ouv = float2(i.uv.x, i.uv.y + progress);
					nuv = float2(i.uv.x, i.uv.y - 1 + progress);
				}
				//下
				if (_Direction == 3)
				{
					rate = i.uv.y;
					ouv = float2(i.uv.x, i.uv.y - progress);
					nuv = float2(i.uv.x, 1 - progress + i.uv.y);
				}
				if (progress < rate)
					return tex2D(_MainTex, ouv);
				else
					return tex2D(_NewTex, nuv);

			}
			ENDCG
		}
	}

	Fallback off
}
