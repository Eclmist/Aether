#pragma strict
@script ExecuteInEditMode()

// This script will copy the changes from one ProceduralMaterial to another.  For use with SFBayStudios
// models, which sometimes have multiple materials per object.  The sourceXYZ should be the same length,
// and in the same order as targetXYZ.  The names may not match from one material to another.

// To find out what the "name" should be, hover over the slider, toggle, etc in the Procedrual Material.
// Often the name will be something like "1_lightness".

var sourceMaterial			: ProceduralMaterial;					// Material to copy from
var targetMaterial			: ProceduralMaterial;					// Material to copy to
var sourceFloats			: String[];								// The source names
var targetFloats			: String[];								// The target names
var sourceColors			: String[];								// The source names
var targetColors			: String[];								// The target names
var sourceBooleans			: String[];								// The source names
var targetBooleans			: String[];								// The target names
var sourceVectors			: String[];								// The source names
var targetVectors			: String[];								// The target names
private var rebuildTextures	: boolean				= false;

function Update () {
	if (sourceMaterial && targetMaterial)
	{
		if (sourceFloats.Length == targetFloats.Length)
			CopyFloats();
		else
			print ("Error: sourceFloats and targetFloats must be the same length.");

		if (sourceColors.Length == targetColors.Length)
			CopyColors();
		else
			print ("Error: sourceColors and targetColors must be the same length.");

		if (sourceBooleans.Length == targetBooleans.Length)
			CopyBooleans();
		else
			print ("Error: sourceBooleans and targetBooleans must be the same length.");

		if (sourceVectors.Length == targetVectors.Length)
			CopyVectors();
		else
			print ("Error: sourceVectors and targetVectors must be the same length.");
	}
}

function CopyFloats(){
	// For each entry, check to see if they match.  If they do not, make them match and then set rebuildTextures to
	// true, so that in LateUpdate() the texture will be rebuilt.
	for (var i : int; i < sourceFloats.Length; i++){
		if (targetMaterial.GetProceduralFloat(targetFloats[i]) != sourceMaterial.GetProceduralFloat(sourceFloats[i]))
		{
			targetMaterial.SetProceduralFloat(targetFloats[i], sourceMaterial.GetProceduralFloat(sourceFloats[i]));
			rebuildTextures		= true;
		}
	}
}

function CopyColors(){
	// For each entry, check to see if they match.  If they do not, make them match and then set rebuildTextures to
	// true, so that in LateUpdate() the texture will be rebuilt.
	for (var i : int; i < sourceColors.Length; i++){
		if (targetMaterial.GetProceduralColor(targetColors[i]) != sourceMaterial.GetProceduralColor(sourceColors[i]))
		{
			targetMaterial.SetProceduralColor(targetColors[i], sourceMaterial.GetProceduralColor(sourceColors[i]));
			rebuildTextures		= true;
		}
	}
}

function CopyBooleans(){
	// For each entry, check to see if they match.  If they do not, make them match and then set rebuildTextures to
	// true, so that in LateUpdate() the texture will be rebuilt.
	for (var i : int; i < sourceBooleans.Length; i++){
		if (targetMaterial.GetProceduralBoolean(targetBooleans[i]) != sourceMaterial.GetProceduralBoolean(sourceBooleans[i]))
		{
			targetMaterial.SetProceduralBoolean(targetBooleans[i], sourceMaterial.GetProceduralBoolean(sourceBooleans[i]));
			rebuildTextures		= true;
		}
	}
}

function CopyVectors(){
	// For each entry, check to see if they match.  If they do not, make them match and then set rebuildTextures to
	// true, so that in LateUpdate() the texture will be rebuilt.
	for (var i : int; i < sourceVectors.Length; i++){
		if (targetMaterial.GetProceduralVector(targetVectors[i]) != sourceMaterial.GetProceduralVector(sourceVectors[i]))
		{
			targetMaterial.SetProceduralVector(targetVectors[i], sourceMaterial.GetProceduralVector(sourceVectors[i]));
			rebuildTextures		= true;
		}
	}
}

function LateUpdate(){
	if (rebuildTextures)
		RebuildTextures();
}

function RebuildTextures(){
	if (!targetMaterial.isProcessing)
	{
		targetMaterial.RebuildTextures();
		rebuildTextures		= false;
	}
}