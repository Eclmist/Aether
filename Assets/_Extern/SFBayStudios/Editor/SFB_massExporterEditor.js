#pragma strict

// This was basically taken from one of the "Learn" videos, which you can find here:
// https://unity3d.com/learn/tutorials/modules/intermediate/editor/adding-buttons-to-inspector

// Have you used "Learn" yet?  If not, you're either alrady good at Unity stuff, or you may
// be missing out!  Check it out!!

@CustomEditor (SFB_massExporter)							// Will attach this to the inspector window of massExporter
public class SFB_massExporterEditor extends Editor
{
    function OnInspectorGUI()
    {
    	var myScript : SFB_massExporter = target;				// A reference to the script
        DrawDefaultInspector();									// Draw the normal inspector first

        if(GUILayout.Button("Export Materials"))				// If we click the button with the name "Copy Settings"
        {
            myScript.ExportSubstances();						// Call the function on v1_CopyThisToThat
        }
    }

    function Test(){
    	Debug.Log("Test");

    }
}

