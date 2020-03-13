var speedDesgrees 						= 10;
var maxSpeed							= 180;
var minHeight							= 1;
var maxHeight							= 6;
var minFOV								= 15;
var maxFOV								= 60;
var target 				: Transform;
var cubes				: GameObject;
var lights				: GameObject;
var controller			: GameObject;
 
function Update() {
    transform.RotateAround (target.position, Vector3.up, speedDesgrees * Time.deltaTime);
    transform.LookAt(target);
	if (Input.GetKey ("q"))
		speedDesgrees = Mathf.Clamp(speedDesgrees - 1, -maxSpeed, maxSpeed);
	if (Input.GetKey ("w"))
		speedDesgrees = Mathf.Clamp(speedDesgrees + 1, -maxSpeed, maxSpeed);
	if (Input.GetKey ("e"))
		speedDesgrees = 0;
	if (Input.GetKey ("a"))
		transform.position.y = Mathf.Clamp(transform.position.y - 0.1, minHeight, maxHeight);
	if (Input.GetKey ("s"))
		transform.position.y = Mathf.Clamp(transform.position.y + 0.1, minHeight, maxHeight);
	if (Input.GetKeyDown ("l"))
		ToggleLights();
	if (Input.GetKeyDown ("c"))
		ToggleCubes();
	if (Input.GetKey ("z"))
		GetComponent(Camera).fieldOfView = Mathf.Clamp(GetComponent(Camera).fieldOfView + 1, minFOV, maxFOV);
	if (Input.GetKey ("x"))
		GetComponent(Camera).fieldOfView = Mathf.Clamp(GetComponent(Camera).fieldOfView - 1, minFOV, maxFOV);
}

function ToggleLights(){
	if (lights.active)
		lights.SetActive(false);
	else
		lights.SetActive(true);
}

function ToggleCubes(){
	if (cubes.active)
		cubes.SetActive(false);
	else
		cubes.SetActive(true);
}

function OnGUI () 
{     
    GUI.Label (Rect (10, 10, 100000, 20000), controller.GetComponent(particleDemoControl).currentParticleName);
	GUI.Label (Rect (10, 45, 100000, 20000), "Q/W Changes Camera Rotation Speed\nA/S Change Camera Height\nZ/X Zoom Camera\nL Toggles Lights\nC Toggles Cubes\nSPACE to Show Particle\nLEFT/RIGHT Arrows Switch Particle\nNote:  Some look better with lights on or off");
}