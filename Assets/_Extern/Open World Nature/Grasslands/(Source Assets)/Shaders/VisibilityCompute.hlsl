float ATH_Compute_Visibility(float3 absWorldPosition)
{
    float2 _WorldVisibilityTextureSize = float2(512, 512);
    float2 uv = (absWorldPosition.rb + _WorldVisibilityTextureSize * 0.5) / _WorldVisibilityTextureSize;
    return SAMPLE_TEXTURE2D(_WorldVisibilityTexture, sampler_WorldVisibilityTexture, uv);
}
