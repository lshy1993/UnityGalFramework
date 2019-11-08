Shader "Hidden/TextShadow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OffsetX ("Offset X", range(-0.2,0.2)) = 0 
		_OffsetY ("Offset Y", range(-0.2,0.2)) = 0 
		_ShadowColor ("Shadow Color", COLOR) = (0,0,0,1)
		_ThresholdAlpha("Threshold Alpha", Range(0,0.5)) = 0.1
	}
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
 
		Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
 
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
			};
 
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
 
			sampler2D _MainTex;
			float4 _MainTex_ST;
 
			half _OffsetX;
			half _OffsetY;
			fixed4 _ShadowColor;
			half _ThresholdAlpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				fixed shadowAlpha = tex2D(_MainTex, i.uv + half2(_OffsetX,_OffsetY)).a;
 
				fixed4 shadowColor = fixed4(_ShadowColor.rgb, _ShadowColor.a * shadowAlpha);
 
				fixed4 finalColor = shadowColor;
 
				fixed stepVal = step(_ThresholdAlpha, col.a);
				finalColor = stepVal * col + (1 - stepVal) * shadowColor;
 
				return finalColor;
			}
			ENDCG
		}
	}
}