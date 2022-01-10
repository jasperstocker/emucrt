// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ECRT"
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
 
    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off Fog { Mode off }
 
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            #pragma target 3.0
 
            struct v2f 
            {
                float4 pos      : POSITION;
                float2 uv       : TEXCOORD0;
                float4 scr_pos : TEXCOORD1;
            };
 
            uniform sampler2D _MainTex;
 
            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
                o.scr_pos = ComputeScreenPos(o.pos);
                return o;
            }
 
            half4 frag(v2f i): COLOR
            {
                float2 ps = i.scr_pos.xy *_ScreenParams.xy / i.scr_pos.w;
                half4 color = tex2D(_MainTex, i.uv);
                int px = (int)ps.x % 14;
                int py = (int)ps.y % 6;
                
                float4 muls = float4(0, 0, 0, 1);

                //LUT

                //red
                if(px == 0 && py == 0) muls.r = 1;
                if(px == 0 && py == 1) muls.r = 1;
                if(px == 0 && py == 2) muls.r = 1;
                if(px == 0 && py == 3) muls.r = 1;
                if(px == 0 && py == 4) muls.r = 1;
                
                if(px == 1 && py == 0) muls.r = 1;
                if(px == 1 && py == 1) muls.r = 1;
                if(px == 1 && py == 2) muls.r = 1;
                if(px == 1 && py == 3) muls.r = 1;
                if(px == 1 && py == 4) muls.r = 1;

                //green
                if(px == 2 && py == 0) muls.g = 1;
                if(px == 2 && py == 1) muls.g = 1;
                if(px == 2 && py == 2) muls.g = 1;
                if(px == 2 && py == 3) muls.g = 1;
                if(px == 2 && py == 4) muls.g = 1;
                
                if(px == 3 && py == 0) muls.g = 1;
                if(px == 3 && py == 1) muls.g = 1;
                if(px == 3 && py == 2) muls.g = 1;
                if(px == 3 && py == 3) muls.g = 1;
                if(px == 3 && py == 4) muls.g = 1;

                //blue
                if(px == 4 && py == 0) muls.b = 1;
                if(px == 4 && py == 1) muls.b = 1;
                if(px == 4 && py == 2) muls.b = 1;
                if(px == 4 && py == 3) muls.b = 1;
                if(px == 4 && py == 4) muls.b = 1;
                
                if(px == 5 && py == 0) muls.b = 1;
                if(px == 5 && py == 1) muls.b = 1;
                if(px == 5 && py == 2) muls.b = 1;
                if(px == 5 && py == 3) muls.b = 1;
                if(px == 5 && py == 4) muls.b = 1;
                
                //sceond phosphor 
                //red
                if(px == 7 && py == 0) muls.r = 1;
                if(px == 7 && py == 1) muls.r = 1;
                if(px == 7 && py == 5) muls.r = 1;
                if(px == 7 && py == 3) muls.r = 1;
                if(px == 7 && py == 4) muls.r = 1;
                
                if(px == 8 && py == 0) muls.r = 1;
                if(px == 8 && py == 1) muls.r = 1;
                if(px == 8 && py == 5) muls.r = 1;
                if(px == 8 && py == 3) muls.r = 1;
                if(px == 8 && py == 4) muls.r = 1;

                //green
                if(px == 9 && py == 0) muls.g = 1;
                if(px == 9 && py == 1) muls.g = 1;
                if(px == 9 && py == 5) muls.g = 1;
                if(px == 9 && py == 3) muls.g = 1;
                if(px == 9 && py == 4) muls.g = 1;
                
                if(px == 10 && py == 0) muls.g = 1;
                if(px == 10 && py == 1) muls.g = 1;
                if(px == 10 && py == 5) muls.g = 1;
                if(px == 10 && py == 3) muls.g = 1;
                if(px == 10 && py == 4) muls.g = 1;

                //blue
                if(px == 11 && py == 0) muls.b = 1;
                if(px == 11 && py == 1) muls.b = 1;
                if(px == 11 && py == 5) muls.b = 1;
                if(px == 11 && py == 3) muls.b = 1;
                if(px == 11 && py == 4) muls.b = 1;
                
                if(px == 12 && py == 0) muls.b = 1;
                if(px == 12 && py == 1) muls.b = 1;
                if(px == 12 && py == 5) muls.b = 1;
                if(px == 12 && py == 3) muls.b = 1;
                if(px == 12 && py == 4) muls.b = 1;
                
                color = color * muls;

                return color;
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}