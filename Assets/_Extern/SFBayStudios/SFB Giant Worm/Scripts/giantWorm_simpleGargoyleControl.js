#pragma strict

var charAnimator		: Animator;
var moveSpeed			: float			= 2.0;

function Start () {
	charAnimator		= GetComponent(Animator);
}

function Update () {
	if (Input.GetKeyDown("right") || Input.GetKeyDown("up") || Input.GetKeyDown("down"))
		charAnimator.SetBool("moving", true);
	if (Input.GetKeyUp("right") || Input.GetKeyUp("up") || Input.GetKeyUp("down"))
		charAnimator.SetBool("moving", false);
	if (Input.GetKeyDown("right"))
		charAnimator.SetTrigger("startWalking");
	if (Input.GetKeyUp("right"))
		charAnimator.SetTrigger("stopWalking");
		
		
	if (Input.GetKey("up"))
		transform.position.y = Mathf.Clamp(transform.position.y + moveSpeed * Time.deltaTime, 0, 10);
	if (Input.GetKey("down"))
		transform.position.y = Mathf.Clamp(transform.position.y + -moveSpeed * Time.deltaTime, 0, 10);
	charAnimator.SetFloat("height", transform.position.y);
	/*if (Input.GetKeyDown("right"))
	{
		transform.position	= transform.position + transform.forward * moveSpeed;
	}
	if (Input.GetKeyDown("up"))
	{
		transform.position	= transform.position + transform.up * moveSpeed;
	}
	if (Input.GetKeyDown("down"))
	{
		transform.position	= transform.position + -transform.up * moveSpeed;
	}*/
}