Shader "Custom/CircleHole" {
    Properties {
        _MainTex ("Old Texture", 2D) = "white" {}
		_TexSize ("Texture Size", vector) = (1920, 1080, 0, 0)
		_Center ("Center", vector) = (0.5, 0.5, 0, 0)
    }
    SubShader {
        LOD 200

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        Pass
        {
			ZTest Always Cull Off ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
            Fog { Mode Off }
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag   
            #include "UnityCG.cginc"            

            sampler2D _MainTex;
			sampler2D _NewTex;
			float2 _Center;

			//是否反向
			float inverse;

			float currentT;
            
            struct v2f 
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.texcoord;
                return o;
            }
            
			half4 frag(v2f i) : COLOR
			{
				//圆的半径随之增大
				float progress = inverse == 0 ? currentT : 1 - currentT;

				bool inCircle = distance(i.uv, _Center) < progress;
				if (inCircle ^ (inverse == 0))
					return tex2D(_MainTex, i.uv);
				else
					return tex2D(_NewTex, i.uv);
            }
            ENDCG
        }
    }
    FallBack off
}
