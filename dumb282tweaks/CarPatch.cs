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
		// Smoke Deflector Type
		if(__instance != null && __instance.carType == TrainCarType.LocoSteamHeavy) {
			// This is a terrible way to get the mesh of the locomotive since a simple gameobject reordering will break it, but it's alright for now. If something breaks, just note that this is likely the reason
			Transform s282Body = __instance.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0);
			// This is a better way of getting the mesh, but I don't feel like trying to understand transform.Find, so I'll come back this later.I think the directory with slashes needs to be exactly parents and children, no skipping levels
			//Transform s282Mesh = __instance.transform.Find("LocoS282A_Body/s282_locomotive_body");
			Material s282Mat = s282Body.GetComponent<MeshRenderer>().material;
			s282Body.gameObject.SetActive(false);

			// Load Base
			Log("Help Please Send Help");

			GameObject s282BodyNew = UnityEngine.Object.Instantiate(baseLoad);
			s282BodyNew.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
			s282BodyNew.transform.parent = __instance.transform;
			s282BodyNew.transform.localPosition = new Vector3(0, 0, 4.896f);
			s282BodyNew.transform.localRotation = Quaternion.identity;
			Log("SEND MORE HELP");

			/*
			// Boiler Type
			switch(Main.Settings.boilerType) {
				case Settings.BoilerType.Default:
					GameObject defaultBoiler = UnityEngine.Object.Instantiate(defaultBoilerLoad);

					defaultBoiler.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
					defaultBoiler.transform.parent = __instance.transform;
					defaultBoiler.transform.localPosition = new Vector3(0, 2.15f, 5.1f);
					defaultBoiler.transform.localRotation = Quaternion.identity;
					break;
				case Settings.BoilerType.Streamlined:
					GameObject streamlinedBoiler = UnityEngine.Object.Instantiate(streamlineBoilerLoad);

					streamlinedBoiler.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
					streamlinedBoiler.transform.parent = __instance.transform;
					streamlinedBoiler.transform.localPosition = new Vector3(0, 2.15f, 5.1f);
					streamlinedBoiler.transform.localRotation = Quaternion.identity;
					break;
			}
			
			// Cab Type
			switch(Main.Settings.cabType) {
				case Settings.CabType.Default:
					GameObject defaultCab = UnityEngine.Object.Instantiate(defaultCabLoad);

					defaultCab.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;

					defaultCab.transform.parent = __instance.transform;
					defaultCab.transform.localPosition = new Vector3(0, -0.13f, 6.98f);
					defaultCab.transform.localRotation = Quaternion.identity;
					break;
				case Settings.CabType.German:
					GameObject germanCab = UnityEngine.Object.Instantiate(germanCabLoad);

					germanCab.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
					germanCab.transform.GetChild(1).GetComponent<MeshRenderer>().material = s282Mat;
					germanCab.transform.GetChild(2).GetComponent<MeshRenderer>().material = s282Mat;
					germanCab.transform.GetChild(3).GetComponent<MeshRenderer>().material = s282Mat;

					germanCab.transform.parent = __instance.transform;
					germanCab.transform.localPosition = new Vector3(0, -0.13f, 6.98f);
					germanCab.transform.localRotation = Quaternion.identity;
					//Log(externalInteractables.name);

					//externalInteractables.gameObject.SetActive(false);
					break;
			}
			*/
			// Smoke Deflector
			switch(Main.Settings.smokeDeflectorType) {
				case Settings.SmokeDeflectorType.None:
					break;
				case Settings.SmokeDeflectorType.Witte:
					GameObject witteSmokeDeflector = UnityEngine.Object.Instantiate(witteSmokeDeflectorsLoad);

					//This is a terrible way to set the material of the smoke deflectors, but oh well
					witteSmokeDeflector.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
					witteSmokeDeflector.transform.GetChild(1).GetComponent<MeshRenderer>().material = s282Mat;
					witteSmokeDeflector.transform.GetChild(3).GetComponent<MeshRenderer>().material = s282Mat;

					witteSmokeDeflector.transform.parent = __instance.transform;
					witteSmokeDeflector.transform.localPosition = new Vector3(0, 0, 4.896f);
					witteSmokeDeflector.transform.localRotation = Quaternion.identity;
					Log("witte");
					if(Main.Settings.bluetooth) {
						witteSmokeDeflector.transform.GetChild(0).gameObject.SetActive(false);
					}
					break;
				case Settings.SmokeDeflectorType.Wagner:
					GameObject wagnerSmokeDeflector = UnityEngine.Object.Instantiate(wagnerSmokeDeflectorsLoad);

					// This is still a terrible way to set the material of the smoke deflectors
					wagnerSmokeDeflector.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;
					wagnerSmokeDeflector.transform.GetChild(1).GetComponent<MeshRenderer>().material = s282Mat;

					wagnerSmokeDeflector.transform.parent = __instance.transform;
					wagnerSmokeDeflector.transform.localPosition = new Vector3(0, 0, 4.896f);
					wagnerSmokeDeflector.transform.localRotation = Quaternion.identity;
					Log("witten't");
					break;
			}
			
			// Smoke Stack
			switch(Main.Settings.smokeStackType) {
				case Settings.SmokeStackType.Default:
					GameObject defaultSmokeStack = UnityEngine.Object.Instantiate(defaultSmokeStackLoad);

					defaultSmokeStack.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;

					defaultSmokeStack.transform.parent = __instance.transform;
					defaultSmokeStack.transform.localPosition = new Vector3(0, 0, 4.896f);
					defaultSmokeStack.transform.localRotation = Quaternion.identity;
					break;
				case Settings.SmokeStackType.Short:
					GameObject shortSmokeStack = UnityEngine.Object.Instantiate(shortSmokeStackLoad);

					shortSmokeStack.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;

					shortSmokeStack.transform.parent = __instance.transform;
					shortSmokeStack.transform.localPosition = new Vector3(0, 0, 4.896f);
					shortSmokeStack.transform.localRotation = Quaternion.identity;
					break;
			}
			/*
			// Extras
			if(Main.Settings.frontCover) {
				GameObject frontCover = UnityEngine.Object.Instantiate(frontCoverLoad);

				frontCover.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;

				frontCover.transform.parent = __instance.transform;
				frontCover.transform.localPosition = new Vector3(0, -0.15f, 6.98f);
				frontCover.transform.localRotation = Quaternion.identity;
			}
			if(Main.Settings.railings) {
				GameObject railings = UnityEngine.Object.Instantiate(railingsLoad);

				railings.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;

				railings.transform.parent = __instance.transform;
				railings.transform.localPosition = new Vector3(0, -0.15f, 6.98f);
				railings.transform.localRotation = Quaternion.identity;
			}
			if(Main.Settings.walkway) {
				GameObject walkway = UnityEngine.Object.Instantiate(walkwayLoad);

				walkway.transform.GetChild(0).GetComponent<MeshRenderer>().material = s282Mat;

				walkway.transform.parent = __instance.transform;
				walkway.transform.localPosition = new Vector3(0, -0.15f, 6.98f);
				walkway.transform.localRotation = Quaternion.identity;
			}
			*/
		}
	}
}
