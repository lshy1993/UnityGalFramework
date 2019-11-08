Shader "Custom/BlurGlass"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Blur("Blur", Range(0,50)) = 0
		_Color("Color",Color) = (1.0,1.0,1.0,1.0)
		_Alpha("Alpha", Range(0,1)) = 0.3
		_ColorOn("On", Range(0,1)) = 0
    }
    SubShader
    {

        Tags{ "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		GrabPass
        {   
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 vertColor : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.vertColor = v.color;
                return o;
            }

            sampler2D _GrabTexture;
            fixed4 _GrabTexture_TexelSize;
			fixed4 _Color;
			float _Alpha;
			sampler2D _MainTex;
			float _ColorOn;

            half4 frag(v2f i) : SV_Target
            {
				
				//遮罩alpha
				fixed4 colm = tex2D(_MainTex, float2(i.grabPos.x,1-i.grabPos.y));
				//原色
				fixed4 col = tex2Dproj(_GrabTexture, i.grabPos);
				
				//正片叠低 = col*_Color*;
				//线性减弱 = min(1.0,_Color+col);

				//叠加后颜色
				fixed4 ncol = col*_Color*_ColorOn + min(1.0,_Color+col)*(1-_ColorOn);

				// 按照蒙版混合
				ncol.a = (1-colm)*_Alpha;
				return ncol;
				//return ncol*(1-colm)*_Alpha;
            }
            ENDCG
        }

        GrabPass
        {   
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 vertColor : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.vertColor = v.color;
                return o;
            }

            sampler2D _GrabTexture;
            fixed4 _GrabTexture_TexelSize;

            float _Blur;
			sampler2D _MainTex;

            half4 frag(v2f i) : SV_Target
            {
                float blur = _Blur;
                blur = max(1, blur);

                fixed4 col = (0, 0, 0, 0);
                float weight_total = 0;

				fixed4 colm = tex2D(_MainTex, float2(i.grabPos.x,1-i.grabPos.y));
				half4 raw = tex2Dproj(_GrabTexture, i.grabPos);

                [loop]
                for (float x = -blur; x <= blur; x += 1)
                {
					
                    float distance_normalized = abs(x / blur);
                    float weight = exp(-0.5 * pow(distance_normalized, 2) * 5.0);
                    weight_total += weight;
                    col += tex2Dproj(_GrabTexture, i.grabPos + float4(x * _GrabTexture_TexelSize.x, 0, 0, 0)) * weight;
                }

                col /= weight_total;
				return lerp(raw, col, 1-colm);
                //return col*colm.a;
            }
            ENDCG
        }
        GrabPass
        {   
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 vertColor : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                o.vertColor = v.color;
                return o;
            }

            sampler2D _GrabTexture;
            fixed4 _GrabTexture_TexelSize;

			sampler2D _MainTex;
            float _Blur;

            half4 frag(v2f i) : SV_Target
            {
                float blur = _Blur;
                blur = max(1, blur);

                fixed4 col = (0, 0, 0, 0);
                float weight_total = 0;

				fixed4 colm = tex2D(_MainTex, float2(i.grabPos.x,1-i.grabPos.y));
				half4 raw = tex2Dproj(_GrabTexture, i.grabPos);

                [loop]
                for (float y = -blur; y <= blur; y += 1)
                {
                    float distance_normalized = abs(y / blur);
                    float weight = exp(-0.5 * pow(distance_normalized, 2) * 5.0);
                    weight_total += weight;
                    col += tex2Dproj(_GrabTexture, i.grabPos + float4(0, y * _GrabTexture_TexelSize.y, 0, 0)) * weight;
                }

                col /= weight_total;
				return lerp(raw, col, 1-colm);
                //return col*colm.a;
            }
            ENDCG
        }

    }
}