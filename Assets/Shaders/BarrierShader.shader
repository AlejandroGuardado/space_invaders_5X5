Shader "SpaceInvaders/BarrierShader"
{
    Properties{
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        _Position("Position", Vector) = (0, 0, 0, 0)
        _ShowDistance("Show Distance", Range(0.1,5.0)) = 0.1
        _NearColor("Near Color", Color) = (1,1,1,1)
        _NearColorFactor("Near Color Factor", Range(0.01,4.0)) = 0.01
        _NearAlphaFactor("Near Color Factor", Range(1,4.0)) = 1.0
    }
    SubShader{
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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                float4 worldSpacePosition : TEXCOORD1;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4  _NearColor;
            fixed _NearColorFactor;
            fixed _NearAlphaFactor;

            #ifdef UNITY_INSTANCING_ENABLED

            UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
            UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Position)
            UNITY_DEFINE_INSTANCED_PROP(fixed, _ShowDistance)
            UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

            #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
            #define _Flip  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)
            #define _Position  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _Position)
            #define _ShowDistance  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, _ShowDistance)

            #endif

            #ifndef UNITY_INSTANCING_ENABLED

            fixed4 _RendererColor;
            fixed2 _Flip;
            float4 _Position;
            fixed _ShowDistance;

            #endif

            float GetColorBlendByDistance(float distanceRatio){
                return pow(distanceRatio, _NearColorFactor);
            }

            v2f vert (appdata v){
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = float4(v.vertex.xy * _Flip, v.vertex.z, 1.0);
                o.vertex = UnityObjectToClipPos(o.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldSpacePosition = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target{    
                UNITY_SETUP_INSTANCE_ID(i);
                float distance = length(_Position - i.worldSpacePosition.xyz);
                if(distance > _ShowDistance){
                    discard;
                }
                fixed4 col = tex2D(_MainTex, i.uv) * _RendererColor;

                float distanceRatio = (distance / _ShowDistance);
                float blend = GetColorBlendByDistance(distanceRatio);
                col.rgb = col.rgb * (blend) + _NearColor * (1 - blend);
                col.a *= GetColorBlendByDistance(1 - distanceRatio) * _NearAlphaFactor;
                return col;
            }
            ENDCG
        }
    }
}
