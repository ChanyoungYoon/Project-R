// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/UnlitCutoutCaster"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff ("Cutoff", Range(0,1)) = 0.5
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout"  "Queue" = "Geometry+0"  }
        LOD 200
        Cull off

        CGPROGRAM
        #pragma surface surf CustomDither vertex:vert alphatest:_Cutoff fullforwardshadows
        #pragma target 3.0

        struct CustomSurfaceOutput
        {
            fixed3 Albedo;
            fixed3 Normal;
            half3 Emission;
            fixed Alpha;    
        };

        sampler2D _MainTex;
        float4 _Color;

        half4 LightingCustomDither(CustomSurfaceOutput s,float3 lightDir,float atten){
            fixed4 c;

            c.rgb = s.Albedo;
            c.a = s.Alpha;

            return c;
        }

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
            float3 worldPos;
        };

        void vert (inout appdata_full v,out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.color = v.color;
        }

        void surf (Input IN, inout CustomSurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb*IN.color.rgb;
            o.Alpha = c.a*IN.color.a;
        }

        ENDCG
        Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			sampler2D _MainTex;
            fixed _Cutoff;

            struct v2f{
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata_full v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv=v.texcoord;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }

            float4 frag(v2f IN):COLOR
            {
                fixed4 c = tex2D(_MainTex,IN.uv);
                clip(c.a-_Cutoff);
                SHADOW_CASTER_FRAGMENT(IN);
            }

			ENDCG
		}
        
    }
}