//Ata   
Shader "Unlit/old_school_hologram_shader"
{
    Properties
    {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _Hol1speed("Hologram 1 Speed", Range(1, 20)) = 3
        _Hol2speed("Hologram 2 Speed", Range(0.3, 5)) = 1
        _FresPow("Fresnel Power", Range(1,5)) = 1.5
        [HDR]_col("Color", Color) = (1,0,0,0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha       
		Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
                float worldNormal : TEXCOORD3;
                float3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex1, _MainTex2;
            float4 _MainTex1_ST, _MainTex2_ST;
            float _Hol1speed, _Hol2speed, _FresPow;
            fixed4 _col;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex1);
                //viewDir worldNormal are for the Fresnel Effect, and the screenPos is being used to make the alpha stripes move vertically independent from the view angle
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.x  * _Hol1speed;
                float st = _SinTime.z * _Hol1speed / 1.5;      // t/2

                st = smoothstep(-5, 5, st);       

                //TRANSPARENCY SLICES

                //so that alpha lines alwazs move vertically
                i.screenPos /= i.vertex.w;

                //Taking transparency values
                float2 holo1uv = i.screenPos;
                holo1uv *= 1 + st;
                holo1uv += t; 
                fixed4 col1 = tex2D(_MainTex1, holo1uv);


                float t2 = _Time.x * _Hol2speed;
                float2 holo2uv = i.screenPos;
                holo2uv += t2;
                fixed4 col2 = tex2D(_MainTex2, holo2uv);
                
                //Fresnel Effect--give color that gradiates from the center to the extend of the object, depending on the view angle
                float fres = pow((1.0 - saturate(dot(normalize(i.worldNormal), normalize(i.viewDir)))), _FresPow);

                _col *= fres;

                return fixed4(_col.xyz, (col1 + col2).x);

            }
            ENDCG
        }
    }
}
