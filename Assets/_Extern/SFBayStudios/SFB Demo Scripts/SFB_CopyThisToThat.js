#pragma strict

import System.Collections.Generic;

var sources			: List.<Source> 	= new List.<Source>();			// Source Materials
var targets			: List.<Target> 	= new List.<Target>();			// Target Materials
var actions			: List.<CopyAction> = new List.<CopyAction>();		// List of all actions (one for each group in target materials)

private var defaultSourceMaterial	: ProceduralMaterial;
private var defaultSourceGroup		: String;

private var includeMain				: boolean			= true;
private var includeEnv				: boolean			= true;
private var parseEnv				: boolean			= false;


class Source {
	var name		: String;													// Human readable name
	var material	: ProceduralMaterial;										// The Procedural Material we'll copy From
	var groups		: List.<Group> = new List.<Group>();						// The groups to choose from
}

class Target {
	var name		: String;													// Human readable name
	var material	: ProceduralMaterial;										// The Mateiral we are copying to
	var groups		: List.<Group> = new List.<Group>();						// Gropus to choose from
}

class CopyAction {
	var sourceMaterial		: int;												// ID # Source Material
	var sourceGroup			: int;												// ID # of the group
	var targetMaterial		: int;												// ID # Target Material
	var targetGroup			: int;												// ID # of the group
}

class Group {
	var name		: String;
	var values		: List.<Value> = new List.<Value>();						// Values to choose from									
}

class Value {
	var name		: String;	
	var id			: int;									
}

function GetCount(type : String) : int {
	if (type	== "sources")
		return sources.Count;
	if (type	== "targets")
		return targets.Count;
	if (type	== "actions")
		return actions.Count;
}

function Options(newIncludeMain : boolean, newIncludeEnv : boolean, newParseEnv : boolean){
	includeMain	= newIncludeMain;
	includeEnv	= newIncludeEnv;
	parseEnv	= newParseEnv;
}

function AddSource(newSource : ProceduralMaterial){
	if (!SourceNameExists(newSource.name))
	{
		var currentCount				= sources.Count;		// Current count of List
		var newSourceObj	: Source;							// Create new Source
		sources.Add(newSourceObj);								// Add a new Source
		sources[currentCount] = new Source();					// set it to be a new action
		sources[currentCount].name		= newSource.name;
		sources[currentCount].material	= newSource;

		var groups = newSource.GetProceduralPropertyDescriptions();
		for (var g : int; g < groups.Length; g++){
			if (groups[g].name.Contains("_") && groups[g].name.Substring(0, 5) != "input")
			{
				var groupNameParts	: String[]	= groups[g].name.Split("_"[0]);

				for (var n : int; n < groupNameParts.Length - 1; n++){
					var newName	: String;

					for (var nn : int; nn <= n; nn++){
						if (nn != 0)
							newName	= newName + "_";
						newName	= newName + "" + groupNameParts[nn];
					}
					
					if (!GroupNameExists(true, currentCount, newName))
					{
						var currentGroupCount			= sources[currentCount].groups.Count;
						var newGroupObj			: Group;
						sources[currentCount].groups.Add(newGroupObj);
						sources[currentCount].groups[currentGroupCount] 			= new Group();

						sources[currentCount].groups[currentGroupCount].name		= newName;

						var allValues	= sources[currentCount].material.GetProceduralPropertyDescriptions ();
						//Debug.Log ("Searching " + allValues.Length + " Values");
						for (var v : int; v < allValues.Length; v++){
							if (allValues[v].name.Length >= sources[currentCount].groups[currentGroupCount].name.Length)
							{
								//print ("Trying " + allValues[v].name + " to match with " + sources[currentCount].groups[currentGroupCount].name);
								if (allValues[v].name.Substring(0, sources[currentCount].groups[currentGroupCount].name.Length) == sources[currentCount].groups[currentGroupCount].name)
								{
									var currentValueCount			= sources[currentCount].groups[currentGroupCount].values.Count;
									var newValueObj			: Value;
									sources[currentCount].groups[currentGroupCount].values.Add(newValueObj);
									sources[currentCount].groups[currentGroupCount].values[currentValueCount] 		= new Value();

									sources[currentCount].groups[currentGroupCount].values[currentValueCount].name 	= allValues[v].name;
									sources[currentCount].groups[currentGroupCount].values[currentValueCount].id 	= v;
								}
							}
						}
					}
				}
			}
		}
	}
}

