using System;
using System.Collections.Generic;

// Token: 0x0200075B RID: 1883
public class NavObjectClass
{
	// Token: 0x0600383E RID: 14398 RVA: 0x00171104 File Offset: 0x0016F304
	public static void Reset()
	{
		NavObjectClass.NavObjectClassList.Clear();
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x00171110 File Offset: 0x0016F310
	public NavObjectMapSettings GetMapSettings(bool isActive)
	{
		if (isActive)
		{
			return this.MapSettings;
		}
		return this.InactiveMapSettings;
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x00171122 File Offset: 0x0016F322
	public NavObjectCompassSettings GetCompassSettings(bool isActive)
	{
		if (isActive)
		{
			return this.CompassSettings;
		}
		return this.InactiveCompassSettings;
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x00171134 File Offset: 0x0016F334
	public NavObjectScreenSettings GetOnScreenSettings(bool isActive)
	{
		if (isActive)
		{
			return this.OnScreenSettings;
		}
		return this.InactiveOnScreenSettings;
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x00171146 File Offset: 0x0016F346
	public NavObjectClass(string name)
	{
		this.NavObjectClassName = name;
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x00171178 File Offset: 0x0016F378
	public static NavObjectClass GetNavObjectClass(string className)
	{
		for (int i = 0; i < NavObjectClass.NavObjectClassList.Count; i++)
		{
			if (NavObjectClass.NavObjectClassList[i].NavObjectClassName == className)
			{
				return NavObjectClass.NavObjectClassList[i];
			}
		}
		return null;
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x001711C0 File Offset: 0x0016F3C0
	public void Init()
	{
		if (this.Properties.Values.ContainsKey("requirement_type"))
		{
			if (!Enum.TryParse<NavObjectClass.RequirementTypes>(this.Properties.Values["requirement_type"], out this.RequirementType))
			{
				this.RequirementType = NavObjectClass.RequirementTypes.None;
			}
			if (this.RequirementType != NavObjectClass.RequirementTypes.None)
			{
				this.Properties.ParseString("requirement_name", ref this.RequirementName);
			}
		}
		this.Properties.ParseString("tag", ref this.Tag);
		this.Properties.ParseBool("use_override_icon", ref this.UseOverrideIcon);
	}

	// Token: 0x04002DEE RID: 11758
	public static List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002DEF RID: 11759
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04002DF0 RID: 11760
	public string NavObjectClassName = "";

	// Token: 0x04002DF1 RID: 11761
	public NavObjectClass.RequirementTypes RequirementType;

	// Token: 0x04002DF2 RID: 11762
	public string RequirementName = "";

	// Token: 0x04002DF3 RID: 11763
	public bool UseOverrideIcon;

	// Token: 0x04002DF4 RID: 11764
	public string Tag;

	// Token: 0x04002DF5 RID: 11765
	public NavObjectMapSettings MapSettings;

	// Token: 0x04002DF6 RID: 11766
	public NavObjectCompassSettings CompassSettings;

	// Token: 0x04002DF7 RID: 11767
	public NavObjectScreenSettings OnScreenSettings;

	// Token: 0x04002DF8 RID: 11768
	public NavObjectMapSettings InactiveMapSettings;

	// Token: 0x04002DF9 RID: 11769
	public NavObjectCompassSettings InactiveCompassSettings;

	// Token: 0x04002DFA RID: 11770
	public NavObjectScreenSettings InactiveOnScreenSettings;

	// Token: 0x0200075C RID: 1884
	public enum RequirementTypes
	{
		// Token: 0x04002DFC RID: 11772
		None,
		// Token: 0x04002DFD RID: 11773
		CVar,
		// Token: 0x04002DFE RID: 11774
		QuestBounds,
		// Token: 0x04002DFF RID: 11775
		Tracking,
		// Token: 0x04002E00 RID: 11776
		NoTag,
		// Token: 0x04002E01 RID: 11777
		InParty,
		// Token: 0x04002E02 RID: 11778
		IsAlly,
		// Token: 0x04002E03 RID: 11779
		IsPlayer,
		// Token: 0x04002E04 RID: 11780
		IsVehicleOwner,
		// Token: 0x04002E05 RID: 11781
		IsOwner,
		// Token: 0x04002E06 RID: 11782
		NoActiveQuests,
		// Token: 0x04002E07 RID: 11783
		MinimumTreasureRadius,
		// Token: 0x04002E08 RID: 11784
		IsTwitchSpawnedSelf,
		// Token: 0x04002E09 RID: 11785
		IsTwitchSpawnedOther
	}
}
