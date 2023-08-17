using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System.Collections.Generic;
using DV;
using DV.Utils;
using DV.Simulation;
using DV.Common;
using System.Linq;
using System.Threading.Tasks;
using DV.RemoteControls;
using DV.Wheels;
using DV.Simulation.Cars;
using DV.ThingTypes;
using LocoSim.Implementations;
using static Oculus.Avatar.CAPI;
using UnityEngine.UI;
using System.ComponentModel;
using static UnityModManagerNet.UnityModManager.Param;

namespace dumb282tweaks;

public static class Main {
	// Variables
	public static UnityModManager.ModEntry Instance { get; private set; }
	public static dumb282tweaksSettings Settings { get; private set; }

	public static bool enabled;

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

	public static GameObject fullCabLoad;
	public static GameObject smokeDeflectorLoad;

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

			// Asset Loading
			var assetPath = Path.Combine(Instance.Path.ToString(), "assets\\");
			var fullCabBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, "FullCab"));
			var smokeDeflectorBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, "SmokeDeflector"));
			fullCabLoad = fullCabBundle.LoadAsset<GameObject>("Assets/fullcab.prefab");
			smokeDeflectorLoad = smokeDeflectorBundle.LoadAsset<GameObject>("Assets/SmokeDeflector.prefab");

		} catch (Exception ex) {
			Instance.Logger.LogException($"Failed to load {Instance.Info.DisplayName}:", ex);
			harmony?.UnpatchAll(Instance.Info.Id);
			return false;
		}

		return true;
	}

	// GUI Rendering
	static void OnGUI(UnityModManager.ModEntry modEntry) {
		GUILayout.BeginVertical();

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

    [HarmonyPatch(typeof(TrainCar), "Start")]
    class CarPatch {
		static void Postfix(ref TrainCar __instance) {
			if(__instance is not null && __instance.carType == TrainCarType.LocoSteamHeavy) {
				GameObject fullCab = UnityEngine.Object.Instantiate(fullCabLoad);
				fullCab.transform.parent = __instance.transform;
				fullCab.transform.localPosition = new Vector3(0, 3, 0);
				fullCab.transform.localRotation = Quaternion.identity;
				fullCab.transform.localScale = new Vector3(1, 1, 1);

				GameObject smokeDeflector = UnityEngine.Object.Instantiate(smokeDeflectorLoad);
				smokeDeflector.transform.parent = __instance.transform;
				smokeDeflector.transform.localPosition = new Vector3(0, 2.55f, 10.23f);
				smokeDeflector.transform.localRotation = Quaternion.identity;
				smokeDeflector.transform.localScale = new Vector3(1, 1, 1);
			}
		}
	}

	public enum CabType {
		[Description("Default 282 Cab")]
		Default,
		[Description("German Cab")]
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
}
