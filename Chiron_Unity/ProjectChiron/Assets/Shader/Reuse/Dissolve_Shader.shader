Shader "Custom/DissolveShader"
{
	//Mehmet
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_Range("Range", Range(-0.1, 1)) = 0
		[HDR]_Color("Color", Color) = (1, 0, 0, 0)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}
		LOD 100
		Cull Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha         //

		CGPROGRAM

		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		sampler2D _DissolveTexture;
		half _Range;
		fixed4 _Color;

		//Make the main texture appear according to the Rangevalue 
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			_Range = 1 - _Range; 
			
			//Value based upon the uv coordinate of the MainTEx and DissolveTex
			half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).b;
			//Clip according to _Range value
			clip(dissolve_value - _Range);

			//Standard surface 
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}