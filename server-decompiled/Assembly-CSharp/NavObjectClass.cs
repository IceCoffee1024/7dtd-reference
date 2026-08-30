using System;
using System.Collections.Generic;

// Token: 0x02000747 RID: 1863
public class NavObjectClass
{
	// Token: 0x0600378E RID: 14222 RVA: 0x0016D5EC File Offset: 0x0016B7EC
	public static void Reset()
	{
		NavObjectClass.NavObjectClassList.Clear();
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x0016D5F8 File Offset: 0x0016B7F8
	public NavObjectMapSettings GetMapSettings(bool isActive)
	{
		if (isActive)
		{
			return this.MapSettings;
		}
		return this.InactiveMapSettings;
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x0016D60A File Offset: 0x0016B80A
	public NavObjectCompassSettings GetCompassSettings(bool isActive)
	{
		if (isActive)
		{
			return this.CompassSettings;
		}
		return this.InactiveCompassSettings;
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x0016D61C File Offset: 0x0016B81C
	public NavObjectScreenSettings GetOnScreenSettings(bool isActive)
	{
		if (isActive)
		{
			return this.OnScreenSettings;
		}
		return this.InactiveOnScreenSettings;
	}

	// Token: 0x06003792 RID: 14226 RVA: 0x0016D62E File Offset: 0x0016B82E
	public NavObjectClass(string name)
	{
		this.NavObjectClassName = name;
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x0016D660 File Offset: 0x0016B860
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

	// Token: 0x06003794 RID: 14228 RVA: 0x0016D6A8 File Offset: 0x0016B8A8
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

	// Token: 0x04002D61 RID: 11617
	public static List<NavObjectClass> NavObjectClassList = new List<NavObjectClass>();

	// Token: 0x04002D62 RID: 11618
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04002D63 RID: 11619
	public string NavObjectClassName = "";

	// Token: 0x04002D64 RID: 11620
	public NavObjectClass.RequirementTypes RequirementType;

	// Token: 0x04002D65 RID: 11621
	public string RequirementName = "";

	// Token: 0x04002D66 RID: 11622
	public bool UseOverrideIcon;

	// Token: 0x04002D67 RID: 11623
	public string Tag;

	// Token: 0x04002D68 RID: 11624
	public NavObjectMapSettings MapSettings;

	// Token: 0x04002D69 RID: 11625
	public NavObjectCompassSettings CompassSettings;

	// Token: 0x04002D6A RID: 11626
	public NavObjectScreenSettings OnScreenSettings;

	// Token: 0x04002D6B RID: 11627
	public NavObjectMapSettings InactiveMapSettings;

	// Token: 0x04002D6C RID: 11628
	public NavObjectCompassSettings InactiveCompassSettings;

	// Token: 0x04002D6D RID: 11629
	public NavObjectScreenSettings InactiveOnScreenSettings;

	// Token: 0x02000748 RID: 1864
	public enum RequirementTypes
	{
		// Token: 0x04002D6F RID: 11631
		None,
		// Token: 0x04002D70 RID: 11632
		CVar,
		// Token: 0x04002D71 RID: 11633
		QuestBounds,
		// Token: 0x04002D72 RID: 11634
		Tracking,
		// Token: 0x04002D73 RID: 11635
		NoTag,
		// Token: 0x04002D74 RID: 11636
		InParty,
		// Token: 0x04002D75 RID: 11637
		IsAlly,
		// Token: 0x04002D76 RID: 11638
		IsPlayer,
		// Token: 0x04002D77 RID: 11639
		IsVehicleOwner,
		// Token: 0x04002D78 RID: 11640
		IsOwner,
		// Token: 0x04002D79 RID: 11641
		NoActiveQuests,
		// Token: 0x04002D7A RID: 11642
		MinimumTreasureRadius,
		// Token: 0x04002D7B RID: 11643
		IsTwitchSpawnedSelf,
		// Token: 0x04002D7C RID: 11644
		IsTwitchSpawnedOther
	}
}
