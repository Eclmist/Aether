#pragma strict

var explodeParticle	: GameObject;

function OnCollisionEnter(collision: Collision){
	var weHit	=	 collision.gameObject;
	print ("Collided with " + weHit.name);
	if (weHit.name == "Terrain")
	{
		var contact: ContactPoint = collision.contacts[0];
		var newParticle		= Instantiate(explodeParticle, contact.point, Quaternion.identity);
		newParticle.transform.position.y = 0.05;
		Destroy(newParticle, 5.0);
		Destroy(gameObject);
	}
}