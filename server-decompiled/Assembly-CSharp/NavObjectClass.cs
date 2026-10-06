using System;
using System.Collections.Generic;

// Token: 0x02000753 RID: 1875
public class NavObjectClass
{
	// Token: 0x06003827 RID: 14375 RVA: 0x0017016C File Offset: 0x0016E36C
	public static void Reset()
	{
		NavObjectClass.NavObjectClassList.Clear();
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x00170178 File Offset: 0x0016E378
	public NavObjectMapSettings GetMapSettings(bool isActive)
	{
		if (isActive)
		{
			return this.MapSettings;
		}
		return this.InactiveMapSettings;
	}

	// Token: 0x06003829 RID: 14377 RVA: 0x0017018A File Offset: 0x0016E38A
	public NavObjectCompassSettings GetCompassSettings(bool isActive)
	{
		if (isActive)
		{
			return this.CompassSettings;
		}
		return this.InactiveCompassSettings;
	}

	// Token: 0x0600382A RID: 14378 RVA: 0x0017019C File Offset: 0x0016E39C
	public NavObjectScreenSettings GetOnScreenSettings(bool isActive)
	{
		if (isActive)
		{
			return this.OnScreenSettings;
		}
		return this.InactiveOnScreenSettings;
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x001701AE File Offset: 0x0016E3AE
	public NavObjectClass(string name)
	{
		this.NavObjectClassName = name;
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x001701E0 File Offset: 0x0016E3E0
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

	// Token: 0x0600382D RID: 14381 RVA: 0x00170228 File Offset: 0x0016E428
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

	// Token: 0x04002DD2 RID: 11730
	public static List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002DD3 RID: 11731
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04002DD4 RID: 11732
	public string NavObjectClassName = "";

	// Token: 0x04002DD5 RID: 11733
	public NavObjectClass.RequirementTypes RequirementType;

	// Token: 0x04002DD6 RID: 11734
	public string RequirementName = "";

	// Token: 0x04002DD7 RID: 11735
	public bool UseOverrideIcon;

	// Token: 0x04002DD8 RID: 11736
	public string Tag;

	// Token: 0x04002DD9 RID: 11737
	public NavObjectMapSettings MapSettings;

	// Token: 0x04002DDA RID: 11738
	public NavObjectCompassSettings CompassSettings;

	// Token: 0x04002DDB RID: 11739
	public NavObjectScreenSettings OnScreenSettings;

	// Token: 0x04002DDC RID: 11740
	public NavObjectMapSettings InactiveMapSettings;

	// Token: 0x04002DDD RID: 11741
	public NavObjectCompassSettings InactiveCompassSettings;

	// Token: 0x04002DDE RID: 11742
	public NavObjectScreenSettings InactiveOnScreenSettings;

	// Token: 0x02000754 RID: 1876
	public enum RequirementTypes
	{
		// Token: 0x04002DE0 RID: 11744
		None,
		// Token: 0x04002DE1 RID: 11745
		CVar,
		// Token: 0x04002DE2 RID: 11746
		QuestBounds,
		// Token: 0x04002DE3 RID: 11747
		Tracking,
		// Token: 0x04002DE4 RID: 11748
		NoTag,
		// Token: 0x04002DE5 RID: 11749
		InParty,
		// Token: 0x04002DE6 RID: 11750
		IsAlly,
		// Token: 0x04002DE7 RID: 11751
		IsPlayer,
		// Token: 0x04002DE8 RID: 11752
		IsVehicleOwner,
		// Token: 0x04002DE9 RID: 11753
		IsOwner,
		// Token: 0x04002DEA RID: 11754
		NoActiveQuests,
		// Token: 0x04002DEB RID: 11755
		MinimumTreasureRadius,
		// Token: 0x04002DEC RID: 11756
		IsTwitchSpawnedSelf,
		// Token: 0x04002DED RID: 11757
		IsTwitchSpawnedOther
	}
}
