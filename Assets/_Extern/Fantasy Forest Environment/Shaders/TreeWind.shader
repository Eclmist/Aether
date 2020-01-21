// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fantasy Forest/TreeWind"
{
	Properties
	{
		[Enum(Off,0,Front,1,Back,2)]_CullMode("Cull Mode", Int) = 2
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_LeavesWindSpeed("Leaves Wind Speed", Range( 0 , 1)) = 0.3
		_LeavesWindStrength("Leaves Wind Strength", Range( 0 , 0.1)) = 0.01
		_WindSpeed("Wind Speed", Range( 0 , 1)) = 1
		_WindStrength("Wind Strength", Range( 0 , 1)) = 1
		_WindMultiplier("Wind Multiplier", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "DisableBatching" = "True" }
		Cull [_CullMode]
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma instancing_options procedural:setup forwardadd
		#pragma multi_compile GPU_FRUSTRUM_ON __
		#include "VS_indirect.cginc"
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows dithercrossfade vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform int _CullMode;
		uniform float _LeavesWindSpeed;
		uniform float _LeavesWindStrength;
		uniform half _WindMultiplier;
		uniform half _WindSpeed;
		uniform float _WindStrength;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _Color;
		uniform half _Smoothness;
		uniform float _Cutoff = 0.5;


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult56 = (float2(0.0 , ( v.texcoord.xy.y * sin( ( ( ase_vertex3Pos + ( ( ase_worldPos * 0.5 ) + ( _Time.y * _LeavesWindSpeed ) ) ) / 0.1 ) ) ).x));
			float temp_output_81_0 = ( ( ( ase_worldPos.x + ase_worldPos.z ) * 0.01 ) + ( _Time.y * _WindSpeed ) );
			float temp_output_97_0 = ( ( ase_vertex3Pos.y * ( ( ( sin( ( temp_output_81_0 * 4.0 ) ) + sin( ( temp_output_81_0 * 15.0 ) ) ) - cos( ( temp_output_81_0 * 5.0 ) ) ) * 0.1 ) ) * _WindStrength );
			float4 appendResult99 = (float4(temp_output_97_0 , 0.0 , temp_output_97_0 , 0.0));
			float4 break101 = mul( appendResult99, unity_ObjectToWorld );
			float3 appendResult102 = (float3(break101.x , 0.0 , break101.z));
			float3 rotatedValue103 = RotateAroundAxis( float3( 0,0,0 ), appendResult102, float3( 0,0,0 ), 0.0 );
			v.vertex.xyz += ( float3( ( ( v.color.g * 1.0 ) * ( appendResult56 * _LeavesWindStrength ) ) ,  0.0 ) + ( ( v.color.r * _WindMultiplier ) * rotatedValue103 ) );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode2 = tex2D( _MainTex, uv_MainTex );
			o.Albedo = ( tex2DNode2 * _Color ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			clip( tex2DNode2.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
735;88;1666;974;5671.781;785.6255;4.687432;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;73;-3868.643,1196.448;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;75;-3796.857,1367.733;Float;False;Constant;_Float6;Float 6;5;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-3562.635,1229.799;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-3716.591,1559.535;Half;False;Property;_WindSpeed;Wind Speed;7;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;76;-3601.954,1442.189;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-3388.541,1540.201;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-3413.731,1265.438;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-3088.242,1340.26;Float;False;Constant;_Float11;Float 11;5;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-3086.805,1455.058;Float;False;Constant;_Float12;Float 12;5;0;Create;True;0;0;False;0;15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-3259.619,1417.282;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-3084.702,1564.576;Float;False;Constant;_Float13;Float 13;5;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2904.584,1254.857;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-2902.947,1369.855;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-2900.844,1490.283;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;86;-2697.087,1370.152;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;88;-2695.733,1253.659;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;90;-2695.311,1491.031;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-2515.292,1294.212;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;92;-2373.373,1464.441;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;58;-2790.621,602.2313;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-2419.284,1592.663;Float;False;Constant;_Float14;Float 14;5;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;120;-2890.852,319.4381;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;123;-2850.796,477.9383;Float;False;Constant;_Float3;Float 3;9;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-2863.766,704.1653;Float;False;Property;_LeavesWindSpeed;Leaves Wind Speed;5;0;Create;True;0;0;False;0;0.3;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2191.811,1482.433;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-2603.604,378.7971;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-2561.796,639.9669;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;94;-2352.863,1145.418;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;124;-2384.292,616.8308;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;57;-2197.952,478.9521;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-1987.634,1381.538;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1997.563,1522.912;Float;False;Property;_WindStrength;Wind Strength;8;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-1818.635,1380.538;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;61;-1889.319,565.314;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1888.128,756.8304;Float;False;Constant;_LeavesWindFrequency;Leaves Wind Frequency;7;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldMatrixNode;98;-1713.635,1487.538;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.DynamicAppendNode;99;-1663.635,1333.538;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;62;-1612.669,579.5077;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;65;-1446.189,578.0557;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;66;-1504.231,419.4901;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-1477.635,1373.538;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4x4;1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;101;-1322.635,1374.538;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1208.338,510.0985;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;106;-1020.044,251.6939;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;70;-1143.351,638.9152;Float;False;Property;_LeavesWindStrength;Leaves Wind Strength;6;0;Create;True;0;0;False;0;0.01;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;117;-792.4565,1276.197;Half;False;Property;_WindMultiplier;Wind Multiplier;9;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;104;-792.4565,1100.197;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;56;-1021.459,510.0992;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;102;-1003.633,1350.538;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-1019.936,421.1339;Float;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-848.7333,520.0093;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;118;-817.936,350.1339;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;103;-776.4565,1388.197;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;-584.4565,1212.197;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-625.3631,419.7438;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-424.4565,1276.197;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;2;-846.5266,-319.9578;Inherit;True;Property;_MainTex;Main Texture;1;0;Create;False;0;0;False;0;-1;None;4c1eb07828ad76c4c88129337f5736cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;72;-842.6423,-106.9163;Float;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-288.5409,-163.9111;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-229.6082,502.6961;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-210.1434,32.49841;Float;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-512.362,198.5323;Half;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;125;-845.97,-491.8615;Float;False;Property;_CullMode;Cull Mode;0;1;[Enum];Create;True;3;Off;0;Front;1;Back;2;0;True;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;ASEMaterialInspector;0;0;StandardSpecular;Fantasy Forest/TreeWind;False;False;False;False;False;False;False;False;False;False;False;False;True;True;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;False;0;0;True;125;-1;0;False;-1;3;Pragma;instancing_options procedural:setup forwardadd;False;;Custom;Pragma;multi_compile GPU_FRUSTRUM_ON __;False;;Custom;Custom;#include "VS_indirect.cginc";True;34c8aabd5780e1c40a73002e125108a2;Custom;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;74;0;73;1
WireConnection;74;1;73;3
WireConnection;79;0;76;0
WireConnection;79;1;77;0
WireConnection;78;0;74;0
WireConnection;78;1;75;0
WireConnection;81;0;78;0
WireConnection;81;1;79;0
WireConnection;84;0;81;0
WireConnection;84;1;80;0
WireConnection;85;0;81;0
WireConnection;85;1;82;0
WireConnection;87;0;81;0
WireConnection;87;1;83;0
WireConnection;86;0;85;0
WireConnection;88;0;84;0
WireConnection;90;0;87;0
WireConnection;89;0;88;0
WireConnection;89;1;86;0
WireConnection;92;0;89;0
WireConnection;92;1;90;0
WireConnection;93;0;92;0
WireConnection;93;1;91;0
WireConnection;122;0;120;0
WireConnection;122;1;123;0
WireConnection;60;0;58;0
WireConnection;60;1;59;0
WireConnection;124;0;122;0
WireConnection;124;1;60;0
WireConnection;95;0;94;2
WireConnection;95;1;93;0
WireConnection;97;0;95;0
WireConnection;97;1;96;0
WireConnection;61;0;57;0
WireConnection;61;1;124;0
WireConnection;99;0;97;0
WireConnection;99;2;97;0
WireConnection;62;0;61;0
WireConnection;62;1;63;0
WireConnection;65;0;62;0
WireConnection;100;0;99;0
WireConnection;100;1;98;0
WireConnection;101;0;100;0
WireConnection;67;0;66;2
WireConnection;67;1;65;0
WireConnection;56;1;67;0
WireConnection;102;0;101;0
WireConnection;102;2;101;2
WireConnection;69;0;56;0
WireConnection;69;1;70;0
WireConnection;118;0;106;2
WireConnection;118;1;119;0
WireConnection;103;3;102;0
WireConnection;116;0;104;1
WireConnection;116;1;117;0
WireConnection;107;0;118;0
WireConnection;107;1;69;0
WireConnection;105;0;116;0
WireConnection;105;1;103;0
WireConnection;71;0;2;0
WireConnection;71;1;72;0
WireConnection;109;0;107;0
WireConnection;109;1;105;0
WireConnection;0;0;71;0
WireConnection;0;4;114;0
WireConnection;0;10;2;4
WireConnection;0;11;109;0
ASEEND*/
//CHKSM=E46B3BAB08F326BB2F464583603844A8562ACA4F