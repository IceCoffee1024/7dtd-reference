using System;
using System.Collections.Generic;

// Token: 0x020010EA RID: 4330
public class UIDisplayInfoManager
{
	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x060089B6 RID: 35254 RVA: 0x0034A223 File Offset: 0x00348423
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

	// Token: 0x060089B7 RID: 35255 RVA: 0x0034A23B File Offset: 0x0034843B
	[PublicizedFrom(EAccessModifier.Private)]
	public UIDisplayInfoManager()
	{
	}

	// Token: 0x17001025 RID: 4133
	// (get) Token: 0x060089B8 RID: 35256 RVA: 0x0034A27A File Offset: 0x0034847A
	public static bool HasInstance
	{
		get
		{
			return UIDisplayInfoManager.instance != null;
		}
	}

	// Token: 0x060089B9 RID: 35257 RVA: 0x0034A284 File Offset: 0x00348484
	public static void Reset()
	{
		if (UIDisplayInfoManager.HasInstance)
		{
			UIDisplayInfoManager.Current.Cleanup();
		}
	}

	// Token: 0x060089BA RID: 35258 RVA: 0x0034A297 File Offset: 0x00348497
	public bool ContainsItemDisplayStats(string tag)
	{
		return this.ItemDisplayStats.ContainsKey(tag);
	}

	// Token: 0x060089BB RID: 35259 RVA: 0x0034A2A5 File Offset: 0x003484A5
	public bool ContainsCraftingCategoryList(string tag)
	{
		return this.CraftingCategoryDisplayLists.ContainsKey(tag);
	}

	// Token: 0x060089BC RID: 35260 RVA: 0x0034A2B3 File Offset: 0x003484B3
	public ItemDisplayEntry GetDisplayStatsForTag(string tag)
	{
		if (this.ItemDisplayStats.ContainsKey(tag))
		{
			return this.ItemDisplayStats[tag];
		}
		return null;
	}

	// Token: 0x060089BD RID: 35261 RVA: 0x0034A2D1 File Offset: 0x003484D1
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

	// Token: 0x060089BE RID: 35262 RVA: 0x0034A2FC File Offset: 0x003484FC
	public void AddItemDisplayInfo(string tag, DisplayInfoEntry displayInfo)
	{
		this.ItemDisplayStats[tag].DisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x060089BF RID: 35263 RVA: 0x0034A356 File Offset: 0x00348556
	public void Cleanup()
	{
		this.ItemDisplayStats.Clear();
		this.CharacterDisplayStats.Clear();
		this.CraftingCategoryDisplayLists.Clear();
	}

	// Token: 0x060089C0 RID: 35264 RVA: 0x0034A379 File Offset: 0x00348579
	public string GetLocalizedName(PassiveEffects statType)
	{
		if (this.StatLocalizationDictionary.ContainsKey(statType))
		{
			return this.StatLocalizationDictionary[statType];
		}
		return "";
	}

	// Token: 0x060089C1 RID: 35265 RVA: 0x0034A39C File Offset: 0x0034859C
	public void AddCharacterDisplayInfo(DisplayInfoEntry displayInfo)
	{
		this.CharacterDisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x060089C2 RID: 35266 RVA: 0x0034A3EB File Offset: 0x003485EB
	public List<DisplayInfoEntry> GetCharacterDisplayInfo()
	{
		return this.CharacterDisplayStats;
	}

	// Token: 0x060089C3 RID: 35267 RVA: 0x0034A3F3 File Offset: 0x003485F3
	public void AddCraftingCategoryDisplayItem(string categoryListName, CraftingCategoryDisplayEntry entry)
	{
		if (!this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			this.CraftingCategoryDisplayLists.Add(categoryListName, new List<CraftingCategoryDisplayEntry>());
		}
		this.CraftingCategoryDisplayLists[categoryListName].Add(entry);
	}

	// Token: 0x060089C4 RID: 35268 RVA: 0x0034A426 File Offset: 0x00348626
	public List<CraftingCategoryDisplayEntry> GetCraftingCategoryDisplayList(string categoryListName)
	{
		if (this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			return this.CraftingCategoryDisplayLists[categoryListName];
		}
		return null;
	}

	// Token: 0x060089C5 RID: 35269 RVA: 0x0034A444 File Offset: 0x00348644
	public void AddTraderCategoryDIsplayItem(CraftingCategoryDisplayEntry entry)
	{
		if (!this.TraderCategoryDisplayDict.ContainsKey(entry.Name))
		{
			this.TraderCategoryDisplayDict.Add(entry.Name, entry);
		}
	}

	// Token: 0x060089C6 RID: 35270 RVA: 0x0034A46B File Offset: 0x0034866B
	public CraftingCategoryDisplayEntry GetTraderCategoryDisplay(string name)
	{
		if (this.TraderCategoryDisplayDict.ContainsKey(name))
		{
			return this.TraderCategoryDisplayDict[name];
		}
		return null;
	}

	// Token: 0x04006667 RID: 26215
	[PublicizedFrom(EAccessModifier.Private)]
	public static UIDisplayInfoManager instance;

	// Token: 0x04006668 RID: 26216
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, ItemDisplayEntry> ItemDisplayStats = new Dictionary<string, ItemDisplayEntry>();

	// Token: 0x04006669 RID: 26217
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DisplayInfoEntry> CharacterDisplayStats = new List<DisplayInfoEntry>();

	// Token: 0x0400666A RID: 26218
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, List<CraftingCategoryDisplayEntry>> CraftingCategoryDisplayLists = new Dictionary<string, List<CraftingCategoryDisplayEntry>>();

	// Token: 0x0400666B RID: 26219
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, CraftingCategoryDisplayEntry> TraderCategoryDisplayDict = new Dictionary<string, CraftingCategoryDisplayEntry>();

	// Token: 0x0400666C RID: 26220
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, string> StatLocalizationDictionary = new EnumDictionary<PassiveEffects, string>();
}
