Shader "Custom/Satellite Billboard Sprite" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Opaque"}
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) + float4(v.vertex.x, v.vertex.y, 0.0, 0.0);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            [maxvertexcount(15)]
            void geom (triangle v2f input[3], inout TriangleStream<v2f> outputStream) {
                v2f newVertex;
                int i;
                for(i = 0; i < 3; i++) {
                    newVertex.vertex = mul(UNITY_MATRIX_P, input[i].vertex);
                    newVertex.uv = input[i].uv;
                    outputStream.Append(newVertex);
                }
                outputStream.RestartStrip();

                for(i = 0; i < 3; i++) {
                    newVertex.vertex = mul(UNITY_MATRIX_P, input[i].vertex + mul(UNITY_MATRIX_MV, float4(2.0, 0.0, 0.0, 0.0)));
                    newVertex.uv = input[i].uv;
                    outputStream.Append(newVertex);
                }
                outputStream.RestartStrip();

                for(i = 0; i < 3; i++) {
                    newVertex.vertex = mul(UNITY_MATRIX_P, input[i].vertex + mul(UNITY_MATRIX_MV, float4(-2.0, 0.0, 0.0, 0.0)));
                    newVertex.uv = input[i].uv;
                    outputStream.Append(newVertex);
                }
                outputStream.RestartStrip();

                for(i = 0; i < 3; i++) {
                    newVertex.vertex = mul(UNITY_MATRIX_P, input[i].vertex + mul(UNITY_MATRIX_MV, float4(0.0, 2.0, 0.0, 0.0)));
                    newVertex.uv = input[i].uv;
                    outputStream.Append(newVertex);
                }
                outputStream.RestartStrip();

                for(i = 0; i < 3; i++) {
                    newVertex.vertex = mul(UNITY_MATRIX_P, input[i].vertex + mul(UNITY_MATRIX_MV, float4(0.0, -2.0, 0.0, 0.0)));
                    newVertex.uv = input[i].uv;
                    outputStream.Append(newVertex);
                }
                outputStream.RestartStrip();
            }
            
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
