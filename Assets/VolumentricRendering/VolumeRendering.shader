Shader "Unlit/VolumeRendering"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float3 _Centre;
            float _Radius;
            
            fixed4 frag (v2f i) : SV_Target
            {
                float3 worldPosition = TEXCOORD1; // World position
                float3 viewDirection = normalize(i.worldPosition - _WorldSpaceCameraPos);
                if ( raycastHit(worldPosition, viewDirection) ) {
                    return fixed4(1,0,0,1); // Red if hit the ball
                }
                else {
                    return fixed4(1,1,1,1); // White otherwise
                }
            }
            ENDCG
        }
    }
}
