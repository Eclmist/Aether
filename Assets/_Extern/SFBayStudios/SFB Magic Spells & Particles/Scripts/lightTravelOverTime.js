#pragma strict

var travelDirection		: Vector3;		// 0,0,1 = forward etc
var local				: boolean		= false;
var speed				: float			= 1.0;
var delay				: float			= 0.0;
var delayCount			: float;

function Start () {
	delayCount = delay;
}

function Update () {
	if (delayCount <= 0)
	{
		if (local)
			transform.localPosition += travelDirection * Time.deltaTime * speed;
		else if (!local)
			transform.position += travelDirection * Time.deltaTime * speed;
	}
	else
		delayCount -= Time.deltaTime;
}