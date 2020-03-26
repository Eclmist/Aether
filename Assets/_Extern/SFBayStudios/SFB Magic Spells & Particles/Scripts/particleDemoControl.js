#pragma strict

var particleNumber		: int				= 0;		// The current selected particle from particles[]
var particles			: GameObject[];					// The particles Availalbe for the demo
var targets				: GameObject[];					// Targets for shooting particles
var targetNumber		: int				= 0;		// The current target
var currentParticle		: GameObject;					// Holds the current in-game object
var currentParticleName : String;

function Start () {
	currentParticleName = "Current: " + particles[particleNumber].name + " (" + (particleNumber + 1) + " of " + particles.Length + ")";
}

function Update () {
	if (Input.GetKeyDown ("space"))
		ShowNewParticle();
	if (Input.GetKeyDown ("left"))
		SwitchParticle(-1);
	if (Input.GetKeyDown ("right"))
		SwitchParticle(1);
}

function ShowNewParticle(){
	if (currentParticle)
		Destroy(currentParticle);
	currentParticle = Instantiate(particles[particleNumber], Vector3(0, 0, 0), Quaternion.identity);
	currentParticle.transform.position 		= currentParticle.GetComponent(demoParticleControl).startPosition;
	currentParticle.transform.eulerAngles 	= currentParticle.GetComponent(demoParticleControl).startRotation;
	if (currentParticle.GetComponent(demoParticleControl).shootsTarget)
	{
		currentParticle.transform.LookAt(targets[targetNumber].transform);
		targetNumber += 1;
		if (targetNumber < 0)
			targetNumber = targets.Length - 1;
		else if (targetNumber == targets.Length)
			targetNumber = 0;
	}
}

function SwitchParticle(value : int){
	particleNumber += value;
	if (particleNumber < 0)
		particleNumber = particles.Length - 1;
	else if (particleNumber == particles.Length)
		particleNumber = 0;
	currentParticleName = "Current: " + particles[particleNumber].name + " (" + (particleNumber + 1) + " of " + particles.Length + ")";
}