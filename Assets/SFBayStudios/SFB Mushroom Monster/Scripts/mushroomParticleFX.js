#pragma strict

var castSpot		: Transform;
var castPrefab		: GameObject;

var animator		: Animator;

function CastMagic(){
	var newSpell		= Instantiate(castPrefab, castSpot.position, castSpot.rotation);
	Destroy(newSpell, 5.0);
}

function UpdateLocomotion(newValue : float){
	animator.SetFloat("locomotion", newValue);
}