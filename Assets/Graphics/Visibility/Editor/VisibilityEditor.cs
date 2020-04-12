using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VisibilityEditor : EditorWindow
{
    [MenuItem("Tools/Aether Visibility/Show In Editor")]
    static void ResetVisibilityInEditor()
    {
        Shader.SetGlobalTexture("_WorldVisibilityTexture", Texture2D.whiteTexture);
    }

    [MenuItem("Tools/Aether Visibility/Hide In Editor")]
    static void ClearVisibilityInEditor()
    {
        Shader.SetGlobalTexture("_WorldVisibilityTexture", Texture2D.blackTexture);
    }
}
