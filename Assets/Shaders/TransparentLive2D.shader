// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TransparentLive2D"
{
	Properties{
		_MainTex("Particle Texture", 2D) = "white" {}
	}
	
	Category{
	
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZTest Always Cull Off ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Fog{ Mode off }

		BindChannels{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		SubShader{
			Pass{
				SetTexture[_MainTex]{
					combine texture * primary
				}
			}
		}
	}
}
