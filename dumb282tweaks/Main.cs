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
	[AllowNull] public static GameObject germanCabLoad;
	[AllowNull] public static GameObject betterCabLoad;
	// Interiors
	[AllowNull] public static GameObject defaultInteriorLoad;
	[AllowNull] public static GameObject betterInteriorLoad;
	// Smoke Deflectors
	[AllowNull] public static GameObject wagnerSmokeDeflectorsLoad;
	[AllowNull] public static GameObject witteSmokeDeflectorsLoad;
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

	// GUI Rendering
	static void OnGUI(UnityModManager.ModEntry modEntry) {
		GUILayout.BeginVertical();

		GUILayout.Label("These settings are applied on train spawn, meaning rejoining the game will refresh all 282 locos to the settings specified here. But, if you don't unload the train, it will keep whatever settings were there previously. This is a temporary solution until I have a proper comms radio GUI implemented.");
		GUILayout.Label("Also, reloading a save will currently break things and the tweaks won't load. This isn't good.");

		GUILayout.Label("Boiler Type");
		Settings.boilerType = (Settings.BoilerType) GUILayout.SelectionGrid((int) Settings.boilerType, boilerTypeTexts, 1, "toggle");
		GUILayout.Label("Cab Type");
		Settings.cabType = (Settings.CabType)GUILayout.SelectionGrid((int) Settings.cabType, cabTypeTexts, 1, "toggle");
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

		GUILayout.Label("The dumb in Dumb S282 Tweaks");
		Settings.bluetooth = GUILayout.Toggle(Settings.bluetooth, "Bluetooth");
		Settings.flattensyour282 = GUILayout.Toggle(Settings.flattensyour282, "Flattens Your 282");

		GUILayout.EndVertical();
	}
	static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
		Settings.Save(modEntry);
	}

	static void LoadAllAssets() {
		string assetPath = Path.Combine(Instance.Path.ToString(), "assets");
		// Base
		string baseAssetPath = Path.Combine(assetPath, "Based");
		baseLoad = LoadAssetBundle(baseAssetPath, "base", "Assets/Base.prefab");
		// Boilers
		/*
		string boilersAssetPath = Path.Combine(assetPath, "Boilers");
		defaultBoilerLoad = LoadAssetBundle(boilersAssetPath, "defaultBoiler", "Assets/DefaultBoiler.prefab");
		streamlineBoilerLoad = LoadAssetBundle(boilersAssetPath, "streamlineBoiler", "Assets/StreamlineBoiler.prefab");
		// Cabs
		string cabsAssetPath = Path.Combine(assetPath, "Cabs");
		defaultCabLoad = LoadAssetBundle(cabsAssetPath, "defaultcab", "Assets/DefaultCab.prefab");
		betterCabLoad = LoadAssetBundle(cabsAssetPath, "bettercab", "Assets/BetterCab.prefab");
		germanCabLoad = LoadAssetBundle(cabsAssetPath, "germancab", "Assets/GermanCab.prefab");
		// Cow Catchers
		string cowCatcherAssetPath = Path.Combine(assetPath, "CowCatchers");
		defaultCabLoad = LoadAssetBundle(cowCatcherAssetPath, "defaultcowcatcher", "Assets/DefaultCowCatcher.prefab");
		germanCabLoad = LoadAssetBundle(cowCatcherAssetPath, "nocowcatcher", "Assets/NoCowCatcher.prefab");
		germanCabLoad = LoadAssetBundle(cowCatcherAssetPath, "streamlinecowcatcher", "Assets/StreamlineCowCatcher.prefab");
		Interiors
		string interiorsAssetPath = Path.Combine(assetPath, "Interiors");
		defaultInteriorLoad = LoadAssetBundle(interiorsAssetPath, "defaultinterior", "Assets/DefaultInterior.prefab");
		betterInteriorLoad = LoadAssetBundle(interiorsAssetPath, "betterinterior", "Assets/BetterInterior.prefab");
		// Smoke Deflectors*/
		string smokeDeflectorsAssetPath = Path.Combine(assetPath, "SmokeDeflectors");
		wagnerSmokeDeflectorsLoad = LoadAssetBundle(smokeDeflectorsAssetPath, "wagnersmokedeflectors", "Assets/WagnerSmokeDeflectors.prefab");
		witteSmokeDeflectorsLoad = LoadAssetBundle(smokeDeflectorsAssetPath, "wittesmokedeflectors", "Assets/WitteSmokeDeflectors.prefab");
		// Smoke Stacks
		string smokeStacksAssetPath = Path.Combine(assetPath, "SmokeStacks");
		defaultSmokeStackLoad = LoadAssetBundle(smokeStacksAssetPath, "defaultsmokestack", "Assets/DefaultSmokeStack.prefab");
		shortSmokeStackLoad = LoadAssetBundle(smokeStacksAssetPath, "shortsmokestack", "Assets/ShortSmokeStack.prefab");
		balloonSmokeStackLoad = LoadAssetBundle(smokeStacksAssetPath, "balloonsmokestack", "Assets/BalloonSmokeStack.prefab");
		// Extras
		/*
		string extrasAssetPath = Path.Combine(assetPath, "Extras");
		frontCoverLoad = LoadAssetBundle(extrasAssetPath, "frontcover", "Assets/FrontCover.prefab");
		railingsLoad = LoadAssetBundle(extrasAssetPath, "railings", "Assets/Railings.prefab");
		walkwayLoad = LoadAssetBundle(extrasAssetPath, "walkway", "Assets/Walkway.prefab");*/
	}

	public static GameObject LoadAssetBundle(String path, String assetBundle, String directory) {
		AssetBundle loadedBundle = AssetBundle.LoadFromFile(Path.Combine(path, assetBundle));
		GameObject loadedObject = loadedBundle.LoadAsset<GameObject>(directory);
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
