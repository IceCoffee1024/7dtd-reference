using System;
using System.Collections.Generic;

// Token: 0x020010FA RID: 4346
public class UIDisplayInfoManager
{
	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06008A09 RID: 35337 RVA: 0x0034C2A7 File Offset: 0x0034A4A7
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

	// Token: 0x06008A0A RID: 35338 RVA: 0x0034C2C0 File Offset: 0x0034A4C0
	[PublicizedFrom(EAccessModifier.Private)]
	public UIDisplayInfoManager()
	{
	}

	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x06008A0B RID: 35339 RVA: 0x0034C315 File Offset: 0x0034A515
	public static bool HasInstance
	{
		get
		{
			return UIDisplayInfoManager.instance != null;
		}
	}

	// Token: 0x06008A0C RID: 35340 RVA: 0x0034C31F File Offset: 0x0034A51F
	public static void Reset()
	{
		if (UIDisplayInfoManager.HasInstance)
		{
			UIDisplayInfoManager.Current.Cleanup();
		}
	}

	// Token: 0x06008A0D RID: 35341 RVA: 0x0034C332 File Offset: 0x0034A532
	public bool ContainsItemDisplayStats(string tag)
	{
		return this.ItemDisplayStats.ContainsKey(tag);
	}

	// Token: 0x06008A0E RID: 35342 RVA: 0x0034C340 File Offset: 0x0034A540
	public bool ContainsCraftingCategoryList(string tag)
	{
		return this.CraftingCategoryDisplayLists.ContainsKey(tag);
	}

	// Token: 0x06008A0F RID: 35343 RVA: 0x0034C34E File Offset: 0x0034A54E
	public ItemDisplayEntry GetDisplayStatsForTag(string tag)
	{
		if (this.ItemDisplayStats.ContainsKey(tag))
		{
			return this.ItemDisplayStats[tag];
		}
		return null;
	}

	// Token: 0x06008A10 RID: 35344 RVA: 0x0034C36C File Offset: 0x0034A56C
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

	// Token: 0x06008A11 RID: 35345 RVA: 0x0034C394 File Offset: 0x0034A594
	public void AddItemDisplayInfo(string tag, DisplayInfoEntry displayInfo)
	{
		this.ItemDisplayStats[tag].DisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x06008A12 RID: 35346 RVA: 0x0034C3EE File Offset: 0x0034A5EE
	public void Cleanup()
	{
		this.ItemDisplayStats.Clear();
		this.CharacterDisplayStats.Clear();
		this.CraftingCategoryDisplayLists.Clear();
	}

	// Token: 0x06008A13 RID: 35347 RVA: 0x0034C411 File Offset: 0x0034A611
	public string GetLocalizedName(PassiveEffects statType)
	{
		if (this.StatLocalizationDictionary.ContainsKey(statType))
		{
			return this.StatLocalizationDictionary[statType];
		}
		return "";
	}

	// Token: 0x06008A14 RID: 35348 RVA: 0x0034C434 File Offset: 0x0034A634
	public void AddCharacterDisplayInfo(DisplayInfoEntry displayInfo)
	{
		this.CharacterDisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x06008A15 RID: 35349 RVA: 0x0034C483 File Offset: 0x0034A683
	public List<DisplayInfoEntry> GetCharacterDisplayInfo()
	{
		return this.CharacterDisplayStats;
	}

	// Token: 0x06008A16 RID: 35350 RVA: 0x0034C48B File Offset: 0x0034A68B
	public void AddCraftingCategoryDisplayItem(string categoryListName, CraftingCategoryDisplayEntry entry)
	{
		if (!this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			this.CraftingCategoryDisplayLists.Add(categoryListName, new List<CraftingCategoryDisplayEntry>());
		}
		this.CraftingCategoryDisplayLists[categoryListName].Add(entry);
	}

	// Token: 0x06008A17 RID: 35351 RVA: 0x0034C4BE File Offset: 0x0034A6BE
	public List<CraftingCategoryDisplayEntry> GetCraftingCategoryDisplayList(string categoryListName)
	{
		if (this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			return this.CraftingCategoryDisplayLists[categoryListName];
		}
		return null;
	}

	// Token: 0x06008A18 RID: 35352 RVA: 0x0034C4DC File Offset: 0x0034A6DC
	public void AddTraderCategoryDIsplayItem(CraftingCategoryDisplayEntry entry)
	{
		if (!this.TraderCategoryDisplayDict.ContainsKey(entry.Name))
		{
			this.TraderCategoryDisplayDict.Add(entry.Name, entry);
			this.TraderCategoryDisplayList.Add(entry);
		}
	}

	// Token: 0x06008A19 RID: 35353 RVA: 0x0034C50F File Offset: 0x0034A70F
	public CraftingCategoryDisplayEntry GetTraderCategoryDisplay(string name)
	{
		if (this.TraderCategoryDisplayDict.ContainsKey(name))
		{
			return this.TraderCategoryDisplayDict[name];
		}
		return null;
	}

	// Token: 0x06008A1A RID: 35354 RVA: 0x0034C52D File Offset: 0x0034A72D
	public IReadOnlyList<CraftingCategoryDisplayEntry> GetTraderCategoryDisplayList()
	{
		return this.TraderCategoryDisplayList.AsReadOnly();
	}

	// Token: 0x040066A6 RID: 26278
	[PublicizedFrom(EAccessModifier.Private)]
	public static UIDisplayInfoManager instance;

	// Token: 0x040066A7 RID: 26279
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, ItemDisplayEntry> ItemDisplayStats = new Dictionary<string, ItemDisplayEntry>();

	// Token: 0x040066A8 RID: 26280
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DisplayInfoEntry> CharacterDisplayStats = new List<DisplayInfoEntry>();

	// Token: 0x040066A9 RID: 26281
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, List<CraftingCategoryDisplayEntry>> CraftingCategoryDisplayLists = new Dictionary<string, List<CraftingCategoryDisplayEntry>>();

	// Token: 0x040066AA RID: 26282
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, CraftingCategoryDisplayEntry> TraderCategoryDisplayDict = new Dictionary<string, CraftingCategoryDisplayEntry>();

	// Token: 0x040066AB RID: 26283
	[PublicizedFrom(EAccessModifier.Private)]
	public List<CraftingCategoryDisplayEntry> TraderCategoryDisplayList = new List<CraftingCategoryDisplayEntry>();

	// Token: 0x040066AC RID: 26284
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, string> StatLocalizationDictionary = new EnumDictionary<PassiveEffects, string>();
}
