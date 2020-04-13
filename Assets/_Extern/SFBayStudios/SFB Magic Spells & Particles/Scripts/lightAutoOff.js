#pragma strict

var fadeLimit		: float		= 1.0;
var fadePassed		: float		= 0.0;
var fadeSpeed		: float		= 1.0;			// Multiplier of Time.deltaTime

function Start () {

}

function Update () {
	fadePassed += Time.deltaTime;
	if (fadePassed >= fadeLimit)
		FadeOut();
}

function FadeOut(){
	GetComponent(Light).intensity 	= GetComponent(Light).intensity - (Time.deltaTime * fadeSpeed);
	if (GetComponent(Light).intensity == 0)
		Destroy(this.gameObject);
}