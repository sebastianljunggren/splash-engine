Shader "Custom/ParticleSphere" {
	Properties {
		//_LightDir ("Light Direction", Vector) = (1,0,0)
		_DepthBuffer ("Depth Buffer", 2D) = "white" {}
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
            	float2 ScreenPosition : TEXCOORD3;
			};
			
			VSOutput vert(VSInput IN) // vertex shader 
            {
            	VSOutput OUT = (VSOutput) 0;
            	
            	OUT.Position = mul(UNITY_MATRIX_MVP, IN.Position);
            	OUT.texCoord = IN.TexCoord;
            	
            	OUT.eyeSpacePos = mul(UNITY_MATRIX_MV, IN.Position).xyz;
            	OUT.sphereRadius = 1;
            	OUT.color = float4(1,.5,0,1);
            	
				OUT.ScreenPosition=ComputeScreenPos(OUT.Position);
            	
            	return OUT;
            }
            
            float4 _LightDir;
            sampler2D _DepthBuffer;
            
            struct PSOutput{
            	float4 fragColor : COLOR0;
            	//float fragDepth : DEPTH;
            };
            
            float3 uvToEye(float2 uv, float depth){
            	return float3(uv.x, uv.y, depth);
            }
            
            float3 getEyePos(sampler2D dt, float2 uv){
            	return uvToEye(uv, tex2D(dt, uv).x); 
            }
            
            PSOutput frag(
            	float2 texCoord		: TEXCOORD0,
            	float3 eyeSpacePos	: TEXCOORD1,
            	float sphereRadius	: TEXCOORD2,
            	float4 color		: COLOR0,
            	float2 ScreenPosition : TEXCOORD3) // fragment shader
            {
            	PSOutput OUT;
            	
            	// calculate eye-space sphere normal from texture coordinates
            	float3 N;
            	N.xy = texCoord*2.0-1.0;
            	float r2 = dot(N.xy, N.xy);
            	if (r2 > sphereRadius) discard; // kill pixels outside circle
            	N.z = sqrt(1.0 - r2);
            	
            	// calculate depth
            	//float4 pixelPos = float4(eyeSpacePos + N*sphereRadius, 1.0);
            	//float4 clipSpacePos = mul(pixelPos, UNITY_MATRIX_P);
            	//OUT.fragDepth = clipSpacePos.z / clipSpacePos.w;
            	
            	//float diffuse = max(0.0, dot(N, _LightDir.xyz));
            	//OUT.fragColor = float4(N, 1); // color * diffuse;
            	
            	//float depth = tex2D(_DepthBuffer, texCoord);
            	//OUT.fragColor = float4(depth, depth, depth, 1);
            	
            	// ------------- CODE BELOW FROM SLIDES
            	
            	float maxDepth = 100;
            	float texelSize = 1 / 512.0;
            	sampler2D depthTex = _DepthBuffer; 
            	
            	texCoord = ScreenPosition;
            	
            	// read eye-space depth from texture
				float depth = tex2D(depthTex, texCoord).x;
				
				OUT.fragColor = float4(depth, depth,depth, 1);
				return OUT;
				
				if (depth > maxDepth) {
					discard;
				}

				// calculate eye-space position from depth
				float3 posEye = uvToEye(texCoord, depth);

				// calculate differences
				float3 ddx = getEyePos(depthTex, texCoord + float2(texelSize, 0)) - posEye;
				float3 ddx2 = posEye - getEyePos(depthTex, texCoord + float2(-texelSize, 0));
				if (abs(ddx.z) > abs(ddx2.z)) {
					ddx = ddx2;
				}

				float3 ddy = getEyePos(depthTex, texCoord + float2(0, texelSize)) - posEye;
				float3 ddy2 = posEye - getEyePos(depthTex, texCoord + float2(0, -texelSize));
				if (abs(ddy2.z) < abs(ddy.z)) {
					ddy = ddy2;
				}

				// calculate normal
				float3 n = cross(ddx, ddy);
				n = normalize(n);
            	
            	OUT.fragColor = float4(n, 1);
            	//OUT.fragColor = float4(1, 1, 0, 1);
            	
            	return OUT;
            }
            
            
            ENDCG
		}
	}
	FallBack "Diffuse"
}
