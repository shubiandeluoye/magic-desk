Shader "Custom/FrostedGlass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Blur ("Blur", Range(0, 1)) = 0.5
        _Transparency ("Transparency", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        GrabPass { "_BackgroundTexture" }

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 grabPos : TEXCOORD1;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BackgroundTexture;
            float _Blur;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 mainColor = tex2D(_MainTex, i.uv);
                float4 backgroundColor = tex2Dproj(_BackgroundTexture, i.grabPos);
                
                // Apply blur effect
                float2 direction = float2(1, 1);
                float4 blur = 0;
                for(int j = -4; j <= 4; j++)
                {
                    for(int k = -4; k <= 4; k++)
                    {
                        float2 offset = direction * float2(j, k) * _Blur * 0.01;
                        blur += tex2Dproj(_BackgroundTexture, i.grabPos + float4(offset, 0, 0));
                    }
                }
                blur /= 81;
                
                // Blend with original color
                float4 finalColor = lerp(backgroundColor, blur, _Blur);
                finalColor = lerp(finalColor, mainColor, mainColor.a * (1 - _Transparency));
                
                return finalColor;
            }
            ENDCG
        }
    }
}