function SourceNameExists(sourceName : String) : boolean {
	var totalSources : int		= sources.Count;
	for (var i : int; i < totalSources; i++){
		if (sources[i].name == sourceName)
			return true;
	}
	return false;
}

function TargetNameExists(targetName : String) : boolean {
	var totalTargets : int		= targets.Count;
	for (var i : int; i < totalTargets; i++){
		if (targets[i].name == targetName)
			return true;
	}
	return false;
}

function GroupNameExists(isSource : boolean, listID : int, groupName) : boolean {
	var totalGroups : int		= 0;
	if (isSource)
		totalGroups				= sources[listID].groups.Count;
	else
		totalGroups				= targets[listID].groups.Count;
	for (var g : int; g < totalGroups; g++){
		if (isSource)
		{
			if (sources[listID].groups[g].name == groupName)
				return true;
		}
		else
		{
			if (targets[listID].groups[g].name == groupName)
				return true;
		}
	}
	return false;
}

function AddTarget(newTarget : ProceduralMaterial){
	if (!TargetNameExists(newTarget.name))
	{
		var currentCount				= targets.Count;		// Current count of List
		var newTargetObj	: Target;							// Create new Target
		targets.Add(newTargetObj);								// Add a new Target
		targets[currentCount] = new Target();					// set it to be a new action
		targets[currentCount].name		= newTarget.name;
		targets[currentCount].material	= newTarget;

		var groups = newTarget.GetProceduralPropertyDescriptions();
		for (var g : int; g < groups.Length; g++){
			if (groups[g].name.Contains("_") && groups[g].name.Substring(0, 5) != "input")
			{
				var groupNameParts	: String[]	= groups[g].name.Split("_"[0]);

				for (var n : int; n < groupNameParts.Length - 1; n++){
					var newName	: String;

					for (var nn : int; nn <= n; nn++){
						if (nn != 0)
							newName	= newName + "_";
						newName	= newName + "" + groupNameParts[nn];
					}
					
					if (!GroupNameExists(false, currentCount, newName))
					{
						var currentGroupCount			= targets[currentCount].groups.Count;
						var newGroupObj			: Group;
						targets[currentCount].groups.Add(newGroupObj);
						targets[currentCount].groups[currentGroupCount] 			= new Group();

						targets[currentCount].groups[currentGroupCount].name		= newName;
						var allValues	= targets[currentCount].material.GetProceduralPropertyDescriptions ();
						//Debug.Log ("Searching " + allValues.Length + " Values");
						for (var v : int; v < allValues.Length; v++){
							if (allValues[v].name.Length >= targets[currentCount].groups[currentGroupCount].name.Length)
							{
								if (allValues[v].name.Substring(0, targets[currentCount].groups[currentGroupCount].name.Length) == targets[currentCount].groups[currentGroupCount].name)
								{
									var currentValueCount			= targets[currentCount].groups[currentGroupCount].values.Count;
									var newValueObj			: Value;
									targets[currentCount].groups[currentGroupCount].values.Add(newValueObj);
									targets[currentCount].groups[currentGroupCount].values[currentValueCount] 		= new Value();

									targets[currentCount].groups[currentGroupCount].values[currentValueCount].name 	= allValues[v].name;
									targets[currentCount].groups[currentGroupCount].values[currentValueCount].id 	= v;
								}
							}
						}
					}
				}
			}
		}
		RebuildActions();
	}
}

function RebuildActions(){
	actions.Clear();
	for (var t : int; t < targets.Count; t++){
		for (var g : int; g < targets[t].groups.Count; g++){
			if (targets[t].groups[g].name == "Rune")
			{

			}
			else if (targets[t].groups[g].name == "Main" && !includeMain)
			{

			}
			else if (targets[t].groups[g].name == "Env" && !includeEnv)
			{

			}
			else if (targets[t].groups[g].name == "Env" && includeEnv && parseEnv)
			{

			}
			else if (!ShowParsedEnv(targets[t].groups[g].name))
			{

			}
			else 
			{
				var currentCount						= actions.Count;		// Current count of List
				var newActionObj	: CopyAction;								// Create new Action
				actions.Add(newActionObj);										// Add a new Action
				actions[currentCount] 					= new CopyAction();		// set it to be a new action

				if (targets[t].groups[g].name == "Main" || targets[t].groups[g].name == "Rune" || targets[t].groups[g].name.Substring(0,3) == "Env")
				{
					actions[currentCount].sourceMaterial	= 0;
					actions[currentCount].sourceGroup		= GrabSourceGroupID(0, targets[t].groups[g].name);
					actions[currentCount].targetMaterial	= t;
					actions[currentCount].targetGroup		= GrabTargetGroupID(t, targets[t].groups[g].name);
				}
				else
				{
					actions[currentCount].sourceMaterial	= 0;
					actions[currentCount].sourceGroup		= FirstNonGlobalGroup(t, g, 0);
					actions[currentCount].targetMaterial	= t;
					actions[currentCount].targetGroup		= g;
				}
			}
		}
	}
}

