Shader "Custom/WaterParticleDepth" {
	Properties {
		_LightDir ("Light Direction", Vector) = (1,0,0)
	}
	SubShader {
		Tags {
			"IgnoreProjector"="True"
			"RenderType"="Opaque"
		}
	    Pass {
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct VSInput{
				float4 Position : POSITION;
				float2 TexCoord : TEXCOORD;
			};
			
			struct VSOutput{
				float4 Position : SV_POSITION;
            	float2 texCoord : TEXCOORD0;
            	float3 eyeSpacePos : TEXCOORD1;
            	float sphereRadius : TEXCOORD2;
            	float4 color : COLOR0;
			};
			
			VSOutput vert(VSInput IN) // vertex shader 
            {
            	VSOutput OUT = (VSOutput) 0;
            	
            	OUT.Position = mul(UNITY_MATRIX_MVP, IN.Position);
            	OUT.texCoord = IN.TexCoord;
            	
            	OUT.eyeSpacePos = mul(UNITY_MATRIX_MV, IN.Position).xyz;
            	OUT.sphereRadius = 1;
            	OUT.color = float4(1,.5,0,1);
            	
            	return OUT;
            }
            
            float4 _LightDir;
            
            struct PSOutput{
            	float4 fragColor : COLOR0;
            	//float fragDepth : DEPTH;
            };
            
            PSOutput frag(
            	float2 texCoord		: TEXCOORD0,
            	float3 eyeSpacePos	: TEXCOORD1,
            	float sphereRadius	: TEXCOORD2,
            	float4 color		: COLOR0) // fragment shader
            {
            	PSOutput OUT;
            	
            	// calculate eye-space sphere normal from texture coordinates
            	float3 N;
            	N.xy = texCoord*2.0-1.0;
            	float r2 = dot(N.xy, N.xy);
            	if (r2 > sphereRadius) discard; // kill pixels outside circle
            	N.z = sqrt(1.0 - r2);
            	
            	// calculate depth
            	float4 pixelPos = float4(eyeSpacePos + N*sphereRadius, 1.0);
            	float4 clipSpacePos = mul(pixelPos, UNITY_MATRIX_P);
            	//OUT.fragDepth = clipSpacePos.z / clipSpacePos.w;
            	
            	//float diffuse = max(0.0, dot(N, _LightDir.xyz));
            	//OUT.fragColor = float4(N, 1); // color * diffuse;
            	
            	// !!
            	float depth = clipSpacePos.z / clipSpacePos.w;
            	
            	OUT.fragColor = float4(depth, depth, depth, 1);
            	//OUT.fragColor = float4(r2, r2, r2, 1);
            	
            	return OUT;
            }
            
            ENDCG
		}
	}
	FallBack "Diffuse"
}
