#pragma strict

import System.Collections.Generic;

var blendShapeObjects	: List.<SFB_BlendShapeObject> = new List.<SFB_BlendShapeObject>();  // declaration
var inspectorObjects	: List.<SFB_InspectorObject> = new List.<SFB_InspectorObject>();	

private var selectedShape	: int;						// Used with the UI, set this when user clicks the slider

public class SFB_InspectorObject {
	var objectID			: int;
	var shapeID				: int;
	var blendShapeID		: int;
}

public class SFB_BlendShapeObject {
	var name				: String;
	var object				: Mesh;
	var renderer			: SkinnedMeshRenderer;
	var blendShapes 		: List.<SFB_BlendShape> = new List.<SFB_BlendShape>();
}

public class SFB_BlendShape {
	var name			: String;
	var fullName		: String;
	var isMinus			: boolean	= false;
	var isPlus			: boolean	= false;
	//var oppositeID		: int;
	var inspectorID		: int;
	var id				: int;
	var minValue		: float;
	var maxValue		: float;
	var value			: float;
	var sliderValue		: float;
	var isVisible		: boolean;
	var blendMatches 	: List.<SFB_BlendMatch> = new List.<SFB_BlendMatch>();
}

public class SFB_BlendMatch {
	var name			: String;
	var objectID		: int;
	var shapeID			: int;
}

function SetSelectedShape(newValue : int){
	selectedShape	= newValue;
	print ("selectedShape: " + selectedShape);
}

function SetValueUI(newValue : float){
	newValue *= 100;
	//print ("newValue: " + newValue);
	var objectID		= inspectorObjects[selectedShape].objectID;
	var shapeID			= inspectorObjects[selectedShape].shapeID;
	var blendShapeData	: SFB_BlendShape		= blendShapeObjects[objectID].blendShapes[shapeID];

	if (blendShapeData.isPlus)
	{
		var minusShapeObject	: int			= GetMinusShapeObject(blendShapeData.name);
		var minusShapeID		: int			= GetMinusShapeID(blendShapeData.name);
		var minusShapeData	: SFB_BlendShape	= blendShapeObjects[minusShapeObject].blendShapes[minusShapeID];
		//Debug.Log ("Names: " + blendShapeData.name + " | " + minusShapeData.name);
		blendShapeData.sliderValue	= newValue;
		minusShapeData.sliderValue	= -newValue;
		SetValue(minusShapeObject, minusShapeID, minusShapeData.id, -newValue);
	}
	SetValue(inspectorObjects[selectedShape].objectID, inspectorObjects[selectedShape].shapeID, inspectorObjects[selectedShape].blendShapeID, newValue);
}

function SetValue(objectID : int, shapeID: int, blendShapeID : int, value : float){
	//print ("SetValue(" + objectID + "," + shapeID + "," + blendShapeID + "," + value + ")");
	blendShapeObjects[objectID].renderer.SetBlendShapeWeight(blendShapeID, value);
	if (blendShapeObjects[objectID].blendShapes[shapeID].blendMatches.Count > 0)
	{
		for (var m : int; m < blendShapeObjects[objectID].blendShapes[shapeID].blendMatches.Count; m++){
			var matchObject	= blendShapeObjects[objectID].blendShapes[shapeID].blendMatches[m].objectID;
			var matchShape	= blendShapeObjects[objectID].blendShapes[shapeID].blendMatches[m].shapeID;

			blendShapeObjects[matchObject].renderer.SetBlendShapeWeight(matchShape, value);
		}
	}
}

function AddToInspectorObjects(newObjectID: int, newShapeID : int, newBlendShapeID : int) : int {
	var currentCount	= inspectorObjects.Count;					// Get size of list
	var newObject		: SFB_InspectorObject;						// Create new action
	inspectorObjects.Add(newObject);								// Add a new action
	inspectorObjects[currentCount] 				= new SFB_InspectorObject();					// set it to be a new action
	inspectorObjects[currentCount].objectID		= newObjectID;
	inspectorObjects[currentCount].shapeID		= newShapeID;
	inspectorObjects[currentCount].blendShapeID	= newBlendShapeID;

	return currentCount;
}

