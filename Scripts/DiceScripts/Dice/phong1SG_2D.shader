Shader "Custom/SpriteShader" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Sprite Texture", 2D) = "white" {}
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float4 _Color;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Vertex Shader 함수
            v2f vert(appdata v) {
                v2f o;
                // Sprite의 UV 좌표를 전달
                o.uv = v.uv;
                // Sprite의 정점 좌표를 전달
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // Fragment Shader 함수
            // Sprite의 픽셀 색상 값을 계산
            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color*0.8; //너무 흰색 ->0.8로 명도 조절
            // Sprite의 Alpha값을 기준으로 불투명도를 계산
            col.a = tex2D(_MainTex, i.uv).a;
            return col;
        }
        ENDCG
    }
    }
        FallBack "Diffuse"
}
