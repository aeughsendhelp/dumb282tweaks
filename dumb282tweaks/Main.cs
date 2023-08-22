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
using static dumb282tweaks.Main;

namespace dumb282tweaks;

public static class Main {
	// Variables
	public static UnityModManager.ModEntry Instance { get; private set; }
	public static dumb282tweaksSettings Settings { get; private set; }

	public static bool enabled;

	private static readonly string[] cabTypeTexts = new[] {
		"Default",
		"German"
	};
	public enum CabType {
		[Description("Default 282 Cab")]
		Default,
		[Description("German Cab")]
		German
	}

	private static readonly string[] smokeDeflectorTypeTexts = new[] {
		"None",
		"Witte",
		"Wagner"
	};
	public enum SmokeDeflectorType {
		[Description("No Smoke Deflectors")]
		None,
		[Description("Witte Smoke Deflectors")]
		Witte,
		[Description("Wagner Smoke Deflectors")]
		Wagner,
	}

	private static readonly string[] boilerTypeTexts = new[] {
		"Default",
		"Streamlined"
	};
	public enum BoilerType {
		[Description("Default Boiler")]
		Default,
		[Description("Streamlined Boiler")]
		Streamlined,
	}
	public class dumb282tweaksSettings : UnityModManager.ModSettings {
		public bool toggleTweaks = true;
		public CabType cabType = CabType.Default;
		public SmokeDeflectorType smokeDeflectorType = SmokeDeflectorType.Wagner;
		public BoilerType boilerType = BoilerType.Default;

		public override void Save(UnityModManager.ModEntry modEntry) {
			Save(this, modEntry);
		}
	}

	public static string assetPath;
	//public static GameObject germanCabLoad;
	public static GameObject wagnerSmokeDeflectorsLoad; 
	public static GameObject witteSmokeDeflectorsLoad;
	public static GameObject streamlinedBoilerLoad;

