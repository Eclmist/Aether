using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

[assembly: AlwaysLinkAssembly]
namespace VisualDesignCafe.Rendering.Nature
{
    public static class NatureRendererSrp
    {
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            RenderPipelineManager.beginFrameRendering -= OnBeginFrameRendering;
            RenderPipelineManager.beginFrameRendering += OnBeginFrameRendering;
        }

        private static void OnBeginFrameRendering( ScriptableRenderContext context, Camera[] cameras )
        {
            foreach( var renderer in NatureRenderer.Renderers )
                foreach( var camera in cameras )
                    if( renderer.isActiveAndEnabled )
                        renderer.Render( camera );
        }
    }
}