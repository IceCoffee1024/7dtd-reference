using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AEC RID: 2796
public class ProgressionClass
{
	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x06005306 RID: 21254 RVA: 0x001FEB9B File Offset: 0x001FCD9B
	public bool IsBookGroup
	{
		get
		{
			return this.Type == ProgressionType.BookGroup;
		}
	}

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06005307 RID: 21255 RVA: 0x001FEBA6 File Offset: 0x001FCDA6
	public bool IsBook
	{
		get
		{
			return this.Type == ProgressionType.Book;
		}
	}

	// Token: 0x06005308 RID: 21256 RVA: 0x001FEBB4 File Offset: 0x001FCDB4
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

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x06005309 RID: 21257 RVA: 0x001FEC04 File Offset: 0x001FCE04
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

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x0600530A RID: 21258 RVA: 0x001FEC38 File Offset: 0x001FCE38
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

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x0600530B RID: 21259 RVA: 0x001FEC66 File Offset: 0x001FCE66
	public bool IsPerk
	{
		get
		{
			return this.Type == ProgressionType.Perk;
		}
	}

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x0600530C RID: 21260 RVA: 0x001FEC71 File Offset: 0x001FCE71
	public bool IsSkill
	{
		get
		{
			return this.Type == ProgressionType.Skill;
		}
	}

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x0600530D RID: 21261 RVA: 0x001FEC7C File Offset: 0x001FCE7C
	public bool IsAttribute
	{
		get
		{
			return this.Type == ProgressionType.Attribute;
		}
	}

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x0600530E RID: 21262 RVA: 0x001FEC87 File Offset: 0x001FCE87
	public bool IsCrafting
	{
		get
		{
			return this.Type == ProgressionType.Crafting;
		}
	}

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x0600530F RID: 21263 RVA: 0x001FEC94 File Offset: 0x001FCE94
	// (set) Token: 0x06005310 RID: 21264 RVA: 0x001FECE9 File Offset: 0x001FCEE9
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

	// Token: 0x06005311 RID: 21265 RVA: 0x001FECF4 File Offset: 0x001FCEF4
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

	// Token: 0x06005312 RID: 21266 RVA: 0x001FED5C File Offset: 0x001FCF5C
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

