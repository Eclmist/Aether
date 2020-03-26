#pragma strict

var timeScaleValue		: float		= 1.0;			// 1.0 = real time, 0.0 = paused.
var adjustedValue		: float		= 0.0;			// Runtime adjustment

function Start () {
	SetTime(0.0);
}

function Update () {
	if (Input.GetKeyDown("="))
		SetTime(0.05);
	if (Input.GetKeyDown("-"))
		SetTime(-0.05);
}

function SetTime(newValue : float){
	print ("SetTime(" + newValue + ")");
	adjustedValue	+= newValue;
	Time.timeScale	= Mathf.Clamp(timeScaleValue + adjustedValue, 0, 3.0);
}