function AddObject(newMesh : Mesh, newRenderer : SkinnedMeshRenderer){
	var currentCount	= blendShapeObjects.Count;					// Get size of list
	var newObject	: SFB_BlendShapeObject;								// Create new action
	blendShapeObjects.Add(newObject);									// Add a new action
	blendShapeObjects[currentCount] 			= new SFB_BlendShapeObject();					// set it to be a new action
	blendShapeObjects[currentCount].name		= newMesh.name;
	blendShapeObjects[currentCount].object		= newMesh;
	blendShapeObjects[currentCount].renderer	= newRenderer;

	for (var i : int; i < newMesh.blendShapeCount; i++){
		var newBlendShape	: SFB_BlendShape;								// Create new action
		var currentShapeCount	= blendShapeObjects[currentCount].blendShapes.Count;
		blendShapeObjects[currentCount].blendShapes.Add(newBlendShape);
		blendShapeObjects[currentCount].blendShapes[currentShapeCount] 				= new SFB_BlendShape();
		blendShapeObjects[currentCount].blendShapes[currentShapeCount].id			= i;
		blendShapeObjects[currentCount].blendShapes[currentShapeCount].fullName		= newMesh.GetBlendShapeName(i);
		var humanName																= GetHumanName(newMesh.GetBlendShapeName(i));
		blendShapeObjects[currentCount].blendShapes[currentShapeCount].name			= humanName;
		blendShapeObjects[currentCount].blendShapes[currentShapeCount].isVisible	= DisplayThisBlendshape(newMesh.GetBlendShapeName(i));

		if (humanName)
		{
			var originalLength	= humanName.Length;
			var minusCheck		= humanName.Replace("Minus", "");
			if (minusCheck.Length != originalLength)
			{
				blendShapeObjects[currentCount].blendShapes[currentShapeCount].isMinus		= true;
				blendShapeObjects[currentCount].blendShapes[currentShapeCount].isVisible	= false;
			}
			else	// Is not minus, so list it with an InspectorID
			{
				if (blendShapeObjects[currentCount].blendShapes[currentShapeCount].isVisible)		// Add to InspectorObjects
					blendShapeObjects[currentCount].blendShapes[currentShapeCount].inspectorID	= AddToInspectorObjects(currentCount, currentShapeCount, i);
			}
			var plusCheck		= humanName.Replace("Plus", "");
			if (plusCheck.Length != originalLength)
			{
				blendShapeObjects[currentCount].blendShapes[currentShapeCount].isPlus		= true;
			}
		}

		blendShapeObjects[currentCount].blendShapes[currentShapeCount].value		= blendShapeObjects[currentCount].renderer.GetBlendShapeWeight(i);
		blendShapeObjects[currentCount].blendShapes[currentShapeCount].sliderValue	= blendShapeObjects[currentCount].renderer.GetBlendShapeWeight(i);
	}
}

function GetMinusShapeID(plusName : String) : int {
	//print ("GetMinusShapeID(" + plusName + ")");

	var minusName						= plusName.Replace("Plus", "Minus");

	for (var o : int; o < blendShapeObjects.Count; o++){
		for (var s : int; s < blendShapeObjects[o].blendShapes.Count; s++){
			if (blendShapeObjects[o].blendShapes[s].name == minusName)
				return s;
		}
	}
}

function GetMinusShapeObject(plusName : String) : int {
	//print ("GetMinusShapeObject(" + plusName + ")");

	var minusName						= plusName.Replace("Plus", "Minus");

	for (var o : int; o < blendShapeObjects.Count; o++){
		for (var s : int; s < blendShapeObjects[o].blendShapes.Count; s++){
			if (blendShapeObjects[o].blendShapes[s].name == minusName)
				return o;
		}
	}
}

