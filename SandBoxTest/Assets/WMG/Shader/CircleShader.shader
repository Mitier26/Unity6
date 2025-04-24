Shader "Custom/WorldCircleEffect"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _AreaColor ("Area Color", Color) = (1,1,1,1)
        _Center ("Center (world)", Vector) = (0,0,0,0)
        _Radius ("Radius", Range(0, 500)) = 75
        _Border ("Border", Range(0, 100)) = 12
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        float4 _AreaColor;
        float4 _Center;
        float _Radius;
        float _Border;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float dist = distance(_Center.xyz, IN.worldPos);
            float edgeMin = _Radius;
            float edgeMax = _Radius + _Border;

            float t = saturate((dist - edgeMin) / (edgeMax - edgeMin));

            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = lerp(_AreaColor.rgb, tex.rgb, t);
            o.Alpha = 1 - t; // 내부가 불투명, 외부로 갈수록 투명
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
