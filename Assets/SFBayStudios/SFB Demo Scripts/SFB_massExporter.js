#pragma strict

import System.Collections.Generic;
import System.IO;

var substances			: SubstanceArchive[];					// All the substance we are working with
var groupName			: String;								// Name of the group -- all materials will be under this directory
private var overrideMaterialName	: String;					// Overrides the material name, so you can name it what you want. Only works in single export
private var overrideNameEnabled		: boolean	= false;
private var oldGroupName	: String;
var removeEmissive		: boolean				= true;	
var removeHeight		: boolean				= true;

var setNormalMapMode	: boolean				= true;
var mergeWithGroup		: boolean				= true;			// Merge with group.  If false, will create "Group Name [#]" if conflict exists

var previousGroupName			: String;
var useNormalFromPrevious		: boolean		= false;
var useAOFromPrevious			: boolean		= false;
var useMetalRoughFromPrevious	: boolean		= false;
var useHeightFromPrevious		: boolean		= false;
var useEmissiveFromPrevious		: boolean		= false;

function SetOverrideName(isActive : boolean, newName : String){
	overrideNameEnabled		= isActive;
	overrideMaterialName	= newName;
}

function ExportSubstances(){
	oldGroupName				= groupName;
	if (groupName == "" || groupName == null)
		groupName	= "Unnamed" + Random.Range(11111,99999).ToString();
	if (!mergeWithGroup)
		groupName	= CheckGroupName(groupName);
	Debug.Log("Starting Export (" + groupName + ")");
	for (var i : int; i < substances.Length; i++){
		var substance										= substances[i];
		var substancePath			: String				= AssetDatabase.GetAssetPath( substance.GetInstanceID() );
		var substanceImporter		: SubstanceImporter		= AssetImporter.GetAtPath( substancePath ) as SubstanceImporter;
		var substanceMaterials		: ProceduralMaterial[]	= substanceImporter.GetMaterials();
		for (var s : int; s < substanceMaterials.Length; s++){		
			var substanceMaterial	= substanceMaterials[s];	
			Debug.Log(substanceMaterial.name + " Starting...");

			ExportSubstance(substanceImporter, substanceMaterial);
		}
	}
	groupName	= oldGroupName;
	AssetDatabase.Refresh();
}

function CheckGroupName(groupName : String) : String{
	var checkPath				= "Assets/SFBayStudios/Exported Materials/" + groupName;
	var count					= 0;
	while (Directory.Exists(checkPath) && count < 100){
		count++;
		checkPath				= "Assets/SFBayStudios/Exported Materials/" + groupName + "_" + (count + 1);
	}
	var newGroupName	: String	= groupName;
	if (count != 0)
		newGroupName	= groupName + "_" + (count + 1);
	return newGroupName;
}

function CheckMaterialName(substanceMaterialName : String) : String{
	var checkPath				=  "Assets/SFBayStudios/Exported Materials/" + groupName + "/tex_" + substanceMaterialName;
	var count					= 0;
	while (Directory.Exists(checkPath) && count < 100){
		count++;
		checkPath				= "Assets/SFBayStudios/Exported Materials/" + groupName + "/tex_" + substanceMaterialName + "_" + (count + 1);
	}
	var newMatName	: String	= substanceMaterialName;
	if (count != 0)
		newMatName	= substanceMaterialName + "_" + (count + 1);
	print ("substanceMaterialName: " + substanceMaterialName);
	return newMatName;
}

