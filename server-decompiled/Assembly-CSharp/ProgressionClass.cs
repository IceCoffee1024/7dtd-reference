using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AE1 RID: 2785
public class ProgressionClass
{
	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x060052E1 RID: 21217 RVA: 0x001FD618 File Offset: 0x001FB818
	public bool IsBookGroup
	{
		get
		{
			return this.Type == ProgressionType.BookGroup;
		}
	}

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x060052E2 RID: 21218 RVA: 0x001FD623 File Offset: 0x001FB823
	public bool IsBook
	{
		get
		{
			return this.Type == ProgressionType.Book;
		}
	}

	// Token: 0x060052E3 RID: 21219 RVA: 0x001FD630 File Offset: 0x001FB830
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

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x060052E4 RID: 21220 RVA: 0x001FD680 File Offset: 0x001FB880
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

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x060052E5 RID: 21221 RVA: 0x001FD6B4 File Offset: 0x001FB8B4
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

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x060052E6 RID: 21222 RVA: 0x001FD6E2 File Offset: 0x001FB8E2
	public bool IsPerk
	{
		get
		{
			return this.Type == ProgressionType.Perk;
		}
	}

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x060052E7 RID: 21223 RVA: 0x001FD6ED File Offset: 0x001FB8ED
	public bool IsSkill
	{
		get
		{
			return this.Type == ProgressionType.Skill;
		}
	}

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x060052E8 RID: 21224 RVA: 0x001FD6F8 File Offset: 0x001FB8F8
	public bool IsAttribute
	{
		get
		{
			return this.Type == ProgressionType.Attribute;
		}
	}

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x060052E9 RID: 21225 RVA: 0x001FD703 File Offset: 0x001FB903
	public bool IsCrafting
	{
		get
		{
			return this.Type == ProgressionType.Crafting;
		}
	}

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x060052EA RID: 21226 RVA: 0x001FD710 File Offset: 0x001FB910
	// (set) Token: 0x060052EB RID: 21227 RVA: 0x001FD765 File Offset: 0x001FB965
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

	// Token: 0x060052EC RID: 21228 RVA: 0x001FD770 File Offset: 0x001FB970
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

	// Token: 0x060052ED RID: 21229 RVA: 0x001FD7D8 File Offset: 0x001FB9D8
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

	// Token: 0x060052EE RID: 21230 RVA: 0x001FD856 File Offset: 0x001FBA56
	public void ModifyValue(EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.ModifyValue(_ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x060052EF RID: 21231 RVA: 0x001FD87C File Offset: 0x001FBA7C
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, EntityAlive _ea, ProgressionValue _pv, PassiveEffects _effect, ref float _base_value, ref float _perc_value, FastTags<TagGroup.Global> _tags = default(FastTags<TagGroup.Global>))
	{
		if (this.Effects == null)
		{
			return;
		}
		this.Effects.GetModifiedValueData(_modValueSources, _sourceType, _ea, _effect, ref _base_value, ref _perc_value, _pv.GetCalculatedLevel(_ea), _tags, 1);
	}

	// Token: 0x060052F0 RID: 21232 RVA: 0x001FD8B1 File Offset: 0x001FBAB1
	public bool HasEvents()
	{
		return this.Effects != null && this.Effects.HasEvents();
	}

	// Token: 0x060052F1 RID: 21233 RVA: 0x001FD8C8 File Offset: 0x001FBAC8
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		if (this.Effects != null)
		{
			this.Effects.FireEvent(_eventType, _params);
		}
	}

	// Token: 0x060052F2 RID: 21234 RVA: 0x001FD8DF File Offset: 0x001FBADF
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canRun(RequirementGroup Requirements, MinEventParams _params)
	{
		return Requirements == null || Requirements.IsValid(_params);
	}

	// Token: 0x060052F3 RID: 21235 RVA: 0x001FD8F0 File Offset: 0x001FBAF0
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

	// Token: 0x060052F4 RID: 21236 RVA: 0x001FD92F File Offset: 0x001FBB2F
	public void AddLevelRequirement(LevelRequirement _lr)
	{
		this.LevelRequirements.Add(_lr.Level, _lr);
	}

