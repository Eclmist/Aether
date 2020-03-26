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

function Death(){
	animControl.SetTrigger("death");
}

function IdleBreak(){
	animControl.SetTrigger("idleBreak");
}

function GotHitBody(){
	animControl.SetTrigger("gotHitBody");
}

function GotHitHead(){
	animControl.SetTrigger("gotHitHead");
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

function EndBlock(){
	animControl.SetTrigger("endBlock");
}

function StartBlock(){
	animControl.SetTrigger("startBlock");
}

function Appear(){
	animControl.SetTrigger("appear");
}

function Disappear(){
	animControl.SetTrigger("disappear");
}








