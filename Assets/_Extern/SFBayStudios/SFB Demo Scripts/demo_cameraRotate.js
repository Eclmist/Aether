#pragma strict

var target				: Transform;
var targetOffset		: float			= 1.0;
var speed				: float			= 10.0;


function SetSpeed(newValue : float){
	speed	= newValue;
}

function Update(){
	transform.RotateAround (target.position  + Vector3(0,1,0), Vector3.up, speed * Time.deltaTime);
	transform.LookAt(target.position + Vector3(0,targetOffset,0));
}
