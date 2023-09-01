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
		Transform s282Interior = __instance.interior;
		Transform externalInteractables = s282Interior.transform.Find("LocoS282A_ExternalInteractables(Clone)");
		// The fact that in game it actually is called "things" is funny to me
		Transform cab = s282Interior.transform.Find("LocoS282A_Interior(Clone)/Static/Cab");
		Transform things = s282Interior.transform.Find("LocoS282A_Interior(Clone)/Static/Things");
		Transform windowRMove = externalInteractables.transform.Find("Interactables/WindowR");
		Transform windowLMove = externalInteractables.transform.Find("Interactables/WindowL");

		Log(externalInteractables.name);

		switch(Main.Settings.cabType) {
			case Settings.CabType.Default:
				break;
			case Settings.CabType.German:
				cab.gameObject.SetActive(false);
				things.gameObject.SetActive(false);
				windowRMove.gameObject.SetActive(false);
				windowLMove.gameObject.SetActive(false);
				break;
		}
	}
}
