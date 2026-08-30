using System;
using System.Collections.Generic;

// Token: 0x020010C0 RID: 4288
public class UIDisplayInfoManager
{
	// Token: 0x17000FB6 RID: 4022
	// (get) Token: 0x06008876 RID: 34934 RVA: 0x00341C4F File Offset: 0x0033FE4F
	public static UIDisplayInfoManager Current
	{
		get
		{
			if (UIDisplayInfoManager.instance == null)
			{
				UIDisplayInfoManager.instance = new UIDisplayInfoManager();
			}
			return UIDisplayInfoManager.instance;
		}
	}

	// Token: 0x06008877 RID: 34935 RVA: 0x00341C67 File Offset: 0x0033FE67
	[PublicizedFrom(EAccessModifier.Private)]
	public UIDisplayInfoManager()
	{
	}

	// Token: 0x17000FB7 RID: 4023
	// (get) Token: 0x06008878 RID: 34936 RVA: 0x00341CA6 File Offset: 0x0033FEA6
	public static bool HasInstance
	{
		get
		{
			return UIDisplayInfoManager.instance != null;
		}
	}

	// Token: 0x06008879 RID: 34937 RVA: 0x00341CB0 File Offset: 0x0033FEB0
	public static void Reset()
	{
		if (UIDisplayInfoManager.HasInstance)
		{
			UIDisplayInfoManager.Current.Cleanup();
		}
	}

	// Token: 0x0600887A RID: 34938 RVA: 0x00341CC3 File Offset: 0x0033FEC3
	public bool ContainsItemDisplayStats(string tag)
	{
		return this.ItemDisplayStats.ContainsKey(tag);
	}

	// Token: 0x0600887B RID: 34939 RVA: 0x00341CD1 File Offset: 0x0033FED1
	public bool ContainsCraftingCategoryList(string tag)
	{
		return this.CraftingCategoryDisplayLists.ContainsKey(tag);
	}

	// Token: 0x0600887C RID: 34940 RVA: 0x00341CDF File Offset: 0x0033FEDF
	public ItemDisplayEntry GetDisplayStatsForTag(string tag)
	{
		if (this.ItemDisplayStats.ContainsKey(tag))
		{
			return this.ItemDisplayStats[tag];
		}
		return null;
	}

	// Token: 0x0600887D RID: 34941 RVA: 0x00341CFD File Offset: 0x0033FEFD
	public void AddItemDisplayStats(string tag, string group)
	{
		if (!this.ItemDisplayStats.ContainsKey(tag))
		{
			this.ItemDisplayStats.Add(tag, new ItemDisplayEntry
			{
				DisplayGroup = group
			});
		}
	}

	// Token: 0x0600887E RID: 34942 RVA: 0x00341D28 File Offset: 0x0033FF28
	public void AddItemDisplayInfo(string tag, DisplayInfoEntry displayInfo)
	{
		this.ItemDisplayStats[tag].DisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x0600887F RID: 34943 RVA: 0x00341D82 File Offset: 0x0033FF82
	public void Cleanup()
	{
		this.ItemDisplayStats.Clear();
		this.CharacterDisplayStats.Clear();
		this.CraftingCategoryDisplayLists.Clear();
	}

	// Token: 0x06008880 RID: 34944 RVA: 0x00341DA5 File Offset: 0x0033FFA5
	public string GetLocalizedName(PassiveEffects statType)
	{
		if (this.StatLocalizationDictionary.ContainsKey(statType))
		{
			return this.StatLocalizationDictionary[statType];
		}
		return "";
	}

	// Token: 0x06008881 RID: 34945 RVA: 0x00341DC8 File Offset: 0x0033FFC8
	public void AddCharacterDisplayInfo(DisplayInfoEntry displayInfo)
	{
		this.CharacterDisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x06008882 RID: 34946 RVA: 0x00341E17 File Offset: 0x00340017
	public List<DisplayInfoEntry> GetCharacterDisplayInfo()
	{
		return this.CharacterDisplayStats;
	}

	// Token: 0x06008883 RID: 34947 RVA: 0x00341E1F File Offset: 0x0034001F
	public void AddCraftingCategoryDisplayItem(string categoryListName, CraftingCategoryDisplayEntry entry)
	{
		if (!this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			this.CraftingCategoryDisplayLists.Add(categoryListName, new List<CraftingCategoryDisplayEntry>());
		}
		this.CraftingCategoryDisplayLists[categoryListName].Add(entry);
	}

	// Token: 0x06008884 RID: 34948 RVA: 0x00341E52 File Offset: 0x00340052
	public List<CraftingCategoryDisplayEntry> GetCraftingCategoryDisplayList(string categoryListName)
	{
		if (this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			return this.CraftingCategoryDisplayLists[categoryListName];
		}
		return null;
	}

	// Token: 0x06008885 RID: 34949 RVA: 0x00341E70 File Offset: 0x00340070
	public void AddTraderCategoryDIsplayItem(CraftingCategoryDisplayEntry entry)
	{
		if (!this.TraderCategoryDisplayDict.ContainsKey(entry.Name))
		{
			this.TraderCategoryDisplayDict.Add(entry.Name, entry);
		}
	}

	// Token: 0x06008886 RID: 34950 RVA: 0x00341E97 File Offset: 0x00340097
	public CraftingCategoryDisplayEntry GetTraderCategoryDisplay(string name)
	{
		if (this.TraderCategoryDisplayDict.ContainsKey(name))
		{
			return this.TraderCategoryDisplayDict[name];
		}
		return null;
	}

	// Token: 0x0400655E RID: 25950
	[PublicizedFrom(EAccessModifier.Private)]
	public static UIDisplayInfoManager instance;

	// Token: 0x0400655F RID: 25951
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, ItemDisplayEntry> ItemDisplayStats = new Dictionary<string, ItemDisplayEntry>();

	// Token: 0x04006560 RID: 25952
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DisplayInfoEntry> CharacterDisplayStats = new List<DisplayInfoEntry>();

	// Token: 0x04006561 RID: 25953
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, List<CraftingCategoryDisplayEntry>> CraftingCategoryDisplayLists = new Dictionary<string, List<CraftingCategoryDisplayEntry>>();

	// Token: 0x04006562 RID: 25954
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, CraftingCategoryDisplayEntry> TraderCategoryDisplayDict = new Dictionary<string, CraftingCategoryDisplayEntry>();

	// Token: 0x04006563 RID: 25955
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, string> StatLocalizationDictionary = new EnumDictionary<PassiveEffects, string>();
}
