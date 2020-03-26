#pragma strict

var target			: GameObject;
var animControl		: Animator;

var isStatue		: boolean		= false;
var isWalking		: boolean		= false;

var statuePanel		: GameObject;
var groundPanel		: GameObject;

var walkingText		: GameObject;

function Start () {
	animControl		= target.GetComponent(Animator);
}

function ToggleWalking(){
	if (isWalking)
	{
		if (isStatue)
		{
			isStatue								= false;
			animControl.SetTrigger("makeAlive");
		}
		isWalking									= false;
		walkingText.GetComponent(UI.Text).text		= "Start Walking";
		animControl.SetBool("isWalking", false);
		animControl.SetTrigger("stopWalking");
	}
	else if (!isWalking)
	{
		if (isStatue)
		{
			isStatue								= false;
			animControl.SetTrigger("makeAlive");
		}
		isWalking									= true;
		walkingText.GetComponent(UI.Text).text		= "Stop Walking";
		animControl.SetBool("isWalking", true);
		animControl.SetTrigger("startWalking");
	}
}

function Death(){
	animControl.SetTrigger("die");
}

function IdleBreak(){
	animControl.SetTrigger("idleBreak");
}

function GotHit(){
	animControl.SetTrigger("gotHit");
}

function Attack1(){
	animControl.SetTrigger("attack1");
}

function Attack2(){
	animControl.SetTrigger("attack2");
}

function Attack3(){
	animControl.SetTrigger("attack3");
}

function Statue1(){
	animControl.SetTrigger("statue1");
}

function Statue2(){
	animControl.SetTrigger("statue2");
}

function Statue3(){
	animControl.SetTrigger("statue3");
}

function Attack4(){
	animControl.SetTrigger("attack4");
}

function Block(){
	animControl.SetTrigger("block");
}

function StopAttack4(){
	animControl.SetTrigger("stopAttack4");
}

function StopBlocking(){
	animControl.SetTrigger("stopBlock");
}

function Squash(){
	animControl.SetTrigger("squash");
}

function StartBackward(){
	animControl.SetTrigger("startWalkBack");
}

function StopBackward(){
	animControl.SetTrigger("stopWalkBack");
}








