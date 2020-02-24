using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VisibilityEditor : EditorWindow
{
    [MenuItem("Tools/Aether Visibility/Reset In Editor")]
    static void ResetVisibilityInEditor()
    {
        Shader.SetGlobalTexture("_WorldVisibilityTexture", Resources.Load("default3D") as Texture3D);
    }
}
