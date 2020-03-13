#pragma strict

@CustomEditor (SFB_BlendShapes)							// Will attach this to the inspector window of v1_CopyThisToThat
public class SFB_BlendShapesEditor extends Editor
{
    function OnInspectorGUI()
    {
    	var myScript : SFB_BlendShapes	= target;		// A reference to the script
        DrawDefaultInspector();								// Draw the normal inspector first

		EditorGUILayout.HelpBox("SCRIPTING\nCall SetSelectedShape(shapeIDNumber : int) first\nCall SetValueUI(newValue : float) to set value\nID # is listed next to Blend Shape name", MessageType.Info);
		if (myScript.blendShapeObjects.Count > 0)
		{
			for (var o : int; o < myScript.blendShapeObjects.Count; o++){
				if (myScript.blendShapeObjects[o].blendShapes.Count != 0)
				{
					var blendShapeObject	: SFB_BlendShapeObject	= myScript.blendShapeObjects[o];
					var blendShapeMesh		: Mesh					= blendShapeObject.object;
					var hasShownGroup		: boolean				= false;
					for (var i : int; i < blendShapeObject.blendShapes.Count; i++){
						var blendShapeData	: SFB_BlendShape		= blendShapeObject.blendShapes[i];
						if (blendShapeData.isVisible)
						{
							if (!hasShownGroup)
							{
								hasShownGroup = true;
								EditorGUILayout.BeginHorizontal ();
									GUILayout.Label ("--" + blendShapeMesh.name + "--");
								EditorGUILayout.EndHorizontal ();
							}
						 	EditorGUILayout.BeginHorizontal ();
						 		var displayName	= blendShapeData.name;
						 		if (blendShapeData.isPlus)
						 		{
						 			displayName	= displayName.Replace("Plus", "");
						 			displayName	= displayName.Replace("plus", "");
						 		}
				    			GUILayout.Label (displayName + " (" + blendShapeData.inspectorID + ")", GUILayout.Width(200));
				    			if (blendShapeData.isPlus)
				    			{
				    				var minusShapeObject	: int			= myScript.GetMinusShapeObject(blendShapeData.name);
				    				var minusShapeID		: int			= myScript.GetMinusShapeID(blendShapeData.name);
				    				var minusShapeData	: SFB_BlendShape	= myScript.blendShapeObjects[minusShapeObject].blendShapes[minusShapeID];
				    				var newValue 							= EditorGUILayout.Slider(blendShapeData.sliderValue,-100, 100);
				    				//Debug.Log ("Names: " + blendShapeData.name + " | " + minusShapeData.name);
									blendShapeData.sliderValue	= newValue;
									minusShapeData.sliderValue	= -newValue;
				    			}
				    			else
				    				blendShapeData.sliderValue = EditorGUILayout.Slider(blendShapeData.sliderValue,1, 100);
							EditorGUILayout.EndHorizontal ();

							if (blendShapeData.sliderValue != blendShapeData.value)
							{
								myScript.SetValue(o, i, blendShapeData.id, blendShapeData.sliderValue);
								if (blendShapeData.isPlus)
									myScript.SetValue(minusShapeObject, minusShapeID, minusShapeData.id, minusShapeData.sliderValue);
							}
						}
					}
				}
			}
		}

        if(GUILayout.Button("Reload Blend Shapes"))				// If we click the button with the name "Copy Settings"
        {
            myScript.ReloadBlendShapes();						// Call the function on v1_CopyThisToThat
        }
    }

}

