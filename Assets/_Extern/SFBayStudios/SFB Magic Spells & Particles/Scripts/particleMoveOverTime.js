
var movement		: Vector3;
var local			: boolean	= true;

function Update(){
	if (local)
		transform.localPosition += movement * Time.deltaTime;
	else
		transform.position 		+= movement * Time.deltaTime;
}