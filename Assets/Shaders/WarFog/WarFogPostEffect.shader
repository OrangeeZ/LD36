Shader "Hidden/WarFogPostEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_WarFogTexture("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.worldPos = mul(_Object2World, v.vertex);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _WarFogTexture;
			sampler2D _CameraDepthTexture;
			float4x4 _World2Texture;
			float4x4 _ViewProjectInverse;
			float4x4 _Camera2World;
			fixed _WarFogBrightness;
			fixed _MaxFieldDistance;
			fixed3 _WorldTracerPosition;

            fixed4 GetWorldPosition(fixed2 uv)
            {
                #if UNITY_UV_STARTS_AT_TOP
                    uv.y = 1 - uv.y;
                #endif

                fixed depth = (SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv.xy) * 2 - 1);

                if (Linear01Depth(depth) > 0.99)
                {
                    return 0;
                }

                fixed4 clipSpacePosition = 0;
                clipSpacePosition.xy = uv.xy * 2 - 1;
                clipSpacePosition.z = depth;
                clipSpacePosition.w = 1;

                fixed4 worldPosition = mul(_ViewProjectInverse, clipSpacePosition);
                worldPosition /= worldPosition.w;

                return worldPosition;
            }

			float SampleDistanceField(fixed4 worldPosition)
			{
    			half2 worldCoords = mul(_World2Texture, worldPosition).xz - half2(0.5, 0.5);

                return pow(tex2D(_WarFogTexture, worldCoords).a, 1.1) * _MaxFieldDistance;// - 1 / _MaxFieldDistance;
			}

			fixed4 frag(v2f i) : SV_Target
			{
			    half2 uv = i.uv;
				float4 color = tex2D(_MainTex, uv);

                half threshold = 0.0001;

                fixed4 worldPosition = GetWorldPosition(uv);
                fixed3 direction = (_WorldTracerPosition - worldPosition).xyz;
                direction.y = 0;

                //return (direction / _MaxFieldDistance).xyzz;

                //return ((direction / _MaxFieldDistance).xyzy + 1) / 2;

                fixed distanceToLight = length(direction);
                direction /= distanceToLight;

                fixed distanceFieldValue = 0;//SampleDistanceField(worldPosition);

                //return lerp(color, sqrt(SampleDistanceField(worldPosition) / _MaxFieldDistance), 0.5);

                fixed itr = 0;
                for (; itr < 30; ++itr)
                {
                    if (distanceToLight <= 0) return color;

                    worldPosition.xyz += direction * distanceFieldValue;

                    distanceFieldValue = SampleDistanceField(worldPosition);

                    if (distanceFieldValue < threshold) break;

                    distanceToLight -= distanceFieldValue;
                }

                return 0;

				return lerp(half4(0, 0, 0, 0), color, distanceFieldValue * _WarFogBrightness);
			}
			ENDCG
		}
	}
}
