#pragma strict

var speed		: float		= 2.0;			// Speed of the particle

function Update () {
	// Move particle forward
	transform.Translate(Vector3.forward * Time.deltaTime * speed);
}