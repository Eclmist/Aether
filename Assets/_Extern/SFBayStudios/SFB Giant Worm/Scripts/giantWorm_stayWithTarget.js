#pragma strict

var target			: GameObject;
var animController	: GameObject;
var currentOffset	: float				= 0.0;
var desiredOffset	: float				= 0.0;
var maxOffset		: float				= 1.5;
var offsetMod		: float				= 0.8;

function LateUpdate(){
	transform.position		= target.transform.position;
	transform.position.y	+= currentOffset;
}