	// Token: 0x06005313 RID: 21267 RVA: 0x001FEDDA File Offset: 0x001FCFDA
	public void ModifyValue(EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x06005314 RID: 21268 RVA: 0x001FEE00 File Offset: 0x001FD000
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.GetModifiedValueData(_modValueSources, _sourceType, _ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x06005315 RID: 21269 RVA: 0x001FEE35 File Offset: 0x001FD035
	public bool HasEvents()
	{
		return this.Effects != null && this.Effects.HasEvents();
	}

	// Token: 0x06005316 RID: 21270 RVA: 0x001FEE4C File Offset: 0x001FD04C
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.Effects != null)
		{
			this.Effects.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x06005317 RID: 21271 RVA: 0x001FEE63 File Offset: 0x001FD063
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canRun(RequirementGroup Requirements, MinEventParams _params)
	{
		return Requirements == null || Requirements.IsValid(_params);
	}

	// Token: 0x06005318 RID: 21272 RVA: 0x001FEE74 File Offset: 0x001FD074
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

	// Token: 0x06005319 RID: 21273 RVA: 0x001FEEB3 File Offset: 0x001FD0B3
	public void AddLevelRequirement(LevelRequirement _lr)
	{
		this.LevelRequirements.Add(_lr.Level, _lr);
	}

	// Token: 0x0600531A RID: 21274 RVA: 0x001FEEC7 File Offset: 0x001FD0C7
	public void PostInit()
	{
		this.LevelRequirements.list.Sort((LevelRequirement a, LevelRequirement b) => a.Level - b.Level);
	}

	// Token: 0x0600531B RID: 21275 RVA: 0x001FEEF8 File Offset: 0x001FD0F8
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

	// Token: 0x0600531C RID: 21276 RVA: 0x001FEFB4 File Offset: 0x001FD1B4
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

	// Token: 0x0600531D RID: 21277 RVA: 0x001FEFF0 File Offset: 0x001FD1F0
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

	// Token: 0x0600531E RID: 21278 RVA: 0x001FF070 File Offset: 0x001FD270
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

	// Token: 0x0600531F RID: 21279 RVA: 0x001FF0BC File Offset: 0x001FD2BC
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

	// Token: 0x06005320 RID: 21280 RVA: 0x001FF12E File Offset: 0x001FD32E
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

	// Token: 0x040040EC RID: 16620
	public readonly string Name;

	// Token: 0x040040ED RID: 16621
	public readonly FastTags<TagGroup.Global> NameTag;

	// Token: 0x040040EE RID: 16622
	public float ParentMaxLevelRatio = 1f;

	// Token: 0x040040EF RID: 16623
	public string NameKey;

	// Token: 0x040040F0 RID: 16624
	public string DescKey;

	// Token: 0x040040F1 RID: 16625
	public string LongDescKey;

	// Token: 0x040040F2 RID: 16626
	public string Icon;

	// Token: 0x040040F3 RID: 16627
	public int MinLevel;

	// Token: 0x040040F4 RID: 16628
	public int MaxLevel;

	// Token: 0x040040F5 RID: 16629
	public int BaseCostToLevel;

	// Token: 0x040040F6 RID: 16630
	public float CostMultiplier;

	// Token: 0x040040F7 RID: 16631
	public bool Hidden;

	// Token: 0x040040F8 RID: 16632
	public int[] OverrideCost;

	// Token: 0x040040F9 RID: 16633
	public bool Enabled = true;

	// Token: 0x040040FA RID: 16634
	public ProgressionClass.DisplayTypes DisplayType;

	// Token: 0x040040FB RID: 16635
	public MinEffectController Effects;

	// Token: 0x040040FC RID: 16636
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryList<int, LevelRequirement> LevelRequirements = new DictionaryList<int, LevelRequirement>();

	// Token: 0x040040FD RID: 16637
	public readonly List<ProgressionClass> Children = new List<ProgressionClass>();

	// Token: 0x040040FE RID: 16638
	public string ParentName;

	// Token: 0x040040FF RID: 16639
	public ProgressionType Type;

	// Token: 0x04004100 RID: 16640
	[PublicizedFrom(EAccessModifier.Private)]
	public float listSortOrder;

	// Token: 0x04004101 RID: 16641
	public List<ProgressionClass.DisplayData> DisplayDataList;

	// Token: 0x02000AED RID: 2797
	public enum DisplayTypes
	{
		// Token: 0x04004103 RID: 16643
		Standard,
		// Token: 0x04004104 RID: 16644
		Book,
		// Token: 0x04004105 RID: 16645
		Crafting
	}

	// Token: 0x02000AEE RID: 2798
	public class ListSortOrderComparer : IComparer<ProgressionValue>
	{
		// Token: 0x06005321 RID: 21281 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public ListSortOrderComparer()
		{
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x001FF170 File Offset: 0x001FD370
		public int Compare(ProgressionValue _x, ProgressionValue _y)
		{
			return _x.ProgressionClass.ListSortOrder.CompareTo(_y.ProgressionClass.ListSortOrder);
		}

		// Token: 0x04004106 RID: 16646
		public static ProgressionClass.ListSortOrderComparer Instance = new ProgressionClass.ListSortOrderComparer();
	}

	// Token: 0x02000AEF RID: 2799
	public class DisplayData
	{
		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06005324 RID: 21284 RVA: 0x001FF1A7 File Offset: 0x001FD3A7
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

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06005325 RID: 21285 RVA: 0x001FF1C9 File Offset: 0x001FD3C9
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

		// Token: 0x06005326 RID: 21286 RVA: 0x001FF1F0 File Offset: 0x001FD3F0
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

		// Token: 0x06005327 RID: 21287 RVA: 0x001FF248 File Offset: 0x001FD448
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

		// Token: 0x06005328 RID: 21288 RVA: 0x001FF2A0 File Offset: 0x001FD4A0
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

		// Token: 0x06005329 RID: 21289 RVA: 0x001FF304 File Offset: 0x001FD504
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

		// Token: 0x0600532A RID: 21290 RVA: 0x001FF33C File Offset: 0x001FD53C
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

		// Token: 0x0600532B RID: 21291 RVA: 0x001FF374 File Offset: 0x001FD574
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

		// Token: 0x0600532C RID: 21292 RVA: 0x001FF3A2 File Offset: 0x001FD5A2
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

		// Token: 0x0600532D RID: 21293 RVA: 0x001FF3DC File Offset: 0x001FD5DC
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

		// Token: 0x0600532E RID: 21294 RVA: 0x001FF420 File Offset: 0x001FD620
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

		// Token: 0x0600532F RID: 21295 RVA: 0x001FF460 File Offset: 0x001FD660
		public string GetUnlockItemIconName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetIconName();
			}
			return "";
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x001FF484 File Offset: 0x001FD684
		public string GetUnlockItemName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetLocalizedItemName();
			}
			return "";
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x001FF4A8 File Offset: 0x001FD6A8
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

		// Token: 0x06005332 RID: 21298 RVA: 0x001FF550 File Offset: 0x001FD750
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

		// Token: 0x06005333 RID: 21299 RVA: 0x001FF5A4 File Offset: 0x001FD7A4
		public bool GetUnlockItemLocked(EntityPlayerLocal player, int index)
		{
			ProgressionClass.DisplayData.UnlockData unlockData = this.GetUnlockData(index);
			return unlockData != null && this.GetQualityLevel(player.Progression.GetProgressionValue(this.Owner.Name).Level) <= unlockData.UnlockTier;
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x001FF5EC File Offset: 0x001FD7EC
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

		// Token: 0x06005335 RID: 21301 RVA: 0x001FF6D4 File Offset: 0x001FD8D4
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

		// Token: 0x04004107 RID: 16647
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass item;

		// Token: 0x04004108 RID: 16648
		public string ItemName = "";

		// Token: 0x04004109 RID: 16649
		public string[] CustomIcon;

		// Token: 0x0400410A RID: 16650
		public string[] CustomIconTint;

		// Token: 0x0400410B RID: 16651
		public string[] CustomName;

		// Token: 0x0400410C RID: 16652
		public bool CustomHasQuality;

		// Token: 0x0400410D RID: 16653
		public int[] QualityStarts;

		// Token: 0x0400410E RID: 16654
		public bool Enabled = true;

		// Token: 0x0400410F RID: 16655
		public List<ProgressionClass.DisplayData.UnlockData> UnlockDataList = new List<ProgressionClass.DisplayData.UnlockData>();

		// Token: 0x04004110 RID: 16656
		public static string CompletionSound = "";

		// Token: 0x04004111 RID: 16657
		public ProgressionClass Owner;

		// Token: 0x02000AF0 RID: 2800
		public class UnlockData
		{
			// Token: 0x17000927 RID: 2343
			// (get) Token: 0x06005338 RID: 21304 RVA: 0x001FF785 File Offset: 0x001FD985
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

			// Token: 0x04004112 RID: 16658
			[PublicizedFrom(EAccessModifier.Private)]
			public ItemClass item;

			// Token: 0x04004113 RID: 16659
			public string[] RecipeList;

			// Token: 0x04004114 RID: 16660
			public string ItemName = "";

			// Token: 0x04004115 RID: 16661
			public bool Enabled = true;

			// Token: 0x04004116 RID: 16662
			public int UnlockTier;
		}
	}
}
