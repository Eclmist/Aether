#pragma strict

var subPanels				: GameObject[];
var object					: GameObject;
var rend					: Renderer;
var substance				: ProceduralMaterial;
var loadingPanel			: GameObject;

var baseCount				: int						= 6;
var baseTotal				: int						= 14;
var stampCount				: int						= 2;
var stampTotal				: int						= 5;

var grungeColor				: GameObject[];
var grunge2Color			: GameObject[];
var groundDirtColor			: GameObject[];
var ceilingDirtColor		: GameObject[];
var waterColor				: GameObject[];
var mossColor				: GameObject[];

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
	print ("CheckLoading()");
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
function LoadTop(newMaterial : int){
	print ("LoadTop(" + newMaterial + ")");
	if (newMaterial == 0)
		substance.SetProceduralBoolean("UseDefaultTop", true);
	else
	{
		print ("Do false");
		substance.SetProceduralBoolean("UseDefaultTop", false);
		substance.SetProceduralFloat("TopMaterialNumber", newMaterial);
	}
	RebuildTextures();
}

function LoadBottom(newMaterial : int){
	if (newMaterial == 0)
		substance.SetProceduralBoolean("UseDefaultBottom", true);
	else
	{
		substance.SetProceduralBoolean("UseDefaultBottom", false);
		substance.SetProceduralFloat("BottomMaterialNumber", newMaterial);
	}
	RebuildTextures();
}

// STEM
function StemHue(newValue : float){
	substance.SetProceduralFloat("MushroomStemHue", newValue);
	RebuildTextures();
}

function StemContrast(newValue : float){
	substance.SetProceduralFloat("StemContrast", newValue);
	RebuildTextures();
}

function StemSaturation(newValue : float){
	substance.SetProceduralFloat("MushroomStemSaturation", newValue);
	RebuildTextures();
}

function StemLightness(newValue : float){
	substance.SetProceduralFloat("MushroomStemLightness", newValue);
	RebuildTextures();
}

function StemRoughness(newValue : float){
	substance.SetProceduralFloat("MushroomStemRoughness", newValue);
	RebuildTextures();
}

// BUMPS
function BumpsHue(newValue : float){
	substance.SetProceduralFloat("HeadBumpsHue", newValue);
	RebuildTextures();
}

function BumpsContrast(newValue : float){
	substance.SetProceduralFloat("HeadBumpsContrast", newValue);
	RebuildTextures();
}

function BumpsSaturation(newValue : float){
	substance.SetProceduralFloat("HeadBumpsSaturation", newValue);
	RebuildTextures();
}

function BumpsLightness(newValue : float){
	substance.SetProceduralFloat("HeadBumpsLightness", newValue);
	RebuildTextures();
}

function BumpsRoughness(newValue : float){
	substance.SetProceduralFloat("HeadBumpsRoughness", newValue);
	RebuildTextures();
}

// HEAD
function HeadHue(newValue : float){
	substance.SetProceduralFloat("HeadHue", newValue);
	RebuildTextures();
}

function HeadContrast(newValue : float){
	substance.SetProceduralFloat("HeadContrast", newValue);
	RebuildTextures();
}

function HeadSaturation(newValue : float){
	substance.SetProceduralFloat("HeadSaturation", newValue);
	RebuildTextures();
}

function HeadLightness(newValue : float){
	substance.SetProceduralFloat("HeadLightness", newValue);
	RebuildTextures();
}

function HeadRoughness(newValue : float){
	substance.SetProceduralFloat("HeadRoughness", newValue);
	RebuildTextures();
}

// GRUNGE
function GrungeBalance(newValue : float){
	substance.SetProceduralFloat("GrungeBalance", newValue);
	RebuildTextures();
}

function GrungeLevel(newValue : float){
	substance.SetProceduralFloat("GrungeLevel", newValue);
	RebuildTextures();
}

function GrungeContrast(newValue : float){
	substance.SetProceduralFloat("GrungeContrast", newValue);
	RebuildTextures();
}

function GrungeRoughness(newValue : float){
	substance.SetProceduralFloat("GrungeRoughness", newValue);
	RebuildTextures();
}

