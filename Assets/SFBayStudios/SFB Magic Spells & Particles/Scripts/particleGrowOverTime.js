
var growth		: Vector3;

function Update(){
	transform.localScale += growth * Time.deltaTime;
}