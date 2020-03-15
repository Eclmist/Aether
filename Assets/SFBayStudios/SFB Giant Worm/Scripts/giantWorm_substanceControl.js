#pragma strict

var subPanels				: GameObject[];
var object					: GameObject;
var rend					: Renderer;
var substance				: ProceduralMaterial;
var loadingPanel			: GameObject;

var headColor				: GameObject[];
var carapaceColor			: GameObject[];
var undersideColor			: GameObject[];
var spikesColor				: GameObject[];
var groundDirtColor			: GameObject[];
var surfaceDirtColor		: GameObject[];

var colorDisplayHead		: GameObject;
var colorDisplayCarapace	: GameObject;
var colorDisplayUnderside	: GameObject;
var colorDisplaySpikes		: GameObject;
var colorDisplayGroundDirt	: GameObject;
var colorDisplaySurfaceDirt	: GameObject;

var metalHead				: GameObject;
var metalCarapace			: GameObject;
var metalSpikes				: GameObject;
var metalUnderside			: GameObject;

var tentacles				: GameObject;
var spikes					: GameObject;

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


// BASE MATERIALS
function MetalHead(newValue : boolean){
	substance.SetProceduralBoolean("MetalHead", newValue);
	RebuildTextures();
}

function MetalCarapace(newValue : boolean){
	substance.SetProceduralBoolean("MetalCarapace", newValue);
	RebuildTextures();
}

function MetalUnderside(newValue : boolean){
	substance.SetProceduralBoolean("MetalUnderside", newValue);
	RebuildTextures();
}

function MetalSpikes(newValue : boolean){
	substance.SetProceduralBoolean("MetalSpikes", newValue);
	RebuildTextures();
}

function Tentacles(newValue : boolean){
	tentacles.SetActive(newValue);
}

function Spikes(newValue : boolean){
	spikes.SetActive(newValue);
}

// HEAD
function HeadColor(){
	var newColor	= Color(parseFloat(headColor[0].GetComponent(UI.Slider).value),parseFloat(headColor[1].GetComponent(UI.Slider).value),parseFloat(headColor[2].GetComponent(UI.Slider).value),parseFloat(headColor[3].GetComponent(UI.Slider).value));
	substance.SetProceduralColor("HeadColor", newColor);
	colorDisplayHead.GetComponent(UI.Image).color	= newColor;
	RebuildTextures();
}

function HeadContrast(newValue : float){
	substance.SetProceduralFloat("HeadContrast", newValue);
	RebuildTextures();
}

function HeadRoughness(newValue : float){
	substance.SetProceduralFloat("HeadRoughness", newValue);
	RebuildTextures();
}

// CARAPACE
function CarapaceColor(){
	var newColor	= Color(parseFloat(carapaceColor[0].GetComponent(UI.Slider).value),parseFloat(carapaceColor[1].GetComponent(UI.Slider).value),parseFloat(carapaceColor[2].GetComponent(UI.Slider).value),parseFloat(carapaceColor[3].GetComponent(UI.Slider).value));
	substance.SetProceduralColor("CarapaceColor", newColor);
	colorDisplayCarapace.GetComponent(UI.Image).color	= newColor;
	RebuildTextures();
}

function CarapaceContrast(newValue : float){
	substance.SetProceduralFloat("CarapaceContrast", newValue);
	RebuildTextures();
}

function CarapaceRoughness(newValue : float){
	substance.SetProceduralFloat("CarapaceRoughness", newValue);
	RebuildTextures();
}

// UNDERSIDE
function UndersideColor(){
	var newColor	= Color(parseFloat(undersideColor[0].GetComponent(UI.Slider).value),parseFloat(undersideColor[1].GetComponent(UI.Slider).value),parseFloat(undersideColor[2].GetComponent(UI.Slider).value),parseFloat(undersideColor[3].GetComponent(UI.Slider).value));
	substance.SetProceduralColor("UndersideColor", newColor);
	colorDisplayUnderside.GetComponent(UI.Image).color	= newColor;
	RebuildTextures();
}

function UndersideContrast(newValue : float){
	substance.SetProceduralFloat("UndersideContrast", newValue);
	RebuildTextures();
}

function UndersideRoughness(newValue : float){
	substance.SetProceduralFloat("UndersideRoughness", newValue);
	RebuildTextures();
}

// SPIKES
function SpikesColor(){
	var newColor	= Color(parseFloat(spikesColor[0].GetComponent(UI.Slider).value),parseFloat(spikesColor[1].GetComponent(UI.Slider).value),parseFloat(spikesColor[2].GetComponent(UI.Slider).value),parseFloat(spikesColor[3].GetComponent(UI.Slider).value));
	substance.SetProceduralColor("SpikesColor", newColor);
	colorDisplaySpikes.GetComponent(UI.Image).color	= newColor;
	RebuildTextures();
}

