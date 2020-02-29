Shader "HDRP/Open World Nature/Tree Leaves"
{
    Properties
    {
        _AlphaClip("Alpha Clip", Range(0, 1)) = 0.25
        _Hue("Hue", Range(-0.5, 0.5)) = 0
        _Saturation("Saturation", Range(-1, 1)) = 0
        _Lightness("Lightness", Range(-1, 1)) = 0
        [NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
        [NoScaleOffset]_BumpMap("BumpMap", 2D) = "bump" {}
        [NoScaleOffset]_MaskMap("MaskMap", 2D) = "white" {}
        [NoScaleOffset]_ThicknessMap("Thickness", 2D) = "white" {}
        _StiffnessVariation("StiffnessVariation", Range(0, 1)) = 0.3
        _WindDirectionAndStrength("WindDirectionAndStrength", Vector) = (1, 1, 1, 1)
        _Shiver("Shiver", Vector) = (1, 1, 0, 0)
        [ToggleUI]_BAKEDMASK_ON("BakedMaskOn", Float) = 1
        [ToggleUI]_UVMASK_ON("UvMaskOn", Float) = 0
        [ToggleUI]_VERTEXPOSITIONMASK_ON("VertexPositionMaskOn", Float) = 0
        [HideInInspector]_EmissionColor("Color", Color) = (1, 1, 1, 1)
        [HideInInspector]_RenderQueueType("Vector1", Float) = 1
        [HideInInspector]_StencilRef("Vector1", Int) = 0
        [HideInInspector]_StencilWriteMask("Vector1", Int) = 3
        [HideInInspector]_StencilRefDepth("Vector1", Int) = 0
        [HideInInspector]_StencilWriteMaskDepth("Vector1", Int) = 32
        [HideInInspector]_StencilRefMV("Vector1", Int) = 128
        [HideInInspector]_StencilWriteMaskMV("Vector1", Int) = 128
        [HideInInspector]_StencilRefDistortionVec("Vector1", Int) = 64
        [HideInInspector]_StencilWriteMaskDistortionVec("Vector1", Int) = 64
        [HideInInspector]_StencilWriteMaskGBuffer("Vector1", Int) = 3
        [HideInInspector]_StencilRefGBuffer("Vector1", Int) = 2
        [HideInInspector]_ZTestGBuffer("Vector1", Int) = 4
        [HideInInspector][ToggleUI]_RequireSplitLighting("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_ReceivesSSR("Boolean", Float) = 0
        [HideInInspector]_SurfaceType("Vector1", Float) = 0
        [HideInInspector]_BlendMode("Vector1", Float) = 0
        [HideInInspector]_SrcBlend("Vector1", Float) = 1
        [HideInInspector]_DstBlend("Vector1", Float) = 0
        [HideInInspector]_AlphaSrcBlend("Vector1", Float) = 1
        [HideInInspector]_AlphaDstBlend("Vector1", Float) = 0
        [HideInInspector][ToggleUI]_ZWrite("Boolean", Float) = 1
        [HideInInspector]_CullMode("Vector1", Float) = 2
        [HideInInspector]_TransparentSortPriority("Vector1", Int) = 0
        [HideInInspector]_CullModeForward("Vector1", Float) = 2
        [HideInInspector][Enum(Front, 1, Back, 2)]_TransparentCullMode("Vector1", Float) = 2
        [HideInInspector]_ZTestDepthEqualForOpaque("Vector1", Int) = 4
        [HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)]_ZTestTransparent("Vector1", Float) = 4
        [HideInInspector][ToggleUI]_TransparentBackfaceEnable("Boolean", Float) = 1
        [HideInInspector][ToggleUI]_AlphaCutoffEnable("Boolean", Float) = 1
        [HideInInspector]_AlphaCutoff("Alpha Cutoff", Range(0, 1)) = 0.5
        [HideInInspector][ToggleUI]_UseShadowThreshold("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_DoubleSidedEnable("Boolean", Float) = 1
        [HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)]_DoubleSidedNormalMode("Vector1", Float) = 0
        [HideInInspector]_DoubleSidedConstants("Vector4", Vector) = (1, 1, -1, 0)
        [HideInInspector]_DiffusionProfileHash("Vector1", Float) = 0
        [HideInInspector]_DiffusionProfileAsset("Vector4", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="HDRenderPipeline"
            "RenderType"="HDLitShader"
            "Queue" = "AlphaTest+0"
        }
        
        Pass
        {
            // based on HDLitPass.template
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
        
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull [_CullMode]
        
            
            ZWrite On
        
            ZClip [_ZClip]
        
            
            ColorMask 0
        
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        
            #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            // #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            #define _DISABLE_DECALS 1
            #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        
            //-------------------------------------------------------------------------------------
            // End Variant Definitions
            //-------------------------------------------------------------------------------------
        
            #pragma vertex Vert
            #pragma fragment Frag
        
            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                    #define SHADERPASS SHADERPASS_SHADOWS
                #define RAYTRACING_SHADER_GRAPH_HIGH
                // ACTIVE FIELDS:
                //   DoubleSided
                //   DoubleSided.Flip
                //   FragInputs.isFrontFace
                //   Material.Translucent
                //   Material.Transmission
                //   AlphaTest
                //   DisableDecals
                //   DisableSSR
                //   Specular.EnergyConserving
                //   SurfaceDescriptionInputs.TangentSpaceNormal
                //   SurfaceDescriptionInputs.uv0
                //   VertexDescriptionInputs.VertexColor
                //   VertexDescriptionInputs.ObjectSpaceNormal
                //   VertexDescriptionInputs.WorldSpaceNormal
                //   VertexDescriptionInputs.ObjectSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceBiTangent
                //   VertexDescriptionInputs.ObjectSpacePosition
                //   VertexDescriptionInputs.AbsoluteWorldSpacePosition
                //   VertexDescriptionInputs.uv0
                //   VertexDescriptionInputs.uv1
                //   VertexDescriptionInputs.TimeParameters
                //   SurfaceDescription.Alpha
                //   SurfaceDescription.AlphaClipThreshold
                //   features.modifyMesh
                //   VertexDescription.VertexPosition
                //   VertexDescription.VertexNormal
                //   VertexDescription.VertexTangent
                //   VaryingsMeshToPS.cullFace
                //   FragInputs.texCoord0
                //   AttributesMesh.color
                //   AttributesMesh.normalOS
                //   AttributesMesh.tangentOS
                //   VertexDescriptionInputs.ObjectSpaceBiTangent
                //   AttributesMesh.positionOS
                //   AttributesMesh.uv0
                //   AttributesMesh.uv1
                //   VaryingsMeshToPS.texCoord0
                // Shared Graph Keywords
        
            // this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            // #define ATTRIBUTES_NEED_TEXCOORD2
            // #define ATTRIBUTES_NEED_TEXCOORD3
            #define ATTRIBUTES_NEED_COLOR
            // #define VARYINGS_NEED_POSITION_WS
            // #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            // #define VARYINGS_NEED_TEXCOORD1
            // #define VARYINGS_NEED_TEXCOORD2
            // #define VARYINGS_NEED_TEXCOORD3
            // #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_CULLFACE
            #define HAVE_MESH_MODIFICATION
        
        // We need isFontFace when using double sided
        #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
            #define VARYINGS_NEED_CULLFACE
        #endif
        
            //-------------------------------------------------------------------------------------
            // End Defines
            //-------------------------------------------------------------------------------------
        	
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
            //-------------------------------------------------------------------------------------
            // Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
            // Generated Type: AttributesMesh
            struct AttributesMesh
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL; // optional
                float4 tangentOS : TANGENT; // optional
                float4 uv0 : TEXCOORD0; // optional
                float4 uv1 : TEXCOORD1; // optional
                float4 color : COLOR; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            // Generated Type: VaryingsMeshToPS
            struct VaryingsMeshToPS
            {
                float4 positionCS : SV_Position;
                float4 texCoord0; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            };
            
            // Generated Type: PackedVaryingsMeshToPS
            struct PackedVaryingsMeshToPS
            {
                float4 positionCS : SV_Position; // unpacked
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float4 interp00 : TEXCOORD0; // auto-packed
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
                #endif // conditional
            };
            
            // Packed Type: VaryingsMeshToPS
            PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
            {
                PackedVaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToPS
            VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
            {
                VaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            // Generated Type: VaryingsMeshToDS
            struct VaryingsMeshToDS
            {
                float3 positionRWS;
                float3 normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            
            // Generated Type: PackedVaryingsMeshToDS
            struct PackedVaryingsMeshToDS
            {
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
            };
            
            // Packed Type: VaryingsMeshToDS
            PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
            {
                PackedVaryingsMeshToDS output;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToDS
            VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
            {
                VaryingsMeshToDS output;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            //-------------------------------------------------------------------------------------
            // End Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
        
            //-------------------------------------------------------------------------------------
            // Graph generated code
            //-------------------------------------------------------------------------------------
                    // Shared Graph Properties (uniform inputs)
                    CBUFFER_START(UnityPerMaterial)
                    float _AlphaClip;
                    float _Hue;
                    float _Saturation;
                    float _Lightness;
                    float _StiffnessVariation;
                    float4 _WindDirectionAndStrength;
                    float4 _Shiver;
                    float _BAKEDMASK_ON;
                    float _UVMASK_ON;
                    float _VERTEXPOSITIONMASK_ON;
                    float4 _EmissionColor;
                    float _RenderQueueType;
                    float _StencilRef;
                    float _StencilWriteMask;
                    float _StencilRefDepth;
                    float _StencilWriteMaskDepth;
                    float _StencilRefMV;
                    float _StencilWriteMaskMV;
                    float _StencilRefDistortionVec;
                    float _StencilWriteMaskDistortionVec;
                    float _StencilWriteMaskGBuffer;
                    float _StencilRefGBuffer;
                    float _ZTestGBuffer;
                    float _RequireSplitLighting;
                    float _ReceivesSSR;
                    float _SurfaceType;
                    float _BlendMode;
                    float _SrcBlend;
                    float _DstBlend;
                    float _AlphaSrcBlend;
                    float _AlphaDstBlend;
                    float _ZWrite;
                    float _CullMode;
                    float _TransparentSortPriority;
                    float _CullModeForward;
                    float _TransparentCullMode;
                    float _ZTestDepthEqualForOpaque;
                    float _ZTestTransparent;
                    float _TransparentBackfaceEnable;
                    float _AlphaCutoffEnable;
                    float _AlphaCutoff;
                    float _UseShadowThreshold;
                    float _DoubleSidedEnable;
                    float _DoubleSidedNormalMode;
                    float4 _DoubleSidedConstants;
                    float _DiffusionProfileHash;
                    float4 _DiffusionProfileAsset;
                    CBUFFER_END
                    TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo); float4 _Albedo_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap); float4 _MaskMap_TexelSize;
                    TEXTURE2D(_ThicknessMap); SAMPLER(sampler_ThicknessMap); float4 _ThicknessMap_TexelSize;
                    float4 _GlobalWindDirectionAndStrength;
                    float4 _GlobalShiver;
                    TEXTURE2D(_ShiverNoise); SAMPLER(sampler_ShiverNoise); float4 _ShiverNoise_TexelSize;
                    TEXTURE2D(_GustNoise); SAMPLER(sampler_GustNoise); float4 _GustNoise_TexelSize;
                    SAMPLER(_SampleTexture2D_F86B9939_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_46D09289_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_DBCD6404_Sampler_3_Linear_Repeat);
                
                // Vertex Graph Inputs
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal; // optional
                        float3 WorldSpaceNormal; // optional
                        float3 ObjectSpaceTangent; // optional
                        float3 WorldSpaceTangent; // optional
                        float3 ObjectSpaceBiTangent; // optional
                        float3 WorldSpaceBiTangent; // optional
                        float3 ObjectSpacePosition; // optional
                        float3 AbsoluteWorldSpacePosition; // optional
                        float4 uv0; // optional
                        float4 uv1; // optional
                        float4 VertexColor; // optional
                        float3 TimeParameters; // optional
                    };
                // Vertex Graph Outputs
                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };
                    
                // Pixel Graph Inputs
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal; // optional
                        float4 uv0; // optional
                    };
                // Pixel Graph Outputs
                    struct SurfaceDescription
                    {
                        float Alpha;
                        float AlphaClipThreshold;
                    };
                    
                // Shared Graph Node Functions
                
                    struct Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238
                    {
                    };
                
                    void SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 IN, out float3 PivotInWS_0)
                    {
                        PivotInWS_0 = SHADERGRAPH_OBJECT_POSITION;
                    }
                
                    void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                    {
                        Out = lerp(A, B, T);
                    }
                
                    void Unity_Multiply_float (float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                    {
                        RGBA = float4(R, G, B, A);
                        RGB = float3(R, G, B);
                        RG = float2(R, G);
                    }
                
                    void Unity_Length_float3(float3 In, out float Out)
                    {
                        Out = length(In);
                    }
                
                    void Unity_Multiply_float (float A, float B, out float Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }
                
                    void Unity_Normalize_float3(float3 In, out float3 Out)
                    {
                        Out = normalize(In);
                    }
                
                    void Unity_Maximum_float(float A, float B, out float Out)
                    {
                        Out = max(A, B);
                    }
                
                    void Unity_Multiply_float (float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Maximum_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = max(A, B);
                    }
                
                    struct Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7
                    {
                    };
                
                    void SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(float4 Vector4_14B5A446, float4 Vector4_6887180D, float2 Vector2_F270B07E, float2 Vector2_70BD0D1B, Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 IN, out float3 GustDirection_0, out float GustSpeed_1, out float GustStrength_2, out float ShiverSpeed_3, out float ShiverStrength_4)
                    {
                        float3 _Vector3_E24D7903_Out_0 = float3(0.7, 0, 0.3);
                        float4 _Property_95651D48_Out_0 = Vector4_14B5A446;
                        float4 _Property_FFEF34C6_Out_0 = Vector4_6887180D;
                        float4 _Multiply_7F93D556_Out_2;
                        Unity_Multiply_float(_Property_95651D48_Out_0, _Property_FFEF34C6_Out_0, _Multiply_7F93D556_Out_2);
                        float _Split_1A6C2849_R_1 = _Multiply_7F93D556_Out_2[0];
                        float _Split_1A6C2849_G_2 = _Multiply_7F93D556_Out_2[1];
                        float _Split_1A6C2849_B_3 = _Multiply_7F93D556_Out_2[2];
                        float _Split_1A6C2849_A_4 = _Multiply_7F93D556_Out_2[3];
                        float4 _Combine_769EB158_RGBA_4;
                        float3 _Combine_769EB158_RGB_5;
                        float2 _Combine_769EB158_RG_6;
                        Unity_Combine_float(_Split_1A6C2849_R_1, 0, _Split_1A6C2849_G_2, 0, _Combine_769EB158_RGBA_4, _Combine_769EB158_RGB_5, _Combine_769EB158_RG_6);
                        float _Length_62815FED_Out_1;
                        Unity_Length_float3(_Combine_769EB158_RGB_5, _Length_62815FED_Out_1);
                        float _Multiply_A4A39D4F_Out_2;
                        Unity_Multiply_float(_Length_62815FED_Out_1, 1000, _Multiply_A4A39D4F_Out_2);
                        float _Clamp_4B28219D_Out_3;
                        Unity_Clamp_float(_Multiply_A4A39D4F_Out_2, 0, 1, _Clamp_4B28219D_Out_3);
                        float3 _Lerp_66854A50_Out_3;
                        Unity_Lerp_float3(_Vector3_E24D7903_Out_0, _Combine_769EB158_RGB_5, (_Clamp_4B28219D_Out_3.xxx), _Lerp_66854A50_Out_3);
                        float3 _Normalize_B2778668_Out_1;
                        Unity_Normalize_float3(_Lerp_66854A50_Out_3, _Normalize_B2778668_Out_1);
                        float _Maximum_A3AFA1AB_Out_2;
                        Unity_Maximum_float(_Split_1A6C2849_B_3, 0.01, _Maximum_A3AFA1AB_Out_2);
                        float _Maximum_FCE0058_Out_2;
                        Unity_Maximum_float(0, _Split_1A6C2849_A_4, _Maximum_FCE0058_Out_2);
                        float2 _Property_F062BDE_Out_0 = Vector2_F270B07E;
                        float2 _Property_FB73C895_Out_0 = Vector2_70BD0D1B;
                        float2 _Multiply_76AC0593_Out_2;
                        Unity_Multiply_float(_Property_F062BDE_Out_0, _Property_FB73C895_Out_0, _Multiply_76AC0593_Out_2);
                        float2 _Maximum_E318FF04_Out_2;
                        Unity_Maximum_float2(_Multiply_76AC0593_Out_2, float2(0.01, 0.01), _Maximum_E318FF04_Out_2);
                        float _Split_F437A5E0_R_1 = _Maximum_E318FF04_Out_2[0];
                        float _Split_F437A5E0_G_2 = _Maximum_E318FF04_Out_2[1];
                        float _Split_F437A5E0_B_3 = 0;
                        float _Split_F437A5E0_A_4 = 0;
                        GustDirection_0 = _Normalize_B2778668_Out_1;
                        GustSpeed_1 = _Maximum_A3AFA1AB_Out_2;
                        GustStrength_2 = _Maximum_FCE0058_Out_2;
                        ShiverSpeed_3 = _Split_F437A5E0_R_1;
                        ShiverStrength_4 = _Split_F437A5E0_G_2;
                    }
                
                    void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A - B;
                    }
                
                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Fraction_float(float In, out float Out)
                    {
                        Out = frac(In);
                    }
                
                    void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                    {
                        Out = lerp(False, True, Predicate);
                    }
                
                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A - B;
                    }
                
                    struct Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f
                    {
                    };
                
                    void SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(float Vector1_CCF53CDA, float Vector1_D95E40FE, float2 Vector2_AEE18C41, float2 Vector2_A9CE092C, float Vector1_F2ED6CCC, TEXTURE2D_PARAM(Texture2D_F14459DD, samplerTexture2D_F14459DD), float4 Texture2D_F14459DD_TexelSize, Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f IN, out float GustNoise_0)
                    {
                        float2 _Property_A92CC1B7_Out_0 = Vector2_AEE18C41;
                        float _Property_36B40CE_Out_0 = Vector1_D95E40FE;
                        float _Multiply_9E28D3C4_Out_2;
                        Unity_Multiply_float(_Property_36B40CE_Out_0, 2, _Multiply_9E28D3C4_Out_2);
                        float2 _Add_C54F05FE_Out_2;
                        Unity_Add_float2(_Property_A92CC1B7_Out_0, (_Multiply_9E28D3C4_Out_2.xx), _Add_C54F05FE_Out_2);
                        float2 _Multiply_9CD1691E_Out_2;
                        Unity_Multiply_float(_Add_C54F05FE_Out_2, float2(0.01, 0.01), _Multiply_9CD1691E_Out_2);
                        float2 _Property_D05D9ECB_Out_0 = Vector2_A9CE092C;
                        float _Property_8BFC9AA2_Out_0 = Vector1_CCF53CDA;
                        float2 _Multiply_462DF694_Out_2;
                        Unity_Multiply_float(_Property_D05D9ECB_Out_0, (_Property_8BFC9AA2_Out_0.xx), _Multiply_462DF694_Out_2);
                        float _Property_4DB65C54_Out_0 = Vector1_F2ED6CCC;
                        float2 _Multiply_50FD4B48_Out_2;
                        Unity_Multiply_float(_Multiply_462DF694_Out_2, (_Property_4DB65C54_Out_0.xx), _Multiply_50FD4B48_Out_2);
                        float2 _Subtract_B4A749C2_Out_2;
                        Unity_Subtract_float2(_Multiply_9CD1691E_Out_2, _Multiply_50FD4B48_Out_2, _Subtract_B4A749C2_Out_2);
                        float4 _SampleTexture2DLOD_46D09289_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_F14459DD, samplerTexture2D_F14459DD, _Subtract_B4A749C2_Out_2, 0);
                        float _SampleTexture2DLOD_46D09289_R_5 = _SampleTexture2DLOD_46D09289_RGBA_0.r;
                        float _SampleTexture2DLOD_46D09289_G_6 = _SampleTexture2DLOD_46D09289_RGBA_0.g;
                        float _SampleTexture2DLOD_46D09289_B_7 = _SampleTexture2DLOD_46D09289_RGBA_0.b;
                        float _SampleTexture2DLOD_46D09289_A_8 = _SampleTexture2DLOD_46D09289_RGBA_0.a;
                        GustNoise_0 = _SampleTexture2DLOD_46D09289_R_5;
                    }
                
                    void Unity_Power_float(float A, float B, out float Out)
                    {
                        Out = pow(A, B);
                    }
                
                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }
                
                    struct Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19
                    {
                    };
                
                    void SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(float2 Vector2_CA78C39A, float Vector1_279D2776, Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 IN, out float RandomStiffness_0)
                    {
                        float2 _Property_475BFCB9_Out_0 = Vector2_CA78C39A;
                        float2 _Multiply_7EE00C92_Out_2;
                        Unity_Multiply_float(_Property_475BFCB9_Out_0, float2(10, 10), _Multiply_7EE00C92_Out_2);
                        float _Split_A0FB144F_R_1 = _Multiply_7EE00C92_Out_2[0];
                        float _Split_A0FB144F_G_2 = _Multiply_7EE00C92_Out_2[1];
                        float _Split_A0FB144F_B_3 = 0;
                        float _Split_A0FB144F_A_4 = 0;
                        float _Multiply_2482A544_Out_2;
                        Unity_Multiply_float(_Split_A0FB144F_R_1, _Split_A0FB144F_G_2, _Multiply_2482A544_Out_2);
                        float _Fraction_B90029E4_Out_1;
                        Unity_Fraction_float(_Multiply_2482A544_Out_2, _Fraction_B90029E4_Out_1);
                        float _Power_E2B2B095_Out_2;
                        Unity_Power_float(_Fraction_B90029E4_Out_1, 2, _Power_E2B2B095_Out_2);
                        float _Property_91226CD6_Out_0 = Vector1_279D2776;
                        float _OneMinus_A56B8867_Out_1;
                        Unity_OneMinus_float(_Property_91226CD6_Out_0, _OneMinus_A56B8867_Out_1);
                        float _Clamp_E85434A6_Out_3;
                        Unity_Clamp_float(_Power_E2B2B095_Out_2, _OneMinus_A56B8867_Out_1, 1, _Clamp_E85434A6_Out_3);
                        RandomStiffness_0 = _Clamp_E85434A6_Out_3;
                    }
                
                    struct Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628
                    {
                    };
                
                    void SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(float Vector1_AFC49E6C, float Vector1_A18CF4DF, float Vector1_28AC83F8, float Vector1_E0042E1, float Vector1_1A24AAF, Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 IN, out float GustStrength_0)
                    {
                        float _Property_9A741C0D_Out_0 = Vector1_AFC49E6C;
                        float _Property_F564A485_Out_0 = Vector1_A18CF4DF;
                        float _Multiply_248F3A68_Out_2;
                        Unity_Multiply_float(_Property_9A741C0D_Out_0, _Property_F564A485_Out_0, _Multiply_248F3A68_Out_2);
                        float _Clamp_64D749D9_Out_3;
                        Unity_Clamp_float(_Multiply_248F3A68_Out_2, 0.1, 0.9, _Clamp_64D749D9_Out_3);
                        float _OneMinus_BDC5FAC3_Out_1;
                        Unity_OneMinus_float(_Clamp_64D749D9_Out_3, _OneMinus_BDC5FAC3_Out_1);
                        float _Multiply_E3C6FEFE_Out_2;
                        Unity_Multiply_float(_Multiply_248F3A68_Out_2, _OneMinus_BDC5FAC3_Out_1, _Multiply_E3C6FEFE_Out_2);
                        float _Multiply_9087CA8A_Out_2;
                        Unity_Multiply_float(_Multiply_E3C6FEFE_Out_2, 1.5, _Multiply_9087CA8A_Out_2);
                        float _Property_C7E6777F_Out_0 = Vector1_28AC83F8;
                        float _Multiply_1D329CB_Out_2;
                        Unity_Multiply_float(_Multiply_9087CA8A_Out_2, _Property_C7E6777F_Out_0, _Multiply_1D329CB_Out_2);
                        float _Property_84113466_Out_0 = Vector1_E0042E1;
                        float _Multiply_9501294C_Out_2;
                        Unity_Multiply_float(_Multiply_1D329CB_Out_2, _Property_84113466_Out_0, _Multiply_9501294C_Out_2);
                        float _Property_57C5AF03_Out_0 = Vector1_1A24AAF;
                        float _Multiply_E178164E_Out_2;
                        Unity_Multiply_float(_Multiply_9501294C_Out_2, _Property_57C5AF03_Out_0, _Multiply_E178164E_Out_2);
                        GustStrength_0 = _Multiply_E178164E_Out_2;
                    }
                
                    void Unity_Multiply_float (float3 A, float3 B, out float3 Out)
                    {
                        Out = A * B;
                    }
                
                    struct Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a
                    {
                    };
                
                    void SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(float2 Vector2_287CB44E, float2 Vector2_2A17E6EA, float Vector1_F4B6A491, float Vector1_2C90770B, TEXTURE2D_PARAM(Texture2D_D44B4848, samplerTexture2D_D44B4848), float4 Texture2D_D44B4848_TexelSize, float Vector1_AD94E9BC, Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a IN, out float3 ShiverNoise_0)
                    {
                        float2 _Property_961D8A0_Out_0 = Vector2_287CB44E;
                        float _Property_A414F012_Out_0 = Vector1_AD94E9BC;
                        float _Multiply_7DB42988_Out_2;
                        Unity_Multiply_float(_Property_A414F012_Out_0, 2, _Multiply_7DB42988_Out_2);
                        float2 _Add_4C3CF1F_Out_2;
                        Unity_Add_float2(_Property_961D8A0_Out_0, (_Multiply_7DB42988_Out_2.xx), _Add_4C3CF1F_Out_2);
                        float2 _Property_EBC67BC7_Out_0 = Vector2_2A17E6EA;
                        float _Property_13D296B5_Out_0 = Vector1_F4B6A491;
                        float2 _Multiply_BBB72061_Out_2;
                        Unity_Multiply_float(_Property_EBC67BC7_Out_0, (_Property_13D296B5_Out_0.xx), _Multiply_BBB72061_Out_2);
                        float _Property_3BB601E6_Out_0 = Vector1_2C90770B;
                        float2 _Multiply_FF9010E8_Out_2;
                        Unity_Multiply_float(_Multiply_BBB72061_Out_2, (_Property_3BB601E6_Out_0.xx), _Multiply_FF9010E8_Out_2);
                        float2 _Subtract_6BF2D170_Out_2;
                        Unity_Subtract_float2(_Add_4C3CF1F_Out_2, _Multiply_FF9010E8_Out_2, _Subtract_6BF2D170_Out_2);
                        float4 _SampleTexture2DLOD_DBCD6404_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_D44B4848, samplerTexture2D_D44B4848, _Subtract_6BF2D170_Out_2, 0);
                        float _SampleTexture2DLOD_DBCD6404_R_5 = _SampleTexture2DLOD_DBCD6404_RGBA_0.r;
                        float _SampleTexture2DLOD_DBCD6404_G_6 = _SampleTexture2DLOD_DBCD6404_RGBA_0.g;
                        float _SampleTexture2DLOD_DBCD6404_B_7 = _SampleTexture2DLOD_DBCD6404_RGBA_0.b;
                        float _SampleTexture2DLOD_DBCD6404_A_8 = _SampleTexture2DLOD_DBCD6404_RGBA_0.a;
                        float4 _Combine_E5D76A97_RGBA_4;
                        float3 _Combine_E5D76A97_RGB_5;
                        float2 _Combine_E5D76A97_RG_6;
                        Unity_Combine_float(_SampleTexture2DLOD_DBCD6404_R_5, _SampleTexture2DLOD_DBCD6404_G_6, _SampleTexture2DLOD_DBCD6404_B_7, 0, _Combine_E5D76A97_RGBA_4, _Combine_E5D76A97_RGB_5, _Combine_E5D76A97_RG_6);
                        float3 _Subtract_AA7C02E2_Out_2;
                        Unity_Subtract_float3(_Combine_E5D76A97_RGB_5, float3(0.5, 0.5, 0.5), _Subtract_AA7C02E2_Out_2);
                        float3 _Multiply_5BF7CBD7_Out_2;
                        Unity_Multiply_float(_Subtract_AA7C02E2_Out_2, float3(2, 2, 2), _Multiply_5BF7CBD7_Out_2);
                        ShiverNoise_0 = _Multiply_5BF7CBD7_Out_2;
                    }
                
                    struct Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459
                    {
                    };
                
                    void SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(float3 Vector3_ED0F539A, float2 Vector2_84805101, float Vector1_BDF24CF7, float Vector1_839268A4, float Vector1_A8621014, float Vector1_2DBE6CC0, float Vector1_8A4EF006, float Vector1_ED935C73, Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 IN, out float3 ShiverDirection_0, out float ShiverStrength_1)
                    {
                        float3 _Property_FC94AEBB_Out_0 = Vector3_ED0F539A;
                        float _Property_4FE2271A_Out_0 = Vector1_BDF24CF7;
                        float4 _Combine_328044F1_RGBA_4;
                        float3 _Combine_328044F1_RGB_5;
                        float2 _Combine_328044F1_RG_6;
                        Unity_Combine_float(1, _Property_4FE2271A_Out_0, 1, 0, _Combine_328044F1_RGBA_4, _Combine_328044F1_RGB_5, _Combine_328044F1_RG_6);
                        float3 _Multiply_4FCE02F7_Out_2;
                        Unity_Multiply_float(_Property_FC94AEBB_Out_0, _Combine_328044F1_RGB_5, _Multiply_4FCE02F7_Out_2);
                        float2 _Property_77EED0A8_Out_0 = Vector2_84805101;
                        float _Split_2D66AF35_R_1 = _Property_77EED0A8_Out_0[0];
                        float _Split_2D66AF35_G_2 = _Property_77EED0A8_Out_0[1];
                        float _Split_2D66AF35_B_3 = 0;
                        float _Split_2D66AF35_A_4 = 0;
                        float4 _Combine_C2861A09_RGBA_4;
                        float3 _Combine_C2861A09_RGB_5;
                        float2 _Combine_C2861A09_RG_6;
                        Unity_Combine_float(_Split_2D66AF35_R_1, _Property_4FE2271A_Out_0, _Split_2D66AF35_G_2, 0, _Combine_C2861A09_RGBA_4, _Combine_C2861A09_RGB_5, _Combine_C2861A09_RG_6);
                        float3 _Lerp_A6B0BE86_Out_3;
                        Unity_Lerp_float3(_Multiply_4FCE02F7_Out_2, _Combine_C2861A09_RGB_5, float3(0.5, 0.5, 0.5), _Lerp_A6B0BE86_Out_3);
                        float _Property_BBBC9C1B_Out_0 = Vector1_839268A4;
                        float _Length_F022B321_Out_1;
                        Unity_Length_float3(_Multiply_4FCE02F7_Out_2, _Length_F022B321_Out_1);
                        float _Multiply_BFD84B03_Out_2;
                        Unity_Multiply_float(_Length_F022B321_Out_1, 0.5, _Multiply_BFD84B03_Out_2);
                        float _Multiply_3564B68A_Out_2;
                        Unity_Multiply_float(_Property_BBBC9C1B_Out_0, _Multiply_BFD84B03_Out_2, _Multiply_3564B68A_Out_2);
                        float _Add_83285742_Out_2;
                        Unity_Add_float(_Multiply_3564B68A_Out_2, _Length_F022B321_Out_1, _Add_83285742_Out_2);
                        float _Property_45D94B1_Out_0 = Vector1_2DBE6CC0;
                        float _Multiply_EA43D494_Out_2;
                        Unity_Multiply_float(_Add_83285742_Out_2, _Property_45D94B1_Out_0, _Multiply_EA43D494_Out_2);
                        float _Clamp_C109EA71_Out_3;
                        Unity_Clamp_float(_Multiply_EA43D494_Out_2, 0.1, 0.9, _Clamp_C109EA71_Out_3);
                        float _OneMinus_226F3377_Out_1;
                        Unity_OneMinus_float(_Clamp_C109EA71_Out_3, _OneMinus_226F3377_Out_1);
                        float _Multiply_8680628F_Out_2;
                        Unity_Multiply_float(_Multiply_EA43D494_Out_2, _OneMinus_226F3377_Out_1, _Multiply_8680628F_Out_2);
                        float _Multiply_B14E644_Out_2;
                        Unity_Multiply_float(_Multiply_8680628F_Out_2, 1.5, _Multiply_B14E644_Out_2);
                        float _Property_7F61FC78_Out_0 = Vector1_A8621014;
                        float _Multiply_C89CF7DC_Out_2;
                        Unity_Multiply_float(_Multiply_B14E644_Out_2, _Property_7F61FC78_Out_0, _Multiply_C89CF7DC_Out_2);
                        float _Property_2BD306B6_Out_0 = Vector1_8A4EF006;
                        float _Multiply_E5D34DCC_Out_2;
                        Unity_Multiply_float(_Multiply_C89CF7DC_Out_2, _Property_2BD306B6_Out_0, _Multiply_E5D34DCC_Out_2);
                        float _Property_DBC71A4F_Out_0 = Vector1_ED935C73;
                        float _Multiply_BCACDD38_Out_2;
                        Unity_Multiply_float(_Multiply_E5D34DCC_Out_2, _Property_DBC71A4F_Out_0, _Multiply_BCACDD38_Out_2);
                        ShiverDirection_0 = _Lerp_A6B0BE86_Out_3;
                        ShiverStrength_1 = _Multiply_BCACDD38_Out_2;
                    }
                
                    struct Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364
                    {
                    };
                
                    void SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(float3 Vector3_829210A7, float3 Vector3_1A016C4A, float Vector1_31372BF, float Vector1_E57895AF, TEXTURE2D_PARAM(Texture2D_65F71447, samplerTexture2D_65F71447), float4 Texture2D_65F71447_TexelSize, float Vector1_8836FB6A, TEXTURE2D_PARAM(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), float4 Texture2D_4A3BDB6_TexelSize, float Vector1_14E206AE, float Vector1_7090E96C, float Vector1_51722AC, float Vector1_A3894D2, float Vector1_6F0C3A5A, float Vector1_2D1F6C2F, float Vector1_347751CA, Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 IN, out float GustStrength_0, out float ShiverStrength_1, out float3 ShiverDirection_2)
                    {
                        float _Property_5C7F4A8C_Out_0 = Vector1_31372BF;
                        float _Property_142FEDC3_Out_0 = Vector1_347751CA;
                        float3 _Property_D2FC65C3_Out_0 = Vector3_829210A7;
                        float _Split_8E347DCF_R_1 = _Property_D2FC65C3_Out_0[0];
                        float _Split_8E347DCF_G_2 = _Property_D2FC65C3_Out_0[1];
                        float _Split_8E347DCF_B_3 = _Property_D2FC65C3_Out_0[2];
                        float _Split_8E347DCF_A_4 = 0;
                        float4 _Combine_9B5A76B7_RGBA_4;
                        float3 _Combine_9B5A76B7_RGB_5;
                        float2 _Combine_9B5A76B7_RG_6;
                        Unity_Combine_float(_Split_8E347DCF_R_1, _Split_8E347DCF_B_3, 0, 0, _Combine_9B5A76B7_RGBA_4, _Combine_9B5A76B7_RGB_5, _Combine_9B5A76B7_RG_6);
                        float3 _Property_5653999E_Out_0 = Vector3_1A016C4A;
                        float _Split_B9CBBFE5_R_1 = _Property_5653999E_Out_0[0];
                        float _Split_B9CBBFE5_G_2 = _Property_5653999E_Out_0[1];
                        float _Split_B9CBBFE5_B_3 = _Property_5653999E_Out_0[2];
                        float _Split_B9CBBFE5_A_4 = 0;
                        float4 _Combine_DC44394B_RGBA_4;
                        float3 _Combine_DC44394B_RGB_5;
                        float2 _Combine_DC44394B_RG_6;
                        Unity_Combine_float(_Split_B9CBBFE5_R_1, _Split_B9CBBFE5_B_3, 0, 0, _Combine_DC44394B_RGBA_4, _Combine_DC44394B_RGB_5, _Combine_DC44394B_RG_6);
                        float _Property_3221EFCE_Out_0 = Vector1_E57895AF;
                        Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f _GustNoiseAtPosition_3B28852B;
                        float _GustNoiseAtPosition_3B28852B_GustNoise_0;
                        SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(_Property_5C7F4A8C_Out_0, _Property_142FEDC3_Out_0, _Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_3221EFCE_Out_0, TEXTURE2D_ARGS(Texture2D_65F71447, samplerTexture2D_65F71447), Texture2D_65F71447_TexelSize, _GustNoiseAtPosition_3B28852B, _GustNoiseAtPosition_3B28852B_GustNoise_0);
                        float _Property_1B306054_Out_0 = Vector1_A3894D2;
                        float _Property_1FBC768_Out_0 = Vector1_51722AC;
                        float _Property_9FB10D19_Out_0 = Vector1_14E206AE;
                        Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 _RandomStiffnessAtPosition_C9AD50AB;
                        float _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0;
                        SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(_Combine_9B5A76B7_RG_6, _Property_9FB10D19_Out_0, _RandomStiffnessAtPosition_C9AD50AB, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0);
                        float _Property_EE5A603D_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 _CalculateGustStrength_E2853C74;
                        float _CalculateGustStrength_E2853C74_GustStrength_0;
                        SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(_GustNoiseAtPosition_3B28852B_GustNoise_0, _Property_1B306054_Out_0, _Property_1FBC768_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _Property_EE5A603D_Out_0, _CalculateGustStrength_E2853C74, _CalculateGustStrength_E2853C74_GustStrength_0);
                        float _Property_DFB3FCE0_Out_0 = Vector1_31372BF;
                        float _Property_8A8735CC_Out_0 = Vector1_8836FB6A;
                        Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a _ShiverNoiseAtPosition_35F9220A;
                        float3 _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0;
                        SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(_Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_DFB3FCE0_Out_0, _Property_8A8735CC_Out_0, TEXTURE2D_ARGS(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), Texture2D_4A3BDB6_TexelSize, _Property_142FEDC3_Out_0, _ShiverNoiseAtPosition_35F9220A, _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0);
                        float _Property_65F19953_Out_0 = Vector1_6F0C3A5A;
                        float _Property_3A2F45FE_Out_0 = Vector1_51722AC;
                        float _Property_98EF73E5_Out_0 = Vector1_2D1F6C2F;
                        float _Property_6A278DE2_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 _CalculateShiver_799DE4CB;
                        float3 _CalculateShiver_799DE4CB_ShiverDirection_0;
                        float _CalculateShiver_799DE4CB_ShiverStrength_1;
                        SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(_ShiverNoiseAtPosition_35F9220A_ShiverNoise_0, _Combine_DC44394B_RG_6, _Property_65F19953_Out_0, _CalculateGustStrength_E2853C74_GustStrength_0, _Property_3A2F45FE_Out_0, _Property_98EF73E5_Out_0, _Property_6A278DE2_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _CalculateShiver_799DE4CB, _CalculateShiver_799DE4CB_ShiverDirection_0, _CalculateShiver_799DE4CB_ShiverStrength_1);
                        GustStrength_0 = _CalculateGustStrength_E2853C74_GustStrength_0;
                        ShiverStrength_1 = _CalculateShiver_799DE4CB_ShiverStrength_1;
                        ShiverDirection_2 = _CalculateShiver_799DE4CB_ShiverDirection_0;
                    }
                
                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }
                
                    struct Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01
                    {
                        float3 ObjectSpacePosition;
                    };
                
                    void SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(float3 Vector3_C96069F9, float Vector1_A5EB719C, float Vector1_4D1D3B1A, float3 Vector3_C80E97FF, float3 Vector3_821C320A, float3 Vector3_4BF0DC64, Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 IN, out float3 WorldPosition_0)
                    {
                        float3 _Property_65372844_Out_0 = Vector3_4BF0DC64;
                        float3 _Property_7205E35B_Out_0 = Vector3_821C320A;
                        float _Property_916D8D52_Out_0 = Vector1_4D1D3B1A;
                        float3 _Multiply_CF9DF1B5_Out_2;
                        Unity_Multiply_float(_Property_7205E35B_Out_0, (_Property_916D8D52_Out_0.xxx), _Multiply_CF9DF1B5_Out_2);
                        float3 _Multiply_57D2E5C7_Out_2;
                        Unity_Multiply_float(_Multiply_CF9DF1B5_Out_2, float3(10, 10, 10), _Multiply_57D2E5C7_Out_2);
                        float3 _Add_F265DF09_Out_2;
                        Unity_Add_float3(_Property_65372844_Out_0, _Multiply_57D2E5C7_Out_2, _Add_F265DF09_Out_2);
                        float3 _Property_806C350F_Out_0 = Vector3_C96069F9;
                        float _Property_D017A08E_Out_0 = Vector1_A5EB719C;
                        float3 _Multiply_99498CF9_Out_2;
                        Unity_Multiply_float(_Property_806C350F_Out_0, (_Property_D017A08E_Out_0.xxx), _Multiply_99498CF9_Out_2);
                        float _Split_A5777330_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_A5777330_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_A5777330_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_A5777330_A_4 = 0;
                        float _Clamp_C4364CA5_Out_3;
                        Unity_Clamp_float(_Split_A5777330_G_2, 0, 1, _Clamp_C4364CA5_Out_3);
                        float3 _Multiply_ADC4C2A_Out_2;
                        Unity_Multiply_float(_Multiply_99498CF9_Out_2, (_Clamp_C4364CA5_Out_3.xxx), _Multiply_ADC4C2A_Out_2);
                        float3 _Multiply_49835441_Out_2;
                        Unity_Multiply_float(_Multiply_ADC4C2A_Out_2, float3(10, 10, 10), _Multiply_49835441_Out_2);
                        float3 _Add_B14AAE70_Out_2;
                        Unity_Add_float3(_Add_F265DF09_Out_2, _Multiply_49835441_Out_2, _Add_B14AAE70_Out_2);
                        WorldPosition_0 = _Add_B14AAE70_Out_2;
                    }
                
                    struct Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceTangent;
                        float3 WorldSpaceBiTangent;
                    };
                
                    void SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(float3 Vector3_AAF445D6, Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 IN, out float3 ObjectPosition_1)
                    {
                        float3 _Property_51DA8EE_Out_0 = Vector3_AAF445D6;
                        float3 _Subtract_B236C96B_Out_2;
                        Unity_Subtract_float3(_Property_51DA8EE_Out_0, _WorldSpaceCameraPos, _Subtract_B236C96B_Out_2);
                        float3 _Transform_6FDB2E47_Out_1 = TransformWorldToObject(_Subtract_B236C96B_Out_2.xyz);
                        ObjectPosition_1 = _Transform_6FDB2E47_Out_1;
                    }
                
                // Vertex Graph Evaluation
                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 _GetPivotInWorldSpace_73F19E42;
                        float3 _GetPivotInWorldSpace_73F19E42_PivotInWS_0;
                        SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(_GetPivotInWorldSpace_73F19E42, _GetPivotInWorldSpace_73F19E42_PivotInWS_0);
                        float _Split_64420219_R_1 = IN.VertexColor[0];
                        float _Split_64420219_G_2 = IN.VertexColor[1];
                        float _Split_64420219_B_3 = IN.VertexColor[2];
                        float _Split_64420219_A_4 = IN.VertexColor[3];
                        float3 _Lerp_4531CF63_Out_3;
                        Unity_Lerp_float3(_GetPivotInWorldSpace_73F19E42_PivotInWS_0, IN.AbsoluteWorldSpacePosition, (_Split_64420219_G_2.xxx), _Lerp_4531CF63_Out_3);
                        float4 _Property_D6662DC6_Out_0 = _GlobalWindDirectionAndStrength;
                        float4 _Property_9515B228_Out_0 = _WindDirectionAndStrength;
                        float4 _Property_9A1EF240_Out_0 = _GlobalShiver;
                        float4 _Property_777C8DB2_Out_0 = _Shiver;
                        Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 _GlobalWindParameters_B547F135;
                        float3 _GlobalWindParameters_B547F135_GustDirection_0;
                        float _GlobalWindParameters_B547F135_GustSpeed_1;
                        float _GlobalWindParameters_B547F135_GustStrength_2;
                        float _GlobalWindParameters_B547F135_ShiverSpeed_3;
                        float _GlobalWindParameters_B547F135_ShiverStrength_4;
                        SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(_Property_D6662DC6_Out_0, _Property_9515B228_Out_0, (_Property_9A1EF240_Out_0.xy), (_Property_777C8DB2_Out_0.xy), _GlobalWindParameters_B547F135, _GlobalWindParameters_B547F135_GustDirection_0, _GlobalWindParameters_B547F135_GustSpeed_1, _GlobalWindParameters_B547F135_GustStrength_2, _GlobalWindParameters_B547F135_ShiverSpeed_3, _GlobalWindParameters_B547F135_ShiverStrength_4);
                        float _Property_5F3A390D_Out_0 = _BAKEDMASK_ON;
                        float3 _Subtract_BF2A75CD_Out_2;
                        Unity_Subtract_float3(IN.AbsoluteWorldSpacePosition, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _Subtract_BF2A75CD_Out_2);
                        float _Split_29C81DE4_R_1 = _Subtract_BF2A75CD_Out_2[0];
                        float _Split_29C81DE4_G_2 = _Subtract_BF2A75CD_Out_2[1];
                        float _Split_29C81DE4_B_3 = _Subtract_BF2A75CD_Out_2[2];
                        float _Split_29C81DE4_A_4 = 0;
                        float _Add_6A47DB4F_Out_2;
                        Unity_Add_float(_Split_29C81DE4_R_1, _Split_29C81DE4_G_2, _Add_6A47DB4F_Out_2);
                        float _Add_EC455B5D_Out_2;
                        Unity_Add_float(_Add_6A47DB4F_Out_2, _Split_29C81DE4_B_3, _Add_EC455B5D_Out_2);
                        float _Multiply_F013BB8B_Out_2;
                        Unity_Multiply_float(_Add_EC455B5D_Out_2, 0.4, _Multiply_F013BB8B_Out_2);
                        float _Fraction_7D389816_Out_1;
                        Unity_Fraction_float(_Multiply_F013BB8B_Out_2, _Fraction_7D389816_Out_1);
                        float _Multiply_776D3DAF_Out_2;
                        Unity_Multiply_float(_Fraction_7D389816_Out_1, 0.15, _Multiply_776D3DAF_Out_2);
                        float _Split_E4BB9FEC_R_1 = IN.VertexColor[0];
                        float _Split_E4BB9FEC_G_2 = IN.VertexColor[1];
                        float _Split_E4BB9FEC_B_3 = IN.VertexColor[2];
                        float _Split_E4BB9FEC_A_4 = IN.VertexColor[3];
                        float _Multiply_BC8988C3_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, _Split_E4BB9FEC_G_2, _Multiply_BC8988C3_Out_2);
                        float _Multiply_EC5FE292_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_R_1, 0.3, _Multiply_EC5FE292_Out_2);
                        float _Add_A8423510_Out_2;
                        Unity_Add_float(_Multiply_BC8988C3_Out_2, _Multiply_EC5FE292_Out_2, _Add_A8423510_Out_2);
                        float _Add_CE74358C_Out_2;
                        Unity_Add_float(_Add_A8423510_Out_2, IN.TimeParameters.x, _Add_CE74358C_Out_2);
                        float _Multiply_1CE438D_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_G_2, 0.5, _Multiply_1CE438D_Out_2);
                        float _Add_8718B88C_Out_2;
                        Unity_Add_float(_Add_CE74358C_Out_2, _Multiply_1CE438D_Out_2, _Add_8718B88C_Out_2);
                        float _Property_DBA903E3_Out_0 = _UVMASK_ON;
                        float4 _UV_64D01E18_Out_0 = IN.uv0;
                        float _Split_A5DFBEBE_R_1 = _UV_64D01E18_Out_0[0];
                        float _Split_A5DFBEBE_G_2 = _UV_64D01E18_Out_0[1];
                        float _Split_A5DFBEBE_B_3 = _UV_64D01E18_Out_0[2];
                        float _Split_A5DFBEBE_A_4 = _UV_64D01E18_Out_0[3];
                        float _Multiply_C943DA5C_Out_2;
                        Unity_Multiply_float(_Split_A5DFBEBE_G_2, 0.1, _Multiply_C943DA5C_Out_2);
                        float _Branch_12012434_Out_3;
                        Unity_Branch_float(_Property_DBA903E3_Out_0, _Multiply_C943DA5C_Out_2, 0, _Branch_12012434_Out_3);
                        float _Add_922F2E64_Out_2;
                        Unity_Add_float(IN.TimeParameters.x, _Branch_12012434_Out_3, _Add_922F2E64_Out_2);
                        float _Multiply_2E689843_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, 0.5, _Multiply_2E689843_Out_2);
                        float _Add_ED1EE4DB_Out_2;
                        Unity_Add_float(_Add_922F2E64_Out_2, _Multiply_2E689843_Out_2, _Add_ED1EE4DB_Out_2);
                        float _Branch_291934CD_Out_3;
                        Unity_Branch_float(_Property_5F3A390D_Out_0, _Add_8718B88C_Out_2, _Add_ED1EE4DB_Out_2, _Branch_291934CD_Out_3);
                        float _Property_267CF497_Out_0 = _StiffnessVariation;
                        float _Property_4FB02E51_Out_0 = _BAKEDMASK_ON;
                        float4 _UV_6482E163_Out_0 = IN.uv1;
                        float _Split_2D1A67CF_R_1 = _UV_6482E163_Out_0[0];
                        float _Split_2D1A67CF_G_2 = _UV_6482E163_Out_0[1];
                        float _Split_2D1A67CF_B_3 = _UV_6482E163_Out_0[2];
                        float _Split_2D1A67CF_A_4 = _UV_6482E163_Out_0[3];
                        float _Multiply_F7BD1E76_Out_2;
                        Unity_Multiply_float(_Split_2D1A67CF_R_1, _Split_2D1A67CF_G_2, _Multiply_F7BD1E76_Out_2);
                        float _Property_B1FAFDBF_Out_0 = _UVMASK_ON;
                        float4 _UV_8F58F10B_Out_0 = IN.uv0;
                        float _Split_BD0858B3_R_1 = _UV_8F58F10B_Out_0[0];
                        float _Split_BD0858B3_G_2 = _UV_8F58F10B_Out_0[1];
                        float _Split_BD0858B3_B_3 = _UV_8F58F10B_Out_0[2];
                        float _Split_BD0858B3_A_4 = _UV_8F58F10B_Out_0[3];
                        float _Multiply_3FAD9403_Out_2;
                        Unity_Multiply_float(_Split_BD0858B3_G_2, 0.2, _Multiply_3FAD9403_Out_2);
                        float _Branch_3AF3832A_Out_3;
                        Unity_Branch_float(_Property_B1FAFDBF_Out_0, _Multiply_3FAD9403_Out_2, 1, _Branch_3AF3832A_Out_3);
                        float _Branch_F921E5A9_Out_3;
                        Unity_Branch_float(_Property_4FB02E51_Out_0, _Multiply_F7BD1E76_Out_2, _Branch_3AF3832A_Out_3, _Branch_F921E5A9_Out_3);
                        Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 _GetWindStrength_5806EF0A;
                        float _GetWindStrength_5806EF0A_GustStrength_0;
                        float _GetWindStrength_5806EF0A_ShiverStrength_1;
                        float3 _GetWindStrength_5806EF0A_ShiverDirection_2;
                        SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(_Lerp_4531CF63_Out_3, _GlobalWindParameters_B547F135_GustDirection_0, _Branch_291934CD_Out_3, _GlobalWindParameters_B547F135_GustSpeed_1, TEXTURE2D_ARGS(_GustNoise, sampler_GustNoise), _GustNoise_TexelSize, _GlobalWindParameters_B547F135_ShiverSpeed_3, TEXTURE2D_ARGS(_ShiverNoise, sampler_ShiverNoise), _ShiverNoise_TexelSize, _Property_267CF497_Out_0, 1, _Branch_F921E5A9_Out_3, _GlobalWindParameters_B547F135_GustStrength_2, 0.2, _GlobalWindParameters_B547F135_ShiverStrength_4, 0, _GetWindStrength_5806EF0A, _GetWindStrength_5806EF0A_GustStrength_0, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_ShiverDirection_2);
                        Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 _ApplyTreeWindDisplacement_8E73FF2E;
                        _ApplyTreeWindDisplacement_8E73FF2E.ObjectSpacePosition = IN.ObjectSpacePosition;
                        float3 _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0;
                        SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(_GetWindStrength_5806EF0A_ShiverDirection_2, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_GustStrength_0, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _GlobalWindParameters_B547F135_GustDirection_0, IN.AbsoluteWorldSpacePosition, _ApplyTreeWindDisplacement_8E73FF2E, _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0);
                        Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 _WorldToObject_628B231E;
                        _WorldToObject_628B231E.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _WorldToObject_628B231E.WorldSpaceTangent = IN.WorldSpaceTangent;
                        _WorldToObject_628B231E.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                        float3 _WorldToObject_628B231E_ObjectPosition_1;
                        SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(_ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0, _WorldToObject_628B231E, _WorldToObject_628B231E_ObjectPosition_1);
                        description.VertexPosition = _WorldToObject_628B231E_ObjectPosition_1;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }
                    
                // Pixel Graph Evaluation
                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_F86B9939_RGBA_0 = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, IN.uv0.xy);
                        float _SampleTexture2D_F86B9939_R_4 = _SampleTexture2D_F86B9939_RGBA_0.r;
                        float _SampleTexture2D_F86B9939_G_5 = _SampleTexture2D_F86B9939_RGBA_0.g;
                        float _SampleTexture2D_F86B9939_B_6 = _SampleTexture2D_F86B9939_RGBA_0.b;
                        float _SampleTexture2D_F86B9939_A_7 = _SampleTexture2D_F86B9939_RGBA_0.a;
                        float _Property_ABA23041_Out_0 = _AlphaClip;
                        surface.Alpha = _SampleTexture2D_F86B9939_A_7;
                        surface.AlphaClipThreshold = _Property_ABA23041_Out_0;
                        return surface;
                    }
                    
            //-------------------------------------------------------------------------------------
            // End graph generated code
            //-------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
            
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                // output.ViewSpaceNormal =             TransformWorldToViewDir(output.WorldSpaceNormal);
                // output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                output.ObjectSpacePosition =         input.positionOS;
                // output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                // output.ViewSpacePosition =           TransformWorldToView(output.WorldSpacePosition);
                // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                // output.WorldSpaceViewDirection =     GetWorldSpaceNormalizeViewDir(output.WorldSpacePosition);
                // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(output.WorldSpacePosition), _ProjectionParams.x);
                output.uv0 =                         input.uv0;
                output.uv1 =                         input.uv1;
                // output.uv2 =                         input.uv2;
                // output.uv3 =                         input.uv3;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            {
                // build graph inputs
                VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
                // Override time paramters with used one (This is required to correctly handle motion vector for vertex animation based on time)
                vertexDescriptionInputs.TimeParameters = timeParameters;
            
                // evaluate vertex graph
                VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
            
                // copy graph output to the results
                input.positionOS = vertexDescription.VertexPosition;
                input.normalOS = vertexDescription.VertexNormal;
                input.tangentOS.xyz = vertexDescription.VertexTangent;
            
                return input;
            }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
                FragInputs BuildFragInputs(VaryingsMeshToPS input)
                {
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.positionCS;       // input.positionCS is SV_Position
            
                    // output.positionRWS = input.positionRWS;
                    // output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
                    output.texCoord0 = input.texCoord0;
                    // output.texCoord1 = input.texCoord1;
                    // output.texCoord2 = input.texCoord2;
                    // output.texCoord3 = input.texCoord3;
                    // output.color = input.color;
                    #if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #elif SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #endif // SHADER_STAGE_FRAGMENT
            
                    return output;
                }
            
                SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                    // output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
                    // output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
                    // output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                    // output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
                    // output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
                    // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                    // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                    // output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
                    // output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
                    // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                    // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                    // output.WorldSpaceViewDirection =     normalize(viewWS);
                    // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                    // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                    // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                    // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                    // output.WorldSpacePosition =          input.positionRWS;
                    // output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
                    // output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
                    // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                    // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionRWS);
                    // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
                    output.uv0 =                         input.texCoord0;
                    // output.uv1 =                         input.texCoord1;
                    // output.uv2 =                         input.texCoord2;
                    // output.uv3 =                         input.texCoord3;
                    // output.VertexColor =                 input.color;
                    // output.FaceSign =                    input.isFrontFace;
                    // output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            
                    return output;
                }
            
                // existing HDRP code uses the combined function to go directly from packed to frag inputs
                FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    VaryingsMeshToPS unpacked= UnpackVaryingsMeshToPS(input);
                    return BuildFragInputs(unpacked);
                }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
            void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
            {
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                // copy across graph values, if defined
                // surfaceData.baseColor =                 surfaceDescription.Albedo;
                // surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                // surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                // surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                // surfaceData.metallic =                  surfaceDescription.Metallic;
                // surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                // surfaceData.thickness =                 surfaceDescription.Thickness;
                // surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                // surfaceData.specularColor =             surfaceDescription.Specular;
                // surfaceData.coatMask =                  surfaceDescription.CoatMask;
                // surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                // surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                // surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
        #ifdef _HAS_REFRACTION
                if (_EnableSSRefraction)
                {
                    // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                    // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                    // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                    surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                    surfaceDescription.Alpha = 1.0;
                }
                else
                {
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                    surfaceDescription.Alpha = 1.0;
                }
        #else
                surfaceData.ior = 1.0;
                surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                surfaceData.atDistance = 1.0;
                surfaceData.transmittanceMask = 0.0;
        #endif
                
                // These static material feature allow compile time optimization
                surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
        #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
        #endif
        #ifdef _MATERIAL_FEATURE_TRANSMISSION
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
        #endif
        #ifdef _MATERIAL_FEATURE_ANISOTROPY
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
        #endif
                // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
        #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
        #endif
        #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
        #endif
        
        #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                // Require to have setup baseColor
                // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                // tangent-space normal
                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                // normalTS = surfaceDescription.Normal;
        
                // compute world space normal
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                bentNormalWS = surfaceData.normalWS;
                // GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
                // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                // If user provide bent normal then we process a better term
        #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                // Just use the value passed through via the slot (not active otherwise)
        #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                // If we have bent normal and ambient occlusion, process a specular occlusion
                surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
        #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
        #else
                surfaceData.specularOcclusion = 1.0;
        #endif
        
        #if HAVE_DECALS
                if (_EnableDecals)
                {
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                }
        #endif
        
        #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
        #endif
        
        #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    // TODO: need to update mip info
                    surfaceData.metallic = 0;
                }
        
                // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                // as it can modify attribute use for static lighting
                ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
        #endif
            }
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
                
                // ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
        
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal 
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                // override sampleBakedGI:
                // builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
                // builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
        
                // builtinData.emissiveColor = surfaceDescription.Emission;
        
                // builtinData.depthOffset = surfaceDescription.DepthOffset;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
            //-------------------------------------------------------------------------------------
            // Pass Includes
            //-------------------------------------------------------------------------------------
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
            //-------------------------------------------------------------------------------------
            // End Pass Includes
            //-------------------------------------------------------------------------------------
        
            ENDHLSL
        }
        
        Pass
        {
            // based on HDLitPass.template
            Name "META"
            Tags { "LightMode" = "META" }
        
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull Off
        
            
            
            
            
            
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        
            #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            // #define _ENABLE_FOG_ON_TRANSPARENT 1
            #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            #define _DISABLE_DECALS 1
            #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        
            //-------------------------------------------------------------------------------------
            // End Variant Definitions
            //-------------------------------------------------------------------------------------
        
            #pragma vertex Vert
            #pragma fragment Frag
        
            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                    #define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
                #define RAYTRACING_SHADER_GRAPH_HIGH
                // ACTIVE FIELDS:
                //   DoubleSided
                //   DoubleSided.Flip
                //   FragInputs.isFrontFace
                //   Material.Translucent
                //   Material.Transmission
                //   AlphaTest
                //   DisableDecals
                //   DisableSSR
                //   Specular.EnergyConserving
                //   AmbientOcclusion
                //   SurfaceDescriptionInputs.TangentSpaceNormal
                //   SurfaceDescriptionInputs.uv0
                //   VertexDescriptionInputs.ObjectSpaceNormal
                //   VertexDescriptionInputs.ObjectSpaceTangent
                //   SurfaceDescription.Albedo
                //   SurfaceDescription.Normal
                //   SurfaceDescription.BentNormal
                //   SurfaceDescription.Thickness
                //   SurfaceDescription.DiffusionProfileHash
                //   SurfaceDescription.CoatMask
                //   SurfaceDescription.Emission
                //   SurfaceDescription.Smoothness
                //   SurfaceDescription.Occlusion
                //   SurfaceDescription.Alpha
                //   SurfaceDescription.AlphaClipThreshold
                //   features.modifyMesh
                //   AttributesMesh.normalOS
                //   AttributesMesh.tangentOS
                //   AttributesMesh.uv0
                //   AttributesMesh.uv1
                //   AttributesMesh.color
                //   AttributesMesh.uv2
                //   VaryingsMeshToPS.cullFace
                //   FragInputs.texCoord0
                //   VaryingsMeshToPS.texCoord0
                // Shared Graph Keywords
        
            // this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            // #define ATTRIBUTES_NEED_TEXCOORD3
            #define ATTRIBUTES_NEED_COLOR
            // #define VARYINGS_NEED_POSITION_WS
            // #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            // #define VARYINGS_NEED_TEXCOORD1
            // #define VARYINGS_NEED_TEXCOORD2
            // #define VARYINGS_NEED_TEXCOORD3
            // #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_CULLFACE
            #define HAVE_MESH_MODIFICATION
        
        // We need isFontFace when using double sided
        #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
            #define VARYINGS_NEED_CULLFACE
        #endif
        
            //-------------------------------------------------------------------------------------
            // End Defines
            //-------------------------------------------------------------------------------------
        	
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
            //-------------------------------------------------------------------------------------
            // Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
            // Generated Type: AttributesMesh
            struct AttributesMesh
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL; // optional
                float4 tangentOS : TANGENT; // optional
                float4 uv0 : TEXCOORD0; // optional
                float4 uv1 : TEXCOORD1; // optional
                float4 uv2 : TEXCOORD2; // optional
                float4 color : COLOR; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            // Generated Type: VaryingsMeshToPS
            struct VaryingsMeshToPS
            {
                float4 positionCS : SV_Position;
                float4 texCoord0; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            };
            
            // Generated Type: PackedVaryingsMeshToPS
            struct PackedVaryingsMeshToPS
            {
                float4 positionCS : SV_Position; // unpacked
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float4 interp00 : TEXCOORD0; // auto-packed
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
                #endif // conditional
            };
            
            // Packed Type: VaryingsMeshToPS
            PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
            {
                PackedVaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToPS
            VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
            {
                VaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            // Generated Type: VaryingsMeshToDS
            struct VaryingsMeshToDS
            {
                float3 positionRWS;
                float3 normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            
            // Generated Type: PackedVaryingsMeshToDS
            struct PackedVaryingsMeshToDS
            {
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
            };
            
            // Packed Type: VaryingsMeshToDS
            PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
            {
                PackedVaryingsMeshToDS output;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToDS
            VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
            {
                VaryingsMeshToDS output;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            //-------------------------------------------------------------------------------------
            // End Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
        
            //-------------------------------------------------------------------------------------
            // Graph generated code
            //-------------------------------------------------------------------------------------
                    // Shared Graph Properties (uniform inputs)
                    CBUFFER_START(UnityPerMaterial)
                    float _AlphaClip;
                    float _Hue;
                    float _Saturation;
                    float _Lightness;
                    float _StiffnessVariation;
                    float4 _WindDirectionAndStrength;
                    float4 _Shiver;
                    float _BAKEDMASK_ON;
                    float _UVMASK_ON;
                    float _VERTEXPOSITIONMASK_ON;
                    float4 _EmissionColor;
                    float _RenderQueueType;
                    float _StencilRef;
                    float _StencilWriteMask;
                    float _StencilRefDepth;
                    float _StencilWriteMaskDepth;
                    float _StencilRefMV;
                    float _StencilWriteMaskMV;
                    float _StencilRefDistortionVec;
                    float _StencilWriteMaskDistortionVec;
                    float _StencilWriteMaskGBuffer;
                    float _StencilRefGBuffer;
                    float _ZTestGBuffer;
                    float _RequireSplitLighting;
                    float _ReceivesSSR;
                    float _SurfaceType;
                    float _BlendMode;
                    float _SrcBlend;
                    float _DstBlend;
                    float _AlphaSrcBlend;
                    float _AlphaDstBlend;
                    float _ZWrite;
                    float _CullMode;
                    float _TransparentSortPriority;
                    float _CullModeForward;
                    float _TransparentCullMode;
                    float _ZTestDepthEqualForOpaque;
                    float _ZTestTransparent;
                    float _TransparentBackfaceEnable;
                    float _AlphaCutoffEnable;
                    float _AlphaCutoff;
                    float _UseShadowThreshold;
                    float _DoubleSidedEnable;
                    float _DoubleSidedNormalMode;
                    float4 _DoubleSidedConstants;
                    float _DiffusionProfileHash;
                    float4 _DiffusionProfileAsset;
                    CBUFFER_END
                    TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo); float4 _Albedo_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap); float4 _MaskMap_TexelSize;
                    TEXTURE2D(_ThicknessMap); SAMPLER(sampler_ThicknessMap); float4 _ThicknessMap_TexelSize;
                    float4 _GlobalWindDirectionAndStrength;
                    float4 _GlobalShiver;
                    TEXTURE2D(_ShiverNoise); SAMPLER(sampler_ShiverNoise); float4 _ShiverNoise_TexelSize;
                    TEXTURE2D(_GustNoise); SAMPLER(sampler_GustNoise); float4 _GustNoise_TexelSize;
                    SAMPLER(_SampleTexture2D_F86B9939_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_12F932C1_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_E3683686_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_FFEA8409_Sampler_3_Linear_Repeat);
                
                // Vertex Graph Inputs
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal; // optional
                        float3 ObjectSpaceTangent; // optional
                    };
                // Vertex Graph Outputs
                    struct VertexDescription
                    {
                    };
                    
                // Pixel Graph Inputs
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal; // optional
                        float4 uv0; // optional
                    };
                // Pixel Graph Outputs
                    struct SurfaceDescription
                    {
                        float3 Albedo;
                        float3 Normal;
                        float3 BentNormal;
                        float Thickness;
                        float DiffusionProfileHash;
                        float CoatMask;
                        float3 Emission;
                        float Smoothness;
                        float Occlusion;
                        float Alpha;
                        float AlphaClipThreshold;
                    };
                    
                // Shared Graph Node Functions
                
                    void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
                    {
                        // RGB to HSV
                        float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                        float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                        float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                        float D = Q.x - min(Q.w, Q.y);
                        float E = 1e-4;
                        float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);
                
                        float hue = hsv.x + Offset;
                        hsv.x = (hue < 0)
                                ? hue + 1
                                : (hue > 1)
                                    ? hue - 1
                                    : hue;
                
                        // HSV to RGB
                        float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                        float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
                        Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
                    }
                
                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
                    {
                        float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
                        Out =  luma.xxx + Saturation.xxx * (In - luma.xxx);
                    }
                
                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }
                
                // Vertex Graph Evaluation
                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        return description;
                    }
                    
                // Pixel Graph Evaluation
                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_F86B9939_RGBA_0 = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, IN.uv0.xy);
                        float _SampleTexture2D_F86B9939_R_4 = _SampleTexture2D_F86B9939_RGBA_0.r;
                        float _SampleTexture2D_F86B9939_G_5 = _SampleTexture2D_F86B9939_RGBA_0.g;
                        float _SampleTexture2D_F86B9939_B_6 = _SampleTexture2D_F86B9939_RGBA_0.b;
                        float _SampleTexture2D_F86B9939_A_7 = _SampleTexture2D_F86B9939_RGBA_0.a;
                        float _Property_667D0001_Out_0 = _Hue;
                        float3 _Hue_BE270ED0_Out_2;
                        Unity_Hue_Normalized_float((_SampleTexture2D_F86B9939_RGBA_0.xyz), _Property_667D0001_Out_0, _Hue_BE270ED0_Out_2);
                        float _Property_306B4B17_Out_0 = _Saturation;
                        float _Add_27F91AF7_Out_2;
                        Unity_Add_float(_Property_306B4B17_Out_0, 1, _Add_27F91AF7_Out_2);
                        float3 _Saturation_8EFFDFE8_Out_2;
                        Unity_Saturation_float(_Hue_BE270ED0_Out_2, _Add_27F91AF7_Out_2, _Saturation_8EFFDFE8_Out_2);
                        float _Property_35742C6B_Out_0 = _Lightness;
                        float3 _Add_53649F0F_Out_2;
                        Unity_Add_float3(_Saturation_8EFFDFE8_Out_2, (_Property_35742C6B_Out_0.xxx), _Add_53649F0F_Out_2);
                        float4 _SampleTexture2D_12F932C1_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                        _SampleTexture2D_12F932C1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_12F932C1_RGBA_0);
                        float _SampleTexture2D_12F932C1_R_4 = _SampleTexture2D_12F932C1_RGBA_0.r;
                        float _SampleTexture2D_12F932C1_G_5 = _SampleTexture2D_12F932C1_RGBA_0.g;
                        float _SampleTexture2D_12F932C1_B_6 = _SampleTexture2D_12F932C1_RGBA_0.b;
                        float _SampleTexture2D_12F932C1_A_7 = _SampleTexture2D_12F932C1_RGBA_0.a;
                        float4 _SampleTexture2D_E3683686_RGBA_0 = SAMPLE_TEXTURE2D(_ThicknessMap, sampler_ThicknessMap, IN.uv0.xy);
                        float _SampleTexture2D_E3683686_R_4 = _SampleTexture2D_E3683686_RGBA_0.r;
                        float _SampleTexture2D_E3683686_G_5 = _SampleTexture2D_E3683686_RGBA_0.g;
                        float _SampleTexture2D_E3683686_B_6 = _SampleTexture2D_E3683686_RGBA_0.b;
                        float _SampleTexture2D_E3683686_A_7 = _SampleTexture2D_E3683686_RGBA_0.a;
                        float4 _SampleTexture2D_FFEA8409_RGBA_0 = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, IN.uv0.xy);
                        float _SampleTexture2D_FFEA8409_R_4 = _SampleTexture2D_FFEA8409_RGBA_0.r;
                        float _SampleTexture2D_FFEA8409_G_5 = _SampleTexture2D_FFEA8409_RGBA_0.g;
                        float _SampleTexture2D_FFEA8409_B_6 = _SampleTexture2D_FFEA8409_RGBA_0.b;
                        float _SampleTexture2D_FFEA8409_A_7 = _SampleTexture2D_FFEA8409_RGBA_0.a;
                        float _Property_ABA23041_Out_0 = _AlphaClip;
                        surface.Albedo = _Add_53649F0F_Out_2;
                        surface.Normal = (_SampleTexture2D_12F932C1_RGBA_0.xyz);
                        surface.BentNormal = IN.TangentSpaceNormal;
                        surface.Thickness = _SampleTexture2D_E3683686_R_4;
                        surface.DiffusionProfileHash = _DiffusionProfileHash;
                        surface.CoatMask = 0;
                        surface.Emission = float3(0, 0, 0);
                        surface.Smoothness = _SampleTexture2D_FFEA8409_A_7;
                        surface.Occlusion = _SampleTexture2D_FFEA8409_G_5;
                        surface.Alpha = _SampleTexture2D_F86B9939_A_7;
                        surface.AlphaClipThreshold = _Property_ABA23041_Out_0;
                        return surface;
                    }
                    
            //-------------------------------------------------------------------------------------
            // End graph generated code
            //-------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
            
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                // output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                // output.ViewSpaceNormal =             TransformWorldToViewDir(output.WorldSpaceNormal);
                // output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                output.ObjectSpaceTangent =          input.tangentOS;
                // output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                // output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                // output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                // output.ObjectSpacePosition =         input.positionOS;
                // output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                // output.ViewSpacePosition =           TransformWorldToView(output.WorldSpacePosition);
                // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                // output.WorldSpaceViewDirection =     GetWorldSpaceNormalizeViewDir(output.WorldSpacePosition);
                // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(output.WorldSpacePosition), _ProjectionParams.x);
                // output.uv0 =                         input.uv0;
                // output.uv1 =                         input.uv1;
                // output.uv2 =                         input.uv2;
                // output.uv3 =                         input.uv3;
                // output.VertexColor =                 input.color;
            
                return output;
            }
            
            AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            {
                // build graph inputs
                VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
                // Override time paramters with used one (This is required to correctly handle motion vector for vertex animation based on time)
                // vertexDescriptionInputs.TimeParameters = timeParameters;
            
                // evaluate vertex graph
                VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
            
                // copy graph output to the results
                // input.positionOS = vertexDescription.VertexPosition;
                // input.normalOS = vertexDescription.VertexNormal;
                // input.tangentOS.xyz = vertexDescription.VertexTangent;
            
                return input;
            }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
                FragInputs BuildFragInputs(VaryingsMeshToPS input)
                {
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.positionCS;       // input.positionCS is SV_Position
            
                    // output.positionRWS = input.positionRWS;
                    // output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
                    output.texCoord0 = input.texCoord0;
                    // output.texCoord1 = input.texCoord1;
                    // output.texCoord2 = input.texCoord2;
                    // output.texCoord3 = input.texCoord3;
                    // output.color = input.color;
                    #if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #elif SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #endif // SHADER_STAGE_FRAGMENT
            
                    return output;
                }
            
                SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                    // output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
                    // output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
                    // output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                    // output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
                    // output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
                    // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                    // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                    // output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
                    // output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
                    // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                    // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                    // output.WorldSpaceViewDirection =     normalize(viewWS);
                    // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                    // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                    // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                    // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                    // output.WorldSpacePosition =          input.positionRWS;
                    // output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
                    // output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
                    // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                    // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionRWS);
                    // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
                    output.uv0 =                         input.texCoord0;
                    // output.uv1 =                         input.texCoord1;
                    // output.uv2 =                         input.texCoord2;
                    // output.uv3 =                         input.texCoord3;
                    // output.VertexColor =                 input.color;
                    // output.FaceSign =                    input.isFrontFace;
                    // output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            
                    return output;
                }
            
                // existing HDRP code uses the combined function to go directly from packed to frag inputs
                FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    VaryingsMeshToPS unpacked= UnpackVaryingsMeshToPS(input);
                    return BuildFragInputs(unpacked);
                }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
            void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
            {
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                // copy across graph values, if defined
                surfaceData.baseColor =                 surfaceDescription.Albedo;
                surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                // surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                // surfaceData.metallic =                  surfaceDescription.Metallic;
                // surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                surfaceData.thickness =                 surfaceDescription.Thickness;
                surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                // surfaceData.specularColor =             surfaceDescription.Specular;
                surfaceData.coatMask =                  surfaceDescription.CoatMask;
                // surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                // surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                // surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
        #ifdef _HAS_REFRACTION
                if (_EnableSSRefraction)
                {
                    // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                    // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                    // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                    surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                    surfaceDescription.Alpha = 1.0;
                }
                else
                {
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                    surfaceDescription.Alpha = 1.0;
                }
        #else
                surfaceData.ior = 1.0;
                surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                surfaceData.atDistance = 1.0;
                surfaceData.transmittanceMask = 0.0;
        #endif
                
                // These static material feature allow compile time optimization
                surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
        #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
        #endif
        #ifdef _MATERIAL_FEATURE_TRANSMISSION
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
        #endif
        #ifdef _MATERIAL_FEATURE_ANISOTROPY
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
        #endif
                // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
        #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
        #endif
        #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
        #endif
        
        #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                // Require to have setup baseColor
                // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                // tangent-space normal
                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                // compute world space normal
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                bentNormalWS = surfaceData.normalWS;
                // GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
                // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                // If user provide bent normal then we process a better term
        #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                // Just use the value passed through via the slot (not active otherwise)
        #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                // If we have bent normal and ambient occlusion, process a specular occlusion
                surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
        #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
        #else
                surfaceData.specularOcclusion = 1.0;
        #endif
        
        #if HAVE_DECALS
                if (_EnableDecals)
                {
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                }
        #endif
        
        #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
        #endif
        
        #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    // TODO: need to update mip info
                    surfaceData.metallic = 0;
                }
        
                // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                // as it can modify attribute use for static lighting
                ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
        #endif
            }
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
                
                // ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
        
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal 
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                // override sampleBakedGI:
                // builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
                // builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // builtinData.depthOffset = surfaceDescription.DepthOffset;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
            //-------------------------------------------------------------------------------------
            // Pass Includes
            //-------------------------------------------------------------------------------------
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassLightTransport.hlsl"
            //-------------------------------------------------------------------------------------
            // End Pass Includes
            //-------------------------------------------------------------------------------------
        
            ENDHLSL
        }
        
        Pass
        {
            // based on HDLitPass.template
            Name "SceneSelectionPass"
            Tags { "LightMode" = "SceneSelectionPass" }
        
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            
            
            
            
            
            ColorMask 0
        
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        
            #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            // #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            #define _DISABLE_DECALS 1
            #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        
            //-------------------------------------------------------------------------------------
            // End Variant Definitions
            //-------------------------------------------------------------------------------------
        
            #pragma vertex Vert
            #pragma fragment Frag
        
            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                    #define SHADERPASS SHADERPASS_DEPTH_ONLY
                #define SCENESELECTIONPASS
                #pragma editor_sync_compilation
                #define RAYTRACING_SHADER_GRAPH_HIGH
                // ACTIVE FIELDS:
                //   DoubleSided
                //   DoubleSided.Flip
                //   FragInputs.isFrontFace
                //   Material.Translucent
                //   Material.Transmission
                //   AlphaTest
                //   DisableDecals
                //   DisableSSR
                //   Specular.EnergyConserving
                //   SurfaceDescriptionInputs.TangentSpaceNormal
                //   SurfaceDescriptionInputs.uv0
                //   VertexDescriptionInputs.VertexColor
                //   VertexDescriptionInputs.ObjectSpaceNormal
                //   VertexDescriptionInputs.WorldSpaceNormal
                //   VertexDescriptionInputs.ObjectSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceBiTangent
                //   VertexDescriptionInputs.ObjectSpacePosition
                //   VertexDescriptionInputs.AbsoluteWorldSpacePosition
                //   VertexDescriptionInputs.uv0
                //   VertexDescriptionInputs.uv1
                //   VertexDescriptionInputs.TimeParameters
                //   SurfaceDescription.Alpha
                //   SurfaceDescription.AlphaClipThreshold
                //   features.modifyMesh
                //   VertexDescription.VertexPosition
                //   VertexDescription.VertexNormal
                //   VertexDescription.VertexTangent
                //   VaryingsMeshToPS.cullFace
                //   FragInputs.texCoord0
                //   AttributesMesh.color
                //   AttributesMesh.normalOS
                //   AttributesMesh.tangentOS
                //   VertexDescriptionInputs.ObjectSpaceBiTangent
                //   AttributesMesh.positionOS
                //   AttributesMesh.uv0
                //   AttributesMesh.uv1
                //   VaryingsMeshToPS.texCoord0
                // Shared Graph Keywords
        
            // this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            // #define ATTRIBUTES_NEED_TEXCOORD2
            // #define ATTRIBUTES_NEED_TEXCOORD3
            #define ATTRIBUTES_NEED_COLOR
            // #define VARYINGS_NEED_POSITION_WS
            // #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            // #define VARYINGS_NEED_TEXCOORD1
            // #define VARYINGS_NEED_TEXCOORD2
            // #define VARYINGS_NEED_TEXCOORD3
            // #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_CULLFACE
            #define HAVE_MESH_MODIFICATION
        
        // We need isFontFace when using double sided
        #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
            #define VARYINGS_NEED_CULLFACE
        #endif
        
            //-------------------------------------------------------------------------------------
            // End Defines
            //-------------------------------------------------------------------------------------
        	
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
            //-------------------------------------------------------------------------------------
            // Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
            // Generated Type: AttributesMesh
            struct AttributesMesh
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL; // optional
                float4 tangentOS : TANGENT; // optional
                float4 uv0 : TEXCOORD0; // optional
                float4 uv1 : TEXCOORD1; // optional
                float4 color : COLOR; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            // Generated Type: VaryingsMeshToPS
            struct VaryingsMeshToPS
            {
                float4 positionCS : SV_Position;
                float4 texCoord0; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            };
            
            // Generated Type: PackedVaryingsMeshToPS
            struct PackedVaryingsMeshToPS
            {
                float4 positionCS : SV_Position; // unpacked
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float4 interp00 : TEXCOORD0; // auto-packed
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
                #endif // conditional
            };
            
            // Packed Type: VaryingsMeshToPS
            PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
            {
                PackedVaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToPS
            VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
            {
                VaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            // Generated Type: VaryingsMeshToDS
            struct VaryingsMeshToDS
            {
                float3 positionRWS;
                float3 normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            
            // Generated Type: PackedVaryingsMeshToDS
            struct PackedVaryingsMeshToDS
            {
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
            };
            
            // Packed Type: VaryingsMeshToDS
            PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
            {
                PackedVaryingsMeshToDS output;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToDS
            VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
            {
                VaryingsMeshToDS output;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            //-------------------------------------------------------------------------------------
            // End Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
        
            //-------------------------------------------------------------------------------------
            // Graph generated code
            //-------------------------------------------------------------------------------------
                    // Shared Graph Properties (uniform inputs)
                    CBUFFER_START(UnityPerMaterial)
                    float _AlphaClip;
                    float _Hue;
                    float _Saturation;
                    float _Lightness;
                    float _StiffnessVariation;
                    float4 _WindDirectionAndStrength;
                    float4 _Shiver;
                    float _BAKEDMASK_ON;
                    float _UVMASK_ON;
                    float _VERTEXPOSITIONMASK_ON;
                    float4 _EmissionColor;
                    float _RenderQueueType;
                    float _StencilRef;
                    float _StencilWriteMask;
                    float _StencilRefDepth;
                    float _StencilWriteMaskDepth;
                    float _StencilRefMV;
                    float _StencilWriteMaskMV;
                    float _StencilRefDistortionVec;
                    float _StencilWriteMaskDistortionVec;
                    float _StencilWriteMaskGBuffer;
                    float _StencilRefGBuffer;
                    float _ZTestGBuffer;
                    float _RequireSplitLighting;
                    float _ReceivesSSR;
                    float _SurfaceType;
                    float _BlendMode;
                    float _SrcBlend;
                    float _DstBlend;
                    float _AlphaSrcBlend;
                    float _AlphaDstBlend;
                    float _ZWrite;
                    float _CullMode;
                    float _TransparentSortPriority;
                    float _CullModeForward;
                    float _TransparentCullMode;
                    float _ZTestDepthEqualForOpaque;
                    float _ZTestTransparent;
                    float _TransparentBackfaceEnable;
                    float _AlphaCutoffEnable;
                    float _AlphaCutoff;
                    float _UseShadowThreshold;
                    float _DoubleSidedEnable;
                    float _DoubleSidedNormalMode;
                    float4 _DoubleSidedConstants;
                    float _DiffusionProfileHash;
                    float4 _DiffusionProfileAsset;
                    CBUFFER_END
                    TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo); float4 _Albedo_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap); float4 _MaskMap_TexelSize;
                    TEXTURE2D(_ThicknessMap); SAMPLER(sampler_ThicknessMap); float4 _ThicknessMap_TexelSize;
                    float4 _GlobalWindDirectionAndStrength;
                    float4 _GlobalShiver;
                    TEXTURE2D(_ShiverNoise); SAMPLER(sampler_ShiverNoise); float4 _ShiverNoise_TexelSize;
                    TEXTURE2D(_GustNoise); SAMPLER(sampler_GustNoise); float4 _GustNoise_TexelSize;
                    SAMPLER(_SampleTexture2D_F86B9939_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_46D09289_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_DBCD6404_Sampler_3_Linear_Repeat);
                
                // Vertex Graph Inputs
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal; // optional
                        float3 WorldSpaceNormal; // optional
                        float3 ObjectSpaceTangent; // optional
                        float3 WorldSpaceTangent; // optional
                        float3 ObjectSpaceBiTangent; // optional
                        float3 WorldSpaceBiTangent; // optional
                        float3 ObjectSpacePosition; // optional
                        float3 AbsoluteWorldSpacePosition; // optional
                        float4 uv0; // optional
                        float4 uv1; // optional
                        float4 VertexColor; // optional
                        float3 TimeParameters; // optional
                    };
                // Vertex Graph Outputs
                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };
                    
                // Pixel Graph Inputs
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal; // optional
                        float4 uv0; // optional
                    };
                // Pixel Graph Outputs
                    struct SurfaceDescription
                    {
                        float Alpha;
                        float AlphaClipThreshold;
                    };
                    
                // Shared Graph Node Functions
                
                    struct Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238
                    {
                    };
                
                    void SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 IN, out float3 PivotInWS_0)
                    {
                        PivotInWS_0 = SHADERGRAPH_OBJECT_POSITION;
                    }
                
                    void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                    {
                        Out = lerp(A, B, T);
                    }
                
                    void Unity_Multiply_float (float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                    {
                        RGBA = float4(R, G, B, A);
                        RGB = float3(R, G, B);
                        RG = float2(R, G);
                    }
                
                    void Unity_Length_float3(float3 In, out float Out)
                    {
                        Out = length(In);
                    }
                
                    void Unity_Multiply_float (float A, float B, out float Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }
                
                    void Unity_Normalize_float3(float3 In, out float3 Out)
                    {
                        Out = normalize(In);
                    }
                
                    void Unity_Maximum_float(float A, float B, out float Out)
                    {
                        Out = max(A, B);
                    }
                
                    void Unity_Multiply_float (float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Maximum_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = max(A, B);
                    }
                
                    struct Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7
                    {
                    };
                
                    void SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(float4 Vector4_14B5A446, float4 Vector4_6887180D, float2 Vector2_F270B07E, float2 Vector2_70BD0D1B, Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 IN, out float3 GustDirection_0, out float GustSpeed_1, out float GustStrength_2, out float ShiverSpeed_3, out float ShiverStrength_4)
                    {
                        float3 _Vector3_E24D7903_Out_0 = float3(0.7, 0, 0.3);
                        float4 _Property_95651D48_Out_0 = Vector4_14B5A446;
                        float4 _Property_FFEF34C6_Out_0 = Vector4_6887180D;
                        float4 _Multiply_7F93D556_Out_2;
                        Unity_Multiply_float(_Property_95651D48_Out_0, _Property_FFEF34C6_Out_0, _Multiply_7F93D556_Out_2);
                        float _Split_1A6C2849_R_1 = _Multiply_7F93D556_Out_2[0];
                        float _Split_1A6C2849_G_2 = _Multiply_7F93D556_Out_2[1];
                        float _Split_1A6C2849_B_3 = _Multiply_7F93D556_Out_2[2];
                        float _Split_1A6C2849_A_4 = _Multiply_7F93D556_Out_2[3];
                        float4 _Combine_769EB158_RGBA_4;
                        float3 _Combine_769EB158_RGB_5;
                        float2 _Combine_769EB158_RG_6;
                        Unity_Combine_float(_Split_1A6C2849_R_1, 0, _Split_1A6C2849_G_2, 0, _Combine_769EB158_RGBA_4, _Combine_769EB158_RGB_5, _Combine_769EB158_RG_6);
                        float _Length_62815FED_Out_1;
                        Unity_Length_float3(_Combine_769EB158_RGB_5, _Length_62815FED_Out_1);
                        float _Multiply_A4A39D4F_Out_2;
                        Unity_Multiply_float(_Length_62815FED_Out_1, 1000, _Multiply_A4A39D4F_Out_2);
                        float _Clamp_4B28219D_Out_3;
                        Unity_Clamp_float(_Multiply_A4A39D4F_Out_2, 0, 1, _Clamp_4B28219D_Out_3);
                        float3 _Lerp_66854A50_Out_3;
                        Unity_Lerp_float3(_Vector3_E24D7903_Out_0, _Combine_769EB158_RGB_5, (_Clamp_4B28219D_Out_3.xxx), _Lerp_66854A50_Out_3);
                        float3 _Normalize_B2778668_Out_1;
                        Unity_Normalize_float3(_Lerp_66854A50_Out_3, _Normalize_B2778668_Out_1);
                        float _Maximum_A3AFA1AB_Out_2;
                        Unity_Maximum_float(_Split_1A6C2849_B_3, 0.01, _Maximum_A3AFA1AB_Out_2);
                        float _Maximum_FCE0058_Out_2;
                        Unity_Maximum_float(0, _Split_1A6C2849_A_4, _Maximum_FCE0058_Out_2);
                        float2 _Property_F062BDE_Out_0 = Vector2_F270B07E;
                        float2 _Property_FB73C895_Out_0 = Vector2_70BD0D1B;
                        float2 _Multiply_76AC0593_Out_2;
                        Unity_Multiply_float(_Property_F062BDE_Out_0, _Property_FB73C895_Out_0, _Multiply_76AC0593_Out_2);
                        float2 _Maximum_E318FF04_Out_2;
                        Unity_Maximum_float2(_Multiply_76AC0593_Out_2, float2(0.01, 0.01), _Maximum_E318FF04_Out_2);
                        float _Split_F437A5E0_R_1 = _Maximum_E318FF04_Out_2[0];
                        float _Split_F437A5E0_G_2 = _Maximum_E318FF04_Out_2[1];
                        float _Split_F437A5E0_B_3 = 0;
                        float _Split_F437A5E0_A_4 = 0;
                        GustDirection_0 = _Normalize_B2778668_Out_1;
                        GustSpeed_1 = _Maximum_A3AFA1AB_Out_2;
                        GustStrength_2 = _Maximum_FCE0058_Out_2;
                        ShiverSpeed_3 = _Split_F437A5E0_R_1;
                        ShiverStrength_4 = _Split_F437A5E0_G_2;
                    }
                
                    void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A - B;
                    }
                
                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Fraction_float(float In, out float Out)
                    {
                        Out = frac(In);
                    }
                
                    void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                    {
                        Out = lerp(False, True, Predicate);
                    }
                
                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A - B;
                    }
                
                    struct Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f
                    {
                    };
                
                    void SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(float Vector1_CCF53CDA, float Vector1_D95E40FE, float2 Vector2_AEE18C41, float2 Vector2_A9CE092C, float Vector1_F2ED6CCC, TEXTURE2D_PARAM(Texture2D_F14459DD, samplerTexture2D_F14459DD), float4 Texture2D_F14459DD_TexelSize, Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f IN, out float GustNoise_0)
                    {
                        float2 _Property_A92CC1B7_Out_0 = Vector2_AEE18C41;
                        float _Property_36B40CE_Out_0 = Vector1_D95E40FE;
                        float _Multiply_9E28D3C4_Out_2;
                        Unity_Multiply_float(_Property_36B40CE_Out_0, 2, _Multiply_9E28D3C4_Out_2);
                        float2 _Add_C54F05FE_Out_2;
                        Unity_Add_float2(_Property_A92CC1B7_Out_0, (_Multiply_9E28D3C4_Out_2.xx), _Add_C54F05FE_Out_2);
                        float2 _Multiply_9CD1691E_Out_2;
                        Unity_Multiply_float(_Add_C54F05FE_Out_2, float2(0.01, 0.01), _Multiply_9CD1691E_Out_2);
                        float2 _Property_D05D9ECB_Out_0 = Vector2_A9CE092C;
                        float _Property_8BFC9AA2_Out_0 = Vector1_CCF53CDA;
                        float2 _Multiply_462DF694_Out_2;
                        Unity_Multiply_float(_Property_D05D9ECB_Out_0, (_Property_8BFC9AA2_Out_0.xx), _Multiply_462DF694_Out_2);
                        float _Property_4DB65C54_Out_0 = Vector1_F2ED6CCC;
                        float2 _Multiply_50FD4B48_Out_2;
                        Unity_Multiply_float(_Multiply_462DF694_Out_2, (_Property_4DB65C54_Out_0.xx), _Multiply_50FD4B48_Out_2);
                        float2 _Subtract_B4A749C2_Out_2;
                        Unity_Subtract_float2(_Multiply_9CD1691E_Out_2, _Multiply_50FD4B48_Out_2, _Subtract_B4A749C2_Out_2);
                        float4 _SampleTexture2DLOD_46D09289_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_F14459DD, samplerTexture2D_F14459DD, _Subtract_B4A749C2_Out_2, 0);
                        float _SampleTexture2DLOD_46D09289_R_5 = _SampleTexture2DLOD_46D09289_RGBA_0.r;
                        float _SampleTexture2DLOD_46D09289_G_6 = _SampleTexture2DLOD_46D09289_RGBA_0.g;
                        float _SampleTexture2DLOD_46D09289_B_7 = _SampleTexture2DLOD_46D09289_RGBA_0.b;
                        float _SampleTexture2DLOD_46D09289_A_8 = _SampleTexture2DLOD_46D09289_RGBA_0.a;
                        GustNoise_0 = _SampleTexture2DLOD_46D09289_R_5;
                    }
                
                    void Unity_Power_float(float A, float B, out float Out)
                    {
                        Out = pow(A, B);
                    }
                
                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }
                
                    struct Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19
                    {
                    };
                
                    void SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(float2 Vector2_CA78C39A, float Vector1_279D2776, Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 IN, out float RandomStiffness_0)
                    {
                        float2 _Property_475BFCB9_Out_0 = Vector2_CA78C39A;
                        float2 _Multiply_7EE00C92_Out_2;
                        Unity_Multiply_float(_Property_475BFCB9_Out_0, float2(10, 10), _Multiply_7EE00C92_Out_2);
                        float _Split_A0FB144F_R_1 = _Multiply_7EE00C92_Out_2[0];
                        float _Split_A0FB144F_G_2 = _Multiply_7EE00C92_Out_2[1];
                        float _Split_A0FB144F_B_3 = 0;
                        float _Split_A0FB144F_A_4 = 0;
                        float _Multiply_2482A544_Out_2;
                        Unity_Multiply_float(_Split_A0FB144F_R_1, _Split_A0FB144F_G_2, _Multiply_2482A544_Out_2);
                        float _Fraction_B90029E4_Out_1;
                        Unity_Fraction_float(_Multiply_2482A544_Out_2, _Fraction_B90029E4_Out_1);
                        float _Power_E2B2B095_Out_2;
                        Unity_Power_float(_Fraction_B90029E4_Out_1, 2, _Power_E2B2B095_Out_2);
                        float _Property_91226CD6_Out_0 = Vector1_279D2776;
                        float _OneMinus_A56B8867_Out_1;
                        Unity_OneMinus_float(_Property_91226CD6_Out_0, _OneMinus_A56B8867_Out_1);
                        float _Clamp_E85434A6_Out_3;
                        Unity_Clamp_float(_Power_E2B2B095_Out_2, _OneMinus_A56B8867_Out_1, 1, _Clamp_E85434A6_Out_3);
                        RandomStiffness_0 = _Clamp_E85434A6_Out_3;
                    }
                
                    struct Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628
                    {
                    };
                
                    void SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(float Vector1_AFC49E6C, float Vector1_A18CF4DF, float Vector1_28AC83F8, float Vector1_E0042E1, float Vector1_1A24AAF, Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 IN, out float GustStrength_0)
                    {
                        float _Property_9A741C0D_Out_0 = Vector1_AFC49E6C;
                        float _Property_F564A485_Out_0 = Vector1_A18CF4DF;
                        float _Multiply_248F3A68_Out_2;
                        Unity_Multiply_float(_Property_9A741C0D_Out_0, _Property_F564A485_Out_0, _Multiply_248F3A68_Out_2);
                        float _Clamp_64D749D9_Out_3;
                        Unity_Clamp_float(_Multiply_248F3A68_Out_2, 0.1, 0.9, _Clamp_64D749D9_Out_3);
                        float _OneMinus_BDC5FAC3_Out_1;
                        Unity_OneMinus_float(_Clamp_64D749D9_Out_3, _OneMinus_BDC5FAC3_Out_1);
                        float _Multiply_E3C6FEFE_Out_2;
                        Unity_Multiply_float(_Multiply_248F3A68_Out_2, _OneMinus_BDC5FAC3_Out_1, _Multiply_E3C6FEFE_Out_2);
                        float _Multiply_9087CA8A_Out_2;
                        Unity_Multiply_float(_Multiply_E3C6FEFE_Out_2, 1.5, _Multiply_9087CA8A_Out_2);
                        float _Property_C7E6777F_Out_0 = Vector1_28AC83F8;
                        float _Multiply_1D329CB_Out_2;
                        Unity_Multiply_float(_Multiply_9087CA8A_Out_2, _Property_C7E6777F_Out_0, _Multiply_1D329CB_Out_2);
                        float _Property_84113466_Out_0 = Vector1_E0042E1;
                        float _Multiply_9501294C_Out_2;
                        Unity_Multiply_float(_Multiply_1D329CB_Out_2, _Property_84113466_Out_0, _Multiply_9501294C_Out_2);
                        float _Property_57C5AF03_Out_0 = Vector1_1A24AAF;
                        float _Multiply_E178164E_Out_2;
                        Unity_Multiply_float(_Multiply_9501294C_Out_2, _Property_57C5AF03_Out_0, _Multiply_E178164E_Out_2);
                        GustStrength_0 = _Multiply_E178164E_Out_2;
                    }
                
                    void Unity_Multiply_float (float3 A, float3 B, out float3 Out)
                    {
                        Out = A * B;
                    }
                
                    struct Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a
                    {
                    };
                
                    void SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(float2 Vector2_287CB44E, float2 Vector2_2A17E6EA, float Vector1_F4B6A491, float Vector1_2C90770B, TEXTURE2D_PARAM(Texture2D_D44B4848, samplerTexture2D_D44B4848), float4 Texture2D_D44B4848_TexelSize, float Vector1_AD94E9BC, Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a IN, out float3 ShiverNoise_0)
                    {
                        float2 _Property_961D8A0_Out_0 = Vector2_287CB44E;
                        float _Property_A414F012_Out_0 = Vector1_AD94E9BC;
                        float _Multiply_7DB42988_Out_2;
                        Unity_Multiply_float(_Property_A414F012_Out_0, 2, _Multiply_7DB42988_Out_2);
                        float2 _Add_4C3CF1F_Out_2;
                        Unity_Add_float2(_Property_961D8A0_Out_0, (_Multiply_7DB42988_Out_2.xx), _Add_4C3CF1F_Out_2);
                        float2 _Property_EBC67BC7_Out_0 = Vector2_2A17E6EA;
                        float _Property_13D296B5_Out_0 = Vector1_F4B6A491;
                        float2 _Multiply_BBB72061_Out_2;
                        Unity_Multiply_float(_Property_EBC67BC7_Out_0, (_Property_13D296B5_Out_0.xx), _Multiply_BBB72061_Out_2);
                        float _Property_3BB601E6_Out_0 = Vector1_2C90770B;
                        float2 _Multiply_FF9010E8_Out_2;
                        Unity_Multiply_float(_Multiply_BBB72061_Out_2, (_Property_3BB601E6_Out_0.xx), _Multiply_FF9010E8_Out_2);
                        float2 _Subtract_6BF2D170_Out_2;
                        Unity_Subtract_float2(_Add_4C3CF1F_Out_2, _Multiply_FF9010E8_Out_2, _Subtract_6BF2D170_Out_2);
                        float4 _SampleTexture2DLOD_DBCD6404_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_D44B4848, samplerTexture2D_D44B4848, _Subtract_6BF2D170_Out_2, 0);
                        float _SampleTexture2DLOD_DBCD6404_R_5 = _SampleTexture2DLOD_DBCD6404_RGBA_0.r;
                        float _SampleTexture2DLOD_DBCD6404_G_6 = _SampleTexture2DLOD_DBCD6404_RGBA_0.g;
                        float _SampleTexture2DLOD_DBCD6404_B_7 = _SampleTexture2DLOD_DBCD6404_RGBA_0.b;
                        float _SampleTexture2DLOD_DBCD6404_A_8 = _SampleTexture2DLOD_DBCD6404_RGBA_0.a;
                        float4 _Combine_E5D76A97_RGBA_4;
                        float3 _Combine_E5D76A97_RGB_5;
                        float2 _Combine_E5D76A97_RG_6;
                        Unity_Combine_float(_SampleTexture2DLOD_DBCD6404_R_5, _SampleTexture2DLOD_DBCD6404_G_6, _SampleTexture2DLOD_DBCD6404_B_7, 0, _Combine_E5D76A97_RGBA_4, _Combine_E5D76A97_RGB_5, _Combine_E5D76A97_RG_6);
                        float3 _Subtract_AA7C02E2_Out_2;
                        Unity_Subtract_float3(_Combine_E5D76A97_RGB_5, float3(0.5, 0.5, 0.5), _Subtract_AA7C02E2_Out_2);
                        float3 _Multiply_5BF7CBD7_Out_2;
                        Unity_Multiply_float(_Subtract_AA7C02E2_Out_2, float3(2, 2, 2), _Multiply_5BF7CBD7_Out_2);
                        ShiverNoise_0 = _Multiply_5BF7CBD7_Out_2;
                    }
                
                    struct Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459
                    {
                    };
                
                    void SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(float3 Vector3_ED0F539A, float2 Vector2_84805101, float Vector1_BDF24CF7, float Vector1_839268A4, float Vector1_A8621014, float Vector1_2DBE6CC0, float Vector1_8A4EF006, float Vector1_ED935C73, Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 IN, out float3 ShiverDirection_0, out float ShiverStrength_1)
                    {
                        float3 _Property_FC94AEBB_Out_0 = Vector3_ED0F539A;
                        float _Property_4FE2271A_Out_0 = Vector1_BDF24CF7;
                        float4 _Combine_328044F1_RGBA_4;
                        float3 _Combine_328044F1_RGB_5;
                        float2 _Combine_328044F1_RG_6;
                        Unity_Combine_float(1, _Property_4FE2271A_Out_0, 1, 0, _Combine_328044F1_RGBA_4, _Combine_328044F1_RGB_5, _Combine_328044F1_RG_6);
                        float3 _Multiply_4FCE02F7_Out_2;
                        Unity_Multiply_float(_Property_FC94AEBB_Out_0, _Combine_328044F1_RGB_5, _Multiply_4FCE02F7_Out_2);
                        float2 _Property_77EED0A8_Out_0 = Vector2_84805101;
                        float _Split_2D66AF35_R_1 = _Property_77EED0A8_Out_0[0];
                        float _Split_2D66AF35_G_2 = _Property_77EED0A8_Out_0[1];
                        float _Split_2D66AF35_B_3 = 0;
                        float _Split_2D66AF35_A_4 = 0;
                        float4 _Combine_C2861A09_RGBA_4;
                        float3 _Combine_C2861A09_RGB_5;
                        float2 _Combine_C2861A09_RG_6;
                        Unity_Combine_float(_Split_2D66AF35_R_1, _Property_4FE2271A_Out_0, _Split_2D66AF35_G_2, 0, _Combine_C2861A09_RGBA_4, _Combine_C2861A09_RGB_5, _Combine_C2861A09_RG_6);
                        float3 _Lerp_A6B0BE86_Out_3;
                        Unity_Lerp_float3(_Multiply_4FCE02F7_Out_2, _Combine_C2861A09_RGB_5, float3(0.5, 0.5, 0.5), _Lerp_A6B0BE86_Out_3);
                        float _Property_BBBC9C1B_Out_0 = Vector1_839268A4;
                        float _Length_F022B321_Out_1;
                        Unity_Length_float3(_Multiply_4FCE02F7_Out_2, _Length_F022B321_Out_1);
                        float _Multiply_BFD84B03_Out_2;
                        Unity_Multiply_float(_Length_F022B321_Out_1, 0.5, _Multiply_BFD84B03_Out_2);
                        float _Multiply_3564B68A_Out_2;
                        Unity_Multiply_float(_Property_BBBC9C1B_Out_0, _Multiply_BFD84B03_Out_2, _Multiply_3564B68A_Out_2);
                        float _Add_83285742_Out_2;
                        Unity_Add_float(_Multiply_3564B68A_Out_2, _Length_F022B321_Out_1, _Add_83285742_Out_2);
                        float _Property_45D94B1_Out_0 = Vector1_2DBE6CC0;
                        float _Multiply_EA43D494_Out_2;
                        Unity_Multiply_float(_Add_83285742_Out_2, _Property_45D94B1_Out_0, _Multiply_EA43D494_Out_2);
                        float _Clamp_C109EA71_Out_3;
                        Unity_Clamp_float(_Multiply_EA43D494_Out_2, 0.1, 0.9, _Clamp_C109EA71_Out_3);
                        float _OneMinus_226F3377_Out_1;
                        Unity_OneMinus_float(_Clamp_C109EA71_Out_3, _OneMinus_226F3377_Out_1);
                        float _Multiply_8680628F_Out_2;
                        Unity_Multiply_float(_Multiply_EA43D494_Out_2, _OneMinus_226F3377_Out_1, _Multiply_8680628F_Out_2);
                        float _Multiply_B14E644_Out_2;
                        Unity_Multiply_float(_Multiply_8680628F_Out_2, 1.5, _Multiply_B14E644_Out_2);
                        float _Property_7F61FC78_Out_0 = Vector1_A8621014;
                        float _Multiply_C89CF7DC_Out_2;
                        Unity_Multiply_float(_Multiply_B14E644_Out_2, _Property_7F61FC78_Out_0, _Multiply_C89CF7DC_Out_2);
                        float _Property_2BD306B6_Out_0 = Vector1_8A4EF006;
                        float _Multiply_E5D34DCC_Out_2;
                        Unity_Multiply_float(_Multiply_C89CF7DC_Out_2, _Property_2BD306B6_Out_0, _Multiply_E5D34DCC_Out_2);
                        float _Property_DBC71A4F_Out_0 = Vector1_ED935C73;
                        float _Multiply_BCACDD38_Out_2;
                        Unity_Multiply_float(_Multiply_E5D34DCC_Out_2, _Property_DBC71A4F_Out_0, _Multiply_BCACDD38_Out_2);
                        ShiverDirection_0 = _Lerp_A6B0BE86_Out_3;
                        ShiverStrength_1 = _Multiply_BCACDD38_Out_2;
                    }
                
                    struct Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364
                    {
                    };
                
                    void SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(float3 Vector3_829210A7, float3 Vector3_1A016C4A, float Vector1_31372BF, float Vector1_E57895AF, TEXTURE2D_PARAM(Texture2D_65F71447, samplerTexture2D_65F71447), float4 Texture2D_65F71447_TexelSize, float Vector1_8836FB6A, TEXTURE2D_PARAM(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), float4 Texture2D_4A3BDB6_TexelSize, float Vector1_14E206AE, float Vector1_7090E96C, float Vector1_51722AC, float Vector1_A3894D2, float Vector1_6F0C3A5A, float Vector1_2D1F6C2F, float Vector1_347751CA, Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 IN, out float GustStrength_0, out float ShiverStrength_1, out float3 ShiverDirection_2)
                    {
                        float _Property_5C7F4A8C_Out_0 = Vector1_31372BF;
                        float _Property_142FEDC3_Out_0 = Vector1_347751CA;
                        float3 _Property_D2FC65C3_Out_0 = Vector3_829210A7;
                        float _Split_8E347DCF_R_1 = _Property_D2FC65C3_Out_0[0];
                        float _Split_8E347DCF_G_2 = _Property_D2FC65C3_Out_0[1];
                        float _Split_8E347DCF_B_3 = _Property_D2FC65C3_Out_0[2];
                        float _Split_8E347DCF_A_4 = 0;
                        float4 _Combine_9B5A76B7_RGBA_4;
                        float3 _Combine_9B5A76B7_RGB_5;
                        float2 _Combine_9B5A76B7_RG_6;
                        Unity_Combine_float(_Split_8E347DCF_R_1, _Split_8E347DCF_B_3, 0, 0, _Combine_9B5A76B7_RGBA_4, _Combine_9B5A76B7_RGB_5, _Combine_9B5A76B7_RG_6);
                        float3 _Property_5653999E_Out_0 = Vector3_1A016C4A;
                        float _Split_B9CBBFE5_R_1 = _Property_5653999E_Out_0[0];
                        float _Split_B9CBBFE5_G_2 = _Property_5653999E_Out_0[1];
                        float _Split_B9CBBFE5_B_3 = _Property_5653999E_Out_0[2];
                        float _Split_B9CBBFE5_A_4 = 0;
                        float4 _Combine_DC44394B_RGBA_4;
                        float3 _Combine_DC44394B_RGB_5;
                        float2 _Combine_DC44394B_RG_6;
                        Unity_Combine_float(_Split_B9CBBFE5_R_1, _Split_B9CBBFE5_B_3, 0, 0, _Combine_DC44394B_RGBA_4, _Combine_DC44394B_RGB_5, _Combine_DC44394B_RG_6);
                        float _Property_3221EFCE_Out_0 = Vector1_E57895AF;
                        Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f _GustNoiseAtPosition_3B28852B;
                        float _GustNoiseAtPosition_3B28852B_GustNoise_0;
                        SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(_Property_5C7F4A8C_Out_0, _Property_142FEDC3_Out_0, _Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_3221EFCE_Out_0, TEXTURE2D_ARGS(Texture2D_65F71447, samplerTexture2D_65F71447), Texture2D_65F71447_TexelSize, _GustNoiseAtPosition_3B28852B, _GustNoiseAtPosition_3B28852B_GustNoise_0);
                        float _Property_1B306054_Out_0 = Vector1_A3894D2;
                        float _Property_1FBC768_Out_0 = Vector1_51722AC;
                        float _Property_9FB10D19_Out_0 = Vector1_14E206AE;
                        Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 _RandomStiffnessAtPosition_C9AD50AB;
                        float _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0;
                        SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(_Combine_9B5A76B7_RG_6, _Property_9FB10D19_Out_0, _RandomStiffnessAtPosition_C9AD50AB, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0);
                        float _Property_EE5A603D_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 _CalculateGustStrength_E2853C74;
                        float _CalculateGustStrength_E2853C74_GustStrength_0;
                        SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(_GustNoiseAtPosition_3B28852B_GustNoise_0, _Property_1B306054_Out_0, _Property_1FBC768_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _Property_EE5A603D_Out_0, _CalculateGustStrength_E2853C74, _CalculateGustStrength_E2853C74_GustStrength_0);
                        float _Property_DFB3FCE0_Out_0 = Vector1_31372BF;
                        float _Property_8A8735CC_Out_0 = Vector1_8836FB6A;
                        Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a _ShiverNoiseAtPosition_35F9220A;
                        float3 _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0;
                        SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(_Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_DFB3FCE0_Out_0, _Property_8A8735CC_Out_0, TEXTURE2D_ARGS(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), Texture2D_4A3BDB6_TexelSize, _Property_142FEDC3_Out_0, _ShiverNoiseAtPosition_35F9220A, _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0);
                        float _Property_65F19953_Out_0 = Vector1_6F0C3A5A;
                        float _Property_3A2F45FE_Out_0 = Vector1_51722AC;
                        float _Property_98EF73E5_Out_0 = Vector1_2D1F6C2F;
                        float _Property_6A278DE2_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 _CalculateShiver_799DE4CB;
                        float3 _CalculateShiver_799DE4CB_ShiverDirection_0;
                        float _CalculateShiver_799DE4CB_ShiverStrength_1;
                        SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(_ShiverNoiseAtPosition_35F9220A_ShiverNoise_0, _Combine_DC44394B_RG_6, _Property_65F19953_Out_0, _CalculateGustStrength_E2853C74_GustStrength_0, _Property_3A2F45FE_Out_0, _Property_98EF73E5_Out_0, _Property_6A278DE2_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _CalculateShiver_799DE4CB, _CalculateShiver_799DE4CB_ShiverDirection_0, _CalculateShiver_799DE4CB_ShiverStrength_1);
                        GustStrength_0 = _CalculateGustStrength_E2853C74_GustStrength_0;
                        ShiverStrength_1 = _CalculateShiver_799DE4CB_ShiverStrength_1;
                        ShiverDirection_2 = _CalculateShiver_799DE4CB_ShiverDirection_0;
                    }
                
                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }
                
                    struct Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01
                    {
                        float3 ObjectSpacePosition;
                    };
                
                    void SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(float3 Vector3_C96069F9, float Vector1_A5EB719C, float Vector1_4D1D3B1A, float3 Vector3_C80E97FF, float3 Vector3_821C320A, float3 Vector3_4BF0DC64, Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 IN, out float3 WorldPosition_0)
                    {
                        float3 _Property_65372844_Out_0 = Vector3_4BF0DC64;
                        float3 _Property_7205E35B_Out_0 = Vector3_821C320A;
                        float _Property_916D8D52_Out_0 = Vector1_4D1D3B1A;
                        float3 _Multiply_CF9DF1B5_Out_2;
                        Unity_Multiply_float(_Property_7205E35B_Out_0, (_Property_916D8D52_Out_0.xxx), _Multiply_CF9DF1B5_Out_2);
                        float3 _Multiply_57D2E5C7_Out_2;
                        Unity_Multiply_float(_Multiply_CF9DF1B5_Out_2, float3(10, 10, 10), _Multiply_57D2E5C7_Out_2);
                        float3 _Add_F265DF09_Out_2;
                        Unity_Add_float3(_Property_65372844_Out_0, _Multiply_57D2E5C7_Out_2, _Add_F265DF09_Out_2);
                        float3 _Property_806C350F_Out_0 = Vector3_C96069F9;
                        float _Property_D017A08E_Out_0 = Vector1_A5EB719C;
                        float3 _Multiply_99498CF9_Out_2;
                        Unity_Multiply_float(_Property_806C350F_Out_0, (_Property_D017A08E_Out_0.xxx), _Multiply_99498CF9_Out_2);
                        float _Split_A5777330_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_A5777330_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_A5777330_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_A5777330_A_4 = 0;
                        float _Clamp_C4364CA5_Out_3;
                        Unity_Clamp_float(_Split_A5777330_G_2, 0, 1, _Clamp_C4364CA5_Out_3);
                        float3 _Multiply_ADC4C2A_Out_2;
                        Unity_Multiply_float(_Multiply_99498CF9_Out_2, (_Clamp_C4364CA5_Out_3.xxx), _Multiply_ADC4C2A_Out_2);
                        float3 _Multiply_49835441_Out_2;
                        Unity_Multiply_float(_Multiply_ADC4C2A_Out_2, float3(10, 10, 10), _Multiply_49835441_Out_2);
                        float3 _Add_B14AAE70_Out_2;
                        Unity_Add_float3(_Add_F265DF09_Out_2, _Multiply_49835441_Out_2, _Add_B14AAE70_Out_2);
                        WorldPosition_0 = _Add_B14AAE70_Out_2;
                    }
                
                    struct Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceTangent;
                        float3 WorldSpaceBiTangent;
                    };
                
                    void SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(float3 Vector3_AAF445D6, Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 IN, out float3 ObjectPosition_1)
                    {
                        float3 _Property_51DA8EE_Out_0 = Vector3_AAF445D6;
                        float3 _Subtract_B236C96B_Out_2;
                        Unity_Subtract_float3(_Property_51DA8EE_Out_0, _WorldSpaceCameraPos, _Subtract_B236C96B_Out_2);
                        float3 _Transform_6FDB2E47_Out_1 = TransformWorldToObject(_Subtract_B236C96B_Out_2.xyz);
                        ObjectPosition_1 = _Transform_6FDB2E47_Out_1;
                    }
                
                // Vertex Graph Evaluation
                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 _GetPivotInWorldSpace_73F19E42;
                        float3 _GetPivotInWorldSpace_73F19E42_PivotInWS_0;
                        SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(_GetPivotInWorldSpace_73F19E42, _GetPivotInWorldSpace_73F19E42_PivotInWS_0);
                        float _Split_64420219_R_1 = IN.VertexColor[0];
                        float _Split_64420219_G_2 = IN.VertexColor[1];
                        float _Split_64420219_B_3 = IN.VertexColor[2];
                        float _Split_64420219_A_4 = IN.VertexColor[3];
                        float3 _Lerp_4531CF63_Out_3;
                        Unity_Lerp_float3(_GetPivotInWorldSpace_73F19E42_PivotInWS_0, IN.AbsoluteWorldSpacePosition, (_Split_64420219_G_2.xxx), _Lerp_4531CF63_Out_3);
                        float4 _Property_D6662DC6_Out_0 = _GlobalWindDirectionAndStrength;
                        float4 _Property_9515B228_Out_0 = _WindDirectionAndStrength;
                        float4 _Property_9A1EF240_Out_0 = _GlobalShiver;
                        float4 _Property_777C8DB2_Out_0 = _Shiver;
                        Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 _GlobalWindParameters_B547F135;
                        float3 _GlobalWindParameters_B547F135_GustDirection_0;
                        float _GlobalWindParameters_B547F135_GustSpeed_1;
                        float _GlobalWindParameters_B547F135_GustStrength_2;
                        float _GlobalWindParameters_B547F135_ShiverSpeed_3;
                        float _GlobalWindParameters_B547F135_ShiverStrength_4;
                        SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(_Property_D6662DC6_Out_0, _Property_9515B228_Out_0, (_Property_9A1EF240_Out_0.xy), (_Property_777C8DB2_Out_0.xy), _GlobalWindParameters_B547F135, _GlobalWindParameters_B547F135_GustDirection_0, _GlobalWindParameters_B547F135_GustSpeed_1, _GlobalWindParameters_B547F135_GustStrength_2, _GlobalWindParameters_B547F135_ShiverSpeed_3, _GlobalWindParameters_B547F135_ShiverStrength_4);
                        float _Property_5F3A390D_Out_0 = _BAKEDMASK_ON;
                        float3 _Subtract_BF2A75CD_Out_2;
                        Unity_Subtract_float3(IN.AbsoluteWorldSpacePosition, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _Subtract_BF2A75CD_Out_2);
                        float _Split_29C81DE4_R_1 = _Subtract_BF2A75CD_Out_2[0];
                        float _Split_29C81DE4_G_2 = _Subtract_BF2A75CD_Out_2[1];
                        float _Split_29C81DE4_B_3 = _Subtract_BF2A75CD_Out_2[2];
                        float _Split_29C81DE4_A_4 = 0;
                        float _Add_6A47DB4F_Out_2;
                        Unity_Add_float(_Split_29C81DE4_R_1, _Split_29C81DE4_G_2, _Add_6A47DB4F_Out_2);
                        float _Add_EC455B5D_Out_2;
                        Unity_Add_float(_Add_6A47DB4F_Out_2, _Split_29C81DE4_B_3, _Add_EC455B5D_Out_2);
                        float _Multiply_F013BB8B_Out_2;
                        Unity_Multiply_float(_Add_EC455B5D_Out_2, 0.4, _Multiply_F013BB8B_Out_2);
                        float _Fraction_7D389816_Out_1;
                        Unity_Fraction_float(_Multiply_F013BB8B_Out_2, _Fraction_7D389816_Out_1);
                        float _Multiply_776D3DAF_Out_2;
                        Unity_Multiply_float(_Fraction_7D389816_Out_1, 0.15, _Multiply_776D3DAF_Out_2);
                        float _Split_E4BB9FEC_R_1 = IN.VertexColor[0];
                        float _Split_E4BB9FEC_G_2 = IN.VertexColor[1];
                        float _Split_E4BB9FEC_B_3 = IN.VertexColor[2];
                        float _Split_E4BB9FEC_A_4 = IN.VertexColor[3];
                        float _Multiply_BC8988C3_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, _Split_E4BB9FEC_G_2, _Multiply_BC8988C3_Out_2);
                        float _Multiply_EC5FE292_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_R_1, 0.3, _Multiply_EC5FE292_Out_2);
                        float _Add_A8423510_Out_2;
                        Unity_Add_float(_Multiply_BC8988C3_Out_2, _Multiply_EC5FE292_Out_2, _Add_A8423510_Out_2);
                        float _Add_CE74358C_Out_2;
                        Unity_Add_float(_Add_A8423510_Out_2, IN.TimeParameters.x, _Add_CE74358C_Out_2);
                        float _Multiply_1CE438D_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_G_2, 0.5, _Multiply_1CE438D_Out_2);
                        float _Add_8718B88C_Out_2;
                        Unity_Add_float(_Add_CE74358C_Out_2, _Multiply_1CE438D_Out_2, _Add_8718B88C_Out_2);
                        float _Property_DBA903E3_Out_0 = _UVMASK_ON;
                        float4 _UV_64D01E18_Out_0 = IN.uv0;
                        float _Split_A5DFBEBE_R_1 = _UV_64D01E18_Out_0[0];
                        float _Split_A5DFBEBE_G_2 = _UV_64D01E18_Out_0[1];
                        float _Split_A5DFBEBE_B_3 = _UV_64D01E18_Out_0[2];
                        float _Split_A5DFBEBE_A_4 = _UV_64D01E18_Out_0[3];
                        float _Multiply_C943DA5C_Out_2;
                        Unity_Multiply_float(_Split_A5DFBEBE_G_2, 0.1, _Multiply_C943DA5C_Out_2);
                        float _Branch_12012434_Out_3;
                        Unity_Branch_float(_Property_DBA903E3_Out_0, _Multiply_C943DA5C_Out_2, 0, _Branch_12012434_Out_3);
                        float _Add_922F2E64_Out_2;
                        Unity_Add_float(IN.TimeParameters.x, _Branch_12012434_Out_3, _Add_922F2E64_Out_2);
                        float _Multiply_2E689843_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, 0.5, _Multiply_2E689843_Out_2);
                        float _Add_ED1EE4DB_Out_2;
                        Unity_Add_float(_Add_922F2E64_Out_2, _Multiply_2E689843_Out_2, _Add_ED1EE4DB_Out_2);
                        float _Branch_291934CD_Out_3;
                        Unity_Branch_float(_Property_5F3A390D_Out_0, _Add_8718B88C_Out_2, _Add_ED1EE4DB_Out_2, _Branch_291934CD_Out_3);
                        float _Property_267CF497_Out_0 = _StiffnessVariation;
                        float _Property_4FB02E51_Out_0 = _BAKEDMASK_ON;
                        float4 _UV_6482E163_Out_0 = IN.uv1;
                        float _Split_2D1A67CF_R_1 = _UV_6482E163_Out_0[0];
                        float _Split_2D1A67CF_G_2 = _UV_6482E163_Out_0[1];
                        float _Split_2D1A67CF_B_3 = _UV_6482E163_Out_0[2];
                        float _Split_2D1A67CF_A_4 = _UV_6482E163_Out_0[3];
                        float _Multiply_F7BD1E76_Out_2;
                        Unity_Multiply_float(_Split_2D1A67CF_R_1, _Split_2D1A67CF_G_2, _Multiply_F7BD1E76_Out_2);
                        float _Property_B1FAFDBF_Out_0 = _UVMASK_ON;
                        float4 _UV_8F58F10B_Out_0 = IN.uv0;
                        float _Split_BD0858B3_R_1 = _UV_8F58F10B_Out_0[0];
                        float _Split_BD0858B3_G_2 = _UV_8F58F10B_Out_0[1];
                        float _Split_BD0858B3_B_3 = _UV_8F58F10B_Out_0[2];
                        float _Split_BD0858B3_A_4 = _UV_8F58F10B_Out_0[3];
                        float _Multiply_3FAD9403_Out_2;
                        Unity_Multiply_float(_Split_BD0858B3_G_2, 0.2, _Multiply_3FAD9403_Out_2);
                        float _Branch_3AF3832A_Out_3;
                        Unity_Branch_float(_Property_B1FAFDBF_Out_0, _Multiply_3FAD9403_Out_2, 1, _Branch_3AF3832A_Out_3);
                        float _Branch_F921E5A9_Out_3;
                        Unity_Branch_float(_Property_4FB02E51_Out_0, _Multiply_F7BD1E76_Out_2, _Branch_3AF3832A_Out_3, _Branch_F921E5A9_Out_3);
                        Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 _GetWindStrength_5806EF0A;
                        float _GetWindStrength_5806EF0A_GustStrength_0;
                        float _GetWindStrength_5806EF0A_ShiverStrength_1;
                        float3 _GetWindStrength_5806EF0A_ShiverDirection_2;
                        SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(_Lerp_4531CF63_Out_3, _GlobalWindParameters_B547F135_GustDirection_0, _Branch_291934CD_Out_3, _GlobalWindParameters_B547F135_GustSpeed_1, TEXTURE2D_ARGS(_GustNoise, sampler_GustNoise), _GustNoise_TexelSize, _GlobalWindParameters_B547F135_ShiverSpeed_3, TEXTURE2D_ARGS(_ShiverNoise, sampler_ShiverNoise), _ShiverNoise_TexelSize, _Property_267CF497_Out_0, 1, _Branch_F921E5A9_Out_3, _GlobalWindParameters_B547F135_GustStrength_2, 0.2, _GlobalWindParameters_B547F135_ShiverStrength_4, 0, _GetWindStrength_5806EF0A, _GetWindStrength_5806EF0A_GustStrength_0, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_ShiverDirection_2);
                        Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 _ApplyTreeWindDisplacement_8E73FF2E;
                        _ApplyTreeWindDisplacement_8E73FF2E.ObjectSpacePosition = IN.ObjectSpacePosition;
                        float3 _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0;
                        SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(_GetWindStrength_5806EF0A_ShiverDirection_2, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_GustStrength_0, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _GlobalWindParameters_B547F135_GustDirection_0, IN.AbsoluteWorldSpacePosition, _ApplyTreeWindDisplacement_8E73FF2E, _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0);
                        Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 _WorldToObject_628B231E;
                        _WorldToObject_628B231E.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _WorldToObject_628B231E.WorldSpaceTangent = IN.WorldSpaceTangent;
                        _WorldToObject_628B231E.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                        float3 _WorldToObject_628B231E_ObjectPosition_1;
                        SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(_ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0, _WorldToObject_628B231E, _WorldToObject_628B231E_ObjectPosition_1);
                        description.VertexPosition = _WorldToObject_628B231E_ObjectPosition_1;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }
                    
                // Pixel Graph Evaluation
                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_F86B9939_RGBA_0 = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, IN.uv0.xy);
                        float _SampleTexture2D_F86B9939_R_4 = _SampleTexture2D_F86B9939_RGBA_0.r;
                        float _SampleTexture2D_F86B9939_G_5 = _SampleTexture2D_F86B9939_RGBA_0.g;
                        float _SampleTexture2D_F86B9939_B_6 = _SampleTexture2D_F86B9939_RGBA_0.b;
                        float _SampleTexture2D_F86B9939_A_7 = _SampleTexture2D_F86B9939_RGBA_0.a;
                        float _Property_ABA23041_Out_0 = _AlphaClip;
                        surface.Alpha = _SampleTexture2D_F86B9939_A_7;
                        surface.AlphaClipThreshold = _Property_ABA23041_Out_0;
                        return surface;
                    }
                    
            //-------------------------------------------------------------------------------------
            // End graph generated code
            //-------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
            
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                // output.ViewSpaceNormal =             TransformWorldToViewDir(output.WorldSpaceNormal);
                // output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                output.ObjectSpacePosition =         input.positionOS;
                // output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                // output.ViewSpacePosition =           TransformWorldToView(output.WorldSpacePosition);
                // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                // output.WorldSpaceViewDirection =     GetWorldSpaceNormalizeViewDir(output.WorldSpacePosition);
                // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(output.WorldSpacePosition), _ProjectionParams.x);
                output.uv0 =                         input.uv0;
                output.uv1 =                         input.uv1;
                // output.uv2 =                         input.uv2;
                // output.uv3 =                         input.uv3;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            {
                // build graph inputs
                VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
                // Override time paramters with used one (This is required to correctly handle motion vector for vertex animation based on time)
                vertexDescriptionInputs.TimeParameters = timeParameters;
            
                // evaluate vertex graph
                VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
            
                // copy graph output to the results
                input.positionOS = vertexDescription.VertexPosition;
                input.normalOS = vertexDescription.VertexNormal;
                input.tangentOS.xyz = vertexDescription.VertexTangent;
            
                return input;
            }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
                FragInputs BuildFragInputs(VaryingsMeshToPS input)
                {
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.positionCS;       // input.positionCS is SV_Position
            
                    // output.positionRWS = input.positionRWS;
                    // output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
                    output.texCoord0 = input.texCoord0;
                    // output.texCoord1 = input.texCoord1;
                    // output.texCoord2 = input.texCoord2;
                    // output.texCoord3 = input.texCoord3;
                    // output.color = input.color;
                    #if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #elif SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #endif // SHADER_STAGE_FRAGMENT
            
                    return output;
                }
            
                SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                    // output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
                    // output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
                    // output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                    // output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
                    // output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
                    // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                    // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                    // output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
                    // output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
                    // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                    // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                    // output.WorldSpaceViewDirection =     normalize(viewWS);
                    // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                    // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                    // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                    // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                    // output.WorldSpacePosition =          input.positionRWS;
                    // output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
                    // output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
                    // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                    // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionRWS);
                    // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
                    output.uv0 =                         input.texCoord0;
                    // output.uv1 =                         input.texCoord1;
                    // output.uv2 =                         input.texCoord2;
                    // output.uv3 =                         input.texCoord3;
                    // output.VertexColor =                 input.color;
                    // output.FaceSign =                    input.isFrontFace;
                    // output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            
                    return output;
                }
            
                // existing HDRP code uses the combined function to go directly from packed to frag inputs
                FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    VaryingsMeshToPS unpacked= UnpackVaryingsMeshToPS(input);
                    return BuildFragInputs(unpacked);
                }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
            void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
            {
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                // copy across graph values, if defined
                // surfaceData.baseColor =                 surfaceDescription.Albedo;
                // surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                // surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                // surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                // surfaceData.metallic =                  surfaceDescription.Metallic;
                // surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                // surfaceData.thickness =                 surfaceDescription.Thickness;
                // surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                // surfaceData.specularColor =             surfaceDescription.Specular;
                // surfaceData.coatMask =                  surfaceDescription.CoatMask;
                // surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                // surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                // surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
        #ifdef _HAS_REFRACTION
                if (_EnableSSRefraction)
                {
                    // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                    // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                    // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                    surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                    surfaceDescription.Alpha = 1.0;
                }
                else
                {
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                    surfaceDescription.Alpha = 1.0;
                }
        #else
                surfaceData.ior = 1.0;
                surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                surfaceData.atDistance = 1.0;
                surfaceData.transmittanceMask = 0.0;
        #endif
                
                // These static material feature allow compile time optimization
                surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
        #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
        #endif
        #ifdef _MATERIAL_FEATURE_TRANSMISSION
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
        #endif
        #ifdef _MATERIAL_FEATURE_ANISOTROPY
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
        #endif
                // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
        #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
        #endif
        #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
        #endif
        
        #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                // Require to have setup baseColor
                // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                // tangent-space normal
                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                // normalTS = surfaceDescription.Normal;
        
                // compute world space normal
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                bentNormalWS = surfaceData.normalWS;
                // GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
                // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                // If user provide bent normal then we process a better term
        #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                // Just use the value passed through via the slot (not active otherwise)
        #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                // If we have bent normal and ambient occlusion, process a specular occlusion
                surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
        #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
        #else
                surfaceData.specularOcclusion = 1.0;
        #endif
        
        #if HAVE_DECALS
                if (_EnableDecals)
                {
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                }
        #endif
        
        #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
        #endif
        
        #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    // TODO: need to update mip info
                    surfaceData.metallic = 0;
                }
        
                // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                // as it can modify attribute use for static lighting
                ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
        #endif
            }
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
                
                // ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
        
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal 
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                // override sampleBakedGI:
                // builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
                // builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
        
                // builtinData.emissiveColor = surfaceDescription.Emission;
        
                // builtinData.depthOffset = surfaceDescription.DepthOffset;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
            //-------------------------------------------------------------------------------------
            // Pass Includes
            //-------------------------------------------------------------------------------------
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
            //-------------------------------------------------------------------------------------
            // End Pass Includes
            //-------------------------------------------------------------------------------------
        
            ENDHLSL
        }
        
        Pass
        {
            // based on HDLitPass.template
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
        
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull [_CullMode]
        
            
            ZWrite On
        
            
            // Stencil setup
        Stencil
        {
           WriteMask [_StencilWriteMaskDepth]
           Ref [_StencilRefDepth]
           Comp Always
           Pass Replace
        }
        
            
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        
            #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            // #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            #define _DISABLE_DECALS 1
            #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        
            //-------------------------------------------------------------------------------------
            // End Variant Definitions
            //-------------------------------------------------------------------------------------
        
            #pragma vertex Vert
            #pragma fragment Frag
        
            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                    #define SHADERPASS SHADERPASS_DEPTH_ONLY
                #pragma multi_compile _ WRITE_NORMAL_BUFFER
                #pragma multi_compile _ WRITE_MSAA_DEPTH
                #define RAYTRACING_SHADER_GRAPH_HIGH
                // ACTIVE FIELDS:
                //   DoubleSided
                //   DoubleSided.Flip
                //   FragInputs.isFrontFace
                //   Material.Translucent
                //   Material.Transmission
                //   AlphaTest
                //   DisableDecals
                //   DisableSSR
                //   Specular.EnergyConserving
                //   SurfaceDescriptionInputs.TangentSpaceNormal
                //   SurfaceDescriptionInputs.uv0
                //   VertexDescriptionInputs.VertexColor
                //   VertexDescriptionInputs.ObjectSpaceNormal
                //   VertexDescriptionInputs.WorldSpaceNormal
                //   VertexDescriptionInputs.ObjectSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceBiTangent
                //   VertexDescriptionInputs.ObjectSpacePosition
                //   VertexDescriptionInputs.AbsoluteWorldSpacePosition
                //   VertexDescriptionInputs.uv0
                //   VertexDescriptionInputs.uv1
                //   VertexDescriptionInputs.TimeParameters
                //   SurfaceDescription.Normal
                //   SurfaceDescription.Smoothness
                //   SurfaceDescription.Alpha
                //   SurfaceDescription.AlphaClipThreshold
                //   features.modifyMesh
                //   VertexDescription.VertexPosition
                //   VertexDescription.VertexNormal
                //   VertexDescription.VertexTangent
                //   AttributesMesh.normalOS
                //   AttributesMesh.tangentOS
                //   AttributesMesh.uv0
                //   AttributesMesh.uv1
                //   AttributesMesh.color
                //   AttributesMesh.uv2
                //   AttributesMesh.uv3
                //   FragInputs.tangentToWorld
                //   FragInputs.positionRWS
                //   FragInputs.texCoord0
                //   FragInputs.texCoord1
                //   FragInputs.texCoord2
                //   FragInputs.texCoord3
                //   FragInputs.color
                //   VaryingsMeshToPS.cullFace
                //   VertexDescriptionInputs.ObjectSpaceBiTangent
                //   AttributesMesh.positionOS
                //   VaryingsMeshToPS.tangentWS
                //   VaryingsMeshToPS.normalWS
                //   VaryingsMeshToPS.positionRWS
                //   VaryingsMeshToPS.texCoord0
                //   VaryingsMeshToPS.texCoord1
                //   VaryingsMeshToPS.texCoord2
                //   VaryingsMeshToPS.texCoord3
                //   VaryingsMeshToPS.color
                // Shared Graph Keywords
        
            // this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            #define VARYINGS_NEED_TEXCOORD2
            #define VARYINGS_NEED_TEXCOORD3
            #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_CULLFACE
            #define HAVE_MESH_MODIFICATION
        
        // We need isFontFace when using double sided
        #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
            #define VARYINGS_NEED_CULLFACE
        #endif
        
            //-------------------------------------------------------------------------------------
            // End Defines
            //-------------------------------------------------------------------------------------
        	
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
            //-------------------------------------------------------------------------------------
            // Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
            // Generated Type: AttributesMesh
            struct AttributesMesh
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL; // optional
                float4 tangentOS : TANGENT; // optional
                float4 uv0 : TEXCOORD0; // optional
                float4 uv1 : TEXCOORD1; // optional
                float4 uv2 : TEXCOORD2; // optional
                float4 uv3 : TEXCOORD3; // optional
                float4 color : COLOR; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            // Generated Type: VaryingsMeshToPS
            struct VaryingsMeshToPS
            {
                float4 positionCS : SV_Position;
                float3 positionRWS; // optional
                float3 normalWS; // optional
                float4 tangentWS; // optional
                float4 texCoord0; // optional
                float4 texCoord1; // optional
                float4 texCoord2; // optional
                float4 texCoord3; // optional
                float4 color; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            };
            
            // Generated Type: PackedVaryingsMeshToPS
            struct PackedVaryingsMeshToPS
            {
                float4 positionCS : SV_Position; // unpacked
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
                float4 interp02 : TEXCOORD2; // auto-packed
                float4 interp03 : TEXCOORD3; // auto-packed
                float4 interp04 : TEXCOORD4; // auto-packed
                float4 interp05 : TEXCOORD5; // auto-packed
                float4 interp06 : TEXCOORD6; // auto-packed
                float4 interp07 : TEXCOORD7; // auto-packed
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
                #endif // conditional
            };
            
            // Packed Type: VaryingsMeshToPS
            PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
            {
                PackedVaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyzw = input.texCoord1;
                output.interp05.xyzw = input.texCoord2;
                output.interp06.xyzw = input.texCoord3;
                output.interp07.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToPS
            VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
            {
                VaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.texCoord1 = input.interp04.xyzw;
                output.texCoord2 = input.interp05.xyzw;
                output.texCoord3 = input.interp06.xyzw;
                output.color = input.interp07.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            // Generated Type: VaryingsMeshToDS
            struct VaryingsMeshToDS
            {
                float3 positionRWS;
                float3 normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            
            // Generated Type: PackedVaryingsMeshToDS
            struct PackedVaryingsMeshToDS
            {
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
            };
            
            // Packed Type: VaryingsMeshToDS
            PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
            {
                PackedVaryingsMeshToDS output;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToDS
            VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
            {
                VaryingsMeshToDS output;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            //-------------------------------------------------------------------------------------
            // End Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
        
            //-------------------------------------------------------------------------------------
            // Graph generated code
            //-------------------------------------------------------------------------------------
                    // Shared Graph Properties (uniform inputs)
                    CBUFFER_START(UnityPerMaterial)
                    float _AlphaClip;
                    float _Hue;
                    float _Saturation;
                    float _Lightness;
                    float _StiffnessVariation;
                    float4 _WindDirectionAndStrength;
                    float4 _Shiver;
                    float _BAKEDMASK_ON;
                    float _UVMASK_ON;
                    float _VERTEXPOSITIONMASK_ON;
                    float4 _EmissionColor;
                    float _RenderQueueType;
                    float _StencilRef;
                    float _StencilWriteMask;
                    float _StencilRefDepth;
                    float _StencilWriteMaskDepth;
                    float _StencilRefMV;
                    float _StencilWriteMaskMV;
                    float _StencilRefDistortionVec;
                    float _StencilWriteMaskDistortionVec;
                    float _StencilWriteMaskGBuffer;
                    float _StencilRefGBuffer;
                    float _ZTestGBuffer;
                    float _RequireSplitLighting;
                    float _ReceivesSSR;
                    float _SurfaceType;
                    float _BlendMode;
                    float _SrcBlend;
                    float _DstBlend;
                    float _AlphaSrcBlend;
                    float _AlphaDstBlend;
                    float _ZWrite;
                    float _CullMode;
                    float _TransparentSortPriority;
                    float _CullModeForward;
                    float _TransparentCullMode;
                    float _ZTestDepthEqualForOpaque;
                    float _ZTestTransparent;
                    float _TransparentBackfaceEnable;
                    float _AlphaCutoffEnable;
                    float _AlphaCutoff;
                    float _UseShadowThreshold;
                    float _DoubleSidedEnable;
                    float _DoubleSidedNormalMode;
                    float4 _DoubleSidedConstants;
                    float _DiffusionProfileHash;
                    float4 _DiffusionProfileAsset;
                    CBUFFER_END
                    TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo); float4 _Albedo_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap); float4 _MaskMap_TexelSize;
                    TEXTURE2D(_ThicknessMap); SAMPLER(sampler_ThicknessMap); float4 _ThicknessMap_TexelSize;
                    float4 _GlobalWindDirectionAndStrength;
                    float4 _GlobalShiver;
                    TEXTURE2D(_ShiverNoise); SAMPLER(sampler_ShiverNoise); float4 _ShiverNoise_TexelSize;
                    TEXTURE2D(_GustNoise); SAMPLER(sampler_GustNoise); float4 _GustNoise_TexelSize;
                    SAMPLER(_SampleTexture2D_12F932C1_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_FFEA8409_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_F86B9939_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_46D09289_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_DBCD6404_Sampler_3_Linear_Repeat);
                
                // Vertex Graph Inputs
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal; // optional
                        float3 WorldSpaceNormal; // optional
                        float3 ObjectSpaceTangent; // optional
                        float3 WorldSpaceTangent; // optional
                        float3 ObjectSpaceBiTangent; // optional
                        float3 WorldSpaceBiTangent; // optional
                        float3 ObjectSpacePosition; // optional
                        float3 AbsoluteWorldSpacePosition; // optional
                        float4 uv0; // optional
                        float4 uv1; // optional
                        float4 VertexColor; // optional
                        float3 TimeParameters; // optional
                    };
                // Vertex Graph Outputs
                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };
                    
                // Pixel Graph Inputs
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal; // optional
                        float4 uv0; // optional
                    };
                // Pixel Graph Outputs
                    struct SurfaceDescription
                    {
                        float3 Normal;
                        float Smoothness;
                        float Alpha;
                        float AlphaClipThreshold;
                    };
                    
                // Shared Graph Node Functions
                
                    struct Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238
                    {
                    };
                
                    void SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 IN, out float3 PivotInWS_0)
                    {
                        PivotInWS_0 = SHADERGRAPH_OBJECT_POSITION;
                    }
                
                    void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                    {
                        Out = lerp(A, B, T);
                    }
                
                    void Unity_Multiply_float (float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                    {
                        RGBA = float4(R, G, B, A);
                        RGB = float3(R, G, B);
                        RG = float2(R, G);
                    }
                
                    void Unity_Length_float3(float3 In, out float Out)
                    {
                        Out = length(In);
                    }
                
                    void Unity_Multiply_float (float A, float B, out float Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }
                
                    void Unity_Normalize_float3(float3 In, out float3 Out)
                    {
                        Out = normalize(In);
                    }
                
                    void Unity_Maximum_float(float A, float B, out float Out)
                    {
                        Out = max(A, B);
                    }
                
                    void Unity_Multiply_float (float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Maximum_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = max(A, B);
                    }
                
                    struct Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7
                    {
                    };
                
                    void SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(float4 Vector4_14B5A446, float4 Vector4_6887180D, float2 Vector2_F270B07E, float2 Vector2_70BD0D1B, Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 IN, out float3 GustDirection_0, out float GustSpeed_1, out float GustStrength_2, out float ShiverSpeed_3, out float ShiverStrength_4)
                    {
                        float3 _Vector3_E24D7903_Out_0 = float3(0.7, 0, 0.3);
                        float4 _Property_95651D48_Out_0 = Vector4_14B5A446;
                        float4 _Property_FFEF34C6_Out_0 = Vector4_6887180D;
                        float4 _Multiply_7F93D556_Out_2;
                        Unity_Multiply_float(_Property_95651D48_Out_0, _Property_FFEF34C6_Out_0, _Multiply_7F93D556_Out_2);
                        float _Split_1A6C2849_R_1 = _Multiply_7F93D556_Out_2[0];
                        float _Split_1A6C2849_G_2 = _Multiply_7F93D556_Out_2[1];
                        float _Split_1A6C2849_B_3 = _Multiply_7F93D556_Out_2[2];
                        float _Split_1A6C2849_A_4 = _Multiply_7F93D556_Out_2[3];
                        float4 _Combine_769EB158_RGBA_4;
                        float3 _Combine_769EB158_RGB_5;
                        float2 _Combine_769EB158_RG_6;
                        Unity_Combine_float(_Split_1A6C2849_R_1, 0, _Split_1A6C2849_G_2, 0, _Combine_769EB158_RGBA_4, _Combine_769EB158_RGB_5, _Combine_769EB158_RG_6);
                        float _Length_62815FED_Out_1;
                        Unity_Length_float3(_Combine_769EB158_RGB_5, _Length_62815FED_Out_1);
                        float _Multiply_A4A39D4F_Out_2;
                        Unity_Multiply_float(_Length_62815FED_Out_1, 1000, _Multiply_A4A39D4F_Out_2);
                        float _Clamp_4B28219D_Out_3;
                        Unity_Clamp_float(_Multiply_A4A39D4F_Out_2, 0, 1, _Clamp_4B28219D_Out_3);
                        float3 _Lerp_66854A50_Out_3;
                        Unity_Lerp_float3(_Vector3_E24D7903_Out_0, _Combine_769EB158_RGB_5, (_Clamp_4B28219D_Out_3.xxx), _Lerp_66854A50_Out_3);
                        float3 _Normalize_B2778668_Out_1;
                        Unity_Normalize_float3(_Lerp_66854A50_Out_3, _Normalize_B2778668_Out_1);
                        float _Maximum_A3AFA1AB_Out_2;
                        Unity_Maximum_float(_Split_1A6C2849_B_3, 0.01, _Maximum_A3AFA1AB_Out_2);
                        float _Maximum_FCE0058_Out_2;
                        Unity_Maximum_float(0, _Split_1A6C2849_A_4, _Maximum_FCE0058_Out_2);
                        float2 _Property_F062BDE_Out_0 = Vector2_F270B07E;
                        float2 _Property_FB73C895_Out_0 = Vector2_70BD0D1B;
                        float2 _Multiply_76AC0593_Out_2;
                        Unity_Multiply_float(_Property_F062BDE_Out_0, _Property_FB73C895_Out_0, _Multiply_76AC0593_Out_2);
                        float2 _Maximum_E318FF04_Out_2;
                        Unity_Maximum_float2(_Multiply_76AC0593_Out_2, float2(0.01, 0.01), _Maximum_E318FF04_Out_2);
                        float _Split_F437A5E0_R_1 = _Maximum_E318FF04_Out_2[0];
                        float _Split_F437A5E0_G_2 = _Maximum_E318FF04_Out_2[1];
                        float _Split_F437A5E0_B_3 = 0;
                        float _Split_F437A5E0_A_4 = 0;
                        GustDirection_0 = _Normalize_B2778668_Out_1;
                        GustSpeed_1 = _Maximum_A3AFA1AB_Out_2;
                        GustStrength_2 = _Maximum_FCE0058_Out_2;
                        ShiverSpeed_3 = _Split_F437A5E0_R_1;
                        ShiverStrength_4 = _Split_F437A5E0_G_2;
                    }
                
                    void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A - B;
                    }
                
                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Fraction_float(float In, out float Out)
                    {
                        Out = frac(In);
                    }
                
                    void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                    {
                        Out = lerp(False, True, Predicate);
                    }
                
                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A - B;
                    }
                
                    struct Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f
                    {
                    };
                
                    void SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(float Vector1_CCF53CDA, float Vector1_D95E40FE, float2 Vector2_AEE18C41, float2 Vector2_A9CE092C, float Vector1_F2ED6CCC, TEXTURE2D_PARAM(Texture2D_F14459DD, samplerTexture2D_F14459DD), float4 Texture2D_F14459DD_TexelSize, Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f IN, out float GustNoise_0)
                    {
                        float2 _Property_A92CC1B7_Out_0 = Vector2_AEE18C41;
                        float _Property_36B40CE_Out_0 = Vector1_D95E40FE;
                        float _Multiply_9E28D3C4_Out_2;
                        Unity_Multiply_float(_Property_36B40CE_Out_0, 2, _Multiply_9E28D3C4_Out_2);
                        float2 _Add_C54F05FE_Out_2;
                        Unity_Add_float2(_Property_A92CC1B7_Out_0, (_Multiply_9E28D3C4_Out_2.xx), _Add_C54F05FE_Out_2);
                        float2 _Multiply_9CD1691E_Out_2;
                        Unity_Multiply_float(_Add_C54F05FE_Out_2, float2(0.01, 0.01), _Multiply_9CD1691E_Out_2);
                        float2 _Property_D05D9ECB_Out_0 = Vector2_A9CE092C;
                        float _Property_8BFC9AA2_Out_0 = Vector1_CCF53CDA;
                        float2 _Multiply_462DF694_Out_2;
                        Unity_Multiply_float(_Property_D05D9ECB_Out_0, (_Property_8BFC9AA2_Out_0.xx), _Multiply_462DF694_Out_2);
                        float _Property_4DB65C54_Out_0 = Vector1_F2ED6CCC;
                        float2 _Multiply_50FD4B48_Out_2;
                        Unity_Multiply_float(_Multiply_462DF694_Out_2, (_Property_4DB65C54_Out_0.xx), _Multiply_50FD4B48_Out_2);
                        float2 _Subtract_B4A749C2_Out_2;
                        Unity_Subtract_float2(_Multiply_9CD1691E_Out_2, _Multiply_50FD4B48_Out_2, _Subtract_B4A749C2_Out_2);
                        float4 _SampleTexture2DLOD_46D09289_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_F14459DD, samplerTexture2D_F14459DD, _Subtract_B4A749C2_Out_2, 0);
                        float _SampleTexture2DLOD_46D09289_R_5 = _SampleTexture2DLOD_46D09289_RGBA_0.r;
                        float _SampleTexture2DLOD_46D09289_G_6 = _SampleTexture2DLOD_46D09289_RGBA_0.g;
                        float _SampleTexture2DLOD_46D09289_B_7 = _SampleTexture2DLOD_46D09289_RGBA_0.b;
                        float _SampleTexture2DLOD_46D09289_A_8 = _SampleTexture2DLOD_46D09289_RGBA_0.a;
                        GustNoise_0 = _SampleTexture2DLOD_46D09289_R_5;
                    }
                
                    void Unity_Power_float(float A, float B, out float Out)
                    {
                        Out = pow(A, B);
                    }
                
                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }
                
                    struct Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19
                    {
                    };
                
                    void SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(float2 Vector2_CA78C39A, float Vector1_279D2776, Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 IN, out float RandomStiffness_0)
                    {
                        float2 _Property_475BFCB9_Out_0 = Vector2_CA78C39A;
                        float2 _Multiply_7EE00C92_Out_2;
                        Unity_Multiply_float(_Property_475BFCB9_Out_0, float2(10, 10), _Multiply_7EE00C92_Out_2);
                        float _Split_A0FB144F_R_1 = _Multiply_7EE00C92_Out_2[0];
                        float _Split_A0FB144F_G_2 = _Multiply_7EE00C92_Out_2[1];
                        float _Split_A0FB144F_B_3 = 0;
                        float _Split_A0FB144F_A_4 = 0;
                        float _Multiply_2482A544_Out_2;
                        Unity_Multiply_float(_Split_A0FB144F_R_1, _Split_A0FB144F_G_2, _Multiply_2482A544_Out_2);
                        float _Fraction_B90029E4_Out_1;
                        Unity_Fraction_float(_Multiply_2482A544_Out_2, _Fraction_B90029E4_Out_1);
                        float _Power_E2B2B095_Out_2;
                        Unity_Power_float(_Fraction_B90029E4_Out_1, 2, _Power_E2B2B095_Out_2);
                        float _Property_91226CD6_Out_0 = Vector1_279D2776;
                        float _OneMinus_A56B8867_Out_1;
                        Unity_OneMinus_float(_Property_91226CD6_Out_0, _OneMinus_A56B8867_Out_1);
                        float _Clamp_E85434A6_Out_3;
                        Unity_Clamp_float(_Power_E2B2B095_Out_2, _OneMinus_A56B8867_Out_1, 1, _Clamp_E85434A6_Out_3);
                        RandomStiffness_0 = _Clamp_E85434A6_Out_3;
                    }
                
                    struct Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628
                    {
                    };
                
                    void SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(float Vector1_AFC49E6C, float Vector1_A18CF4DF, float Vector1_28AC83F8, float Vector1_E0042E1, float Vector1_1A24AAF, Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 IN, out float GustStrength_0)
                    {
                        float _Property_9A741C0D_Out_0 = Vector1_AFC49E6C;
                        float _Property_F564A485_Out_0 = Vector1_A18CF4DF;
                        float _Multiply_248F3A68_Out_2;
                        Unity_Multiply_float(_Property_9A741C0D_Out_0, _Property_F564A485_Out_0, _Multiply_248F3A68_Out_2);
                        float _Clamp_64D749D9_Out_3;
                        Unity_Clamp_float(_Multiply_248F3A68_Out_2, 0.1, 0.9, _Clamp_64D749D9_Out_3);
                        float _OneMinus_BDC5FAC3_Out_1;
                        Unity_OneMinus_float(_Clamp_64D749D9_Out_3, _OneMinus_BDC5FAC3_Out_1);
                        float _Multiply_E3C6FEFE_Out_2;
                        Unity_Multiply_float(_Multiply_248F3A68_Out_2, _OneMinus_BDC5FAC3_Out_1, _Multiply_E3C6FEFE_Out_2);
                        float _Multiply_9087CA8A_Out_2;
                        Unity_Multiply_float(_Multiply_E3C6FEFE_Out_2, 1.5, _Multiply_9087CA8A_Out_2);
                        float _Property_C7E6777F_Out_0 = Vector1_28AC83F8;
                        float _Multiply_1D329CB_Out_2;
                        Unity_Multiply_float(_Multiply_9087CA8A_Out_2, _Property_C7E6777F_Out_0, _Multiply_1D329CB_Out_2);
                        float _Property_84113466_Out_0 = Vector1_E0042E1;
                        float _Multiply_9501294C_Out_2;
                        Unity_Multiply_float(_Multiply_1D329CB_Out_2, _Property_84113466_Out_0, _Multiply_9501294C_Out_2);
                        float _Property_57C5AF03_Out_0 = Vector1_1A24AAF;
                        float _Multiply_E178164E_Out_2;
                        Unity_Multiply_float(_Multiply_9501294C_Out_2, _Property_57C5AF03_Out_0, _Multiply_E178164E_Out_2);
                        GustStrength_0 = _Multiply_E178164E_Out_2;
                    }
                
                    void Unity_Multiply_float (float3 A, float3 B, out float3 Out)
                    {
                        Out = A * B;
                    }
                
                    struct Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a
                    {
                    };
                
                    void SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(float2 Vector2_287CB44E, float2 Vector2_2A17E6EA, float Vector1_F4B6A491, float Vector1_2C90770B, TEXTURE2D_PARAM(Texture2D_D44B4848, samplerTexture2D_D44B4848), float4 Texture2D_D44B4848_TexelSize, float Vector1_AD94E9BC, Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a IN, out float3 ShiverNoise_0)
                    {
                        float2 _Property_961D8A0_Out_0 = Vector2_287CB44E;
                        float _Property_A414F012_Out_0 = Vector1_AD94E9BC;
                        float _Multiply_7DB42988_Out_2;
                        Unity_Multiply_float(_Property_A414F012_Out_0, 2, _Multiply_7DB42988_Out_2);
                        float2 _Add_4C3CF1F_Out_2;
                        Unity_Add_float2(_Property_961D8A0_Out_0, (_Multiply_7DB42988_Out_2.xx), _Add_4C3CF1F_Out_2);
                        float2 _Property_EBC67BC7_Out_0 = Vector2_2A17E6EA;
                        float _Property_13D296B5_Out_0 = Vector1_F4B6A491;
                        float2 _Multiply_BBB72061_Out_2;
                        Unity_Multiply_float(_Property_EBC67BC7_Out_0, (_Property_13D296B5_Out_0.xx), _Multiply_BBB72061_Out_2);
                        float _Property_3BB601E6_Out_0 = Vector1_2C90770B;
                        float2 _Multiply_FF9010E8_Out_2;
                        Unity_Multiply_float(_Multiply_BBB72061_Out_2, (_Property_3BB601E6_Out_0.xx), _Multiply_FF9010E8_Out_2);
                        float2 _Subtract_6BF2D170_Out_2;
                        Unity_Subtract_float2(_Add_4C3CF1F_Out_2, _Multiply_FF9010E8_Out_2, _Subtract_6BF2D170_Out_2);
                        float4 _SampleTexture2DLOD_DBCD6404_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_D44B4848, samplerTexture2D_D44B4848, _Subtract_6BF2D170_Out_2, 0);
                        float _SampleTexture2DLOD_DBCD6404_R_5 = _SampleTexture2DLOD_DBCD6404_RGBA_0.r;
                        float _SampleTexture2DLOD_DBCD6404_G_6 = _SampleTexture2DLOD_DBCD6404_RGBA_0.g;
                        float _SampleTexture2DLOD_DBCD6404_B_7 = _SampleTexture2DLOD_DBCD6404_RGBA_0.b;
                        float _SampleTexture2DLOD_DBCD6404_A_8 = _SampleTexture2DLOD_DBCD6404_RGBA_0.a;
                        float4 _Combine_E5D76A97_RGBA_4;
                        float3 _Combine_E5D76A97_RGB_5;
                        float2 _Combine_E5D76A97_RG_6;
                        Unity_Combine_float(_SampleTexture2DLOD_DBCD6404_R_5, _SampleTexture2DLOD_DBCD6404_G_6, _SampleTexture2DLOD_DBCD6404_B_7, 0, _Combine_E5D76A97_RGBA_4, _Combine_E5D76A97_RGB_5, _Combine_E5D76A97_RG_6);
                        float3 _Subtract_AA7C02E2_Out_2;
                        Unity_Subtract_float3(_Combine_E5D76A97_RGB_5, float3(0.5, 0.5, 0.5), _Subtract_AA7C02E2_Out_2);
                        float3 _Multiply_5BF7CBD7_Out_2;
                        Unity_Multiply_float(_Subtract_AA7C02E2_Out_2, float3(2, 2, 2), _Multiply_5BF7CBD7_Out_2);
                        ShiverNoise_0 = _Multiply_5BF7CBD7_Out_2;
                    }
                
                    struct Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459
                    {
                    };
                
                    void SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(float3 Vector3_ED0F539A, float2 Vector2_84805101, float Vector1_BDF24CF7, float Vector1_839268A4, float Vector1_A8621014, float Vector1_2DBE6CC0, float Vector1_8A4EF006, float Vector1_ED935C73, Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 IN, out float3 ShiverDirection_0, out float ShiverStrength_1)
                    {
                        float3 _Property_FC94AEBB_Out_0 = Vector3_ED0F539A;
                        float _Property_4FE2271A_Out_0 = Vector1_BDF24CF7;
                        float4 _Combine_328044F1_RGBA_4;
                        float3 _Combine_328044F1_RGB_5;
                        float2 _Combine_328044F1_RG_6;
                        Unity_Combine_float(1, _Property_4FE2271A_Out_0, 1, 0, _Combine_328044F1_RGBA_4, _Combine_328044F1_RGB_5, _Combine_328044F1_RG_6);
                        float3 _Multiply_4FCE02F7_Out_2;
                        Unity_Multiply_float(_Property_FC94AEBB_Out_0, _Combine_328044F1_RGB_5, _Multiply_4FCE02F7_Out_2);
                        float2 _Property_77EED0A8_Out_0 = Vector2_84805101;
                        float _Split_2D66AF35_R_1 = _Property_77EED0A8_Out_0[0];
                        float _Split_2D66AF35_G_2 = _Property_77EED0A8_Out_0[1];
                        float _Split_2D66AF35_B_3 = 0;
                        float _Split_2D66AF35_A_4 = 0;
                        float4 _Combine_C2861A09_RGBA_4;
                        float3 _Combine_C2861A09_RGB_5;
                        float2 _Combine_C2861A09_RG_6;
                        Unity_Combine_float(_Split_2D66AF35_R_1, _Property_4FE2271A_Out_0, _Split_2D66AF35_G_2, 0, _Combine_C2861A09_RGBA_4, _Combine_C2861A09_RGB_5, _Combine_C2861A09_RG_6);
                        float3 _Lerp_A6B0BE86_Out_3;
                        Unity_Lerp_float3(_Multiply_4FCE02F7_Out_2, _Combine_C2861A09_RGB_5, float3(0.5, 0.5, 0.5), _Lerp_A6B0BE86_Out_3);
                        float _Property_BBBC9C1B_Out_0 = Vector1_839268A4;
                        float _Length_F022B321_Out_1;
                        Unity_Length_float3(_Multiply_4FCE02F7_Out_2, _Length_F022B321_Out_1);
                        float _Multiply_BFD84B03_Out_2;
                        Unity_Multiply_float(_Length_F022B321_Out_1, 0.5, _Multiply_BFD84B03_Out_2);
                        float _Multiply_3564B68A_Out_2;
                        Unity_Multiply_float(_Property_BBBC9C1B_Out_0, _Multiply_BFD84B03_Out_2, _Multiply_3564B68A_Out_2);
                        float _Add_83285742_Out_2;
                        Unity_Add_float(_Multiply_3564B68A_Out_2, _Length_F022B321_Out_1, _Add_83285742_Out_2);
                        float _Property_45D94B1_Out_0 = Vector1_2DBE6CC0;
                        float _Multiply_EA43D494_Out_2;
                        Unity_Multiply_float(_Add_83285742_Out_2, _Property_45D94B1_Out_0, _Multiply_EA43D494_Out_2);
                        float _Clamp_C109EA71_Out_3;
                        Unity_Clamp_float(_Multiply_EA43D494_Out_2, 0.1, 0.9, _Clamp_C109EA71_Out_3);
                        float _OneMinus_226F3377_Out_1;
                        Unity_OneMinus_float(_Clamp_C109EA71_Out_3, _OneMinus_226F3377_Out_1);
                        float _Multiply_8680628F_Out_2;
                        Unity_Multiply_float(_Multiply_EA43D494_Out_2, _OneMinus_226F3377_Out_1, _Multiply_8680628F_Out_2);
                        float _Multiply_B14E644_Out_2;
                        Unity_Multiply_float(_Multiply_8680628F_Out_2, 1.5, _Multiply_B14E644_Out_2);
                        float _Property_7F61FC78_Out_0 = Vector1_A8621014;
                        float _Multiply_C89CF7DC_Out_2;
                        Unity_Multiply_float(_Multiply_B14E644_Out_2, _Property_7F61FC78_Out_0, _Multiply_C89CF7DC_Out_2);
                        float _Property_2BD306B6_Out_0 = Vector1_8A4EF006;
                        float _Multiply_E5D34DCC_Out_2;
                        Unity_Multiply_float(_Multiply_C89CF7DC_Out_2, _Property_2BD306B6_Out_0, _Multiply_E5D34DCC_Out_2);
                        float _Property_DBC71A4F_Out_0 = Vector1_ED935C73;
                        float _Multiply_BCACDD38_Out_2;
                        Unity_Multiply_float(_Multiply_E5D34DCC_Out_2, _Property_DBC71A4F_Out_0, _Multiply_BCACDD38_Out_2);
                        ShiverDirection_0 = _Lerp_A6B0BE86_Out_3;
                        ShiverStrength_1 = _Multiply_BCACDD38_Out_2;
                    }
                
                    struct Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364
                    {
                    };
                
                    void SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(float3 Vector3_829210A7, float3 Vector3_1A016C4A, float Vector1_31372BF, float Vector1_E57895AF, TEXTURE2D_PARAM(Texture2D_65F71447, samplerTexture2D_65F71447), float4 Texture2D_65F71447_TexelSize, float Vector1_8836FB6A, TEXTURE2D_PARAM(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), float4 Texture2D_4A3BDB6_TexelSize, float Vector1_14E206AE, float Vector1_7090E96C, float Vector1_51722AC, float Vector1_A3894D2, float Vector1_6F0C3A5A, float Vector1_2D1F6C2F, float Vector1_347751CA, Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 IN, out float GustStrength_0, out float ShiverStrength_1, out float3 ShiverDirection_2)
                    {
                        float _Property_5C7F4A8C_Out_0 = Vector1_31372BF;
                        float _Property_142FEDC3_Out_0 = Vector1_347751CA;
                        float3 _Property_D2FC65C3_Out_0 = Vector3_829210A7;
                        float _Split_8E347DCF_R_1 = _Property_D2FC65C3_Out_0[0];
                        float _Split_8E347DCF_G_2 = _Property_D2FC65C3_Out_0[1];
                        float _Split_8E347DCF_B_3 = _Property_D2FC65C3_Out_0[2];
                        float _Split_8E347DCF_A_4 = 0;
                        float4 _Combine_9B5A76B7_RGBA_4;
                        float3 _Combine_9B5A76B7_RGB_5;
                        float2 _Combine_9B5A76B7_RG_6;
                        Unity_Combine_float(_Split_8E347DCF_R_1, _Split_8E347DCF_B_3, 0, 0, _Combine_9B5A76B7_RGBA_4, _Combine_9B5A76B7_RGB_5, _Combine_9B5A76B7_RG_6);
                        float3 _Property_5653999E_Out_0 = Vector3_1A016C4A;
                        float _Split_B9CBBFE5_R_1 = _Property_5653999E_Out_0[0];
                        float _Split_B9CBBFE5_G_2 = _Property_5653999E_Out_0[1];
                        float _Split_B9CBBFE5_B_3 = _Property_5653999E_Out_0[2];
                        float _Split_B9CBBFE5_A_4 = 0;
                        float4 _Combine_DC44394B_RGBA_4;
                        float3 _Combine_DC44394B_RGB_5;
                        float2 _Combine_DC44394B_RG_6;
                        Unity_Combine_float(_Split_B9CBBFE5_R_1, _Split_B9CBBFE5_B_3, 0, 0, _Combine_DC44394B_RGBA_4, _Combine_DC44394B_RGB_5, _Combine_DC44394B_RG_6);
                        float _Property_3221EFCE_Out_0 = Vector1_E57895AF;
                        Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f _GustNoiseAtPosition_3B28852B;
                        float _GustNoiseAtPosition_3B28852B_GustNoise_0;
                        SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(_Property_5C7F4A8C_Out_0, _Property_142FEDC3_Out_0, _Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_3221EFCE_Out_0, TEXTURE2D_ARGS(Texture2D_65F71447, samplerTexture2D_65F71447), Texture2D_65F71447_TexelSize, _GustNoiseAtPosition_3B28852B, _GustNoiseAtPosition_3B28852B_GustNoise_0);
                        float _Property_1B306054_Out_0 = Vector1_A3894D2;
                        float _Property_1FBC768_Out_0 = Vector1_51722AC;
                        float _Property_9FB10D19_Out_0 = Vector1_14E206AE;
                        Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 _RandomStiffnessAtPosition_C9AD50AB;
                        float _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0;
                        SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(_Combine_9B5A76B7_RG_6, _Property_9FB10D19_Out_0, _RandomStiffnessAtPosition_C9AD50AB, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0);
                        float _Property_EE5A603D_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 _CalculateGustStrength_E2853C74;
                        float _CalculateGustStrength_E2853C74_GustStrength_0;
                        SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(_GustNoiseAtPosition_3B28852B_GustNoise_0, _Property_1B306054_Out_0, _Property_1FBC768_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _Property_EE5A603D_Out_0, _CalculateGustStrength_E2853C74, _CalculateGustStrength_E2853C74_GustStrength_0);
                        float _Property_DFB3FCE0_Out_0 = Vector1_31372BF;
                        float _Property_8A8735CC_Out_0 = Vector1_8836FB6A;
                        Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a _ShiverNoiseAtPosition_35F9220A;
                        float3 _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0;
                        SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(_Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_DFB3FCE0_Out_0, _Property_8A8735CC_Out_0, TEXTURE2D_ARGS(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), Texture2D_4A3BDB6_TexelSize, _Property_142FEDC3_Out_0, _ShiverNoiseAtPosition_35F9220A, _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0);
                        float _Property_65F19953_Out_0 = Vector1_6F0C3A5A;
                        float _Property_3A2F45FE_Out_0 = Vector1_51722AC;
                        float _Property_98EF73E5_Out_0 = Vector1_2D1F6C2F;
                        float _Property_6A278DE2_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 _CalculateShiver_799DE4CB;
                        float3 _CalculateShiver_799DE4CB_ShiverDirection_0;
                        float _CalculateShiver_799DE4CB_ShiverStrength_1;
                        SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(_ShiverNoiseAtPosition_35F9220A_ShiverNoise_0, _Combine_DC44394B_RG_6, _Property_65F19953_Out_0, _CalculateGustStrength_E2853C74_GustStrength_0, _Property_3A2F45FE_Out_0, _Property_98EF73E5_Out_0, _Property_6A278DE2_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _CalculateShiver_799DE4CB, _CalculateShiver_799DE4CB_ShiverDirection_0, _CalculateShiver_799DE4CB_ShiverStrength_1);
                        GustStrength_0 = _CalculateGustStrength_E2853C74_GustStrength_0;
                        ShiverStrength_1 = _CalculateShiver_799DE4CB_ShiverStrength_1;
                        ShiverDirection_2 = _CalculateShiver_799DE4CB_ShiverDirection_0;
                    }
                
                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }
                
                    struct Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01
                    {
                        float3 ObjectSpacePosition;
                    };
                
                    void SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(float3 Vector3_C96069F9, float Vector1_A5EB719C, float Vector1_4D1D3B1A, float3 Vector3_C80E97FF, float3 Vector3_821C320A, float3 Vector3_4BF0DC64, Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 IN, out float3 WorldPosition_0)
                    {
                        float3 _Property_65372844_Out_0 = Vector3_4BF0DC64;
                        float3 _Property_7205E35B_Out_0 = Vector3_821C320A;
                        float _Property_916D8D52_Out_0 = Vector1_4D1D3B1A;
                        float3 _Multiply_CF9DF1B5_Out_2;
                        Unity_Multiply_float(_Property_7205E35B_Out_0, (_Property_916D8D52_Out_0.xxx), _Multiply_CF9DF1B5_Out_2);
                        float3 _Multiply_57D2E5C7_Out_2;
                        Unity_Multiply_float(_Multiply_CF9DF1B5_Out_2, float3(10, 10, 10), _Multiply_57D2E5C7_Out_2);
                        float3 _Add_F265DF09_Out_2;
                        Unity_Add_float3(_Property_65372844_Out_0, _Multiply_57D2E5C7_Out_2, _Add_F265DF09_Out_2);
                        float3 _Property_806C350F_Out_0 = Vector3_C96069F9;
                        float _Property_D017A08E_Out_0 = Vector1_A5EB719C;
                        float3 _Multiply_99498CF9_Out_2;
                        Unity_Multiply_float(_Property_806C350F_Out_0, (_Property_D017A08E_Out_0.xxx), _Multiply_99498CF9_Out_2);
                        float _Split_A5777330_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_A5777330_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_A5777330_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_A5777330_A_4 = 0;
                        float _Clamp_C4364CA5_Out_3;
                        Unity_Clamp_float(_Split_A5777330_G_2, 0, 1, _Clamp_C4364CA5_Out_3);
                        float3 _Multiply_ADC4C2A_Out_2;
                        Unity_Multiply_float(_Multiply_99498CF9_Out_2, (_Clamp_C4364CA5_Out_3.xxx), _Multiply_ADC4C2A_Out_2);
                        float3 _Multiply_49835441_Out_2;
                        Unity_Multiply_float(_Multiply_ADC4C2A_Out_2, float3(10, 10, 10), _Multiply_49835441_Out_2);
                        float3 _Add_B14AAE70_Out_2;
                        Unity_Add_float3(_Add_F265DF09_Out_2, _Multiply_49835441_Out_2, _Add_B14AAE70_Out_2);
                        WorldPosition_0 = _Add_B14AAE70_Out_2;
                    }
                
                    struct Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceTangent;
                        float3 WorldSpaceBiTangent;
                    };
                
                    void SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(float3 Vector3_AAF445D6, Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 IN, out float3 ObjectPosition_1)
                    {
                        float3 _Property_51DA8EE_Out_0 = Vector3_AAF445D6;
                        float3 _Subtract_B236C96B_Out_2;
                        Unity_Subtract_float3(_Property_51DA8EE_Out_0, _WorldSpaceCameraPos, _Subtract_B236C96B_Out_2);
                        float3 _Transform_6FDB2E47_Out_1 = TransformWorldToObject(_Subtract_B236C96B_Out_2.xyz);
                        ObjectPosition_1 = _Transform_6FDB2E47_Out_1;
                    }
                
                // Vertex Graph Evaluation
                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 _GetPivotInWorldSpace_73F19E42;
                        float3 _GetPivotInWorldSpace_73F19E42_PivotInWS_0;
                        SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(_GetPivotInWorldSpace_73F19E42, _GetPivotInWorldSpace_73F19E42_PivotInWS_0);
                        float _Split_64420219_R_1 = IN.VertexColor[0];
                        float _Split_64420219_G_2 = IN.VertexColor[1];
                        float _Split_64420219_B_3 = IN.VertexColor[2];
                        float _Split_64420219_A_4 = IN.VertexColor[3];
                        float3 _Lerp_4531CF63_Out_3;
                        Unity_Lerp_float3(_GetPivotInWorldSpace_73F19E42_PivotInWS_0, IN.AbsoluteWorldSpacePosition, (_Split_64420219_G_2.xxx), _Lerp_4531CF63_Out_3);
                        float4 _Property_D6662DC6_Out_0 = _GlobalWindDirectionAndStrength;
                        float4 _Property_9515B228_Out_0 = _WindDirectionAndStrength;
                        float4 _Property_9A1EF240_Out_0 = _GlobalShiver;
                        float4 _Property_777C8DB2_Out_0 = _Shiver;
                        Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 _GlobalWindParameters_B547F135;
                        float3 _GlobalWindParameters_B547F135_GustDirection_0;
                        float _GlobalWindParameters_B547F135_GustSpeed_1;
                        float _GlobalWindParameters_B547F135_GustStrength_2;
                        float _GlobalWindParameters_B547F135_ShiverSpeed_3;
                        float _GlobalWindParameters_B547F135_ShiverStrength_4;
                        SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(_Property_D6662DC6_Out_0, _Property_9515B228_Out_0, (_Property_9A1EF240_Out_0.xy), (_Property_777C8DB2_Out_0.xy), _GlobalWindParameters_B547F135, _GlobalWindParameters_B547F135_GustDirection_0, _GlobalWindParameters_B547F135_GustSpeed_1, _GlobalWindParameters_B547F135_GustStrength_2, _GlobalWindParameters_B547F135_ShiverSpeed_3, _GlobalWindParameters_B547F135_ShiverStrength_4);
                        float _Property_5F3A390D_Out_0 = _BAKEDMASK_ON;
                        float3 _Subtract_BF2A75CD_Out_2;
                        Unity_Subtract_float3(IN.AbsoluteWorldSpacePosition, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _Subtract_BF2A75CD_Out_2);
                        float _Split_29C81DE4_R_1 = _Subtract_BF2A75CD_Out_2[0];
                        float _Split_29C81DE4_G_2 = _Subtract_BF2A75CD_Out_2[1];
                        float _Split_29C81DE4_B_3 = _Subtract_BF2A75CD_Out_2[2];
                        float _Split_29C81DE4_A_4 = 0;
                        float _Add_6A47DB4F_Out_2;
                        Unity_Add_float(_Split_29C81DE4_R_1, _Split_29C81DE4_G_2, _Add_6A47DB4F_Out_2);
                        float _Add_EC455B5D_Out_2;
                        Unity_Add_float(_Add_6A47DB4F_Out_2, _Split_29C81DE4_B_3, _Add_EC455B5D_Out_2);
                        float _Multiply_F013BB8B_Out_2;
                        Unity_Multiply_float(_Add_EC455B5D_Out_2, 0.4, _Multiply_F013BB8B_Out_2);
                        float _Fraction_7D389816_Out_1;
                        Unity_Fraction_float(_Multiply_F013BB8B_Out_2, _Fraction_7D389816_Out_1);
                        float _Multiply_776D3DAF_Out_2;
                        Unity_Multiply_float(_Fraction_7D389816_Out_1, 0.15, _Multiply_776D3DAF_Out_2);
                        float _Split_E4BB9FEC_R_1 = IN.VertexColor[0];
                        float _Split_E4BB9FEC_G_2 = IN.VertexColor[1];
                        float _Split_E4BB9FEC_B_3 = IN.VertexColor[2];
                        float _Split_E4BB9FEC_A_4 = IN.VertexColor[3];
                        float _Multiply_BC8988C3_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, _Split_E4BB9FEC_G_2, _Multiply_BC8988C3_Out_2);
                        float _Multiply_EC5FE292_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_R_1, 0.3, _Multiply_EC5FE292_Out_2);
                        float _Add_A8423510_Out_2;
                        Unity_Add_float(_Multiply_BC8988C3_Out_2, _Multiply_EC5FE292_Out_2, _Add_A8423510_Out_2);
                        float _Add_CE74358C_Out_2;
                        Unity_Add_float(_Add_A8423510_Out_2, IN.TimeParameters.x, _Add_CE74358C_Out_2);
                        float _Multiply_1CE438D_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_G_2, 0.5, _Multiply_1CE438D_Out_2);
                        float _Add_8718B88C_Out_2;
                        Unity_Add_float(_Add_CE74358C_Out_2, _Multiply_1CE438D_Out_2, _Add_8718B88C_Out_2);
                        float _Property_DBA903E3_Out_0 = _UVMASK_ON;
                        float4 _UV_64D01E18_Out_0 = IN.uv0;
                        float _Split_A5DFBEBE_R_1 = _UV_64D01E18_Out_0[0];
                        float _Split_A5DFBEBE_G_2 = _UV_64D01E18_Out_0[1];
                        float _Split_A5DFBEBE_B_3 = _UV_64D01E18_Out_0[2];
                        float _Split_A5DFBEBE_A_4 = _UV_64D01E18_Out_0[3];
                        float _Multiply_C943DA5C_Out_2;
                        Unity_Multiply_float(_Split_A5DFBEBE_G_2, 0.1, _Multiply_C943DA5C_Out_2);
                        float _Branch_12012434_Out_3;
                        Unity_Branch_float(_Property_DBA903E3_Out_0, _Multiply_C943DA5C_Out_2, 0, _Branch_12012434_Out_3);
                        float _Add_922F2E64_Out_2;
                        Unity_Add_float(IN.TimeParameters.x, _Branch_12012434_Out_3, _Add_922F2E64_Out_2);
                        float _Multiply_2E689843_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, 0.5, _Multiply_2E689843_Out_2);
                        float _Add_ED1EE4DB_Out_2;
                        Unity_Add_float(_Add_922F2E64_Out_2, _Multiply_2E689843_Out_2, _Add_ED1EE4DB_Out_2);
                        float _Branch_291934CD_Out_3;
                        Unity_Branch_float(_Property_5F3A390D_Out_0, _Add_8718B88C_Out_2, _Add_ED1EE4DB_Out_2, _Branch_291934CD_Out_3);
                        float _Property_267CF497_Out_0 = _StiffnessVariation;
                        float _Property_4FB02E51_Out_0 = _BAKEDMASK_ON;
                        float4 _UV_6482E163_Out_0 = IN.uv1;
                        float _Split_2D1A67CF_R_1 = _UV_6482E163_Out_0[0];
                        float _Split_2D1A67CF_G_2 = _UV_6482E163_Out_0[1];
                        float _Split_2D1A67CF_B_3 = _UV_6482E163_Out_0[2];
                        float _Split_2D1A67CF_A_4 = _UV_6482E163_Out_0[3];
                        float _Multiply_F7BD1E76_Out_2;
                        Unity_Multiply_float(_Split_2D1A67CF_R_1, _Split_2D1A67CF_G_2, _Multiply_F7BD1E76_Out_2);
                        float _Property_B1FAFDBF_Out_0 = _UVMASK_ON;
                        float4 _UV_8F58F10B_Out_0 = IN.uv0;
                        float _Split_BD0858B3_R_1 = _UV_8F58F10B_Out_0[0];
                        float _Split_BD0858B3_G_2 = _UV_8F58F10B_Out_0[1];
                        float _Split_BD0858B3_B_3 = _UV_8F58F10B_Out_0[2];
                        float _Split_BD0858B3_A_4 = _UV_8F58F10B_Out_0[3];
                        float _Multiply_3FAD9403_Out_2;
                        Unity_Multiply_float(_Split_BD0858B3_G_2, 0.2, _Multiply_3FAD9403_Out_2);
                        float _Branch_3AF3832A_Out_3;
                        Unity_Branch_float(_Property_B1FAFDBF_Out_0, _Multiply_3FAD9403_Out_2, 1, _Branch_3AF3832A_Out_3);
                        float _Branch_F921E5A9_Out_3;
                        Unity_Branch_float(_Property_4FB02E51_Out_0, _Multiply_F7BD1E76_Out_2, _Branch_3AF3832A_Out_3, _Branch_F921E5A9_Out_3);
                        Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 _GetWindStrength_5806EF0A;
                        float _GetWindStrength_5806EF0A_GustStrength_0;
                        float _GetWindStrength_5806EF0A_ShiverStrength_1;
                        float3 _GetWindStrength_5806EF0A_ShiverDirection_2;
                        SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(_Lerp_4531CF63_Out_3, _GlobalWindParameters_B547F135_GustDirection_0, _Branch_291934CD_Out_3, _GlobalWindParameters_B547F135_GustSpeed_1, TEXTURE2D_ARGS(_GustNoise, sampler_GustNoise), _GustNoise_TexelSize, _GlobalWindParameters_B547F135_ShiverSpeed_3, TEXTURE2D_ARGS(_ShiverNoise, sampler_ShiverNoise), _ShiverNoise_TexelSize, _Property_267CF497_Out_0, 1, _Branch_F921E5A9_Out_3, _GlobalWindParameters_B547F135_GustStrength_2, 0.2, _GlobalWindParameters_B547F135_ShiverStrength_4, 0, _GetWindStrength_5806EF0A, _GetWindStrength_5806EF0A_GustStrength_0, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_ShiverDirection_2);
                        Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 _ApplyTreeWindDisplacement_8E73FF2E;
                        _ApplyTreeWindDisplacement_8E73FF2E.ObjectSpacePosition = IN.ObjectSpacePosition;
                        float3 _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0;
                        SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(_GetWindStrength_5806EF0A_ShiverDirection_2, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_GustStrength_0, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _GlobalWindParameters_B547F135_GustDirection_0, IN.AbsoluteWorldSpacePosition, _ApplyTreeWindDisplacement_8E73FF2E, _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0);
                        Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 _WorldToObject_628B231E;
                        _WorldToObject_628B231E.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _WorldToObject_628B231E.WorldSpaceTangent = IN.WorldSpaceTangent;
                        _WorldToObject_628B231E.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                        float3 _WorldToObject_628B231E_ObjectPosition_1;
                        SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(_ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0, _WorldToObject_628B231E, _WorldToObject_628B231E_ObjectPosition_1);
                        description.VertexPosition = _WorldToObject_628B231E_ObjectPosition_1;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }
                    
                // Pixel Graph Evaluation
                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_12F932C1_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                        _SampleTexture2D_12F932C1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_12F932C1_RGBA_0);
                        float _SampleTexture2D_12F932C1_R_4 = _SampleTexture2D_12F932C1_RGBA_0.r;
                        float _SampleTexture2D_12F932C1_G_5 = _SampleTexture2D_12F932C1_RGBA_0.g;
                        float _SampleTexture2D_12F932C1_B_6 = _SampleTexture2D_12F932C1_RGBA_0.b;
                        float _SampleTexture2D_12F932C1_A_7 = _SampleTexture2D_12F932C1_RGBA_0.a;
                        float4 _SampleTexture2D_FFEA8409_RGBA_0 = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, IN.uv0.xy);
                        float _SampleTexture2D_FFEA8409_R_4 = _SampleTexture2D_FFEA8409_RGBA_0.r;
                        float _SampleTexture2D_FFEA8409_G_5 = _SampleTexture2D_FFEA8409_RGBA_0.g;
                        float _SampleTexture2D_FFEA8409_B_6 = _SampleTexture2D_FFEA8409_RGBA_0.b;
                        float _SampleTexture2D_FFEA8409_A_7 = _SampleTexture2D_FFEA8409_RGBA_0.a;
                        float4 _SampleTexture2D_F86B9939_RGBA_0 = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, IN.uv0.xy);
                        float _SampleTexture2D_F86B9939_R_4 = _SampleTexture2D_F86B9939_RGBA_0.r;
                        float _SampleTexture2D_F86B9939_G_5 = _SampleTexture2D_F86B9939_RGBA_0.g;
                        float _SampleTexture2D_F86B9939_B_6 = _SampleTexture2D_F86B9939_RGBA_0.b;
                        float _SampleTexture2D_F86B9939_A_7 = _SampleTexture2D_F86B9939_RGBA_0.a;
                        float _Property_ABA23041_Out_0 = _AlphaClip;
                        surface.Normal = (_SampleTexture2D_12F932C1_RGBA_0.xyz);
                        surface.Smoothness = _SampleTexture2D_FFEA8409_A_7;
                        surface.Alpha = _SampleTexture2D_F86B9939_A_7;
                        surface.AlphaClipThreshold = _Property_ABA23041_Out_0;
                        return surface;
                    }
                    
            //-------------------------------------------------------------------------------------
            // End graph generated code
            //-------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
            
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                // output.ViewSpaceNormal =             TransformWorldToViewDir(output.WorldSpaceNormal);
                // output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                output.ObjectSpacePosition =         input.positionOS;
                // output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                // output.ViewSpacePosition =           TransformWorldToView(output.WorldSpacePosition);
                // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                // output.WorldSpaceViewDirection =     GetWorldSpaceNormalizeViewDir(output.WorldSpacePosition);
                // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(output.WorldSpacePosition), _ProjectionParams.x);
                output.uv0 =                         input.uv0;
                output.uv1 =                         input.uv1;
                // output.uv2 =                         input.uv2;
                // output.uv3 =                         input.uv3;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            {
                // build graph inputs
                VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
                // Override time paramters with used one (This is required to correctly handle motion vector for vertex animation based on time)
                vertexDescriptionInputs.TimeParameters = timeParameters;
            
                // evaluate vertex graph
                VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
            
                // copy graph output to the results
                input.positionOS = vertexDescription.VertexPosition;
                input.normalOS = vertexDescription.VertexNormal;
                input.tangentOS.xyz = vertexDescription.VertexTangent;
            
                return input;
            }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
                FragInputs BuildFragInputs(VaryingsMeshToPS input)
                {
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.positionCS;       // input.positionCS is SV_Position
            
                    output.positionRWS = input.positionRWS;
                    output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
                    output.texCoord0 = input.texCoord0;
                    output.texCoord1 = input.texCoord1;
                    output.texCoord2 = input.texCoord2;
                    output.texCoord3 = input.texCoord3;
                    output.color = input.color;
                    #if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #elif SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #endif // SHADER_STAGE_FRAGMENT
            
                    return output;
                }
            
                SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                    // output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
                    // output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
                    // output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                    // output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
                    // output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
                    // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                    // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                    // output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
                    // output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
                    // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                    // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                    // output.WorldSpaceViewDirection =     normalize(viewWS);
                    // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                    // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                    // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                    // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                    // output.WorldSpacePosition =          input.positionRWS;
                    // output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
                    // output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
                    // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                    // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionRWS);
                    // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
                    output.uv0 =                         input.texCoord0;
                    // output.uv1 =                         input.texCoord1;
                    // output.uv2 =                         input.texCoord2;
                    // output.uv3 =                         input.texCoord3;
                    // output.VertexColor =                 input.color;
                    // output.FaceSign =                    input.isFrontFace;
                    // output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            
                    return output;
                }
            
                // existing HDRP code uses the combined function to go directly from packed to frag inputs
                FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    VaryingsMeshToPS unpacked= UnpackVaryingsMeshToPS(input);
                    return BuildFragInputs(unpacked);
                }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
            void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
            {
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                // copy across graph values, if defined
                // surfaceData.baseColor =                 surfaceDescription.Albedo;
                surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                // surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                // surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                // surfaceData.metallic =                  surfaceDescription.Metallic;
                // surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                // surfaceData.thickness =                 surfaceDescription.Thickness;
                // surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                // surfaceData.specularColor =             surfaceDescription.Specular;
                // surfaceData.coatMask =                  surfaceDescription.CoatMask;
                // surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                // surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                // surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
        #ifdef _HAS_REFRACTION
                if (_EnableSSRefraction)
                {
                    // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                    // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                    // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                    surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                    surfaceDescription.Alpha = 1.0;
                }
                else
                {
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                    surfaceDescription.Alpha = 1.0;
                }
        #else
                surfaceData.ior = 1.0;
                surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                surfaceData.atDistance = 1.0;
                surfaceData.transmittanceMask = 0.0;
        #endif
                
                // These static material feature allow compile time optimization
                surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
        #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
        #endif
        #ifdef _MATERIAL_FEATURE_TRANSMISSION
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
        #endif
        #ifdef _MATERIAL_FEATURE_ANISOTROPY
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
        #endif
                // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
        #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
        #endif
        #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
        #endif
        
        #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                // Require to have setup baseColor
                // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                // tangent-space normal
                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                // compute world space normal
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                bentNormalWS = surfaceData.normalWS;
                // GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
                // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                // If user provide bent normal then we process a better term
        #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                // Just use the value passed through via the slot (not active otherwise)
        #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                // If we have bent normal and ambient occlusion, process a specular occlusion
                surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
        #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
        #else
                surfaceData.specularOcclusion = 1.0;
        #endif
        
        #if HAVE_DECALS
                if (_EnableDecals)
                {
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                }
        #endif
        
        #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
        #endif
        
        #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    // TODO: need to update mip info
                    surfaceData.metallic = 0;
                }
        
                // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                // as it can modify attribute use for static lighting
                ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
        #endif
            }
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
                
                // ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
        
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal 
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                // override sampleBakedGI:
                // builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
                // builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
        
                // builtinData.emissiveColor = surfaceDescription.Emission;
        
                // builtinData.depthOffset = surfaceDescription.DepthOffset;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
            //-------------------------------------------------------------------------------------
            // Pass Includes
            //-------------------------------------------------------------------------------------
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
            //-------------------------------------------------------------------------------------
            // End Pass Includes
            //-------------------------------------------------------------------------------------
        
            ENDHLSL
        }
        
        Pass
        {
            // based on HDLitPass.template
            Name "GBuffer"
            Tags { "LightMode" = "GBuffer" }
        
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull [_CullMode]
        
            ZTest [_ZTestGBuffer]
        
            
            
            // Stencil setup
        Stencil
        {
           WriteMask [_StencilWriteMaskGBuffer]
           Ref [_StencilRefGBuffer]
           Comp Always
           Pass Replace
        }
        
            
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        
            #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            // #define _ENABLE_FOG_ON_TRANSPARENT 1
            #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            #define _DISABLE_DECALS 1
            #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        
            //-------------------------------------------------------------------------------------
            // End Variant Definitions
            //-------------------------------------------------------------------------------------
        
            #pragma vertex Vert
            #pragma fragment Frag
        
            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                    #define SHADERPASS SHADERPASS_GBUFFER
                #pragma multi_compile _ DEBUG_DISPLAY
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ DYNAMICLIGHTMAP_ON
                #pragma multi_compile _ SHADOWS_SHADOWMASK
                #pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
                #pragma multi_compile _ LIGHT_LAYERS
                #define RAYTRACING_SHADER_GRAPH_HIGH
                #ifndef DEBUG_DISPLAY
    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
    #endif
                // ACTIVE FIELDS:
                //   DoubleSided
                //   DoubleSided.Flip
                //   FragInputs.isFrontFace
                //   Material.Translucent
                //   Material.Transmission
                //   AlphaTest
                //   DisableDecals
                //   DisableSSR
                //   Specular.EnergyConserving
                //   AmbientOcclusion
                //   SurfaceDescriptionInputs.TangentSpaceNormal
                //   SurfaceDescriptionInputs.uv0
                //   VertexDescriptionInputs.VertexColor
                //   VertexDescriptionInputs.ObjectSpaceNormal
                //   VertexDescriptionInputs.WorldSpaceNormal
                //   VertexDescriptionInputs.ObjectSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceBiTangent
                //   VertexDescriptionInputs.ObjectSpacePosition
                //   VertexDescriptionInputs.AbsoluteWorldSpacePosition
                //   VertexDescriptionInputs.uv0
                //   VertexDescriptionInputs.uv1
                //   VertexDescriptionInputs.TimeParameters
                //   SurfaceDescription.Albedo
                //   SurfaceDescription.Normal
                //   SurfaceDescription.BentNormal
                //   SurfaceDescription.Thickness
                //   SurfaceDescription.DiffusionProfileHash
                //   SurfaceDescription.CoatMask
                //   SurfaceDescription.Emission
                //   SurfaceDescription.Smoothness
                //   SurfaceDescription.Occlusion
                //   SurfaceDescription.Alpha
                //   SurfaceDescription.AlphaClipThreshold
                //   features.modifyMesh
                //   VertexDescription.VertexPosition
                //   VertexDescription.VertexNormal
                //   VertexDescription.VertexTangent
                //   FragInputs.tangentToWorld
                //   FragInputs.positionRWS
                //   FragInputs.texCoord1
                //   FragInputs.texCoord2
                //   VaryingsMeshToPS.cullFace
                //   FragInputs.texCoord0
                //   AttributesMesh.color
                //   AttributesMesh.normalOS
                //   AttributesMesh.tangentOS
                //   VertexDescriptionInputs.ObjectSpaceBiTangent
                //   AttributesMesh.positionOS
                //   AttributesMesh.uv0
                //   AttributesMesh.uv1
                //   VaryingsMeshToPS.tangentWS
                //   VaryingsMeshToPS.normalWS
                //   VaryingsMeshToPS.positionRWS
                //   VaryingsMeshToPS.texCoord1
                //   VaryingsMeshToPS.texCoord2
                //   VaryingsMeshToPS.texCoord0
                //   AttributesMesh.uv2
                // Shared Graph Keywords
        
            // this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            // #define ATTRIBUTES_NEED_TEXCOORD3
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            #define VARYINGS_NEED_TEXCOORD2
            // #define VARYINGS_NEED_TEXCOORD3
            // #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_CULLFACE
            #define HAVE_MESH_MODIFICATION
        
        // We need isFontFace when using double sided
        #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
            #define VARYINGS_NEED_CULLFACE
        #endif
        
            //-------------------------------------------------------------------------------------
            // End Defines
            //-------------------------------------------------------------------------------------
        	
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
            //-------------------------------------------------------------------------------------
            // Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
            // Generated Type: AttributesMesh
            struct AttributesMesh
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL; // optional
                float4 tangentOS : TANGENT; // optional
                float4 uv0 : TEXCOORD0; // optional
                float4 uv1 : TEXCOORD1; // optional
                float4 uv2 : TEXCOORD2; // optional
                float4 color : COLOR; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            // Generated Type: VaryingsMeshToPS
            struct VaryingsMeshToPS
            {
                float4 positionCS : SV_Position;
                float3 positionRWS; // optional
                float3 normalWS; // optional
                float4 tangentWS; // optional
                float4 texCoord0; // optional
                float4 texCoord1; // optional
                float4 texCoord2; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            };
            
            // Generated Type: PackedVaryingsMeshToPS
            struct PackedVaryingsMeshToPS
            {
                float4 positionCS : SV_Position; // unpacked
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
                float4 interp02 : TEXCOORD2; // auto-packed
                float4 interp03 : TEXCOORD3; // auto-packed
                float4 interp04 : TEXCOORD4; // auto-packed
                float4 interp05 : TEXCOORD5; // auto-packed
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
                #endif // conditional
            };
            
            // Packed Type: VaryingsMeshToPS
            PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
            {
                PackedVaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyzw = input.texCoord1;
                output.interp05.xyzw = input.texCoord2;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToPS
            VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
            {
                VaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.texCoord1 = input.interp04.xyzw;
                output.texCoord2 = input.interp05.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            // Generated Type: VaryingsMeshToDS
            struct VaryingsMeshToDS
            {
                float3 positionRWS;
                float3 normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            
            // Generated Type: PackedVaryingsMeshToDS
            struct PackedVaryingsMeshToDS
            {
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
            };
            
            // Packed Type: VaryingsMeshToDS
            PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
            {
                PackedVaryingsMeshToDS output;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToDS
            VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
            {
                VaryingsMeshToDS output;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            //-------------------------------------------------------------------------------------
            // End Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
        
            //-------------------------------------------------------------------------------------
            // Graph generated code
            //-------------------------------------------------------------------------------------
                    // Shared Graph Properties (uniform inputs)
                    CBUFFER_START(UnityPerMaterial)
                    float _AlphaClip;
                    float _Hue;
                    float _Saturation;
                    float _Lightness;
                    float _StiffnessVariation;
                    float4 _WindDirectionAndStrength;
                    float4 _Shiver;
                    float _BAKEDMASK_ON;
                    float _UVMASK_ON;
                    float _VERTEXPOSITIONMASK_ON;
                    float4 _EmissionColor;
                    float _RenderQueueType;
                    float _StencilRef;
                    float _StencilWriteMask;
                    float _StencilRefDepth;
                    float _StencilWriteMaskDepth;
                    float _StencilRefMV;
                    float _StencilWriteMaskMV;
                    float _StencilRefDistortionVec;
                    float _StencilWriteMaskDistortionVec;
                    float _StencilWriteMaskGBuffer;
                    float _StencilRefGBuffer;
                    float _ZTestGBuffer;
                    float _RequireSplitLighting;
                    float _ReceivesSSR;
                    float _SurfaceType;
                    float _BlendMode;
                    float _SrcBlend;
                    float _DstBlend;
                    float _AlphaSrcBlend;
                    float _AlphaDstBlend;
                    float _ZWrite;
                    float _CullMode;
                    float _TransparentSortPriority;
                    float _CullModeForward;
                    float _TransparentCullMode;
                    float _ZTestDepthEqualForOpaque;
                    float _ZTestTransparent;
                    float _TransparentBackfaceEnable;
                    float _AlphaCutoffEnable;
                    float _AlphaCutoff;
                    float _UseShadowThreshold;
                    float _DoubleSidedEnable;
                    float _DoubleSidedNormalMode;
                    float4 _DoubleSidedConstants;
                    float _DiffusionProfileHash;
                    float4 _DiffusionProfileAsset;
                    CBUFFER_END
                    TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo); float4 _Albedo_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap); float4 _MaskMap_TexelSize;
                    TEXTURE2D(_ThicknessMap); SAMPLER(sampler_ThicknessMap); float4 _ThicknessMap_TexelSize;
                    float4 _GlobalWindDirectionAndStrength;
                    float4 _GlobalShiver;
                    TEXTURE2D(_ShiverNoise); SAMPLER(sampler_ShiverNoise); float4 _ShiverNoise_TexelSize;
                    TEXTURE2D(_GustNoise); SAMPLER(sampler_GustNoise); float4 _GustNoise_TexelSize;
                    SAMPLER(_SampleTexture2D_F86B9939_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_12F932C1_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_E3683686_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_FFEA8409_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_46D09289_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_DBCD6404_Sampler_3_Linear_Repeat);
                
                // Vertex Graph Inputs
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal; // optional
                        float3 WorldSpaceNormal; // optional
                        float3 ObjectSpaceTangent; // optional
                        float3 WorldSpaceTangent; // optional
                        float3 ObjectSpaceBiTangent; // optional
                        float3 WorldSpaceBiTangent; // optional
                        float3 ObjectSpacePosition; // optional
                        float3 AbsoluteWorldSpacePosition; // optional
                        float4 uv0; // optional
                        float4 uv1; // optional
                        float4 VertexColor; // optional
                        float3 TimeParameters; // optional
                    };
                // Vertex Graph Outputs
                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };
                    
                // Pixel Graph Inputs
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal; // optional
                        float4 uv0; // optional
                    };
                // Pixel Graph Outputs
                    struct SurfaceDescription
                    {
                        float3 Albedo;
                        float3 Normal;
                        float3 BentNormal;
                        float Thickness;
                        float DiffusionProfileHash;
                        float CoatMask;
                        float3 Emission;
                        float Smoothness;
                        float Occlusion;
                        float Alpha;
                        float AlphaClipThreshold;
                    };
                    
                // Shared Graph Node Functions
                
                    void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
                    {
                        // RGB to HSV
                        float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                        float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                        float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                        float D = Q.x - min(Q.w, Q.y);
                        float E = 1e-4;
                        float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);
                
                        float hue = hsv.x + Offset;
                        hsv.x = (hue < 0)
                                ? hue + 1
                                : (hue > 1)
                                    ? hue - 1
                                    : hue;
                
                        // HSV to RGB
                        float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                        float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
                        Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
                    }
                
                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
                    {
                        float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
                        Out =  luma.xxx + Saturation.xxx * (In - luma.xxx);
                    }
                
                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }
                
                    struct Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238
                    {
                    };
                
                    void SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 IN, out float3 PivotInWS_0)
                    {
                        PivotInWS_0 = SHADERGRAPH_OBJECT_POSITION;
                    }
                
                    void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                    {
                        Out = lerp(A, B, T);
                    }
                
                    void Unity_Multiply_float (float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                    {
                        RGBA = float4(R, G, B, A);
                        RGB = float3(R, G, B);
                        RG = float2(R, G);
                    }
                
                    void Unity_Length_float3(float3 In, out float Out)
                    {
                        Out = length(In);
                    }
                
                    void Unity_Multiply_float (float A, float B, out float Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }
                
                    void Unity_Normalize_float3(float3 In, out float3 Out)
                    {
                        Out = normalize(In);
                    }
                
                    void Unity_Maximum_float(float A, float B, out float Out)
                    {
                        Out = max(A, B);
                    }
                
                    void Unity_Multiply_float (float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Maximum_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = max(A, B);
                    }
                
                    struct Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7
                    {
                    };
                
                    void SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(float4 Vector4_14B5A446, float4 Vector4_6887180D, float2 Vector2_F270B07E, float2 Vector2_70BD0D1B, Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 IN, out float3 GustDirection_0, out float GustSpeed_1, out float GustStrength_2, out float ShiverSpeed_3, out float ShiverStrength_4)
                    {
                        float3 _Vector3_E24D7903_Out_0 = float3(0.7, 0, 0.3);
                        float4 _Property_95651D48_Out_0 = Vector4_14B5A446;
                        float4 _Property_FFEF34C6_Out_0 = Vector4_6887180D;
                        float4 _Multiply_7F93D556_Out_2;
                        Unity_Multiply_float(_Property_95651D48_Out_0, _Property_FFEF34C6_Out_0, _Multiply_7F93D556_Out_2);
                        float _Split_1A6C2849_R_1 = _Multiply_7F93D556_Out_2[0];
                        float _Split_1A6C2849_G_2 = _Multiply_7F93D556_Out_2[1];
                        float _Split_1A6C2849_B_3 = _Multiply_7F93D556_Out_2[2];
                        float _Split_1A6C2849_A_4 = _Multiply_7F93D556_Out_2[3];
                        float4 _Combine_769EB158_RGBA_4;
                        float3 _Combine_769EB158_RGB_5;
                        float2 _Combine_769EB158_RG_6;
                        Unity_Combine_float(_Split_1A6C2849_R_1, 0, _Split_1A6C2849_G_2, 0, _Combine_769EB158_RGBA_4, _Combine_769EB158_RGB_5, _Combine_769EB158_RG_6);
                        float _Length_62815FED_Out_1;
                        Unity_Length_float3(_Combine_769EB158_RGB_5, _Length_62815FED_Out_1);
                        float _Multiply_A4A39D4F_Out_2;
                        Unity_Multiply_float(_Length_62815FED_Out_1, 1000, _Multiply_A4A39D4F_Out_2);
                        float _Clamp_4B28219D_Out_3;
                        Unity_Clamp_float(_Multiply_A4A39D4F_Out_2, 0, 1, _Clamp_4B28219D_Out_3);
                        float3 _Lerp_66854A50_Out_3;
                        Unity_Lerp_float3(_Vector3_E24D7903_Out_0, _Combine_769EB158_RGB_5, (_Clamp_4B28219D_Out_3.xxx), _Lerp_66854A50_Out_3);
                        float3 _Normalize_B2778668_Out_1;
                        Unity_Normalize_float3(_Lerp_66854A50_Out_3, _Normalize_B2778668_Out_1);
                        float _Maximum_A3AFA1AB_Out_2;
                        Unity_Maximum_float(_Split_1A6C2849_B_3, 0.01, _Maximum_A3AFA1AB_Out_2);
                        float _Maximum_FCE0058_Out_2;
                        Unity_Maximum_float(0, _Split_1A6C2849_A_4, _Maximum_FCE0058_Out_2);
                        float2 _Property_F062BDE_Out_0 = Vector2_F270B07E;
                        float2 _Property_FB73C895_Out_0 = Vector2_70BD0D1B;
                        float2 _Multiply_76AC0593_Out_2;
                        Unity_Multiply_float(_Property_F062BDE_Out_0, _Property_FB73C895_Out_0, _Multiply_76AC0593_Out_2);
                        float2 _Maximum_E318FF04_Out_2;
                        Unity_Maximum_float2(_Multiply_76AC0593_Out_2, float2(0.01, 0.01), _Maximum_E318FF04_Out_2);
                        float _Split_F437A5E0_R_1 = _Maximum_E318FF04_Out_2[0];
                        float _Split_F437A5E0_G_2 = _Maximum_E318FF04_Out_2[1];
                        float _Split_F437A5E0_B_3 = 0;
                        float _Split_F437A5E0_A_4 = 0;
                        GustDirection_0 = _Normalize_B2778668_Out_1;
                        GustSpeed_1 = _Maximum_A3AFA1AB_Out_2;
                        GustStrength_2 = _Maximum_FCE0058_Out_2;
                        ShiverSpeed_3 = _Split_F437A5E0_R_1;
                        ShiverStrength_4 = _Split_F437A5E0_G_2;
                    }
                
                    void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A - B;
                    }
                
                    void Unity_Fraction_float(float In, out float Out)
                    {
                        Out = frac(In);
                    }
                
                    void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                    {
                        Out = lerp(False, True, Predicate);
                    }
                
                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A - B;
                    }
                
                    struct Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f
                    {
                    };
                
                    void SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(float Vector1_CCF53CDA, float Vector1_D95E40FE, float2 Vector2_AEE18C41, float2 Vector2_A9CE092C, float Vector1_F2ED6CCC, TEXTURE2D_PARAM(Texture2D_F14459DD, samplerTexture2D_F14459DD), float4 Texture2D_F14459DD_TexelSize, Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f IN, out float GustNoise_0)
                    {
                        float2 _Property_A92CC1B7_Out_0 = Vector2_AEE18C41;
                        float _Property_36B40CE_Out_0 = Vector1_D95E40FE;
                        float _Multiply_9E28D3C4_Out_2;
                        Unity_Multiply_float(_Property_36B40CE_Out_0, 2, _Multiply_9E28D3C4_Out_2);
                        float2 _Add_C54F05FE_Out_2;
                        Unity_Add_float2(_Property_A92CC1B7_Out_0, (_Multiply_9E28D3C4_Out_2.xx), _Add_C54F05FE_Out_2);
                        float2 _Multiply_9CD1691E_Out_2;
                        Unity_Multiply_float(_Add_C54F05FE_Out_2, float2(0.01, 0.01), _Multiply_9CD1691E_Out_2);
                        float2 _Property_D05D9ECB_Out_0 = Vector2_A9CE092C;
                        float _Property_8BFC9AA2_Out_0 = Vector1_CCF53CDA;
                        float2 _Multiply_462DF694_Out_2;
                        Unity_Multiply_float(_Property_D05D9ECB_Out_0, (_Property_8BFC9AA2_Out_0.xx), _Multiply_462DF694_Out_2);
                        float _Property_4DB65C54_Out_0 = Vector1_F2ED6CCC;
                        float2 _Multiply_50FD4B48_Out_2;
                        Unity_Multiply_float(_Multiply_462DF694_Out_2, (_Property_4DB65C54_Out_0.xx), _Multiply_50FD4B48_Out_2);
                        float2 _Subtract_B4A749C2_Out_2;
                        Unity_Subtract_float2(_Multiply_9CD1691E_Out_2, _Multiply_50FD4B48_Out_2, _Subtract_B4A749C2_Out_2);
                        float4 _SampleTexture2DLOD_46D09289_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_F14459DD, samplerTexture2D_F14459DD, _Subtract_B4A749C2_Out_2, 0);
                        float _SampleTexture2DLOD_46D09289_R_5 = _SampleTexture2DLOD_46D09289_RGBA_0.r;
                        float _SampleTexture2DLOD_46D09289_G_6 = _SampleTexture2DLOD_46D09289_RGBA_0.g;
                        float _SampleTexture2DLOD_46D09289_B_7 = _SampleTexture2DLOD_46D09289_RGBA_0.b;
                        float _SampleTexture2DLOD_46D09289_A_8 = _SampleTexture2DLOD_46D09289_RGBA_0.a;
                        GustNoise_0 = _SampleTexture2DLOD_46D09289_R_5;
                    }
                
                    void Unity_Power_float(float A, float B, out float Out)
                    {
                        Out = pow(A, B);
                    }
                
                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }
                
                    struct Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19
                    {
                    };
                
                    void SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(float2 Vector2_CA78C39A, float Vector1_279D2776, Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 IN, out float RandomStiffness_0)
                    {
                        float2 _Property_475BFCB9_Out_0 = Vector2_CA78C39A;
                        float2 _Multiply_7EE00C92_Out_2;
                        Unity_Multiply_float(_Property_475BFCB9_Out_0, float2(10, 10), _Multiply_7EE00C92_Out_2);
                        float _Split_A0FB144F_R_1 = _Multiply_7EE00C92_Out_2[0];
                        float _Split_A0FB144F_G_2 = _Multiply_7EE00C92_Out_2[1];
                        float _Split_A0FB144F_B_3 = 0;
                        float _Split_A0FB144F_A_4 = 0;
                        float _Multiply_2482A544_Out_2;
                        Unity_Multiply_float(_Split_A0FB144F_R_1, _Split_A0FB144F_G_2, _Multiply_2482A544_Out_2);
                        float _Fraction_B90029E4_Out_1;
                        Unity_Fraction_float(_Multiply_2482A544_Out_2, _Fraction_B90029E4_Out_1);
                        float _Power_E2B2B095_Out_2;
                        Unity_Power_float(_Fraction_B90029E4_Out_1, 2, _Power_E2B2B095_Out_2);
                        float _Property_91226CD6_Out_0 = Vector1_279D2776;
                        float _OneMinus_A56B8867_Out_1;
                        Unity_OneMinus_float(_Property_91226CD6_Out_0, _OneMinus_A56B8867_Out_1);
                        float _Clamp_E85434A6_Out_3;
                        Unity_Clamp_float(_Power_E2B2B095_Out_2, _OneMinus_A56B8867_Out_1, 1, _Clamp_E85434A6_Out_3);
                        RandomStiffness_0 = _Clamp_E85434A6_Out_3;
                    }
                
                    struct Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628
                    {
                    };
                
                    void SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(float Vector1_AFC49E6C, float Vector1_A18CF4DF, float Vector1_28AC83F8, float Vector1_E0042E1, float Vector1_1A24AAF, Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 IN, out float GustStrength_0)
                    {
                        float _Property_9A741C0D_Out_0 = Vector1_AFC49E6C;
                        float _Property_F564A485_Out_0 = Vector1_A18CF4DF;
                        float _Multiply_248F3A68_Out_2;
                        Unity_Multiply_float(_Property_9A741C0D_Out_0, _Property_F564A485_Out_0, _Multiply_248F3A68_Out_2);
                        float _Clamp_64D749D9_Out_3;
                        Unity_Clamp_float(_Multiply_248F3A68_Out_2, 0.1, 0.9, _Clamp_64D749D9_Out_3);
                        float _OneMinus_BDC5FAC3_Out_1;
                        Unity_OneMinus_float(_Clamp_64D749D9_Out_3, _OneMinus_BDC5FAC3_Out_1);
                        float _Multiply_E3C6FEFE_Out_2;
                        Unity_Multiply_float(_Multiply_248F3A68_Out_2, _OneMinus_BDC5FAC3_Out_1, _Multiply_E3C6FEFE_Out_2);
                        float _Multiply_9087CA8A_Out_2;
                        Unity_Multiply_float(_Multiply_E3C6FEFE_Out_2, 1.5, _Multiply_9087CA8A_Out_2);
                        float _Property_C7E6777F_Out_0 = Vector1_28AC83F8;
                        float _Multiply_1D329CB_Out_2;
                        Unity_Multiply_float(_Multiply_9087CA8A_Out_2, _Property_C7E6777F_Out_0, _Multiply_1D329CB_Out_2);
                        float _Property_84113466_Out_0 = Vector1_E0042E1;
                        float _Multiply_9501294C_Out_2;
                        Unity_Multiply_float(_Multiply_1D329CB_Out_2, _Property_84113466_Out_0, _Multiply_9501294C_Out_2);
                        float _Property_57C5AF03_Out_0 = Vector1_1A24AAF;
                        float _Multiply_E178164E_Out_2;
                        Unity_Multiply_float(_Multiply_9501294C_Out_2, _Property_57C5AF03_Out_0, _Multiply_E178164E_Out_2);
                        GustStrength_0 = _Multiply_E178164E_Out_2;
                    }
                
                    void Unity_Multiply_float (float3 A, float3 B, out float3 Out)
                    {
                        Out = A * B;
                    }
                
                    struct Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a
                    {
                    };
                
                    void SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(float2 Vector2_287CB44E, float2 Vector2_2A17E6EA, float Vector1_F4B6A491, float Vector1_2C90770B, TEXTURE2D_PARAM(Texture2D_D44B4848, samplerTexture2D_D44B4848), float4 Texture2D_D44B4848_TexelSize, float Vector1_AD94E9BC, Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a IN, out float3 ShiverNoise_0)
                    {
                        float2 _Property_961D8A0_Out_0 = Vector2_287CB44E;
                        float _Property_A414F012_Out_0 = Vector1_AD94E9BC;
                        float _Multiply_7DB42988_Out_2;
                        Unity_Multiply_float(_Property_A414F012_Out_0, 2, _Multiply_7DB42988_Out_2);
                        float2 _Add_4C3CF1F_Out_2;
                        Unity_Add_float2(_Property_961D8A0_Out_0, (_Multiply_7DB42988_Out_2.xx), _Add_4C3CF1F_Out_2);
                        float2 _Property_EBC67BC7_Out_0 = Vector2_2A17E6EA;
                        float _Property_13D296B5_Out_0 = Vector1_F4B6A491;
                        float2 _Multiply_BBB72061_Out_2;
                        Unity_Multiply_float(_Property_EBC67BC7_Out_0, (_Property_13D296B5_Out_0.xx), _Multiply_BBB72061_Out_2);
                        float _Property_3BB601E6_Out_0 = Vector1_2C90770B;
                        float2 _Multiply_FF9010E8_Out_2;
                        Unity_Multiply_float(_Multiply_BBB72061_Out_2, (_Property_3BB601E6_Out_0.xx), _Multiply_FF9010E8_Out_2);
                        float2 _Subtract_6BF2D170_Out_2;
                        Unity_Subtract_float2(_Add_4C3CF1F_Out_2, _Multiply_FF9010E8_Out_2, _Subtract_6BF2D170_Out_2);
                        float4 _SampleTexture2DLOD_DBCD6404_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_D44B4848, samplerTexture2D_D44B4848, _Subtract_6BF2D170_Out_2, 0);
                        float _SampleTexture2DLOD_DBCD6404_R_5 = _SampleTexture2DLOD_DBCD6404_RGBA_0.r;
                        float _SampleTexture2DLOD_DBCD6404_G_6 = _SampleTexture2DLOD_DBCD6404_RGBA_0.g;
                        float _SampleTexture2DLOD_DBCD6404_B_7 = _SampleTexture2DLOD_DBCD6404_RGBA_0.b;
                        float _SampleTexture2DLOD_DBCD6404_A_8 = _SampleTexture2DLOD_DBCD6404_RGBA_0.a;
                        float4 _Combine_E5D76A97_RGBA_4;
                        float3 _Combine_E5D76A97_RGB_5;
                        float2 _Combine_E5D76A97_RG_6;
                        Unity_Combine_float(_SampleTexture2DLOD_DBCD6404_R_5, _SampleTexture2DLOD_DBCD6404_G_6, _SampleTexture2DLOD_DBCD6404_B_7, 0, _Combine_E5D76A97_RGBA_4, _Combine_E5D76A97_RGB_5, _Combine_E5D76A97_RG_6);
                        float3 _Subtract_AA7C02E2_Out_2;
                        Unity_Subtract_float3(_Combine_E5D76A97_RGB_5, float3(0.5, 0.5, 0.5), _Subtract_AA7C02E2_Out_2);
                        float3 _Multiply_5BF7CBD7_Out_2;
                        Unity_Multiply_float(_Subtract_AA7C02E2_Out_2, float3(2, 2, 2), _Multiply_5BF7CBD7_Out_2);
                        ShiverNoise_0 = _Multiply_5BF7CBD7_Out_2;
                    }
                
                    struct Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459
                    {
                    };
                
                    void SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(float3 Vector3_ED0F539A, float2 Vector2_84805101, float Vector1_BDF24CF7, float Vector1_839268A4, float Vector1_A8621014, float Vector1_2DBE6CC0, float Vector1_8A4EF006, float Vector1_ED935C73, Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 IN, out float3 ShiverDirection_0, out float ShiverStrength_1)
                    {
                        float3 _Property_FC94AEBB_Out_0 = Vector3_ED0F539A;
                        float _Property_4FE2271A_Out_0 = Vector1_BDF24CF7;
                        float4 _Combine_328044F1_RGBA_4;
                        float3 _Combine_328044F1_RGB_5;
                        float2 _Combine_328044F1_RG_6;
                        Unity_Combine_float(1, _Property_4FE2271A_Out_0, 1, 0, _Combine_328044F1_RGBA_4, _Combine_328044F1_RGB_5, _Combine_328044F1_RG_6);
                        float3 _Multiply_4FCE02F7_Out_2;
                        Unity_Multiply_float(_Property_FC94AEBB_Out_0, _Combine_328044F1_RGB_5, _Multiply_4FCE02F7_Out_2);
                        float2 _Property_77EED0A8_Out_0 = Vector2_84805101;
                        float _Split_2D66AF35_R_1 = _Property_77EED0A8_Out_0[0];
                        float _Split_2D66AF35_G_2 = _Property_77EED0A8_Out_0[1];
                        float _Split_2D66AF35_B_3 = 0;
                        float _Split_2D66AF35_A_4 = 0;
                        float4 _Combine_C2861A09_RGBA_4;
                        float3 _Combine_C2861A09_RGB_5;
                        float2 _Combine_C2861A09_RG_6;
                        Unity_Combine_float(_Split_2D66AF35_R_1, _Property_4FE2271A_Out_0, _Split_2D66AF35_G_2, 0, _Combine_C2861A09_RGBA_4, _Combine_C2861A09_RGB_5, _Combine_C2861A09_RG_6);
                        float3 _Lerp_A6B0BE86_Out_3;
                        Unity_Lerp_float3(_Multiply_4FCE02F7_Out_2, _Combine_C2861A09_RGB_5, float3(0.5, 0.5, 0.5), _Lerp_A6B0BE86_Out_3);
                        float _Property_BBBC9C1B_Out_0 = Vector1_839268A4;
                        float _Length_F022B321_Out_1;
                        Unity_Length_float3(_Multiply_4FCE02F7_Out_2, _Length_F022B321_Out_1);
                        float _Multiply_BFD84B03_Out_2;
                        Unity_Multiply_float(_Length_F022B321_Out_1, 0.5, _Multiply_BFD84B03_Out_2);
                        float _Multiply_3564B68A_Out_2;
                        Unity_Multiply_float(_Property_BBBC9C1B_Out_0, _Multiply_BFD84B03_Out_2, _Multiply_3564B68A_Out_2);
                        float _Add_83285742_Out_2;
                        Unity_Add_float(_Multiply_3564B68A_Out_2, _Length_F022B321_Out_1, _Add_83285742_Out_2);
                        float _Property_45D94B1_Out_0 = Vector1_2DBE6CC0;
                        float _Multiply_EA43D494_Out_2;
                        Unity_Multiply_float(_Add_83285742_Out_2, _Property_45D94B1_Out_0, _Multiply_EA43D494_Out_2);
                        float _Clamp_C109EA71_Out_3;
                        Unity_Clamp_float(_Multiply_EA43D494_Out_2, 0.1, 0.9, _Clamp_C109EA71_Out_3);
                        float _OneMinus_226F3377_Out_1;
                        Unity_OneMinus_float(_Clamp_C109EA71_Out_3, _OneMinus_226F3377_Out_1);
                        float _Multiply_8680628F_Out_2;
                        Unity_Multiply_float(_Multiply_EA43D494_Out_2, _OneMinus_226F3377_Out_1, _Multiply_8680628F_Out_2);
                        float _Multiply_B14E644_Out_2;
                        Unity_Multiply_float(_Multiply_8680628F_Out_2, 1.5, _Multiply_B14E644_Out_2);
                        float _Property_7F61FC78_Out_0 = Vector1_A8621014;
                        float _Multiply_C89CF7DC_Out_2;
                        Unity_Multiply_float(_Multiply_B14E644_Out_2, _Property_7F61FC78_Out_0, _Multiply_C89CF7DC_Out_2);
                        float _Property_2BD306B6_Out_0 = Vector1_8A4EF006;
                        float _Multiply_E5D34DCC_Out_2;
                        Unity_Multiply_float(_Multiply_C89CF7DC_Out_2, _Property_2BD306B6_Out_0, _Multiply_E5D34DCC_Out_2);
                        float _Property_DBC71A4F_Out_0 = Vector1_ED935C73;
                        float _Multiply_BCACDD38_Out_2;
                        Unity_Multiply_float(_Multiply_E5D34DCC_Out_2, _Property_DBC71A4F_Out_0, _Multiply_BCACDD38_Out_2);
                        ShiverDirection_0 = _Lerp_A6B0BE86_Out_3;
                        ShiverStrength_1 = _Multiply_BCACDD38_Out_2;
                    }
                
                    struct Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364
                    {
                    };
                
                    void SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(float3 Vector3_829210A7, float3 Vector3_1A016C4A, float Vector1_31372BF, float Vector1_E57895AF, TEXTURE2D_PARAM(Texture2D_65F71447, samplerTexture2D_65F71447), float4 Texture2D_65F71447_TexelSize, float Vector1_8836FB6A, TEXTURE2D_PARAM(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), float4 Texture2D_4A3BDB6_TexelSize, float Vector1_14E206AE, float Vector1_7090E96C, float Vector1_51722AC, float Vector1_A3894D2, float Vector1_6F0C3A5A, float Vector1_2D1F6C2F, float Vector1_347751CA, Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 IN, out float GustStrength_0, out float ShiverStrength_1, out float3 ShiverDirection_2)
                    {
                        float _Property_5C7F4A8C_Out_0 = Vector1_31372BF;
                        float _Property_142FEDC3_Out_0 = Vector1_347751CA;
                        float3 _Property_D2FC65C3_Out_0 = Vector3_829210A7;
                        float _Split_8E347DCF_R_1 = _Property_D2FC65C3_Out_0[0];
                        float _Split_8E347DCF_G_2 = _Property_D2FC65C3_Out_0[1];
                        float _Split_8E347DCF_B_3 = _Property_D2FC65C3_Out_0[2];
                        float _Split_8E347DCF_A_4 = 0;
                        float4 _Combine_9B5A76B7_RGBA_4;
                        float3 _Combine_9B5A76B7_RGB_5;
                        float2 _Combine_9B5A76B7_RG_6;
                        Unity_Combine_float(_Split_8E347DCF_R_1, _Split_8E347DCF_B_3, 0, 0, _Combine_9B5A76B7_RGBA_4, _Combine_9B5A76B7_RGB_5, _Combine_9B5A76B7_RG_6);
                        float3 _Property_5653999E_Out_0 = Vector3_1A016C4A;
                        float _Split_B9CBBFE5_R_1 = _Property_5653999E_Out_0[0];
                        float _Split_B9CBBFE5_G_2 = _Property_5653999E_Out_0[1];
                        float _Split_B9CBBFE5_B_3 = _Property_5653999E_Out_0[2];
                        float _Split_B9CBBFE5_A_4 = 0;
                        float4 _Combine_DC44394B_RGBA_4;
                        float3 _Combine_DC44394B_RGB_5;
                        float2 _Combine_DC44394B_RG_6;
                        Unity_Combine_float(_Split_B9CBBFE5_R_1, _Split_B9CBBFE5_B_3, 0, 0, _Combine_DC44394B_RGBA_4, _Combine_DC44394B_RGB_5, _Combine_DC44394B_RG_6);
                        float _Property_3221EFCE_Out_0 = Vector1_E57895AF;
                        Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f _GustNoiseAtPosition_3B28852B;
                        float _GustNoiseAtPosition_3B28852B_GustNoise_0;
                        SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(_Property_5C7F4A8C_Out_0, _Property_142FEDC3_Out_0, _Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_3221EFCE_Out_0, TEXTURE2D_ARGS(Texture2D_65F71447, samplerTexture2D_65F71447), Texture2D_65F71447_TexelSize, _GustNoiseAtPosition_3B28852B, _GustNoiseAtPosition_3B28852B_GustNoise_0);
                        float _Property_1B306054_Out_0 = Vector1_A3894D2;
                        float _Property_1FBC768_Out_0 = Vector1_51722AC;
                        float _Property_9FB10D19_Out_0 = Vector1_14E206AE;
                        Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 _RandomStiffnessAtPosition_C9AD50AB;
                        float _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0;
                        SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(_Combine_9B5A76B7_RG_6, _Property_9FB10D19_Out_0, _RandomStiffnessAtPosition_C9AD50AB, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0);
                        float _Property_EE5A603D_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 _CalculateGustStrength_E2853C74;
                        float _CalculateGustStrength_E2853C74_GustStrength_0;
                        SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(_GustNoiseAtPosition_3B28852B_GustNoise_0, _Property_1B306054_Out_0, _Property_1FBC768_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _Property_EE5A603D_Out_0, _CalculateGustStrength_E2853C74, _CalculateGustStrength_E2853C74_GustStrength_0);
                        float _Property_DFB3FCE0_Out_0 = Vector1_31372BF;
                        float _Property_8A8735CC_Out_0 = Vector1_8836FB6A;
                        Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a _ShiverNoiseAtPosition_35F9220A;
                        float3 _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0;
                        SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(_Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_DFB3FCE0_Out_0, _Property_8A8735CC_Out_0, TEXTURE2D_ARGS(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), Texture2D_4A3BDB6_TexelSize, _Property_142FEDC3_Out_0, _ShiverNoiseAtPosition_35F9220A, _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0);
                        float _Property_65F19953_Out_0 = Vector1_6F0C3A5A;
                        float _Property_3A2F45FE_Out_0 = Vector1_51722AC;
                        float _Property_98EF73E5_Out_0 = Vector1_2D1F6C2F;
                        float _Property_6A278DE2_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 _CalculateShiver_799DE4CB;
                        float3 _CalculateShiver_799DE4CB_ShiverDirection_0;
                        float _CalculateShiver_799DE4CB_ShiverStrength_1;
                        SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(_ShiverNoiseAtPosition_35F9220A_ShiverNoise_0, _Combine_DC44394B_RG_6, _Property_65F19953_Out_0, _CalculateGustStrength_E2853C74_GustStrength_0, _Property_3A2F45FE_Out_0, _Property_98EF73E5_Out_0, _Property_6A278DE2_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _CalculateShiver_799DE4CB, _CalculateShiver_799DE4CB_ShiverDirection_0, _CalculateShiver_799DE4CB_ShiverStrength_1);
                        GustStrength_0 = _CalculateGustStrength_E2853C74_GustStrength_0;
                        ShiverStrength_1 = _CalculateShiver_799DE4CB_ShiverStrength_1;
                        ShiverDirection_2 = _CalculateShiver_799DE4CB_ShiverDirection_0;
                    }
                
                    struct Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01
                    {
                        float3 ObjectSpacePosition;
                    };
                
                    void SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(float3 Vector3_C96069F9, float Vector1_A5EB719C, float Vector1_4D1D3B1A, float3 Vector3_C80E97FF, float3 Vector3_821C320A, float3 Vector3_4BF0DC64, Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 IN, out float3 WorldPosition_0)
                    {
                        float3 _Property_65372844_Out_0 = Vector3_4BF0DC64;
                        float3 _Property_7205E35B_Out_0 = Vector3_821C320A;
                        float _Property_916D8D52_Out_0 = Vector1_4D1D3B1A;
                        float3 _Multiply_CF9DF1B5_Out_2;
                        Unity_Multiply_float(_Property_7205E35B_Out_0, (_Property_916D8D52_Out_0.xxx), _Multiply_CF9DF1B5_Out_2);
                        float3 _Multiply_57D2E5C7_Out_2;
                        Unity_Multiply_float(_Multiply_CF9DF1B5_Out_2, float3(10, 10, 10), _Multiply_57D2E5C7_Out_2);
                        float3 _Add_F265DF09_Out_2;
                        Unity_Add_float3(_Property_65372844_Out_0, _Multiply_57D2E5C7_Out_2, _Add_F265DF09_Out_2);
                        float3 _Property_806C350F_Out_0 = Vector3_C96069F9;
                        float _Property_D017A08E_Out_0 = Vector1_A5EB719C;
                        float3 _Multiply_99498CF9_Out_2;
                        Unity_Multiply_float(_Property_806C350F_Out_0, (_Property_D017A08E_Out_0.xxx), _Multiply_99498CF9_Out_2);
                        float _Split_A5777330_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_A5777330_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_A5777330_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_A5777330_A_4 = 0;
                        float _Clamp_C4364CA5_Out_3;
                        Unity_Clamp_float(_Split_A5777330_G_2, 0, 1, _Clamp_C4364CA5_Out_3);
                        float3 _Multiply_ADC4C2A_Out_2;
                        Unity_Multiply_float(_Multiply_99498CF9_Out_2, (_Clamp_C4364CA5_Out_3.xxx), _Multiply_ADC4C2A_Out_2);
                        float3 _Multiply_49835441_Out_2;
                        Unity_Multiply_float(_Multiply_ADC4C2A_Out_2, float3(10, 10, 10), _Multiply_49835441_Out_2);
                        float3 _Add_B14AAE70_Out_2;
                        Unity_Add_float3(_Add_F265DF09_Out_2, _Multiply_49835441_Out_2, _Add_B14AAE70_Out_2);
                        WorldPosition_0 = _Add_B14AAE70_Out_2;
                    }
                
                    struct Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceTangent;
                        float3 WorldSpaceBiTangent;
                    };
                
                    void SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(float3 Vector3_AAF445D6, Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 IN, out float3 ObjectPosition_1)
                    {
                        float3 _Property_51DA8EE_Out_0 = Vector3_AAF445D6;
                        float3 _Subtract_B236C96B_Out_2;
                        Unity_Subtract_float3(_Property_51DA8EE_Out_0, _WorldSpaceCameraPos, _Subtract_B236C96B_Out_2);
                        float3 _Transform_6FDB2E47_Out_1 = TransformWorldToObject(_Subtract_B236C96B_Out_2.xyz);
                        ObjectPosition_1 = _Transform_6FDB2E47_Out_1;
                    }
                
                // Vertex Graph Evaluation
                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 _GetPivotInWorldSpace_73F19E42;
                        float3 _GetPivotInWorldSpace_73F19E42_PivotInWS_0;
                        SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(_GetPivotInWorldSpace_73F19E42, _GetPivotInWorldSpace_73F19E42_PivotInWS_0);
                        float _Split_64420219_R_1 = IN.VertexColor[0];
                        float _Split_64420219_G_2 = IN.VertexColor[1];
                        float _Split_64420219_B_3 = IN.VertexColor[2];
                        float _Split_64420219_A_4 = IN.VertexColor[3];
                        float3 _Lerp_4531CF63_Out_3;
                        Unity_Lerp_float3(_GetPivotInWorldSpace_73F19E42_PivotInWS_0, IN.AbsoluteWorldSpacePosition, (_Split_64420219_G_2.xxx), _Lerp_4531CF63_Out_3);
                        float4 _Property_D6662DC6_Out_0 = _GlobalWindDirectionAndStrength;
                        float4 _Property_9515B228_Out_0 = _WindDirectionAndStrength;
                        float4 _Property_9A1EF240_Out_0 = _GlobalShiver;
                        float4 _Property_777C8DB2_Out_0 = _Shiver;
                        Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 _GlobalWindParameters_B547F135;
                        float3 _GlobalWindParameters_B547F135_GustDirection_0;
                        float _GlobalWindParameters_B547F135_GustSpeed_1;
                        float _GlobalWindParameters_B547F135_GustStrength_2;
                        float _GlobalWindParameters_B547F135_ShiverSpeed_3;
                        float _GlobalWindParameters_B547F135_ShiverStrength_4;
                        SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(_Property_D6662DC6_Out_0, _Property_9515B228_Out_0, (_Property_9A1EF240_Out_0.xy), (_Property_777C8DB2_Out_0.xy), _GlobalWindParameters_B547F135, _GlobalWindParameters_B547F135_GustDirection_0, _GlobalWindParameters_B547F135_GustSpeed_1, _GlobalWindParameters_B547F135_GustStrength_2, _GlobalWindParameters_B547F135_ShiverSpeed_3, _GlobalWindParameters_B547F135_ShiverStrength_4);
                        float _Property_5F3A390D_Out_0 = _BAKEDMASK_ON;
                        float3 _Subtract_BF2A75CD_Out_2;
                        Unity_Subtract_float3(IN.AbsoluteWorldSpacePosition, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _Subtract_BF2A75CD_Out_2);
                        float _Split_29C81DE4_R_1 = _Subtract_BF2A75CD_Out_2[0];
                        float _Split_29C81DE4_G_2 = _Subtract_BF2A75CD_Out_2[1];
                        float _Split_29C81DE4_B_3 = _Subtract_BF2A75CD_Out_2[2];
                        float _Split_29C81DE4_A_4 = 0;
                        float _Add_6A47DB4F_Out_2;
                        Unity_Add_float(_Split_29C81DE4_R_1, _Split_29C81DE4_G_2, _Add_6A47DB4F_Out_2);
                        float _Add_EC455B5D_Out_2;
                        Unity_Add_float(_Add_6A47DB4F_Out_2, _Split_29C81DE4_B_3, _Add_EC455B5D_Out_2);
                        float _Multiply_F013BB8B_Out_2;
                        Unity_Multiply_float(_Add_EC455B5D_Out_2, 0.4, _Multiply_F013BB8B_Out_2);
                        float _Fraction_7D389816_Out_1;
                        Unity_Fraction_float(_Multiply_F013BB8B_Out_2, _Fraction_7D389816_Out_1);
                        float _Multiply_776D3DAF_Out_2;
                        Unity_Multiply_float(_Fraction_7D389816_Out_1, 0.15, _Multiply_776D3DAF_Out_2);
                        float _Split_E4BB9FEC_R_1 = IN.VertexColor[0];
                        float _Split_E4BB9FEC_G_2 = IN.VertexColor[1];
                        float _Split_E4BB9FEC_B_3 = IN.VertexColor[2];
                        float _Split_E4BB9FEC_A_4 = IN.VertexColor[3];
                        float _Multiply_BC8988C3_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, _Split_E4BB9FEC_G_2, _Multiply_BC8988C3_Out_2);
                        float _Multiply_EC5FE292_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_R_1, 0.3, _Multiply_EC5FE292_Out_2);
                        float _Add_A8423510_Out_2;
                        Unity_Add_float(_Multiply_BC8988C3_Out_2, _Multiply_EC5FE292_Out_2, _Add_A8423510_Out_2);
                        float _Add_CE74358C_Out_2;
                        Unity_Add_float(_Add_A8423510_Out_2, IN.TimeParameters.x, _Add_CE74358C_Out_2);
                        float _Multiply_1CE438D_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_G_2, 0.5, _Multiply_1CE438D_Out_2);
                        float _Add_8718B88C_Out_2;
                        Unity_Add_float(_Add_CE74358C_Out_2, _Multiply_1CE438D_Out_2, _Add_8718B88C_Out_2);
                        float _Property_DBA903E3_Out_0 = _UVMASK_ON;
                        float4 _UV_64D01E18_Out_0 = IN.uv0;
                        float _Split_A5DFBEBE_R_1 = _UV_64D01E18_Out_0[0];
                        float _Split_A5DFBEBE_G_2 = _UV_64D01E18_Out_0[1];
                        float _Split_A5DFBEBE_B_3 = _UV_64D01E18_Out_0[2];
                        float _Split_A5DFBEBE_A_4 = _UV_64D01E18_Out_0[3];
                        float _Multiply_C943DA5C_Out_2;
                        Unity_Multiply_float(_Split_A5DFBEBE_G_2, 0.1, _Multiply_C943DA5C_Out_2);
                        float _Branch_12012434_Out_3;
                        Unity_Branch_float(_Property_DBA903E3_Out_0, _Multiply_C943DA5C_Out_2, 0, _Branch_12012434_Out_3);
                        float _Add_922F2E64_Out_2;
                        Unity_Add_float(IN.TimeParameters.x, _Branch_12012434_Out_3, _Add_922F2E64_Out_2);
                        float _Multiply_2E689843_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, 0.5, _Multiply_2E689843_Out_2);
                        float _Add_ED1EE4DB_Out_2;
                        Unity_Add_float(_Add_922F2E64_Out_2, _Multiply_2E689843_Out_2, _Add_ED1EE4DB_Out_2);
                        float _Branch_291934CD_Out_3;
                        Unity_Branch_float(_Property_5F3A390D_Out_0, _Add_8718B88C_Out_2, _Add_ED1EE4DB_Out_2, _Branch_291934CD_Out_3);
                        float _Property_267CF497_Out_0 = _StiffnessVariation;
                        float _Property_4FB02E51_Out_0 = _BAKEDMASK_ON;
                        float4 _UV_6482E163_Out_0 = IN.uv1;
                        float _Split_2D1A67CF_R_1 = _UV_6482E163_Out_0[0];
                        float _Split_2D1A67CF_G_2 = _UV_6482E163_Out_0[1];
                        float _Split_2D1A67CF_B_3 = _UV_6482E163_Out_0[2];
                        float _Split_2D1A67CF_A_4 = _UV_6482E163_Out_0[3];
                        float _Multiply_F7BD1E76_Out_2;
                        Unity_Multiply_float(_Split_2D1A67CF_R_1, _Split_2D1A67CF_G_2, _Multiply_F7BD1E76_Out_2);
                        float _Property_B1FAFDBF_Out_0 = _UVMASK_ON;
                        float4 _UV_8F58F10B_Out_0 = IN.uv0;
                        float _Split_BD0858B3_R_1 = _UV_8F58F10B_Out_0[0];
                        float _Split_BD0858B3_G_2 = _UV_8F58F10B_Out_0[1];
                        float _Split_BD0858B3_B_3 = _UV_8F58F10B_Out_0[2];
                        float _Split_BD0858B3_A_4 = _UV_8F58F10B_Out_0[3];
                        float _Multiply_3FAD9403_Out_2;
                        Unity_Multiply_float(_Split_BD0858B3_G_2, 0.2, _Multiply_3FAD9403_Out_2);
                        float _Branch_3AF3832A_Out_3;
                        Unity_Branch_float(_Property_B1FAFDBF_Out_0, _Multiply_3FAD9403_Out_2, 1, _Branch_3AF3832A_Out_3);
                        float _Branch_F921E5A9_Out_3;
                        Unity_Branch_float(_Property_4FB02E51_Out_0, _Multiply_F7BD1E76_Out_2, _Branch_3AF3832A_Out_3, _Branch_F921E5A9_Out_3);
                        Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 _GetWindStrength_5806EF0A;
                        float _GetWindStrength_5806EF0A_GustStrength_0;
                        float _GetWindStrength_5806EF0A_ShiverStrength_1;
                        float3 _GetWindStrength_5806EF0A_ShiverDirection_2;
                        SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(_Lerp_4531CF63_Out_3, _GlobalWindParameters_B547F135_GustDirection_0, _Branch_291934CD_Out_3, _GlobalWindParameters_B547F135_GustSpeed_1, TEXTURE2D_ARGS(_GustNoise, sampler_GustNoise), _GustNoise_TexelSize, _GlobalWindParameters_B547F135_ShiverSpeed_3, TEXTURE2D_ARGS(_ShiverNoise, sampler_ShiverNoise), _ShiverNoise_TexelSize, _Property_267CF497_Out_0, 1, _Branch_F921E5A9_Out_3, _GlobalWindParameters_B547F135_GustStrength_2, 0.2, _GlobalWindParameters_B547F135_ShiverStrength_4, 0, _GetWindStrength_5806EF0A, _GetWindStrength_5806EF0A_GustStrength_0, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_ShiverDirection_2);
                        Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 _ApplyTreeWindDisplacement_8E73FF2E;
                        _ApplyTreeWindDisplacement_8E73FF2E.ObjectSpacePosition = IN.ObjectSpacePosition;
                        float3 _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0;
                        SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(_GetWindStrength_5806EF0A_ShiverDirection_2, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_GustStrength_0, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _GlobalWindParameters_B547F135_GustDirection_0, IN.AbsoluteWorldSpacePosition, _ApplyTreeWindDisplacement_8E73FF2E, _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0);
                        Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 _WorldToObject_628B231E;
                        _WorldToObject_628B231E.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _WorldToObject_628B231E.WorldSpaceTangent = IN.WorldSpaceTangent;
                        _WorldToObject_628B231E.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                        float3 _WorldToObject_628B231E_ObjectPosition_1;
                        SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(_ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0, _WorldToObject_628B231E, _WorldToObject_628B231E_ObjectPosition_1);
                        description.VertexPosition = _WorldToObject_628B231E_ObjectPosition_1;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }
                    
                // Pixel Graph Evaluation
                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_F86B9939_RGBA_0 = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, IN.uv0.xy);
                        float _SampleTexture2D_F86B9939_R_4 = _SampleTexture2D_F86B9939_RGBA_0.r;
                        float _SampleTexture2D_F86B9939_G_5 = _SampleTexture2D_F86B9939_RGBA_0.g;
                        float _SampleTexture2D_F86B9939_B_6 = _SampleTexture2D_F86B9939_RGBA_0.b;
                        float _SampleTexture2D_F86B9939_A_7 = _SampleTexture2D_F86B9939_RGBA_0.a;
                        float _Property_667D0001_Out_0 = _Hue;
                        float3 _Hue_BE270ED0_Out_2;
                        Unity_Hue_Normalized_float((_SampleTexture2D_F86B9939_RGBA_0.xyz), _Property_667D0001_Out_0, _Hue_BE270ED0_Out_2);
                        float _Property_306B4B17_Out_0 = _Saturation;
                        float _Add_27F91AF7_Out_2;
                        Unity_Add_float(_Property_306B4B17_Out_0, 1, _Add_27F91AF7_Out_2);
                        float3 _Saturation_8EFFDFE8_Out_2;
                        Unity_Saturation_float(_Hue_BE270ED0_Out_2, _Add_27F91AF7_Out_2, _Saturation_8EFFDFE8_Out_2);
                        float _Property_35742C6B_Out_0 = _Lightness;
                        float3 _Add_53649F0F_Out_2;
                        Unity_Add_float3(_Saturation_8EFFDFE8_Out_2, (_Property_35742C6B_Out_0.xxx), _Add_53649F0F_Out_2);
                        float4 _SampleTexture2D_12F932C1_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                        _SampleTexture2D_12F932C1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_12F932C1_RGBA_0);
                        float _SampleTexture2D_12F932C1_R_4 = _SampleTexture2D_12F932C1_RGBA_0.r;
                        float _SampleTexture2D_12F932C1_G_5 = _SampleTexture2D_12F932C1_RGBA_0.g;
                        float _SampleTexture2D_12F932C1_B_6 = _SampleTexture2D_12F932C1_RGBA_0.b;
                        float _SampleTexture2D_12F932C1_A_7 = _SampleTexture2D_12F932C1_RGBA_0.a;
                        float4 _SampleTexture2D_E3683686_RGBA_0 = SAMPLE_TEXTURE2D(_ThicknessMap, sampler_ThicknessMap, IN.uv0.xy);
                        float _SampleTexture2D_E3683686_R_4 = _SampleTexture2D_E3683686_RGBA_0.r;
                        float _SampleTexture2D_E3683686_G_5 = _SampleTexture2D_E3683686_RGBA_0.g;
                        float _SampleTexture2D_E3683686_B_6 = _SampleTexture2D_E3683686_RGBA_0.b;
                        float _SampleTexture2D_E3683686_A_7 = _SampleTexture2D_E3683686_RGBA_0.a;
                        float4 _SampleTexture2D_FFEA8409_RGBA_0 = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, IN.uv0.xy);
                        float _SampleTexture2D_FFEA8409_R_4 = _SampleTexture2D_FFEA8409_RGBA_0.r;
                        float _SampleTexture2D_FFEA8409_G_5 = _SampleTexture2D_FFEA8409_RGBA_0.g;
                        float _SampleTexture2D_FFEA8409_B_6 = _SampleTexture2D_FFEA8409_RGBA_0.b;
                        float _SampleTexture2D_FFEA8409_A_7 = _SampleTexture2D_FFEA8409_RGBA_0.a;
                        float _Property_ABA23041_Out_0 = _AlphaClip;
                        surface.Albedo = _Add_53649F0F_Out_2;
                        surface.Normal = (_SampleTexture2D_12F932C1_RGBA_0.xyz);
                        surface.BentNormal = IN.TangentSpaceNormal;
                        surface.Thickness = _SampleTexture2D_E3683686_R_4;
                        surface.DiffusionProfileHash = _DiffusionProfileHash;
                        surface.CoatMask = 0;
                        surface.Emission = float3(0, 0, 0);
                        surface.Smoothness = _SampleTexture2D_FFEA8409_A_7;
                        surface.Occlusion = _SampleTexture2D_FFEA8409_G_5;
                        surface.Alpha = _SampleTexture2D_F86B9939_A_7;
                        surface.AlphaClipThreshold = _Property_ABA23041_Out_0;
                        return surface;
                    }
                    
            //-------------------------------------------------------------------------------------
            // End graph generated code
            //-------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
            
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                // output.ViewSpaceNormal =             TransformWorldToViewDir(output.WorldSpaceNormal);
                // output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                output.ObjectSpacePosition =         input.positionOS;
                // output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                // output.ViewSpacePosition =           TransformWorldToView(output.WorldSpacePosition);
                // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                // output.WorldSpaceViewDirection =     GetWorldSpaceNormalizeViewDir(output.WorldSpacePosition);
                // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(output.WorldSpacePosition), _ProjectionParams.x);
                output.uv0 =                         input.uv0;
                output.uv1 =                         input.uv1;
                // output.uv2 =                         input.uv2;
                // output.uv3 =                         input.uv3;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            {
                // build graph inputs
                VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
                // Override time paramters with used one (This is required to correctly handle motion vector for vertex animation based on time)
                vertexDescriptionInputs.TimeParameters = timeParameters;
            
                // evaluate vertex graph
                VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
            
                // copy graph output to the results
                input.positionOS = vertexDescription.VertexPosition;
                input.normalOS = vertexDescription.VertexNormal;
                input.tangentOS.xyz = vertexDescription.VertexTangent;
            
                return input;
            }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
                FragInputs BuildFragInputs(VaryingsMeshToPS input)
                {
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.positionCS;       // input.positionCS is SV_Position
            
                    output.positionRWS = input.positionRWS;
                    output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
                    output.texCoord0 = input.texCoord0;
                    output.texCoord1 = input.texCoord1;
                    output.texCoord2 = input.texCoord2;
                    // output.texCoord3 = input.texCoord3;
                    // output.color = input.color;
                    #if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #elif SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #endif // SHADER_STAGE_FRAGMENT
            
                    return output;
                }
            
                SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                    // output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
                    // output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
                    // output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                    // output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
                    // output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
                    // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                    // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                    // output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
                    // output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
                    // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                    // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                    // output.WorldSpaceViewDirection =     normalize(viewWS);
                    // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                    // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                    // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                    // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                    // output.WorldSpacePosition =          input.positionRWS;
                    // output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
                    // output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
                    // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                    // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionRWS);
                    // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
                    output.uv0 =                         input.texCoord0;
                    // output.uv1 =                         input.texCoord1;
                    // output.uv2 =                         input.texCoord2;
                    // output.uv3 =                         input.texCoord3;
                    // output.VertexColor =                 input.color;
                    // output.FaceSign =                    input.isFrontFace;
                    // output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            
                    return output;
                }
            
                // existing HDRP code uses the combined function to go directly from packed to frag inputs
                FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    VaryingsMeshToPS unpacked= UnpackVaryingsMeshToPS(input);
                    return BuildFragInputs(unpacked);
                }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
            void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
            {
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                // copy across graph values, if defined
                surfaceData.baseColor =                 surfaceDescription.Albedo;
                surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                // surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                // surfaceData.metallic =                  surfaceDescription.Metallic;
                // surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                surfaceData.thickness =                 surfaceDescription.Thickness;
                surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                // surfaceData.specularColor =             surfaceDescription.Specular;
                surfaceData.coatMask =                  surfaceDescription.CoatMask;
                // surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                // surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                // surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
        #ifdef _HAS_REFRACTION
                if (_EnableSSRefraction)
                {
                    // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                    // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                    // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                    surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                    surfaceDescription.Alpha = 1.0;
                }
                else
                {
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                    surfaceDescription.Alpha = 1.0;
                }
        #else
                surfaceData.ior = 1.0;
                surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                surfaceData.atDistance = 1.0;
                surfaceData.transmittanceMask = 0.0;
        #endif
                
                // These static material feature allow compile time optimization
                surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
        #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
        #endif
        #ifdef _MATERIAL_FEATURE_TRANSMISSION
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
        #endif
        #ifdef _MATERIAL_FEATURE_ANISOTROPY
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
        #endif
                // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
        #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
        #endif
        #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
        #endif
        
        #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                // Require to have setup baseColor
                // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                // tangent-space normal
                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                // compute world space normal
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                bentNormalWS = surfaceData.normalWS;
                // GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
                // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                // If user provide bent normal then we process a better term
        #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                // Just use the value passed through via the slot (not active otherwise)
        #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                // If we have bent normal and ambient occlusion, process a specular occlusion
                surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
        #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
        #else
                surfaceData.specularOcclusion = 1.0;
        #endif
        
        #if HAVE_DECALS
                if (_EnableDecals)
                {
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                }
        #endif
        
        #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
        #endif
        
        #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    // TODO: need to update mip info
                    surfaceData.metallic = 0;
                }
        
                // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                // as it can modify attribute use for static lighting
                ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
        #endif
            }
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
                
                // ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
        
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal 
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                // override sampleBakedGI:
                // builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
                // builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // builtinData.depthOffset = surfaceDescription.DepthOffset;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
            //-------------------------------------------------------------------------------------
            // Pass Includes
            //-------------------------------------------------------------------------------------
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassGBuffer.hlsl"
            //-------------------------------------------------------------------------------------
            // End Pass Includes
            //-------------------------------------------------------------------------------------
        
            ENDHLSL
        }
        
        Pass
        {
            // based on HDLitPass.template
            Name "MotionVectors"
            Tags { "LightMode" = "MotionVectors" }
        
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            
            Cull [_CullMode]
        
            
            ZWrite On
        
            
            // Stencil setup
        Stencil
        {
           WriteMask [_StencilWriteMaskMV]
           Ref [_StencilRefMV]
           Comp Always
           Pass Replace
        }
        
            
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        
            #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            // #define _ENABLE_FOG_ON_TRANSPARENT 1
            // #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            #define _DISABLE_DECALS 1
            #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        
            //-------------------------------------------------------------------------------------
            // End Variant Definitions
            //-------------------------------------------------------------------------------------
        
            #pragma vertex Vert
            #pragma fragment Frag
        
            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                    #define SHADERPASS SHADERPASS_MOTION_VECTORS
                #pragma multi_compile _ WRITE_NORMAL_BUFFER
                #pragma multi_compile _ WRITE_MSAA_DEPTH
                #define RAYTRACING_SHADER_GRAPH_HIGH
                // ACTIVE FIELDS:
                //   DoubleSided
                //   Material.Translucent
                //   Material.Transmission
                //   AlphaTest
                //   DisableDecals
                //   DisableSSR
                //   Specular.EnergyConserving
                //   SurfaceDescriptionInputs.TangentSpaceNormal
                //   SurfaceDescriptionInputs.uv0
                //   VertexDescriptionInputs.VertexColor
                //   VertexDescriptionInputs.ObjectSpaceNormal
                //   VertexDescriptionInputs.WorldSpaceNormal
                //   VertexDescriptionInputs.ObjectSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceBiTangent
                //   VertexDescriptionInputs.ObjectSpacePosition
                //   VertexDescriptionInputs.AbsoluteWorldSpacePosition
                //   VertexDescriptionInputs.uv0
                //   VertexDescriptionInputs.uv1
                //   VertexDescriptionInputs.TimeParameters
                //   SurfaceDescription.Normal
                //   SurfaceDescription.Smoothness
                //   SurfaceDescription.Alpha
                //   SurfaceDescription.AlphaClipThreshold
                //   features.modifyMesh
                //   VertexDescription.VertexPosition
                //   VertexDescription.VertexNormal
                //   VertexDescription.VertexTangent
                //   AttributesMesh.normalOS
                //   AttributesMesh.tangentOS
                //   AttributesMesh.uv0
                //   AttributesMesh.uv1
                //   AttributesMesh.color
                //   AttributesMesh.uv2
                //   AttributesMesh.uv3
                //   FragInputs.tangentToWorld
                //   FragInputs.positionRWS
                //   FragInputs.texCoord0
                //   FragInputs.texCoord1
                //   FragInputs.texCoord2
                //   FragInputs.texCoord3
                //   FragInputs.color
                //   VertexDescriptionInputs.ObjectSpaceBiTangent
                //   AttributesMesh.positionOS
                //   VaryingsMeshToPS.tangentWS
                //   VaryingsMeshToPS.normalWS
                //   VaryingsMeshToPS.positionRWS
                //   VaryingsMeshToPS.texCoord0
                //   VaryingsMeshToPS.texCoord1
                //   VaryingsMeshToPS.texCoord2
                //   VaryingsMeshToPS.texCoord3
                //   VaryingsMeshToPS.color
                // Shared Graph Keywords
        
            // this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            #define VARYINGS_NEED_TEXCOORD2
            #define VARYINGS_NEED_TEXCOORD3
            #define VARYINGS_NEED_COLOR
            // #define VARYINGS_NEED_CULLFACE
            #define HAVE_MESH_MODIFICATION
        
        // We need isFontFace when using double sided
        #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
            #define VARYINGS_NEED_CULLFACE
        #endif
        
            //-------------------------------------------------------------------------------------
            // End Defines
            //-------------------------------------------------------------------------------------
        	
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
            //-------------------------------------------------------------------------------------
            // Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
            // Generated Type: AttributesMesh
            struct AttributesMesh
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL; // optional
                float4 tangentOS : TANGENT; // optional
                float4 uv0 : TEXCOORD0; // optional
                float4 uv1 : TEXCOORD1; // optional
                float4 uv2 : TEXCOORD2; // optional
                float4 uv3 : TEXCOORD3; // optional
                float4 color : COLOR; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            // Generated Type: VaryingsMeshToPS
            struct VaryingsMeshToPS
            {
                float4 positionCS : SV_Position;
                float3 positionRWS; // optional
                float3 normalWS; // optional
                float4 tangentWS; // optional
                float4 texCoord0; // optional
                float4 texCoord1; // optional
                float4 texCoord2; // optional
                float4 texCoord3; // optional
                float4 color; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            };
            
            // Generated Type: PackedVaryingsMeshToPS
            struct PackedVaryingsMeshToPS
            {
                float4 positionCS : SV_Position; // unpacked
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
                float4 interp02 : TEXCOORD2; // auto-packed
                float4 interp03 : TEXCOORD3; // auto-packed
                float4 interp04 : TEXCOORD4; // auto-packed
                float4 interp05 : TEXCOORD5; // auto-packed
                float4 interp06 : TEXCOORD6; // auto-packed
                float4 interp07 : TEXCOORD7; // auto-packed
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
                #endif // conditional
            };
            
            // Packed Type: VaryingsMeshToPS
            PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
            {
                PackedVaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyzw = input.texCoord1;
                output.interp05.xyzw = input.texCoord2;
                output.interp06.xyzw = input.texCoord3;
                output.interp07.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToPS
            VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
            {
                VaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.texCoord1 = input.interp04.xyzw;
                output.texCoord2 = input.interp05.xyzw;
                output.texCoord3 = input.interp06.xyzw;
                output.color = input.interp07.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            // Generated Type: VaryingsMeshToDS
            struct VaryingsMeshToDS
            {
                float3 positionRWS;
                float3 normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            
            // Generated Type: PackedVaryingsMeshToDS
            struct PackedVaryingsMeshToDS
            {
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
            };
            
            // Packed Type: VaryingsMeshToDS
            PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
            {
                PackedVaryingsMeshToDS output;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToDS
            VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
            {
                VaryingsMeshToDS output;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            //-------------------------------------------------------------------------------------
            // End Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
        
            //-------------------------------------------------------------------------------------
            // Graph generated code
            //-------------------------------------------------------------------------------------
                    // Shared Graph Properties (uniform inputs)
                    CBUFFER_START(UnityPerMaterial)
                    float _AlphaClip;
                    float _Hue;
                    float _Saturation;
                    float _Lightness;
                    float _StiffnessVariation;
                    float4 _WindDirectionAndStrength;
                    float4 _Shiver;
                    float _BAKEDMASK_ON;
                    float _UVMASK_ON;
                    float _VERTEXPOSITIONMASK_ON;
                    float4 _EmissionColor;
                    float _RenderQueueType;
                    float _StencilRef;
                    float _StencilWriteMask;
                    float _StencilRefDepth;
                    float _StencilWriteMaskDepth;
                    float _StencilRefMV;
                    float _StencilWriteMaskMV;
                    float _StencilRefDistortionVec;
                    float _StencilWriteMaskDistortionVec;
                    float _StencilWriteMaskGBuffer;
                    float _StencilRefGBuffer;
                    float _ZTestGBuffer;
                    float _RequireSplitLighting;
                    float _ReceivesSSR;
                    float _SurfaceType;
                    float _BlendMode;
                    float _SrcBlend;
                    float _DstBlend;
                    float _AlphaSrcBlend;
                    float _AlphaDstBlend;
                    float _ZWrite;
                    float _CullMode;
                    float _TransparentSortPriority;
                    float _CullModeForward;
                    float _TransparentCullMode;
                    float _ZTestDepthEqualForOpaque;
                    float _ZTestTransparent;
                    float _TransparentBackfaceEnable;
                    float _AlphaCutoffEnable;
                    float _AlphaCutoff;
                    float _UseShadowThreshold;
                    float _DoubleSidedEnable;
                    float _DoubleSidedNormalMode;
                    float4 _DoubleSidedConstants;
                    float _DiffusionProfileHash;
                    float4 _DiffusionProfileAsset;
                    CBUFFER_END
                    TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo); float4 _Albedo_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap); float4 _MaskMap_TexelSize;
                    TEXTURE2D(_ThicknessMap); SAMPLER(sampler_ThicknessMap); float4 _ThicknessMap_TexelSize;
                    float4 _GlobalWindDirectionAndStrength;
                    float4 _GlobalShiver;
                    TEXTURE2D(_ShiverNoise); SAMPLER(sampler_ShiverNoise); float4 _ShiverNoise_TexelSize;
                    TEXTURE2D(_GustNoise); SAMPLER(sampler_GustNoise); float4 _GustNoise_TexelSize;
                    SAMPLER(_SampleTexture2D_12F932C1_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_FFEA8409_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_F86B9939_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_46D09289_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_DBCD6404_Sampler_3_Linear_Repeat);
                
                // Vertex Graph Inputs
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal; // optional
                        float3 WorldSpaceNormal; // optional
                        float3 ObjectSpaceTangent; // optional
                        float3 WorldSpaceTangent; // optional
                        float3 ObjectSpaceBiTangent; // optional
                        float3 WorldSpaceBiTangent; // optional
                        float3 ObjectSpacePosition; // optional
                        float3 AbsoluteWorldSpacePosition; // optional
                        float4 uv0; // optional
                        float4 uv1; // optional
                        float4 VertexColor; // optional
                        float3 TimeParameters; // optional
                    };
                // Vertex Graph Outputs
                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };
                    
                // Pixel Graph Inputs
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal; // optional
                        float4 uv0; // optional
                    };
                // Pixel Graph Outputs
                    struct SurfaceDescription
                    {
                        float3 Normal;
                        float Smoothness;
                        float Alpha;
                        float AlphaClipThreshold;
                    };
                    
                // Shared Graph Node Functions
                
                    struct Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238
                    {
                    };
                
                    void SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 IN, out float3 PivotInWS_0)
                    {
                        PivotInWS_0 = SHADERGRAPH_OBJECT_POSITION;
                    }
                
                    void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                    {
                        Out = lerp(A, B, T);
                    }
                
                    void Unity_Multiply_float (float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                    {
                        RGBA = float4(R, G, B, A);
                        RGB = float3(R, G, B);
                        RG = float2(R, G);
                    }
                
                    void Unity_Length_float3(float3 In, out float Out)
                    {
                        Out = length(In);
                    }
                
                    void Unity_Multiply_float (float A, float B, out float Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }
                
                    void Unity_Normalize_float3(float3 In, out float3 Out)
                    {
                        Out = normalize(In);
                    }
                
                    void Unity_Maximum_float(float A, float B, out float Out)
                    {
                        Out = max(A, B);
                    }
                
                    void Unity_Multiply_float (float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Maximum_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = max(A, B);
                    }
                
                    struct Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7
                    {
                    };
                
                    void SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(float4 Vector4_14B5A446, float4 Vector4_6887180D, float2 Vector2_F270B07E, float2 Vector2_70BD0D1B, Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 IN, out float3 GustDirection_0, out float GustSpeed_1, out float GustStrength_2, out float ShiverSpeed_3, out float ShiverStrength_4)
                    {
                        float3 _Vector3_E24D7903_Out_0 = float3(0.7, 0, 0.3);
                        float4 _Property_95651D48_Out_0 = Vector4_14B5A446;
                        float4 _Property_FFEF34C6_Out_0 = Vector4_6887180D;
                        float4 _Multiply_7F93D556_Out_2;
                        Unity_Multiply_float(_Property_95651D48_Out_0, _Property_FFEF34C6_Out_0, _Multiply_7F93D556_Out_2);
                        float _Split_1A6C2849_R_1 = _Multiply_7F93D556_Out_2[0];
                        float _Split_1A6C2849_G_2 = _Multiply_7F93D556_Out_2[1];
                        float _Split_1A6C2849_B_3 = _Multiply_7F93D556_Out_2[2];
                        float _Split_1A6C2849_A_4 = _Multiply_7F93D556_Out_2[3];
                        float4 _Combine_769EB158_RGBA_4;
                        float3 _Combine_769EB158_RGB_5;
                        float2 _Combine_769EB158_RG_6;
                        Unity_Combine_float(_Split_1A6C2849_R_1, 0, _Split_1A6C2849_G_2, 0, _Combine_769EB158_RGBA_4, _Combine_769EB158_RGB_5, _Combine_769EB158_RG_6);
                        float _Length_62815FED_Out_1;
                        Unity_Length_float3(_Combine_769EB158_RGB_5, _Length_62815FED_Out_1);
                        float _Multiply_A4A39D4F_Out_2;
                        Unity_Multiply_float(_Length_62815FED_Out_1, 1000, _Multiply_A4A39D4F_Out_2);
                        float _Clamp_4B28219D_Out_3;
                        Unity_Clamp_float(_Multiply_A4A39D4F_Out_2, 0, 1, _Clamp_4B28219D_Out_3);
                        float3 _Lerp_66854A50_Out_3;
                        Unity_Lerp_float3(_Vector3_E24D7903_Out_0, _Combine_769EB158_RGB_5, (_Clamp_4B28219D_Out_3.xxx), _Lerp_66854A50_Out_3);
                        float3 _Normalize_B2778668_Out_1;
                        Unity_Normalize_float3(_Lerp_66854A50_Out_3, _Normalize_B2778668_Out_1);
                        float _Maximum_A3AFA1AB_Out_2;
                        Unity_Maximum_float(_Split_1A6C2849_B_3, 0.01, _Maximum_A3AFA1AB_Out_2);
                        float _Maximum_FCE0058_Out_2;
                        Unity_Maximum_float(0, _Split_1A6C2849_A_4, _Maximum_FCE0058_Out_2);
                        float2 _Property_F062BDE_Out_0 = Vector2_F270B07E;
                        float2 _Property_FB73C895_Out_0 = Vector2_70BD0D1B;
                        float2 _Multiply_76AC0593_Out_2;
                        Unity_Multiply_float(_Property_F062BDE_Out_0, _Property_FB73C895_Out_0, _Multiply_76AC0593_Out_2);
                        float2 _Maximum_E318FF04_Out_2;
                        Unity_Maximum_float2(_Multiply_76AC0593_Out_2, float2(0.01, 0.01), _Maximum_E318FF04_Out_2);
                        float _Split_F437A5E0_R_1 = _Maximum_E318FF04_Out_2[0];
                        float _Split_F437A5E0_G_2 = _Maximum_E318FF04_Out_2[1];
                        float _Split_F437A5E0_B_3 = 0;
                        float _Split_F437A5E0_A_4 = 0;
                        GustDirection_0 = _Normalize_B2778668_Out_1;
                        GustSpeed_1 = _Maximum_A3AFA1AB_Out_2;
                        GustStrength_2 = _Maximum_FCE0058_Out_2;
                        ShiverSpeed_3 = _Split_F437A5E0_R_1;
                        ShiverStrength_4 = _Split_F437A5E0_G_2;
                    }
                
                    void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A - B;
                    }
                
                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Fraction_float(float In, out float Out)
                    {
                        Out = frac(In);
                    }
                
                    void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                    {
                        Out = lerp(False, True, Predicate);
                    }
                
                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A - B;
                    }
                
                    struct Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f
                    {
                    };
                
                    void SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(float Vector1_CCF53CDA, float Vector1_D95E40FE, float2 Vector2_AEE18C41, float2 Vector2_A9CE092C, float Vector1_F2ED6CCC, TEXTURE2D_PARAM(Texture2D_F14459DD, samplerTexture2D_F14459DD), float4 Texture2D_F14459DD_TexelSize, Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f IN, out float GustNoise_0)
                    {
                        float2 _Property_A92CC1B7_Out_0 = Vector2_AEE18C41;
                        float _Property_36B40CE_Out_0 = Vector1_D95E40FE;
                        float _Multiply_9E28D3C4_Out_2;
                        Unity_Multiply_float(_Property_36B40CE_Out_0, 2, _Multiply_9E28D3C4_Out_2);
                        float2 _Add_C54F05FE_Out_2;
                        Unity_Add_float2(_Property_A92CC1B7_Out_0, (_Multiply_9E28D3C4_Out_2.xx), _Add_C54F05FE_Out_2);
                        float2 _Multiply_9CD1691E_Out_2;
                        Unity_Multiply_float(_Add_C54F05FE_Out_2, float2(0.01, 0.01), _Multiply_9CD1691E_Out_2);
                        float2 _Property_D05D9ECB_Out_0 = Vector2_A9CE092C;
                        float _Property_8BFC9AA2_Out_0 = Vector1_CCF53CDA;
                        float2 _Multiply_462DF694_Out_2;
                        Unity_Multiply_float(_Property_D05D9ECB_Out_0, (_Property_8BFC9AA2_Out_0.xx), _Multiply_462DF694_Out_2);
                        float _Property_4DB65C54_Out_0 = Vector1_F2ED6CCC;
                        float2 _Multiply_50FD4B48_Out_2;
                        Unity_Multiply_float(_Multiply_462DF694_Out_2, (_Property_4DB65C54_Out_0.xx), _Multiply_50FD4B48_Out_2);
                        float2 _Subtract_B4A749C2_Out_2;
                        Unity_Subtract_float2(_Multiply_9CD1691E_Out_2, _Multiply_50FD4B48_Out_2, _Subtract_B4A749C2_Out_2);
                        float4 _SampleTexture2DLOD_46D09289_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_F14459DD, samplerTexture2D_F14459DD, _Subtract_B4A749C2_Out_2, 0);
                        float _SampleTexture2DLOD_46D09289_R_5 = _SampleTexture2DLOD_46D09289_RGBA_0.r;
                        float _SampleTexture2DLOD_46D09289_G_6 = _SampleTexture2DLOD_46D09289_RGBA_0.g;
                        float _SampleTexture2DLOD_46D09289_B_7 = _SampleTexture2DLOD_46D09289_RGBA_0.b;
                        float _SampleTexture2DLOD_46D09289_A_8 = _SampleTexture2DLOD_46D09289_RGBA_0.a;
                        GustNoise_0 = _SampleTexture2DLOD_46D09289_R_5;
                    }
                
                    void Unity_Power_float(float A, float B, out float Out)
                    {
                        Out = pow(A, B);
                    }
                
                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }
                
                    struct Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19
                    {
                    };
                
                    void SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(float2 Vector2_CA78C39A, float Vector1_279D2776, Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 IN, out float RandomStiffness_0)
                    {
                        float2 _Property_475BFCB9_Out_0 = Vector2_CA78C39A;
                        float2 _Multiply_7EE00C92_Out_2;
                        Unity_Multiply_float(_Property_475BFCB9_Out_0, float2(10, 10), _Multiply_7EE00C92_Out_2);
                        float _Split_A0FB144F_R_1 = _Multiply_7EE00C92_Out_2[0];
                        float _Split_A0FB144F_G_2 = _Multiply_7EE00C92_Out_2[1];
                        float _Split_A0FB144F_B_3 = 0;
                        float _Split_A0FB144F_A_4 = 0;
                        float _Multiply_2482A544_Out_2;
                        Unity_Multiply_float(_Split_A0FB144F_R_1, _Split_A0FB144F_G_2, _Multiply_2482A544_Out_2);
                        float _Fraction_B90029E4_Out_1;
                        Unity_Fraction_float(_Multiply_2482A544_Out_2, _Fraction_B90029E4_Out_1);
                        float _Power_E2B2B095_Out_2;
                        Unity_Power_float(_Fraction_B90029E4_Out_1, 2, _Power_E2B2B095_Out_2);
                        float _Property_91226CD6_Out_0 = Vector1_279D2776;
                        float _OneMinus_A56B8867_Out_1;
                        Unity_OneMinus_float(_Property_91226CD6_Out_0, _OneMinus_A56B8867_Out_1);
                        float _Clamp_E85434A6_Out_3;
                        Unity_Clamp_float(_Power_E2B2B095_Out_2, _OneMinus_A56B8867_Out_1, 1, _Clamp_E85434A6_Out_3);
                        RandomStiffness_0 = _Clamp_E85434A6_Out_3;
                    }
                
                    struct Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628
                    {
                    };
                
                    void SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(float Vector1_AFC49E6C, float Vector1_A18CF4DF, float Vector1_28AC83F8, float Vector1_E0042E1, float Vector1_1A24AAF, Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 IN, out float GustStrength_0)
                    {
                        float _Property_9A741C0D_Out_0 = Vector1_AFC49E6C;
                        float _Property_F564A485_Out_0 = Vector1_A18CF4DF;
                        float _Multiply_248F3A68_Out_2;
                        Unity_Multiply_float(_Property_9A741C0D_Out_0, _Property_F564A485_Out_0, _Multiply_248F3A68_Out_2);
                        float _Clamp_64D749D9_Out_3;
                        Unity_Clamp_float(_Multiply_248F3A68_Out_2, 0.1, 0.9, _Clamp_64D749D9_Out_3);
                        float _OneMinus_BDC5FAC3_Out_1;
                        Unity_OneMinus_float(_Clamp_64D749D9_Out_3, _OneMinus_BDC5FAC3_Out_1);
                        float _Multiply_E3C6FEFE_Out_2;
                        Unity_Multiply_float(_Multiply_248F3A68_Out_2, _OneMinus_BDC5FAC3_Out_1, _Multiply_E3C6FEFE_Out_2);
                        float _Multiply_9087CA8A_Out_2;
                        Unity_Multiply_float(_Multiply_E3C6FEFE_Out_2, 1.5, _Multiply_9087CA8A_Out_2);
                        float _Property_C7E6777F_Out_0 = Vector1_28AC83F8;
                        float _Multiply_1D329CB_Out_2;
                        Unity_Multiply_float(_Multiply_9087CA8A_Out_2, _Property_C7E6777F_Out_0, _Multiply_1D329CB_Out_2);
                        float _Property_84113466_Out_0 = Vector1_E0042E1;
                        float _Multiply_9501294C_Out_2;
                        Unity_Multiply_float(_Multiply_1D329CB_Out_2, _Property_84113466_Out_0, _Multiply_9501294C_Out_2);
                        float _Property_57C5AF03_Out_0 = Vector1_1A24AAF;
                        float _Multiply_E178164E_Out_2;
                        Unity_Multiply_float(_Multiply_9501294C_Out_2, _Property_57C5AF03_Out_0, _Multiply_E178164E_Out_2);
                        GustStrength_0 = _Multiply_E178164E_Out_2;
                    }
                
                    void Unity_Multiply_float (float3 A, float3 B, out float3 Out)
                    {
                        Out = A * B;
                    }
                
                    struct Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a
                    {
                    };
                
                    void SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(float2 Vector2_287CB44E, float2 Vector2_2A17E6EA, float Vector1_F4B6A491, float Vector1_2C90770B, TEXTURE2D_PARAM(Texture2D_D44B4848, samplerTexture2D_D44B4848), float4 Texture2D_D44B4848_TexelSize, float Vector1_AD94E9BC, Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a IN, out float3 ShiverNoise_0)
                    {
                        float2 _Property_961D8A0_Out_0 = Vector2_287CB44E;
                        float _Property_A414F012_Out_0 = Vector1_AD94E9BC;
                        float _Multiply_7DB42988_Out_2;
                        Unity_Multiply_float(_Property_A414F012_Out_0, 2, _Multiply_7DB42988_Out_2);
                        float2 _Add_4C3CF1F_Out_2;
                        Unity_Add_float2(_Property_961D8A0_Out_0, (_Multiply_7DB42988_Out_2.xx), _Add_4C3CF1F_Out_2);
                        float2 _Property_EBC67BC7_Out_0 = Vector2_2A17E6EA;
                        float _Property_13D296B5_Out_0 = Vector1_F4B6A491;
                        float2 _Multiply_BBB72061_Out_2;
                        Unity_Multiply_float(_Property_EBC67BC7_Out_0, (_Property_13D296B5_Out_0.xx), _Multiply_BBB72061_Out_2);
                        float _Property_3BB601E6_Out_0 = Vector1_2C90770B;
                        float2 _Multiply_FF9010E8_Out_2;
                        Unity_Multiply_float(_Multiply_BBB72061_Out_2, (_Property_3BB601E6_Out_0.xx), _Multiply_FF9010E8_Out_2);
                        float2 _Subtract_6BF2D170_Out_2;
                        Unity_Subtract_float2(_Add_4C3CF1F_Out_2, _Multiply_FF9010E8_Out_2, _Subtract_6BF2D170_Out_2);
                        float4 _SampleTexture2DLOD_DBCD6404_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_D44B4848, samplerTexture2D_D44B4848, _Subtract_6BF2D170_Out_2, 0);
                        float _SampleTexture2DLOD_DBCD6404_R_5 = _SampleTexture2DLOD_DBCD6404_RGBA_0.r;
                        float _SampleTexture2DLOD_DBCD6404_G_6 = _SampleTexture2DLOD_DBCD6404_RGBA_0.g;
                        float _SampleTexture2DLOD_DBCD6404_B_7 = _SampleTexture2DLOD_DBCD6404_RGBA_0.b;
                        float _SampleTexture2DLOD_DBCD6404_A_8 = _SampleTexture2DLOD_DBCD6404_RGBA_0.a;
                        float4 _Combine_E5D76A97_RGBA_4;
                        float3 _Combine_E5D76A97_RGB_5;
                        float2 _Combine_E5D76A97_RG_6;
                        Unity_Combine_float(_SampleTexture2DLOD_DBCD6404_R_5, _SampleTexture2DLOD_DBCD6404_G_6, _SampleTexture2DLOD_DBCD6404_B_7, 0, _Combine_E5D76A97_RGBA_4, _Combine_E5D76A97_RGB_5, _Combine_E5D76A97_RG_6);
                        float3 _Subtract_AA7C02E2_Out_2;
                        Unity_Subtract_float3(_Combine_E5D76A97_RGB_5, float3(0.5, 0.5, 0.5), _Subtract_AA7C02E2_Out_2);
                        float3 _Multiply_5BF7CBD7_Out_2;
                        Unity_Multiply_float(_Subtract_AA7C02E2_Out_2, float3(2, 2, 2), _Multiply_5BF7CBD7_Out_2);
                        ShiverNoise_0 = _Multiply_5BF7CBD7_Out_2;
                    }
                
                    struct Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459
                    {
                    };
                
                    void SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(float3 Vector3_ED0F539A, float2 Vector2_84805101, float Vector1_BDF24CF7, float Vector1_839268A4, float Vector1_A8621014, float Vector1_2DBE6CC0, float Vector1_8A4EF006, float Vector1_ED935C73, Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 IN, out float3 ShiverDirection_0, out float ShiverStrength_1)
                    {
                        float3 _Property_FC94AEBB_Out_0 = Vector3_ED0F539A;
                        float _Property_4FE2271A_Out_0 = Vector1_BDF24CF7;
                        float4 _Combine_328044F1_RGBA_4;
                        float3 _Combine_328044F1_RGB_5;
                        float2 _Combine_328044F1_RG_6;
                        Unity_Combine_float(1, _Property_4FE2271A_Out_0, 1, 0, _Combine_328044F1_RGBA_4, _Combine_328044F1_RGB_5, _Combine_328044F1_RG_6);
                        float3 _Multiply_4FCE02F7_Out_2;
                        Unity_Multiply_float(_Property_FC94AEBB_Out_0, _Combine_328044F1_RGB_5, _Multiply_4FCE02F7_Out_2);
                        float2 _Property_77EED0A8_Out_0 = Vector2_84805101;
                        float _Split_2D66AF35_R_1 = _Property_77EED0A8_Out_0[0];
                        float _Split_2D66AF35_G_2 = _Property_77EED0A8_Out_0[1];
                        float _Split_2D66AF35_B_3 = 0;
                        float _Split_2D66AF35_A_4 = 0;
                        float4 _Combine_C2861A09_RGBA_4;
                        float3 _Combine_C2861A09_RGB_5;
                        float2 _Combine_C2861A09_RG_6;
                        Unity_Combine_float(_Split_2D66AF35_R_1, _Property_4FE2271A_Out_0, _Split_2D66AF35_G_2, 0, _Combine_C2861A09_RGBA_4, _Combine_C2861A09_RGB_5, _Combine_C2861A09_RG_6);
                        float3 _Lerp_A6B0BE86_Out_3;
                        Unity_Lerp_float3(_Multiply_4FCE02F7_Out_2, _Combine_C2861A09_RGB_5, float3(0.5, 0.5, 0.5), _Lerp_A6B0BE86_Out_3);
                        float _Property_BBBC9C1B_Out_0 = Vector1_839268A4;
                        float _Length_F022B321_Out_1;
                        Unity_Length_float3(_Multiply_4FCE02F7_Out_2, _Length_F022B321_Out_1);
                        float _Multiply_BFD84B03_Out_2;
                        Unity_Multiply_float(_Length_F022B321_Out_1, 0.5, _Multiply_BFD84B03_Out_2);
                        float _Multiply_3564B68A_Out_2;
                        Unity_Multiply_float(_Property_BBBC9C1B_Out_0, _Multiply_BFD84B03_Out_2, _Multiply_3564B68A_Out_2);
                        float _Add_83285742_Out_2;
                        Unity_Add_float(_Multiply_3564B68A_Out_2, _Length_F022B321_Out_1, _Add_83285742_Out_2);
                        float _Property_45D94B1_Out_0 = Vector1_2DBE6CC0;
                        float _Multiply_EA43D494_Out_2;
                        Unity_Multiply_float(_Add_83285742_Out_2, _Property_45D94B1_Out_0, _Multiply_EA43D494_Out_2);
                        float _Clamp_C109EA71_Out_3;
                        Unity_Clamp_float(_Multiply_EA43D494_Out_2, 0.1, 0.9, _Clamp_C109EA71_Out_3);
                        float _OneMinus_226F3377_Out_1;
                        Unity_OneMinus_float(_Clamp_C109EA71_Out_3, _OneMinus_226F3377_Out_1);
                        float _Multiply_8680628F_Out_2;
                        Unity_Multiply_float(_Multiply_EA43D494_Out_2, _OneMinus_226F3377_Out_1, _Multiply_8680628F_Out_2);
                        float _Multiply_B14E644_Out_2;
                        Unity_Multiply_float(_Multiply_8680628F_Out_2, 1.5, _Multiply_B14E644_Out_2);
                        float _Property_7F61FC78_Out_0 = Vector1_A8621014;
                        float _Multiply_C89CF7DC_Out_2;
                        Unity_Multiply_float(_Multiply_B14E644_Out_2, _Property_7F61FC78_Out_0, _Multiply_C89CF7DC_Out_2);
                        float _Property_2BD306B6_Out_0 = Vector1_8A4EF006;
                        float _Multiply_E5D34DCC_Out_2;
                        Unity_Multiply_float(_Multiply_C89CF7DC_Out_2, _Property_2BD306B6_Out_0, _Multiply_E5D34DCC_Out_2);
                        float _Property_DBC71A4F_Out_0 = Vector1_ED935C73;
                        float _Multiply_BCACDD38_Out_2;
                        Unity_Multiply_float(_Multiply_E5D34DCC_Out_2, _Property_DBC71A4F_Out_0, _Multiply_BCACDD38_Out_2);
                        ShiverDirection_0 = _Lerp_A6B0BE86_Out_3;
                        ShiverStrength_1 = _Multiply_BCACDD38_Out_2;
                    }
                
                    struct Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364
                    {
                    };
                
                    void SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(float3 Vector3_829210A7, float3 Vector3_1A016C4A, float Vector1_31372BF, float Vector1_E57895AF, TEXTURE2D_PARAM(Texture2D_65F71447, samplerTexture2D_65F71447), float4 Texture2D_65F71447_TexelSize, float Vector1_8836FB6A, TEXTURE2D_PARAM(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), float4 Texture2D_4A3BDB6_TexelSize, float Vector1_14E206AE, float Vector1_7090E96C, float Vector1_51722AC, float Vector1_A3894D2, float Vector1_6F0C3A5A, float Vector1_2D1F6C2F, float Vector1_347751CA, Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 IN, out float GustStrength_0, out float ShiverStrength_1, out float3 ShiverDirection_2)
                    {
                        float _Property_5C7F4A8C_Out_0 = Vector1_31372BF;
                        float _Property_142FEDC3_Out_0 = Vector1_347751CA;
                        float3 _Property_D2FC65C3_Out_0 = Vector3_829210A7;
                        float _Split_8E347DCF_R_1 = _Property_D2FC65C3_Out_0[0];
                        float _Split_8E347DCF_G_2 = _Property_D2FC65C3_Out_0[1];
                        float _Split_8E347DCF_B_3 = _Property_D2FC65C3_Out_0[2];
                        float _Split_8E347DCF_A_4 = 0;
                        float4 _Combine_9B5A76B7_RGBA_4;
                        float3 _Combine_9B5A76B7_RGB_5;
                        float2 _Combine_9B5A76B7_RG_6;
                        Unity_Combine_float(_Split_8E347DCF_R_1, _Split_8E347DCF_B_3, 0, 0, _Combine_9B5A76B7_RGBA_4, _Combine_9B5A76B7_RGB_5, _Combine_9B5A76B7_RG_6);
                        float3 _Property_5653999E_Out_0 = Vector3_1A016C4A;
                        float _Split_B9CBBFE5_R_1 = _Property_5653999E_Out_0[0];
                        float _Split_B9CBBFE5_G_2 = _Property_5653999E_Out_0[1];
                        float _Split_B9CBBFE5_B_3 = _Property_5653999E_Out_0[2];
                        float _Split_B9CBBFE5_A_4 = 0;
                        float4 _Combine_DC44394B_RGBA_4;
                        float3 _Combine_DC44394B_RGB_5;
                        float2 _Combine_DC44394B_RG_6;
                        Unity_Combine_float(_Split_B9CBBFE5_R_1, _Split_B9CBBFE5_B_3, 0, 0, _Combine_DC44394B_RGBA_4, _Combine_DC44394B_RGB_5, _Combine_DC44394B_RG_6);
                        float _Property_3221EFCE_Out_0 = Vector1_E57895AF;
                        Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f _GustNoiseAtPosition_3B28852B;
                        float _GustNoiseAtPosition_3B28852B_GustNoise_0;
                        SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(_Property_5C7F4A8C_Out_0, _Property_142FEDC3_Out_0, _Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_3221EFCE_Out_0, TEXTURE2D_ARGS(Texture2D_65F71447, samplerTexture2D_65F71447), Texture2D_65F71447_TexelSize, _GustNoiseAtPosition_3B28852B, _GustNoiseAtPosition_3B28852B_GustNoise_0);
                        float _Property_1B306054_Out_0 = Vector1_A3894D2;
                        float _Property_1FBC768_Out_0 = Vector1_51722AC;
                        float _Property_9FB10D19_Out_0 = Vector1_14E206AE;
                        Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 _RandomStiffnessAtPosition_C9AD50AB;
                        float _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0;
                        SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(_Combine_9B5A76B7_RG_6, _Property_9FB10D19_Out_0, _RandomStiffnessAtPosition_C9AD50AB, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0);
                        float _Property_EE5A603D_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 _CalculateGustStrength_E2853C74;
                        float _CalculateGustStrength_E2853C74_GustStrength_0;
                        SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(_GustNoiseAtPosition_3B28852B_GustNoise_0, _Property_1B306054_Out_0, _Property_1FBC768_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _Property_EE5A603D_Out_0, _CalculateGustStrength_E2853C74, _CalculateGustStrength_E2853C74_GustStrength_0);
                        float _Property_DFB3FCE0_Out_0 = Vector1_31372BF;
                        float _Property_8A8735CC_Out_0 = Vector1_8836FB6A;
                        Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a _ShiverNoiseAtPosition_35F9220A;
                        float3 _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0;
                        SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(_Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_DFB3FCE0_Out_0, _Property_8A8735CC_Out_0, TEXTURE2D_ARGS(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), Texture2D_4A3BDB6_TexelSize, _Property_142FEDC3_Out_0, _ShiverNoiseAtPosition_35F9220A, _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0);
                        float _Property_65F19953_Out_0 = Vector1_6F0C3A5A;
                        float _Property_3A2F45FE_Out_0 = Vector1_51722AC;
                        float _Property_98EF73E5_Out_0 = Vector1_2D1F6C2F;
                        float _Property_6A278DE2_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 _CalculateShiver_799DE4CB;
                        float3 _CalculateShiver_799DE4CB_ShiverDirection_0;
                        float _CalculateShiver_799DE4CB_ShiverStrength_1;
                        SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(_ShiverNoiseAtPosition_35F9220A_ShiverNoise_0, _Combine_DC44394B_RG_6, _Property_65F19953_Out_0, _CalculateGustStrength_E2853C74_GustStrength_0, _Property_3A2F45FE_Out_0, _Property_98EF73E5_Out_0, _Property_6A278DE2_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _CalculateShiver_799DE4CB, _CalculateShiver_799DE4CB_ShiverDirection_0, _CalculateShiver_799DE4CB_ShiverStrength_1);
                        GustStrength_0 = _CalculateGustStrength_E2853C74_GustStrength_0;
                        ShiverStrength_1 = _CalculateShiver_799DE4CB_ShiverStrength_1;
                        ShiverDirection_2 = _CalculateShiver_799DE4CB_ShiverDirection_0;
                    }
                
                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }
                
                    struct Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01
                    {
                        float3 ObjectSpacePosition;
                    };
                
                    void SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(float3 Vector3_C96069F9, float Vector1_A5EB719C, float Vector1_4D1D3B1A, float3 Vector3_C80E97FF, float3 Vector3_821C320A, float3 Vector3_4BF0DC64, Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 IN, out float3 WorldPosition_0)
                    {
                        float3 _Property_65372844_Out_0 = Vector3_4BF0DC64;
                        float3 _Property_7205E35B_Out_0 = Vector3_821C320A;
                        float _Property_916D8D52_Out_0 = Vector1_4D1D3B1A;
                        float3 _Multiply_CF9DF1B5_Out_2;
                        Unity_Multiply_float(_Property_7205E35B_Out_0, (_Property_916D8D52_Out_0.xxx), _Multiply_CF9DF1B5_Out_2);
                        float3 _Multiply_57D2E5C7_Out_2;
                        Unity_Multiply_float(_Multiply_CF9DF1B5_Out_2, float3(10, 10, 10), _Multiply_57D2E5C7_Out_2);
                        float3 _Add_F265DF09_Out_2;
                        Unity_Add_float3(_Property_65372844_Out_0, _Multiply_57D2E5C7_Out_2, _Add_F265DF09_Out_2);
                        float3 _Property_806C350F_Out_0 = Vector3_C96069F9;
                        float _Property_D017A08E_Out_0 = Vector1_A5EB719C;
                        float3 _Multiply_99498CF9_Out_2;
                        Unity_Multiply_float(_Property_806C350F_Out_0, (_Property_D017A08E_Out_0.xxx), _Multiply_99498CF9_Out_2);
                        float _Split_A5777330_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_A5777330_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_A5777330_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_A5777330_A_4 = 0;
                        float _Clamp_C4364CA5_Out_3;
                        Unity_Clamp_float(_Split_A5777330_G_2, 0, 1, _Clamp_C4364CA5_Out_3);
                        float3 _Multiply_ADC4C2A_Out_2;
                        Unity_Multiply_float(_Multiply_99498CF9_Out_2, (_Clamp_C4364CA5_Out_3.xxx), _Multiply_ADC4C2A_Out_2);
                        float3 _Multiply_49835441_Out_2;
                        Unity_Multiply_float(_Multiply_ADC4C2A_Out_2, float3(10, 10, 10), _Multiply_49835441_Out_2);
                        float3 _Add_B14AAE70_Out_2;
                        Unity_Add_float3(_Add_F265DF09_Out_2, _Multiply_49835441_Out_2, _Add_B14AAE70_Out_2);
                        WorldPosition_0 = _Add_B14AAE70_Out_2;
                    }
                
                    struct Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceTangent;
                        float3 WorldSpaceBiTangent;
                    };
                
                    void SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(float3 Vector3_AAF445D6, Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 IN, out float3 ObjectPosition_1)
                    {
                        float3 _Property_51DA8EE_Out_0 = Vector3_AAF445D6;
                        float3 _Subtract_B236C96B_Out_2;
                        Unity_Subtract_float3(_Property_51DA8EE_Out_0, _WorldSpaceCameraPos, _Subtract_B236C96B_Out_2);
                        float3 _Transform_6FDB2E47_Out_1 = TransformWorldToObject(_Subtract_B236C96B_Out_2.xyz);
                        ObjectPosition_1 = _Transform_6FDB2E47_Out_1;
                    }
                
                // Vertex Graph Evaluation
                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 _GetPivotInWorldSpace_73F19E42;
                        float3 _GetPivotInWorldSpace_73F19E42_PivotInWS_0;
                        SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(_GetPivotInWorldSpace_73F19E42, _GetPivotInWorldSpace_73F19E42_PivotInWS_0);
                        float _Split_64420219_R_1 = IN.VertexColor[0];
                        float _Split_64420219_G_2 = IN.VertexColor[1];
                        float _Split_64420219_B_3 = IN.VertexColor[2];
                        float _Split_64420219_A_4 = IN.VertexColor[3];
                        float3 _Lerp_4531CF63_Out_3;
                        Unity_Lerp_float3(_GetPivotInWorldSpace_73F19E42_PivotInWS_0, IN.AbsoluteWorldSpacePosition, (_Split_64420219_G_2.xxx), _Lerp_4531CF63_Out_3);
                        float4 _Property_D6662DC6_Out_0 = _GlobalWindDirectionAndStrength;
                        float4 _Property_9515B228_Out_0 = _WindDirectionAndStrength;
                        float4 _Property_9A1EF240_Out_0 = _GlobalShiver;
                        float4 _Property_777C8DB2_Out_0 = _Shiver;
                        Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 _GlobalWindParameters_B547F135;
                        float3 _GlobalWindParameters_B547F135_GustDirection_0;
                        float _GlobalWindParameters_B547F135_GustSpeed_1;
                        float _GlobalWindParameters_B547F135_GustStrength_2;
                        float _GlobalWindParameters_B547F135_ShiverSpeed_3;
                        float _GlobalWindParameters_B547F135_ShiverStrength_4;
                        SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(_Property_D6662DC6_Out_0, _Property_9515B228_Out_0, (_Property_9A1EF240_Out_0.xy), (_Property_777C8DB2_Out_0.xy), _GlobalWindParameters_B547F135, _GlobalWindParameters_B547F135_GustDirection_0, _GlobalWindParameters_B547F135_GustSpeed_1, _GlobalWindParameters_B547F135_GustStrength_2, _GlobalWindParameters_B547F135_ShiverSpeed_3, _GlobalWindParameters_B547F135_ShiverStrength_4);
                        float _Property_5F3A390D_Out_0 = _BAKEDMASK_ON;
                        float3 _Subtract_BF2A75CD_Out_2;
                        Unity_Subtract_float3(IN.AbsoluteWorldSpacePosition, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _Subtract_BF2A75CD_Out_2);
                        float _Split_29C81DE4_R_1 = _Subtract_BF2A75CD_Out_2[0];
                        float _Split_29C81DE4_G_2 = _Subtract_BF2A75CD_Out_2[1];
                        float _Split_29C81DE4_B_3 = _Subtract_BF2A75CD_Out_2[2];
                        float _Split_29C81DE4_A_4 = 0;
                        float _Add_6A47DB4F_Out_2;
                        Unity_Add_float(_Split_29C81DE4_R_1, _Split_29C81DE4_G_2, _Add_6A47DB4F_Out_2);
                        float _Add_EC455B5D_Out_2;
                        Unity_Add_float(_Add_6A47DB4F_Out_2, _Split_29C81DE4_B_3, _Add_EC455B5D_Out_2);
                        float _Multiply_F013BB8B_Out_2;
                        Unity_Multiply_float(_Add_EC455B5D_Out_2, 0.4, _Multiply_F013BB8B_Out_2);
                        float _Fraction_7D389816_Out_1;
                        Unity_Fraction_float(_Multiply_F013BB8B_Out_2, _Fraction_7D389816_Out_1);
                        float _Multiply_776D3DAF_Out_2;
                        Unity_Multiply_float(_Fraction_7D389816_Out_1, 0.15, _Multiply_776D3DAF_Out_2);
                        float _Split_E4BB9FEC_R_1 = IN.VertexColor[0];
                        float _Split_E4BB9FEC_G_2 = IN.VertexColor[1];
                        float _Split_E4BB9FEC_B_3 = IN.VertexColor[2];
                        float _Split_E4BB9FEC_A_4 = IN.VertexColor[3];
                        float _Multiply_BC8988C3_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, _Split_E4BB9FEC_G_2, _Multiply_BC8988C3_Out_2);
                        float _Multiply_EC5FE292_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_R_1, 0.3, _Multiply_EC5FE292_Out_2);
                        float _Add_A8423510_Out_2;
                        Unity_Add_float(_Multiply_BC8988C3_Out_2, _Multiply_EC5FE292_Out_2, _Add_A8423510_Out_2);
                        float _Add_CE74358C_Out_2;
                        Unity_Add_float(_Add_A8423510_Out_2, IN.TimeParameters.x, _Add_CE74358C_Out_2);
                        float _Multiply_1CE438D_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_G_2, 0.5, _Multiply_1CE438D_Out_2);
                        float _Add_8718B88C_Out_2;
                        Unity_Add_float(_Add_CE74358C_Out_2, _Multiply_1CE438D_Out_2, _Add_8718B88C_Out_2);
                        float _Property_DBA903E3_Out_0 = _UVMASK_ON;
                        float4 _UV_64D01E18_Out_0 = IN.uv0;
                        float _Split_A5DFBEBE_R_1 = _UV_64D01E18_Out_0[0];
                        float _Split_A5DFBEBE_G_2 = _UV_64D01E18_Out_0[1];
                        float _Split_A5DFBEBE_B_3 = _UV_64D01E18_Out_0[2];
                        float _Split_A5DFBEBE_A_4 = _UV_64D01E18_Out_0[3];
                        float _Multiply_C943DA5C_Out_2;
                        Unity_Multiply_float(_Split_A5DFBEBE_G_2, 0.1, _Multiply_C943DA5C_Out_2);
                        float _Branch_12012434_Out_3;
                        Unity_Branch_float(_Property_DBA903E3_Out_0, _Multiply_C943DA5C_Out_2, 0, _Branch_12012434_Out_3);
                        float _Add_922F2E64_Out_2;
                        Unity_Add_float(IN.TimeParameters.x, _Branch_12012434_Out_3, _Add_922F2E64_Out_2);
                        float _Multiply_2E689843_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, 0.5, _Multiply_2E689843_Out_2);
                        float _Add_ED1EE4DB_Out_2;
                        Unity_Add_float(_Add_922F2E64_Out_2, _Multiply_2E689843_Out_2, _Add_ED1EE4DB_Out_2);
                        float _Branch_291934CD_Out_3;
                        Unity_Branch_float(_Property_5F3A390D_Out_0, _Add_8718B88C_Out_2, _Add_ED1EE4DB_Out_2, _Branch_291934CD_Out_3);
                        float _Property_267CF497_Out_0 = _StiffnessVariation;
                        float _Property_4FB02E51_Out_0 = _BAKEDMASK_ON;
                        float4 _UV_6482E163_Out_0 = IN.uv1;
                        float _Split_2D1A67CF_R_1 = _UV_6482E163_Out_0[0];
                        float _Split_2D1A67CF_G_2 = _UV_6482E163_Out_0[1];
                        float _Split_2D1A67CF_B_3 = _UV_6482E163_Out_0[2];
                        float _Split_2D1A67CF_A_4 = _UV_6482E163_Out_0[3];
                        float _Multiply_F7BD1E76_Out_2;
                        Unity_Multiply_float(_Split_2D1A67CF_R_1, _Split_2D1A67CF_G_2, _Multiply_F7BD1E76_Out_2);
                        float _Property_B1FAFDBF_Out_0 = _UVMASK_ON;
                        float4 _UV_8F58F10B_Out_0 = IN.uv0;
                        float _Split_BD0858B3_R_1 = _UV_8F58F10B_Out_0[0];
                        float _Split_BD0858B3_G_2 = _UV_8F58F10B_Out_0[1];
                        float _Split_BD0858B3_B_3 = _UV_8F58F10B_Out_0[2];
                        float _Split_BD0858B3_A_4 = _UV_8F58F10B_Out_0[3];
                        float _Multiply_3FAD9403_Out_2;
                        Unity_Multiply_float(_Split_BD0858B3_G_2, 0.2, _Multiply_3FAD9403_Out_2);
                        float _Branch_3AF3832A_Out_3;
                        Unity_Branch_float(_Property_B1FAFDBF_Out_0, _Multiply_3FAD9403_Out_2, 1, _Branch_3AF3832A_Out_3);
                        float _Branch_F921E5A9_Out_3;
                        Unity_Branch_float(_Property_4FB02E51_Out_0, _Multiply_F7BD1E76_Out_2, _Branch_3AF3832A_Out_3, _Branch_F921E5A9_Out_3);
                        Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 _GetWindStrength_5806EF0A;
                        float _GetWindStrength_5806EF0A_GustStrength_0;
                        float _GetWindStrength_5806EF0A_ShiverStrength_1;
                        float3 _GetWindStrength_5806EF0A_ShiverDirection_2;
                        SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(_Lerp_4531CF63_Out_3, _GlobalWindParameters_B547F135_GustDirection_0, _Branch_291934CD_Out_3, _GlobalWindParameters_B547F135_GustSpeed_1, TEXTURE2D_ARGS(_GustNoise, sampler_GustNoise), _GustNoise_TexelSize, _GlobalWindParameters_B547F135_ShiverSpeed_3, TEXTURE2D_ARGS(_ShiverNoise, sampler_ShiverNoise), _ShiverNoise_TexelSize, _Property_267CF497_Out_0, 1, _Branch_F921E5A9_Out_3, _GlobalWindParameters_B547F135_GustStrength_2, 0.2, _GlobalWindParameters_B547F135_ShiverStrength_4, 0, _GetWindStrength_5806EF0A, _GetWindStrength_5806EF0A_GustStrength_0, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_ShiverDirection_2);
                        Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 _ApplyTreeWindDisplacement_8E73FF2E;
                        _ApplyTreeWindDisplacement_8E73FF2E.ObjectSpacePosition = IN.ObjectSpacePosition;
                        float3 _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0;
                        SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(_GetWindStrength_5806EF0A_ShiverDirection_2, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_GustStrength_0, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _GlobalWindParameters_B547F135_GustDirection_0, IN.AbsoluteWorldSpacePosition, _ApplyTreeWindDisplacement_8E73FF2E, _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0);
                        Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 _WorldToObject_628B231E;
                        _WorldToObject_628B231E.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _WorldToObject_628B231E.WorldSpaceTangent = IN.WorldSpaceTangent;
                        _WorldToObject_628B231E.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                        float3 _WorldToObject_628B231E_ObjectPosition_1;
                        SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(_ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0, _WorldToObject_628B231E, _WorldToObject_628B231E_ObjectPosition_1);
                        description.VertexPosition = _WorldToObject_628B231E_ObjectPosition_1;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }
                    
                // Pixel Graph Evaluation
                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_12F932C1_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                        _SampleTexture2D_12F932C1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_12F932C1_RGBA_0);
                        float _SampleTexture2D_12F932C1_R_4 = _SampleTexture2D_12F932C1_RGBA_0.r;
                        float _SampleTexture2D_12F932C1_G_5 = _SampleTexture2D_12F932C1_RGBA_0.g;
                        float _SampleTexture2D_12F932C1_B_6 = _SampleTexture2D_12F932C1_RGBA_0.b;
                        float _SampleTexture2D_12F932C1_A_7 = _SampleTexture2D_12F932C1_RGBA_0.a;
                        float4 _SampleTexture2D_FFEA8409_RGBA_0 = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, IN.uv0.xy);
                        float _SampleTexture2D_FFEA8409_R_4 = _SampleTexture2D_FFEA8409_RGBA_0.r;
                        float _SampleTexture2D_FFEA8409_G_5 = _SampleTexture2D_FFEA8409_RGBA_0.g;
                        float _SampleTexture2D_FFEA8409_B_6 = _SampleTexture2D_FFEA8409_RGBA_0.b;
                        float _SampleTexture2D_FFEA8409_A_7 = _SampleTexture2D_FFEA8409_RGBA_0.a;
                        float4 _SampleTexture2D_F86B9939_RGBA_0 = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, IN.uv0.xy);
                        float _SampleTexture2D_F86B9939_R_4 = _SampleTexture2D_F86B9939_RGBA_0.r;
                        float _SampleTexture2D_F86B9939_G_5 = _SampleTexture2D_F86B9939_RGBA_0.g;
                        float _SampleTexture2D_F86B9939_B_6 = _SampleTexture2D_F86B9939_RGBA_0.b;
                        float _SampleTexture2D_F86B9939_A_7 = _SampleTexture2D_F86B9939_RGBA_0.a;
                        float _Property_ABA23041_Out_0 = _AlphaClip;
                        surface.Normal = (_SampleTexture2D_12F932C1_RGBA_0.xyz);
                        surface.Smoothness = _SampleTexture2D_FFEA8409_A_7;
                        surface.Alpha = _SampleTexture2D_F86B9939_A_7;
                        surface.AlphaClipThreshold = _Property_ABA23041_Out_0;
                        return surface;
                    }
                    
            //-------------------------------------------------------------------------------------
            // End graph generated code
            //-------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
            
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                // output.ViewSpaceNormal =             TransformWorldToViewDir(output.WorldSpaceNormal);
                // output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                output.ObjectSpacePosition =         input.positionOS;
                // output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                // output.ViewSpacePosition =           TransformWorldToView(output.WorldSpacePosition);
                // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                // output.WorldSpaceViewDirection =     GetWorldSpaceNormalizeViewDir(output.WorldSpacePosition);
                // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(output.WorldSpacePosition), _ProjectionParams.x);
                output.uv0 =                         input.uv0;
                output.uv1 =                         input.uv1;
                // output.uv2 =                         input.uv2;
                // output.uv3 =                         input.uv3;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            {
                // build graph inputs
                VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
                // Override time paramters with used one (This is required to correctly handle motion vector for vertex animation based on time)
                vertexDescriptionInputs.TimeParameters = timeParameters;
            
                // evaluate vertex graph
                VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
            
                // copy graph output to the results
                input.positionOS = vertexDescription.VertexPosition;
                input.normalOS = vertexDescription.VertexNormal;
                input.tangentOS.xyz = vertexDescription.VertexTangent;
            
                return input;
            }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
                FragInputs BuildFragInputs(VaryingsMeshToPS input)
                {
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.positionCS;       // input.positionCS is SV_Position
            
                    output.positionRWS = input.positionRWS;
                    output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
                    output.texCoord0 = input.texCoord0;
                    output.texCoord1 = input.texCoord1;
                    output.texCoord2 = input.texCoord2;
                    output.texCoord3 = input.texCoord3;
                    output.color = input.color;
                    #if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #elif SHADER_STAGE_FRAGMENT
                    // output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #endif // SHADER_STAGE_FRAGMENT
            
                    return output;
                }
            
                SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                    // output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
                    // output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
                    // output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                    // output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
                    // output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
                    // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                    // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                    // output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
                    // output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
                    // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                    // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                    // output.WorldSpaceViewDirection =     normalize(viewWS);
                    // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                    // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                    // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                    // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                    // output.WorldSpacePosition =          input.positionRWS;
                    // output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
                    // output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
                    // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                    // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionRWS);
                    // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
                    output.uv0 =                         input.texCoord0;
                    // output.uv1 =                         input.texCoord1;
                    // output.uv2 =                         input.texCoord2;
                    // output.uv3 =                         input.texCoord3;
                    // output.VertexColor =                 input.color;
                    // output.FaceSign =                    input.isFrontFace;
                    // output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            
                    return output;
                }
            
                // existing HDRP code uses the combined function to go directly from packed to frag inputs
                FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    VaryingsMeshToPS unpacked= UnpackVaryingsMeshToPS(input);
                    return BuildFragInputs(unpacked);
                }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
            void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
            {
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                // copy across graph values, if defined
                // surfaceData.baseColor =                 surfaceDescription.Albedo;
                surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                // surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                // surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                // surfaceData.metallic =                  surfaceDescription.Metallic;
                // surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                // surfaceData.thickness =                 surfaceDescription.Thickness;
                // surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                // surfaceData.specularColor =             surfaceDescription.Specular;
                // surfaceData.coatMask =                  surfaceDescription.CoatMask;
                // surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                // surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                // surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
        #ifdef _HAS_REFRACTION
                if (_EnableSSRefraction)
                {
                    // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                    // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                    // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                    surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                    surfaceDescription.Alpha = 1.0;
                }
                else
                {
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                    surfaceDescription.Alpha = 1.0;
                }
        #else
                surfaceData.ior = 1.0;
                surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                surfaceData.atDistance = 1.0;
                surfaceData.transmittanceMask = 0.0;
        #endif
                
                // These static material feature allow compile time optimization
                surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
        #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
        #endif
        #ifdef _MATERIAL_FEATURE_TRANSMISSION
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
        #endif
        #ifdef _MATERIAL_FEATURE_ANISOTROPY
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
        #endif
                // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
        #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
        #endif
        #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
        #endif
        
        #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                // Require to have setup baseColor
                // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                // tangent-space normal
                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                // compute world space normal
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                bentNormalWS = surfaceData.normalWS;
                // GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
                // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                // If user provide bent normal then we process a better term
        #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                // Just use the value passed through via the slot (not active otherwise)
        #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                // If we have bent normal and ambient occlusion, process a specular occlusion
                surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
        #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
        #else
                surfaceData.specularOcclusion = 1.0;
        #endif
        
        #if HAVE_DECALS
                if (_EnableDecals)
                {
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                }
        #endif
        
        #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
        #endif
        
        #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    // TODO: need to update mip info
                    surfaceData.metallic = 0;
                }
        
                // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                // as it can modify attribute use for static lighting
                ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
        #endif
            }
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
                
                // ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
        
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal 
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                // override sampleBakedGI:
                // builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
                // builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
        
                // builtinData.emissiveColor = surfaceDescription.Emission;
        
                // builtinData.depthOffset = surfaceDescription.DepthOffset;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
            //-------------------------------------------------------------------------------------
            // Pass Includes
            //-------------------------------------------------------------------------------------
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassMotionVectors.hlsl"
            //-------------------------------------------------------------------------------------
            // End Pass Includes
            //-------------------------------------------------------------------------------------
        
            ENDHLSL
        }
        
        Pass
        {
            // based on HDLitPass.template
            Name "Forward"
            Tags { "LightMode" = "Forward" }
        
            //-------------------------------------------------------------------------------------
            // Render Modes (Blend, Cull, ZTest, Stencil, etc)
            //-------------------------------------------------------------------------------------
            Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
        
            Cull [_CullModeForward]
        
            ZTest Equal
        
            ZWrite [_ZWrite]
        
            
            // Stencil setup
        Stencil
        {
           WriteMask [_StencilWriteMask]
           Ref [_StencilRef]
           Comp Always
           Pass Replace
        }
        
            ColorMask [_ColorMaskTransparentVel] 1
        
            //-------------------------------------------------------------------------------------
            // End Render Modes
            //-------------------------------------------------------------------------------------
        
            HLSLPROGRAM
        
            #pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            //#pragma enable_d3d11_debug_symbols
        
            #pragma multi_compile_instancing
        #pragma instancing_options renderinglayer
        
            #pragma multi_compile _ LOD_FADE_CROSSFADE
        
            #pragma shader_feature _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _DOUBLESIDED_ON
            #pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
        
            //-------------------------------------------------------------------------------------
            // Variant Definitions (active field translations to HDRP defines)
            //-------------------------------------------------------------------------------------
            // #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #define _MATERIAL_FEATURE_TRANSMISSION 1
            // #define _MATERIAL_FEATURE_ANISOTROPY 1
            // #define _MATERIAL_FEATURE_IRIDESCENCE 1
            // #define _MATERIAL_FEATURE_SPECULAR_COLOR 1
            // #define _ENABLE_FOG_ON_TRANSPARENT 1
            #define _AMBIENT_OCCLUSION 1
            // #define _SPECULAR_OCCLUSION_FROM_AO 1
            // #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            // #define _SPECULAR_OCCLUSION_CUSTOM 1
            #define _ENERGY_CONSERVING_SPECULAR 1
            // #define _ENABLE_GEOMETRIC_SPECULAR_AA 1
            // #define _HAS_REFRACTION 1
            // #define _REFRACTION_PLANE 1
            // #define _REFRACTION_SPHERE 1
            #define _DISABLE_DECALS 1
            #define _DISABLE_SSR 1
            // #define _ADD_PRECOMPUTED_VELOCITY
            // #define _WRITE_TRANSPARENT_MOTION_VECTOR 1
            // #define _DEPTHOFFSET_ON 1
            // #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
        
            //-------------------------------------------------------------------------------------
            // End Variant Definitions
            //-------------------------------------------------------------------------------------
        
            #pragma vertex Vert
            #pragma fragment Frag
        
            // If we use subsurface scattering, enable output split lighting (for forward pass)
            #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
            #define OUTPUT_SPLIT_LIGHTING
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
            // define FragInputs structure
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            //-------------------------------------------------------------------------------------
            // Defines
            //-------------------------------------------------------------------------------------
                    #define SHADERPASS SHADERPASS_FORWARD
                #pragma multi_compile _ DEBUG_DISPLAY
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ DYNAMICLIGHTMAP_ON
                #pragma multi_compile _ SHADOWS_SHADOWMASK
                #pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
                #pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
                #pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH
                #ifndef DEBUG_DISPLAY
    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
    #endif
                // ACTIVE FIELDS:
                //   DoubleSided
                //   DoubleSided.Flip
                //   FragInputs.isFrontFace
                //   Material.Translucent
                //   Material.Transmission
                //   AlphaTest
                //   DisableDecals
                //   DisableSSR
                //   Specular.EnergyConserving
                //   AmbientOcclusion
                //   SurfaceDescriptionInputs.TangentSpaceNormal
                //   SurfaceDescriptionInputs.uv0
                //   VertexDescriptionInputs.VertexColor
                //   VertexDescriptionInputs.ObjectSpaceNormal
                //   VertexDescriptionInputs.WorldSpaceNormal
                //   VertexDescriptionInputs.ObjectSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceTangent
                //   VertexDescriptionInputs.WorldSpaceBiTangent
                //   VertexDescriptionInputs.ObjectSpacePosition
                //   VertexDescriptionInputs.AbsoluteWorldSpacePosition
                //   VertexDescriptionInputs.uv0
                //   VertexDescriptionInputs.uv1
                //   VertexDescriptionInputs.TimeParameters
                //   SurfaceDescription.Albedo
                //   SurfaceDescription.Normal
                //   SurfaceDescription.BentNormal
                //   SurfaceDescription.Thickness
                //   SurfaceDescription.DiffusionProfileHash
                //   SurfaceDescription.CoatMask
                //   SurfaceDescription.Emission
                //   SurfaceDescription.Smoothness
                //   SurfaceDescription.Occlusion
                //   SurfaceDescription.Alpha
                //   SurfaceDescription.AlphaClipThreshold
                //   features.modifyMesh
                //   VertexDescription.VertexPosition
                //   VertexDescription.VertexNormal
                //   VertexDescription.VertexTangent
                //   FragInputs.tangentToWorld
                //   FragInputs.positionRWS
                //   FragInputs.texCoord1
                //   FragInputs.texCoord2
                //   VaryingsMeshToPS.cullFace
                //   FragInputs.texCoord0
                //   AttributesMesh.color
                //   AttributesMesh.normalOS
                //   AttributesMesh.tangentOS
                //   VertexDescriptionInputs.ObjectSpaceBiTangent
                //   AttributesMesh.positionOS
                //   AttributesMesh.uv0
                //   AttributesMesh.uv1
                //   VaryingsMeshToPS.tangentWS
                //   VaryingsMeshToPS.normalWS
                //   VaryingsMeshToPS.positionRWS
                //   VaryingsMeshToPS.texCoord1
                //   VaryingsMeshToPS.texCoord2
                //   VaryingsMeshToPS.texCoord0
                //   AttributesMesh.uv2
                // Shared Graph Keywords
        
            // this translates the new dependency tracker into the old preprocessor definitions for the existing HDRP shader code
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            // #define ATTRIBUTES_NEED_TEXCOORD3
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            #define VARYINGS_NEED_TEXCOORD2
            // #define VARYINGS_NEED_TEXCOORD3
            // #define VARYINGS_NEED_COLOR
            #define VARYINGS_NEED_CULLFACE
            #define HAVE_MESH_MODIFICATION
        
        // We need isFontFace when using double sided
        #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
            #define VARYINGS_NEED_CULLFACE
        #endif
        
            //-------------------------------------------------------------------------------------
            // End Defines
            //-------------------------------------------------------------------------------------
        	
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        
        #if (SHADERPASS == SHADERPASS_FORWARD)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
            #define HAS_LIGHTLOOP
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
        #else
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
        #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // Used by SceneSelectionPass
            int _ObjectId;
            int _PassValue;
        
            //-------------------------------------------------------------------------------------
            // Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
            // Generated Type: AttributesMesh
            struct AttributesMesh
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL; // optional
                float4 tangentOS : TANGENT; // optional
                float4 uv0 : TEXCOORD0; // optional
                float4 uv1 : TEXCOORD1; // optional
                float4 uv2 : TEXCOORD2; // optional
                float4 color : COLOR; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            // Generated Type: VaryingsMeshToPS
            struct VaryingsMeshToPS
            {
                float4 positionCS : SV_Position;
                float3 positionRWS; // optional
                float3 normalWS; // optional
                float4 tangentWS; // optional
                float4 texCoord0; // optional
                float4 texCoord1; // optional
                float4 texCoord2; // optional
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif // defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            };
            
            // Generated Type: PackedVaryingsMeshToPS
            struct PackedVaryingsMeshToPS
            {
                float4 positionCS : SV_Position; // unpacked
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
                float4 interp02 : TEXCOORD2; // auto-packed
                float4 interp03 : TEXCOORD3; // auto-packed
                float4 interp04 : TEXCOORD4; // auto-packed
                float4 interp05 : TEXCOORD5; // auto-packed
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC; // unpacked
                #endif // conditional
            };
            
            // Packed Type: VaryingsMeshToPS
            PackedVaryingsMeshToPS PackVaryingsMeshToPS(VaryingsMeshToPS input)
            {
                PackedVaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyzw = input.texCoord1;
                output.interp05.xyzw = input.texCoord2;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToPS
            VaryingsMeshToPS UnpackVaryingsMeshToPS(PackedVaryingsMeshToPS input)
            {
                VaryingsMeshToPS output;
                output.positionCS = input.positionCS;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.texCoord1 = input.interp04.xyzw;
                output.texCoord2 = input.interp05.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif // conditional
                return output;
            }
            // Generated Type: VaryingsMeshToDS
            struct VaryingsMeshToDS
            {
                float3 positionRWS;
                float3 normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif // UNITY_ANY_INSTANCING_ENABLED
            };
            
            // Generated Type: PackedVaryingsMeshToDS
            struct PackedVaryingsMeshToDS
            {
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID; // unpacked
                #endif // conditional
                float3 interp00 : TEXCOORD0; // auto-packed
                float3 interp01 : TEXCOORD1; // auto-packed
            };
            
            // Packed Type: VaryingsMeshToDS
            PackedVaryingsMeshToDS PackVaryingsMeshToDS(VaryingsMeshToDS input)
            {
                PackedVaryingsMeshToDS output;
                output.interp00.xyz = input.positionRWS;
                output.interp01.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            
            // Unpacked Type: VaryingsMeshToDS
            VaryingsMeshToDS UnpackVaryingsMeshToDS(PackedVaryingsMeshToDS input)
            {
                VaryingsMeshToDS output;
                output.positionRWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif // conditional
                return output;
            }
            //-------------------------------------------------------------------------------------
            // End Interpolator Packing And Struct Declarations
            //-------------------------------------------------------------------------------------
        
            //-------------------------------------------------------------------------------------
            // Graph generated code
            //-------------------------------------------------------------------------------------
                    // Shared Graph Properties (uniform inputs)
                    CBUFFER_START(UnityPerMaterial)
                    float _AlphaClip;
                    float _Hue;
                    float _Saturation;
                    float _Lightness;
                    float _StiffnessVariation;
                    float4 _WindDirectionAndStrength;
                    float4 _Shiver;
                    float _BAKEDMASK_ON;
                    float _UVMASK_ON;
                    float _VERTEXPOSITIONMASK_ON;
                    float4 _EmissionColor;
                    float _RenderQueueType;
                    float _StencilRef;
                    float _StencilWriteMask;
                    float _StencilRefDepth;
                    float _StencilWriteMaskDepth;
                    float _StencilRefMV;
                    float _StencilWriteMaskMV;
                    float _StencilRefDistortionVec;
                    float _StencilWriteMaskDistortionVec;
                    float _StencilWriteMaskGBuffer;
                    float _StencilRefGBuffer;
                    float _ZTestGBuffer;
                    float _RequireSplitLighting;
                    float _ReceivesSSR;
                    float _SurfaceType;
                    float _BlendMode;
                    float _SrcBlend;
                    float _DstBlend;
                    float _AlphaSrcBlend;
                    float _AlphaDstBlend;
                    float _ZWrite;
                    float _CullMode;
                    float _TransparentSortPriority;
                    float _CullModeForward;
                    float _TransparentCullMode;
                    float _ZTestDepthEqualForOpaque;
                    float _ZTestTransparent;
                    float _TransparentBackfaceEnable;
                    float _AlphaCutoffEnable;
                    float _AlphaCutoff;
                    float _UseShadowThreshold;
                    float _DoubleSidedEnable;
                    float _DoubleSidedNormalMode;
                    float4 _DoubleSidedConstants;
                    float _DiffusionProfileHash;
                    float4 _DiffusionProfileAsset;
                    CBUFFER_END
                    TEXTURE2D(_Albedo); SAMPLER(sampler_Albedo); float4 _Albedo_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MaskMap); SAMPLER(sampler_MaskMap); float4 _MaskMap_TexelSize;
                    TEXTURE2D(_ThicknessMap); SAMPLER(sampler_ThicknessMap); float4 _ThicknessMap_TexelSize;
                    float4 _GlobalWindDirectionAndStrength;
                    float4 _GlobalShiver;
                    TEXTURE2D(_ShiverNoise); SAMPLER(sampler_ShiverNoise); float4 _ShiverNoise_TexelSize;
                    TEXTURE2D(_GustNoise); SAMPLER(sampler_GustNoise); float4 _GustNoise_TexelSize;
                    SAMPLER(_SampleTexture2D_F86B9939_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_12F932C1_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_E3683686_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2D_FFEA8409_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_46D09289_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_DBCD6404_Sampler_3_Linear_Repeat);
                
                // Vertex Graph Inputs
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal; // optional
                        float3 WorldSpaceNormal; // optional
                        float3 ObjectSpaceTangent; // optional
                        float3 WorldSpaceTangent; // optional
                        float3 ObjectSpaceBiTangent; // optional
                        float3 WorldSpaceBiTangent; // optional
                        float3 ObjectSpacePosition; // optional
                        float3 AbsoluteWorldSpacePosition; // optional
                        float4 uv0; // optional
                        float4 uv1; // optional
                        float4 VertexColor; // optional
                        float3 TimeParameters; // optional
                    };
                // Vertex Graph Outputs
                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };
                    
                // Pixel Graph Inputs
                    struct SurfaceDescriptionInputs
                    {
                        float3 TangentSpaceNormal; // optional
                        float4 uv0; // optional
                    };
                // Pixel Graph Outputs
                    struct SurfaceDescription
                    {
                        float3 Albedo;
                        float3 Normal;
                        float3 BentNormal;
                        float Thickness;
                        float DiffusionProfileHash;
                        float CoatMask;
                        float3 Emission;
                        float Smoothness;
                        float Occlusion;
                        float Alpha;
                        float AlphaClipThreshold;
                    };
                    
                // Shared Graph Node Functions
                
                    void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
                    {
                        // RGB to HSV
                        float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                        float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                        float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                        float D = Q.x - min(Q.w, Q.y);
                        float E = 1e-4;
                        float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);
                
                        float hue = hsv.x + Offset;
                        hsv.x = (hue < 0)
                                ? hue + 1
                                : (hue > 1)
                                    ? hue - 1
                                    : hue;
                
                        // HSV to RGB
                        float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                        float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
                        Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
                    }
                
                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
                    {
                        float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
                        Out =  luma.xxx + Saturation.xxx * (In - luma.xxx);
                    }
                
                    void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A + B;
                    }
                
                    struct Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238
                    {
                    };
                
                    void SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 IN, out float3 PivotInWS_0)
                    {
                        PivotInWS_0 = SHADERGRAPH_OBJECT_POSITION;
                    }
                
                    void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                    {
                        Out = lerp(A, B, T);
                    }
                
                    void Unity_Multiply_float (float4 A, float4 B, out float4 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                    {
                        RGBA = float4(R, G, B, A);
                        RGB = float3(R, G, B);
                        RG = float2(R, G);
                    }
                
                    void Unity_Length_float3(float3 In, out float Out)
                    {
                        Out = length(In);
                    }
                
                    void Unity_Multiply_float (float A, float B, out float Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                    {
                        Out = clamp(In, Min, Max);
                    }
                
                    void Unity_Normalize_float3(float3 In, out float3 Out)
                    {
                        Out = normalize(In);
                    }
                
                    void Unity_Maximum_float(float A, float B, out float Out)
                    {
                        Out = max(A, B);
                    }
                
                    void Unity_Multiply_float (float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }
                
                    void Unity_Maximum_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = max(A, B);
                    }
                
                    struct Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7
                    {
                    };
                
                    void SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(float4 Vector4_14B5A446, float4 Vector4_6887180D, float2 Vector2_F270B07E, float2 Vector2_70BD0D1B, Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 IN, out float3 GustDirection_0, out float GustSpeed_1, out float GustStrength_2, out float ShiverSpeed_3, out float ShiverStrength_4)
                    {
                        float3 _Vector3_E24D7903_Out_0 = float3(0.7, 0, 0.3);
                        float4 _Property_95651D48_Out_0 = Vector4_14B5A446;
                        float4 _Property_FFEF34C6_Out_0 = Vector4_6887180D;
                        float4 _Multiply_7F93D556_Out_2;
                        Unity_Multiply_float(_Property_95651D48_Out_0, _Property_FFEF34C6_Out_0, _Multiply_7F93D556_Out_2);
                        float _Split_1A6C2849_R_1 = _Multiply_7F93D556_Out_2[0];
                        float _Split_1A6C2849_G_2 = _Multiply_7F93D556_Out_2[1];
                        float _Split_1A6C2849_B_3 = _Multiply_7F93D556_Out_2[2];
                        float _Split_1A6C2849_A_4 = _Multiply_7F93D556_Out_2[3];
                        float4 _Combine_769EB158_RGBA_4;
                        float3 _Combine_769EB158_RGB_5;
                        float2 _Combine_769EB158_RG_6;
                        Unity_Combine_float(_Split_1A6C2849_R_1, 0, _Split_1A6C2849_G_2, 0, _Combine_769EB158_RGBA_4, _Combine_769EB158_RGB_5, _Combine_769EB158_RG_6);
                        float _Length_62815FED_Out_1;
                        Unity_Length_float3(_Combine_769EB158_RGB_5, _Length_62815FED_Out_1);
                        float _Multiply_A4A39D4F_Out_2;
                        Unity_Multiply_float(_Length_62815FED_Out_1, 1000, _Multiply_A4A39D4F_Out_2);
                        float _Clamp_4B28219D_Out_3;
                        Unity_Clamp_float(_Multiply_A4A39D4F_Out_2, 0, 1, _Clamp_4B28219D_Out_3);
                        float3 _Lerp_66854A50_Out_3;
                        Unity_Lerp_float3(_Vector3_E24D7903_Out_0, _Combine_769EB158_RGB_5, (_Clamp_4B28219D_Out_3.xxx), _Lerp_66854A50_Out_3);
                        float3 _Normalize_B2778668_Out_1;
                        Unity_Normalize_float3(_Lerp_66854A50_Out_3, _Normalize_B2778668_Out_1);
                        float _Maximum_A3AFA1AB_Out_2;
                        Unity_Maximum_float(_Split_1A6C2849_B_3, 0.01, _Maximum_A3AFA1AB_Out_2);
                        float _Maximum_FCE0058_Out_2;
                        Unity_Maximum_float(0, _Split_1A6C2849_A_4, _Maximum_FCE0058_Out_2);
                        float2 _Property_F062BDE_Out_0 = Vector2_F270B07E;
                        float2 _Property_FB73C895_Out_0 = Vector2_70BD0D1B;
                        float2 _Multiply_76AC0593_Out_2;
                        Unity_Multiply_float(_Property_F062BDE_Out_0, _Property_FB73C895_Out_0, _Multiply_76AC0593_Out_2);
                        float2 _Maximum_E318FF04_Out_2;
                        Unity_Maximum_float2(_Multiply_76AC0593_Out_2, float2(0.01, 0.01), _Maximum_E318FF04_Out_2);
                        float _Split_F437A5E0_R_1 = _Maximum_E318FF04_Out_2[0];
                        float _Split_F437A5E0_G_2 = _Maximum_E318FF04_Out_2[1];
                        float _Split_F437A5E0_B_3 = 0;
                        float _Split_F437A5E0_A_4 = 0;
                        GustDirection_0 = _Normalize_B2778668_Out_1;
                        GustSpeed_1 = _Maximum_A3AFA1AB_Out_2;
                        GustStrength_2 = _Maximum_FCE0058_Out_2;
                        ShiverSpeed_3 = _Split_F437A5E0_R_1;
                        ShiverStrength_4 = _Split_F437A5E0_G_2;
                    }
                
                    void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
                    {
                        Out = A - B;
                    }
                
                    void Unity_Fraction_float(float In, out float Out)
                    {
                        Out = frac(In);
                    }
                
                    void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                    {
                        Out = lerp(False, True, Predicate);
                    }
                
                    void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A + B;
                    }
                
                    void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
                    {
                        Out = A - B;
                    }
                
                    struct Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f
                    {
                    };
                
                    void SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(float Vector1_CCF53CDA, float Vector1_D95E40FE, float2 Vector2_AEE18C41, float2 Vector2_A9CE092C, float Vector1_F2ED6CCC, TEXTURE2D_PARAM(Texture2D_F14459DD, samplerTexture2D_F14459DD), float4 Texture2D_F14459DD_TexelSize, Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f IN, out float GustNoise_0)
                    {
                        float2 _Property_A92CC1B7_Out_0 = Vector2_AEE18C41;
                        float _Property_36B40CE_Out_0 = Vector1_D95E40FE;
                        float _Multiply_9E28D3C4_Out_2;
                        Unity_Multiply_float(_Property_36B40CE_Out_0, 2, _Multiply_9E28D3C4_Out_2);
                        float2 _Add_C54F05FE_Out_2;
                        Unity_Add_float2(_Property_A92CC1B7_Out_0, (_Multiply_9E28D3C4_Out_2.xx), _Add_C54F05FE_Out_2);
                        float2 _Multiply_9CD1691E_Out_2;
                        Unity_Multiply_float(_Add_C54F05FE_Out_2, float2(0.01, 0.01), _Multiply_9CD1691E_Out_2);
                        float2 _Property_D05D9ECB_Out_0 = Vector2_A9CE092C;
                        float _Property_8BFC9AA2_Out_0 = Vector1_CCF53CDA;
                        float2 _Multiply_462DF694_Out_2;
                        Unity_Multiply_float(_Property_D05D9ECB_Out_0, (_Property_8BFC9AA2_Out_0.xx), _Multiply_462DF694_Out_2);
                        float _Property_4DB65C54_Out_0 = Vector1_F2ED6CCC;
                        float2 _Multiply_50FD4B48_Out_2;
                        Unity_Multiply_float(_Multiply_462DF694_Out_2, (_Property_4DB65C54_Out_0.xx), _Multiply_50FD4B48_Out_2);
                        float2 _Subtract_B4A749C2_Out_2;
                        Unity_Subtract_float2(_Multiply_9CD1691E_Out_2, _Multiply_50FD4B48_Out_2, _Subtract_B4A749C2_Out_2);
                        float4 _SampleTexture2DLOD_46D09289_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_F14459DD, samplerTexture2D_F14459DD, _Subtract_B4A749C2_Out_2, 0);
                        float _SampleTexture2DLOD_46D09289_R_5 = _SampleTexture2DLOD_46D09289_RGBA_0.r;
                        float _SampleTexture2DLOD_46D09289_G_6 = _SampleTexture2DLOD_46D09289_RGBA_0.g;
                        float _SampleTexture2DLOD_46D09289_B_7 = _SampleTexture2DLOD_46D09289_RGBA_0.b;
                        float _SampleTexture2DLOD_46D09289_A_8 = _SampleTexture2DLOD_46D09289_RGBA_0.a;
                        GustNoise_0 = _SampleTexture2DLOD_46D09289_R_5;
                    }
                
                    void Unity_Power_float(float A, float B, out float Out)
                    {
                        Out = pow(A, B);
                    }
                
                    void Unity_OneMinus_float(float In, out float Out)
                    {
                        Out = 1 - In;
                    }
                
                    struct Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19
                    {
                    };
                
                    void SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(float2 Vector2_CA78C39A, float Vector1_279D2776, Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 IN, out float RandomStiffness_0)
                    {
                        float2 _Property_475BFCB9_Out_0 = Vector2_CA78C39A;
                        float2 _Multiply_7EE00C92_Out_2;
                        Unity_Multiply_float(_Property_475BFCB9_Out_0, float2(10, 10), _Multiply_7EE00C92_Out_2);
                        float _Split_A0FB144F_R_1 = _Multiply_7EE00C92_Out_2[0];
                        float _Split_A0FB144F_G_2 = _Multiply_7EE00C92_Out_2[1];
                        float _Split_A0FB144F_B_3 = 0;
                        float _Split_A0FB144F_A_4 = 0;
                        float _Multiply_2482A544_Out_2;
                        Unity_Multiply_float(_Split_A0FB144F_R_1, _Split_A0FB144F_G_2, _Multiply_2482A544_Out_2);
                        float _Fraction_B90029E4_Out_1;
                        Unity_Fraction_float(_Multiply_2482A544_Out_2, _Fraction_B90029E4_Out_1);
                        float _Power_E2B2B095_Out_2;
                        Unity_Power_float(_Fraction_B90029E4_Out_1, 2, _Power_E2B2B095_Out_2);
                        float _Property_91226CD6_Out_0 = Vector1_279D2776;
                        float _OneMinus_A56B8867_Out_1;
                        Unity_OneMinus_float(_Property_91226CD6_Out_0, _OneMinus_A56B8867_Out_1);
                        float _Clamp_E85434A6_Out_3;
                        Unity_Clamp_float(_Power_E2B2B095_Out_2, _OneMinus_A56B8867_Out_1, 1, _Clamp_E85434A6_Out_3);
                        RandomStiffness_0 = _Clamp_E85434A6_Out_3;
                    }
                
                    struct Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628
                    {
                    };
                
                    void SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(float Vector1_AFC49E6C, float Vector1_A18CF4DF, float Vector1_28AC83F8, float Vector1_E0042E1, float Vector1_1A24AAF, Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 IN, out float GustStrength_0)
                    {
                        float _Property_9A741C0D_Out_0 = Vector1_AFC49E6C;
                        float _Property_F564A485_Out_0 = Vector1_A18CF4DF;
                        float _Multiply_248F3A68_Out_2;
                        Unity_Multiply_float(_Property_9A741C0D_Out_0, _Property_F564A485_Out_0, _Multiply_248F3A68_Out_2);
                        float _Clamp_64D749D9_Out_3;
                        Unity_Clamp_float(_Multiply_248F3A68_Out_2, 0.1, 0.9, _Clamp_64D749D9_Out_3);
                        float _OneMinus_BDC5FAC3_Out_1;
                        Unity_OneMinus_float(_Clamp_64D749D9_Out_3, _OneMinus_BDC5FAC3_Out_1);
                        float _Multiply_E3C6FEFE_Out_2;
                        Unity_Multiply_float(_Multiply_248F3A68_Out_2, _OneMinus_BDC5FAC3_Out_1, _Multiply_E3C6FEFE_Out_2);
                        float _Multiply_9087CA8A_Out_2;
                        Unity_Multiply_float(_Multiply_E3C6FEFE_Out_2, 1.5, _Multiply_9087CA8A_Out_2);
                        float _Property_C7E6777F_Out_0 = Vector1_28AC83F8;
                        float _Multiply_1D329CB_Out_2;
                        Unity_Multiply_float(_Multiply_9087CA8A_Out_2, _Property_C7E6777F_Out_0, _Multiply_1D329CB_Out_2);
                        float _Property_84113466_Out_0 = Vector1_E0042E1;
                        float _Multiply_9501294C_Out_2;
                        Unity_Multiply_float(_Multiply_1D329CB_Out_2, _Property_84113466_Out_0, _Multiply_9501294C_Out_2);
                        float _Property_57C5AF03_Out_0 = Vector1_1A24AAF;
                        float _Multiply_E178164E_Out_2;
                        Unity_Multiply_float(_Multiply_9501294C_Out_2, _Property_57C5AF03_Out_0, _Multiply_E178164E_Out_2);
                        GustStrength_0 = _Multiply_E178164E_Out_2;
                    }
                
                    void Unity_Multiply_float (float3 A, float3 B, out float3 Out)
                    {
                        Out = A * B;
                    }
                
                    struct Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a
                    {
                    };
                
                    void SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(float2 Vector2_287CB44E, float2 Vector2_2A17E6EA, float Vector1_F4B6A491, float Vector1_2C90770B, TEXTURE2D_PARAM(Texture2D_D44B4848, samplerTexture2D_D44B4848), float4 Texture2D_D44B4848_TexelSize, float Vector1_AD94E9BC, Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a IN, out float3 ShiverNoise_0)
                    {
                        float2 _Property_961D8A0_Out_0 = Vector2_287CB44E;
                        float _Property_A414F012_Out_0 = Vector1_AD94E9BC;
                        float _Multiply_7DB42988_Out_2;
                        Unity_Multiply_float(_Property_A414F012_Out_0, 2, _Multiply_7DB42988_Out_2);
                        float2 _Add_4C3CF1F_Out_2;
                        Unity_Add_float2(_Property_961D8A0_Out_0, (_Multiply_7DB42988_Out_2.xx), _Add_4C3CF1F_Out_2);
                        float2 _Property_EBC67BC7_Out_0 = Vector2_2A17E6EA;
                        float _Property_13D296B5_Out_0 = Vector1_F4B6A491;
                        float2 _Multiply_BBB72061_Out_2;
                        Unity_Multiply_float(_Property_EBC67BC7_Out_0, (_Property_13D296B5_Out_0.xx), _Multiply_BBB72061_Out_2);
                        float _Property_3BB601E6_Out_0 = Vector1_2C90770B;
                        float2 _Multiply_FF9010E8_Out_2;
                        Unity_Multiply_float(_Multiply_BBB72061_Out_2, (_Property_3BB601E6_Out_0.xx), _Multiply_FF9010E8_Out_2);
                        float2 _Subtract_6BF2D170_Out_2;
                        Unity_Subtract_float2(_Add_4C3CF1F_Out_2, _Multiply_FF9010E8_Out_2, _Subtract_6BF2D170_Out_2);
                        float4 _SampleTexture2DLOD_DBCD6404_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_D44B4848, samplerTexture2D_D44B4848, _Subtract_6BF2D170_Out_2, 0);
                        float _SampleTexture2DLOD_DBCD6404_R_5 = _SampleTexture2DLOD_DBCD6404_RGBA_0.r;
                        float _SampleTexture2DLOD_DBCD6404_G_6 = _SampleTexture2DLOD_DBCD6404_RGBA_0.g;
                        float _SampleTexture2DLOD_DBCD6404_B_7 = _SampleTexture2DLOD_DBCD6404_RGBA_0.b;
                        float _SampleTexture2DLOD_DBCD6404_A_8 = _SampleTexture2DLOD_DBCD6404_RGBA_0.a;
                        float4 _Combine_E5D76A97_RGBA_4;
                        float3 _Combine_E5D76A97_RGB_5;
                        float2 _Combine_E5D76A97_RG_6;
                        Unity_Combine_float(_SampleTexture2DLOD_DBCD6404_R_5, _SampleTexture2DLOD_DBCD6404_G_6, _SampleTexture2DLOD_DBCD6404_B_7, 0, _Combine_E5D76A97_RGBA_4, _Combine_E5D76A97_RGB_5, _Combine_E5D76A97_RG_6);
                        float3 _Subtract_AA7C02E2_Out_2;
                        Unity_Subtract_float3(_Combine_E5D76A97_RGB_5, float3(0.5, 0.5, 0.5), _Subtract_AA7C02E2_Out_2);
                        float3 _Multiply_5BF7CBD7_Out_2;
                        Unity_Multiply_float(_Subtract_AA7C02E2_Out_2, float3(2, 2, 2), _Multiply_5BF7CBD7_Out_2);
                        ShiverNoise_0 = _Multiply_5BF7CBD7_Out_2;
                    }
                
                    struct Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459
                    {
                    };
                
                    void SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(float3 Vector3_ED0F539A, float2 Vector2_84805101, float Vector1_BDF24CF7, float Vector1_839268A4, float Vector1_A8621014, float Vector1_2DBE6CC0, float Vector1_8A4EF006, float Vector1_ED935C73, Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 IN, out float3 ShiverDirection_0, out float ShiverStrength_1)
                    {
                        float3 _Property_FC94AEBB_Out_0 = Vector3_ED0F539A;
                        float _Property_4FE2271A_Out_0 = Vector1_BDF24CF7;
                        float4 _Combine_328044F1_RGBA_4;
                        float3 _Combine_328044F1_RGB_5;
                        float2 _Combine_328044F1_RG_6;
                        Unity_Combine_float(1, _Property_4FE2271A_Out_0, 1, 0, _Combine_328044F1_RGBA_4, _Combine_328044F1_RGB_5, _Combine_328044F1_RG_6);
                        float3 _Multiply_4FCE02F7_Out_2;
                        Unity_Multiply_float(_Property_FC94AEBB_Out_0, _Combine_328044F1_RGB_5, _Multiply_4FCE02F7_Out_2);
                        float2 _Property_77EED0A8_Out_0 = Vector2_84805101;
                        float _Split_2D66AF35_R_1 = _Property_77EED0A8_Out_0[0];
                        float _Split_2D66AF35_G_2 = _Property_77EED0A8_Out_0[1];
                        float _Split_2D66AF35_B_3 = 0;
                        float _Split_2D66AF35_A_4 = 0;
                        float4 _Combine_C2861A09_RGBA_4;
                        float3 _Combine_C2861A09_RGB_5;
                        float2 _Combine_C2861A09_RG_6;
                        Unity_Combine_float(_Split_2D66AF35_R_1, _Property_4FE2271A_Out_0, _Split_2D66AF35_G_2, 0, _Combine_C2861A09_RGBA_4, _Combine_C2861A09_RGB_5, _Combine_C2861A09_RG_6);
                        float3 _Lerp_A6B0BE86_Out_3;
                        Unity_Lerp_float3(_Multiply_4FCE02F7_Out_2, _Combine_C2861A09_RGB_5, float3(0.5, 0.5, 0.5), _Lerp_A6B0BE86_Out_3);
                        float _Property_BBBC9C1B_Out_0 = Vector1_839268A4;
                        float _Length_F022B321_Out_1;
                        Unity_Length_float3(_Multiply_4FCE02F7_Out_2, _Length_F022B321_Out_1);
                        float _Multiply_BFD84B03_Out_2;
                        Unity_Multiply_float(_Length_F022B321_Out_1, 0.5, _Multiply_BFD84B03_Out_2);
                        float _Multiply_3564B68A_Out_2;
                        Unity_Multiply_float(_Property_BBBC9C1B_Out_0, _Multiply_BFD84B03_Out_2, _Multiply_3564B68A_Out_2);
                        float _Add_83285742_Out_2;
                        Unity_Add_float(_Multiply_3564B68A_Out_2, _Length_F022B321_Out_1, _Add_83285742_Out_2);
                        float _Property_45D94B1_Out_0 = Vector1_2DBE6CC0;
                        float _Multiply_EA43D494_Out_2;
                        Unity_Multiply_float(_Add_83285742_Out_2, _Property_45D94B1_Out_0, _Multiply_EA43D494_Out_2);
                        float _Clamp_C109EA71_Out_3;
                        Unity_Clamp_float(_Multiply_EA43D494_Out_2, 0.1, 0.9, _Clamp_C109EA71_Out_3);
                        float _OneMinus_226F3377_Out_1;
                        Unity_OneMinus_float(_Clamp_C109EA71_Out_3, _OneMinus_226F3377_Out_1);
                        float _Multiply_8680628F_Out_2;
                        Unity_Multiply_float(_Multiply_EA43D494_Out_2, _OneMinus_226F3377_Out_1, _Multiply_8680628F_Out_2);
                        float _Multiply_B14E644_Out_2;
                        Unity_Multiply_float(_Multiply_8680628F_Out_2, 1.5, _Multiply_B14E644_Out_2);
                        float _Property_7F61FC78_Out_0 = Vector1_A8621014;
                        float _Multiply_C89CF7DC_Out_2;
                        Unity_Multiply_float(_Multiply_B14E644_Out_2, _Property_7F61FC78_Out_0, _Multiply_C89CF7DC_Out_2);
                        float _Property_2BD306B6_Out_0 = Vector1_8A4EF006;
                        float _Multiply_E5D34DCC_Out_2;
                        Unity_Multiply_float(_Multiply_C89CF7DC_Out_2, _Property_2BD306B6_Out_0, _Multiply_E5D34DCC_Out_2);
                        float _Property_DBC71A4F_Out_0 = Vector1_ED935C73;
                        float _Multiply_BCACDD38_Out_2;
                        Unity_Multiply_float(_Multiply_E5D34DCC_Out_2, _Property_DBC71A4F_Out_0, _Multiply_BCACDD38_Out_2);
                        ShiverDirection_0 = _Lerp_A6B0BE86_Out_3;
                        ShiverStrength_1 = _Multiply_BCACDD38_Out_2;
                    }
                
                    struct Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364
                    {
                    };
                
                    void SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(float3 Vector3_829210A7, float3 Vector3_1A016C4A, float Vector1_31372BF, float Vector1_E57895AF, TEXTURE2D_PARAM(Texture2D_65F71447, samplerTexture2D_65F71447), float4 Texture2D_65F71447_TexelSize, float Vector1_8836FB6A, TEXTURE2D_PARAM(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), float4 Texture2D_4A3BDB6_TexelSize, float Vector1_14E206AE, float Vector1_7090E96C, float Vector1_51722AC, float Vector1_A3894D2, float Vector1_6F0C3A5A, float Vector1_2D1F6C2F, float Vector1_347751CA, Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 IN, out float GustStrength_0, out float ShiverStrength_1, out float3 ShiverDirection_2)
                    {
                        float _Property_5C7F4A8C_Out_0 = Vector1_31372BF;
                        float _Property_142FEDC3_Out_0 = Vector1_347751CA;
                        float3 _Property_D2FC65C3_Out_0 = Vector3_829210A7;
                        float _Split_8E347DCF_R_1 = _Property_D2FC65C3_Out_0[0];
                        float _Split_8E347DCF_G_2 = _Property_D2FC65C3_Out_0[1];
                        float _Split_8E347DCF_B_3 = _Property_D2FC65C3_Out_0[2];
                        float _Split_8E347DCF_A_4 = 0;
                        float4 _Combine_9B5A76B7_RGBA_4;
                        float3 _Combine_9B5A76B7_RGB_5;
                        float2 _Combine_9B5A76B7_RG_6;
                        Unity_Combine_float(_Split_8E347DCF_R_1, _Split_8E347DCF_B_3, 0, 0, _Combine_9B5A76B7_RGBA_4, _Combine_9B5A76B7_RGB_5, _Combine_9B5A76B7_RG_6);
                        float3 _Property_5653999E_Out_0 = Vector3_1A016C4A;
                        float _Split_B9CBBFE5_R_1 = _Property_5653999E_Out_0[0];
                        float _Split_B9CBBFE5_G_2 = _Property_5653999E_Out_0[1];
                        float _Split_B9CBBFE5_B_3 = _Property_5653999E_Out_0[2];
                        float _Split_B9CBBFE5_A_4 = 0;
                        float4 _Combine_DC44394B_RGBA_4;
                        float3 _Combine_DC44394B_RGB_5;
                        float2 _Combine_DC44394B_RG_6;
                        Unity_Combine_float(_Split_B9CBBFE5_R_1, _Split_B9CBBFE5_B_3, 0, 0, _Combine_DC44394B_RGBA_4, _Combine_DC44394B_RGB_5, _Combine_DC44394B_RG_6);
                        float _Property_3221EFCE_Out_0 = Vector1_E57895AF;
                        Bindings_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f _GustNoiseAtPosition_3B28852B;
                        float _GustNoiseAtPosition_3B28852B_GustNoise_0;
                        SG_GustNoiseAtPosition_3a62cb513a478e64d9de042732615b0f(_Property_5C7F4A8C_Out_0, _Property_142FEDC3_Out_0, _Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_3221EFCE_Out_0, TEXTURE2D_ARGS(Texture2D_65F71447, samplerTexture2D_65F71447), Texture2D_65F71447_TexelSize, _GustNoiseAtPosition_3B28852B, _GustNoiseAtPosition_3B28852B_GustNoise_0);
                        float _Property_1B306054_Out_0 = Vector1_A3894D2;
                        float _Property_1FBC768_Out_0 = Vector1_51722AC;
                        float _Property_9FB10D19_Out_0 = Vector1_14E206AE;
                        Bindings_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19 _RandomStiffnessAtPosition_C9AD50AB;
                        float _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0;
                        SG_RandomStiffnessAtPosition_54185eec8ce79f44f864454f4caf0b19(_Combine_9B5A76B7_RG_6, _Property_9FB10D19_Out_0, _RandomStiffnessAtPosition_C9AD50AB, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0);
                        float _Property_EE5A603D_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628 _CalculateGustStrength_E2853C74;
                        float _CalculateGustStrength_E2853C74_GustStrength_0;
                        SG_CalculateGustStrength_37587ae2cefd5034ebbc219dea66a628(_GustNoiseAtPosition_3B28852B_GustNoise_0, _Property_1B306054_Out_0, _Property_1FBC768_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _Property_EE5A603D_Out_0, _CalculateGustStrength_E2853C74, _CalculateGustStrength_E2853C74_GustStrength_0);
                        float _Property_DFB3FCE0_Out_0 = Vector1_31372BF;
                        float _Property_8A8735CC_Out_0 = Vector1_8836FB6A;
                        Bindings_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a _ShiverNoiseAtPosition_35F9220A;
                        float3 _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0;
                        SG_ShiverNoiseAtPosition_677cf303a0132864e98cbf35206e8f1a(_Combine_9B5A76B7_RG_6, _Combine_DC44394B_RG_6, _Property_DFB3FCE0_Out_0, _Property_8A8735CC_Out_0, TEXTURE2D_ARGS(Texture2D_4A3BDB6, samplerTexture2D_4A3BDB6), Texture2D_4A3BDB6_TexelSize, _Property_142FEDC3_Out_0, _ShiverNoiseAtPosition_35F9220A, _ShiverNoiseAtPosition_35F9220A_ShiverNoise_0);
                        float _Property_65F19953_Out_0 = Vector1_6F0C3A5A;
                        float _Property_3A2F45FE_Out_0 = Vector1_51722AC;
                        float _Property_98EF73E5_Out_0 = Vector1_2D1F6C2F;
                        float _Property_6A278DE2_Out_0 = Vector1_7090E96C;
                        Bindings_CalculateShiver_2b675aaec6ba57b449ad461cc799d459 _CalculateShiver_799DE4CB;
                        float3 _CalculateShiver_799DE4CB_ShiverDirection_0;
                        float _CalculateShiver_799DE4CB_ShiverStrength_1;
                        SG_CalculateShiver_2b675aaec6ba57b449ad461cc799d459(_ShiverNoiseAtPosition_35F9220A_ShiverNoise_0, _Combine_DC44394B_RG_6, _Property_65F19953_Out_0, _CalculateGustStrength_E2853C74_GustStrength_0, _Property_3A2F45FE_Out_0, _Property_98EF73E5_Out_0, _Property_6A278DE2_Out_0, _RandomStiffnessAtPosition_C9AD50AB_RandomStiffness_0, _CalculateShiver_799DE4CB, _CalculateShiver_799DE4CB_ShiverDirection_0, _CalculateShiver_799DE4CB_ShiverStrength_1);
                        GustStrength_0 = _CalculateGustStrength_E2853C74_GustStrength_0;
                        ShiverStrength_1 = _CalculateShiver_799DE4CB_ShiverStrength_1;
                        ShiverDirection_2 = _CalculateShiver_799DE4CB_ShiverDirection_0;
                    }
                
                    struct Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01
                    {
                        float3 ObjectSpacePosition;
                    };
                
                    void SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(float3 Vector3_C96069F9, float Vector1_A5EB719C, float Vector1_4D1D3B1A, float3 Vector3_C80E97FF, float3 Vector3_821C320A, float3 Vector3_4BF0DC64, Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 IN, out float3 WorldPosition_0)
                    {
                        float3 _Property_65372844_Out_0 = Vector3_4BF0DC64;
                        float3 _Property_7205E35B_Out_0 = Vector3_821C320A;
                        float _Property_916D8D52_Out_0 = Vector1_4D1D3B1A;
                        float3 _Multiply_CF9DF1B5_Out_2;
                        Unity_Multiply_float(_Property_7205E35B_Out_0, (_Property_916D8D52_Out_0.xxx), _Multiply_CF9DF1B5_Out_2);
                        float3 _Multiply_57D2E5C7_Out_2;
                        Unity_Multiply_float(_Multiply_CF9DF1B5_Out_2, float3(10, 10, 10), _Multiply_57D2E5C7_Out_2);
                        float3 _Add_F265DF09_Out_2;
                        Unity_Add_float3(_Property_65372844_Out_0, _Multiply_57D2E5C7_Out_2, _Add_F265DF09_Out_2);
                        float3 _Property_806C350F_Out_0 = Vector3_C96069F9;
                        float _Property_D017A08E_Out_0 = Vector1_A5EB719C;
                        float3 _Multiply_99498CF9_Out_2;
                        Unity_Multiply_float(_Property_806C350F_Out_0, (_Property_D017A08E_Out_0.xxx), _Multiply_99498CF9_Out_2);
                        float _Split_A5777330_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_A5777330_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_A5777330_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_A5777330_A_4 = 0;
                        float _Clamp_C4364CA5_Out_3;
                        Unity_Clamp_float(_Split_A5777330_G_2, 0, 1, _Clamp_C4364CA5_Out_3);
                        float3 _Multiply_ADC4C2A_Out_2;
                        Unity_Multiply_float(_Multiply_99498CF9_Out_2, (_Clamp_C4364CA5_Out_3.xxx), _Multiply_ADC4C2A_Out_2);
                        float3 _Multiply_49835441_Out_2;
                        Unity_Multiply_float(_Multiply_ADC4C2A_Out_2, float3(10, 10, 10), _Multiply_49835441_Out_2);
                        float3 _Add_B14AAE70_Out_2;
                        Unity_Add_float3(_Add_F265DF09_Out_2, _Multiply_49835441_Out_2, _Add_B14AAE70_Out_2);
                        WorldPosition_0 = _Add_B14AAE70_Out_2;
                    }
                
                    struct Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549
                    {
                        float3 WorldSpaceNormal;
                        float3 WorldSpaceTangent;
                        float3 WorldSpaceBiTangent;
                    };
                
                    void SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(float3 Vector3_AAF445D6, Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 IN, out float3 ObjectPosition_1)
                    {
                        float3 _Property_51DA8EE_Out_0 = Vector3_AAF445D6;
                        float3 _Subtract_B236C96B_Out_2;
                        Unity_Subtract_float3(_Property_51DA8EE_Out_0, _WorldSpaceCameraPos, _Subtract_B236C96B_Out_2);
                        float3 _Transform_6FDB2E47_Out_1 = TransformWorldToObject(_Subtract_B236C96B_Out_2.xyz);
                        ObjectPosition_1 = _Transform_6FDB2E47_Out_1;
                    }
                
                // Vertex Graph Evaluation
                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        Bindings_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238 _GetPivotInWorldSpace_73F19E42;
                        float3 _GetPivotInWorldSpace_73F19E42_PivotInWS_0;
                        SG_GetPivotInWorldSpace_1854f41e48e6c5e4eb69a62e216c3238(_GetPivotInWorldSpace_73F19E42, _GetPivotInWorldSpace_73F19E42_PivotInWS_0);
                        float _Split_64420219_R_1 = IN.VertexColor[0];
                        float _Split_64420219_G_2 = IN.VertexColor[1];
                        float _Split_64420219_B_3 = IN.VertexColor[2];
                        float _Split_64420219_A_4 = IN.VertexColor[3];
                        float3 _Lerp_4531CF63_Out_3;
                        Unity_Lerp_float3(_GetPivotInWorldSpace_73F19E42_PivotInWS_0, IN.AbsoluteWorldSpacePosition, (_Split_64420219_G_2.xxx), _Lerp_4531CF63_Out_3);
                        float4 _Property_D6662DC6_Out_0 = _GlobalWindDirectionAndStrength;
                        float4 _Property_9515B228_Out_0 = _WindDirectionAndStrength;
                        float4 _Property_9A1EF240_Out_0 = _GlobalShiver;
                        float4 _Property_777C8DB2_Out_0 = _Shiver;
                        Bindings_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7 _GlobalWindParameters_B547F135;
                        float3 _GlobalWindParameters_B547F135_GustDirection_0;
                        float _GlobalWindParameters_B547F135_GustSpeed_1;
                        float _GlobalWindParameters_B547F135_GustStrength_2;
                        float _GlobalWindParameters_B547F135_ShiverSpeed_3;
                        float _GlobalWindParameters_B547F135_ShiverStrength_4;
                        SG_GlobalWindParameters_a3ea19cf24a41654f9e670ebabad18b7(_Property_D6662DC6_Out_0, _Property_9515B228_Out_0, (_Property_9A1EF240_Out_0.xy), (_Property_777C8DB2_Out_0.xy), _GlobalWindParameters_B547F135, _GlobalWindParameters_B547F135_GustDirection_0, _GlobalWindParameters_B547F135_GustSpeed_1, _GlobalWindParameters_B547F135_GustStrength_2, _GlobalWindParameters_B547F135_ShiverSpeed_3, _GlobalWindParameters_B547F135_ShiverStrength_4);
                        float _Property_5F3A390D_Out_0 = _BAKEDMASK_ON;
                        float3 _Subtract_BF2A75CD_Out_2;
                        Unity_Subtract_float3(IN.AbsoluteWorldSpacePosition, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _Subtract_BF2A75CD_Out_2);
                        float _Split_29C81DE4_R_1 = _Subtract_BF2A75CD_Out_2[0];
                        float _Split_29C81DE4_G_2 = _Subtract_BF2A75CD_Out_2[1];
                        float _Split_29C81DE4_B_3 = _Subtract_BF2A75CD_Out_2[2];
                        float _Split_29C81DE4_A_4 = 0;
                        float _Add_6A47DB4F_Out_2;
                        Unity_Add_float(_Split_29C81DE4_R_1, _Split_29C81DE4_G_2, _Add_6A47DB4F_Out_2);
                        float _Add_EC455B5D_Out_2;
                        Unity_Add_float(_Add_6A47DB4F_Out_2, _Split_29C81DE4_B_3, _Add_EC455B5D_Out_2);
                        float _Multiply_F013BB8B_Out_2;
                        Unity_Multiply_float(_Add_EC455B5D_Out_2, 0.4, _Multiply_F013BB8B_Out_2);
                        float _Fraction_7D389816_Out_1;
                        Unity_Fraction_float(_Multiply_F013BB8B_Out_2, _Fraction_7D389816_Out_1);
                        float _Multiply_776D3DAF_Out_2;
                        Unity_Multiply_float(_Fraction_7D389816_Out_1, 0.15, _Multiply_776D3DAF_Out_2);
                        float _Split_E4BB9FEC_R_1 = IN.VertexColor[0];
                        float _Split_E4BB9FEC_G_2 = IN.VertexColor[1];
                        float _Split_E4BB9FEC_B_3 = IN.VertexColor[2];
                        float _Split_E4BB9FEC_A_4 = IN.VertexColor[3];
                        float _Multiply_BC8988C3_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, _Split_E4BB9FEC_G_2, _Multiply_BC8988C3_Out_2);
                        float _Multiply_EC5FE292_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_R_1, 0.3, _Multiply_EC5FE292_Out_2);
                        float _Add_A8423510_Out_2;
                        Unity_Add_float(_Multiply_BC8988C3_Out_2, _Multiply_EC5FE292_Out_2, _Add_A8423510_Out_2);
                        float _Add_CE74358C_Out_2;
                        Unity_Add_float(_Add_A8423510_Out_2, IN.TimeParameters.x, _Add_CE74358C_Out_2);
                        float _Multiply_1CE438D_Out_2;
                        Unity_Multiply_float(_Split_E4BB9FEC_G_2, 0.5, _Multiply_1CE438D_Out_2);
                        float _Add_8718B88C_Out_2;
                        Unity_Add_float(_Add_CE74358C_Out_2, _Multiply_1CE438D_Out_2, _Add_8718B88C_Out_2);
                        float _Property_DBA903E3_Out_0 = _UVMASK_ON;
                        float4 _UV_64D01E18_Out_0 = IN.uv0;
                        float _Split_A5DFBEBE_R_1 = _UV_64D01E18_Out_0[0];
                        float _Split_A5DFBEBE_G_2 = _UV_64D01E18_Out_0[1];
                        float _Split_A5DFBEBE_B_3 = _UV_64D01E18_Out_0[2];
                        float _Split_A5DFBEBE_A_4 = _UV_64D01E18_Out_0[3];
                        float _Multiply_C943DA5C_Out_2;
                        Unity_Multiply_float(_Split_A5DFBEBE_G_2, 0.1, _Multiply_C943DA5C_Out_2);
                        float _Branch_12012434_Out_3;
                        Unity_Branch_float(_Property_DBA903E3_Out_0, _Multiply_C943DA5C_Out_2, 0, _Branch_12012434_Out_3);
                        float _Add_922F2E64_Out_2;
                        Unity_Add_float(IN.TimeParameters.x, _Branch_12012434_Out_3, _Add_922F2E64_Out_2);
                        float _Multiply_2E689843_Out_2;
                        Unity_Multiply_float(_Multiply_776D3DAF_Out_2, 0.5, _Multiply_2E689843_Out_2);
                        float _Add_ED1EE4DB_Out_2;
                        Unity_Add_float(_Add_922F2E64_Out_2, _Multiply_2E689843_Out_2, _Add_ED1EE4DB_Out_2);
                        float _Branch_291934CD_Out_3;
                        Unity_Branch_float(_Property_5F3A390D_Out_0, _Add_8718B88C_Out_2, _Add_ED1EE4DB_Out_2, _Branch_291934CD_Out_3);
                        float _Property_267CF497_Out_0 = _StiffnessVariation;
                        float _Property_4FB02E51_Out_0 = _BAKEDMASK_ON;
                        float4 _UV_6482E163_Out_0 = IN.uv1;
                        float _Split_2D1A67CF_R_1 = _UV_6482E163_Out_0[0];
                        float _Split_2D1A67CF_G_2 = _UV_6482E163_Out_0[1];
                        float _Split_2D1A67CF_B_3 = _UV_6482E163_Out_0[2];
                        float _Split_2D1A67CF_A_4 = _UV_6482E163_Out_0[3];
                        float _Multiply_F7BD1E76_Out_2;
                        Unity_Multiply_float(_Split_2D1A67CF_R_1, _Split_2D1A67CF_G_2, _Multiply_F7BD1E76_Out_2);
                        float _Property_B1FAFDBF_Out_0 = _UVMASK_ON;
                        float4 _UV_8F58F10B_Out_0 = IN.uv0;
                        float _Split_BD0858B3_R_1 = _UV_8F58F10B_Out_0[0];
                        float _Split_BD0858B3_G_2 = _UV_8F58F10B_Out_0[1];
                        float _Split_BD0858B3_B_3 = _UV_8F58F10B_Out_0[2];
                        float _Split_BD0858B3_A_4 = _UV_8F58F10B_Out_0[3];
                        float _Multiply_3FAD9403_Out_2;
                        Unity_Multiply_float(_Split_BD0858B3_G_2, 0.2, _Multiply_3FAD9403_Out_2);
                        float _Branch_3AF3832A_Out_3;
                        Unity_Branch_float(_Property_B1FAFDBF_Out_0, _Multiply_3FAD9403_Out_2, 1, _Branch_3AF3832A_Out_3);
                        float _Branch_F921E5A9_Out_3;
                        Unity_Branch_float(_Property_4FB02E51_Out_0, _Multiply_F7BD1E76_Out_2, _Branch_3AF3832A_Out_3, _Branch_F921E5A9_Out_3);
                        Bindings_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364 _GetWindStrength_5806EF0A;
                        float _GetWindStrength_5806EF0A_GustStrength_0;
                        float _GetWindStrength_5806EF0A_ShiverStrength_1;
                        float3 _GetWindStrength_5806EF0A_ShiverDirection_2;
                        SG_GetWindStrength_e01e2c42537f3fa47bc76ed6e6291364(_Lerp_4531CF63_Out_3, _GlobalWindParameters_B547F135_GustDirection_0, _Branch_291934CD_Out_3, _GlobalWindParameters_B547F135_GustSpeed_1, TEXTURE2D_ARGS(_GustNoise, sampler_GustNoise), _GustNoise_TexelSize, _GlobalWindParameters_B547F135_ShiverSpeed_3, TEXTURE2D_ARGS(_ShiverNoise, sampler_ShiverNoise), _ShiverNoise_TexelSize, _Property_267CF497_Out_0, 1, _Branch_F921E5A9_Out_3, _GlobalWindParameters_B547F135_GustStrength_2, 0.2, _GlobalWindParameters_B547F135_ShiverStrength_4, 0, _GetWindStrength_5806EF0A, _GetWindStrength_5806EF0A_GustStrength_0, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_ShiverDirection_2);
                        Bindings_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01 _ApplyTreeWindDisplacement_8E73FF2E;
                        _ApplyTreeWindDisplacement_8E73FF2E.ObjectSpacePosition = IN.ObjectSpacePosition;
                        float3 _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0;
                        SG_ApplyTreeWindDisplacement_a14dde8a0bb9d794f95b12472fe09b01(_GetWindStrength_5806EF0A_ShiverDirection_2, _GetWindStrength_5806EF0A_ShiverStrength_1, _GetWindStrength_5806EF0A_GustStrength_0, _GetPivotInWorldSpace_73F19E42_PivotInWS_0, _GlobalWindParameters_B547F135_GustDirection_0, IN.AbsoluteWorldSpacePosition, _ApplyTreeWindDisplacement_8E73FF2E, _ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0);
                        Bindings_WorldToObject_c1006c77aa2ba8543ab0d5302579f549 _WorldToObject_628B231E;
                        _WorldToObject_628B231E.WorldSpaceNormal = IN.WorldSpaceNormal;
                        _WorldToObject_628B231E.WorldSpaceTangent = IN.WorldSpaceTangent;
                        _WorldToObject_628B231E.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
                        float3 _WorldToObject_628B231E_ObjectPosition_1;
                        SG_WorldToObject_c1006c77aa2ba8543ab0d5302579f549(_ApplyTreeWindDisplacement_8E73FF2E_WorldPosition_0, _WorldToObject_628B231E, _WorldToObject_628B231E_ObjectPosition_1);
                        description.VertexPosition = _WorldToObject_628B231E_ObjectPosition_1;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }
                    
                // Pixel Graph Evaluation
                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float4 _SampleTexture2D_F86B9939_RGBA_0 = SAMPLE_TEXTURE2D(_Albedo, sampler_Albedo, IN.uv0.xy);
                        float _SampleTexture2D_F86B9939_R_4 = _SampleTexture2D_F86B9939_RGBA_0.r;
                        float _SampleTexture2D_F86B9939_G_5 = _SampleTexture2D_F86B9939_RGBA_0.g;
                        float _SampleTexture2D_F86B9939_B_6 = _SampleTexture2D_F86B9939_RGBA_0.b;
                        float _SampleTexture2D_F86B9939_A_7 = _SampleTexture2D_F86B9939_RGBA_0.a;
                        float _Property_667D0001_Out_0 = _Hue;
                        float3 _Hue_BE270ED0_Out_2;
                        Unity_Hue_Normalized_float((_SampleTexture2D_F86B9939_RGBA_0.xyz), _Property_667D0001_Out_0, _Hue_BE270ED0_Out_2);
                        float _Property_306B4B17_Out_0 = _Saturation;
                        float _Add_27F91AF7_Out_2;
                        Unity_Add_float(_Property_306B4B17_Out_0, 1, _Add_27F91AF7_Out_2);
                        float3 _Saturation_8EFFDFE8_Out_2;
                        Unity_Saturation_float(_Hue_BE270ED0_Out_2, _Add_27F91AF7_Out_2, _Saturation_8EFFDFE8_Out_2);
                        float _Property_35742C6B_Out_0 = _Lightness;
                        float3 _Add_53649F0F_Out_2;
                        Unity_Add_float3(_Saturation_8EFFDFE8_Out_2, (_Property_35742C6B_Out_0.xxx), _Add_53649F0F_Out_2);
                        float4 _SampleTexture2D_12F932C1_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                        _SampleTexture2D_12F932C1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_12F932C1_RGBA_0);
                        float _SampleTexture2D_12F932C1_R_4 = _SampleTexture2D_12F932C1_RGBA_0.r;
                        float _SampleTexture2D_12F932C1_G_5 = _SampleTexture2D_12F932C1_RGBA_0.g;
                        float _SampleTexture2D_12F932C1_B_6 = _SampleTexture2D_12F932C1_RGBA_0.b;
                        float _SampleTexture2D_12F932C1_A_7 = _SampleTexture2D_12F932C1_RGBA_0.a;
                        float4 _SampleTexture2D_E3683686_RGBA_0 = SAMPLE_TEXTURE2D(_ThicknessMap, sampler_ThicknessMap, IN.uv0.xy);
                        float _SampleTexture2D_E3683686_R_4 = _SampleTexture2D_E3683686_RGBA_0.r;
                        float _SampleTexture2D_E3683686_G_5 = _SampleTexture2D_E3683686_RGBA_0.g;
                        float _SampleTexture2D_E3683686_B_6 = _SampleTexture2D_E3683686_RGBA_0.b;
                        float _SampleTexture2D_E3683686_A_7 = _SampleTexture2D_E3683686_RGBA_0.a;
                        float4 _SampleTexture2D_FFEA8409_RGBA_0 = SAMPLE_TEXTURE2D(_MaskMap, sampler_MaskMap, IN.uv0.xy);
                        float _SampleTexture2D_FFEA8409_R_4 = _SampleTexture2D_FFEA8409_RGBA_0.r;
                        float _SampleTexture2D_FFEA8409_G_5 = _SampleTexture2D_FFEA8409_RGBA_0.g;
                        float _SampleTexture2D_FFEA8409_B_6 = _SampleTexture2D_FFEA8409_RGBA_0.b;
                        float _SampleTexture2D_FFEA8409_A_7 = _SampleTexture2D_FFEA8409_RGBA_0.a;
                        float _Property_ABA23041_Out_0 = _AlphaClip;
                        surface.Albedo = _Add_53649F0F_Out_2;
                        surface.Normal = (_SampleTexture2D_12F932C1_RGBA_0.xyz);
                        surface.BentNormal = IN.TangentSpaceNormal;
                        surface.Thickness = _SampleTexture2D_E3683686_R_4;
                        surface.DiffusionProfileHash = _DiffusionProfileHash;
                        surface.CoatMask = 0;
                        surface.Emission = float3(0, 0, 0);
                        surface.Smoothness = _SampleTexture2D_FFEA8409_A_7;
                        surface.Occlusion = _SampleTexture2D_FFEA8409_G_5;
                        surface.Alpha = _SampleTexture2D_F86B9939_A_7;
                        surface.AlphaClipThreshold = _Property_ABA23041_Out_0;
                        return surface;
                    }
                    
            //-------------------------------------------------------------------------------------
            // End graph generated code
            //-------------------------------------------------------------------------------------
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
            
            VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                // output.ViewSpaceNormal =             TransformWorldToViewDir(output.WorldSpaceNormal);
                // output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.WorldSpaceTangent =           TransformObjectToWorldDir(input.tangentOS.xyz);
                // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                output.ObjectSpaceBiTangent =        normalize(cross(input.normalOS, input.tangentOS) * (input.tangentOS.w > 0.0f ? 1.0f : -1.0f) * GetOddNegativeScale());
                output.WorldSpaceBiTangent =         TransformObjectToWorldDir(output.ObjectSpaceBiTangent);
                // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                output.ObjectSpacePosition =         input.positionOS;
                // output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                // output.ViewSpacePosition =           TransformWorldToView(output.WorldSpacePosition);
                // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(TransformObjectToWorld(input.positionOS));
                // output.WorldSpaceViewDirection =     GetWorldSpaceNormalizeViewDir(output.WorldSpacePosition);
                // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(output.WorldSpacePosition), _ProjectionParams.x);
                output.uv0 =                         input.uv0;
                output.uv1 =                         input.uv1;
                // output.uv2 =                         input.uv2;
                // output.uv3 =                         input.uv3;
                output.VertexColor =                 input.color;
            
                return output;
            }
            
            AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            {
                // build graph inputs
                VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
                // Override time paramters with used one (This is required to correctly handle motion vector for vertex animation based on time)
                vertexDescriptionInputs.TimeParameters = timeParameters;
            
                // evaluate vertex graph
                VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
            
                // copy graph output to the results
                input.positionOS = vertexDescription.VertexPosition;
                input.normalOS = vertexDescription.VertexNormal;
                input.tangentOS.xyz = vertexDescription.VertexTangent;
            
                return input;
            }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : VertexAnimation.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
        //-------------------------------------------------------------------------------------
            // TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
                FragInputs BuildFragInputs(VaryingsMeshToPS input)
                {
                    FragInputs output;
                    ZERO_INITIALIZE(FragInputs, output);
            
                    // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
                    // TODO: this is a really poor workaround, but the variable is used in a bunch of places
                    // to compute normals which are then passed on elsewhere to compute other values...
                    output.tangentToWorld = k_identity3x3;
                    output.positionSS = input.positionCS;       // input.positionCS is SV_Position
            
                    output.positionRWS = input.positionRWS;
                    output.tangentToWorld = BuildTangentToWorld(input.tangentWS, input.normalWS);
                    output.texCoord0 = input.texCoord0;
                    output.texCoord1 = input.texCoord1;
                    output.texCoord2 = input.texCoord2;
                    // output.texCoord3 = input.texCoord3;
                    // output.color = input.color;
                    #if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #elif SHADER_STAGE_FRAGMENT
                    output.isFrontFace = IS_FRONT_VFACE(input.cullFace, true, false);
                    #endif // SHADER_STAGE_FRAGMENT
            
                    return output;
                }
            
                SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                    // output.WorldSpaceNormal =            normalize(input.tangentToWorld[2].xyz);
                    // output.ObjectSpaceNormal =           mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M);           // transposed multiplication by inverse matrix to handle normal scale
                    // output.ViewSpaceNormal =             mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_I_V);         // transposed multiplication by inverse matrix to handle normal scale
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                    // output.WorldSpaceTangent =           input.tangentToWorld[0].xyz;
                    // output.ObjectSpaceTangent =          TransformWorldToObjectDir(output.WorldSpaceTangent);
                    // output.ViewSpaceTangent =            TransformWorldToViewDir(output.WorldSpaceTangent);
                    // output.TangentSpaceTangent =         float3(1.0f, 0.0f, 0.0f);
                    // output.WorldSpaceBiTangent =         input.tangentToWorld[1].xyz;
                    // output.ObjectSpaceBiTangent =        TransformWorldToObjectDir(output.WorldSpaceBiTangent);
                    // output.ViewSpaceBiTangent =          TransformWorldToViewDir(output.WorldSpaceBiTangent);
                    // output.TangentSpaceBiTangent =       float3(0.0f, 1.0f, 0.0f);
                    // output.WorldSpaceViewDirection =     normalize(viewWS);
                    // output.ObjectSpaceViewDirection =    TransformWorldToObjectDir(output.WorldSpaceViewDirection);
                    // output.ViewSpaceViewDirection =      TransformWorldToViewDir(output.WorldSpaceViewDirection);
                    // float3x3 tangentSpaceTransform =     float3x3(output.WorldSpaceTangent,output.WorldSpaceBiTangent,output.WorldSpaceNormal);
                    // output.TangentSpaceViewDirection =   mul(tangentSpaceTransform, output.WorldSpaceViewDirection);
                    // output.WorldSpacePosition =          input.positionRWS;
                    // output.ObjectSpacePosition =         TransformWorldToObject(input.positionRWS);
                    // output.ViewSpacePosition =           TransformWorldToView(input.positionRWS);
                    // output.TangentSpacePosition =        float3(0.0f, 0.0f, 0.0f);
                    // output.AbsoluteWorldSpacePosition =  GetAbsolutePositionWS(input.positionRWS);
                    // output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionRWS), _ProjectionParams.x);
                    output.uv0 =                         input.texCoord0;
                    // output.uv1 =                         input.texCoord1;
                    // output.uv2 =                         input.texCoord2;
                    // output.uv3 =                         input.texCoord3;
                    // output.VertexColor =                 input.color;
                    // output.FaceSign =                    input.isFrontFace;
                    // output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            
                    return output;
                }
            
                // existing HDRP code uses the combined function to go directly from packed to frag inputs
                FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    VaryingsMeshToPS unpacked= UnpackVaryingsMeshToPS(input);
                    return BuildFragInputs(unpacked);
                }
            
            //-------------------------------------------------------------------------------------
            // END TEMPLATE INCLUDE : SharedCode.template.hlsl
            //-------------------------------------------------------------------------------------
            
        
            void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
            {
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(SurfaceData, surfaceData);
        
                // copy across graph values, if defined
                surfaceData.baseColor =                 surfaceDescription.Albedo;
                surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
                // surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
                // surfaceData.metallic =                  surfaceDescription.Metallic;
                // surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
                surfaceData.thickness =                 surfaceDescription.Thickness;
                surfaceData.diffusionProfileHash =      asuint(surfaceDescription.DiffusionProfileHash);
                // surfaceData.specularColor =             surfaceDescription.Specular;
                surfaceData.coatMask =                  surfaceDescription.CoatMask;
                // surfaceData.anisotropy =                surfaceDescription.Anisotropy;
                // surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
                // surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
        
        #ifdef _HAS_REFRACTION
                if (_EnableSSRefraction)
                {
                    // surfaceData.ior =                       surfaceDescription.RefractionIndex;
                    // surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                    // surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                    surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                    surfaceDescription.Alpha = 1.0;
                }
                else
                {
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                    surfaceDescription.Alpha = 1.0;
                }
        #else
                surfaceData.ior = 1.0;
                surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                surfaceData.atDistance = 1.0;
                surfaceData.transmittanceMask = 0.0;
        #endif
                
                // These static material feature allow compile time optimization
                surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
        #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
        #endif
        #ifdef _MATERIAL_FEATURE_TRANSMISSION
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
        #endif
        #ifdef _MATERIAL_FEATURE_ANISOTROPY
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
        #endif
                // surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
        
        #ifdef _MATERIAL_FEATURE_IRIDESCENCE
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
        #endif
        #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
        #endif
        
        #if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                // Require to have setup baseColor
                // Reproduce the energy conservation done in legacy Unity. Not ideal but better for compatibility and users can unchek it
                surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                // tangent-space normal
                float3 normalTS = float3(0.0f, 0.0f, 1.0f);
                normalTS = surfaceDescription.Normal;
        
                // compute world space normal
                GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
        
                bentNormalWS = surfaceData.normalWS;
                // GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants);
        
                surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
        
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz);    // The tangent is not normalize in tangentToWorld for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
                // surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
                // By default we use the ambient occlusion with Tri-ace trick (apply outside) for specular occlusion.
                // If user provide bent normal then we process a better term
        #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                // Just use the value passed through via the slot (not active otherwise)
        #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                // If we have bent normal and ambient occlusion, process a specular occlusion
                surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
        #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
        #else
                surfaceData.specularOcclusion = 1.0;
        #endif
        
        #if HAVE_DECALS
                if (_EnableDecals)
                {
                    DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, surfaceDescription.Alpha);
                    ApplyDecalToSurfaceData(decalSurfaceData, surfaceData);
                }
        #endif
        
        #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
        #endif
        
        #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    // TODO: need to update mip info
                    surfaceData.metallic = 0;
                }
        
                // We need to call ApplyDebugToSurfaceData after filling the surfarcedata and before filling builtinData
                // as it can modify attribute use for static lighting
                ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
        #endif
            }
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
        #ifdef _DOUBLESIDED_ON
            float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
        #else
            float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
        #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPrepass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdDepthPostpass);
                // DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
                
                // ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
        
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal 
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                // override sampleBakedGI:
                // builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
                // builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // builtinData.depthOffset = surfaceDescription.DepthOffset;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
            //-------------------------------------------------------------------------------------
            // Pass Includes
            //-------------------------------------------------------------------------------------
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForward.hlsl"
            //-------------------------------------------------------------------------------------
            // End Pass Includes
            //-------------------------------------------------------------------------------------
        
            ENDHLSL
        }
        
    }
    CustomEditor "VisualDesignCafe.Nature.Editor.HdrpNatureMaterialEditor"
    FallBack "Hidden/InternalErrorShader"
}
