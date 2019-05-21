Shader "Unlit/Simple Paricle Unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
        LOD 100
        
        Blend One OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata //vetex input
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float3 uv : TEXCOORD0;
            };

            struct v2f //vertex output
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                // Vertex animation
                float sineFrequency = 5.0;
                float sineAmplitude = 4.0;

                float sineOffset = sin(_Time.y * sineFrequency) * sineAmplitude;
                float agePercent = v.uv.z;
                
                float3 vertexOffset = float3(0, sineOffset * agePercent, 0);

                v.vertex.xyz += vertexOffset;
                

                o.vertex = UnityObjectToClipPos(v.vertex);

                // Initialize outgoing colour with the data recieved from the particle system stored in the colour vertex input.
                o.color = v.color;
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);

                // Initialize outgoing uv.z (which hods particle age percent)

                o.uv.z = v.uv.z;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                 // Multiply texture colour with the particle system's vertex colour input.
                 col *= i.color;

                float particleAgePercent = i.uv.z;
                float4 colorRed = float4(1, 0, 0, 1);

                // Lerp from texture color to red based on particle age percent
                col = lerp(col, colorRed * col.a, particleAgePercent);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
