using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ACF RID: 2767
public class ProgressionClass
{
	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x060052C8 RID: 21192 RVA: 0x001FA3B8 File Offset: 0x001F85B8
	public bool IsBookGroup
	{
		get
		{
			return this.Type == ProgressionType.BookGroup;
		}
	}

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x060052C9 RID: 21193 RVA: 0x001FA3C3 File Offset: 0x001F85C3
	public bool IsBook
	{
		get
		{
			return this.Type == ProgressionType.Book;
		}
	}

	// Token: 0x060052CA RID: 21194 RVA: 0x001FA3D0 File Offset: 0x001F85D0
	public bool ValidDisplay(ProgressionClass.DisplayTypes displayType)
	{
		switch (displayType)
		{
		case ProgressionClass.DisplayTypes.Standard:
			return this.Type != ProgressionType.BookGroup && this.Type != ProgressionType.Crafting;
		case ProgressionClass.DisplayTypes.Book:
			return this.Type == ProgressionType.BookGroup;
		case ProgressionClass.DisplayTypes.Crafting:
			return this.Type == ProgressionType.Crafting;
		default:
			return false;
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x060052CB RID: 21195 RVA: 0x001FA420 File Offset: 0x001F8620
	public ProgressionCurrencyType CurrencyType
	{
		get
		{
			switch (this.Type)
			{
			case ProgressionType.Attribute:
				return ProgressionCurrencyType.SP;
			case ProgressionType.Skill:
				return ProgressionCurrencyType.XP;
			case ProgressionType.Perk:
				return ProgressionCurrencyType.SP;
			default:
				return ProgressionCurrencyType.None;
			}
		}
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x060052CC RID: 21196 RVA: 0x001FA454 File Offset: 0x001F8654
	public ProgressionClass Parent
	{
		get
		{
			if (this.ParentName == null)
			{
				return this;
			}
			ProgressionClass progressionClass;
			if (Progression.ProgressionClasses.TryGetValue(this.ParentName, out progressionClass))
			{
				return progressionClass;
			}
			return null;
		}
	}

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x060052CD RID: 21197 RVA: 0x001FA482 File Offset: 0x001F8682
	public bool IsPerk
	{
		get
		{
			return this.Type == ProgressionType.Perk;
		}
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x060052CE RID: 21198 RVA: 0x001FA48D File Offset: 0x001F868D
	public bool IsSkill
	{
		get
		{
			return this.Type == ProgressionType.Skill;
		}
	}

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x060052CF RID: 21199 RVA: 0x001FA498 File Offset: 0x001F8698
	public bool IsAttribute
	{
		get
		{
			return this.Type == ProgressionType.Attribute;
		}
	}

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x060052D0 RID: 21200 RVA: 0x001FA4A3 File Offset: 0x001F86A3
	public bool IsCrafting
	{
		get
		{
			return this.Type == ProgressionType.Crafting;
		}
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x060052D1 RID: 21201 RVA: 0x001FA4B0 File Offset: 0x001F86B0
	// (set) Token: 0x060052D2 RID: 21202 RVA: 0x001FA505 File Offset: 0x001F8705
	public float ListSortOrder
	{
		get
		{
			if (this.IsPerk)
			{
				return this.Parent.ListSortOrder + this.listSortOrder * 0.001f;
			}
			if (this.IsSkill)
			{
				return this.Parent.ListSortOrder + this.listSortOrder;
			}
			return this.listSortOrder * 100f;
		}
		set
		{
			this.listSortOrder = value;
		}
	}

	// Token: 0x060052D3 RID: 21203 RVA: 0x001FA510 File Offset: 0x001F8710
	public ProgressionClass.DisplayData AddDisplayData(string _item, int[] _qualityStarts, string[] _customIcon, string[] _customIconTint, string[] _customName, bool _customHasQuality)
	{
		if (this.DisplayDataList == null)
		{
			this.DisplayDataList = new List<ProgressionClass.DisplayData>();
		}
		ProgressionClass.DisplayData displayData = new ProgressionClass.DisplayData
		{
			ItemName = _item,
			QualityStarts = _qualityStarts,
			Owner = this,
			CustomIcon = _customIcon,
			CustomIconTint = _customIconTint,
			CustomName = _customName,
			CustomHasQuality = _customHasQuality
		};
		this.DisplayDataList.Add(displayData);
		return displayData;
	}

	// Token: 0x060052D4 RID: 21204 RVA: 0x001FA578 File Offset: 0x001F8778
	public ProgressionClass(string _name)
	{
		this.Name = _name;
		this.NameKey = this.Name;
		this.NameTag = FastTags<TagGroup.Global>.GetTag(_name);
		this.DescKey = "";
		this.ListSortOrder = float.MaxValue;
		this.ParentName = null;
		this.Type = ProgressionType.None;
	}

	// Token: 0x060052D5 RID: 21205 RVA: 0x001FA5F6 File Offset: 0x001F87F6
	public void ModifyValue(EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x060052D6 RID: 21206 RVA: 0x001FA61C File Offset: 0x001F881C
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.GetModifiedValueData(_modValueSources, _sourceType, _ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x060052D7 RID: 21207 RVA: 0x001FA651 File Offset: 0x001F8851
	public bool HasEvents()
	{
		return this.Effects != null && this.Effects.HasEvents();
	}

	// Token: 0x060052D8 RID: 21208 RVA: 0x001FA668 File Offset: 0x001F8868
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.Effects != null)
		{
			this.Effects.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x060052D9 RID: 21209 RVA: 0x001FA67F File Offset: 0x001F887F
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canRun(RequirementGroup Requirements, MinEventParams _params)
	{
		return Requirements == null || Requirements.IsValid(_params);
	}

	// Token: 0x060052DA RID: 21210 RVA: 0x001FA690 File Offset: 0x001F8890
	public LevelRequirement GetRequirementsForLevel(int _level)
	{
		if (this.LevelRequirements.Count <= 0)
		{
			return new LevelRequirement(_level);
		}
		LevelRequirement levelRequirement;
		if (this.LevelRequirements.dict.TryGetValue(_level, out levelRequirement))
		{
			return levelRequirement;
		}
		return new LevelRequirement(_level);
	}

	// Token: 0x060052DB RID: 21211 RVA: 0x001FA6CF File Offset: 0x001F88CF
	public void AddLevelRequirement(LevelRequirement _lr)
	{
		this.LevelRequirements.Add(_lr.Level, _lr);
	}

	// Token: 0x060052DC RID: 21212 RVA: 0x001FA6E3 File Offset: 0x001F88E3
	public void PostInit()
	{
		this.LevelRequirements.list.Sort((LevelRequirement a, LevelRequirement b) => a.Level - b.Level);
	}

	// Token: 0x060052DD RID: 21213 RVA: 0x001FA714 File Offset: 0x001F8914
	public static int GetCalculatedMaxLevel(EntityAlive _ea, ProgressionValue _pv)
	{
		ProgressionClass progressionClass = _pv.ProgressionClass;
		int num = 0;
		if (progressionClass.LevelRequirements.Count > 0)
		{
			List<LevelRequirement> list = progressionClass.LevelRequirements.list;
			int i = 0;
			int num2 = list.Count;
			while (i < num2)
			{
				int num3 = (num2 + i) / 2;
				LevelRequirement levelRequirement = list[num3];
				if (ProgressionClass.canRun(levelRequirement.Requirements, _ea.MinEventContext))
				{
					num = levelRequirement.Level;
					i = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
			}
			if (!progressionClass.IsAttribute)
			{
				if (num < progressionClass.MinLevel)
				{
					num = progressionClass.MinLevel;
				}
				if (num > progressionClass.MaxLevel)
				{
					num = progressionClass.MaxLevel;
				}
			}
		}
		else if (progressionClass.IsAttribute)
		{
			num = 20;
		}
		else
		{
			num = progressionClass.MaxLevel;
		}
		return num;
	}

	// Token: 0x060052DE RID: 21214 RVA: 0x001FA7D0 File Offset: 0x001F89D0
	public int CalculatedCostForLevel(int _level)
	{
		if (this.OverrideCost == null)
		{
			return (int)(Mathf.Pow(this.CostMultiplier, (float)_level) * (float)this.BaseCostToLevel);
		}
		if (_level - 1 < this.OverrideCost.Length)
		{
			return this.OverrideCost[_level - 1];
		}
		return 0;
	}

	// Token: 0x060052DF RID: 21215 RVA: 0x001FA80C File Offset: 0x001F8A0C
	public float GetPercentThisLevel(ProgressionValue _pv)
	{
		if (this.Type != ProgressionType.Skill)
		{
			return 0f;
		}
		if (_pv.Level == this.MaxLevel)
		{
			return 0f;
		}
		float num = (float)((int)(Mathf.Pow(this.CostMultiplier, (float)_pv.Level) * (float)this.BaseCostToLevel) - _pv.CostForNextLevel) / (Mathf.Pow(this.CostMultiplier, (float)_pv.Level) * (float)this.BaseCostToLevel);
		if (!float.IsNaN(num))
		{
			return num;
		}
		return 0f;
	}

	// Token: 0x060052E0 RID: 21216 RVA: 0x001FA88C File Offset: 0x001F8A8C
	public void HandleCheckCrafting(EntityPlayerLocal _player, int _oldLevel, int _newLevel)
	{
		if (this.DisplayDataList != null)
		{
			for (int i = 0; i < this.DisplayDataList.Count; i++)
			{
				ProgressionClass.DisplayData displayData = this.DisplayDataList[i];
				if (displayData.Enabled)
				{
					displayData.HandleCheckCrafting(_player, _oldLevel, _newLevel);
				}
			}
		}
	}

	// Token: 0x060052E1 RID: 21217 RVA: 0x001FA8D8 File Offset: 0x001F8AD8
	public void UpdateForSandbox()
	{
		if (this.DisplayDataList != null)
		{
			ProgressionClass.DisplayData displayData = null;
			this.Enabled = false;
			for (int i = this.DisplayDataList.Count - 1; i >= 0; i--)
			{
				ProgressionClass.DisplayData displayData2 = this.DisplayDataList[i];
				if (displayData2.HandleCheckEnabled())
				{
					this.Enabled = true;
					if (displayData != null)
					{
						this.MaxLevel = displayData2.QualityStarts[displayData2.QualityStarts.Length - 1];
						displayData = null;
					}
				}
				else
				{
					displayData = displayData2;
				}
			}
		}
	}

	// Token: 0x060052E2 RID: 21218 RVA: 0x001FA94A File Offset: 0x001F8B4A
	public override string ToString()
	{
		return string.Format("{0}, {1}, lvl {2} to {3}", new object[]
		{
			base.ToString(),
			this.Name,
			this.MinLevel,
			this.MaxLevel
		});
	}

	// Token: 0x0400408A RID: 16522
	public readonly string Name;

	// Token: 0x0400408B RID: 16523
	public readonly FastTags<TagGroup.Global> NameTag;

	// Token: 0x0400408C RID: 16524
	public float ParentMaxLevelRatio = 1f;

	// Token: 0x0400408D RID: 16525
	public string NameKey;

	// Token: 0x0400408E RID: 16526
	public string DescKey;

	// Token: 0x0400408F RID: 16527
	public string LongDescKey;

	// Token: 0x04004090 RID: 16528
	public string Icon;

	// Token: 0x04004091 RID: 16529
	public int MinLevel;

	// Token: 0x04004092 RID: 16530
	public int MaxLevel;

	// Token: 0x04004093 RID: 16531
	public int BaseCostToLevel;

	// Token: 0x04004094 RID: 16532
	public float CostMultiplier;

	// Token: 0x04004095 RID: 16533
	public bool Hidden;

	// Token: 0x04004096 RID: 16534
	public int[] OverrideCost;

	// Token: 0x04004097 RID: 16535
	public bool Enabled = true;

	// Token: 0x04004098 RID: 16536
	public ProgressionClass.DisplayTypes DisplayType;

	// Token: 0x04004099 RID: 16537
	public MinEffectController Effects;

	// Token: 0x0400409A RID: 16538
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryList<int, LevelRequirement> LevelRequirements = new DictionaryList<int, LevelRequirement>();

	// Token: 0x0400409B RID: 16539
	public readonly List<ProgressionClass> Children = new List<ProgressionClass>();

	// Token: 0x0400409C RID: 16540
	public string ParentName;

	// Token: 0x0400409D RID: 16541
	public ProgressionType Type;

	// Token: 0x0400409E RID: 16542
	[PublicizedFrom(EAccessModifier.Private)]
	public float listSortOrder;

	// Token: 0x0400409F RID: 16543
	public List<ProgressionClass.DisplayData> DisplayDataList;

	// Token: 0x02000AD0 RID: 2768
	public enum DisplayTypes
	{
		// Token: 0x040040A1 RID: 16545
		Standard,
		// Token: 0x040040A2 RID: 16546
		Book,
		// Token: 0x040040A3 RID: 16547
		Crafting
	}

	// Token: 0x02000AD1 RID: 2769
	public class ListSortOrderComparer : IComparer<ProgressionValue>
	{
		// Token: 0x060052E3 RID: 21219 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public ListSortOrderComparer()
		{
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x001FA98C File Offset: 0x001F8B8C
		public int Compare(ProgressionValue _x, ProgressionValue _y)
		{
			return _x.ProgressionClass.ListSortOrder.CompareTo(_y.ProgressionClass.ListSortOrder);
		}

		// Token: 0x040040A4 RID: 16548
		public static ProgressionClass.ListSortOrderComparer Instance = new ProgressionClass.ListSortOrderComparer();
	}

	// Token: 0x02000AD2 RID: 2770
	public class DisplayData
	{
		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x001FA9C3 File Offset: 0x001F8BC3
		public ItemClass Item
		{
			get
			{
				if (this.item == null)
				{
					this.item = ItemClass.GetItemClass(this.ItemName, false);
				}
				return this.item;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060052E7 RID: 21223 RVA: 0x001FA9E5 File Offset: 0x001F8BE5
		public bool HasQuality
		{
			get
			{
				if (this.ItemName != "")
				{
					return this.Item.HasQuality;
				}
				return this.CustomHasQuality;
			}
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x001FAA0C File Offset: 0x001F8C0C
		public string GetName(int level)
		{
			if (this.ItemName != "")
			{
				return this.Item.GetLocalizedItemName();
			}
			if (this.CustomName == null)
			{
				return "";
			}
			int num = this.GetQualityLevel(level);
			if (num >= this.CustomName.Length)
			{
				num = 0;
			}
			return this.CustomName[num];
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x001FAA64 File Offset: 0x001F8C64
		public string GetIcon(int level)
		{
			if (this.ItemName != "")
			{
				return this.Item.GetIconName();
			}
			if (this.CustomIcon == null)
			{
				return "";
			}
			int num = this.GetQualityLevel(level);
			if (num >= this.CustomIcon.Length)
			{
				num = 0;
			}
			return this.CustomIcon[num];
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x001FAABC File Offset: 0x001F8CBC
		public string GetIconTint(int level)
		{
			if (this.ItemName != "")
			{
				return Utils.ColorToHex(this.Item.GetIconTint(null));
			}
			if (this.CustomIconTint == null)
			{
				return "FFFFFF";
			}
			int num = this.GetQualityLevel(level);
			if (num >= this.CustomName.Length)
			{
				num = 0;
			}
			return this.CustomIconTint[num];
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x001FAB20 File Offset: 0x001F8D20
		public int GetQualityLevel(int level)
		{
			for (int i = 0; i < this.QualityStarts.Length; i++)
			{
				if (this.QualityStarts[i] > level)
				{
					return i;
				}
			}
			return this.QualityStarts.Length;
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x001FAB58 File Offset: 0x001F8D58
		public int GetNextPoints(int level)
		{
			for (int i = 0; i < this.QualityStarts.Length; i++)
			{
				if (this.QualityStarts[i] > level)
				{
					return this.QualityStarts[i];
				}
			}
			return 0;
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x001FAB90 File Offset: 0x001F8D90
		public bool IsComplete(int level)
		{
			for (int i = 0; i < this.QualityStarts.Length; i++)
			{
				if (this.QualityStarts[i] > level)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x001FABBE File Offset: 0x001F8DBE
		public void AddUnlockData(string itemName, int unlockTier, string[] recipeList)
		{
			if (this.UnlockDataList == null)
			{
				this.UnlockDataList = new List<ProgressionClass.DisplayData.UnlockData>();
			}
			this.UnlockDataList.Add(new ProgressionClass.DisplayData.UnlockData
			{
				ItemName = itemName,
				UnlockTier = unlockTier,
				RecipeList = recipeList
			});
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x001FABF8 File Offset: 0x001F8DF8
		public ItemClass GetUnlockItem(int index)
		{
			if (this.UnlockDataList == null)
			{
				return null;
			}
			if (index >= this.UnlockDataList.Count)
			{
				return null;
			}
			ProgressionClass.DisplayData.UnlockData unlockData = this.UnlockDataList[index];
			if (!unlockData.Enabled)
			{
				return null;
			}
			return unlockData.Item;
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x001FAC3C File Offset: 0x001F8E3C
		[PublicizedFrom(EAccessModifier.Private)]
		public ProgressionClass.DisplayData.UnlockData GetUnlockData(int index)
		{
			if (this.UnlockDataList == null)
			{
				return null;
			}
			if (index >= this.UnlockDataList.Count)
			{
				return null;
			}
			ProgressionClass.DisplayData.UnlockData unlockData = this.UnlockDataList[index];
			if (!unlockData.Enabled)
			{
				return null;
			}
			return unlockData;
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x001FAC7C File Offset: 0x001F8E7C
		public string GetUnlockItemIconName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetIconName();
			}
			return "";
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x001FACA0 File Offset: 0x001F8EA0
		public string GetUnlockItemName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetLocalizedItemName();
			}
			return "";
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x001FACC4 File Offset: 0x001F8EC4
		public List<int> GetUnlockItemRecipes(int index)
		{
			if (this.UnlockDataList == null)
			{
				return null;
			}
			if (index >= this.UnlockDataList.Count)
			{
				return null;
			}
			List<int> list = new List<int>();
			if (this.Item != null)
			{
				list.Add(this.Item.Id);
			}
			else
			{
				ProgressionClass.DisplayData.UnlockData unlockData = this.UnlockDataList[index];
				if (unlockData != null)
				{
					if (unlockData.RecipeList != null)
					{
						for (int i = 0; i < unlockData.RecipeList.Length; i++)
						{
							list.Add(ItemClass.GetItemClass(unlockData.RecipeList[i], true).Id);
						}
					}
					else if (unlockData.Item != null)
					{
						list.Add(unlockData.Item.Id);
					}
				}
			}
			return list;
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x001FAD6C File Offset: 0x001F8F6C
		public string GetUnlockItemIconAtlas(EntityPlayerLocal player, int index)
		{
			ProgressionClass.DisplayData.UnlockData unlockData = this.GetUnlockData(index);
			if (unlockData == null)
			{
				return "ItemIconAtlas";
			}
			if (this.GetQualityLevel(player.Progression.GetProgressionValue(this.Owner.Name).Level) <= unlockData.UnlockTier)
			{
				return "ItemIconAtlasGreyscale";
			}
			return "ItemIconAtlas";
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x001FADC0 File Offset: 0x001F8FC0
		public bool GetUnlockItemLocked(EntityPlayerLocal player, int index)
		{
			ProgressionClass.DisplayData.UnlockData unlockData = this.GetUnlockData(index);
			return unlockData != null && this.GetQualityLevel(player.Progression.GetProgressionValue(this.Owner.Name).Level) <= unlockData.UnlockTier;
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x001FAE08 File Offset: 0x001F9008
		public void HandleCheckCrafting(EntityPlayerLocal _player, int _oldLevel, int _newLevel)
		{
			if (this.UnlockDataList == null)
			{
				return;
			}
			for (int i = 0; i < this.QualityStarts.Length; i++)
			{
				int num = this.QualityStarts[i];
				if (_oldLevel < num && _newLevel >= num)
				{
					if (this.HasQuality)
					{
						GameManager.ShowTooltip(_player, Localization.Get("ttCraftingSkillUnlockQuality", false, null), new string[]
						{
							Localization.Get(this.Owner.NameKey, false, null),
							this.GetName(_newLevel),
							(i + 1).ToString()
						}, ProgressionClass.DisplayData.CompletionSound, null, false, false, 0f);
					}
					else
					{
						GameManager.ShowTooltip(_player, Localization.Get("ttCraftingSkillUnlock", false, null), new string[]
						{
							Localization.Get(this.Owner.NameKey, false, null),
							this.GetName(_oldLevel)
						}, ProgressionClass.DisplayData.CompletionSound, null, false, false, 0f);
					}
				}
			}
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x001FAEF0 File Offset: 0x001F90F0
		public bool HandleCheckEnabled()
		{
			if (this.UnlockDataList == null)
			{
				this.Enabled = true;
				return true;
			}
			bool flag = false;
			for (int i = 0; i < this.UnlockDataList.Count; i++)
			{
				ProgressionClass.DisplayData.UnlockData unlockData = this.UnlockDataList[i];
				unlockData.Enabled = unlockData.Item == null || unlockData.Item.ItemTechType <= ItemClass.MaxTechType;
				if (unlockData.Enabled)
				{
					flag = true;
				}
			}
			this.Enabled = flag;
			return this.Enabled;
		}

		// Token: 0x040040A5 RID: 16549
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass item;

		// Token: 0x040040A6 RID: 16550
		public string ItemName = "";

		// Token: 0x040040A7 RID: 16551
		public string[] CustomIcon;

		// Token: 0x040040A8 RID: 16552
		public string[] CustomIconTint;

		// Token: 0x040040A9 RID: 16553
		public string[] CustomName;

		// Token: 0x040040AA RID: 16554
		public bool CustomHasQuality;

		// Token: 0x040040AB RID: 16555
		public int[] QualityStarts;

		// Token: 0x040040AC RID: 16556
		public bool Enabled = true;

		// Token: 0x040040AD RID: 16557
		public List<ProgressionClass.DisplayData.UnlockData> UnlockDataList = new List<ProgressionClass.DisplayData.UnlockData>();

		// Token: 0x040040AE RID: 16558
		public static string CompletionSound = "";

		// Token: 0x040040AF RID: 16559
		public ProgressionClass Owner;

		// Token: 0x02000AD3 RID: 2771
		public class UnlockData
		{
			// Token: 0x170008FF RID: 2303
			// (get) Token: 0x060052FA RID: 21242 RVA: 0x001FAFA1 File Offset: 0x001F91A1
			public ItemClass Item
			{
				get
				{
					if (this.item == null)
					{
						this.item = ItemClass.GetItemClass(this.ItemName, false);
					}
					return this.item;
				}
			}

			// Token: 0x040040B0 RID: 16560
			[PublicizedFrom(EAccessModifier.Private)]
			public ItemClass item;

			// Token: 0x040040B1 RID: 16561
			public string[] RecipeList;

			// Token: 0x040040B2 RID: 16562
			public string ItemName = "";

			// Token: 0x040040B3 RID: 16563
			public bool Enabled = true;

			// Token: 0x040040B4 RID: 16564
			public int UnlockTier;
		}
	}
}