function ExportSubstance(substanceImporter : SubstanceImporter, substanceMaterial : ProceduralMaterial){
	var substanceMaterialName			= substanceMaterial.name;
	if (overrideNameEnabled)
		substanceMaterialName			= overrideMaterialName;
	substanceMaterialName				= CheckMaterialName(substanceMaterialName);
	var path							= "Assets/SFBayStudios/Exported Materials/" + groupName + "/tex_" + substanceMaterialName + "/";
	var previousPath					= "Assets/SFBayStudios/Exported Materials/" + previousGroupName + "/tex_" + substanceMaterial.name + "/";
	substanceImporter.ExportBitmaps(substanceMaterial, path, true);
	RemoveFilesPreImport(path, substanceMaterial.name);
	var newMaterial 		: Material 	= CreateMaterial(substanceMaterial, substanceMaterialName);
	var propertyCount 		: int 		= ShaderUtil.GetPropertyCount (newMaterial.shader);
	var materialTextures 	: Texture[] = substanceMaterial.GetGeneratedTextures();
	for (var materialTexture : ProceduralTexture in materialTextures) {
		Debug.Log("materialTexture.name: " + materialTexture.name);
		if (materialTexture.name == substanceMaterial.name + "_albedoOpacity" || 
			materialTexture.name == substanceMaterial.name + "_normal" || 
			materialTexture.name == substanceMaterial.name + "_ambientOcclusion" || 
			materialTexture.name == substanceMaterial.name + "_metallicRoughness" || 
			materialTexture.name == substanceMaterial.name + "_height" || 
			materialTexture.name == substanceMaterial.name + "_emissive")
		{
			var newTexturePath 	: String;
			if (previousPath == path)
			{
				print ("previousPath & path Are the Same!");
				newTexturePath 					= path + materialTexture.name + ".tga";
			}
			else if ((materialTexture.name == substanceMaterial.name + "_normal" && useNormalFromPrevious && System.IO.File.Exists(previousPath + materialTexture.name + ".tga")) || 
				(materialTexture.name == substanceMaterial.name + "_ambientOcclusion" && useAOFromPrevious && System.IO.File.Exists(previousPath + materialTexture.name + ".tga")) || 
				(materialTexture.name == substanceMaterial.name + "_metallicRoughness" && useMetalRoughFromPrevious && System.IO.File.Exists(previousPath + materialTexture.name + ".tga")) || 
				(!removeEmissive && materialTexture.name == substanceMaterial.name + "_height" && useHeightFromPrevious && System.IO.File.Exists(previousPath + materialTexture.name + ".tga")) || 
				(!removeHeight && materialTexture.name == substanceMaterial.name + "_emissive" && useEmissiveFromPrevious && System.IO.File.Exists(previousPath + materialTexture.name + ".tga")))
			{
				// Select the previous texture path instead, and delete this new map
				newTexturePath 				= previousPath + materialTexture.name + ".tga";
				print ("newTexturePath: " + newTexturePath);
				DeleteTexture(path, materialTexture.name);
			}
			else	// Else use the new path
			{
				newTexturePath 					= path + materialTexture.name + ".tga";
				print ("Normal newTexturePath: " + newTexturePath);
			}
			var newTextureAsset : Texture 	= AssetDatabase.LoadAssetAtPath (newTexturePath, typeof(Texture)) as Texture;
			for (var i : int = 0; i < propertyCount; i++) {
				if (ShaderUtil.GetPropertyType (newMaterial.shader, i) == ShaderUtil.ShaderPropertyType.TexEnv) {
					var propertyName	: String	= ShaderUtil.GetPropertyName (newMaterial.shader, i);
					if (newMaterial.GetTexture (propertyName) != null && newMaterial.GetTexture (propertyName).name == newTextureAsset.name) {
						newMaterial.SetTexture (propertyName, newTextureAsset);
					}
				}
			}
			if (setNormalMapMode)
			{
				if (materialTexture.GetProceduralOutputType () == ProceduralOutputType.Normal) {
					var textureImporter : TextureImporter	= AssetImporter.GetAtPath (newTexturePath) as TextureImporter;
					textureImporter.textureType = TextureImporterType.Bump;
					AssetDatabase.ImportAsset (newTexturePath);
				}
			}
		}
		else
			DeleteTexture(path, materialTexture.name);

		if (removeEmissive && materialTexture.name == substanceMaterial.name + "_emissive")
			DeleteTexture(path, materialTexture.name);
		else if (removeHeight && materialTexture.name == substanceMaterial.name + "_height")
			DeleteTexture(path, materialTexture.name);
	}
}

// Do this before we import, so we don't waste time importing files we'll just delete later.
function RemoveFilesPreImport(path : String, fileName : String){
	DeleteFromDisk(path, fileName + "_albedo");
	DeleteFromDisk(path, fileName + "_opacity");
	DeleteFromDisk(path, fileName + "_roughness");
	DeleteFromDisk(path, fileName + "_metallic");
}

function DeleteFromDisk(path : String, textureName : String)
{
	FileUtil.DeleteFileOrDirectory(path + "" + textureName + ".tga");
}

function DeleteTexture(path : String, textureName : String)
{
	print ("Deleting Texture: " + path + "" + textureName + ".tga");
	AssetDatabase.DeleteAsset(path + "" + textureName + ".tga");
}

function CreateMaterial(sourceMaterial : ProceduralMaterial, newName : String) : Material{
	var matPath		: String	= "Assets/SFBayStudios/Exported Materials/" + groupName + "/" + newName + ".mat";
	var newMaterial : Material 	= new Material (sourceMaterial.shader);
	newMaterial.CopyPropertiesFromMaterial (sourceMaterial);
	AssetDatabase.CreateAsset (newMaterial, matPath);
	AssetDatabase.Refresh();
	if (removeEmissive)
		newMaterial.SetColor ("_EmissionColor", Color(0,0,0,0));
	return newMaterial;
}


