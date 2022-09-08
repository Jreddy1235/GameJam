using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Startup
{

	#if UNITY_EDITOR

	static Startup ()
	{
		PlayerSettings.keystorePass = "111111";
		PlayerSettings.keyaliasPass = "111111";
	}

	#endif
}