#pragma strict

var footstep		: GameObject;				// Footstep Particle
var clapDust		: GameObject;				// Clap Particle
var laser			: GameObject[];				// Laser Particle

var leftFoot		: Transform;				// Left foot position
var rightFoot		: Transform;				// Right foot position
var clap			: Transform;				// Clap particle spawn position

var animator		: Animator;

function Start(){
	animator	= GetComponent.<Animator>();
}

function SetLocomotion(newValue : float){
	animator.SetFloat("locomotion", newValue);
}

function LeftFoot(){
	if (leftFoot)
	{
		var newParticle		= Instantiate(footstep, leftFoot.position, Quaternion.identity);
		Destroy(newParticle, 5.0);
	}
}

function RightFoot(){
	if (rightFoot)
	{
		var newParticle		= Instantiate(footstep, rightFoot.position, Quaternion.identity);
		Destroy(newParticle, 5.0);
	}
}

function Laser(){
	for (var i : int; i < laser.Length; i++)
	{
		if (laser[i])
		{
			if (laser[i].GetComponent(ParticleSystem))
				laser[i].GetComponent(ParticleSystem).enableEmission = true;
			else if (laser[i].GetComponent(Light))
				laser[i].SetActive(true);
		}
	}
}

function Clap(){
	if (clap)
	{
		var newParticle		= Instantiate(clapDust, clap.position, Quaternion.identity);
		Destroy(newParticle, 5.0);
	}
}

function LaserStop(){
	for (var i : int; i < laser.Length; i++)
	{
		if (laser[i].GetComponent(ParticleSystem))
			laser[i].GetComponent(ParticleSystem).enableEmission = false;
		else if (laser[i].GetComponent(Light))
			laser[i].SetActive(false);
	}
}