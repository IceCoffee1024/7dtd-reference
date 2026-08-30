using System;
using System.Collections.Generic;

// Token: 0x02000751 RID: 1873
public class NavObjectClass
{
	// Token: 0x060037E4 RID: 14308 RVA: 0x0016F808 File Offset: 0x0016DA08
	public static void Reset()
	{
		NavObjectClass.NavObjectClassList.Clear();
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x0016F814 File Offset: 0x0016DA14
	public NavObjectMapSettings GetMapSettings(bool isActive)
	{
		if (isActive)
		{
			return this.MapSettings;
		}
		return this.InactiveMapSettings;
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x0016F826 File Offset: 0x0016DA26
	public NavObjectCompassSettings GetCompassSettings(bool isActive)
	{
		if (isActive)
		{
			return this.CompassSettings;
		}
		return this.InactiveCompassSettings;
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x0016F838 File Offset: 0x0016DA38
	public NavObjectScreenSettings GetOnScreenSettings(bool isActive)
	{
		if (isActive)
		{
			return this.OnScreenSettings;
		}
		return this.InactiveOnScreenSettings;
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x0016F84A File Offset: 0x0016DA4A
	public NavObjectClass(string name)
	{
		this.NavObjectClassName = name;
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x0016F87C File Offset: 0x0016DA7C
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

	// Token: 0x060037EA RID: 14314 RVA: 0x0016F8C4 File Offset: 0x0016DAC4
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

	// Token: 0x04002DD9 RID: 11737
	public static List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002DDA RID: 11738
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04002DDB RID: 11739
	public string NavObjectClassName = "";

	// Token: 0x04002DDC RID: 11740
	public NavObjectClass.RequirementTypes RequirementType;

	// Token: 0x04002DDD RID: 11741
	public string RequirementName = "";

	// Token: 0x04002DDE RID: 11742
	public bool UseOverrideIcon;

	// Token: 0x04002DDF RID: 11743
	public string Tag;

	// Token: 0x04002DE0 RID: 11744
	public NavObjectMapSettings MapSettings;

	// Token: 0x04002DE1 RID: 11745
	public NavObjectCompassSettings CompassSettings;

	// Token: 0x04002DE2 RID: 11746
	public NavObjectScreenSettings OnScreenSettings;

	// Token: 0x04002DE3 RID: 11747
	public NavObjectMapSettings InactiveMapSettings;

	// Token: 0x04002DE4 RID: 11748
	public NavObjectCompassSettings InactiveCompassSettings;

	// Token: 0x04002DE5 RID: 11749
	public NavObjectScreenSettings InactiveOnScreenSettings;

	// Token: 0x02000752 RID: 1874
	public enum RequirementTypes
	{
		// Token: 0x04002DE7 RID: 11751
		None,
		// Token: 0x04002DE8 RID: 11752
		CVar,
		// Token: 0x04002DE9 RID: 11753
		QuestBounds,
		// Token: 0x04002DEA RID: 11754
		Tracking,
		// Token: 0x04002DEB RID: 11755
		NoTag,
		// Token: 0x04002DEC RID: 11756
		InParty,
		// Token: 0x04002DED RID: 11757
		IsAlly,
		// Token: 0x04002DEE RID: 11758
		IsPlayer,
		// Token: 0x04002DEF RID: 11759
		IsVehicleOwner,
		// Token: 0x04002DF0 RID: 11760
		IsOwner,
		// Token: 0x04002DF1 RID: 11761
		NoActiveQuests,
		// Token: 0x04002DF2 RID: 11762
		MinimumTreasureRadius,
		// Token: 0x04002DF3 RID: 11763
		IsTwitchSpawnedSelf,
		// Token: 0x04002DF4 RID: 11764
		IsTwitchSpawnedOther
	}
}
