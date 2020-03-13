#pragma strict

var subPanels				: GameObject[];
var object					: GameObject;
var rend					: Renderer;
var substance				: ProceduralMaterial;
var loadingPanel			: GameObject;

var surfaceDirtColor		: GameObject[];
var groundDirtColor			: GameObject[];
var ceilingDirtColor		: GameObject[];
var mossColor				: GameObject[];
var bodyOverlayColor		: GameObject[];

var colorBody				: GameObject;
var colorSurfaceDirt		: GameObject;
var colorGroundDirt			: GameObject;
var colorCeilingDirt		: GameObject;
var colorMoss				: GameObject;

var mainBody				: GameObject;

function Start() {
	rend 		= object.GetComponent.<Renderer>();
	substance 	= rend.sharedMaterial as ProceduralMaterial;
	loadingPanel.SetActive(true);
	if (!IsInvoking("CheckLoading"))
		InvokeRepeating("CheckLoading", 0, 0.2);
}

function LoadSubPanel(panelNumber : int){
	for (var i : int; i < subPanels.Length; i++){
		subPanels[i].SetActive(false);
	}
	subPanels[panelNumber].SetActive(true);
}

function CloseSubPanels(){
	for (var i : int; i < subPanels.Length; i++){
		subPanels[i].SetActive(false);
	}
}

function CheckLoading(){
	//print ("CheckLoading()");
	if (!substance.isProcessing)
	{
		loadingPanel.SetActive(false);
		CancelInvoke("CheckLoading");
	}
}

function RebuildTextures(){
	substance.RebuildTextures();
	loadingPanel.SetActive(true);
	InvokeRepeating("CheckLoading", 0, 0.1);
}

// BODY
function BodyHue(newValue : float){
	//print ("BodyHue(" + newValue + ")");
	substance.SetProceduralFloat("bodyHue", newValue);
	substance.SetProceduralFloat("headSpikesHue", newValue);
	RebuildTextures();
}

function BodyContrast(newValue : float){
	substance.SetProceduralFloat("bodyContrast", newValue);
	substance.SetProceduralFloat("headSpikesContrast", newValue);
	RebuildTextures();
}

function BodySaturation(newValue : float){
	substance.SetProceduralFloat("bodySaturation", newValue);
	substance.SetProceduralFloat("headSpikesSaturation", newValue);
	RebuildTextures();
}

function BodyLightness(newValue : float){
	substance.SetProceduralFloat("bodyLightness", newValue);
	substance.SetProceduralFloat("headSpikesLightness", newValue);
	RebuildTextures();
}

function BodyOverlayColor(){
	print ("R Value: " + bodyOverlayColor[0].GetComponent(UI.Slider).value);
	substance.SetProceduralColor("bodyOverlayColor", Color(bodyOverlayColor[0].GetComponent(UI.Slider).value,bodyOverlayColor[1].GetComponent(UI.Slider).value,bodyOverlayColor[2].GetComponent(UI.Slider).value,1));
	substance.SetProceduralColor("headSpikesOverlayColor", Color(bodyOverlayColor[0].GetComponent(UI.Slider).value,bodyOverlayColor[1].GetComponent(UI.Slider).value,bodyOverlayColor[2].GetComponent(UI.Slider).value,1));
	substance.SetProceduralFloat("bodyOverlayBlend", bodyOverlayColor[3].GetComponent(UI.Slider).value);
	substance.SetProceduralFloat("headSpikesOverlayBlend", bodyOverlayColor[3].GetComponent(UI.Slider).value);
	colorBody.GetComponent(UI.Image).color		= Color(bodyOverlayColor[0].GetComponent(UI.Slider).value,bodyOverlayColor[1].GetComponent(UI.Slider).value,bodyOverlayColor[2].GetComponent(UI.Slider).value,1);
	RebuildTextures();
}

function BodyRoughnessShift(newValue : float){
	substance.SetProceduralFloat("bodyRoughnessShift", newValue);
	substance.SetProceduralFloat("headSpikesRoughnessShift", newValue);
	RebuildTextures();
}

