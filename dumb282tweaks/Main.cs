using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace dumb282tweaks;

public static class Main {
	// Variables
	[AllowNull] public static UnityModManager.ModEntry Instance { get; private set; }
	[AllowNull] public static dumb282tweaksSettings Settings { get; private set; }


	public static readonly string[] boilerTypeTexts = new[] {
		"Default",
		"Streamlined"
	};
	public static readonly string[] cabTypeTexts = new[] {
		"Default",
		"German"
	};
	public static readonly string[] smokeDeflectorTypeTexts = new[] {
		"None",
		"Witte",
		"Wagner"
	};
	public static readonly string[] smokeStackTypeTexts = new[] {
		"Default",
		"Short",
		"Balloon",
	};
	public static readonly string[] cowCatcherTypeTexts = new[] {
		"Default",
		"Streamlined",
		"None",
	};

	// Base
	[AllowNull] public static GameObject baseLoad;
	// Boilers
	[AllowNull] public static GameObject defaultBoilerLoad;
	[AllowNull] public static GameObject streamlineBoilerLoad;
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
	[AllowNull] public static GameObject frontSmokeBoxDoorLoad; // this is a dumb name but i dont want it to get too verbose so this works
	// Smoke Deflectors
	[AllowNull] public static GameObject wagnerSmokeDeflectorsLoad;
	[AllowNull] public static GameObject witteSmokeDeflectorsLoad;
	[AllowNull] public static GameObject chineseSmokeDeflectorsLoad;
	// Smoke Stacks
	[AllowNull] public static GameObject defaultSmokeStackLoad;
	[AllowNull] public static GameObject shortSmokeStackLoad;
	[AllowNull] public static GameObject balloonSmokeStackLoad;
	// Extras
	[AllowNull] public static GameObject frontCoverLoad;
	[AllowNull] public static GameObject railingsLoad;
	[AllowNull] public static GameObject walkwayLoad;


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
		Settings.boilerType = (Settings.BoilerType) GUILayout.SelectionGrid((int) Settings.boilerType, boilerTypeTexts, 1, "toggle");
		GUILayout.Label("Cab Type");
		Settings.cabType = (Settings.CabType) GUILayout.SelectionGrid((int) Settings.cabType, cabTypeTexts, 1, "toggle");
		GUILayout.Label("Cow Catcher Type");
		Settings.cowCatcherType = (Settings.CowCatcherType) GUILayout.SelectionGrid((int) Settings.cowCatcherType, cowCatcherTypeTexts, 1, "toggle");
		GUILayout.Label("Smoke Deflector Type");
		Settings.smokeDeflectorType = (Settings.SmokeDeflectorType) GUILayout.SelectionGrid((int) Settings.smokeDeflectorType, smokeDeflectorTypeTexts, 1, "toggle");
		GUILayout.Label("Smoke Stack Type");
		Settings.smokeStackType = (Settings.SmokeStackType) GUILayout.SelectionGrid((int) Settings.smokeStackType, smokeStackTypeTexts, 1, "toggle");

		GUILayout.Label("Extras");
		Settings.frontCover = GUILayout.Toggle(Settings.frontCover, "Front Cover");
		Settings.railings = GUILayout.Toggle(Settings.railings, "Railings");
		Settings.walkway = GUILayout.Toggle(Settings.walkway, "Walkway");

		GUILayout.EndVertical();
	}
	static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
		Settings.Save(modEntry);
	}

	static void LoadAllAssets() {
		// Base
		AssetBundle baseReal = LoadAssetBundle("base"); // I can't call it base because that's a thing from c#, idk what it does
		baseLoad = LoadAssetFromBundle(baseReal, "Assets/Base.prefab");
		// Boilers
		AssetBundle boilers = LoadAssetBundle("boilers");
		defaultBoilerLoad = LoadAssetFromBundle(boilers, "Assets/DefaultBoiler.prefab");
		//streamlineBoilerLoad = LoadAssetBundle("boilers", "Assets/StreamlineBoiler.prefab");
		// Cabs
		AssetBundle cabs = LoadAssetBundle("cabs");
		defaultCabLoad = LoadAssetFromBundle(cabs, "Assets/DefaultCab.prefab");
		betterCabLoad = LoadAssetFromBundle(cabs, "Assets/BetterCab.prefab");
		germanCabLoad = LoadAssetFromBundle(cabs, "Assets/GermanCab.prefab");
		// Cowcatchers
		AssetBundle cowCatchers = LoadAssetBundle("cowcatchers");
		defaultCowCatcherLoad = LoadAssetFromBundle(cowCatchers, "Assets/DefaultCowCatcher.prefab");
		noCowCatcherLoad = LoadAssetFromBundle(cowCatchers, "Assets/NoCowCatcher.prefab");
		streamlinedCowCatcherLoad = LoadAssetFromBundle(cowCatchers, "Assets/StreamlinedCowCatcher.prefab");
		// Interiors
		//AssetBundle interiors = LoadAssetBundle("interiors");
		//defaultInteriorLoad = LoadAssetFromBundle(interiors, "Assets/Base.prefab");
		//betterInteriorLoad = LoadAssetFromBundle(interiors, "Assets/Base.prefab");
		// Smoke Box Door
		AssetBundle smokeBoxDoor = LoadAssetBundle("smokeboxdoors");
		defaultSmokeBoxDoorLoad = LoadAssetFromBundle(smokeBoxDoor, "Assets/DefaultSmokeBoxDoor.prefab");
		frontSmokeBoxDoorLoad = LoadAssetFromBundle(smokeBoxDoor, "Assets/FrontSmokeBoxDoor.prefab");
		// Smoke Deflectors
		AssetBundle smokeDeflectors = LoadAssetBundle("smokedeflectors");
		wagnerSmokeDeflectorsLoad = LoadAssetFromBundle(smokeDeflectors, "Assets/WagnerSmokeDeflectors.prefab");
		witteSmokeDeflectorsLoad = LoadAssetFromBundle(smokeDeflectors, "Assets/WitteSmokeDeflectors.prefab");
		// chineseSmokeDeflectorsLoad = LoadAssetFromBundle("smokedeflectors", "Assets/ChineseSmokeDeflectors.prefab");
		// Smoke Stacks
		AssetBundle smokeStacks = LoadAssetBundle("smokestacks");
		defaultSmokeStackLoad = LoadAssetFromBundle(smokeStacks, "Assets/DefaultSmokeStack.prefab");
		shortSmokeStackLoad = LoadAssetFromBundle(smokeStacks, "Assets/ShortSmokeStack.prefab");
		balloonSmokeStackLoad = LoadAssetFromBundle(smokeStacks, "Assets/BalloonSmokeStack.prefab");
		// Extras
		AssetBundle extras = LoadAssetBundle("extras");
		frontCoverLoad = LoadAssetFromBundle(extras, "Assets/FrontCover.prefab");
		railingsLoad = LoadAssetFromBundle(extras, "Assets/Railings.prefab");
		walkwayLoad = LoadAssetFromBundle(extras, "Assets/Walkways.prefab");
	}

	public static AssetBundle LoadAssetBundle(string assetBundle) {
		string assetPath = Path.Combine(Instance.Path.ToString(), "assets");
		Instance.Logger.Log("i sexed benny");

		AssetBundle loadedBundle = AssetBundle.LoadFromFile(Path.Combine(assetPath, assetBundle));
		return loadedBundle;
	}

	public static GameObject LoadAssetFromBundle(AssetBundle assetBundle, String directory) {
		GameObject loadedObject = assetBundle.LoadAsset<GameObject>(directory);
		return loadedObject;
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
