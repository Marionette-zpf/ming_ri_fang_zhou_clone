
Shader "Custom/AttackArea" {
    Properties {
        _BaseMap ("Base Map", 2D) = "white" {}
        _TillingStrength ("Tilling Strength", Float) = 1.0
        _Speed ("Speed", Float) = 1.0
    }
    SubShader {
        Tags {"RenderPipeline"="UniversalRenderPipeline" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float4 _BaseMap_ST;
        float _TillingStrength;
        float _Speed;
        CBUFFER_END
        ENDHLSL

        Pass {
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct a2v {
                float4 positionOS   : POSITION;
            };

            struct v2f {
                float4 positionCS   : SV_POSITION;
                float4 positionWS   : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            v2f vert(a2v v) {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.positionWS = mul(GetObjectToWorldMatrix(), v.positionOS);
                return o;
            }

            half4 frag(v2f i) : SV_Target {

                float2 uv = (i.positionWS.x + i.positionWS.z) * _TillingStrength;
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv - _Time.y * _Speed);
                baseMap.a = baseMap.r;
                return baseMap;
            }
            ENDHLSL
        }
    }
}

