int num;
sampler TextureSampler : register(s0);
sampler DistortionSampler : register(s1);

float4 PixelShaderFunction(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 distortion = tex2D(DistortionSampler, sin(num * 50) * texCoord / 3);
    texCoord += distortion * 0.2 - 0.15;

	color.a = 0.5;
	color.bg -= 0.5 * sin(num * 25);

    return tex2D(TextureSampler, texCoord) * color;
}

technique Shaders
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}