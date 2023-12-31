using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using static dumb282tweaks.Settings;

namespace dumb282tweaks;

public static class Main {
	static public float objOffset = 4.887f;

	[AllowNull] public static UnityModManager.ModEntry Instance { get; private set; }
	[AllowNull] public static dumb282tweaksSettings Settings { get; private set; }

	public static readonly string[] boilerTypeTexts = new[] {
		"Default",
		"Streamlined",
		"Chonky"
	};
	public static readonly string[] cabTypeTexts = new[] {
		"Default",
		"Better",
		"German"
	};
	public static readonly string[] cowCatcherTypeTexts = new[] {
		"Default",
		"Streamlined",
		"None",
	};
	public static readonly string[] smokeBoxDoorType = new[] {
		"Default",
		"Center"
	};
	public static readonly string[] smokeDeflectorTypeTexts = new[] {
		"None",
		"Witte",
		"Wagner",
		"Chinese"
	};
	public static readonly string[] smokeStackTypeTexts = new[] {
		"Default",
		"Short",
		"Balloon",
	};

	// Base
	[AllowNull] public static GameObject baseLoad;
	// Boilers
	[AllowNull] public static GameObject defaultBoilerLoad;
	[AllowNull] public static GameObject streamlineBoilerLoad;
	[AllowNull] public static GameObject chonkyBoilerLoad;
	// Cabs
	[AllowNull] public static GameObject defaultCabLoad;
	[AllowNull] public static GameObject betterCabLoad;
	[AllowNull] public static GameObject germanCabLoad;
	// Cowcatchers
	[AllowNull] public static GameObject defaultCowCatcherLoad;
	[AllowNull] public static GameObject noCowCatcherLoad;
	[AllowNull] public static GameObject streamlinedCowCatcherLoad;
	// Interiors
	[AllowNull] public static GameObject defaultInteriorLoad;
	[AllowNull] public static GameObject betterInteriorLoad;
	// Smoke Box Door
	[AllowNull] public static GameObject defaultSmokeBoxDoorLoad;
	[AllowNull] public static GameObject centerSmokeBoxDoorLoad;
	// Smoke Deflectors
	[AllowNull] public static GameObject wagnerSmokeDeflectorsLoad;
	[AllowNull] public static GameObject witteSmokeDeflectorsLoad;
	[AllowNull] public static GameObject chineseSmokeDeflectorsLoad;
	// Smoke Stacks
	[AllowNull] public static GameObject defaultSmokeStackLoad;
	[AllowNull] public static GameObject shortSmokeStackLoad;
	[AllowNull] public static GameObject balloonSmokeStackLoad;
	// Extras
	[AllowNull] public static GameObject railingsLoad;
	[AllowNull] public static GameObject walkwayLoad;
	[AllowNull] public static GameObject frontCoverLoad;

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