function VisibleBlendShapes(newMesh : Mesh) : int {
	var totalObject	: int	= 0;
	for (var i : int; i < newMesh.blendShapeCount; i++){
		if (DisplayThisBlendshape(newMesh.GetBlendShapeName(i)))
		{
			totalObject++;
		}
	}
	return totalObject;
}


function MatchedBlendShapes(newMesh : Mesh) : int {
	var totalObject	: int	= 0;
	for (var i : int; i < newMesh.blendShapeCount; i++){
		if (MatchThisBlendshape(newMesh.GetBlendShapeName(i)))
		{
			totalObject++;
		}
	}
	return totalObject;
}

function GetHumanName(blendShapeName : String) : String {
	var humanName	: String;
	var periodParse	= blendShapeName.Split("."[0]);
	if (periodParse.Length > 1)
	{
		var nameParse	= periodParse[1].Split("_"[0]);
		if (nameParse.Length >= 3)
		{

			return nameParse[2];
		}
	}
	else
	{
		var nameParse2	= blendShapeName.Split("_"[0]);
		if (nameParse2.Length >= 3)
		{

			return nameParse2[2];
		}
	}

	return humanName;
}

function GetHumanNameMatch(blendShapeName : String) : String {
	var humanName	: String;
	var periodParse	= blendShapeName.Split("."[0]);
	if (blendShapeName.Contains("."))
	{
		var nameParse	= periodParse[1].Split("_"[0]);
		if (nameParse.Length >= 4)
		{
			return nameParse[3];
		}
	}
	else
	{
		var nameParse2	= blendShapeName.Split("_"[0]);
		if (nameParse2.Length >= 4)
		{
			return nameParse2[3];
		}
	}


	return humanName;
}

function MatchThisBlendshape(blendShapeName : String) : boolean {
	var periodParse	= blendShapeName.Split("."[0]);
	if (periodParse.Length > 1)
	{
		var nameParse	= periodParse[1].Split("_"[0]);
		if (nameParse.Length >= 3)
		{
			if (nameParse[1]	== "BSM")
				return true;
		}
	}
	else
	{
		var nameParse2	= blendShapeName.Split("_"[0]);
		if (nameParse2.Length >= 3)
		{
			if (nameParse2[1]	== "BSM")
				return true;
		}
	}

	return false;
}

function DisplayThisBlendshape(blendShapeName : String) : boolean {
	var periodParse	= blendShapeName.Split("."[0]);
	if (periodParse.Length > 1)
	{
		var nameParse	= periodParse[1].Split("_"[0]);
		if (nameParse.Length >= 3)
		{
			if (nameParse[1]	== "BS")
				return true;
		}
	}
	else
	{
		var nameParse2	= blendShapeName.Split("_"[0]);
		if (nameParse2.Length >= 3)
		{
			if (nameParse2[1]	== "BS")
				return true;
		}
	}

	return false;
}

function ReloadBlendShapes(){
	blendShapeObjects.Clear();
	inspectorObjects.Clear();
	var childCount	= gameObject.transform.childCount;
	for (var i : int; i < childCount; i++){
		var childObject	= gameObject.transform.GetChild(i);
		if (childObject.GetComponent.<SkinnedMeshRenderer>())
		{
			var newRenderer	= childObject.GetComponent.<SkinnedMeshRenderer>();
			var newMesh		= newRenderer.sharedMesh;
			if (newMesh.blendShapeCount > 0)
			{
				AddObject(newMesh, newRenderer);
			}
		}
	}

	for (var m : int; m < blendShapeObjects.Count; m++){
		//Debug.Log ("Going to Check For Matches: " + blendShapeObjects[m].name);
		for (var i2 : int; i2 < blendShapeObjects[m].object.blendShapeCount; i2++){
			if (MatchThisBlendshape(blendShapeObjects[m].object.GetBlendShapeName(i2)))
			{
				var matchHumanName	= GetHumanNameMatch(blendShapeObjects[m].object.GetBlendShapeName(i2));
				var matchObjectID	= m;
				var matchShapeID	= i2;
				//Debug.Log ("Found Match: " + matchHumanName + "(" + m + " | " + i2 + ")");
				AddMatchToNamedShape(matchHumanName, blendShapeObjects[m].object.GetBlendShapeName(i2), matchObjectID, matchShapeID);
			}
		}
	}
}

