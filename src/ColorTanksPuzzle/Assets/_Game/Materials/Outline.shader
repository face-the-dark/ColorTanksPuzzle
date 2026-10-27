Shader "Custom/Outline"
{
    Properties
    {
        _OutlineColor ("Color", Color) = (0,0,0,1)
        _OutlineWidth ("Width", Float) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" }
        Pass
        {
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float _OutlineWidth;

            struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; };
            struct v2f { float4 pos : SV_POSITION; };

            v2f vert(appdata v)
{
    v2f o;
    float3 dir = normalize(v.vertex.xyz);   // направление от центра, а не нормаль
    float3 p = v.vertex.xyz + dir * _OutlineWidth;
    o.pos = UnityObjectToClipPos(float4(p, 1));
    return o;
}

            fixed4 frag(v2f i) : SV_Target { return _OutlineColor; }
            ENDCG
        }
    }
}