Shader "Unlit/rample"
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

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // apply fog
                                float len = length(i.uv-.5);

                float sd = 0;
                float d = 0;
                float count = 6;
                for(int j = 0 ; j < count ; j++){
                    float jj = j/count;
                    float iout = sin(_Time.x*4+j*6.28)*.25+.5;
                    float l = length(i.uv-float2(.5+sin(jj*6.28)*iout,.5+cos(jj*6.28)*iout)  );
                    d += sin(l*sin(_Time.x)*70+_Time.x+sin(l*(sin(_Time.x*5+len*10)+2)*25+_Time.z)*sin(_Time.y));
                }
                d/=count;
                d*=sin(len*100+_Time.y);
                d+=1;
                d*=.5;
                sd = d;//sin(d*10+_Time.y);
                float rad = (atan2(i.uv.y-.5,i.uv.x-.5)+3.1415)*(1/6.28);
                fixed4 col = tex2D(_MainTex,  float2(sd*.1+_Time.x+len,_Time.x));

                UNITY_APPLY_FOG(i.fogCoord, col);
                return (1-len*2)*2*((1-len*2)*((sin(_Time.y*6.28*4)+1)*.5)*(1-sd)*(1-col)+col*sd);
            }
            ENDCG
        }
    }
}
