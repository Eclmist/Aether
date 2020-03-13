#pragma strict

var objectToWatch		: GameObject;
var objectToSwitch		: GameObject;

function Update(){
	print ("objectowatch: " + objectToWatch.activeSelf);
	objectToSwitch.SetActive(!objectToWatch.activeSelf);
}