	// Token: 0x060052F5 RID: 21237 RVA: 0x001FD943 File Offset: 0x001FBB43
	public void PostInit()
	{
		this.LevelRequirements.list.Sort((LevelRequirement a, LevelRequirement b) => a.Level - b.Level);
	}

	// Token: 0x060052F6 RID: 21238 RVA: 0x001FD974 File Offset: 0x001FBB74
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

	// Token: 0x060052F7 RID: 21239 RVA: 0x001FDA30 File Offset: 0x001FBC30
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

	// Token: 0x060052F8 RID: 21240 RVA: 0x001FDA6C File Offset: 0x001FBC6C
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

	// Token: 0x060052F9 RID: 21241 RVA: 0x001FDAEC File Offset: 0x001FBCEC
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

	// Token: 0x060052FA RID: 21242 RVA: 0x001FDB38 File Offset: 0x001FBD38
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

	// Token: 0x060052FB RID: 21243 RVA: 0x001FDBAA File Offset: 0x001FBDAA
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

	// Token: 0x040040C7 RID: 16583
	public readonly string Name;

	// Token: 0x040040C8 RID: 16584
	public readonly FastTags<TagGroup.Global> NameTag;

	// Token: 0x040040C9 RID: 16585
	public float ParentMaxLevelRatio = 1f;

	// Token: 0x040040CA RID: 16586
	public string NameKey;

	// Token: 0x040040CB RID: 16587
	public string DescKey;

	// Token: 0x040040CC RID: 16588
	public string LongDescKey;

	// Token: 0x040040CD RID: 16589
	public string Icon;

	// Token: 0x040040CE RID: 16590
	public int MinLevel;

	// Token: 0x040040CF RID: 16591
	public int MaxLevel;

	// Token: 0x040040D0 RID: 16592
	public int BaseCostToLevel;

	// Token: 0x040040D1 RID: 16593
	public float CostMultiplier;

	// Token: 0x040040D2 RID: 16594
	public bool Hidden;

	// Token: 0x040040D3 RID: 16595
	public int[] OverrideCost;

	// Token: 0x040040D4 RID: 16596
	public bool Enabled = true;

	// Token: 0x040040D5 RID: 16597
	public ProgressionClass.DisplayTypes DisplayType;

	// Token: 0x040040D6 RID: 16598
	public MinEffectController Effects;

	// Token: 0x040040D7 RID: 16599
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryList<int, LevelRequirement> LevelRequirements = new DictionaryList<int, LevelRequirement>();

	// Token: 0x040040D8 RID: 16600
	public readonly List<ProgressionClass> Children = new List<ProgressionClass>();

	// Token: 0x040040D9 RID: 16601
	public string ParentName;

	// Token: 0x040040DA RID: 16602
	public ProgressionType Type;

	// Token: 0x040040DB RID: 16603
	[PublicizedFrom(EAccessModifier.Private)]
	public float listSortOrder;

	// Token: 0x040040DC RID: 16604
	public List<ProgressionClass.DisplayData> DisplayDataList;

	// Token: 0x02000AE2 RID: 2786
	public enum DisplayTypes
	{
		// Token: 0x040040DE RID: 16606
		Standard,
		// Token: 0x040040DF RID: 16607
		Book,
		// Token: 0x040040E0 RID: 16608
		Crafting
	}

	// Token: 0x02000AE3 RID: 2787
	public class ListSortOrderComparer : IComparer<ProgressionValue>
	{
		// Token: 0x060052FC RID: 21244 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Private)]
		public ListSortOrderComparer()
		{
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x001FDBEC File Offset: 0x001FBDEC
		public int Compare(ProgressionValue _x, ProgressionValue _y)
		{
			return _x.ProgressionClass.ListSortOrder.CompareTo(_y.ProgressionClass.ListSortOrder);
		}

		// Token: 0x040040E1 RID: 16609
		public static ProgressionClass.ListSortOrderComparer Instance = new ProgressionClass.ListSortOrderComparer();
	}

	// Token: 0x02000AE4 RID: 2788
	public class DisplayData
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060052FF RID: 21247 RVA: 0x001FDC23 File Offset: 0x001FBE23
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

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06005300 RID: 21248 RVA: 0x001FDC45 File Offset: 0x001FBE45
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