function SpikesContrast(newValue : float){
	substance.SetProceduralFloat("SpikesContrast", newValue);
	RebuildTextures();
}

function SpikesRoughness(newValue : float){
	substance.SetProceduralFloat("SpikesRoughness", newValue);
	RebuildTextures();
}

// SKIN
function SkinHue(newValue : float){
	substance.SetProceduralFloat("SkinHue", newValue);
	RebuildTextures();
}

function SkinSaturation(newValue : float){
	substance.SetProceduralFloat("SkinSaturation", newValue);
	RebuildTextures();
}

function SkinLightness(newValue : float){
	substance.SetProceduralFloat("SkinLightness", newValue);
	RebuildTextures();
}

function SkinRoughness(newValue : float){
	substance.SetProceduralFloat("SkinRoughness", newValue);
	RebuildTextures();
}

function SkinContrast(newValue : float){
	substance.SetProceduralFloat("SkinContrast", newValue);
	RebuildTextures();
}

// MOUTH
function MouthHue(newValue : float){
	substance.SetProceduralFloat("MouthHue", newValue);
	RebuildTextures();
}

function MouthSaturation(newValue : float){
	substance.SetProceduralFloat("MouthSaturation", newValue);
	RebuildTextures();
}

function MouthLightness(newValue : float){
	substance.SetProceduralFloat("MouthLightness", newValue);
	RebuildTextures();
}

function MouthRoughness(newValue : float){
	substance.SetProceduralFloat("MouthRoughness", newValue);
	RebuildTextures();
}

function MouthContrast(newValue : float){
	substance.SetProceduralFloat("MouthContrast", newValue);
	RebuildTextures();
}


// TENTACLES
function TentaclesHue(newValue : float){
	substance.SetProceduralFloat("TentaclesHue", newValue);
	RebuildTextures();
}

function TentaclesSaturation(newValue : float){
	substance.SetProceduralFloat("TentaclesSaturation", newValue);
	RebuildTextures();
}

function TentaclesLightness(newValue : float){
	substance.SetProceduralFloat("TentaclesLightness", newValue);
	RebuildTextures();
}

function TentaclesRoughness(newValue : float){
	substance.SetProceduralFloat("TentaclesRoughness", newValue);
	RebuildTextures();
}

function TentaclesContrast(newValue : float){
	substance.SetProceduralFloat("TentaclesContrast", newValue);
	RebuildTextures();
}

// GROUND DIRT
function GroundDirtLevel(newValue : float){
	substance.SetProceduralFloat("GroundDirtLevel", newValue);
	RebuildTextures();
}

function GroundDirtContrast(newValue : float){
	substance.SetProceduralFloat("GroundDirtContrast", newValue);
	RebuildTextures();
}

function GroundDirtHeight(newValue : float){
	substance.SetProceduralFloat("GroundDirtHeight", newValue);
	RebuildTextures();
}

function GroundDirtRoughness(newValue : float){
	substance.SetProceduralFloat("GroundDirtRoughness", newValue);
	RebuildTextures();
}

function GroundDirtColor(){
	var newColor	= Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value),parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value),parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value),parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value));
	substance.SetProceduralColor("GroundDirtColor", newColor);
	colorDisplayGroundDirt.GetComponent(UI.Image).color	= newColor;
	RebuildTextures();
}

// SURFACE DIRT
function DirtLevel(newValue : float){
	substance.SetProceduralFloat("DirtLevel", newValue);
	RebuildTextures();
}

function DirtContrast(newValue : float){
	substance.SetProceduralFloat("DirtContrast", newValue);
	RebuildTextures();
}

function DirtGrungeAmount(newValue : float){
	substance.SetProceduralFloat("DirtGrungeAmount", newValue);
	RebuildTextures();
}

function DirtRoughness(newValue : float){
	substance.SetProceduralFloat("DirtRoughness", newValue);
	RebuildTextures();
}

function DirtColor(){
	var newColor	= Color(parseFloat(surfaceDirtColor[0].GetComponent(UI.Slider).value),parseFloat(surfaceDirtColor[1].GetComponent(UI.Slider).value),parseFloat(surfaceDirtColor[2].GetComponent(UI.Slider).value),parseFloat(surfaceDirtColor[3].GetComponent(UI.Slider).value));
	substance.SetProceduralColor("DirtColor", newColor);
	colorDisplaySurfaceDirt.GetComponent(UI.Image).color	= newColor;
	RebuildTextures();
}



































































