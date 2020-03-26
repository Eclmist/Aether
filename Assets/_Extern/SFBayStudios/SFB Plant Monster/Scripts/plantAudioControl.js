#pragma strict

var attack1				: AudioClip[];									// Array of audio available
var attack1Level		: float				= 1.0;						// Volume Level
var attack2				: AudioClip[];									// Array of audio available
var attack2Level		: float				= 1.0;						// Volume Level
var castWarmup			: AudioClip;									
var castWarmupLevel		: float				= 1.0;						// Volume Level
var castShoot			: AudioClip[];									// Array of audio available
var castShootLevel		: float				= 1.0;						// Volume Level
var castHit				: AudioClip[];									// Array of audio available
var castHitLevel		: float				= 1.0;						// Volume Level
var goToMonster			: AudioClip[];									// Array of audio available
var goToMonsterLevel	: float				= 1.0;						// Volume Level
var death				: AudioClip[];									// Array of audio available
var deathLevel			: float				= 1.0;						// Volume Level
var folliageLoop		: AudioClip;
var folliageLoopLevel	: float				= 1.0;						// Volume Level
var gotHit				: AudioClip[];									// Array of audio available
var gotHitLevel			: float				= 1.0;						// Volume Level
var idleBreak			: AudioClip[];									// Array of audio available
var idleBreakLevel		: float				= 1.0;						// Volume Level
var jump				: AudioClip[];									// Array of audio available
var jumpLevel			: float				= 1.0;						// Volume Level
var goToPlant			: AudioClip[];									// Array of audio available
var goToPlantLevel		: float				= 1.0;						// Volume Level
var skip				: AudioClip[];									// Array of audio available
var skipLevel			: float				= 1.0;						// Volume Level
var walkBack			: AudioClip[];									// Array of audio available
var walkBackLevel		: float				= 1.0;						// Volume Level
var walkForward			: AudioClip[];									// Array of audio available
var walkForwardLevel	: float				= 1.0;						// Volume Level

var audioSource			: AudioSource;
var startAudioLevel		: float;
var desiredAudioLevel	: float				= 0.0;

var folliageTimer		: float				= 0.0;

function Start () {
	audioSource		= GetComponent.<AudioSource>();
}

function Update () {
	print ("Vol/Des: " + audioSource.volume + "/" + desiredAudioLevel);
	if (audioSource.volume >= desiredAudioLevel)
	{
		print ("Volume is greater!");
		if (startAudioLevel == 0)
			startAudioLevel = 1;
		audioSource.volume	-= Time.deltaTime * startAudioLevel;
		if (audioSource.volume < desiredAudioLevel)
			audioSource.volume	= desiredAudioLevel;
		if (audioSource.volume <= 0)
		{
			audioSource.volume	= 0;
			audioSource.Stop();
		}
	}
	else if (audioSource.volume < desiredAudioLevel)
	{
		print ("Volume is smaller!");
		audioSource.volume	= Mathf.Clamp(audioSource.volume + Time.deltaTime, 0, desiredAudioLevel);
	}

	if (folliageTimer > 0)
	{
		folliageTimer -= Time.deltaTime;
		if (folliageTimer <= 0)
			StopAudioFolliage();
	}
}

function WalkForward(stepNumber : int){
	StartAudioFolliage(0.35);
	var audioToPlay	: AudioClip		= walkForward[stepNumber];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, walkForwardLevel);
}

function WalkBackward(stepNumber : int){
	StartAudioFolliage(0.35);
	var audioToPlay	: AudioClip		= walkBack[stepNumber];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, walkBackLevel);
}

function AudioSkip(){
	StartAudioFolliage(1.0);
	var audioToPlay	: AudioClip		= skip[Random.Range(0,skip.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, skipLevel);
}

function AudioJump(){
	StartAudioFolliage(1.0);
	var audioToPlay	: AudioClip		= jump[Random.Range(0,jump.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, jumpLevel);
}

function AudioIdleBreak(){
	var audioToPlay	: AudioClip		= idleBreak[Random.Range(0,idleBreak.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, idleBreakLevel);
}

function AudioGotHit(){
	var audioToPlay	: AudioClip		= gotHit[Random.Range(0,gotHit.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, gotHitLevel);
}

function AudioDeath(){
	var audioToPlay	: AudioClip		= death[Random.Range(0,death.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, deathLevel);
}

function AudioGoToMonster(){
	var audioToPlay	: AudioClip		= goToMonster[Random.Range(0,goToMonster.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, goToMonsterLevel);
}

function AudioGoToPlant(){
	var audioToPlay	: AudioClip		= goToPlant[Random.Range(0,goToPlant.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, goToPlantLevel);
}

function AudioAttack1(){
	var audioToPlay	: AudioClip		= attack1[Random.Range(0,attack1.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, attack1Level);
}

function AudioAttack2(){
	var audioToPlay	: AudioClip		= attack2[Random.Range(0,attack2.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, attack2Level);
}

function AudioCastShoot(){
	var audioToPlay	: AudioClip		= castShoot[Random.Range(0,castShoot.Length)];
	audioSource.PlayClipAtPoint(audioToPlay, transform.position, castShootLevel);
}

function AudioWalkForward(){
	StartAudioFolliage(1.0);
	//var audioToPlay	: AudioClip		= walkForward[Random.Range(0,walkForward.Length)];
	//audioSource.PlayClipAtPoint(audioToPlay, transform.position, walkForwardLevel);
}

function AudioWalkBack(){
	StartAudioFolliage(1.0);
	//var audioToPlay	: AudioClip		= walkBack[Random.Range(0,walkBack.Length)];
	//audioSource.PlayClipAtPoint(audioToPlay, transform.position, walkBackLevel);
}

function StartAudioCastWarmup(){
	audioSource.clip	= castWarmup;
	audioSource.volume	= Mathf.Clamp(audioSource.volume, 0, castWarmupLevel);
	desiredAudioLevel	= castWarmupLevel;
	if (!audioSource.isPlaying)
		audioSource.Play();
}

function StopAudioCastWarmup(){
	startAudioLevel		= audioSource.volume;
	desiredAudioLevel	= 0.0;
}

function StartAudioFolliage(timerValue : float){
	folliageTimer		= timerValue;
	audioSource.clip	= folliageLoop;
	desiredAudioLevel	= folliageLoopLevel;
	if (!audioSource.isPlaying)
		audioSource.Play();
}

function StopAudioFolliage(){
	startAudioLevel		= audioSource.volume;
	desiredAudioLevel	= 0.0;
}


