// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ScreenBlend"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlendTex ("Base (RGB)", 2D) = "white" {}
		_Blend("Blend Amount", Float) = 1
		_Brighten("Brightness Multiplier", Float) = 1.5
        _BlackPoint("Black Point", Float) = 0
        _WhitePoint("White Point", Float) = 1
    }

    SubShader
    {
//        BlendOp Add
//        Blend OneMinusDstColor One, One Zero // screen
        //Blend SrcAlpha One, One Zero // linear dodge
        ZTest Always Cull Off ZWrite Off Fog
        {
            Mode off
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _BlendTex;
            float4 _MainTex_ST;
            float4 _BlendTex_ST;
            float _Blend;
            float _Brighten;
            float _BlackPoint;
            float _WhitePoint;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                float4 base = tex2D(_MainTex, i.texcoord);
                float4 blend = 1 - (1 - tex2D(_BlendTex, i.texcoord) * _Brighten ) * (1 - tex2D(_MainTex, i.texcoord));
                float4 use = lerp(base, blend, _Blend);
                use = (use - _BlackPoint) * (1 / (_WhitePoint - _BlackPoint));
                return use;
            }
            ENDCG
        }
    }
}