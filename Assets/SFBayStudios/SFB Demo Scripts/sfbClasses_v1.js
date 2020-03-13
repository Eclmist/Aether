#pragma strict

import System.Collections.Generic;

public class Actions{
	var action				: Action[];
}

public class Action{
	var sourceMaterial		: ProceduralMaterial;
	var sourceName			: String;
	var targetMaterial		: ProceduralMaterial;
	var targetName			: String;
}