Shader "SpaceInvaders/FadeTransition"{
    Properties{
        _Color ("Color", Color) = (0,0,0,1)
        _Fill ("Fill", Range(0.0, 1.0)) = 0.0
    }

    SubShader{
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend Zero OneMinusSrcAlpha
        LOD 100

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed _Fill;
            half4 _Color;

            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target{
                //Simple alpha cutoff
                fixed4 col = half4(_Color.r, _Color.g, _Color.b, _Fill);
                return col;
            }
            ENDCG
        }
    }
}
