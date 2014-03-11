Shader "Custom/DepthWriter" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 scrPos: TEXCOORD1;
			};

			//Our Vertex Shader
			v2f vert (appdata_base v) {
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.scrPos=ComputeScreenPos(o.pos);
				//o.scrPos.y = 1 - o.scrPos.y;
				return o;
			}

			//Our Fragment Shader
			float4 frag (v2f i) : COLOR0 {
			
				//float4 color = tex2D(_MainTex, i.scrPos.xy);
				float depth = tex2D(_CameraDepthTexture, i.scrPos.xy);
				
				return float4(depth, depth, depth, 1);
				
				//OUT.norm = float4(normalValues, 1);
				//OUT.depth = float4(depthValue);
				
				//half4 c = tex2D(_MainTex, i.scrPos.xy);
				
				//if (i.scrPos.x < .5 && i.scrPos.y < .5)
					//output = half4(depthValue,depthValue,depthValue,1);
				
				//return half4(normalValues, 1); //  output;
				//half4 output = half4(i.scrPos.x,i.scrPos.y,0,1);
				
				//o.col = 
				//o.norm = normal;
				
				//float4 output;
				
				//color.rgb *= depth;
				//if (i.scrPos.x < .5)
					//output = color; //float4(normal, 1);
				//else
					//output = float4(depth, depth, depth, 1);
				//return output;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