function FirstNonGlobalGroup(targetID : int, groupID : int, sourceID : int) : int {
	var totalGroups				= sources[sourceID].groups.Count;
	for (var m : int; m < totalGroups; m++){
		if (sources[sourceID].groups[m].name == targets[targetID].groups[groupID].name)
			return m;
	}
	for (var g : int; g < totalGroups; g++){
		if (sources[sourceID].groups[g].name != "Main" && sources[sourceID].groups[g].name != "Rune" && sources[sourceID].groups[g].name.Substring(0,3) != "Env")
			return g;
	}
	return 0;
}

function GrabSourceGroupID(source : int, groupName : String) : int {
	var totalGroups				= sources[source].groups.Count;
	for (var g : int; g < totalGroups; g++){
		if (sources[source].groups[g].name	== groupName)
			return g;
	}
	return 0;
}

function GrabTargetGroupID(target : int, groupName : String) : int {
	var totalGroups				= targets[target].groups.Count;
	for (var g : int; g < totalGroups; g++){
		if (targets[target].groups[g].name	== groupName)
			return g;
	}
	return 0;
}

function ShowParsedEnv(name : String) : boolean {
	var firstThree	= name.Substring(0, 3);
	if (firstThree != "Env")
		return true;
	else
	{
		if (!includeEnv)
			return false;
		if (name.Contains("_") && !parseEnv)
			return false;
	}
	return true;
}

function SingleAction(actionID : int){
	CopyThisToThat(actionID);
	StartRebuildingTexture(actions[actionID].targetMaterial);
}

function DeleteAction(actionID : int){
	Undo.RecordObject(this, "Deleted Action");
	actions.RemoveAt(actionID); 
}

