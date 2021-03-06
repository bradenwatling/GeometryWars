int num;
sampler TextureSampler : register(s0);

float4 PixelShaderFunction(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	color.g = cos(num * 500);
	color.r = tan(num * 500);
	color.b -= sin(num * 500);

    return tex2D(TextureSampler, texCoord) * color;
}

technique Shaders
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}