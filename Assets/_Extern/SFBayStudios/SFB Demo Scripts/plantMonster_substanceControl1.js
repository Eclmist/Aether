#pragma strict

var subPanels				: GameObject[];
var object					: GameObject;
var rend					: Renderer;
var substance				: ProceduralMaterial;
var substanceFlowers		: ProceduralMaterial;
var substanceVines			: ProceduralMaterial;
var substanceThorns			: ProceduralMaterial;
var loadingPanel			: GameObject;

var groundDirtColor			: GameObject[];
var ceilingDirtColor		: GameObject[];
var mossColor				: GameObject[];
var plantColor				: GameObject[];
var mouthColor				: GameObject[];
var vinesColor				: GameObject[];
var vines1Color				: GameObject[];
var vines2Color				: GameObject[];
var leavesColor				: GameObject[];
var teethColor				: GameObject[];
var thornColor				: GameObject[];
var pollenColor				: GameObject[];
var flower1Color			: GameObject[];
var flower2Color			: GameObject[];

var colorGroundDirt			: GameObject;
var colorCeilingDirt		: GameObject;
var colorMoss				: GameObject;
var colorPlant				: GameObject;
var colorMouth				: GameObject;
var colorVines				: GameObject;
var colorVines1				: GameObject;
var colorVines2				: GameObject;
var colorLeaves				: GameObject;
var colorTeeth				: GameObject;
var colorThorn				: GameObject;
var colorPollen				: GameObject;
var colorFlower1			: GameObject;
var colorFlower2			: GameObject;

var mainBody				: GameObject;
var plantFlowers			: GameObject;
var plantVines				: GameObject;
var plantThorns				: GameObject;
var plantLeaves				: GameObject;
var plantPollens			: GameObject;
var plantTeeth				: GameObject;

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
	if (!substance.isProcessing && !substanceFlowers.isProcessing && !substanceThorns.isProcessing && !substanceVines.isProcessing)
	{
		loadingPanel.SetActive(false);
		CancelInvoke("CheckLoading");
	}
	else
	{
		print (substance.isProcessing + " | " + substanceFlowers.isProcessing + " | " + substanceThorns.isProcessing + " | " + substanceVines.isProcessing);
	}
}

function RebuildTextures(){
	substance.RebuildTextures();
	loadingPanel.SetActive(true);
	InvokeRepeating("CheckLoading", 0, 0.1);
}

function RebuildTexturesVines(){
	substanceVines.RebuildTextures();
	loadingPanel.SetActive(true);
	InvokeRepeating("CheckLoading", 0, 0.1);
}

function RebuildTexturesFlowers(){
	substanceFlowers.RebuildTextures();
	loadingPanel.SetActive(true);
	InvokeRepeating("CheckLoading", 0, 0.1);
}

function RebuildTexturesThorns(){
	substanceThorns.RebuildTextures();
	loadingPanel.SetActive(true);
	InvokeRepeating("CheckLoading", 0, 0.1);
}

function ToggleThorns(newStatus : boolean){
	plantThorns.SetActive(newStatus);
}

function ToggleVines(newStatus : boolean){
	plantVines.SetActive(newStatus);
}

function ToggleFlowers(newStatus : boolean){
	plantFlowers.SetActive(newStatus);
}

function ToggleLeaves(newStatus : boolean){
	plantLeaves.SetActive(newStatus);
}

function TogglePollens(newStatus : boolean){
	plantPollens.SetActive(newStatus);
}

function ToggleTeeth(newStatus : boolean){
	plantTeeth.SetActive(newStatus);
}

// PLANT
function PlantHue(newValue : float){
	substance.SetProceduralFloat("PlantHue", newValue);
	RebuildTextures();
}

function PlantSaturation(newValue : float){
	substance.SetProceduralFloat("PlantSaturation", newValue);
	RebuildTextures();
}

function PlantLightness(newValue : float){
	substance.SetProceduralFloat("PlantLightness", newValue);
	RebuildTextures();
}

function PlantContrast(newValue : float){
	substance.SetProceduralFloat("PlantContrast", newValue);
	RebuildTextures();
}