// SURFACE DIRT
function SurfaceDirtRoughness(newValue : float){
	substance.SetProceduralFloat("surfaceDirtRoughness", newValue);
	RebuildTextures();
}

function SurfaceDirtLevel(newValue : float){
	substance.SetProceduralFloat("surfaceDirtLevel", newValue);
	RebuildTextures();
}

function SurfaceDirtContrast(newValue : float){
	substance.SetProceduralFloat("surfaceDirtContrast", newValue);
	RebuildTextures();
}

function SurfaceDirtGrunge(newValue : float){
	substance.SetProceduralFloat("surfaceDirtGrunge", newValue);
	RebuildTextures();
}

function SurfaceDirtColor(){
	substance.SetProceduralColor("surfaceDirtColor", Color(surfaceDirtColor[0].GetComponent(UI.Slider).value,surfaceDirtColor[1].GetComponent(UI.Slider).value,surfaceDirtColor[2].GetComponent(UI.Slider).value,surfaceDirtColor[3].GetComponent(UI.Slider).value));
	colorSurfaceDirt.GetComponent(UI.Image).color		= Color(surfaceDirtColor[0].GetComponent(UI.Slider).value,surfaceDirtColor[1].GetComponent(UI.Slider).value,surfaceDirtColor[2].GetComponent(UI.Slider).value,1);
	RebuildTextures();
}

// GROUND DIRT
function GroundDirtLevel(newValue : float){
	substance.SetProceduralFloat("groundDirtLevel", newValue);
	RebuildTextures();
}

function GroundDirtContrast(newValue : float){
	substance.SetProceduralFloat("groundDirtContrast", newValue);
	RebuildTextures();
}

function GroundDirtHeight(newValue : float){
	substance.SetProceduralFloat("groundDirtHeight", newValue);
	RebuildTextures();
}

function GroundDirtRoughness(newValue : float){
	substance.SetProceduralFloat("groundDirtRoughness", newValue);
	RebuildTextures();
}

function GroundDirtColor(){
	substance.SetProceduralColor("groundDirtColor", Color(groundDirtColor[0].GetComponent(UI.Slider).value,groundDirtColor[1].GetComponent(UI.Slider).value,groundDirtColor[2].GetComponent(UI.Slider).value,groundDirtColor[3].GetComponent(UI.Slider).value));
	colorGroundDirt.GetComponent(UI.Image).color		= Color(groundDirtColor[0].GetComponent(UI.Slider).value,groundDirtColor[1].GetComponent(UI.Slider).value,groundDirtColor[2].GetComponent(UI.Slider).value,1);
	RebuildTextures();
}

// CEILING DIRT
function CeilingDirtLevel(newValue : float){
	substance.SetProceduralFloat("ceilingDirtLevel", newValue);
	RebuildTextures();
}

function CeilingDirtContrast(newValue : float){
	substance.SetProceduralFloat("ceilingDirtContrast", newValue);
	RebuildTextures();
}

function CeilingDirtHeight(newValue : float){
	substance.SetProceduralFloat("ceilingDirtHeight", newValue);
	RebuildTextures();
}

function CeilingDirtRoughness(newValue : float){
	substance.SetProceduralFloat("ceilingDirtRoughness", newValue);
	RebuildTextures();
}

function CeilingDirtColor(){
	substance.SetProceduralColor("ceilingDirtColor", Color(ceilingDirtColor[0].GetComponent(UI.Slider).value,ceilingDirtColor[1].GetComponent(UI.Slider).value,ceilingDirtColor[2].GetComponent(UI.Slider).value,ceilingDirtColor[3].GetComponent(UI.Slider).value));
	colorCeilingDirt.GetComponent(UI.Image).color		= Color(ceilingDirtColor[0].GetComponent(UI.Slider).value,ceilingDirtColor[1].GetComponent(UI.Slider).value,ceilingDirtColor[2].GetComponent(UI.Slider).value,1);
	RebuildTextures();
}

