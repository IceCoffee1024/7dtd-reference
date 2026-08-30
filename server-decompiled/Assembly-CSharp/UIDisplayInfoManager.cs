using System;
using System.Collections.Generic;

// Token: 0x020010D9 RID: 4313
public class UIDisplayInfoManager
{
	// Token: 0x17000FEF RID: 4079
	// (get) Token: 0x06008968 RID: 35176 RVA: 0x00346473 File Offset: 0x00344673
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

	// Token: 0x06008969 RID: 35177 RVA: 0x0034648B File Offset: 0x0034468B
	[PublicizedFrom(EAccessModifier.Private)]
	public UIDisplayInfoManager()
	{
	}

	// Token: 0x17000FF0 RID: 4080
	// (get) Token: 0x0600896A RID: 35178 RVA: 0x003464CA File Offset: 0x003446CA
	public static bool HasInstance
	{
		get
		{
			return UIDisplayInfoManager.instance != null;
		}
	}

	// Token: 0x0600896B RID: 35179 RVA: 0x003464D4 File Offset: 0x003446D4
	public static void Reset()
	{
		if (UIDisplayInfoManager.HasInstance)
		{
			UIDisplayInfoManager.Current.Cleanup();
		}
	}

	// Token: 0x0600896C RID: 35180 RVA: 0x003464E7 File Offset: 0x003446E7
	public bool ContainsItemDisplayStats(string tag)
	{
		return this.ItemDisplayStats.ContainsKey(tag);
	}

	// Token: 0x0600896D RID: 35181 RVA: 0x003464F5 File Offset: 0x003446F5
	public bool ContainsCraftingCategoryList(string tag)
	{
		return this.CraftingCategoryDisplayLists.ContainsKey(tag);
	}

	// Token: 0x0600896E RID: 35182 RVA: 0x00346503 File Offset: 0x00344703
	public ItemDisplayEntry GetDisplayStatsForTag(string tag)
	{
		if (this.ItemDisplayStats.ContainsKey(tag))
		{
			return this.ItemDisplayStats[tag];
		}
		return null;
	}

	// Token: 0x0600896F RID: 35183 RVA: 0x00346521 File Offset: 0x00344721
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

	// Token: 0x06008970 RID: 35184 RVA: 0x0034654C File Offset: 0x0034474C
	public void AddItemDisplayInfo(string tag, DisplayInfoEntry displayInfo)
	{
		this.ItemDisplayStats[tag].DisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x06008971 RID: 35185 RVA: 0x003465A6 File Offset: 0x003447A6
	public void Cleanup()
	{
		this.ItemDisplayStats.Clear();
		this.CharacterDisplayStats.Clear();
		this.CraftingCategoryDisplayLists.Clear();
	}

	// Token: 0x06008972 RID: 35186 RVA: 0x003465C9 File Offset: 0x003447C9
	public string GetLocalizedName(PassiveEffects statType)
	{
		if (this.StatLocalizationDictionary.ContainsKey(statType))
		{
			return this.StatLocalizationDictionary[statType];
		}
		return "";
	}

	// Token: 0x06008973 RID: 35187 RVA: 0x003465EC File Offset: 0x003447EC
	public void AddCharacterDisplayInfo(DisplayInfoEntry displayInfo)
	{
		this.CharacterDisplayStats.Add(displayInfo);
		if (!this.StatLocalizationDictionary.ContainsKey(displayInfo.StatType))
		{
			this.StatLocalizationDictionary.Add(displayInfo.StatType, Localization.Get(displayInfo.StatType.ToStringCached<PassiveEffects>(), false, null));
		}
	}

	// Token: 0x06008974 RID: 35188 RVA: 0x0034663B File Offset: 0x0034483B
	public List<DisplayInfoEntry> GetCharacterDisplayInfo()
	{
		return this.CharacterDisplayStats;
	}

	// Token: 0x06008975 RID: 35189 RVA: 0x00346643 File Offset: 0x00344843
	public void AddCraftingCategoryDisplayItem(string categoryListName, CraftingCategoryDisplayEntry entry)
	{
		if (!this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			this.CraftingCategoryDisplayLists.Add(categoryListName, new List<CraftingCategoryDisplayEntry>());
		}
		this.CraftingCategoryDisplayLists[categoryListName].Add(entry);
	}

	// Token: 0x06008976 RID: 35190 RVA: 0x00346676 File Offset: 0x00344876
	public List<CraftingCategoryDisplayEntry> GetCraftingCategoryDisplayList(string categoryListName)
	{
		if (this.CraftingCategoryDisplayLists.ContainsKey(categoryListName))
		{
			return this.CraftingCategoryDisplayLists[categoryListName];
		}
		return null;
	}

	// Token: 0x06008977 RID: 35191 RVA: 0x00346694 File Offset: 0x00344894
	public void AddTraderCategoryDIsplayItem(CraftingCategoryDisplayEntry entry)
	{
		if (!this.TraderCategoryDisplayDict.ContainsKey(entry.Name))
		{
			this.TraderCategoryDisplayDict.Add(entry.Name, entry);
		}
	}

	// Token: 0x06008978 RID: 35192 RVA: 0x003466BB File Offset: 0x003448BB
	public CraftingCategoryDisplayEntry GetTraderCategoryDisplay(string name)
	{
		if (this.TraderCategoryDisplayDict.ContainsKey(name))
		{
			return this.TraderCategoryDisplayDict[name];
		}
		return null;
	}

	// Token: 0x04006625 RID: 26149
	[PublicizedFrom(EAccessModifier.Private)]
	public static UIDisplayInfoManager instance;

	// Token: 0x04006626 RID: 26150
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, ItemDisplayEntry> ItemDisplayStats = new Dictionary<string, ItemDisplayEntry>();

	// Token: 0x04006627 RID: 26151
	[PublicizedFrom(EAccessModifier.Private)]
	public List<DisplayInfoEntry> CharacterDisplayStats = new List<DisplayInfoEntry>();

	// Token: 0x04006628 RID: 26152
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, List<CraftingCategoryDisplayEntry>> CraftingCategoryDisplayLists = new Dictionary<string, List<CraftingCategoryDisplayEntry>>();

	// Token: 0x04006629 RID: 26153
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, CraftingCategoryDisplayEntry> TraderCategoryDisplayDict = new Dictionary<string, CraftingCategoryDisplayEntry>();

	// Token: 0x0400662A RID: 26154
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PassiveEffects, string> StatLocalizationDictionary = new EnumDictionary<PassiveEffects, string>();
}