function GrungeColor(){
	print ("Color: " + parseFloat(grungeColor[0].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(grungeColor[1].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(grungeColor[2].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(grungeColor[3].GetComponent(UI.Text).text));
	substance.SetProceduralColor("GrungeColor", Color(parseFloat(grungeColor[0].GetComponent(UI.Text).text) / 255,parseFloat(grungeColor[1].GetComponent(UI.Text).text) / 255,parseFloat(grungeColor[2].GetComponent(UI.Text).text) / 255,parseFloat(grungeColor[3].GetComponent(UI.Text).text) / 255));
	RebuildTextures();
}

// GRUNGE 2
function Grunge2Balance(newValue : float){
	substance.SetProceduralFloat("Grunge2Balance", newValue);
	RebuildTextures();
}

function Grunge2Volume(newValue : float){
	substance.SetProceduralFloat("Grunge2Volume", newValue);
	RebuildTextures();
}

function Grunge2Contrast(newValue : float){
	substance.SetProceduralFloat("Grunge2Contrast", newValue);
	RebuildTextures();
}

function Grunge2Roughness(newValue : float){
	substance.SetProceduralFloat("Grunge2Roughness", newValue);
	RebuildTextures();
}

function Grunge2OffsetX(newValue : float){
	var currentOffset	: Vector2	= substance.GetProceduralVector("Grunge2Offset");
	substance.SetProceduralVector("Grunge2Offset", Vector2(newValue,currentOffset[1]));
	RebuildTextures();
}

function Grunge2OffsetY(newValue : float){
	var currentOffset	: Vector2	= substance.GetProceduralVector("Grunge2Offset");
	substance.SetProceduralVector("Grunge2Offset", Vector2(currentOffset[0],newValue));
	RebuildTextures();
}

function Grunge2Rotation(newValue : float){
	substance.SetProceduralFloat("Grunge2Rotation", newValue);
	RebuildTextures();
}

function Grunge2Color(){
	print ("Color: " + parseFloat(grunge2Color[0].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(grunge2Color[1].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(grunge2Color[2].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(grunge2Color[3].GetComponent(UI.Text).text));
	substance.SetProceduralColor("Grunge2Color", Color(parseFloat(grunge2Color[0].GetComponent(UI.Text).text) / 255,parseFloat(grunge2Color[1].GetComponent(UI.Text).text) / 255,parseFloat(grunge2Color[2].GetComponent(UI.Text).text) / 255,parseFloat(grunge2Color[3].GetComponent(UI.Text).text) / 255));
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
	print ("Color: " + parseFloat(groundDirtColor[0].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(groundDirtColor[1].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(groundDirtColor[2].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(groundDirtColor[3].GetComponent(UI.Text).text));
	substance.SetProceduralColor("GroundDirtColor", Color(parseFloat(groundDirtColor[0].GetComponent(UI.Text).text) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Text).text) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Text).text) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Text).text) / 255));
	RebuildTextures();
}

// CEILING DIRT
function CeilingDirtLevel(newValue : float){
	substance.SetProceduralFloat("CeilingDirtLevel", newValue);
	RebuildTextures();
}

function CeilingDirtContrast(newValue : float){
	substance.SetProceduralFloat("CeilingDirtContrast", newValue);
	RebuildTextures();
}

function CeilingDirtHeight(newValue : float){
	substance.SetProceduralFloat("CeilingDirtHeight", newValue);
	RebuildTextures();
}

function CeilingDirtRoughness(newValue : float){
	substance.SetProceduralFloat("CeilingDirtRoughness", newValue);
	RebuildTextures();
}

function CeilingDirtColor(){
	print ("Color: " + parseFloat(ceilingDirtColor[0].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(ceilingDirtColor[1].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(ceilingDirtColor[2].GetComponent(UI.Text).text));
	print ("Color: " + parseFloat(ceilingDirtColor[3].GetComponent(UI.Text).text));
	substance.SetProceduralColor("CeilingDirtColor", Color(parseFloat(ceilingDirtColor[0].GetComponent(UI.Text).text) / 255,parseFloat(ceilingDirtColor[1].GetComponent(UI.Text).text) / 255,parseFloat(ceilingDirtColor[2].GetComponent(UI.Text).text) / 255,parseFloat(ceilingDirtColor[3].GetComponent(UI.Text).text) / 255));
	RebuildTextures();
}

// SFX
function MossColor(){
	substance.SetProceduralColor("SFXWaterColor", Color(parseFloat(waterColor[0].GetComponent(UI.Text).text) / 255,parseFloat(waterColor[1].GetComponent(UI.Text).text) / 255,parseFloat(waterColor[2].GetComponent(UI.Text).text) / 255,parseFloat(waterColor[3].GetComponent(UI.Text).text) / 255));
	RebuildTextures();
}

function WaterColor(){
	substance.SetProceduralColor("SFXMossColor", Color(parseFloat(mossColor[0].GetComponent(UI.Text).text) / 255,parseFloat(mossColor[1].GetComponent(UI.Text).text) / 255,parseFloat(mossColor[2].GetComponent(UI.Text).text) / 255,parseFloat(mossColor[3].GetComponent(UI.Text).text) / 255));
	RebuildTextures();
}

function MossScale(newValue : float){
	substance.SetProceduralFloat("SFXMossScale", newValue);
	RebuildTextures();
}

function Moss(newValue : float){
	substance.SetProceduralFloat("SFXMoss", newValue);
	RebuildTextures();
}

function Snow(newValue : float){
	substance.SetProceduralFloat("SFXSnow", newValue);
	RebuildTextures();
}

function Ice(newValue : float){
	substance.SetProceduralFloat("SFXIce", newValue);
	RebuildTextures();
}

function WaterLevel(newValue : float){
	substance.SetProceduralFloat("SFXWaterLevel", newValue);
	RebuildTextures();
}

function WaterDetails(newValue : float){
	substance.SetProceduralFloat("SFXWaterDetails", newValue);
	RebuildTextures();
}

function IceDetails(newValue : float){
	substance.SetProceduralFloat("SFXIceDetails", newValue);
	RebuildTextures();
}

function Refraction(newValue : float){
	substance.SetProceduralFloat("SFXRefraction", newValue);
	RebuildTextures();
}

function Reflection(newValue : float){
	substance.SetProceduralFloat("SFXReflection", newValue);
	RebuildTextures();
}

function ReflectionDistance(newValue : float){
	substance.SetProceduralFloat("SFXReflectionDistance", newValue);
	RebuildTextures();
}

function FlowDirection(newValue : float){
	substance.SetProceduralFloat("SFXFlowDirection", newValue);
	RebuildTextures();
}









































































