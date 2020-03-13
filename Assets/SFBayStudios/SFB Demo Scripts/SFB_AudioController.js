#pragma strict
	
var audioClips				: AudioClips[];						// Array of single use audio clips
var audioLoops				: AudioClips[];						// Array of loops
var pitchByTimescale		: boolean			= true;

private var minLoopVolume	: float				= 0.2;			// Minimum volume for loop adjusted by speed
private var maxLoopVolume	: float				= 1.0;			// Maximum volume for loop adjusted by speed
private var volumeBySpeed	: boolean			= false;		// Should we adjust volume by speed?
private var pitchBySpeed	: boolean			= false;		// Should we adjust the pitch by speed?
private var pitchMin		: float				= 1.0;			// Desired min Pitch
private var pitchMax		: float				= 1.0;			// Desired max pitch
private var desiredPitch	: float				= 1.0;			// Desired Pitch
private var desiredMin		: float				= 0.0;			// The goal minimum volume
private var desiredMax		: float				= 1.0;			// The goal maximum volume
private var volumeMin		: float				= 0.0;			// The current minimum
private var volumeMax		: float				= 1.0;			// The current maximum
private var volumeSpeed		: float				= 1.0;			// Speed modifier for volume changes
private var isWalking		: boolean			= false;		// Are we walking?
private var isFlying		: boolean			= false;		// Are we flying?
private var audioSource		: AudioSource;						// Default volume for current looped clip
private var animator		: Animator;
private var loopPlaying		: boolean			= false;

class AudioClips{
	var name			: String;
	var audioClips		: AudioClip[];
	var volume			: float				= 1.0;
	var volumeBySpeed	: boolean			= false;
	var minVolume		: float				= 0.2;
	var pitchBySpeed	: boolean			= false;
	var minPitch		: float				= 1.0;
	var maxPitch		: float				= 1.0;
}

function Start(){
	audioSource			= GetComponent.<AudioSource>();
	animator			= GetComponent.<Animator>();
}

function Update(){
	UpdatePitch();
	if (audioSource.isPlaying)
		UpdateVolume();
	//CheckLoop();
}

function CheckLoop(){
	var animatorStateInfo	= animator.GetCurrentAnimatorStateInfo(0);
	if ((animatorStateInfo.IsName("Base Layer.Ground Locomotion")) && animator.GetFloat("locomotion") != 0 && !isWalking)
	{
		isWalking		= true;
		StartLoop("Walking Loop");
	}
	if (isWalking && (!animatorStateInfo.IsName("Base Layer.Ground Locomotion") || animator.GetFloat("locomotion") == 0.0))
	{
		isWalking		= false;
	}
	if ((animatorStateInfo.IsName("Base Layer.Air Locomotion") || animatorStateInfo.IsName("Base Layer.ground to air")) && !isFlying)
	{
		isFlying		= true;
		StartLoop("Flying Loop");
	}
	if (isFlying && (!animatorStateInfo.IsName("Base Layer.Air Locomotion") 
					&& !animatorStateInfo.IsName("Base Layer.ground to air")
					&& !animatorStateInfo.IsName("Base Layer.fly dodge")
					&& !animatorStateInfo.IsName("Base Layer.fly hit")))
	{
		isFlying		= false;
	}

	//if (!isWalking && !isFlying)
	//	StopLoop();
}

function UpdateVolume(){
	if (volumeMin != desiredMin)
		volumeMin			= Mathf.MoveTowards(volumeMin, desiredMin, volumeSpeed * Time.deltaTime);
	if (volumeMax != desiredMax)
		volumeMax			= Mathf.MoveTowards(volumeMax, desiredMax, volumeSpeed * Time.deltaTime);
	audioSource.volume		= volumeMax;
	if (volumeBySpeed)
		audioSource.volume	= Mathf.Clamp(audioSource.volume * Mathf.Abs(animator.GetFloat("locomotion")), volumeMin, volumeMax);
	
	if (audioSource.volume	== 0.0)
	{
		volumeBySpeed		= false;
		audioSource.Stop();
	}
}

function UpdatePitch(){
	if (pitchBySpeed)
	{
		var pitchRangeNegative	= 1.0 - pitchMin;
		var pitchRangePositive	= pitchMax - 1.0;
		var locomotion			= animator.GetFloat("locomotion");
		if (locomotion > 0.0)
			desiredPitch 	= 1.0 + (pitchRangePositive * locomotion);
		else if (locomotion < 0.0)
			desiredPitch 	= 1.0 + (pitchRangeNegative * locomotion);
		else
			desiredPitch	= 1.0;
	}
	else
		desiredPitch	= 1.0;
	audioSource.pitch	= Mathf.MoveTowards(audioSource.pitch, desiredPitch, Time.deltaTime);
	if (pitchByTimescale)
		audioSource.pitch	= audioSource.pitch * Time.timeScale;
}

function PlayAudio(name : String){
	var id		: int 	= AudioClipID(name);
	var volume	: float	= audioClips[id].volume;
	var audioClip		= audioClips[id].audioClips[Random.Range(0,audioClips[id].audioClips.Length)];
	if (audioClips[id].volumeBySpeed)
		volume			= Mathf.Clamp(volume * Mathf.Abs(animator.GetFloat("locomotion")), audioClips[id].minVolume, audioClips[id].volume);
	audioSource.PlayClipAtPoint(audioClip, transform.position, volume);
}

function StartLoop(name : String){
	if (!loopPlaying)
	{
		var id		: int	= AudioLoopID(name);
		desiredMin			= audioLoops[id].minVolume;
		desiredMax			= audioLoops[id].volume;
		minLoopVolume		= audioLoops[id].minVolume;
		maxLoopVolume		= audioLoops[id].volume;
		volumeBySpeed		= audioLoops[id].volumeBySpeed;
		pitchBySpeed		= audioLoops[id].pitchBySpeed;
		pitchMin			= audioLoops[id].minPitch;
		pitchMax			= audioLoops[id].maxPitch;
		var audioClip		= audioLoops[id].audioClips[Random.Range(0,audioLoops[id].audioClips.Length)];
		audioSource.clip	= audioClip;
		audioSource.Play();
		loopPlaying			= true;
	}
}

function StopLoop(){
	desiredMin			= 0.0;
	desiredMax			= 0.0;
	loopPlaying			= false;
}

function AudioClipID(name : String) : int {
	for (var i : int; i < audioClips.Length; i++){
		if (audioClips[i].name == name)
			return i;
	}
}

function AudioLoopID(name : String) : int {
	for (var i : int; i < audioLoops.Length; i++){
		if (audioLoops[i].name == name)
			return i;
	}
}