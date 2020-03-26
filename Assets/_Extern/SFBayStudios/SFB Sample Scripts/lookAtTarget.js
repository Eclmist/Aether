#pragma strict

var targetObject	: GameObject;					// Target object, or object which the character will look at.
var turnSpeed		: float				= 4.0;		// Speed at which the turn will occur.  1 = 1 unit / second.

function Start(){
	if (!targetObject)
		targetObject		= GameObject.Find("PlayerController");
}

function Update(){
	// Creates a Vector with the X/Z of the target, but the Y of the source
	// Otherwise, when the playerObject gets close to the characer doing the looking,
	// it will lean back or forward in order to Look At the player.
	if (targetObject)
	{
		var targetPositionFixed		= Vector3(targetObject.transform.position.x, transform.position.y, targetObject.transform.position.z);		
		var targetRotation 			= Quaternion.LookRotation(targetPositionFixed - transform.position);									// Get rotation between two objects
		transform.rotation			= Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);						// Turn towards targetObject over time.
	}
}

// Call this function in any way you'd like to set a new targetObject.  One method is to use
// SendMessage() as so:  SendMessage ("SetNewTarget", newTargetObject);
// The script that calls SendMessage() must be on the same object as this script.
function SetNewTarget(newTarget : GameObject){
	targetObject		= newTarget;
}



