		// Token: 0x06005301 RID: 21249 RVA: 0x001FDC6C File Offset: 0x001FBE6C
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

		// Token: 0x06005302 RID: 21250 RVA: 0x001FDCC4 File Offset: 0x001FBEC4
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

		// Token: 0x06005303 RID: 21251 RVA: 0x001FDD1C File Offset: 0x001FBF1C
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

		// Token: 0x06005304 RID: 21252 RVA: 0x001FDD80 File Offset: 0x001FBF80
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

		// Token: 0x06005305 RID: 21253 RVA: 0x001FDDB8 File Offset: 0x001FBFB8
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

		// Token: 0x06005306 RID: 21254 RVA: 0x001FDDF0 File Offset: 0x001FBFF0
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

		// Token: 0x06005307 RID: 21255 RVA: 0x001FDE1E File Offset: 0x001FC01E
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

		// Token: 0x06005308 RID: 21256 RVA: 0x001FDE58 File Offset: 0x001FC058
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

		// Token: 0x06005309 RID: 21257 RVA: 0x001FDE9C File Offset: 0x001FC09C
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

		// Token: 0x0600530A RID: 21258 RVA: 0x001FDEDC File Offset: 0x001FC0DC
		public string GetUnlockItemIconName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetIconName();
			}
			return "";
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x001FDF00 File Offset: 0x001FC100
		public string GetUnlockItemName(int index)
		{
			ItemClass unlockItem = this.GetUnlockItem(index);
			if (unlockItem != null)
			{
				return unlockItem.GetLocalizedItemName();
			}
			return "";
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x001FDF24 File Offset: 0x001FC124
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

		// Token: 0x0600530D RID: 21261 RVA: 0x001FDFCC File Offset: 0x001FC1CC
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

		// Token: 0x0600530E RID: 21262 RVA: 0x001FE020 File Offset: 0x001FC220
		public bool GetUnlockItemLocked(EntityPlayerLocal player, int index)
		{
			ProgressionClass.DisplayData.UnlockData unlockData = this.GetUnlockData(index);
			return unlockData != null && this.GetQualityLevel(player.Progression.GetProgressionValue(this.Owner.Name).Level) <= unlockData.UnlockTier;
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x001FE068 File Offset: 0x001FC268
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

		// Token: 0x06005310 RID: 21264 RVA: 0x001FE150 File Offset: 0x001FC350
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

		// Token: 0x040040E2 RID: 16610
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass item;

		// Token: 0x040040E3 RID: 16611
		public string ItemName = "";

		// Token: 0x040040E4 RID: 16612
		public string[] CustomIcon;

		// Token: 0x040040E5 RID: 16613
		public string[] CustomIconTint;

		// Token: 0x040040E6 RID: 16614
		public string[] CustomName;

		// Token: 0x040040E7 RID: 16615
		public bool CustomHasQuality;

		// Token: 0x040040E8 RID: 16616
		public int[] QualityStarts;

		// Token: 0x040040E9 RID: 16617
		public bool Enabled = true;

		// Token: 0x040040EA RID: 16618
		public List<ProgressionClass.DisplayData.UnlockData> UnlockDataList = new List<ProgressionClass.DisplayData.UnlockData>();

		// Token: 0x040040EB RID: 16619
		public static string CompletionSound = "";

		// Token: 0x040040EC RID: 16620
		public ProgressionClass Owner;

		// Token: 0x02000AE5 RID: 2789
		public class UnlockData
		{
			// Token: 0x17000921 RID: 2337
			// (get) Token: 0x06005313 RID: 21267 RVA: 0x001FE201 File Offset: 0x001FC401
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

			// Token: 0x040040ED RID: 16621
			[PublicizedFrom(EAccessModifier.Private)]
			public ItemClass item;

			// Token: 0x040040EE RID: 16622
			public string[] RecipeList;

			// Token: 0x040040EF RID: 16623
			public string ItemName = "";

			// Token: 0x040040F0 RID: 16624
			public bool Enabled = true;

			// Token: 0x040040F1 RID: 16625
			public int UnlockTier;
		}
	}
}
