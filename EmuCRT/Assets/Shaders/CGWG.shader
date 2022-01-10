Shader "Unlit/CGWG"
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
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

/*
struct tex_coords
{
   float4 c01_11; 
   float4 c21_31;
   float4 c02_12;
   float4 c22_32;
   float3 mod_factor_ratio_scale;
};

struct input
{
   float2 video_size;
   float2 texture_size;
   float2 output_size;
};

void main_vertex
(
   float4 position : POSITION,
   out float4 oPosition : POSITION,
   uniform float4x4 modelViewProj,

   float4 color : COLOR,
   out float4 oColor : COLOR,

   float2 tex : TEXCOORD,

   uniform input IN,
   out tex_coords coords
)
{
   oPosition = mul(modelViewProj, position);
   oColor = color;

   float2 delta = 1.0 / IN.texture_size;
   float dx = delta.x;
   float dy = delta.y;

   coords = tex_coords (
      float4(tex + float2(-dx, 0.0), tex + float2(0.0, 0.0)),
      float4(tex + float2(dx, 0.0), tex + float2(2.0 * dx, 0.0)),
      float4(tex + float2(-dx, dy), tex + float2(0.0, dy)),
      float4(tex + float2(dx, dy), tex + float2(2.0 * dx, dy)),
      float3(tex.x * IN.output_size.x * IN.texture_size.x / IN.video_size.x, tex * IN.texture_size)
   );
}

#define TEX2D(c) tex2D(s0 ,(c))
#define PI 3.141592653589
#define gamma 2.7

float4 main_fragment(in tex_coords co, uniform input IN, uniform sampler2D s0 : TEXUNIT0) : COLOR
{
   float2 uv_ratio = frac(co.mod_factor_ratio_scale.yz);
   float3 col, col2;

   float4x3 texes0 = float4x3(TEX2D(co.c01_11.xy).xyz, TEX2D(co.c01_11.zw).xyz, TEX2D(co.c21_31.xy).xyz, TEX2D(co.c21_31.zw).xyz);
   float4x3 texes1 = float4x3(TEX2D(co.c02_12.xy).xyz, TEX2D(co.c02_12.zw).xyz, TEX2D(co.c22_32.xy).xyz, TEX2D(co.c22_32.zw).xyz);

   float4 coeffs = float4(1.0 + uv_ratio.x, uv_ratio.x, 1.0 - uv_ratio.x, 2.0 - uv_ratio.x) + 0.005;
   coeffs = sin(PI * coeffs) * sin(0.5 * PI * coeffs) / (coeffs * coeffs);
   coeffs = coeffs / dot(coeffs, float(1.0));

   float3 weights = float3(3.33 * uv_ratio.y);
   float3 weights2 = float3(uv_ratio.y * -3.33 + 3.33);

   col = saturate(mul(coeffs, texes0));
   col2 = saturate(mul(coeffs, texes1));

   float3 wid = 2.0 * pow(col, float3(4.0)) + 2.0;
   float3 wid2 = 2.0 * pow(col2, float3(4.0)) + 2.0;

   col = pow(col, float3(gamma));
   col2 = pow(col2, float3(gamma));

   float3 sqrt1 = rsqrt(0.5 * wid);
   float3 sqrt2 = rsqrt(0.5 * wid2);

   float3 pow_mul1 = weights * sqrt1;
   float3 pow_mul2 = weights2 * sqrt2;

   float3 div1 = 0.1320 * wid + 0.392;
   float3 div2 = 0.1320 * wid2 + 0.392;

   float3 pow1 = -pow(pow_mul1, wid);
   float3 pow2 = -pow(pow_mul2, wid2);

   weights = exp(pow1) / div1;
   weights2 = exp(pow2) / div2;

   float3 multi = col * weights + col2 * weights2;
   float3 mcol = lerp(float3(1.0, 0.7, 1.0), float3(0.7, 1.0, 0.7), floor(fmod(co.mod_factor_ratio_scale.x, 2.0)));

   return float4(pow(mcol * multi, float3(0.454545)), 1.0);
}*/