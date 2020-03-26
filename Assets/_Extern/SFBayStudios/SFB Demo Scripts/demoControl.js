#pragma strict

var cameraObject			: GameObject;					// The camera object (not the parent)

function Start(){

}

function SetCameraHeight(newValue : float){
	cameraObject.transform.position.y	= newValue;
}

function SetTimescale(newValue : float){
	Time.timeScale	= newValue;
}