function CopyThisToThat(actionID : int){
	

	var sourceMaterialID	= actions[actionID].sourceMaterial;
	var sourceGroupID		= actions[actionID].sourceGroup;
	var targetMaterialID	= actions[actionID].targetMaterial;
	var targetGroupID		= actions[actionID].targetGroup;
	var sourceMaterial		= sources[sourceMaterialID].material;
	var targetMaterial		= targets[targetMaterialID].material;
	var sourceName			= sources[sourceMaterialID].groups[sourceGroupID].name;		// Group name
	var targetName			= targets[targetMaterialID].groups[targetGroupID].name;		// Group name

	Debug.Log ("Copying " + sources[sourceMaterialID].name + " " + sourceName + " to " + targets[targetMaterialID].name + " " + targetName);

	var totalCopied	: int	= 0;

	// For each Source Value...
	for (var sv : int; sv < sources[sourceMaterialID].groups[sourceGroupID].values.Count; sv++){
		var allSourceValues		= sources[sourceMaterialID].material.GetProceduralPropertyDescriptions ();
		var sourceNameParts		= sources[sourceMaterialID].groups[sourceGroupID].values[sv].name.Split("_"[0]);
		var sourceSuffix		= sourceNameParts[sourceNameParts.Length - 1];
		var sourceValueID		= sources[sourceMaterialID].groups[sourceGroupID].values[sv].id;

		// For each Target Value, see if the suffix matches
		for (var v : int; v < targets[targetMaterialID].groups[targetGroupID].values.Count; v++){
			var allTargetValues				= targets[targetMaterialID].material.GetProceduralPropertyDescriptions ();
			var targetNameParts				= targets[targetMaterialID].groups[targetGroupID].values[v].name.Split("_"[0]);
			var targetSuffix				= targetNameParts[targetNameParts.Length - 1];
			var targetValueID				= targets[targetMaterialID].groups[targetGroupID].values[v].id;
			var canCopy			: boolean	= false;


			if (targetNameParts[0] != "Env" && targetSuffix == sourceSuffix)
				canCopy	= true;
			else if (targets[targetMaterialID].groups[targetGroupID].values[v].name == sources[sourceMaterialID].groups[sourceGroupID].values[sv].name)
				canCopy = true;

			if (canCopy)
			{
				Debug.Log ("Copying Source " + sources[sourceMaterialID].groups[sourceGroupID].values[sv].name + " to " + targets[targetMaterialID].groups[targetGroupID].values[v].name);
				// Find out what type the property is, then copy it to the target
				if (allTargetValues[targetValueID].type == ProceduralPropertyType.Boolean)
					targetMaterial.SetProceduralBoolean(allTargetValues[targetValueID].name, sourceMaterial.GetProceduralBoolean (allSourceValues[sourceValueID].name));
				else if (allTargetValues[targetValueID].type == ProceduralPropertyType.Float)
					targetMaterial.SetProceduralFloat(allTargetValues[targetValueID].name, sourceMaterial.GetProceduralFloat (allSourceValues[sourceValueID].name));
				else if (allTargetValues[targetValueID].type == ProceduralPropertyType.Vector2 || allTargetValues[targetValueID].type == ProceduralPropertyType.Vector3 || allTargetValues[targetValueID].type == ProceduralPropertyType.Vector4)
					targetMaterial.SetProceduralVector(allTargetValues[targetValueID].name, sourceMaterial.GetProceduralVector (allSourceValues[sourceValueID].name));
				else if (allTargetValues[targetValueID].type == ProceduralPropertyType.Color3 || allTargetValues[targetValueID].type == ProceduralPropertyType.Color4)
					targetMaterial.SetProceduralColor(allTargetValues[targetValueID].name, sourceMaterial.GetProceduralColor (allSourceValues[sourceValueID].name));
				else if (allTargetValues[targetValueID].type == ProceduralPropertyType.Enum)
					targetMaterial.SetProceduralEnum(allTargetValues[targetValueID].name, sourceMaterial.GetProceduralEnum (allSourceValues[sourceValueID].name));
				else if (allTargetValues[targetValueID].type == ProceduralPropertyType.Texture)
					targetMaterial.SetProceduralTexture(allTargetValues[targetValueID].name, sourceMaterial.GetProceduralTexture (allSourceValues[sourceValueID].name));
				totalCopied++;			// Add one to the count
				//Debug.Log ("Copied " + allSourceValues[sourceValueID].name + " (" + allSourceValues[sourceValueID].type + ") to " + allTargetValues[targetValueID].name + " (" + allTargetValues[targetValueID].type + ")");
			}
		}
	}
	Debug.Log ("Copied " + totalCopied + " Fields");								
	Debug.Log ("-------------------------");
}


function AllActions(){
	for (var a : int; a < actions.Count; a++){
		CopyThisToThat(a);
	}
	RebuildAllTextures();
}

function SourceGroupIDs(sourceID : int) : int[] {
	var allIDs	: int[];
	allIDs 		= new int[sources[sourceID].groups.Count];
	for (var i : int; i < sources[sourceID].groups.Count; i++)
	{
		if (sources[sourceID].groups[i].name != "Main" && sources[sourceID].groups[i].name != "Rune" && sources[sourceID].groups[i].name.Substring(0,3) != "Env")
			allIDs[i] = i;
	}
	return allIDs;
}

function SourceGroupNames(sourceID : int) : String[] {
	var allNames	: String[];
	allNames 		= new String[sources[sourceID].groups.Count];
	for (var i : int; i < sources[sourceID].groups.Count; i++)
	{
		if (sources[sourceID].groups[i].name != "Main" && sources[sourceID].groups[i].name != "Rune" && sources[sourceID].groups[i].name.Substring(0,3) != "Env")
			allNames[i] = sources[sourceID].groups[i].name;
	}
	return allNames;
}

function SourceIDs() : int[] {
	var allIDs	: int[];
	allIDs 		= new int[sources.Count];
	for (var i : int; i < sources.Count; i++)
	{
		allIDs[i] = i;
	}
	return allIDs;
}

function SourceNames() : String[] {
	var allNames	: String[];
	allNames 		= new String[sources.Count];
	for (var i : int; i < sources.Count; i++)
	{
		allNames[i] = sources[i].name;
	}
	return allNames;
}

function RebuildAllTextures(){
	for (var t : int; t < targets.Count; t++){
		StartRebuildingTexture(t);
	}
}

function StartRebuildingTexture(targetID : int){
	targets[targetID].material.StopRebuilds();
	if (!targets[targetID].material.isProcessing)
	{
		var substanceImporter : SubstanceImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(targets[targetID].material));
		substanceImporter.SaveAndReimport();
		targets[targetID].material.RebuildTextures();
	}
}