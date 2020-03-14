using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VisibilityEditor : EditorWindow
{
    [MenuItem("Tools/Aether Visibility/Reset In Editor")]
    static void ResetVisibilityInEditor()
    {
        Shader.SetGlobalTexture("_WorldVisibilityTexture", Texture2D.whiteTexture);
    }
}
