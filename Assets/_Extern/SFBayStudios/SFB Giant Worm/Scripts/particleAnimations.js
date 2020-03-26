#pragma strict

var magicSpell		: ParticleSystem[];
var groundParticles	: ParticleSystem[];

function StartMagicSpell(){
	for (var i : int; i < magicSpell.Length; i++){
		magicSpell[i].Play();
	}
}

function StopMagicSpell(){
	for (var i : int; i < magicSpell.Length; i++){
		magicSpell[i].Stop();
	}
}

function StartGround(){
	for (var i : int; i < groundParticles.Length; i++){
		groundParticles[i].Play();
	}
}

function StopGround(){
	for (var i : int; i < groundParticles.Length; i++){
		groundParticles[i].Stop();
	}
}