function PlantOverlayBlend(newValue : float){
	colorPlant.GetComponent(UI.Image).color	= Color(parseFloat(plantColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralFloat("PlantOverlayBlend", newValue);
	RebuildTextures();
}

function PlantRoughnessShift(newValue : float){
	substance.SetProceduralFloat("PlantRoughnessShift", newValue);
	RebuildTextures();
}

function PlantOverlayColor(){
	colorPlant.GetComponent(UI.Image).color	= Color(parseFloat(plantColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralColor("PlantOverlayColor", Color(parseFloat(plantColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(plantColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTextures();
}

// MOUTH
function MouthHue(newValue : float){
	substance.SetProceduralFloat("3_Hue", newValue);
	RebuildTextures();
}

function MouthSaturation(newValue : float){
	substance.SetProceduralFloat("3_Saturation", newValue);
	RebuildTextures();
}

function MouthLightness(newValue : float){
	substance.SetProceduralFloat("3_Lightness", newValue);
	RebuildTextures();
}

function MouthContrast(newValue : float){
	substance.SetProceduralFloat("3_Contrast", newValue);
	RebuildTextures();
}

function MouthOverlayBlend(newValue : float){
	colorMouth.GetComponent(UI.Image).color	= Color(parseFloat(mouthColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralFloat("3_OverlayBlend", newValue);
	RebuildTextures();
}

function MouthRoughnessShift(newValue : float){
	substance.SetProceduralFloat("3_RoughnessShift", newValue);
	RebuildTextures();
}

function MouthOverlayColor(){
	colorMouth.GetComponent(UI.Image).color	= Color(parseFloat(mouthColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralColor("3_OverlayColor", Color(parseFloat(mouthColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mouthColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTextures();
}

// PLANT VINES
function VinesHue(newValue : float){
	substance.SetProceduralFloat("VinesHue", newValue);
	RebuildTextures();
}

function VinesSaturation(newValue : float){
	substance.SetProceduralFloat("VinesSaturation", newValue);
	RebuildTextures();
}

function VinesLightness(newValue : float){
	substance.SetProceduralFloat("VinesLightness", newValue);
	RebuildTextures();
}

function VinesContrast(newValue : float){
	substance.SetProceduralFloat("VinesContrast", newValue);
	RebuildTextures();
}

function VinesOverlayBlend(newValue : float){
	colorVines.GetComponent(UI.Image).color	= Color(parseFloat(vinesColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralFloat("VinesOverlayBlend", newValue);
	RebuildTextures();
}

function VinesRoughnessShift(newValue : float){
	substance.SetProceduralFloat("VinesRoughnessShift", newValue);
	RebuildTextures();
}

function VinesOverlayColor(){
	colorVines.GetComponent(UI.Image).color	= Color(parseFloat(vinesColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralColor("VinesOverlayColor", Color(parseFloat(vinesColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(vinesColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTextures();
}

// PLANT VINES 1
function VinesHue1(newValue : float){
	substanceVines.SetProceduralFloat("3_Hue", newValue);
	RebuildTexturesVines();
}

function VinesSaturation1(newValue : float){
	substanceVines.SetProceduralFloat("3_Saturation", newValue);
	RebuildTexturesVines();
}

function VinesLightness1(newValue : float){
	substanceVines.SetProceduralFloat("3_Lightness", newValue);
	RebuildTexturesVines();
}

function VinesContrast1(newValue : float){
	substanceVines.SetProceduralFloat("3_Contrast", newValue);
	RebuildTexturesVines();
}

function VinesOverlayBlend1(newValue : float){
	substanceVines.SetProceduralFloat("3_OverlayBlend", newValue);
	RebuildTexturesVines();
}

function VinesRoughnessShift1(newValue : float){
	substanceVines.SetProceduralFloat("3_RoughnessShift", newValue);
	RebuildTexturesVines();
}

function VinesOverlayColor1(){
	colorVines1.GetComponent(UI.Image).color	= Color(parseFloat(vines1Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(vines1Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(vines1Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(vines1Color[3].GetComponent(UI.Slider).value) / 255);
	substanceVines.SetProceduralColor("3_OverlayColor", Color(parseFloat(vines1Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(vines1Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(vines1Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(vines1Color[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesVines();
}

// PLANT VINES 2
function VinesHue2(newValue : float){
	substanceVines.SetProceduralFloat("4_Hue", newValue);
	RebuildTexturesVines();
}

function VinesSaturation2(newValue : float){
	substanceVines.SetProceduralFloat("4_Saturation", newValue);
	RebuildTexturesVines();
}

function VinesLightness2(newValue : float){
	substanceVines.SetProceduralFloat("4_Lightness", newValue);
	RebuildTexturesVines();
}

function VinesContrast2(newValue : float){
	substanceVines.SetProceduralFloat("4_Contrast", newValue);
	RebuildTexturesVines();
}

function VinesOverlayBlend2(newValue : float){
	substanceVines.SetProceduralFloat("4_OverlayBlend", newValue);
	RebuildTexturesVines();
}

function VinesRoughnessShift2(newValue : float){
	substanceVines.SetProceduralFloat("4_RoughnessShift", newValue);
	RebuildTexturesVines();
}

function VinesOverlayColor2(){
	colorVines2.GetComponent(UI.Image).color	= Color(parseFloat(vines2Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(vines2Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(vines2Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(vines2Color[3].GetComponent(UI.Slider).value) / 255);
	substanceVines.SetProceduralColor("4_OverlayColor", Color(parseFloat(vines2Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(vines2Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(vines2Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(vines2Color[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesVines();
}

// PLANT LEAVES
function LeavesHue(newValue : float){
	substanceVines.SetProceduralFloat("1_Hue", newValue);
	substanceVines.SetProceduralFloat("2_Hue", newValue);
	RebuildTexturesVines();
}

function LeavesSaturation(newValue : float){
	substanceVines.SetProceduralFloat("1_Saturation", newValue);
	substanceVines.SetProceduralFloat("2_Saturation", newValue);
	RebuildTexturesVines();
}

function LeavesLightness(newValue : float){
	substanceVines.SetProceduralFloat("1_Lightness", newValue);
	substanceVines.SetProceduralFloat("2_Lightness", newValue);
	RebuildTexturesVines();
}

function LeavesContrast(newValue : float){
	substanceVines.SetProceduralFloat("1_Contrast", newValue);
	substanceVines.SetProceduralFloat("2_Contrast", newValue);
	RebuildTexturesVines();
}

function LeavesOverlayBlend(newValue : float){
	colorLeaves.GetComponent(UI.Image).color	= Color(parseFloat(leavesColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[3].GetComponent(UI.Slider).value) / 255);
	substanceVines.SetProceduralFloat("1_OverlayBlend", newValue);
	substanceVines.SetProceduralFloat("2_OverlayBlend", newValue);
	RebuildTexturesVines();
}

function LeavesRoughnessShift(newValue : float){
	substanceVines.SetProceduralFloat("1_RoughnessShift", newValue);
	substanceVines.SetProceduralFloat("2_RoughnessShift", newValue);
	RebuildTexturesVines();
}

function LeavesOverlayColor(){
	colorLeaves.GetComponent(UI.Image).color	= Color(parseFloat(leavesColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[3].GetComponent(UI.Slider).value) / 255);
	substanceVines.SetProceduralColor("1_OverlayColor", Color(parseFloat(leavesColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[3].GetComponent(UI.Slider).value) / 255));
	substanceVines.SetProceduralColor("2_OverlayColor", Color(parseFloat(leavesColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(leavesColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesVines();
}

// PLANT POLLENS
function PollenHue(newValue : float){
	substanceVines.SetProceduralFloat("1_Hue", newValue);
	substanceFlowers.SetProceduralFloat("2_Hue", newValue);
	RebuildTexturesFlowers();
}

function PollenSaturation(newValue : float){
	substanceFlowers.SetProceduralFloat("1_Saturation", newValue);
	substanceFlowers.SetProceduralFloat("2_Saturation", newValue);
	RebuildTexturesFlowers();
}

function PollenLightness(newValue : float){
	substanceFlowers.SetProceduralFloat("1_Lightness", newValue);
	substanceFlowers.SetProceduralFloat("2_Lightness", newValue);
	RebuildTexturesFlowers();
}

function PollenContrast(newValue : float){
	substanceFlowers.SetProceduralFloat("1_Contrast", newValue);
	substanceFlowers.SetProceduralFloat("2_Contrast", newValue);
	RebuildTexturesFlowers();
}

function PollenOverlayBlend(newValue : float){
	colorPollen.GetComponent(UI.Image).color	= Color(parseFloat(pollenColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[3].GetComponent(UI.Slider).value) / 255);
	substanceFlowers.SetProceduralFloat("1_OverlayBlend", newValue);
	substanceFlowers.SetProceduralFloat("2_OverlayBlend", newValue);
	RebuildTexturesFlowers();
}

function PollenRoughnessShift(newValue : float){
	substanceFlowers.SetProceduralFloat("1_RoughnessShift", newValue);
	substanceFlowers.SetProceduralFloat("2_RoughnessShift", newValue);
	RebuildTexturesFlowers();
}

function PollenOverlayColor(){
	colorPollen.GetComponent(UI.Image).color	= Color(parseFloat(pollenColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[3].GetComponent(UI.Slider).value) / 255);
	substanceFlowers.SetProceduralColor("1_OverlayColor", Color(parseFloat(pollenColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[3].GetComponent(UI.Slider).value) / 255));
	substanceFlowers.SetProceduralColor("2_OverlayColor", Color(parseFloat(pollenColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(pollenColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesFlowers();
}

// PLANT FLOWER 1
function Flower1Hue(newValue : float){
	substanceFlowers.SetProceduralFloat("3_Hue", newValue);
	substanceFlowers.SetProceduralFloat("4_Hue", newValue);
	RebuildTexturesFlowers();
}

function Flower1Saturation(newValue : float){
	substanceFlowers.SetProceduralFloat("3_Saturation", newValue);
	substanceFlowers.SetProceduralFloat("4_Saturation", newValue);
	RebuildTexturesFlowers();
}

function Flower1Lightness(newValue : float){
	substanceFlowers.SetProceduralFloat("3_Lightness", newValue);
	substanceFlowers.SetProceduralFloat("4_Lightness", newValue);
	RebuildTexturesFlowers();
}

function Flower1Contrast(newValue : float){
	substanceFlowers.SetProceduralFloat("3_Contrast", newValue);
	substanceFlowers.SetProceduralFloat("4_Contrast", newValue);
	RebuildTexturesFlowers();
}

function Flower1OverlayBlend(newValue : float){
	colorFlower1.GetComponent(UI.Image).color	= Color(parseFloat(flower1Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[3].GetComponent(UI.Slider).value) / 255);
	substanceFlowers.SetProceduralFloat("3_OverlayBlend", newValue);
	substanceFlowers.SetProceduralFloat("4_OverlayBlend", newValue);
	RebuildTexturesFlowers();
}

function Flower1RoughnessShift(newValue : float){
	substanceFlowers.SetProceduralFloat("3_RoughnessShift", newValue);
	substanceFlowers.SetProceduralFloat("4_RoughnessShift", newValue);
	RebuildTexturesFlowers();
}

function Flower1OverlayColor(){
	colorFlower1.GetComponent(UI.Image).color	= Color(parseFloat(flower1Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[3].GetComponent(UI.Slider).value) / 255);
	substanceFlowers.SetProceduralColor("3_OverlayColor", Color(parseFloat(flower1Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[3].GetComponent(UI.Slider).value) / 255));
	substanceFlowers.SetProceduralColor("4_OverlayColor", Color(parseFloat(flower1Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower1Color[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesFlowers();
}

// PLANT FLOWER 2
function Flower2Hue(newValue : float){
	substanceFlowers.SetProceduralFloat("5_Hue", newValue);
	substanceFlowers.SetProceduralFloat("6_Hue", newValue);
	RebuildTexturesFlowers();
}

function Flower2Saturation(newValue : float){
	substanceFlowers.SetProceduralFloat("5_Saturation", newValue);
	substanceFlowers.SetProceduralFloat("6_Saturation", newValue);
	RebuildTexturesFlowers();
}

function Flower2Lightness(newValue : float){
	substanceFlowers.SetProceduralFloat("5_Lightness", newValue);
	substanceFlowers.SetProceduralFloat("6_Lightness", newValue);
	RebuildTexturesFlowers();
}

function Flower2Contrast(newValue : float){
	substanceFlowers.SetProceduralFloat("5_Contrast", newValue);
	substanceFlowers.SetProceduralFloat("6_Contrast", newValue);
	RebuildTexturesFlowers();
}

function Flower2OverlayBlend(newValue : float){
	colorFlower2.GetComponent(UI.Image).color	= Color(parseFloat(flower2Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[3].GetComponent(UI.Slider).value) / 255);
	substanceFlowers.SetProceduralFloat("5_OverlayBlend", newValue);
	substanceFlowers.SetProceduralFloat("6_OverlayBlend", newValue);
	RebuildTexturesFlowers();
}

function Flower2RoughnessShift(newValue : float){
	substanceFlowers.SetProceduralFloat("5_RoughnessShift", newValue);
	substanceFlowers.SetProceduralFloat("6_RoughnessShift", newValue);
	RebuildTexturesFlowers();
}

function Flower2OverlayColor(){
	colorFlower2.GetComponent(UI.Image).color	= Color(parseFloat(flower2Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[3].GetComponent(UI.Slider).value) / 255);
	substanceFlowers.SetProceduralColor("5_OverlayColor", Color(parseFloat(flower2Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[3].GetComponent(UI.Slider).value) / 255));
	substanceFlowers.SetProceduralColor("6_OverlayColor", Color(parseFloat(flower2Color[0].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[1].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[2].GetComponent(UI.Slider).value) / 255,parseFloat(flower2Color[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesFlowers();
}


// PLANT TEETH
function TeethHue(newValue : float){
	substanceThorns.SetProceduralFloat("1_Hue", newValue);
	RebuildTexturesThorns();
}

function TeethSaturation(newValue : float){
	substanceThorns.SetProceduralFloat("1_Saturation", newValue);
	RebuildTexturesThorns();
}

function TeethLightness(newValue : float){
	substanceThorns.SetProceduralFloat("1_Lightness", newValue);
	RebuildTexturesThorns();
}

function TeethContrast(newValue : float){
	substanceThorns.SetProceduralFloat("1_Contrast", newValue);
	RebuildTexturesThorns();
}

function TeethOverlayBlend(newValue : float){
	colorTeeth.GetComponent(UI.Image).color	= Color(parseFloat(teethColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[3].GetComponent(UI.Slider).value) / 255);
	substanceThorns.SetProceduralFloat("1_OverlayBlend", newValue);
	RebuildTexturesThorns();
}

function TeethRoughnessShift(newValue : float){
	substanceThorns.SetProceduralFloat("1_RoughnessShift", newValue);
	RebuildTexturesThorns();
}

function TeethOverlayColor(){
	colorTeeth.GetComponent(UI.Image).color	= Color(parseFloat(teethColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[3].GetComponent(UI.Slider).value) / 255);
	substanceThorns.SetProceduralColor("1_OverlayColor", Color(parseFloat(teethColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(teethColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesThorns();
}


// PLANT THORNS
function ThornsHue(newValue : float){
	substanceThorns.SetProceduralFloat("3_Hue", newValue);
	substanceThorns.SetProceduralFloat("2_Hue", newValue);
	substanceThorns.SetProceduralFloat("4_Hue", newValue);
	RebuildTexturesThorns();
}

function ThornsSaturation(newValue : float){
	substanceThorns.SetProceduralFloat("3_Saturation", newValue);
	substanceThorns.SetProceduralFloat("2_Saturation", newValue);
	substanceThorns.SetProceduralFloat("4_Saturation", newValue);
	RebuildTexturesThorns();
}

function ThornsLightness(newValue : float){
	substanceThorns.SetProceduralFloat("3_Lightness", newValue);
	substanceThorns.SetProceduralFloat("2_Lightness", newValue);
	substanceThorns.SetProceduralFloat("4_Lightness", newValue);
	RebuildTexturesThorns();
}

function ThornsContrast(newValue : float){
	substanceThorns.SetProceduralFloat("3_Contrast", newValue);
	substanceThorns.SetProceduralFloat("2_Contrast", newValue);
	substanceThorns.SetProceduralFloat("4_Contrast", newValue);
	RebuildTexturesThorns();
}

function ThornsOverlayBlend(newValue : float){
	colorThorn.GetComponent(UI.Image).color	= Color(parseFloat(thornColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[3].GetComponent(UI.Slider).value) / 255);
	substanceThorns.SetProceduralFloat("3_OverlayBlend", newValue);
	substanceThorns.SetProceduralFloat("2_OverlayBlend", newValue);
	substanceThorns.SetProceduralFloat("4_OverlayBlend", newValue);
	RebuildTexturesThorns();
}

function ThornsRoughnessShift(newValue : float){
	substanceThorns.SetProceduralFloat("3_RoughnessShift", newValue);
	substanceThorns.SetProceduralFloat("2_RoughnessShift", newValue);
	substanceThorns.SetProceduralFloat("4_RoughnessShift", newValue);
	RebuildTexturesThorns();
}

function ThornsOverlayColor(){
	colorThorn.GetComponent(UI.Image).color	= Color(parseFloat(thornColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[3].GetComponent(UI.Slider).value) / 255);
	substanceThorns.SetProceduralColor("3_OverlayColor", Color(parseFloat(thornColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[3].GetComponent(UI.Slider).value) / 255));
	substanceThorns.SetProceduralColor("2_OverlayColor", Color(parseFloat(thornColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[3].GetComponent(UI.Slider).value) / 255));
	substanceThorns.SetProceduralColor("4_OverlayColor", Color(parseFloat(thornColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(thornColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTexturesThorns();
}



// GROUND DIRT
function GroundDirtLevel(newValue : float){
	substance.SetProceduralFloat("groundDirtLevel", newValue);
	substanceVines.SetProceduralFloat("groundDirtLevel", newValue);
	substanceThorns.SetProceduralFloat("groundDirtLevel", newValue);
	substanceFlowers.SetProceduralFloat("groundDirtLevel", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function GroundDirtContrast(newValue : float){
	substance.SetProceduralFloat("groundDirtContrast", newValue);
	substanceVines.SetProceduralFloat("groundDirtContrast", newValue);
	substanceThorns.SetProceduralFloat("groundDirtContrast", newValue);
	substanceFlowers.SetProceduralFloat("groundDirtContrast", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function GroundDirtHeight(newValue : float){
	substance.SetProceduralFloat("groundDirtHeight", newValue);
	substanceVines.SetProceduralFloat("groundDirtHeight", newValue);
	substanceThorns.SetProceduralFloat("groundDirtHeight", newValue);
	substanceFlowers.SetProceduralFloat("groundDirtHeight", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function GroundDirtRoughness(newValue : float){
	substance.SetProceduralFloat("groundDirtRoughness", newValue);
	substanceVines.SetProceduralFloat("groundDirtRoughness", newValue);
	substanceThorns.SetProceduralFloat("groundDirtRoughness", newValue);
	substanceFlowers.SetProceduralFloat("groundDirtRoughness", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function GroundDirtColor(){
	colorGroundDirt.GetComponent(UI.Image).color	= Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralColor("groundDirtColor", Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value) / 255));
	substanceVines.SetProceduralColor("groundDirtColor", Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value) / 255));
	substanceFlowers.SetProceduralColor("groundDirtColor", Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value) / 255));
	substanceThorns.SetProceduralColor("groundDirtColor", Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

// CEILING DIRT
function CeilingDirtLevel(newValue : float){
	substance.SetProceduralFloat("ceiingDirtLevel", newValue);
	substanceVines.SetProceduralFloat("ceiingDirtLevel", newValue);
	substanceThorns.SetProceduralFloat("ceiingDirtLevel", newValue);
	substanceFlowers.SetProceduralFloat("ceiingDirtLevel", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function CeilingDirtContrast(newValue : float){
	substance.SetProceduralFloat("ceiingDirtContrast", newValue);
	substanceVines.SetProceduralFloat("ceiingDirtContrast", newValue);
	substanceThorns.SetProceduralFloat("ceiingDirtContrast", newValue);
	substanceFlowers.SetProceduralFloat("ceiingDirtContrast", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function CeilingDirtHeight(newValue : float){
	substance.SetProceduralFloat("ceiingDirtHeight", newValue);
	substanceVines.SetProceduralFloat("ceiingDirtHeight", newValue);
	substanceThorns.SetProceduralFloat("ceiingDirtHeight", newValue);
	substanceFlowers.SetProceduralFloat("ceiingDirtHeight", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function CeilingDirtRoughness(newValue : float){
	substance.SetProceduralFloat("ceiingDirtRoughness", newValue);
	substanceVines.SetProceduralFloat("ceiingDirtRoughness", newValue);
	substanceThorns.SetProceduralFloat("ceiingDirtRoughness", newValue);
	substanceFlowers.SetProceduralFloat("ceiingDirtRoughness", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function CeilingDirtColor(){
	colorGroundDirt.GetComponent(UI.Image).color	= Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralColor("ceilingDirtColor", Color(parseFloat(ceilingDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[3].GetComponent(UI.Slider).value) / 255));
	substanceVines.SetProceduralColor("ceilingDirtColor", Color(parseFloat(ceilingDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[3].GetComponent(UI.Slider).value) / 255));
	substanceFlowers.SetProceduralColor("ceilingDirtColor", Color(parseFloat(ceilingDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[3].GetComponent(UI.Slider).value) / 255));
	substanceThorns.SetProceduralColor("ceilingDirtColor", Color(parseFloat(ceilingDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(ceilingDirtColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

// SFX
function MossColor(){
	colorGroundDirt.GetComponent(UI.Image).color	= Color(parseFloat(groundDirtColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(groundDirtColor[3].GetComponent(UI.Slider).value) / 255);
	substance.SetProceduralColor("sfxMossColor", Color(parseFloat(mossColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[3].GetComponent(UI.Slider).value) / 255));
	substanceVines.SetProceduralColor("sfxMossColor", Color(parseFloat(mossColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[3].GetComponent(UI.Slider).value) / 255));
	substanceFlowers.SetProceduralColor("sfxMossColor", Color(parseFloat(mossColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[3].GetComponent(UI.Slider).value) / 255));
	substanceThorns.SetProceduralColor("sfxMossColor", Color(parseFloat(mossColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function WaterColor(){
	substance.SetProceduralColor("SFXMossColor", Color(parseFloat(mossColor[0].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[1].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[2].GetComponent(UI.Slider).value) / 255,parseFloat(mossColor[3].GetComponent(UI.Slider).value) / 255));
	RebuildTextures();
}

function MossScale(newValue : float){
	substance.SetProceduralFloat("SFXMossScale", newValue);
	RebuildTextures();
}

function Moss(newValue : float){
	substance.SetProceduralFloat("sfxMoss", newValue);
	substanceVines.SetProceduralFloat("sfxMoss", newValue);
	substanceThorns.SetProceduralFloat("sfxMoss", newValue);
	substanceFlowers.SetProceduralFloat("sfxMoss", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
}

function Snow(newValue : float){
	substance.SetProceduralFloat("sfxSnow", newValue);
	substanceVines.SetProceduralFloat("sfxSnow", newValue);
	substanceThorns.SetProceduralFloat("sfxSnow", newValue);
	substanceFlowers.SetProceduralFloat("sfxSnow", newValue);
	RebuildTextures();
	RebuildTexturesThorns();
	RebuildTexturesVines();
	RebuildTexturesFlowers();
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









































































