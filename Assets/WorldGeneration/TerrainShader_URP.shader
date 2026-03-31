Shader "Custom/TerrainShader_URP"
{
    Properties
    {
        _TerrainGradient ("Terrain Gradient", 2D) = "white" {}
        _MinTerrainHeight ("Min Height", Float) = 0
        _MaxTerrainHeight ("Max Height", Float) = 10
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_TerrainGradient);
            SAMPLER(sampler_TerrainGradient);

            float _MinTerrainHeight;
            float _MaxTerrainHeight;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.worldPos = TransformObjectToWorld(v.positionOS.xyz);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float heightValue = saturate(
                    (i.worldPos.y - _MinTerrainHeight) /
                    max(0.0001, (_MaxTerrainHeight - _MinTerrainHeight))
                );

                float3 col = SAMPLE_TEXTURE2D(_TerrainGradient, sampler_TerrainGradient, float2(0, heightValue)).rgb;

                return half4(col, 1);
            }
            ENDHLSL
        }
    }
}