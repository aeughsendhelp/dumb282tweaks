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
			bool chonk = false;
			// This is a terrible way to get the mesh of the locomotive since a simple gameobject reordering will break it, but it's alright for now. If something breaks, just note that this is likely the reason
			Transform originalBody = __instance.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0);
			Transform originalSmokeBoxDoor = __instance.transform.GetChild(5).transform.GetChild(0).transform.GetChild(7);
			//Transform originalHeadlights = __instance.transform.GetChild(5).transform.GetChild(13);
			// This is a better way of getting the mesh, but I don't feel like trying to understand transform.Find, so I'll come back this later. I think the directory with slashes needs to be exactly parents and children, no skipping levels
			//Transform s282Mesh = carInstance.transform.Find("LocoS282A_Body/s282_locomotive_body");
			Material s282Mat = originalBody.GetComponent<MeshRenderer>().material;
			originalBody.gameObject.SetActive(false);
			originalSmokeBoxDoor.gameObject.SetActive(false);
			//originalHeadlights.gameObject.SetActive(false);

			GameObject baseReal = InstantiateLoadedObject(baseLoad, s282Mat, __instance.transform);

			if(Main.Settings.boilerType == Settings.BoilerType.Chonky) {
				chonk = true;
			} else {
				chonk = false;
			}

			// Boiler
			switch(Main.Settings.boilerType) {
				case Settings.BoilerType.Default:
					GameObject defaultBoiler = InstantiateLoadedObject(defaultBoilerLoad, s282Mat, __instance.transform);
					break;
				case Settings.BoilerType.Streamlined:
					GameObject streamlineBoiler = InstantiateLoadedObject(streamlineBoilerLoad, s282Mat, __instance.transform);
					break;
				case Settings.BoilerType.Chonky:
					GameObject chonkyBoiler = InstantiateLoadedObject(chonkyBoilerLoad, s282Mat, __instance.transform);
					chonkyBoiler.transform.localPosition = new Vector3(0, 0.2f, objOffset);
					break;
			}
			// Cab
			switch(Main.Settings.cabType) {
				case Settings.CabType.Default:
					GameObject defaultCab = InstantiateLoadedObject(defaultCabLoad, s282Mat, __instance.transform);
					break;
				case Settings.CabType.Better:
					GameObject betterCab = InstantiateLoadedObject(betterCabLoad, s282Mat, __instance.transform);

					//GameObject betterCabInterior = InstantiateLoadedObject(betterInteriorLoad, s282Mat, __instance.transform);
					break;
				case Settings.CabType.German:
					GameObject germanCab = InstantiateLoadedObject(germanCabLoad, s282Mat, __instance.transform);
					break;
			}
			// Cow Catchers
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
			// Smoke Box Door
			switch(Main.Settings.smokeBoxDoorType) {
				case Settings.SmokeBoxDoorType.Default:
					GameObject defaultSmokeBoxDoor = InstantiateLoadedObject(defaultSmokeBoxDoorLoad, s282Mat, __instance.transform);
					defaultSmokeBoxDoor.transform.localPosition = new Vector3(0, 2.60208f, 5.69122f + objOffset);
					if(chonk) {
						defaultSmokeBoxDoor.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
					}
					break;
				case Settings.SmokeBoxDoorType.Center:
					//GameObject centerSmokeBoxDoor = InstantiateLoadedObject(centerSmokeBoxDoorLoad, s282Mat, __instance.transform);
					//centerSmokeBoxDoor.transform.localPosition = new Vector3(0, 2.60208f, 5.69122f + objOffset);
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
			// Extras
			if(Main.Settings.railings) {
				GameObject railings = InstantiateLoadedObject(railingsLoad, s282Mat, __instance.transform);
			}
			if(Main.Settings.walkway) {
				GameObject walkways = InstantiateLoadedObject(walkwayLoad, s282Mat, __instance.transform);
			}
			if(Main.Settings.frontCover) {
				GameObject frontCover = InstantiateLoadedObject(frontCoverLoad, s282Mat, __instance.transform);
			}
		}
	}
}