// SFX
function MossColor(){
	substance.SetProceduralColor("sfxMossColor", Color(mossColor[0].GetComponent(UI.Slider).value,mossColor[1].GetComponent(UI.Slider).value,mossColor[2].GetComponent(UI.Slider).value,ceilingDirtColor[3].GetComponent(UI.Slider).value));
	colorMoss.GetComponent(UI.Image).color		= Color(mossColor[0].GetComponent(UI.Slider).value,mossColor[1].GetComponent(UI.Slider).value,mossColor[2].GetComponent(UI.Slider).value,1);
	RebuildTextures();
}

function MossScale(newValue : float){
	substance.SetProceduralFloat("sfxMossScale", newValue);
	RebuildTextures();
}

function Moss(newValue : float){
	substance.SetProceduralFloat("sfxMoss", newValue);
	RebuildTextures();
}

function Snow(newValue : float){
	substance.SetProceduralFloat("sfxSnow", newValue);
	RebuildTextures();
}

function Ice(newValue : float){
	substance.SetProceduralFloat("sfxIceLevel", newValue);
	RebuildTextures();
}

// ROCK
function RockDust(newValue : float){
	substance.SetProceduralFloat("rockDust", newValue);
	RebuildTextures();
}

function RockDirtiness(newValue : float){
	substance.SetProceduralFloat("rockDirtiness", newValue);
	RebuildTextures();
}

function RockUsedRock(newValue : float){
	substance.SetProceduralFloat("rockUsedRock", newValue);
	RebuildTextures();
}

function RockUsedDesaturation(newValue : float){
	substance.SetProceduralFloat("rockUsedDesaturation", newValue);
	RebuildTextures();
}

function RockUsedBrightness(newValue : float){
	substance.SetProceduralFloat("rockUsedBrightness", newValue);
	RebuildTextures();
}

function RockCracksScale(newValue : float){
	substance.SetProceduralFloat("rockCracksScale", newValue);
	RebuildTextures();
}

function RockCracksIntensity(newValue : float){
	substance.SetProceduralFloat("rockCracksIntensity", newValue);
	RebuildTextures();
}

function RockAge(newValue : float){
	substance.SetProceduralFloat("rockAge", newValue);
	RebuildTextures();
}

function RockAgeThreshold(newValue : float){
	substance.SetProceduralFloat("rockAgeThreshold", newValue);
	RebuildTextures();
}

// EDGE DAMAGE
function EdgeDamageLevel(newValue : float){
	substance.SetProceduralFloat("edgeDamageLevel", newValue);
	RebuildTextures();
}

function EdgeDamageNotchLevel(newValue : float){
	substance.SetProceduralFloat("edgeDamageNotchLevel", newValue);
	RebuildTextures();
}

function EdgeDamageContrast(newValue : float){
	substance.SetProceduralFloat("edgeDamageContrast", newValue);
	RebuildTextures();
}

function EdgeDamageNotchContrast(newValue : float){
	substance.SetProceduralFloat("edgeDamageNotchContrast", newValue);
	RebuildTextures();
}

function EdgeDamageIntensity(newValue : float){
	substance.SetProceduralFloat("edgeDamageIntensity", newValue);
	RebuildTextures();
}

function EdgeDamageHue(newValue : float){
	substance.SetProceduralFloat("edgeDamageHue", newValue);
	RebuildTextures();
}

function EdgeDamageSaturation(newValue : float){
	substance.SetProceduralFloat("edgeDamageSaturation", newValue);
	RebuildTextures();
}

function EdgeDamageLightness(newValue : float){
	substance.SetProceduralFloat("edgeDamageLightness", newValue);
	RebuildTextures();
}

function EdgeDamageRoughnessShift(newValue : float){
	substance.SetProceduralFloat("edgeDamageRoughnessShift", newValue);
	RebuildTextures();
}






































































