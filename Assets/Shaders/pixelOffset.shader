Shader "Unlit/pixelOffset"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _Reverse("Reverse", float) = 0
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
            sampler2D _Noise;
            float4 _MainTex_ST;
            float _Reverse;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 n = pow(tex2D(_Noise, i.uv),1);
                fixed4 col = pow(tex2D(_MainTex, i.uv),.45);
                fixed4 noised = fixed4(
                    sin(col.r*3.1415*2+n.r*_Reverse)*.5+.5,
                    sin(col.g*3.1415*2+n.g*_Reverse)*.5+.5,
                    sin(col.b*3.1415*2+n.b*_Reverse)*.5+.5,
                    1)
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return noised;
            }
            ENDCG
        }
    }
}
