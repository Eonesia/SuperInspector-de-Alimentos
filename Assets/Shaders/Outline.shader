Shader "Unlit/Outline"
{
    Properties
    {
        _OutlineColor ("Outline Color (Emissive)", Color) = (0, 1, 0, 1)
        _OutlineWidth ("Outline Width", Float) = 0.03
        _EmissionIntensity ("Emission Intensity", Float) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Name "OutlinePass"
            Tags { "LightMode"="UniversalForward" }

            Cull Front
            ZWrite Off

            // Quitamos transparencia, hacemos el material opaco
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float4 _OutlineColor;
            float _OutlineWidth;
            float _EmissionIntensity;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 pos = IN.positionOS.xyz + normalize(IN.normalOS) * _OutlineWidth;
                OUT.positionHCS = TransformObjectToHClip(pos);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 emissiveColor = _OutlineColor * _EmissionIntensity;
                emissiveColor.a = 1.0; // Aseguramos opacidad total
                return emissiveColor;
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}