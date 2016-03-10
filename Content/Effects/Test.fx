float4x4 World;
float4x4 View;
float4x4 Projection;
float4 CameraPosition : POSITION0;
float SpotAngle;
float SpotReach;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION;
	float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection); 
	float3 viewPostionNorm = normalize(viewPosition.xyz);
	float angle = abs(acos(dot(viewPostionNorm, float3(0, 0, -1))));   

	if(distance(viewPosition.xyz,float3(0,0,0))<SpotReach && angle<radians(SpotAngle)) {
		output.Color = float4(1.0, 1.0, 1.0,1.0);
	}
	else {
		output.Color = float4(0.5,0.5,0.5,1);
	}

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return input.Color;
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}