			LoadAllAssets();
		} catch(Exception ex) {
			Instance.Logger.LogException($"Failed to load {Instance.Info.DisplayName}:", ex);
			harmony?.UnpatchAll(Instance.Info.Id);
			return false;
		}

		return true;
	}

	//GUI Rendering
	static void OnGUI(UnityModManager.ModEntry modEntry) {
		GUILayout.BeginVertical();

		GUILayout.Label("These settings are applied on train spawn, meaning rejoining the game will refresh all 282 locos to the settings specified here. But, if you don't unload the train, it will keep whatever settings were there previously. This is a temporary solution until I have a proper comms radio GUI implemented.");
		GUILayout.Label("Also, reloading a save will currently break things and the tweaks won't load. This isn't good.");

		GUILayout.Label("Boiler Type");
		Settings.boilerType = (BoilerType) GUILayout.SelectionGrid((int) Settings.boilerType, boilerTypeTexts, 1, "toggle");
		GUILayout.Label("Cab Type");
		Settings.cabType = (CabType) GUILayout.SelectionGrid((int) Settings.cabType, cabTypeTexts, 1, "toggle");
		GUILayout.Label("Cow Catcher Type");
		Settings.cowCatcherType = (CowCatcherType) GUILayout.SelectionGrid((int) Settings.cowCatcherType, cowCatcherTypeTexts, 1, "toggle");
		GUILayout.Label("Smoke Deflector Type");
		Settings.smokeDeflectorType = (SmokeDeflectorType) GUILayout.SelectionGrid((int) Settings.smokeDeflectorType, smokeDeflectorTypeTexts, 1, "toggle");
		GUILayout.Label("Smoke Box Door Type");
		Settings.smokeBoxDoorType = (SmokeBoxDoorType) GUILayout.SelectionGrid((int) Settings.smokeBoxDoorType, smokeBoxDoorType, 1, "toggle");
		GUILayout.Label("Smoke Stack Type");
		Settings.smokeStackType = (SmokeStackType) GUILayout.SelectionGrid((int) Settings.smokeStackType, smokeStackTypeTexts, 1, "toggle");

		GUILayout.Label("Extras");
		Settings.railings = GUILayout.Toggle(Settings.railings, "Railings");
		Settings.walkway = GUILayout.Toggle(Settings.walkway, "Walkway");
		Settings.frontCover = GUILayout.Toggle(Settings.frontCover, "Front Cover");

		GUILayout.EndVertical();
	}
	static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
		Settings.Save(modEntry);
	}

	static void LoadAllAssets() {
		// Base
		AssetBundle baseReal = LoadAssetBundle("base"); // I can't call it base because that's a c# thing or smth
		baseLoad = LoadAssetFromBundle(baseReal, "Base.prefab");
		// Boilers
		AssetBundle boilers = LoadAssetBundle("boilers");
		defaultBoilerLoad = LoadAssetFromBundle(boilers, "DefaultBoiler.prefab");
		streamlineBoilerLoad = LoadAssetFromBundle(boilers, "StreamlineBoiler.prefab");
		chonkyBoilerLoad = LoadAssetFromBundle(boilers, "ChonkyBoiler.prefab");

		// Cabs
		AssetBundle cabs = LoadAssetBundle("cabs");
		defaultCabLoad = LoadAssetFromBundle(cabs, "DefaultCab.prefab");
		betterCabLoad = LoadAssetFromBundle(cabs, "BetterCab.prefab");
		germanCabLoad = LoadAssetFromBundle(cabs, "GermanCab.prefab");
		// Cowcatchers
		AssetBundle cowCatchers = LoadAssetBundle("cowcatchers");
		defaultCowCatcherLoad = LoadAssetFromBundle(cowCatchers, "DefaultCowCatcher.prefab");
		noCowCatcherLoad = LoadAssetFromBundle(cowCatchers, "NoCowCatcher.prefab");
		//streamlinedCowCatcherLoad = LoadAssetFromBundle(cowCatchers, "StreamlinedCowCatcher.prefab");
		// Interiors
		AssetBundle interiors = LoadAssetBundle("interiors");
		defaultInteriorLoad = LoadAssetFromBundle(interiors, "DefaultInterior.prefab");
		betterInteriorLoad = LoadAssetFromBundle(interiors, "BetterInterior.prefab");
		// Smoke Box Door
		AssetBundle smokeBoxDoor = LoadAssetBundle("smokeboxdoors");
		defaultSmokeBoxDoorLoad = LoadAssetFromBundle(smokeBoxDoor, "DefaultSmokeBoxDoor.prefab");
		//centerSmokeBoxDoorLoad = LoadAssetFromBundle(smokeBoxDoor, "CenterSmokeBoxDoor.prefab");
		// Smoke Deflectors
		AssetBundle smokeDeflectors = LoadAssetBundle("smokedeflectors");
		wagnerSmokeDeflectorsLoad = LoadAssetFromBundle(smokeDeflectors, "WagnerSmokeDeflectors.prefab");
		witteSmokeDeflectorsLoad = LoadAssetFromBundle(smokeDeflectors, "WitteSmokeDeflectors.prefab");
		// chineseSmokeDeflectorsLoad = LoadAssetFromBundle("smokedeflectors", "ChineseSmokeDeflectors.prefab");
		// Smoke Stacks
		AssetBundle smokeStacks = LoadAssetBundle("smokestacks");
		defaultSmokeStackLoad = LoadAssetFromBundle(smokeStacks, "DefaultSmokeStack.prefab");
		shortSmokeStackLoad = LoadAssetFromBundle(smokeStacks, "ShortSmokeStack.prefab");
		balloonSmokeStackLoad = LoadAssetFromBundle(smokeStacks, "BalloonSmokeStack.prefab");
		// Extras
		AssetBundle extras = LoadAssetBundle("extras");
		frontCoverLoad = LoadAssetFromBundle(extras, "FrontCover.prefab");
		railingsLoad = LoadAssetFromBundle(extras, "Railings.prefab");
		walkwayLoad = LoadAssetFromBundle(extras, "Walkways.prefab");
	}

	public static AssetBundle LoadAssetBundle(string assetBundle) {
		string assetPath = Path.Combine(Instance.Path.ToString(), "assets");

		AssetBundle loadedBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, assetBundle));
		return loadedBundle;
	}

	public static GameObject LoadAssetFromBundle(AssetBundle assetBundle, String assetName) {
		GameObject loadedObject = assetBundle.LoadAsset<GameObject>("Assets/" + assetName);

		if(loadedObject == null) {
			Error("Failed to load " + assetName);
			return null;
		}

		return loadedObject;
	}

	public static GameObject InstantiateLoadedObject(GameObject toLoad, Material mat, Transform toParent) {
		GameObject obj = UnityEngine.Object.Instantiate(toLoad);

		for(int i = 0; i < obj.transform.childCount; i++) {
			obj.transform.GetChild(i).GetComponent<MeshRenderer>().material = mat;
		}

		obj.transform.parent = toParent;
		obj.transform.localPosition = new Vector3(0, 0, objOffset);
		obj.transform.localRotation = Quaternion.identity;

		return obj;
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
