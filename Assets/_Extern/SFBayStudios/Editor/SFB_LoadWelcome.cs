using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class Startup {
	static Startup()
	{ 
		if (EditorApplication.timeSinceStartup < 10) {
			var loadedCount = EditorPrefs.GetInt("SFB_LoadedCount");
			EditorPrefs.SetInt("SFB_LoadedCount", loadedCount + 1);
			Debug.Log ("Infinity Series: Please Register, Review & Rate!\nDelete Assets/SFBayStudios/Editor/SFB_LoadWelcome.cs to stop this message.");
			Debug.Log ("Visit www.InfinityPBR.com to register your asset & get access to\nmaterials/textures, bonus downloads, pre-release content & support.");
			Debug.Log ("Please review & rate your purchase.  Your positive review let's\nothers know they can trust our work.  We can't thank you enough!");
			if (EditorPrefs.GetInt("SFB_Welcome") != 1 && EditorPrefs.GetInt("SFB_LoadedCount") % 7 == 0 )
			{
				var option = EditorUtility.DisplayDialogComplex(
					"Infinity Series: Please Register, Review & Rate!",
					"Visit www.InfinityPBR.com to register your asset for materials/textures, bonus downloads, pre-release content & support.\n\nPlease review & rate your purchase.  Your review & rating helps others know how high quality our work is!",
					"Go There Now",
					"Remind Me Later",
					"Don't Show Again");
				switch (option) {
					// Save Scene
				case 0:
					Application.OpenURL ("http://www.InfinityPBR.com/");
					break;
					// Save and Quit.
				case 1:
					break;
					// Quit Without saving.
				case 2:
					EditorPrefs.SetInt("SFB_Welcome", 1);
					break;
				default:
					Debug.LogError("Unrecognized option.");
					break;
				}
			}
		}
	}
}
