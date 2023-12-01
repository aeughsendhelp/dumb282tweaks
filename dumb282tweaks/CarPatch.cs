using DV.ThingTypes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static dumb282tweaks.Main;
using UnityEngine;

namespace dumb282tweaks;

[HarmonyPatch(typeof(TrainCar), "Start")]
class CarPatch {
	static void Postfix(ref TrainCar __instance) {
		if(__instance != null && __instance.carType == TrainCarType.LocoSteamHeavy) {
			// This is a terrible way to get the mesh of the locomotive since a simple gameobject reordering will break it, but it's alright for now. If something breaks, just note that this is likely the reason
			Transform originalBody = __instance.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0);
			// This is a better way of getting the mesh, but I don't feel like trying to understand transform.Find, so I'll come back this later. I think the directory with slashes needs to be exactly parents and children, no skipping levels
			//Transform s282Mesh = carInstance.transform.Find("LocoS282A_Body/s282_locomotive_body");
			Material s282Mat = originalBody.GetComponent<MeshRenderer>().material;
			originalBody.gameObject.SetActive(false);

			GameObject baseReal = InstantiateLoadedObject(baseLoad, s282Mat, __instance.transform);
			GameObject defaultBoiler = InstantiateLoadedObject(defaultBoilerLoad, s282Mat, __instance.transform);
			GameObject defaultCab = InstantiateLoadedObject(defaultCabLoad, s282Mat, __instance.transform);


			switch(Main.Settings.cowCatcherType) {
				case Settings.CowCatcherType.Default:
					GameObject defaultCowCatcher = InstantiateLoadedObject(defaultCowCatcherLoad, s282Mat, __instance.transform);
					break;
				case Settings.CowCatcherType.None:
					GameObject noCowCatcher = InstantiateLoadedObject(noCowCatcherLoad, s282Mat, __instance.transform);
					break;
				case Settings.CowCatcherType.Streamlined:
					GameObject streamlinedCowCatcher = InstantiateLoadedObject(streamlinedCowCatcherLoad, s282Mat, __instance.transform);
					break;
			}
			// Smoke Deflectors
			switch(Main.Settings.smokeDeflectorType) {
				case Settings.SmokeDeflectorType.None:
					break;
				case Settings.SmokeDeflectorType.Witte:
					GameObject witteSmokeDeflector = InstantiateLoadedObject(witteSmokeDeflectorsLoad, s282Mat, __instance.transform);
					break;
				case Settings.SmokeDeflectorType.Wagner:
					GameObject wagnerSmokeDeflector = InstantiateLoadedObject(wagnerSmokeDeflectorsLoad, s282Mat, __instance.transform);
					break;
			}
			// Smoke Stacks
			switch(Main.Settings.smokeStackType) {
				case Settings.SmokeStackType.Default:
					GameObject defaultSmokeStack = InstantiateLoadedObject(defaultSmokeStackLoad, s282Mat, __instance.transform);
					break;
				case Settings.SmokeStackType.Short:
					GameObject shortSmokeStack = InstantiateLoadedObject(shortSmokeStackLoad, s282Mat, __instance.transform);
					break;
				case Settings.SmokeStackType.Balloon:
					GameObject balloonSmokeStack = InstantiateLoadedObject(balloonSmokeStackLoad, s282Mat, __instance.transform);
					break;
			}
		}
	}

	static GameObject InstantiateLoadedObject(GameObject toLoad, Material mat, Transform toParent) {
		GameObject obj = UnityEngine.Object.Instantiate(toLoad);

		for (int i = 0; i < obj.transform.childCount; i++) {
			obj.transform.GetChild(i).GetComponent<MeshRenderer>().material = mat;
		}

		obj.transform.parent = toParent;
		obj.transform.localPosition = new Vector3(0, 0, 4.887f);
		obj.transform.localRotation = Quaternion.identity;
		return obj;
	}
}
