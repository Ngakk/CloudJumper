// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-7691-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31980,y:32689,ptovrint:False,ptlb:SkyBlue,ptin:_SkyBlue,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.52,c2:0.8,c3:0.92,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:4662,x:31495,y:32865,varname:node_4662,prsc:2;n:type:ShaderForge.SFN_Color,id:7996,x:32027,y:33066,ptovrint:False,ptlb:DeepSpaceBlue,ptin:_DeepSpaceBlue,varname:node_7996,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.03,c2:0.015,c3:0.415,c4:1;n:type:ShaderForge.SFN_Lerp,id:8311,x:32279,y:32896,varname:node_8311,prsc:2|A-7241-RGB,B-7996-RGB,T-585-OUT;n:type:ShaderForge.SFN_Divide,id:2203,x:31790,y:32894,varname:node_2203,prsc:2|A-4662-Y,B-6981-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6981,x:31528,y:33091,ptovrint:False,ptlb:Expandong,ptin:_Expandong,varname:node_6981,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:100;n:type:ShaderForge.SFN_Clamp01,id:585,x:31980,y:32894,varname:node_585,prsc:2|IN-2203-OUT;n:type:ShaderForge.SFN_Color,id:2153,x:32216,y:32528,ptovrint:False,ptlb:ForeGroundColor,ptin:_ForeGroundColor,varname:node_2153,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:7691,x:32505,y:32761,varname:node_7691,prsc:2|A-2153-RGB,B-8311-OUT,T-5584-OUT;n:type:ShaderForge.SFN_Slider,id:5584,x:32166,y:32745,ptovrint:False,ptlb:Transparency,ptin:_Transparency,varname:node_5584,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4,max:1;proporder:7241-7996-6981-2153-5584;pass:END;sub:END;*/

Shader "Shader Forge/SkySpace" {
    Properties {
        _SkyBlue ("SkyBlue", Color) = (0.52,0.8,0.92,1)
        _DeepSpaceBlue ("DeepSpaceBlue", Color) = (0.03,0.015,0.415,1)
        _Expandong ("Expandong", Float ) = 100
        _ForeGroundColor ("ForeGroundColor", Color) = (0.5,0.5,0.5,1)
        _Transparency ("Transparency", Range(0, 1)) = 0.4
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _SkyBlue;
            uniform float4 _DeepSpaceBlue;
            uniform float _Expandong;
            uniform float4 _ForeGroundColor;
            uniform float _Transparency;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float3 emissive = lerp(_ForeGroundColor.rgb,lerp(_SkyBlue.rgb,_DeepSpaceBlue.rgb,saturate((i.posWorld.g/_Expandong))),_Transparency);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
