using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System.Collections.Generic;
using DV;
using UnityEngine.UI;
using System.ComponentModel;

namespace dumb282tweaks;

public static class Main {
	// Variables
	public static UnityModManager.ModEntry Instance { get; private set; }
	public static dumb282tweaksSettings Settings { get; private set; }

	private static readonly string[] cabTypeTexts = new[] {
		"Default",
		//"Soviet",
		"German"
	};

	private static readonly string[] smokeDeflectorTypeTexts = new[] {
		"Default",
		"Soviet",
		"German"
	};

	private static AssetBundle loadedAssetBundle;

	// Load
	private static bool Load(UnityModManager.ModEntry modEntry) {
		Harmony? harmony = null;

		try {
			Instance = modEntry;
			Settings = UnityModManager.ModSettings.Load<dumb282tweaksSettings>(Instance);

			Instance.OnGUI = OnGUI;
			Instance.OnSaveGUI = OnSaveGUI;

			harmony = new Harmony(Instance.Info.Id);
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			WorldStreamingInit.LoadingFinished += GameLoaded;
		} catch (Exception ex) {
			Instance.Logger.LogException($"Failed to load {Instance.Info.DisplayName}:", ex);
			harmony?.UnpatchAll(Instance.Info.Id);
			return false;
		}

		return true;
	}

	private static void GameLoaded() {
		WorldMover worldMoverScript = GameObject.Find("WorldMover").GetComponent<WorldMover>();

		// Asset Loading
		var modelPath = Path.Combine(Instance.Path.ToString(), "assets\\fullcab");
		var loadedAssetBundle = AssetBundle.LoadFromFile(modelPath);

		GameObject fullCabLoad = loadedAssetBundle.LoadAsset<GameObject>("Assets/fullcab.prefab");

		//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject fullCab = GameObject.Instantiate(fullCabLoad);
		fullCab.transform.position = PlayerManager.GetWorldAbsolutePlayerPosition() + new Vector3(0, 6, 0);
		fullCab.transform.localScale = new Vector3(1, 1, 1);

		fullCab.transform.position += WorldMover.currentMove;
		worldMoverScript.AddObjectToMove(fullCab.transform);

		// Find all GameObjects
		//foreach(GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()) {

		//}
	}

	// GUI Rendering
	static void OnGUI(UnityModManager.ModEntry modEntry) {
		GUILayout.BeginVertical();

		Settings.toggleTweaks = GUILayout.Toggle(Settings.toggleTweaks, "Toggle Tweaks");

		GUILayout.Label("Cab Type");
		Settings.cabType = (CabType)GUILayout.SelectionGrid((int)Settings.cabType, cabTypeTexts, 1, "toggle");
		GUILayout.Space(2);

		GUILayout.Label("Smoke Deflector Type");
		Settings.smokeDeflectorType = (SmokeDeflectorType) GUILayout.SelectionGrid((int) Settings.smokeDeflectorType, smokeDeflectorTypeTexts, 1, "toggle");

		GUILayout.EndVertical();
	}
	static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
		Settings.Save(modEntry);
	}

	// Logger Commands
	public static void Log(string message) {
		Instance.Logger.Log(message);
	}
	public static void Warning(string message) {
		Instance.Logger.Warning(message);
	}
	public static void Error(string message) {
		Instance.Logger.Error(message);
	}

	public enum CabType {
		[Description("Default 282 Cab")]
		Default,
		[Description("German Type Cab")]
		German
	}

	public enum SmokeDeflectorType {
		[Description("No Smoke Deflectors")]
		None,
		[Description("Soviet Smoke Deflectors")]
		Soviet,
		[Description("German Smoke Deflectors")]
		German,
	}

	public class dumb282tweaksSettings : UnityModManager.ModSettings {
		public bool toggleTweaks = true;
		public CabType cabType = CabType.German;
		public SmokeDeflectorType smokeDeflectorType = SmokeDeflectorType.Soviet;

		public override void Save(UnityModManager.ModEntry modEntry) {
			Save(this, modEntry);
		}
	}
}
