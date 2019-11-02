Shader "Custom/Shutter" {
    Properties {
        _MainTex ("Old Texture", 2D) = "white" {}
        _TexSize ("Texture Size", vector) = (1920, 1080, 0, 0)
    }

    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _NewTex;
            float4 _TexSize;

			//自定义个数
			int _FenceNum = 0;
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
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.texcoord;
                return o;
            }
            
            float4 frag (v2f i) : COLOR
            {
				float fence_rate, _FenceWidth;
				//默认个数
				float _FenceVerNum = 12;
				float _FenceHorNum = 16;
				//左
			if (_Direction == 0) {
				if (_FenceNum == 0) _FenceNum = _FenceHorNum;
				_FenceWidth = _TexSize.x / _FenceNum;
				fence_rate = fmod(i.uv.x * _TexSize.x, _FenceWidth) / _FenceWidth;
			}
				//右
			if (_Direction == 1) {
				if (_FenceNum == 0) _FenceNum = _FenceHorNum;
				_FenceWidth = _TexSize.x / _FenceNum;
				fence_rate = 1 - fmod(i.uv.x * _TexSize.x, _FenceWidth) / _FenceWidth;
			}
				//上
			if (_Direction == 2) {
				if (_FenceNum == 0) _FenceNum = _FenceVerNum;
				_FenceWidth = _TexSize.y / _FenceNum;
				fence_rate = 1 - fmod(i.uv.y * _TexSize.y, _FenceWidth) / _FenceWidth;
			}
				//下
			if (_Direction == 3) {
				if (_FenceNum == 0) _FenceNum = _FenceVerNum;
				_FenceWidth = _TexSize.y / _FenceNum;
				fence_rate = fmod(i.uv.y * _TexSize.y, _FenceWidth) / _FenceWidth;
			}
				//当前进度(0-1)
				float progress = currentT;
                if (progress < fence_rate)
                    return tex2D(_MainTex, i.uv);
                else
                    return tex2D(_NewTex, i.uv);
            }
            ENDCG
        }
    }

    Fallback off
}