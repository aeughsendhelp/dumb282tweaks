using DV.ThingTypes;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static dumb282tweaks.Main;

namespace dumb282tweaks;

[HarmonyPatch(typeof(TrainCar), "LoadInterior")]
class InteriorPatch {
	static void Postfix(ref TrainCar __instance) {
		if(__instance != null && __instance.carType == TrainCarType.LocoSteamHeavy) {
			GameObject originalInterior = __instance.loadedInterior;
			GameObject externalInteractables = __instance.loadedExternalInteractables;

			GameObject originalCab = originalInterior.transform.GetChild(4).GetChild(0).gameObject;
			GameObject originalThings = originalInterior.transform.GetChild(4).GetChild(2).gameObject;
			GameObject windowL = externalInteractables.transform.GetChild(0).GetChild(4).gameObject;
			GameObject windowR = externalInteractables.transform.GetChild(0).GetChild(3).gameObject;

			originalCab.SetActive(false);
			originalThings.SetActive(false);
			windowL.SetActive(false);
			windowR.SetActive(false);

			Material cabMat = originalCab.GetComponent<MeshRenderer>().material;
			Material thingsMat = originalThings.GetComponent<MeshRenderer>().material;

			GameObject betterInterior = InstantiateLoadedObject(betterInteriorLoad, thingsMat, originalInterior.transform);

			// Interior
			//switch(Main.Settings.cabType) {
			//	case Settings.CabType.Default:
			//		//GameObject DefaultInterior = InstantiateLoadedObject(defaultInteriorLoad, thingsMat, __instance.transform);
			//		break;
			//	case Settings.CabType.Better:
			//		//GameObject betterInterior = InstantiateLoadedObject(betterInteriorLoad, thingsMat, __instance.transform);
			//		break;
			//	case Settings.CabType.German:
			//		//GameObject germanInterior = InstantiateLoadedObject(germanCabLoad, thingsMat, __instance.transform);
			//		Log("No german cab interior yet, womp womp");
			//		break;
			//}
			//Transform s282Interior = __instance.interior;
			//Transform externalInteractables = s282Interior.transform.Find("LocoS282A_ExternalInteractables(Clone)");
			//// The fact that in game it actually is called "things" is funny to me
			//Transform cab = s282Interior.transform.Find("LocoS282A_Interior(Clone)/Static/Cab");
			//Transform things = s282Interior.transform.Find("LocoS282A_Interior(Clone)/Static/Things");
			//Transform windowRMove = externalInteractables.transform.Find("Interactables/WindowR");
			//Transform windowLMove = externalInteractables.transform.Find("Interactables/WindowL");

			//Log(externalInteractables.name);

			//switch(Main.Settings.cabType) {
			//	case Settings.CabType.Default:
			//		break;
			//	case Settings.CabType.German:
			//		cab.gameObject.SetActive(false);
			//		things.gameObject.SetActive(false);
			//		windowRMove.gameObject.SetActive(false);
			//		windowLMove.gameObject.SetActive(false);
			//		break;
			//}
		}
	}
}
