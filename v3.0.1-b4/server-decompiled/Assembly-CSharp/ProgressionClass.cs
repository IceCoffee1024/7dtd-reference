using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ABE RID: 2750
public class ProgressionClass
{
	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x06005242 RID: 21058 RVA: 0x001F7920 File Offset: 0x001F5B20
	public bool IsBookGroup
	{
		get
		{
			return this.Type == ProgressionType.BookGroup;
		}
	}

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x06005243 RID: 21059 RVA: 0x001F792B File Offset: 0x001F5B2B
	public bool IsBook
	{
		get
		{
			return this.Type == ProgressionType.Book;
		}
	}

	// Token: 0x06005244 RID: 21060 RVA: 0x001F7938 File Offset: 0x001F5B38
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

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x06005245 RID: 21061 RVA: 0x001F7988 File Offset: 0x001F5B88
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

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06005246 RID: 21062 RVA: 0x001F79BC File Offset: 0x001F5BBC
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

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x06005247 RID: 21063 RVA: 0x001F79EA File Offset: 0x001F5BEA
	public bool IsPerk
	{
		get
		{
			return this.Type == ProgressionType.Perk;
		}
	}

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x06005248 RID: 21064 RVA: 0x001F79F5 File Offset: 0x001F5BF5
	public bool IsSkill
	{
		get
		{
			return this.Type == ProgressionType.Skill;
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06005249 RID: 21065 RVA: 0x001F7A00 File Offset: 0x001F5C00
	public bool IsAttribute
	{
		get
		{
			return this.Type == ProgressionType.Attribute;
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x0600524A RID: 21066 RVA: 0x001F7A0B File Offset: 0x001F5C0B
	public bool IsCrafting
	{
		get
		{
			return this.Type == ProgressionType.Crafting;
		}
	}

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x0600524B RID: 21067 RVA: 0x001F7A18 File Offset: 0x001F5C18
	// (set) Token: 0x0600524C RID: 21068 RVA: 0x001F7A6D File Offset: 0x001F5C6D
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

	// Token: 0x0600524D RID: 21069 RVA: 0x001F7A78 File Offset: 0x001F5C78
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

	// Token: 0x0600524E RID: 21070 RVA: 0x001F7AE0 File Offset: 0x001F5CE0
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

	// Token: 0x0600524F RID: 21071 RVA: 0x001F7B5E File Offset: 0x001F5D5E
	public void ModifyValue(EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x06005250 RID: 21072 RVA: 0x001F7B84 File Offset: 0x001F5D84
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.GetModifiedValueData(_modValueSources, _sourceType, _ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x06005251 RID: 21073 RVA: 0x001F7BB9 File Offset: 0x001F5DB9
	public bool HasEvents()
	{
		return this.Effects != null && this.Effects.HasEvents();
	}

	// Token: 0x06005252 RID: 21074 RVA: 0x001F7BD0 File Offset: 0x001F5DD0
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.Effects != null)
		{
			this.Effects.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x06005253 RID: 21075 RVA: 0x001F7BE7 File Offset: 0x001F5DE7
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canRun(RequirementGroup Requirements, MinEventParams _params)
	{
		return Requirements == null || Requirements.IsValid(_params);
	}

	// Token: 0x06005254 RID: 21076 RVA: 0x001F7BF8 File Offset: 0x001F5DF8
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

	// Token: 0x06005255 RID: 21077 RVA: 0x001F7C37 File Offset: 0x001F5E37
	public void AddLevelRequirement(LevelRequirement _lr)
	{
		this.LevelRequirements.Add(_lr.Level, _lr);
	}

	// Token: 0x06005256 RID: 21078 RVA: 0x001F7C4B File Offset: 0x001F5E4B
	public void PostInit()
	{
		this.LevelRequirements.list.Sort((LevelRequirement a, LevelRequirement b) => a.Level - b.Level);
	}

	// Token: 0x06005257 RID: 21079 RVA: 0x001F7C7C File Offset: 0x001F5E7C
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

	// Token: 0x06005258 RID: 21080 RVA: 0x001F7D38 File Offset: 0x001F5F38
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

	// Token: 0x06005259 RID: 21081 RVA: 0x001F7D74 File Offset: 0x001F5F74
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

	// Token: 0x0600525A RID: 21082 RVA: 0x001F7DF4 File Offset: 0x001F5FF4
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

	// Token: 0x0600525B RID: 21083 RVA: 0x001F7E40 File Offset: 0x001F6040
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

	// Token: 0x0600525C RID: 21084 RVA: 0x001F7EB2 File Offset: 0x001F60B2
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

	// Token: 0x04003FF9 RID: 16377
	public readonly string Name;

	// Token: 0x04003FFA RID: 16378
	public readonly FastTags<TagGroup.Global> NameTag;

	// Token: 0x04003FFB RID: 16379
	public float ParentMaxLevelRatio = 1f;

	// Token: 0x04003FFC RID: 16380
	public string NameKey;

	// Token: 0x04003FFD RID: 16381
	public string DescKey;

	// Token: 0x04003FFE RID: 16382
	public string LongDescKey;

	// Token: 0x04003FFF RID: 16383
	public string Icon;

	// Token: 0x04004000 RID: 16384
	public int MinLevel;

	// Token: 0x04004001 RID: 16385
	public int MaxLevel;

	// Token: 0x04004002 RID: 16386
	public int BaseCostToLevel;

	// Token: 0x04004003 RID: 16387
	public float CostMultiplier;

	// Token: 0x04004004 RID: 16388
	public bool Hidden;

	// Token: 0x04004005 RID: 16389
	public int[] OverrideCost;

	// Token: 0x04004006 RID: 16390
	public bool Enabled = true;

	// Token: 0x04004007 RID: 16391
	public ProgressionClass.DisplayTypes DisplayType;

	// Token: 0x04004008 RID: 16392
	public MinEffectController Effects;

	// Token: 0x04004009 RID: 16393
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryList<int, LevelRequirement> LevelRequirements = new DictionaryList<int, LevelRequirement>();

	// Token: 0x0400400A RID: 16394
	public readonly List<ProgressionClass> Children = new List<ProgressionClass>();

	// Token: 0x0400400B RID: 16395
	public string ParentName;

	// Token: 0x0400400C RID: 16396
	public ProgressionType Type;

	// Token: 0x0400400D RID: 16397
	[PublicizedFrom(EAccessModifier.Private)]
	public float listSortOrder;

	// Token: 0x0400400E RID: 16398
	public List<ProgressionClass.DisplayData> DisplayDataList;

	// Token: 0x02000ABF RID: 2751
	public enum DisplayTypes
	{
		// Token: 0x04004010 RID: 16400
		Standard,
		// Token: 0x04004011 RID: 16401
		Book,
		// Token: 0x04004012 RID: 16402
		Crafting
	}

	// Token: 0x02000AC0 RID: 2752
	public class ListSortOrderComparer : IComparer<ProgressionValue>
	{
		// Token: 0x0600525D RID: 21085 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public ListSortOrderComparer()
		{
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x001F7EF4 File Offset: 0x001F60F4
		public int Compare(ProgressionValue _x, ProgressionValue _y)
		{
			return _x.ProgressionClass.ListSortOrder.CompareTo(_y.ProgressionClass.ListSortOrder);
		}

		// Token: 0x04004013 RID: 16403
		public static ProgressionClass.ListSortOrderComparer Instance = new ProgressionClass.ListSortOrderComparer();
	}

	// Token: 0x02000AC1 RID: 2753
	public class DisplayData
	{
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x001F7F2B File Offset: 0x001F612B
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

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x001F7F4D File Offset: 0x001F614D
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

		// Token: 0x06005262 RID: 21090 RVA: 0x001F7F74 File Offset: 0x001F6174
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

		// Token: 0x06005263 RID: 21091 RVA: 0x001F7FCC File Offset: 0x001F61CC
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

		// Token: 0x06005264 RID: 21092 RVA: 0x001F8024 File Offset: 0x001F6224
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

		// Token: 0x06005265 RID: 21093 RVA: 0x001F8088 File Offset: 0x001F6288
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

		// Token: 0x06005266 RID: 21094 RVA: 0x001F80C0 File Offset: 0x001F62C0
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

		// Token: 0x06005267 RID: 21095 RVA: 0x001F80F8 File Offset: 0x001F62F8
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

		// Token: 0x06005268 RID: 21096 RVA: 0x001F8126 File Offset: 0x001F6326
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

		// Token: 0x06005269 RID: 21097 RVA: 0x001F8160 File Offset: 0x001F6360
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

		// Token: 0x0600526A RID: 21098 RVA: 0x001F81A4 File Offset: 0x001F63A4
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

		// Token: 0x0600526B RID: 21099 RVA: 0x001F81E4 File Offset: 0x001F63E4
		public string GetUnlockItemIconName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetIconName();
			}
			return "";
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x001F8208 File Offset: 0x001F6408
		public string GetUnlockItemName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetLocalizedItemName();
			}
			return "";
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x001F822C File Offset: 0x001F642C
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

		// Token: 0x0600526E RID: 21102 RVA: 0x001F82D4 File Offset: 0x001F64D4
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

		// Token: 0x0600526F RID: 21103 RVA: 0x001F8328 File Offset: 0x001F6528
		public bool GetUnlockItemLocked(EntityPlayerLocal player, int index)
		{
			ProgressionClass.DisplayData.UnlockData unlockData = this.GetUnlockData(index);
			return unlockData != null && this.GetQualityLevel(player.Progression.GetProgressionValue(this.Owner.Name).Level) <= unlockData.UnlockTier;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x001F8370 File Offset: 0x001F6570
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

		// Token: 0x06005271 RID: 21105 RVA: 0x001F8458 File Offset: 0x001F6658
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

		// Token: 0x04004014 RID: 16404
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass item;

		// Token: 0x04004015 RID: 16405
		public string ItemName = "";

		// Token: 0x04004016 RID: 16406
		public string[] CustomIcon;

		// Token: 0x04004017 RID: 16407
		public string[] CustomIconTint;

		// Token: 0x04004018 RID: 16408
		public string[] CustomName;

		// Token: 0x04004019 RID: 16409
		public bool CustomHasQuality;

		// Token: 0x0400401A RID: 16410
		public int[] QualityStarts;

		// Token: 0x0400401B RID: 16411
		public bool Enabled = true;

		// Token: 0x0400401C RID: 16412
		public List<ProgressionClass.DisplayData.UnlockData> UnlockDataList = new List<ProgressionClass.DisplayData.UnlockData>();

		// Token: 0x0400401D RID: 16413
		public static string CompletionSound = "";

		// Token: 0x0400401E RID: 16414
		public ProgressionClass Owner;

		// Token: 0x02000AC2 RID: 2754
		public class UnlockData
		{
			// Token: 0x170008ED RID: 2285
			// (get) Token: 0x06005274 RID: 21108 RVA: 0x001F8509 File Offset: 0x001F6709
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

			// Token: 0x0400401F RID: 16415
			[PublicizedFrom(EAccessModifier.Private)]
			public ItemClass item;

			// Token: 0x04004020 RID: 16416
			public string[] RecipeList;

			// Token: 0x04004021 RID: 16417
			public string ItemName = "";

			// Token: 0x04004022 RID: 16418
			public bool Enabled = true;

			// Token: 0x04004023 RID: 16419
			public int UnlockTier;
		}
	}
}