	// Load
	private static bool Load(UnityModManager.ModEntry modEntry) {
		Harmony? harmony = null;

		try {
			Instance = modEntry;
			Settings = UnityModManager.ModSettings.Load<dumb282tweaksSettings>(Instance);

			Instance.OnGUI = OnGUI;
			Instance.OnSaveGUI = OnSaveGUI;
			modEntry.Logger.Log("gam");

			harmony = new Harmony(Instance.Info.Id);
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			modEntry.Logger.Log("gamer");

			assetPath = Path.Combine(Instance.Path.ToString(), "assets\\");
			// Smoke Deflectors
			var wagnerSmokeDeflectorsBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, "wagnersmokedeflectors"));
			wagnerSmokeDeflectorsLoad = wagnerSmokeDeflectorsBundle.LoadAsset<GameObject>("Assets/WagnerSmokeDeflectors.prefab");
			var witteSmokeDeflectorsBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, "wittesmokedeflectors"));
			witteSmokeDeflectorsLoad = witteSmokeDeflectorsBundle.LoadAsset<GameObject>("Assets/WitteSmokeDeflectors.prefab");

			// Boiler
			var streamlinedBoilerBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, "streamline"));
			streamlinedBoilerLoad = streamlinedBoilerBundle.LoadAsset<GameObject>("Assets/Streamline.prefab");
		} catch(Exception ex) {
			Instance.Logger.LogException($"Failed to load {Instance.Info.DisplayName}:", ex);
			harmony?.UnpatchAll(Instance.Info.Id);
			return false;
		}

		return true;
	}

	// GUI Rendering
	static void OnGUI(UnityModManager.ModEntry modEntry) {
		GUILayout.BeginVertical();

		GUILayout.Label("These settings are applied on train spawn, meaning rejoining the game will refresh all 282 locos to the settings specified here, but if you don't unload the train it will keep whatever settings were there previously. This is a temporary solution until I have a proper GUI implemented.");

		GUILayout.Label("Cab Type");
		Settings.cabType = (CabType)GUILayout.SelectionGrid((int) Settings.cabType, cabTypeTexts, 1, "toggle");
		GUILayout.Space(2);

		GUILayout.Label("Smoke Deflector Type");
		Settings.smokeDeflectorType = (SmokeDeflectorType) GUILayout.SelectionGrid((int) Settings.smokeDeflectorType, smokeDeflectorTypeTexts, 1, "toggle");

		GUILayout.Label("Boiler Type");
		Settings.boilerType = (BoilerType) GUILayout.SelectionGrid((int) Settings.boilerType, boilerTypeTexts, 1, "toggle");


		GUILayout.EndVertical();
	}
	static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
		Settings.Save(modEntry);
	}


	[HarmonyPatch(typeof(TrainCar), "Start")]
	class CarPatch {
		static void Postfix(ref TrainCar __instance) {
			//switch(Settings.cabType) {
			//	case CabType.Default:
			//		break;
			//	case CabType.German:
			//		if(__instance != null && __instance.carType == TrainCarType.LocoSteamHeavy) {

			//			var fullCabBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, "fullcab"));
			//			var fullCabLoad = fullCabBundle.LoadAsset<GameObject>("Assets/FullCab.prefab");

			//			GameObject fullCab = UnityEngine.Object.Instantiate(fullCabLoad);
			//			fullCab.transform.parent = __instance.transform;
			//			fullCab.transform.localPosition = new Vector3(0, 3, 0);
			//			fullCab.transform.localRotation = Quaternion.identity;
			//		}
			//		break;
			//}
			// Smoke Deflector Type
			if(__instance != null && __instance.carType == TrainCarType.LocoSteamHeavy) {
				// This is a terrible way to get the material of the locomotive since a simple gameobject reordering will break it, but it's alright for now. If something breaks, just note that this is likely the reason
				Material s282Mat = __instance.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().material;

				Log(Path.Combine(assetPath, "wittesmokedeflectors"));

				switch(Settings.smokeDeflectorType) {
					case SmokeDeflectorType.None:
						break;
					case SmokeDeflectorType.Witte:
						GameObject witteSmokeDeflector = UnityEngine.Object.Instantiate(witteSmokeDeflectorsLoad);

						// This is also a terrible way to set the material of the smoke deflectors, but oh well
						witteSmokeDeflector.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
						witteSmokeDeflector.transform.GetChild(1).GetComponent<MeshRenderer>().material = s282Mat;

						//Log(smokeDeflector.GetComponent<MeshRenderer>().material.name.ToString());
						witteSmokeDeflector.transform.parent = __instance.transform;
						witteSmokeDeflector.transform.localPosition = new Vector3(0, 2.1f, 5f);
						witteSmokeDeflector.transform.localRotation = Quaternion.identity;
						break;
					case SmokeDeflectorType.Wagner:
						GameObject wagnerSmokeDeflector = UnityEngine.Object.Instantiate(wagnerSmokeDeflectorsLoad);

						// This is also a terrible way to set the material of the smoke deflectors, but oh well
						wagnerSmokeDeflector.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
						wagnerSmokeDeflector.transform.GetChild(1).GetComponent<MeshRenderer>().material = s282Mat;

						//Log(smokeDeflector.GetComponent<MeshRenderer>().material.name.ToString());
						wagnerSmokeDeflector.transform.parent = __instance.transform;
						wagnerSmokeDeflector.transform.localPosition = new Vector3(0, 2.1f, 5f);
						wagnerSmokeDeflector.transform.localRotation = Quaternion.identity;
						break;
				}

				switch(Settings.boilerType) {
					case BoilerType.Default:
						break;
					case BoilerType.Streamlined:
						GameObject streamlinedBoiler = UnityEngine.Object.Instantiate(streamlinedBoilerLoad);

						// This is also a terrible way to set the material of the smoke deflectors, but oh well
						streamlinedBoiler.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
						streamlinedBoiler.transform.GetChild(1).GetComponent<MeshRenderer>().material = s282Mat;
						streamlinedBoiler.transform.GetChild(2).GetComponent<MeshRenderer>().material = s282Mat;
						streamlinedBoiler.transform.GetChild(3).GetComponent<MeshRenderer>().material = s282Mat;

						//Log(smokeDeflector.GetComponent<MeshRenderer>().material.name.ToString());
						streamlinedBoiler.transform.parent = __instance.transform;
						streamlinedBoiler.transform.localPosition = new Vector3(0, 2.15f, 5.1f);
						streamlinedBoiler.transform.localRotation = Quaternion.identity;
						break;
				}
			}
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
