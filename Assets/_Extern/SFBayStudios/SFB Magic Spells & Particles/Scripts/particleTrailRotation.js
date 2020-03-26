
var rotationSpeed	: Vector3;
var local			: boolean	= true;

function Update(){
	if (local)
		transform.Rotate(rotationSpeed * Time.deltaTime);
	else
		transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
}