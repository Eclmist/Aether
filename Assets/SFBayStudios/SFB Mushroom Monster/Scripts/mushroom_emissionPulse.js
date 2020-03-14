#pragma strict

var objectsWithEmission		: GameObject[];
var emissionColor			: Color			= Color(0.5,0,0);
var maxEmission				: float			= 0.5;

function Update () {
	for (var i : int; i < objectsWithEmission.Length; i++)
	{
		var newAlpha		= Mathf.PingPong(Time.time, maxEmission);
		var tempRenderer	= objectsWithEmission[i].GetComponent.<Renderer>();
		DynamicGI.SetEmissive(tempRenderer,Color(emissionColor.r,emissionColor.g,emissionColor.b,newAlpha));
		
		
		if (i == 0)
			print ("newAlpha: " + newAlpha);
		
		//rend.material.SetColor ("_SpecColor", Color.red);
		//objectsWithEmission[i].GetComponent.<Renderer>().emissive		= Mathf.PingPong(Time.time, maxEmission);
		//objectsWithEmission[i].DynamicGI.SetEmissive(Renderer,Color,Mathf.PingPong(Time.time, maxEmission));
	}
}