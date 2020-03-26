#pragma strict

var fadeLimit		: float		= 1.0;
var fadePassed		: float		= 0.0;
var fadeSpeed		: float		= 1.0;			// Multiplier of Time.deltaTime
var maxIntensity	: float		= 0.8;
var done			: boolean	= false;
var maxTime			: float		= 0.0;			// Leave 0.0 if not in use!
var timePassed		: float		= 0.0;

function Start () {

}

function Update () {
	if (maxTime != 0.0 && !done)
	{
		timePassed += Time.deltaTime;
		if (maxTime <= timePassed)
			done = true;
	}
	if (!done)
	{
		fadePassed += Time.deltaTime;
		if (fadePassed >= fadeLimit)
			FadeIn();
	}
}

function FadeIn(){
	GetComponent(Light).intensity 	= Mathf.Clamp(GetComponent(Light).intensity + (Time.deltaTime * fadeSpeed), 0, maxIntensity);
	if (GetComponent(Light).intensity == maxIntensity)
		done = true;
}