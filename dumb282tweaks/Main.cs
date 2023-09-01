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

	public static readonly string[] cabTypeTexts = new[] {
		"Default",
		"German"
	};
	public static readonly string[] smokeDeflectorTypeTexts = new[] {
		"None",
		"Witte",
		"Wagner"
	};
	public static readonly string[] boilerTypeTexts = new[] {
		"Default",
		"Streamlined"
	};
	public static readonly string[] smokeStackTypeTexts = new[] {
		"Default",
		"Short",
	};
	// Base
	[AllowNull] public static GameObject s282BodyLoad;
	// Boilers
	[AllowNull] public static GameObject streamlineLoad;
	// Cabs
	[AllowNull] public static GameObject defaultCabLoad;
	[AllowNull] public static GameObject germanCabLoad;

	[AllowNull] public static GameObject wagnerSmokeDeflectorsLoad;
	[AllowNull] public static GameObject witteSmokeDeflectorsLoad;

	[AllowNull] public static GameObject defaultSmokeStackLoad;
	[AllowNull] public static GameObject shortSmokeStackLoad;

	[AllowNull] public static GameObject cowCatcherLoad;
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

			string assetPath = Path.Combine(Instance.Path.ToString(), "assets\\");

			// Base
			string baseAssetPath = Path.Combine(assetPath, "Base\\");
			var s282BodyBundle = AssetBundle.LoadFromFile(Path.Combine(baseAssetPath, "s282body"));
			s282BodyLoad = s282BodyBundle.LoadAsset<GameObject>("Assets/S282Body.prefab");
			// Boilers
			string boilersAssetPat = Path.Combine(assetPath, "Boilers\\");
			var streamlineBundle = AssetBundle.LoadFromFile(Path.Combine(boilersAssetPat, "streamline"));
			streamlineLoad = streamlineBundle.LoadAsset<GameObject>("Assets/Streamline.prefab");
			// Cabs
			string cabsAssetPath = Path.Combine(assetPath, "Cabs\\");
			var defaultCabBundle = AssetBundle.LoadFromFile(Path.Combine(cabsAssetPath, "defaultcab"));
			defaultCabLoad = defaultCabBundle.LoadAsset<GameObject>("Assets/DefaultCab.prefab");
			var germanCabBundle = AssetBundle.LoadFromFile(Path.Combine(cabsAssetPath, "germancab"));
			germanCabLoad = germanCabBundle.LoadAsset<GameObject>("Assets/GermanCab.prefab");
			// Smoke Deflectors
			string smokeDeflectorsAssetPath = Path.Combine(assetPath, "SmokeDeflectors\\");
			var wagnerSmokeDeflectorsBundle = AssetBundle.LoadFromFile(Path.Combine(smokeDeflectorsAssetPath, "wagnersmokedeflectors"));
			wagnerSmokeDeflectorsLoad = wagnerSmokeDeflectorsBundle.LoadAsset<GameObject>("Assets/WagnerSmokeDeflectors.prefab");
			var witteSmokeDeflectorsBundle = AssetBundle.LoadFromFile(Path.Combine(smokeDeflectorsAssetPath, "wittesmokedeflectors"));
			witteSmokeDeflectorsLoad = witteSmokeDeflectorsBundle.LoadAsset<GameObject>("Assets/WitteSmokeDeflectors.prefab");
			// Smoke Stacks
			string smokeStacksAssetPath = Path.Combine(assetPath, "SmokeStacks\\");
			var defaultSmokeStackBundle = AssetBundle.LoadFromFile(Path.Combine(smokeStacksAssetPath, "defaultsmokestack"));
			defaultSmokeStackLoad = defaultSmokeStackBundle.LoadAsset<GameObject>("Assets/DefaultSmokeStack.prefab");
			var shortSmokeStackBundle = AssetBundle.LoadFromFile(Path.Combine(smokeStacksAssetPath, "shortsmokestack"));
			shortSmokeStackLoad = shortSmokeStackBundle.LoadAsset<GameObject>("Assets/ShortSmokeStack.prefab");
			// Extras
			string extrasAssetPath = Path.Combine(assetPath, "Extras\\");
			var cowCatcherBundle = AssetBundle.LoadFromFile(Path.Combine(extrasAssetPath, "cowcatcher"));
			cowCatcherLoad = cowCatcherBundle.LoadAsset<GameObject>("Assets/CowCatcher.prefab");
			var frontCoverBundle = AssetBundle.LoadFromFile(Path.Combine(extrasAssetPath, "frontcover"));
			frontCoverLoad = frontCoverBundle.LoadAsset<GameObject>("Assets/FrontCover.prefab");
			var railingsBundle = AssetBundle.LoadFromFile(Path.Combine(extrasAssetPath, "railings"));
			railingsLoad = railingsBundle.LoadAsset<GameObject>("Assets/Railings.prefab");
			var walkwayBundle = AssetBundle.LoadFromFile(Path.Combine(extrasAssetPath, "walkway"));
			walkwayLoad = walkwayBundle.LoadAsset<GameObject>("Assets/Walkway.prefab");
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
		GUILayout.Label("Also, reloading a save will currently break things and the tweaks won't load. This isn't good.");

		GUILayout.Label("Boiler Type");
		Settings.boilerType = (Settings.BoilerType) GUILayout.SelectionGrid((int) Settings.boilerType, boilerTypeTexts, 1, "toggle");
		GUILayout.Label("Cab Type");
		Settings.cabType = (Settings.CabType)GUILayout.SelectionGrid((int) Settings.cabType, cabTypeTexts, 1, "toggle");
		GUILayout.Label("Smoke Deflector Type");
		Settings.smokeDeflectorType = (Settings.SmokeDeflectorType) GUILayout.SelectionGrid((int) Settings.smokeDeflectorType, smokeDeflectorTypeTexts, 1, "toggle");
		GUILayout.Label("Smoke Stack Type");
		Settings.smokeStackType = (Settings.SmokeStackType) GUILayout.SelectionGrid((int) Settings.smokeStackType, smokeStackTypeTexts, 1, "toggle");

		GUILayout.Label("Extras");
		Settings.cowCatcher = GUILayout.Toggle(Settings.cowCatcher, "Cow Catcher");
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
