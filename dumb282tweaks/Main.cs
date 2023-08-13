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
using static dumb282tweaks.Main;

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

			//var assets = AssetBundle.LoadFromFile(Path.Combine(modEntry.Path, "model.fullcab"));
			//var fullcab = assets.LoadAsset<GameObject>("fullcab");
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

		Settings.toggleTweaks = GUILayout.Toggle(Settings.toggleTweaks, "Toggle Tweaks");

		GUILayout.Label("Cab Type");
		Settings.cabType = (CabType)GUILayout.SelectionGrid((int)Settings.cabType, cabTypeTexts, 1, "toggle");
		GUILayout.Space(2);

		GUILayout.Label("Smoke Deflector Type");
		Settings.smokeDeflectorType = (SmokeDeflectorType) GUILayout.SelectionGrid((int) Settings.smokeDeflectorType, smokeDeflectorTypeTexts, 1, "toggle");
		GUILayout.Space(2);

		GUILayout.Label("Texture Utility");

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
