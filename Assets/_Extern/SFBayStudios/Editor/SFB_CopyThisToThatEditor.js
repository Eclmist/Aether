#pragma strict

// This was basically taken from one of the "Learn" videos, which you can find here:
// https://unity3d.com/learn/tutorials/modules/intermediate/editor/adding-buttons-to-inspector

// Have you used "Learn" yet?  If not, you're either already good at Unity stuff, or you may
// be missing out!  Check it out!!

@CustomEditor (SFB_CopyThisToThat)							// Will attach this to the inspector window of v1_CopyThisToThat
public class SFB_CopyThisToThatEditor extends Editor
{

	var showNewBox			: boolean			= false;
	var newBoxTitle			: String			= "Add New Source & Target Materials";
	var showOptionsBox		: boolean			= false;
	var showOptionsTitle	: String			= "Options";
	var showCopyBox			: boolean			= true;
	var copyBoxTitle		: String			= "Copy This To That Details";

	var sourceMaterialOptions	: String[];
	var sourceMaterialIndex		: int				= 0;

	var newSourceMaterial		: ProceduralMaterial;
	var newTargetMaterial		: ProceduralMaterial;

	var includeMain				: boolean			= true;
	var includeEnv				: boolean			= true;
	var parseEnv				: boolean			= false;

