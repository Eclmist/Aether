#include "noiseSimplex.cginc"

float2 _WorldVisibilityTextureSize = float2(512, 512);

float4 Sample_Visibility(float3 absWorldPosition)
{
    float2 uv = (absWorldPosition.rb + _WorldVisibilityTextureSize * 0.5) / _WorldVisibilityTextureSize;
    return SAMPLE_TEXTURE2D(_WorldVisibilityTexture, sampler_WorldVisibilityTexture, uv);
}

float ATH_Compute_Visibility(float3 absWorldPosition)
{
    return Sample_Visibility(absWorldPosition).r;
}

float ATH_Compute_Visibility_G(float3 absWorldPosition, float alphaClip)
{
    float4 visTex = Sample_Visibility(absWorldPosition);
    float remapHeight = saturate(absWorldPosition.y / 50.0f);
    float visibility = visTex.r;
    visibility = min(visibility, smoothstep(visTex.g, visTex.g - alphaClip, remapHeight));
    visibility -= 0.1f * (snoise(absWorldPosition + _Time.r * 5) + 1) / 2;
    return visibility;
}

float3 ATH_Compute_Emission(float3 absWorldPosition, float alphaClip)
{
    float visTex = ATH_Compute_Visibility_G(absWorldPosition, alphaClip);
    float glowAmt = 1 - smoothstep(0, alphaClip + 0.15, visTex);
    glowAmt = saturate(glowAmt * glowAmt);
    return glowAmt * 10000000 * float3(0.6, 0.3, 1.0);
}

float InterleavedGradientNoise(float2 uv)
{
    float3 magic = float3(0.06711056, 0.00583715, 52.9829189);
    return frac(magic.z * frac(dot(uv, magic.xy)));
}