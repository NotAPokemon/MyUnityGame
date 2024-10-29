Shader "Universal Render Pipeline/Terrain/terrain"
{
    Properties
    {

    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque"
            }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        const static int maxColorCount = 8;
        const static float epsilon = 0.0001;

        int baseColorCount;
        float3 baseColors[maxColorCount];
        float baseWeights[maxColorCount];
        float baseBlends[maxColorCount];

        float Min;
        float Max;

        struct Input
        {
            float3 worldPos;
        };

        float inversLerp(float a, float b, float value){
            return saturate((value-a)/(b-a));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float hPercent = inversLerp(Min, Max, IN.worldPos.y);
            for(int i =0; i<baseColorCount; i++)
            {
                float drawStrenght = saturate(sign(hPercent - baseWeights[i]));
                o.Albedo = o.Albedo * (1-drawStrenght) + baseColors[i] * drawStrenght;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}



