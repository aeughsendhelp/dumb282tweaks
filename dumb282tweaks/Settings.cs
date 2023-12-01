using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using static dumb282tweaks.Settings;

namespace dumb282tweaks;

public static class Settings {
	public enum Preset {
		[Description("Default")]
		Default,
		[Description("Streamlined")]
		Streamlined
	}
	public enum BoilerType {
		[Description("Default Boiler")]
		Default,
		[Description("Streamlined Boiler")]
		Streamlined
	}
	public enum CabType {
		[Description("Default 282 Cab")]
		Default,
		[Description("Default 282 Cab")]
		Better,
		[Description("German Cab")]
		German
	}
	public enum CowCatcherType {
		[Description("Default 282 Cab")]
		Default,
		[Description("German Cab")]
		Streamlined,
		[Description("No Cab")]
		None
	}
	public enum SmokeBoxDoorType {
		[Description("Default Smoke Box Door")]
		Default,
		[Description("American-style headlight in center")]
		Center
	}
	public enum SmokeDeflectorType {
		[Description("No Smoke Deflectors")]
		None,
		[Description("Witte Smoke Deflectors")]
		Witte,
		[Description("Wagner Smoke Deflectors")]
		Wagner
	}
	public enum SmokeStackType {
		[Description("Default Smoke Stack")]
		Default,
		[Description("Short Smoke Stack")]
		Short,
		[Description("Balloon Smoke Stack")]
		Balloon
	}
}

public class dumb282tweaksSettings : UnityModManager.ModSettings {
	public BoilerType boilerType = BoilerType.Default;
	public CabType cabType = CabType.Better;
	public CowCatcherType cowCatcherType = CowCatcherType.Default;
	public SmokeBoxDoorType smokeBoxDoorType = SmokeBoxDoorType.Default;
	public SmokeDeflectorType smokeDeflectorType = SmokeDeflectorType.Wagner;
	public SmokeStackType smokeStackType = SmokeStackType.Short;

	public bool railings = true;
	public bool walkway = true;
	public bool frontCover = false;

	public override void Save(UnityModManager.ModEntry modEntry) {
		Save(this, modEntry);
	}
}

