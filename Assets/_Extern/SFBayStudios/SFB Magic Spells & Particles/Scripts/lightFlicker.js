#pragma strict

var flickerMin		: float		= 0.1;
var flickerMax		: float		= 0.3;
var flickerCount	: float		= 0.3;
var intensityMin	: float		= 0.9;
var intensityMax	: float		= 1.1;

function Start () {

}

function Update () {
	flickerCount = Mathf.Clamp(flickerCount - Time.deltaTime, 0, flickerMax);
	if (flickerCount == 0)
	{
		var newIntensity 				= Random.Range(intensityMin, intensityMax);
		GetComponent(Light).intensity 	= newIntensity;
		var newTime					 	= Random.Range(flickerMin, flickerMax);
		flickerCount					= newTime;
	}
}