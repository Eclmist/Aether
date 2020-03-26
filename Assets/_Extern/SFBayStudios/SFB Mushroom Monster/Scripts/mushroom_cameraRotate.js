#pragma strict

var target				: Transform;
var speed				: float			= 10.0;
var speedChangeMod		: float			= 20.0;
var maxSpeed			: float			= 100.0;
var zoomSpeed			: float			= 1.0;
var riseSpeed			: float			= 1.0;
var tiltSpeed			: float			= 1.0;
var maxZoom				: float			= 25.0;
var minZoom				: float			= 70.0;
var maxHeight			: float			= 0.0;
var minHeight			: float			= 0.0;
var maxTilt				: float			= 0.0;
var minTilt				: float			= 0.0;

var increasingRight		: boolean		= false;
var increasingLeft		: boolean		= false;
var zoomingIn			: boolean		= false;
var zoomingOut			: boolean		= false;
var raising				: boolean		= false;
var lowering			: boolean		= false;
var tiltingUp			: boolean		= false;
var tiltingDown			: boolean		= false;

function Start(){
	FixTilt();
}

function Update(){
	transform.RotateAround (target.position  + Vector3(0,1,0), Vector3.up, speed * Time.deltaTime);
	if (increasingRight)
		speed = Mathf.Clamp(speed - (Time.deltaTime * speedChangeMod), -maxSpeed, maxSpeed);
	if (increasingLeft)
		speed = Mathf.Clamp(speed + (Time.deltaTime * speedChangeMod), -maxSpeed, maxSpeed);
	if (zoomingIn)
		GetComponent(Camera).fieldOfView = Mathf.Clamp(GetComponent(Camera).fieldOfView - (zoomSpeed * Time.deltaTime), maxZoom, minZoom);
	if (zoomingOut)
		GetComponent(Camera).fieldOfView = Mathf.Clamp(GetComponent(Camera).fieldOfView + (zoomSpeed * Time.deltaTime), maxZoom, minZoom);

	if (raising)
	{
		transform.position.y		= Mathf.Clamp(transform.position.y + (riseSpeed * Time.deltaTime), minHeight, maxHeight);
		FixTilt();
	}
	if (lowering)
	{
		transform.position.y		= Mathf.Clamp(transform.position.y + (-riseSpeed * Time.deltaTime), minHeight, maxHeight);
		FixTilt();
	}
}

function FixTilt(){
	var heightPercent				= (transform.position.y - minHeight) / (maxHeight - minHeight);
	var tiltValue					= minTilt + ((maxTilt - minTilt) * heightPercent);
	transform.eulerAngles.x			= tiltValue;
}

function IncreaseLeft(){
	increasingLeft 		= true;
}

function IncreaseRight(){
	increasingRight 	= true;
}

function ZoomIn(){
	zoomingIn	 		= true;
}

function ZoomOut(){
	zoomingOut	 		= true;
}

function StopIncreasing(){
	increasingLeft 		= false;
	increasingRight 	= false;
}

function StopZooming(){
	zoomingIn	 		= false;
	zoomingOut		 	= false;
}

function StopMovement(){
	speed = 0;
}

function Raise(){
	raising 			= true;
}

function Lower(){
	lowering			= true;
}

function StopRise(){
	raising				= false;
	lowering			= false;
}
