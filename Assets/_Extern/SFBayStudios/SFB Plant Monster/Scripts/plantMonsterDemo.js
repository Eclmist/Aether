#pragma strict

var animator			: Animator;

var gravityParticle		: ParticleSystem[];
var iceBallPrefab		: GameObject;
var iceBallForceY		: float					= 500;
var iceBallForceSide	: float					= 50;
var iceBallForceSideMin	: float					= 25;
var magicSpawnPos		: Transform;
var puffParticle		: GameObject;

function Start(){
	animator	= GetComponent.<Animator>();
}

function UpdateLocomotion(newValue : float){
	animator.SetFloat("locomotion", newValue);
}

function CastingWarmup(start : int){
	for (var i : int; i < gravityParticle.Length; i++){
		if (start == 1)
			gravityParticle[i].Play();
		else
			gravityParticle[i].Stop();
	}
}

function IceBall(){
	var newIceBall 				= Instantiate(iceBallPrefab, magicSpawnPos.position, Quaternion.identity);
	newIceBall.transform.LookAt(newIceBall.transform.position + Vector3.up);
	newIceBall.GetComponent.<Rigidbody>().AddForce(Vector3(0, iceBallForceY, 0));
	NudgeBall(newIceBall);
}

function NudgeBall(newIceBall : GameObject){
	yield WaitForSeconds(1.0);

	var randomX		: float;
	var randomZ		: float;
	while (randomX > -iceBallForceSideMin && randomX < iceBallForceSideMin)
		randomX	= Random.Range(-iceBallForceSide, iceBallForceSide);
	while (randomZ > -iceBallForceSideMin && randomZ < iceBallForceSideMin)
		randomZ	= Random.Range(-iceBallForceSide, iceBallForceSide);
	newIceBall.GetComponent.<Rigidbody>().AddForce(Vector3(randomX, 0, randomZ));
}

function Puff(){
	var newPuff = Instantiate(puffParticle, magicSpawnPos.position, Quaternion.identity);
	Destroy(newPuff, 5.0);
}