function AddMatchToNamedShape(humanName : String, matchName : String, matchObjectID, matchShapeID){
	for (var o : int; o < blendShapeObjects.Count; o++){
		for (var s : int; s < blendShapeObjects[o].blendShapes.Count; s++){
			if (blendShapeObjects[o].blendShapes[s].name == humanName)
			{
				//print ("Will Match This to " + blendShapeObjects[o].blendShapes[s].name);
				var newBlendMatch		: SFB_BlendMatch;
				var currentMatchCount	= blendShapeObjects[o].blendShapes[s].blendMatches.Count;
				blendShapeObjects[o].blendShapes[s].blendMatches.Add(newBlendMatch);
				blendShapeObjects[o].blendShapes[s].blendMatches[currentMatchCount] 			= new SFB_BlendMatch();
				blendShapeObjects[o].blendShapes[s].blendMatches[currentMatchCount].name		= matchName;
				blendShapeObjects[o].blendShapes[s].blendMatches[currentMatchCount].objectID	= matchObjectID;
				blendShapeObjects[o].blendShapes[s].blendMatches[currentMatchCount].shapeID		= matchShapeID;
			}
		}
	}
}

function FindMatches(name : String, id : int, shapeID: int, objectID : int){
	//Debug.Log ("Find Matches for " + name + " (" + id + ")");
	var childCount	= gameObject.transform.childCount;
	var i2 : int = 0;
	for (var i : int; i < childCount; i++){
		var childObject	= gameObject.transform.GetChild(i);
		if (childObject.GetComponent.<SkinnedMeshRenderer>())
		{
		//	print ("Find Matches for ID " + i2);
			var newRenderer	= childObject.GetComponent.<SkinnedMeshRenderer>();
			var newMesh		= newRenderer.sharedMesh;
			if (newMesh.blendShapeCount > 0)
			{
				CheckMatch(i2, newMesh, newRenderer, name, shapeID, objectID);
			}
			i2++;
		}
	}
}

function CheckMatch(matchObjectID : int, newMesh : Mesh, newRenderer : SkinnedMeshRenderer, matchName : String, shapeID : int, objectID : int){
	if (MatchedBlendShapes(newMesh) != 0)
	{
		for (var i : int; i < newMesh.blendShapeCount; i++){
			if (GetHumanNameMatch(newMesh.GetBlendShapeName(i)) == matchName)
			{
				//Debug.Log ("Match Blend Shape: " + newMesh.GetBlendShapeName(i));
				var newBlendMatch	: SFB_BlendMatch;								// Create new action
				var currentMatchCount	= blendShapeObjects[objectID].blendShapes[shapeID].blendMatches.Count;
				blendShapeObjects[objectID].blendShapes[shapeID].blendMatches.Add(newBlendMatch);
				blendShapeObjects[objectID].blendShapes[shapeID].blendMatches[currentMatchCount] 			= new SFB_BlendMatch();
				blendShapeObjects[objectID].blendShapes[shapeID].blendMatches[currentMatchCount].name		= newMesh.GetBlendShapeName(i);
				blendShapeObjects[objectID].blendShapes[shapeID].blendMatches[currentMatchCount].objectID	= matchObjectID;
				blendShapeObjects[objectID].blendShapes[shapeID].blendMatches[currentMatchCount].shapeID	= i;
			}
		}
	}
}