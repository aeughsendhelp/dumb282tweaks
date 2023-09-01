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
	public enum CabType {
		[Description("Default 282 Cab")]
		Default,
		[Description("German Cab")]
		German
	}
	public enum BoilerType {
		[Description("Default Boiler")]
		Default,
		[Description("Streamlined Boiler")]
		Streamlined,
	}
	public enum SmokeDeflectorType {
		[Description("No Smoke Deflectors")]
		None,
		[Description("Witte Smoke Deflectors")]
		Witte,
		[Description("Wagner Smoke Deflectors")]
		Wagner,
	}
	public enum SmokeStackType {
		[Description("Default Smoke Stack")]
		Default,
		[Description("Short Smoke Stack")]
		Short,
	}
}

public class dumb282tweaksSettings : UnityModManager.ModSettings {
	public bool bluetooth = false;
	public bool flattensyour282 = false;

	public bool regularBody = true;
	public BoilerType boilerType = BoilerType.Default;
	public CabType cabType = CabType.Default;
	public SmokeDeflectorType smokeDeflectorType = SmokeDeflectorType.Wagner;
	public SmokeStackType smokeStackType = SmokeStackType.Default;

	public bool cowCatcher = true;
	public bool frontCover = false;
	public bool railings = true;
	public bool walkway = true;

	public override void Save(UnityModManager.ModEntry modEntry) {
		Save(this, modEntry);
	}
}

