Shader "SpaceInvaders/GameEntityShader"{
    Properties{
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _DissolveTex("_Dissolve Texture", 2D) = "white" {}
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
        [Toggle]_Dissolve("Dissolve", Int) = 0
        _DissolveFactor("Dissolve Factor", Range(0.0,1.0)) = 0.0
        _DissolveBorderCutoff("Dissolve Border Cutoff", Range(0.0,1.0)) = 1.0
        _DissolveBorderColor("_Dissolve Border Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float4 _MainTex_ST;
            fixed4 _DissolveBorderColor;
            fixed _DissolveBorderCutoff;

            #ifdef UNITY_INSTANCING_ENABLED

            UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
            UNITY_DEFINE_INSTANCED_PROP(int, _Dissolve)
            UNITY_DEFINE_INSTANCED_PROP(fixed, _DissolveFactor)
            UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

            #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
            #define _Dissolve  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Dissolve)
            #define _DissolveFactor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _DissolveFactor)

            #endif

            #ifndef UNITY_INSTANCING_ENABLED
                fixed4 _RendererColor;
                int _Dissolve;
                fixed _DissolveFactor;
            #endif

            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target{
                fixed4 color = tex2D(_MainTex, i.uv) * _RendererColor;
                if(_Dissolve > 0){
                    fixed dissolveAlpha = tex2D(_DissolveTex, i.uv).r;
                    color.rgb = _DissolveFactor + _DissolveBorderCutoff > dissolveAlpha ? _DissolveBorderColor.rgb : color.rgb;
                    dissolveAlpha = step(_DissolveFactor, dissolveAlpha);
                    color.a = color.a > 0 ? dissolveAlpha : color.a;
                }
                return color;
            }
            ENDCG
        }
    }
}
