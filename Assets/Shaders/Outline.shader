Shader "Custom/OutlineOnly" {
    Properties {
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = .02
    }
    SubShader {
        Tags { "Queue"="Transparent" }

        Pass {
            Cull Front
            ZWrite Off // Don't hide the object behind the outline
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;

            v2f vert(appdata v) {
                v2f o;
                // Extrude vertices along the normal in object space
                v.vertex.xyz += v.normal * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 _OutlineColor;

            fixed4 frag(v2f i) : SV_Target {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    FallBack "VertexLit"
}
