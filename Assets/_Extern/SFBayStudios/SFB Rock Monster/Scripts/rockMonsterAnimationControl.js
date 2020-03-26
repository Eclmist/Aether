#pragma strict

var target			: GameObject;
var animControl		: Animator;

function Start () {
	animControl		= target.GetComponent(Animator);
}

function Death(){
	animControl.SetTrigger("death");
}

function IdleBreak(){
	animControl.SetTrigger("idleBreak");
}

function GotHit(){
	animControl.SetTrigger("gotHit");
}

function Attack1A(){
	animControl.SetTrigger("attack1A");
}

function Attack1B(){
	animControl.SetTrigger("attack1B");
}

function Attack2(){
	animControl.SetTrigger("attack2");
}

function RubbleToIdle(){
	animControl.SetTrigger("rubbleToIdle");
}

function IdleToRubble(){
	animControl.SetTrigger("idleToRubble");
}

function Magic(){
	animControl.SetTrigger("magic");
}

function Block(){
	animControl.SetTrigger("block");
}

function Jump(){
	animControl.SetTrigger("jump");
}

function LookLeft(){
	animControl.SetTrigger("lookLeft");
}

function LookRight(){
	animControl.SetTrigger("lookRight");
}

function StopBlocking(){
	animControl.SetTrigger("stopBlock");
}

function Walk(){
	animControl.SetFloat("locomotion", 1.0);
}

function WalkBackward(){
	animControl.SetFloat("locomotion", 0.0);
}

function Idle(){
	animControl.SetFloat("locomotion", 0.5);
}

function SetLocomotionSpeed(newValue : float){
	animControl.SetFloat("locomotion", newValue);
}






