#pragma strict

// This will print the total triangle count of all objects in the mesh & children of it to the console.

@MenuItem ("Window/Polycount %#p")
static function ShowPolycount () {
	if (Selection.activeObject.GetType() == UnityEngine.GameObject)
	{
		var totalTriCount		: int		= 0;
		// Get count of parent object if applicable
		if (Selection.activeGameObject.GetComponent.<MeshFilter>())
		{
			var parentMesh 		: Mesh 	= Selection.activeGameObject.GetComponent.<MeshFilter>().sharedMesh;
			var parentTri		: int	= parentMesh.triangles.Length / 3;
			totalTriCount		+= parentTri;
		}
		// Get count of all children of object
		var allChildren = Selection.activeGameObject.GetComponentsInChildren(Transform);
		for (var child : Transform in allChildren) {
			//print ("Object: " + child.name);
			if (child.gameObject.GetComponent.<MeshFilter>())
			{
				var objMesh 		: Mesh 	= child.gameObject.GetComponent.<MeshFilter>().sharedMesh;
				var triCount		: int	= objMesh.triangles.Length / 3;
				totalTriCount		+= triCount;
			}
			else if (child.gameObject.GetComponent.<SkinnedMeshRenderer>())
			{
				var objMesh2 		: Mesh 	= child.gameObject.GetComponent.<SkinnedMeshRenderer>().sharedMesh;
				var triCount2		: int	= objMesh2.triangles.Length / 3;
				totalTriCount		+= triCount2;
			}
		}
		print ("There are " + totalTriCount + " triangles.");
	}
}

@MenuItem ("Window/Save Texture", true)
    static function ValidateObject() {
        return Selection.activeObject.GetType() == UnityEngine.GameObject;
    }

