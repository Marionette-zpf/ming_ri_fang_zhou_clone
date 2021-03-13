
Shader "Custom/SimpleToon" {
    Properties {
        _BaseMap ("BaseMap", 2D) = "white" {}
        _BumpMap ("NormalMap", 2D) = "white" {}

        _SpecularTex ("SpecularTex", 2D) = "white" {}
        _RampTex ("RampTex", 2D) = "white" {}

        _OutlineWidth("OutlineWidth", Float) = 0.1

        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _CoolColor ("CoolColor", Color) = (0.0, 0.0, 1.0, 1.0)
        _WarmColor ("WarmColor", Color) = (1.0, 0.0, 0.0, 1.0)
        _FresnelColor ("FresnelColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _OutlineColor ("OutlineColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _SpecularColor ("SpecularColor", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _Color;
            float4 _CoolColor;
            float4 _WarmColor;
            float4 _FresnelColor;
            float4 _SpecularColor;
            float4 _OutlineColor;
            float _OutlineWidth;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 posWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDirWS : TEXCOORD3;
            };

            // TEXTURE2D(_BaseMap);
            // SAMPLER(sampler_BaseMap);

            TEXTURE2D(_SpecularTex);
            SAMPLER(sampler_SpecularTex);

            TEXTURE2D(_RampTex);
            SAMPLER(sampler_RampTex);

            // TEXTURE2D(_BumpMap);
            // SAMPLER(sampler_BumpMap);
  

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                o.posWS = mul(unity_ObjectToWorld, v.vertex);
                o.normalWS = TransformObjectToWorldNormal (v.normal);
                o.viewDirWS = _WorldSpaceCameraPos.xyz - o.posWS;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {   
                float3 normalWS = SampleNormal(i.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap)).xyz;
                float3 lightDirWS = normalize(_MainLightPosition);
                float3 viewDirWS = normalize(i.viewDirWS);
                float4 VDN = pow(1 - dot(viewDirWS, normalWS), 6) * _FresnelColor;
                float3 halfLV = normalize(viewDirWS + lightDirWS);

                float halfLambert = dot(normalWS, lightDirWS) * 0.5 + 0.5;
                float specular = pow(dot(halfLV, normalWS), 200);

                float rampSpecular = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, float2(specular, 0));
                float rampShadow = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, float2(halfLambert, 0));

                float4 cwColor = lerp(_CoolColor, _WarmColor, rampShadow);
                
                float4 rimColor = VDN * rampShadow;
                float4 specularCol = specular * _SpecularColor * pow(SAMPLE_TEXTURE2D(_SpecularTex, sampler_SpecularTex, i.uv).a, 4) ;

                float4 col =  SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * smoothstep(-0.2, 0.5, rampShadow);
                return float4(normalWS.xyz, 1);
            }
            ENDHLSL
        }
    }
}