	function OnInspectorGUI()
    {
    	var myScript : SFB_CopyThisToThat = target;			// A reference to the script
       

		EditorGUILayout.HelpBox("1. Set options.\n2. Add your Source material(s) below.  First will be default.\n2. Add your Target material(s) below.  List will update, do not set source groups until all targets are added.\n4. Click \"Copy All\" to copy entire list.\n* Click \"Reset List\" to reset all settings and restore deleted actions.\n* For more information on how to use this script, visit www.InfinityPBR.com/CopyThisToThat/", MessageType.Info);

		showOptionsBox = EditorGUILayout.Foldout(showOptionsBox, showOptionsTitle);
		if (showOptionsBox)
		{
			includeMain				= EditorGUILayout.Toggle ("Include \"Main\"", includeMain);
			includeEnv				= EditorGUILayout.Toggle ("Include \"Env\"", includeEnv);
			parseEnv				= EditorGUILayout.Toggle ("Parse \"Env\"", parseEnv);
			myScript.Options(includeMain,includeEnv,parseEnv);

			GUI.color 			= Color.red;
			if (GUILayout.Button ("Rebuild Actions List", GUILayout.Height(20)))
			{
				myScript.RebuildActions();
			}
			GUI.color 			= Color.white;
		}

		var fullBoxTitle		= newBoxTitle;
		if (myScript.GetCount("sources") == 0)
			fullBoxTitle		= fullBoxTitle + " (Add At Least One Source Material)";
		else
		{
			fullBoxTitle		= fullBoxTitle + " (" + myScript.GetCount("sources") + " Source Material";
			if (myScript.GetCount("sources") != 1)
				fullBoxTitle	= fullBoxTitle + "s";
			fullBoxTitle		= fullBoxTitle + " & " + myScript.GetCount("targets") + " Target Material";
			if (myScript.GetCount("targets") != 1)
				fullBoxTitle	= fullBoxTitle + "s";
			fullBoxTitle		= fullBoxTitle + ")";
		} 
		showNewBox = EditorGUILayout.Foldout(showNewBox, fullBoxTitle);
		if (showNewBox)
		{
			//sourceMaterialIndex = EditorGUILayout.Popup(sourceMaterialIndex, myScript.GetSourceMaterialNames());
			EditorGUILayout.BeginHorizontal ();
				newSourceMaterial 	= EditorGUILayout.ObjectField(newSourceMaterial, ProceduralMaterial, true);
				if (GUILayout.Button ("Add Source"))
				{
					myScript.AddSource(newSourceMaterial);
				}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
				newTargetMaterial 	= EditorGUILayout.ObjectField(newTargetMaterial, ProceduralMaterial, true);
				if (myScript.GetCount("sources") == 0)
				{
					GUI.color 			= Color(0.5,0.5,0.5,1);
					GUILayout.Button ("Add Target");
					GUI.color 			= Color.white;
				}
				else
				{
					if (GUILayout.Button ("Add Target"))
						myScript.AddTarget(newTargetMaterial);
				}
			EditorGUILayout.EndHorizontal ();
		}

		var fullCopyBoxTitle	= copyBoxTitle;
		fullCopyBoxTitle		= fullCopyBoxTitle + " (" + myScript.GetCount("actions") + " Action";
		if (myScript.GetCount("actions") != 1)
			fullCopyBoxTitle	= fullCopyBoxTitle + "s";
		fullCopyBoxTitle		= fullCopyBoxTitle + ")";
		showCopyBox = EditorGUILayout.Foldout(showCopyBox, fullCopyBoxTitle);
		if (showCopyBox)
		{
			var actionsCount		= myScript.GetCount("actions");
			var targetsCount		= myScript.GetCount("targets");
			//var guiCenter			: GUIStyle;
			//guiCenter.alignment		= TextAnchor.UpperCenter;

			for (var t : int; t < targetsCount; t++){
				GUILayout.Label ("--------------------" + myScript.targets[t].name + "--------------------");
				EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.BeginVertical ();
						for (var sm : int; sm < myScript.GetCount("actions"); sm++){
							if (myScript.actions[sm].targetMaterial == t)
							{
								var sourceMaterialID	= myScript.actions[sm].sourceMaterial;
								if (myScript.GetCount("sources") > 1)
								{
									var newID	= EditorGUILayout.IntPopup(myScript.actions[sm].sourceMaterial, myScript.SourceNames(), myScript.SourceIDs());
									if (myScript.actions[sm].sourceMaterial	!= newID)
									{
										myScript.actions[sm].sourceGroup	= myScript.FirstNonGlobalGroup(newID, myScript.actions[sm].targetMaterial, myScript.actions[sm].targetGroup);
									}
									myScript.actions[sm].sourceMaterial	= newID;
									sourceMaterialID					= myScript.actions[sm].sourceMaterial;
								}
								else
									GUILayout.Label (myScript.sources[sourceMaterialID].name);
							}
						}
					EditorGUILayout.EndVertical ();

					EditorGUILayout.BeginVertical ();
						for (var sg : int; sg < myScript.GetCount("actions"); sg++){
							if (myScript.actions[sg].targetMaterial == t)
							{
								var sourceID		: int		= myScript.actions[sg].sourceMaterial;
								var sourceGroupID	: int		= myScript.actions[sg].sourceGroup;
								var sourceGroupName	: String	= myScript.sources[sourceMaterialID].groups[sourceGroupID].name;
								if (sourceGroupName.Substring(0, 3) != "Env" && sourceGroupName.Substring(0, 4) != "Main" && sourceGroupName.Substring(0, 4) != "Rune")
									myScript.actions[sg].sourceGroup = EditorGUILayout.IntPopup(myScript.actions[sg].sourceGroup, myScript.SourceGroupNames(sourceID), myScript.SourceGroupIDs(sourceID));
								else
									GUILayout.Label (sourceGroupName);
							}
						}
					EditorGUILayout.EndVertical ();

					EditorGUILayout.BeginVertical ();
						for (var a : int; a < myScript.GetCount("actions"); a++){
							if (myScript.actions[a].targetMaterial == t)
								GUILayout.Label ("-->");
						}
					EditorGUILayout.EndVertical ();

					EditorGUILayout.BeginVertical ();
						for (var tg : int; tg < myScript.GetCount("actions"); tg++){
							if (myScript.actions[tg].targetMaterial == t)
							{
								var targetMaterialID_G	= myScript.actions[tg].targetMaterial;
								var targetGroupID		= myScript.actions[tg].targetGroup;
								GUILayout.Label (myScript.targets[targetMaterialID_G].groups[targetGroupID].name);
							}
						}
					EditorGUILayout.EndVertical ();

					EditorGUILayout.BeginVertical ();
						GUI.color 			= Color.green;
						for (var bc : int; bc < myScript.GetCount("actions"); bc++){
							if (myScript.actions[bc].targetMaterial == t)
							{
								if (GUILayout.Button ("Copy", GUILayout.Height(15)))
								{
									myScript.SingleAction(bc);
								}
							}
						}
						GUI.color 			= Color.white;
					EditorGUILayout.EndVertical ();

					EditorGUILayout.BeginVertical ();
						GUI.color 			= Color.red;
						for (var bd : int; bd < myScript.GetCount("actions"); bd++){
							if (myScript.actions[bd].targetMaterial == t)
							{
								if (GUILayout.Button ("Delete", GUILayout.Height(15)))
								{
									myScript.DeleteAction(bd);
								}
							}
						}
						GUI.color 			= Color.white;
					EditorGUILayout.EndVertical ();
				EditorGUILayout.EndHorizontal ();
			}		
		}
		if (myScript.GetCount("actions") > 0)
			GUI.color 			= Color.green;
		else
			GUI.color 			= Color(0.5,0.5,0.5,1);
		if (GUILayout.Button ("Copy All Actions", GUILayout.Height(40)))
		{
			myScript.AllActions();
		}
		GUI.color 			= Color.white;
		DrawDefaultInspector();								// Draw the normal inspector first
	}
}

