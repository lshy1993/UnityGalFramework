Shader "Custom/RotateFade" {
	Properties {
		_MainTex ("Old Texture", 2D) = "white" {}
		_TexSize("Texture Size", vector) = (1920, 1080, 0, 0)
		_Center("Center", vector) = (0.5, 0.5, 0, 0)
	}
	SubShader {
		LOD 200
		
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Fog { Mode off }
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MaskTex;
			sampler2D _NewTex;
			float2 _Center;

			//是否反向
			float inverse;
			//进度(0-1)
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
				float progress = inverse == 0 ? currentT : 1 - currentT;
				//渐变区角度
				float _fadeAngle =  180.0 / 360 * UNITY_PI;
				//旋转轴角度
				float _Angle = progress * (2 * UNITY_PI + _fadeAngle);
				//当前点到中心点与y轴的夹角
				float _uvAngle;
				float x = i.uv.x - _Center.x;
				float y = i.uv.y - _Center.y;
				//分象限操作
				if (x == 0 && y > 0)
					_uvAngle = 0;
				if (x == 0 && y < 0)
					_uvAngle = UNITY_PI;
				if (y == 0 && x > 0)
					_uvAngle = 0.5*UNITY_PI;
				if (y == 0 && x < 0)
					_uvAngle = 1.5*UNITY_PI;
				if (x > 0 && y > 0)
					_uvAngle = atan(x / y);
				if (x > 0 && y < 0)
					_uvAngle = UNITY_PI + atan(x / y);
				if (x < 0 && y < 0)
					_uvAngle = UNITY_PI + atan(x / y);
				if (x < 0 && y > 0)
					_uvAngle = 2 * UNITY_PI + atan(x / y);
				//混合度计算
				float gray;
				if (_uvAngle <= _Angle - _fadeAngle) {
					//渐变区以内的区域 新图
					gray = 1 - inverse;
				}
				else if (_uvAngle >= _Angle) {
					//旋转轴以外的区域 旧图
					gray = 0 + inverse;
				}
				else {
					float tempAngle = _uvAngle - (_Angle - _fadeAngle);
					gray = inverse == 0 ? 1 - tempAngle / _fadeAngle : tempAngle / _fadeAngle;
				}
				//贴图混合
				float4 oldp = tex2D(_MainTex, i.uv);
				float4 newp = tex2D(_NewTex, i.uv);
				float3 outp = oldp.rgb*(1 - gray) + newp.rgb*gray;
				return float4(outp, 1);
			}
			ENDCG
		}
	}

	Fallback off
}
