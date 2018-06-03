Shader "Custom/Geometry Particle" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ParticleSize ("Particle Size", float) = 0.1
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
                float3 color : TEXCOORD1;
            };

            struct Particle {
                float3 position;
                float3 velocity;
                float3 color;
            };

            StructuredBuffer<Particle> particleBuffer;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ParticleSize;
            
            v2f vert (appdata v, uint instance_id : SV_InstanceID) {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MV, float4(particleBuffer[instance_id].position, 1.0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = particleBuffer[instance_id].color;
                return o;
            }

            [maxvertexcount(4)]
            void geom (point v2f input[1], inout TriangleStream<v2f> outputStream) {
                v2f newVertex;
                newVertex.color = input[0].color;
                float2 newxy;

                newxy = input[0].vertex.xy + float2 (-0.5, -0.5) * _ParticleSize;
                newVertex.vertex = mul(UNITY_MATRIX_P, float4(newxy.x, newxy.y, input[0].vertex.z, input[0].vertex.w));
                newVertex.uv = float2 (0.0, 0.0);
                outputStream.Append(newVertex);

                newxy = input[0].vertex.xy + float2 (-0.5, 0.5) * _ParticleSize;
                newVertex.vertex = mul(UNITY_MATRIX_P, float4(newxy.x, newxy.y, input[0].vertex.z, input[0].vertex.w));
                newVertex.uv = float2 (0.0, 1.0);
                outputStream.Append(newVertex);

                newxy = input[0].vertex.xy + float2 (0.5, -0.5) * _ParticleSize;
                newVertex.vertex = mul(UNITY_MATRIX_P, float4(newxy.x, newxy.y, input[0].vertex.z, input[0].vertex.w));
                newVertex.uv = float2 (1.0, 0.0);
                outputStream.Append(newVertex);

                newxy = input[0].vertex.xy + float2 (0.5, 0.5) * _ParticleSize;
                newVertex.vertex = mul(UNITY_MATRIX_P, float4(newxy.x, newxy.y, input[0].vertex.z, input[0].vertex.w));
                newVertex.uv = float2 (1.0, 1.0);
                outputStream.Append(newVertex);
            }
            
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * float4(i.color, 1.0);
                return col;
            }
            ENDCG
        }
    }
}
