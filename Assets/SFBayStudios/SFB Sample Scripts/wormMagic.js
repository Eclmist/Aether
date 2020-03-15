#pragma strict

// Attach this script to your character, and call "CastSpell" from an animation event.
// In this example, you should also include a spellNumber int as part of the animation
// event.

var magicSpells		: GameObject[];			// Array of spell particles
var magicSpawnPos	: Transform[];			// Array of positions where spells will be spawned.
											// Should be 1 for 1 with MagicSpells[]
var charAnimator	: Animator;				// The Animator attached.

function Start () {
	// Cache the animator.
	charAnimator		= GetComponent(Animator);
}

// This can be used w/o the SpellNumber for simpler situations.  Just make sure the two variables
// above are not arrays.
function CastSpell(spellNumber : int){
	// Instantiates a magicSpell prefab at magicSpawnPos
	var newSpellObject		= Instantiate(magicSpells[spellNumber], magicSpawnPos[spellNumber].position, Quaternion.identity);
	// If you want specific logic for specific spells, you can put it here.  In this following lines,
	// we ensure that the magic spell in the 0 spot of the array looks up when it's created.
	if (spellNumber == 0)
		newSpellObject.transform.LookAt(newSpellObject.transform.position + Vector3(0,1,0));
}


// ----------------------------------------------------------
// YOU ARE FREE TO USE THESE SCRIPTS IN YOUR PROJECT IN ANY WAY YOU WOULD LIKE!
// IF YOU HAVE PURCHASED OUR MODELS TO USE IN YOUR PROJECTS, WE THANK YOU VERY
// MUCH FOR YOUR SUPPORT!  WITHOUT IT, WE COULDN'T KEEP MAKING MORE KILLER
// ASSETS FOR YOUR GAMES.
// ----------------------------------------------------------