#pragma strict

var colors		: Color[];
var minTime		: float		= 0.1;
var maxTime		: float		= 0.25;
var curTime		: float		= 0.0;
var curColor	: Color;
var everyFrame	: boolean	= false;

function Start () {

}

function Update () {
	if (colors.Length != 0)
	{
		curTime = Mathf.Clamp(curTime - Time.deltaTime, 0, maxTime);
		if (curTime == 0 || everyFrame)
		{
			var randomTime 				= Random.Range(minTime, maxTime);
			curTime 					= randomTime;
			var randomColor 			= Random.Range(0, colors.Length);
			while (randomColor == curColor)
				randomColor 			= Random.Range(0, colors.Length);
			GetComponent(Light).color 	= colors[randomColor];
		}
	}
}