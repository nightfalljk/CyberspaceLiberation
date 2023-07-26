//Ata   
Shader "Unlit/Monroe_shader"
{
    Properties
	{
        
        _PointSize("Point Size", Range(0,0.1)) = 0.0017
        
        _TesselationFactor("Tesselation Factor", Range(1, 5)) = 3.49


        //copy data
        _copyDistance("Holog Copy 1 Distance", Range(0,5)) = 0.22

        //ANIMATION

        //Center ANIM
        _centerAnimSinPower("Center Anim SINUS POWER", Range(0, 1)) = 0.217
        _centerAnimCosPower("Center Anim COSINUS POWER", Range(0, 1)) = 0.035

        _centerAnimSize("Center Anim SIZE", Range(1.7, 6)) = 1.8
        _centerAnimOffset("Center Anim Second Sinus Offset", Range(5,40)) = 23

        //LEFT SLIDER
        _leftSliderSpeed("Left Slider SPEED", Range(1, 5)) = 2.41


        //RIGHT SLIDER
        _rightSliderSpeed("Right Slider SPEED", Range(1, 5)) = 3

	}
	SubShader
	{

        Cull Off
        Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha  
		Zwrite Off

        Pass
        {
            COLORMASK 0
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

           fixed4 frag(v2f i) : SV_Target
           {
                return (1,1,1,1);
           }

           ENDHLSL
        }
       

		Pass
		{
            Offset 0, -100000       //Pushing Z-Buffer to make the back of the model invisible
			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag
            #pragma geometry geom
            #pragma hull hull
            #pragma domain domain

            #pragma target 4.6

			#include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float3 normal : NORMAL;
			};

			
            struct v2g
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                
            };
            struct g2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float2 id : TEXCOORD0;
            };

            struct tessFactors
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;     
            };

            //----------PROPERTIES----------------

            float4 _MainColorLight, _MainColorDark;
            float _PointSize, _TesselationFactor,
            _copyDistance,
            _centerAnimSinPower, _centerAnimCosPower, _centerAnimSize, _centerAnimOffset,
            _leftSliderSpeed,
            _rightSliderSpeed;

			appdata vert(appdata v) 
			{
				return v;
			}

            //TESSELATION
            tessFactors patch(InputPatch<appdata, 3> ip)
            {
                _TesselationFactor += dot(ip[0].normal, _WorldSpaceLightPos0.xyz);
                tessFactors t;
                t.edge[0] = _TesselationFactor;
                t.edge[1] = _TesselationFactor;
                t.edge[2] = _TesselationFactor;
                t.inside = _TesselationFactor;
                return t;
            }

            [UNITY_domain("tri")]
            [UNITY_outputcontrolpoints(3)]
            [UNITY_outputtopology("point")]
            [UNITY_partitioning("fractional_even")]
            [UNITY_patchconstantfunc("patch")]
            appdata hull(InputPatch<appdata, 3> ip, uint id: SV_OutputControlPointID)
            {
            return ip[id];
            }

            #define INTERPOLATE(fieldName) data.fieldName = op[0].fieldName * dl.x + op[1].fieldName * dl.y  + op[2].fieldName * dl.z ;
                                            

            [UNITY_domain("tri")]
            v2g domain(tessFactors tf, OutputPatch<appdata, 3> op, float3 dl : SV_DomainLocation)
            {
                appdata data;
                INTERPOLATE(vertex);
                INTERPOLATE(normal);

                v2g o;
                o.vertex = UnityObjectToClipPos(data.vertex);
                o.worldNormal = UnityObjectToWorldNormal(data.normal);
                return o;

            }

            //Helper Function
            float remap(float In, float2 InMinMax, float2 OutMinMax)
            {
            return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }




            [maxvertexcount(84)]
            void geom(point v2g p[1], inout TriangleStream<g2f> triStream)
            {
                float pointSizeY = _PointSize * _ScreenParams.x / _ScreenParams.y;      //fixing the holograms unit size considering screen resolution
                    float4 quadVertices[4];

                    g2f newVert;
                    newVert.worldNormal = p[0].worldNormal;
                    float t = _Time.y;
                    float st = _SinTime.z;
                    float ct = _CosTime.z;

                    //Creating vertices of the original position and the 20 copies used for the effect, assigning them indices for frag shader

                    for(int j = 0; j <= 21; j++){
                        newVert.id = float2(j, j);
                        float realIndex = j - 21 / 2;

                        quadVertices[0] = p[0].vertex + float4(_copyDistance * realIndex, 0, 0, 0) + float4(_PointSize, pointSizeY, 0, 0);
                        quadVertices[1] = p[0].vertex + float4(_copyDistance * realIndex, 0, 0, 0) + float4(-_PointSize, pointSizeY, 0, 0);
                        quadVertices[2] = p[0].vertex + float4(_copyDistance * realIndex, 0, 0, 0) + float4(_PointSize, -pointSizeY, 0, 0);
                        quadVertices[3] = p[0].vertex + float4(_copyDistance * realIndex, 0, 0, 0) + float4(-_PointSize, -pointSizeY, 0, 0);

                    
                        newVert.vertex = quadVertices[0];
                        triStream.Append(newVert);

                        newVert.vertex = quadVertices[1];
                        triStream.Append(newVert);

                        newVert.vertex = quadVertices[2];
                        triStream.Append(newVert);

                        newVert.vertex = quadVertices[3];
                        triStream.Append(newVert);

                        triStream.RestartStrip();
                    }

            }

			fixed4 frag(g2f i) : SV_Target
			{

                float t = _Time.y;
                float st = sin(_Time.y / 2);
                float ct = _CosTime.z;

                float alpha = 1;
                float3 rgb = (0,0,0);
                //taking index to [-10,10]
                int realindex = i.id.x - 10;


                //------------CENTER-------------

                //lerp alpha color of the fragment depending on the distance to the sin_time value


                //calculate the position of the original model

                float3 original_model = i.vertex - float4(_copyDistance * realindex, 0, 0, 0);

                float middleProg = (st * 8.0);      //this is 8 where there are +-4 models to animate to extend the length of transparent time

                //Calculate the position of the sinus according to the original model 

                float3 sin_for_origin1 = float3(original_model.x + middleProg * _copyDistance * _centerAnimSize, original_model.y, original_model.z);
                float3 sin_for_origin2 = float3(original_model.x - middleProg * _copyDistance * _centerAnimSize, original_model.y, original_model.z);
                
                float dist_to_sin1 = (sin_for_origin1.x - i.vertex.x) * (sin_for_origin1.x - i.vertex.x) + (sin_for_origin1.z - i.vertex.z) * (sin_for_origin1.z - i.vertex.z);
                float dist_to_sin2 = (sin_for_origin2.x - i.vertex.x) * (sin_for_origin2.x - i.vertex.x) + (sin_for_origin2.z - i.vertex.z) * (sin_for_origin2.z - i.vertex.z);

                //calculate alpha value depending on the distance to sin wave and the offset
                alpha = lerp(0.75,0, dist_to_sin1 - _centerAnimSinPower);
                alpha += lerp(0.55 + (ct* _centerAnimOffset /100),0, dist_to_sin1 - _centerAnimSinPower);       
         
                //---------------------LEFT SLIDE------------------------- (Left 6 copies)
                //calculate offseted time_step
                float slide_prog = sin(_Time.y / (5 - _leftSliderSpeed) + 200);  
                slide_prog = remap(slide_prog, float2(-1, 1), float2(-0.5,1));
                
                 
                slide_prog *= 70;
               
                float dist_to_sin = -1;
               
                    float3 leftest = float3(original_model.x - 10 * _copyDistance, original_model.y, original_model.z);
                    float3 sin_for_leftest = float3(leftest.x + slide_prog * _copyDistance , original_model.y, original_model.z);
            

                    //find positions of most left and right copies(left/right) copy
                    //and then the distance of the fragment to the atMost(l)

                    dist_to_sin = (sin_for_leftest.x - i.vertex.x) * (sin_for_leftest.x - i.vertex.x) + (sin_for_leftest.z - i.vertex.z) * (sin_for_leftest.z - i.vertex.z);

                    //give color to the fragment accorgind to its id
                    rgb = float3(smoothstep(0, 21 * floor(smoothstep(-1, 1, ct)), i.id.x), smoothstep(0, 63, i.id.x),st);


                    //Animate left side of the copies(offseted index)
                    if(realindex < - 4.1){
                        alpha = 0.75 - dist_to_sin;      
                    }

                //-----------------------RIGHT SIDE-----------------------------

                float right_slide_prog = sin(_Time.y / (5 - _rightSliderSpeed) + 49);
                right_slide_prog = remap(right_slide_prog, float2(-1, 1), float2(-0.75,1));

                right_slide_prog *= 50;
                float right_dist = 0;
                    
                    float3 rightest = float3(original_model.x + 10 * _copyDistance, original_model.y, original_model.z);
                    float3 sin_for_rightest = float3(leftest.x - slide_prog * _copyDistance , original_model.y, original_model.z);

                    right_dist = (sin_for_rightest.x - i.vertex.x) * (sin_for_rightest.x - i.vertex.x) + (sin_for_rightest.z - i.vertex.z) * (sin_for_rightest.z - i.vertex.z);

                    //Animate right side of copies
                    if(realindex > 4.1){
                        alpha = 0.75 - right_dist; 
                    }



                //-----------------------REDUCE ALPHA IN CENTER-------------

                float dist_to_origin = (original_model.x - i.vertex.x) * (original_model.x - i.vertex.x) + (original_model.z - i.vertex.z) * (original_model.z - i.vertex.z);
                if(abs(realindex) < 3)
                    alpha = lerp(0, alpha / 1.1, dist_to_origin);


                float intensity = remap(dist_to_origin, float2(0, 10 * _copyDistance), float2(1, 8));
                rgb *= intensity;

                if(i.id.x == 10)
                    return float4(rgb.x * 10, rgb.y * 10, rgb.z * 10,1);

                return float4(rgb.x, rgb.y, rgb.z, saturate(alpha));






			}

			ENDHLSL
		}
	}
}

