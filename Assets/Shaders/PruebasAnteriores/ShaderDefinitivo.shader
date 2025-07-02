Shader "Unlit/ShaderDefinitivo"
{
 
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Brightness ("Brightness", Range(0,1)) = 0.3
        _Strength ("Strength", Range(0,1)) = 0.5
        _Color ("Color", Color) = (1,1,1,1)
        _Detail ("Detail", Range(0,1)) = 0.3

        _OutlineColor ("Outline Color", Color) = (0, 1, 0, 1)
        _OutlineScale ("Outline Scale", Float) = 1.05
        _OutlineAlpha ("Outline Alpha", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Outline Pass: malla escalada para outline visible desde cualquier ángulo
        Pass
        {
            Name "OutlinePass"
            Tags { "LightMode"="UniversalForward" }

            Cull Off      // Que se vea desde todos lados
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float _OutlineScale;
            float4 _OutlineColor;

            Varyings vertOutline(Attributes IN)
            {
                Varyings OUT;

                // Escalamos la posición en espacio objeto para expandir la malla
                float4 posScaled = IN.positionOS * _OutlineScale;

                OUT.positionHCS = mul(UNITY_MATRIX_MVP, posScaled);

                return OUT;
            }

            half4 fragOutline(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }

        // Toon Pass: el shader toon normal
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Brightness;
            float _Strength;
            float4 _Color;
            float _Detail;

            float Toon(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
                return floor(NdotL / _Detail);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= Toon(i.worldNormal, _WorldSpaceLightPos0.xyz) * _Strength * _Color + _Brightness;
                return col;
            }
            ENDCG
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}