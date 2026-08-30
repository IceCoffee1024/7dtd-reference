using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Linq;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using XMLData.Item;

// Token: 0x020005E9 RID: 1513
[Preserve]
public class ItemClass : ItemData
{
	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06003110 RID: 12560 RVA: 0x0013EDAC File Offset: 0x0013CFAC
	public string[] Attachments
	{
		get
		{
			return this.attachments;
		}
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06003111 RID: 12561 RVA: 0x0013EDB4 File Offset: 0x0013CFB4
	public bool HasQuality
	{
		get
		{
			return this.Effects != null && this.Effects.IsOwnerTiered();
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06003112 RID: 12562 RVA: 0x0013EDCB File Offset: 0x0013CFCB
	public virtual bool IsLimitedItem
	{
		get
		{
			return this.IsLimited;
		}
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x0013EDD4 File Offset: 0x0013CFD4
	public bool CanMoveToLocation(XUiC_ItemStack.StackLocationTypes locationType)
	{
		bool flag = true;
		if (this.bRestrictedMove)
		{
			flag = false;
			using (List<XUiC_ItemStack.StackLocationTypes>.Enumerator enumerator = this.restrictedTo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == locationType)
					{
						flag = true;
						break;
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06003114 RID: 12564 RVA: 0x0013EE34 File Offset: 0x0013D034
	public RecipeUnlockData[] UnlockedBy
	{
		get
		{
			if (this.unlockedBy == null)
			{
				if (this.Properties.Values.ContainsKey(ItemClass.PropUnlockedBy))
				{
					string[] array = this.Properties.Values[ItemClass.PropUnlockedBy].Split(',', StringSplitOptions.None);
					if (array.Length != 0)
					{
						this.unlockedBy = new RecipeUnlockData[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							this.unlockedBy[i] = new RecipeUnlockData(array[i]);
						}
					}
				}
				else
				{
					this.unlockedBy = new RecipeUnlockData[0];
				}
			}
			return this.unlockedBy;
		}
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x0013EEC4 File Offset: 0x0013D0C4
	public ItemClass()
	{
		this.bCanHold = true;
		this.bCanDrop = true;
		this.bCraftingTool = false;
		this.Properties = new DynamicProperties();
		this.attachments = null;
		this.Actions = new ItemAction[5];
		for (int i = 0; i < this.Actions.Length; i++)
		{
			this.Actions[i] = null;
		}
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x0013F101 File Offset: 0x0013D301
	public void SetId(int _id)
	{
		this.pId = _id;
		if (this.Effects != null)
		{
			this.Effects.ParentPointer = _id;
		}
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x0013F124 File Offset: 0x0013D324
	public virtual void Init()
	{
		string itemName = this.GetItemName();
		ItemClass.nameToItem[itemName] = this;
		ItemClass.nameToItemCaseInsensitive[itemName] = this;
		ItemClass.itemNames.Add(itemName);
		if (this.Properties.Values.ContainsKey(ItemClass.PropTags))
		{
			this.ItemTags = FastTags<TagGroup.Global>.Parse(this.Properties.Values[ItemClass.PropTags]);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropDistractionTags))
		{
			this.DistractionTags = FastTags<TagGroup.Global>.Parse(this.Properties.Values[ItemClass.PropDistractionTags]);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropFuelValue))
		{
			int num = 0;
			int.TryParse(this.Properties.Values[ItemClass.PropFuelValue], out num);
			base.FuelValue = new DataItem<int>(num);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropWeight))
		{
			int num2;
			int.TryParse(this.Properties.Values[ItemClass.PropWeight], out num2);
			base.Weight = new DataItem<int>(num2);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropImageEffectOnActive))
		{
			string text = this.Properties.Values[ItemClass.PropImageEffectOnActive].ToString();
			base.ImageEffectOnActive = new DataItem<string>(text);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropActive))
		{
			bool flag = false;
			StringParsers.TryParseBool(this.Properties.Values[ItemClass.PropActive], out flag);
			base.Active = new DataItem<bool>(flag);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropAlwaysActive))
		{
			bool flag2 = false;
			StringParsers.TryParseBool(this.Properties.Values[ItemClass.PropAlwaysActive], out flag2);
			base.AlwaysActive = new DataItem<bool>(flag2);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropPlaySoundOnActive))
		{
			string text2 = this.Properties.Values[ItemClass.PropPlaySoundOnActive].ToString();
			base.PlaySoundOnActive = new DataItem<string>(text2);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropLightSource))
		{
			this.LightSource = new DataItem<string>(this.Properties.Values[ItemClass.PropLightSource]);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropLightValue))
		{
			this.lightValue = StringParsers.ParseFloat(this.Properties.Values[ItemClass.PropLightValue], 0, -1, NumberStyles.Any);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropSoundSightIn))
		{
			this.soundSightIn = this.Properties.Values[ItemClass.PropSoundSightIn].ToString();
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropSoundSightOut))
		{
			this.soundSightOut = this.Properties.Values[ItemClass.PropSoundSightOut].ToString();
		}
		this.Properties.ParseBool(ItemClass.PropIgnoreKeystoneSound, ref this.ignoreKeystoneSound);
		if (this.Properties.Values.ContainsKey(ItemClass.PropActivateObject))
		{
			this.ActivateObject = new DataItem<string>(this.Properties.Values[ItemClass.PropActivateObject]);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropThrowableDecoy))
		{
			bool flag3;
			StringParsers.TryParseBool(this.Properties.Values[ItemClass.PropThrowableDecoy], out flag3);
			this.ThrowableDecoy = new DataItem<bool>(flag3);
		}
		else
		{
			this.ThrowableDecoy = new DataItem<bool>(false);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCustomIcon))
		{
			this.CustomIcon = new DataItem<string>(this.Properties.Values[ItemClass.PropCustomIcon]);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCustomIconTint))
		{
			this.CustomIconTint = StringParsers.ParseHexColor(this.Properties.Values[ItemClass.PropCustomIconTint]);
		}
		else
		{
			this.CustomIconTint = Color.white;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropGroupName))
		{
			string[] array = this.Properties.Values[ItemClass.PropGroupName].Split(',', StringSplitOptions.None);
			if (array.Length != 0)
			{
				this.Groups = new string[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.Groups[i] = array[i].Trim();
					if (this.Groups[i] == "Limited")
					{
						this.IsLimited = true;
					}
				}
			}
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCritChance))
		{
			this.CritChance = new DataItem<float>(StringParsers.ParseFloat(this.Properties.Values[ItemClass.PropCritChance], 0, -1, NumberStyles.Any));
		}
		else
		{
			this.CritChance = new DataItem<float>(0f);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropVehicleSlotType))
		{
			this.VehicleSlotType = this.Properties.Values[ItemClass.PropVehicleSlotType];
		}
		else
		{
			this.VehicleSlotType = string.Empty;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropHoldingItemHidden))
		{
			this.HoldingItemHidden = StringParsers.ParseBool(this.Properties.Values[ItemClass.PropHoldingItemHidden], 0, -1, true);
		}
		else
		{
			this.HoldingItemHidden = false;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCraftExpValue))
		{
			StringParsers.TryParseFloat(this.Properties.Values[ItemClass.PropCraftExpValue], out this.CraftComponentExp);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropRepairExpMultiplier))
		{
			StringParsers.TryParseFloat(this.Properties.Values[ItemClass.PropRepairExpMultiplier], out this.RepairExpMultiplier);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCraftTimeValue))
		{
			StringParsers.TryParseFloat(this.Properties.Values[ItemClass.PropCraftTimeValue], out this.CraftComponentTime);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropLootExpValue))
		{
			StringParsers.TryParseFloat(this.Properties.Values[ItemClass.PropLootExpValue], out this.LootExp);
		}
		this.Properties.ParseFloat(ItemClass.PropEconomicValue, ref this.EconomicValue);
		this.Properties.ParseFloat(ItemClass.PropEconomicSellScale, ref this.EconomicSellScale);
		this.Properties.ParseInt(ItemClass.PropEconomicBundleSize, ref this.EconomicBundleSize);
		this.Properties.ParseBool(ItemClass.PropSellableToTrader, ref this.SellableToTrader);
		if (this.Properties.Values.ContainsKey(ItemClass.PropCreativeMode))
		{
			this.CreativeMode = EnumUtils.Parse<EnumCreativeMode>(this.Properties.Values[ItemClass.PropCreativeMode], false);
		}
		this.SortOrder = this.Properties.GetString(ItemClass.PropCreativeSort1);
		this.SortOrder += this.Properties.GetString(ItemClass.PropCreativeSort2);
		if (this.Properties.Values.ContainsKey(ItemClass.PropCraftingSkillExp) && !int.TryParse(this.Properties.Values[ItemClass.PropCraftingSkillExp], out this.CraftingSkillExp))
		{
			this.CraftingSkillExp = 10;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropActionSkillExp) && !int.TryParse(this.Properties.Values[ItemClass.PropActionSkillExp], out this.ActionSkillExp))
		{
			this.ActionSkillExp = 10;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropInsulation) && !StringParsers.TryParseFloat(this.Properties.Values[ItemClass.PropInsulation], out this.Insulation))
		{
			this.Insulation = 0f;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropWaterproof) && !StringParsers.TryParseFloat(this.Properties.Values[ItemClass.PropWaterproof], out this.WaterProof))
		{
			this.WaterProof = 0f;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropEncumbrance) && !StringParsers.TryParseFloat(this.Properties.Values[ItemClass.PropEncumbrance], out this.Encumbrance))
		{
			this.Encumbrance = 0f;
		}
		this.Properties.ParseString(ItemClass.PropSoundPickup, ref this.SoundPickup);
		this.Properties.ParseString(ItemClass.PropSoundPlace, ref this.SoundPlace);
		this.Properties.ParseString(ItemClass.PropSoundHolster, ref this.SoundHolster);
		this.Properties.ParseString(ItemClass.PropSoundUnholster, ref this.SoundUnholster);
		this.Properties.ParseString(ItemClass.PropSoundStick, ref this.SoundStick);
		this.Properties.ParseString(ItemClass.PropSoundTick, ref this.SoundTick);
		this.Properties.ParseString(ItemClass.PropSoundImpactHit, ref this.SoundImpactHit);
		this.Properties.ParseString(ItemClass.PropSoundImpactGraze, ref this.SoundImpactGraze);
		this.Properties.ParseBool(ItemClass.PropHasReloadAnim, ref this.HasReloadAnim);
		this.Properties.ParseFloat(ItemClass.PropSmell, ref this.Smell);
		if (this.SoundTick != null)
		{
			string[] array2 = this.SoundTick.Split(',', StringSplitOptions.None);
			this.SoundTick = array2[0];
			if (array2.Length >= 2)
			{
				this.SoundTickDelay = StringParsers.ParseFloat(array2[1], 0, -1, NumberStyles.Any);
			}
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropDescriptionKey))
		{
			this.DescriptionKey = this.Properties.Values[ItemClass.PropDescriptionKey];
		}
		else
		{
			this.DescriptionKey = base.Name + "Desc";
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropResourceUnit))
		{
			this.IsResourceUnit = StringParsers.ParseBool(this.Properties.Values[ItemClass.PropResourceUnit], 0, -1, true);
		}
		this.Properties.ParseString(ItemClass.PropReplaceResourceUnit, ref this.ReplaceResourceUnit);
		if (this.Properties.Values.ContainsKey(ItemClass.PropMeltTimePerUnit))
		{
			this.MeltTimePerUnit = StringParsers.ParseFloat(this.Properties.Values[ItemClass.PropMeltTimePerUnit], 0, -1, NumberStyles.Any);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropActionSkillGroup))
		{
			this.ActionSkillGroup = this.Properties.Values[ItemClass.PropActionSkillGroup];
		}
		else
		{
			this.ActionSkillGroup = "";
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCraftingSkillGroup))
		{
			this.CraftingSkillGroup = this.Properties.Values[ItemClass.PropCraftingSkillGroup];
		}
		else
		{
			this.CraftingSkillGroup = "";
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCrosshairOnAim))
		{
			this.bShowCrosshairOnAiming = StringParsers.ParseBool(this.Properties.Values[ItemClass.PropCrosshairOnAim], 0, -1, true);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropCrosshairUpAfterShot))
		{
			this.bCrosshairUpAfterShot = StringParsers.ParseBool(this.Properties.Values[ItemClass.PropCrosshairUpAfterShot], 0, -1, true);
		}
		else
		{
			this.bCrosshairUpAfterShot = true;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropUsableUnderwater))
		{
			this.UsableUnderwater = StringParsers.ParseBool(this.Properties.Values[ItemClass.PropUsableUnderwater], 0, -1, true);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropItemTypeIcon))
		{
			this.ItemTypeIcon = this.Properties.Values[ItemClass.PropItemTypeIcon];
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropAltItemTypeIcon))
		{
			this.AltItemTypeIcon = this.Properties.Values[ItemClass.PropAltItemTypeIcon];
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropAltItemTypeIconColor))
		{
			this.AltItemTypeIconColor = StringParsers.ParseHexColor(this.Properties.Values[ItemClass.PropAltItemTypeIconColor]);
		}
		else
		{
			this.AltItemTypeIconColor = Color.white;
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropUnlocks))
		{
			this.Unlocks = this.Properties.Values[ItemClass.PropUnlocks];
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropNavObject))
		{
			this.NavObject = this.Properties.Values[ItemClass.PropNavObject];
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropQuestItem))
		{
			this.IsQuestItem = StringParsers.ParseBool(this.Properties.Values[ItemClass.PropQuestItem], 0, -1, true);
		}
		this.Properties.ParseBool(ItemClass.PropShowQuality, ref this.ShowQualityBar);
		this.Properties.ParseByte(ItemClass.PropQualityMin, ref this.QualityMin);
		if (this.Properties.Values.ContainsKey(ItemClass.PropNoScrapping))
		{
			this.NoScrapping = StringParsers.ParseBool(this.Properties.Values[ItemClass.PropNoScrapping], 0, -1, true);
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropScrapTimeOverride))
		{
			this.ScrapTimeOverride = StringParsers.ParseFloat(this.Properties.Values[ItemClass.PropScrapTimeOverride], 0, -1, NumberStyles.Any);
		}
		if (this.Properties.Classes.ContainsKey("SDCS"))
		{
			this.SDCSData = new SDCSUtils.SlotData();
			if (this.Properties.Contains("SDCS", "Prefab"))
			{
				this.SDCSData.PrefabName = this.Properties.GetString("SDCS", "Prefab");
			}
			if (this.Properties.Contains("SDCS", "TransformName"))
			{
				this.SDCSData.PartName = this.Properties.GetString("SDCS", "TransformName");
			}
			if (this.Properties.Contains("SDCS", "Excludes"))
			{
				this.SDCSData.BaseToTurnOff = this.Properties.GetString("SDCS", "Excludes");
			}
			if (this.Properties.Contains("SDCS", "HairMaskType"))
			{
				this.SDCSData.HairMaskType = (SDCSUtils.SlotData.HairMaskTypes)Enum.Parse(typeof(SDCSUtils.SlotData.HairMaskTypes), this.Properties.GetString("SDCS", "HairMaskType"));
			}
			if (this.Properties.Contains("SDCS", "FacialHairMaskType"))
			{
				this.SDCSData.FacialHairMaskType = (SDCSUtils.SlotData.HairMaskTypes)Enum.Parse(typeof(SDCSUtils.SlotData.HairMaskTypes), this.Properties.GetString("SDCS", "FacialHairMaskType"));
			}
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropDisplayType))
		{
			this.DisplayType = this.Properties.Values[ItemClass.PropDisplayType];
		}
		else
		{
			this.DisplayType = "";
		}
		this.Properties.ParseString(ItemClass.PropTraderStageTemplate, ref this.TraderStageTemplate);
		this.Properties.ParseString(ItemClass.PropTrackerIndexName, ref this.TrackerIndexName);
		this.Properties.ParseString(ItemClass.PropTrackerNavObject, ref this.TrackerNavObject);
		this.Properties.ParseInt(ItemClass.PropMaxModsAllowed, ref this.MaxModsAllowed);
		string text3 = "";
		this.Properties.ParseString(ItemClass.PropRestrictedMove, ref text3);
		string[] array3 = text3.Split(',', StringSplitOptions.None);
		if (text3.Length > 0)
		{
			if (!bool.TryParse(array3[0], out this.bRestrictedMove))
			{
				this.bRestrictedMove = false;
			}
			if (this.bRestrictedMove)
			{
				for (int j = 1; j < array3.Length; j++)
				{
					XUiC_ItemStack.StackLocationTypes stackLocationTypes;
					if (Enum.TryParse<XUiC_ItemStack.StackLocationTypes>(array3[j], out stackLocationTypes))
					{
						this.restrictedTo.Add(stackLocationTypes);
					}
				}
			}
		}
		if (this.Properties.Values.ContainsKey("TraderQualityMod"))
		{
			float num3 = 1f;
			float num4 = 1f;
			StringParsers.ParseMinMaxCount(this.Properties.Values["TraderQualityMod"], out num3, out num4);
			this.TraderQualityMinMod = num3;
			this.TraderQualityMaxMod = num4;
		}
		else
		{
			this.TraderQualityMaxMod = 0f;
			this.TraderQualityMinMod = 0f;
		}
		this.Properties.ParseString(ItemClass.PropPreviousItemType, ref this.previousTechTypeName);
		this.CalculateTechType();
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x001401AC File Offset: 0x0013E3AC
	public void LateInit()
	{
		if (this.HasQuality)
		{
			this.Stacknumber.Value = 1;
		}
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x001401C4 File Offset: 0x0013E3C4
	public static void InitStatic()
	{
		ItemClass.list = new ItemClass[ItemClass.MAX_ITEMS];
		ItemClass.itemActionNames = new string[5];
		for (int i = 0; i < 5; i++)
		{
			ItemClass.itemActionNames[i] = "Action" + i.ToString();
		}
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x00140210 File Offset: 0x0013E410
	public static void LateInitAll()
	{
		for (int i = 0; i < ItemClass.MAX_ITEMS; i++)
		{
			if (ItemClass.list[i] != null)
			{
				ItemClass.list[i].LateInit();
			}
		}
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x00140242 File Offset: 0x0013E442
	public static void Cleanup()
	{
		ItemClass.list = null;
		ItemClass.nameToItem.Clear();
		ItemClass.nameToItemCaseInsensitive.Clear();
		ItemClass.itemNames.Clear();
		ItemClass.itemActionNames = null;
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x0014026E File Offset: 0x0013E46E
	public virtual int GetInitialMetadata(ItemValue _itemValue)
	{
		if (this.Actions[0] == null)
		{
			return 0;
		}
		return this.Actions[0].GetInitialMeta(_itemValue);
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x0014028A File Offset: 0x0013E48A
	public static ItemClass GetForId(int _id)
	{
		if (ItemClass.list == null || (ulong)_id >= (ulong)((long)ItemClass.list.Length))
		{
			return null;
		}
		return ItemClass.list[_id];
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x001402A8 File Offset: 0x0013E4A8
	public static ItemValue GetItem(string _itemName, bool _caseInsensitive = false)
	{
		ItemClass itemClass = ItemClass.GetItemClass(_itemName, _caseInsensitive);
		if (itemClass != null)
		{
			return new ItemValue(itemClass.Id, false);
		}
		return ItemValue.None;
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x001402D4 File Offset: 0x0013E4D4
	public static ItemValue CreateItemValue(string _itemName, int _quality, bool _caseInsensitive = false)
	{
		ItemClass itemClass = ItemClass.GetItemClass(_itemName, _caseInsensitive);
		if (itemClass != null)
		{
			return new ItemValue(itemClass.Id, _quality, _quality, false, null, 1f);
		}
		return ItemValue.None;
	}

	// Token: 0x06003120 RID: 12576 RVA: 0x00140308 File Offset: 0x0013E508
	public static ItemClass GetItemClass(string _itemName, bool _caseInsensitive = false)
	{
		ItemClass itemClass;
		if (_caseInsensitive)
		{
			ItemClass.nameToItemCaseInsensitive.TryGetValue(_itemName, out itemClass);
		}
		else
		{
			ItemClass.nameToItem.TryGetValue(_itemName, out itemClass);
		}
		return itemClass;
	}

	// Token: 0x06003121 RID: 12577 RVA: 0x00140338 File Offset: 0x0013E538
	public static void GetItemsAndBlocks(List<ItemClass> _targetList, int _idStart = -1, int _idEndExcl = -1, ItemClass.FilterItem[] _filterExprs = null, string _nameFilter = null, bool _bShowUserHidden = false, EnumCreativeMode _currentCreativeMode = EnumCreativeMode.Player, bool _showFavorites = false, bool _sortBySortOrder = false, XUi _xui = null)
	{
		_targetList.Clear();
		if (_idStart < 0)
		{
			_idStart = 0;
		}
		if (_idEndExcl < 0)
		{
			_idEndExcl = ItemClass.list.Length;
		}
		if (string.IsNullOrEmpty(_nameFilter))
		{
			_nameFilter = null;
		}
		int num = -1;
		if (_nameFilter != null)
		{
			if (_nameFilter.Length >= 2 && _nameFilter.StartsWith("#", StringComparison.OrdinalIgnoreCase))
			{
				_nameFilter = _nameFilter.Substring(2);
			}
			int.TryParse(_nameFilter, out num);
		}
		int i = _idStart;
		while (i < _idEndExcl)
		{
			Block block = null;
			if (i >= Block.ItemsStartHere)
			{
				goto IL_0078;
			}
			block = Block.list[i];
			if (block != null)
			{
				goto IL_0078;
			}
			IL_0170:
			i++;
			continue;
			IL_0078:
			ItemClass forId = ItemClass.GetForId(i);
			if (forId == null)
			{
				goto IL_0170;
			}
			EnumCreativeMode creativeMode = forId.CreativeMode;
			if (creativeMode != EnumCreativeMode.None && creativeMode != EnumCreativeMode.Test && (creativeMode == EnumCreativeMode.All || _currentCreativeMode == creativeMode || _bShowUserHidden) && (creativeMode != EnumCreativeMode.Console || (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent()) && (!_showFavorites || !(_xui != null) || _xui.playerUI.entityPlayer.favoriteCreativeStacks.Contains((ushort)i)))
			{
				if (_filterExprs != null)
				{
					bool flag = false;
					for (int j = 0; j < _filterExprs.Length; j++)
					{
						if (_filterExprs[j] != null)
						{
							flag = _filterExprs[j](forId, block);
							if (flag)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						goto IL_0170;
					}
				}
				if (_nameFilter != null)
				{
					string text = forId.GetLocalizedItemName() ?? Localization.Get(forId.Name, false, null);
					if ((num < 0 || forId.Id != num) && !forId.Name.ContainsCaseInsensitive(_nameFilter) && !text.ContainsCaseInsensitive(_nameFilter))
					{
						goto IL_0170;
					}
				}
				_targetList.Add(forId);
				goto IL_0170;
			}
			goto IL_0170;
		}
		if (_sortBySortOrder)
		{
			_targetList.Sort(delegate(ItemClass _icA, ItemClass _icB)
			{
				int num2 = string.CompareOrdinal(_icA.SortOrder, _icB.SortOrder);
				if (num2 != 0)
				{
					return num2;
				}
				return _icA.Id.CompareTo(_icB.Id);
			});
		}
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x001404EC File Offset: 0x0013E6EC
	public static void CreateItemStacks(IEnumerable<ItemClass> _itemClassList, List<ItemStack> _targetList, string _nameFilter = null)
	{
		_targetList.Clear();
		GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(GameManager.frameCount);
		int num = 1;
		int num2 = 6;
		if (_nameFilter != null && _nameFilter.Length >= 2 && _nameFilter.IndexOf("#", StringComparison.OrdinalIgnoreCase) == 0)
		{
			int num3 = (int)char.GetNumericValue(_nameFilter[1]);
			if (num3 >= 1 && num3 <= 6)
			{
				num = num3;
				num2 = num3;
			}
		}
		foreach (ItemClass itemClass in _itemClassList)
		{
			ItemValue itemValue = new ItemValue(itemClass.Id, num, num2, true, null, 1f);
			int num4 = (int)((itemValue.Quality == 0) ? 0 : ((itemValue.Quality - 1) * 20));
			itemClass.AddGSStats(itemValue, num4, gameRandom);
			itemValue.Meta = itemValue.ItemClass.GetInitialMetadata(itemValue);
			ItemStack itemStack = new ItemStack(itemValue, itemClass.Stacknumber.Value);
			_targetList.Add(itemStack);
		}
		GameRandomManager.Instance.FreeGameRandom(gameRandom);
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x00140600 File Offset: 0x0013E800
	public virtual bool IsHUDDisabled(ItemInventoryData _data)
	{
		return (this.Actions[0] != null && this.Actions[0].IsHUDDisabled((_data != null) ? _data.actionData[0] : null)) || (this.Actions[1] != null && this.Actions[1].IsHUDDisabled((_data != null) ? _data.actionData[1] : null));
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x00140668 File Offset: 0x0013E868
	public virtual bool IsLightSource()
	{
		return this.LightSource != null;
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x00140676 File Offset: 0x0013E876
	public virtual Transform CloneModel(GameObject _reuseThisGO, World _world, BlockValue _blockValue, Vector3[] _vertices, Vector3 _position, Transform _parent, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World, TextureFullArray _textureFullArray = default(TextureFullArray))
	{
		return this.CloneModel(_world, _blockValue.ToItemValue(), _position, _parent, _purpose, _textureFullArray);
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x00140690 File Offset: 0x0013E890
	public virtual Transform CloneModel(World _world, ItemValue _itemValue, Vector3 _position, Transform _parent, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World, TextureFullArray _textureFullArray = default(TextureFullArray))
	{
		GameObject gameObject = null;
		if (this.CanHold())
		{
			string text = null;
			if (_purpose == BlockShape.MeshPurpose.Drop)
			{
				text = _itemValue.GetPropertyOverride("DropMeshFile", this.DropMeshFile);
			}
			if (_purpose == BlockShape.MeshPurpose.Hold)
			{
				text = _itemValue.GetPropertyOverride("HandMeshfile", this.HandMeshFile);
			}
			if (text == null)
			{
				text = _itemValue.GetPropertyOverride("Meshfile", this.MeshFile);
			}
			string text2 = ((text != null) ? GameIO.GetFilenameFromPathWithoutExtension(text) : null);
			if (this.renderGameObject == null || (text != null && !text2.Equals(this.renderGameObject.name)))
			{
				this.renderGameObject = DataLoader.LoadAsset<GameObject>(text, false);
			}
			gameObject = this.renderGameObject;
			if (gameObject == null)
			{
				gameObject = LoadManager.LoadAsset<GameObject>("@:Other/Items/Crafting/leather.fbx", null, null, false, true, false).Asset;
			}
		}
		if (gameObject == null)
		{
			return null;
		}
		try
		{
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
			Transform transform = gameObject2.transform;
			transform.SetParent(_parent, false);
			transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			gameObject2.SetActive(false);
			if (_purpose == BlockShape.MeshPurpose.Hold)
			{
				Collider[] componentsInChildren = gameObject2.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				if (gameObject2 != null)
				{
					Animator[] componentsInChildren2 = gameObject2.GetComponentsInChildren<Animator>();
					for (int i = 0; i < componentsInChildren2.Length; i++)
					{
						componentsInChildren2[i].writeDefaultValuesOnDisable = true;
					}
				}
			}
			UpdateLightOnAllMaterials updateLightOnAllMaterials = gameObject2.GetComponent<UpdateLightOnAllMaterials>();
			if (updateLightOnAllMaterials == null)
			{
				updateLightOnAllMaterials = gameObject2.AddComponent<UpdateLightOnAllMaterials>();
			}
			string text3 = "255,255,255";
			this.Properties.ParseString(Block.PropTintColor, ref text3);
			Vector3 vector = Block.StringToVector3(_itemValue.GetPropertyOverride(Block.PropTintColor, text3));
			updateLightOnAllMaterials.SetTintColorForItem(vector);
			return transform;
		}
		catch (Exception ex)
		{
			Log.Error("Instantiate of '" + this.MeshFile + "' led to error: " + ex.Message);
			Log.Error(ex.StackTrace);
		}
		return null;
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x00140884 File Offset: 0x0013EA84
	public void setLocalizedItemName(string _localizedName)
	{
		this.localizedName = _localizedName;
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x0014088D File Offset: 0x0013EA8D
	public virtual string GetLocalizedItemName()
	{
		return this.localizedName;
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x00140895 File Offset: 0x0013EA95
	public void SetName(string _name)
	{
		this.pName = _name;
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x0014089E File Offset: 0x0013EA9E
	public virtual string GetItemName()
	{
		return base.Name;
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x001408A6 File Offset: 0x0013EAA6
	public virtual string GetItemDescriptionKey()
	{
		return this.DescriptionKey;
	}

	// Token: 0x0600312C RID: 12588 RVA: 0x001408AE File Offset: 0x0013EAAE
	public virtual string GetIconName()
	{
		if (this.CustomIcon != null && this.CustomIcon.Value.Length > 0)
		{
			return this.CustomIcon.Value;
		}
		return base.Name;
	}

	// Token: 0x0600312D RID: 12589 RVA: 0x001408E4 File Offset: 0x0013EAE4
	public virtual Color GetIconTint(ItemValue _instance = null)
	{
		if (_instance != null)
		{
			string text = "NONE";
			string propertyOverride = _instance.GetPropertyOverride("CustomIconTint", text);
			if (!propertyOverride.Equals(text))
			{
				return StringParsers.ParseHexColor(propertyOverride);
			}
		}
		return this.CustomIconTint;
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x0014091D File Offset: 0x0013EB1D
	public virtual bool IsGun()
	{
		return this.Actions[0] is ItemActionAttack;
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x0014092F File Offset: 0x0013EB2F
	public virtual bool IsDynamicMelee()
	{
		return this.Actions[0] is ItemActionDynamic;
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x00140941 File Offset: 0x0013EB41
	public virtual bool CanStack()
	{
		return this.Stacknumber.Value > 1;
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x00140951 File Offset: 0x0013EB51
	public void SetCanHold(bool _b)
	{
		this.bCanHold = _b;
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x0014095A File Offset: 0x0013EB5A
	public virtual bool CanHold()
	{
		return this.bCanHold;
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x00140962 File Offset: 0x0013EB62
	public void SetCanDrop(bool _b)
	{
		this.bCanDrop = _b;
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x0014096B File Offset: 0x0013EB6B
	public virtual bool CanDrop(ItemValue _iv = null)
	{
		return this.bCanDrop;
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x00140973 File Offset: 0x0013EB73
	public virtual bool RestrictedMove(ItemValue _iv = null)
	{
		return this.bRestrictedMove;
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Deactivate(ItemValue _iv)
	{
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool KeepOnDeath()
	{
		return false;
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanPlaceInContainer()
	{
		return true;
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x0014097C File Offset: 0x0013EB7C
	public virtual string CanInteract(ItemInventoryData _data)
	{
		ItemAction itemAction = this.Actions[2];
		if (itemAction == null)
		{
			return null;
		}
		return itemAction.CanInteract(_data.actionData[2]);
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x001409A9 File Offset: 0x0013EBA9
	public void Interact(ItemInventoryData _data)
	{
		this.ExecuteAction(2, _data, false, null);
		this.ExecuteAction(2, _data, true, null);
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x001409C0 File Offset: 0x0013EBC0
	public bool CanExecuteAction(int actionIdx, EntityAlive holdingEntity, ItemValue itemValue)
	{
		if (this.Actions[actionIdx] == null || this.Actions[actionIdx].ExecutionRequirements == null)
		{
			return true;
		}
		holdingEntity.MinEventContext.ItemValue = itemValue;
		return this.Actions[actionIdx].ExecutionRequirements.IsValid(holdingEntity.MinEventContext);
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x00140A0C File Offset: 0x0013EC0C
	public virtual void ExecuteAction(int _actionIdx, ItemInventoryData _data, bool _bReleased, PlayerActionsLocal _playerActions)
	{
		ItemAction curAction = this.Actions[_actionIdx];
		if (curAction == null)
		{
			return;
		}
		if (curAction is ItemActionDynamicMelee)
		{
			bool flag = _bReleased;
			if (this.Actions.Length >= 2 && this.Actions[0] != null && this.Actions[1] != null)
			{
				flag = !this.Actions[1].IsActionRunning(_data.actionData[1]) && !this.Actions[0].IsActionRunning(_data.actionData[0]);
			}
			if (!flag)
			{
				return;
			}
			if (!_bReleased)
			{
				flag &= curAction.CanExecute(_data.actionData[_actionIdx]);
			}
			if (!flag)
			{
				return;
			}
			if (_data != null && _data.holdingEntity.emodel != null && _data.holdingEntity.emodel.avatarController != null)
			{
				_data.holdingEntity.emodel.avatarController.UpdateInt(AvatarController.itemActionIndexHash, _actionIdx, true);
			}
			curAction.ExecuteAction((_data != null) ? _data.actionData[_actionIdx] : null, _bReleased);
			return;
		}
		else
		{
			global::ItemActionData actionData = _data.actionData[_actionIdx];
			bool flag2 = _bReleased || !curAction.IsActionRunning(actionData);
			if (!flag2)
			{
				return;
			}
			if (!_bReleased)
			{
				flag2 &= curAction.CanExecute(actionData);
			}
			if (!flag2)
			{
				GameManager.ShowTooltip(_data.holdingEntity as EntityPlayerLocal, Localization.Get("ttCannotUseAtThisTime", false, null), string.Empty, "ui_denied", null, false, false, 0f);
				return;
			}
			if (_data != null && _data.holdingEntity.emodel != null && _data.holdingEntity.emodel.avatarController != null)
			{
				_data.holdingEntity.emodel.avatarController.UpdateInt(AvatarController.itemActionIndexHash, _actionIdx, true);
			}
			if (!_bReleased)
			{
				if (!actionData.HasExecuted)
				{
					_data.holdingEntity.MinEventContext.ItemValue = _data.itemValue;
					_data.holdingEntity.FireEvent(MinEvent.Start[_actionIdx], true);
				}
				if (curAction is ItemActionRanged && !(curAction is ItemActionLauncher) && !(curAction is ItemActionCatapult))
				{
					actionData.HasExecuted = true;
					curAction.ExecuteAction(actionData, _bReleased);
					return;
				}
				if (!actionData.HasExecuted)
				{
					ItemActionEat itemActionEat = curAction as ItemActionEat;
					if (itemActionEat != null)
					{
						LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_data.holdingEntity as EntityPlayerLocal);
						XUi xui = ((uiforPlayer != null) ? uiforPlayer.xui : null);
						if (itemActionEat.UsePrompt && !xui.IsUsingItemActionEntryPromptComplete)
						{
							XUiC_MessageBoxWindowGroup.ShowOkCancel(xui, Localization.Get(itemActionEat.PromptTitle, false, null), Localization.Get(itemActionEat.PromptDescription, false, null), "", delegate
							{
								_data.holdingEntity.MinEventContext.ItemValue = _data.holdingEntity.inventory.holdingItemItemValue;
								actionData.HasExecuted = false;
								curAction.ExecuteAction(actionData, true);
								xui.IsUsingItemActionEntryPromptComplete = false;
							}, null, true, true, false);
							return;
						}
						xui.IsUsingItemActionEntryPromptComplete = false;
					}
					_data.holdingEntity.MinEventContext.ItemValue = _data.holdingEntity.inventory.holdingItemItemValue;
					actionData.HasExecuted = true;
					curAction.ExecuteAction(actionData, _bReleased);
					return;
				}
			}
			else if (actionData.HasExecuted)
			{
				if (!(curAction is ItemActionUseOther))
				{
					ItemValue itemValue = _data.itemValue;
					if (!curAction.IsEndDelayed() || !curAction.UseAnimation)
					{
						_data.holdingEntity.MinEventContext.ItemValue = itemValue;
						_data.holdingEntity.FireEvent(MinEvent.End[_actionIdx], true);
					}
					if (_data.holdingEntity as EntityPlayerLocal != null)
					{
						ItemClass itemClass = itemValue.ItemClass;
						if (itemClass != null && itemClass.HasAnyTags(this.stopBleed) && _data.holdingEntity.Buffs.HasBuff("buffInjuryBleeding"))
						{
							IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
							if (achievementManager != null)
							{
								achievementManager.SetAchievementStat(EnumAchievementDataStat.BleedOutStopped, 1);
							}
						}
					}
				}
				if (curAction is ItemActionActivate && _bReleased && !(flag2 & curAction.CanExecute(actionData)))
				{
					GameManager.ShowTooltip(_data.holdingEntity as EntityPlayerLocal, Localization.Get("ttCannotUseAtThisTime", false, null), string.Empty, "ui_denied", null, false, false, 0f);
					actionData.HasExecuted = true;
					return;
				}
				_data.holdingEntity.MinEventContext.ItemValue = _data.holdingEntity.inventory.holdingItemItemValue;
				actionData.HasExecuted = false;
				curAction.ExecuteAction(actionData, _bReleased);
			}
			return;
		}
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x00140F60 File Offset: 0x0013F160
	public virtual bool IsActionRunning(ItemInventoryData _data)
	{
		for (int i = 0; i < 3; i++)
		{
			ItemAction itemAction = this.Actions[i];
			if (itemAction != null && itemAction.IsActionRunning(_data.actionData[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingItemActivated(ItemInventoryData _data)
	{
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnPlacedAsCatalyst(ItemInventoryData _data)
	{
	}

	// Token: 0x06003140 RID: 12608 RVA: 0x00140F9C File Offset: 0x0013F19C
	public virtual void StartHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		for (int i = 0; i < 3; i++)
		{
			ItemAction itemAction = this.Actions[i];
			if (itemAction != null)
			{
				itemAction.StartHolding(_data.actionData[i]);
			}
		}
		if (this.Actions[0] != null || this.Actions[1] != null)
		{
			_data.holdingEntitySoundID = -1;
		}
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x00140FF0 File Offset: 0x0013F1F0
	public virtual void CleanupHoldingActions(ItemInventoryData _data)
	{
		if (_data == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			ItemAction itemAction = this.Actions[i];
			if (itemAction != null)
			{
				itemAction.Cleanup(_data.actionData[i]);
			}
		}
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x0014102C File Offset: 0x0013F22C
	public virtual void OnHoldingUpdate(ItemInventoryData _data)
	{
		EntityAlive holdingEntity = _data.holdingEntity;
		for (int i = 0; i < 3; i++)
		{
			ItemAction itemAction = this.Actions[i];
			if (itemAction != null)
			{
				holdingEntity.MinEventContext.ItemValue = holdingEntity.inventory.holdingItemItemValue;
				holdingEntity.MinEventContext.ItemActionData = _data.actionData[i];
				holdingEntity.FireEvent(MinEvent.Update[i], true);
				itemAction.OnHoldingUpdate((_data != null) ? _data.actionData[i] : null);
			}
		}
		if (this.Properties.Values.ContainsKey(ItemClass.PropSoundIdle) && !_data.holdingEntity.isEntityRemote)
		{
			if (_data.holdingEntitySoundID == 0 && _data.itemValue.Meta == 0)
			{
				Manager.BroadcastStop(_data.holdingEntity.entityId, this.Properties.Values[ItemClass.PropSoundIdle]);
				return;
			}
			if (_data.holdingEntitySoundID == -1 && _data.itemValue.Meta > 0)
			{
				Manager.BroadcastPlay(_data.holdingEntity, this.Properties.Values[ItemClass.PropSoundIdle], false, 1f);
				_data.holdingEntitySoundID = 0;
			}
		}
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingReset(ItemInventoryData _data)
	{
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x00141154 File Offset: 0x0013F354
	public void StopHoldingAudio(ItemInventoryData _data)
	{
		if (this.Properties.GetValue(ItemClass.PropSoundIdle) != null && _data.holdingEntitySoundID == 0)
		{
			Manager.BroadcastStop(_data.holdingEntity.entityId, this.Properties.GetValue(ItemClass.PropSoundIdle));
		}
		_data.holdingEntitySoundID = -2;
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x001411A4 File Offset: 0x0013F3A4
	public virtual void StopHolding(ItemInventoryData _data, Transform _modelTransform)
	{
		this.StopHoldingAudio(_data);
		if (_data.holdingEntity is EntityPlayer && !_data.holdingEntity.isEntityRemote && _data.holdingEntity.AimingGun)
		{
			_data.holdingEntity.AimingGun = false;
		}
		for (int i = 0; i < 3; i++)
		{
			ItemAction itemAction = this.Actions[i];
			if (itemAction != null)
			{
				itemAction.StopHolding(_data.actionData[i]);
			}
		}
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnMeshCreated(ItemWorldData _data)
	{
	}

	// Token: 0x06003147 RID: 12615 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnDroppedUpdate(ItemWorldData _data)
	{
	}

	// Token: 0x06003148 RID: 12616 RVA: 0x0004E0D7 File Offset: 0x0004C2D7
	public virtual BlockValue OnConvertToBlockValue(ItemValue _itemValue, BlockValue _blueprintBlockValue)
	{
		return _blueprintBlockValue;
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x00141218 File Offset: 0x0013F418
	public ItemInventoryData CreateInventoryData(ItemStack _itemStack, IGameManager _gameManager, EntityAlive _holdingEntity, int _slotIdxInInventory)
	{
		ItemInventoryData itemInventoryData = this.createItemInventoryData(_itemStack, _gameManager, _holdingEntity, _slotIdxInInventory);
		itemInventoryData.actionData[0] = ((this.Actions[0] != null) ? this.Actions[0].CreateModifierData(itemInventoryData, 0) : null);
		itemInventoryData.actionData[1] = ((this.Actions[1] != null) ? this.Actions[1].CreateModifierData(itemInventoryData, 1) : null);
		if (this.Actions[2] != null)
		{
			itemInventoryData.actionData.Add(this.Actions[2].CreateModifierData(itemInventoryData, 2));
		}
		return itemInventoryData;
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x001412A6 File Offset: 0x0013F4A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual ItemInventoryData createItemInventoryData(ItemStack _itemStack, IGameManager _gameManager, EntityAlive _holdingEntity, int _slotIdxInInventory)
	{
		return new ItemInventoryData(this, _itemStack, _gameManager, _holdingEntity, _slotIdxInInventory);
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x001412B3 File Offset: 0x0013F4B3
	public virtual ItemWorldData CreateWorldData(IGameManager _gm, EntityItem _entityItem, ItemValue _itemValue, int _belongsEntityId)
	{
		return new ItemWorldData(_gm, _itemValue, _entityItem, _belongsEntityId);
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x001412C0 File Offset: 0x0013F4C0
	public virtual void OnHUD(ItemInventoryData _data, int _x, int _y)
	{
		if (this.Actions[0] != null)
		{
			this.Actions[0].OnHUD(_data.actionData[0], _x, _y);
		}
		if (this.Actions[1] != null)
		{
			this.Actions[1].OnHUD(_data.actionData[1], _x, _y);
		}
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x00141318 File Offset: 0x0013F518
	public virtual void OnScreenOverlay(ItemInventoryData _data)
	{
		if (this.Actions[0] != null)
		{
			this.Actions[0].OnScreenOverlay(_data.actionData[0]);
		}
		if (this.Actions[1] != null)
		{
			this.Actions[1].OnScreenOverlay(_data.actionData[1]);
		}
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x0014136C File Offset: 0x0013F56C
	public virtual RenderCubeType GetFocusType(ItemInventoryData _data)
	{
		if (!this.CanHold())
		{
			return RenderCubeType.None;
		}
		RenderCubeType renderCubeType = RenderCubeType.None;
		if (this.Actions[0] != null)
		{
			renderCubeType = this.Actions[0].GetFocusType((_data != null) ? _data.actionData[0] : null);
		}
		RenderCubeType renderCubeType2 = RenderCubeType.None;
		if (this.Actions[1] != null)
		{
			renderCubeType2 = this.Actions[1].GetFocusType((_data != null) ? _data.actionData[1] : null);
		}
		if (renderCubeType <= renderCubeType2)
		{
			return renderCubeType2;
		}
		return renderCubeType;
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x001413E2 File Offset: 0x0013F5E2
	public virtual float GetFocusRange()
	{
		if (this.Actions[0] != null && this.Actions[0] is ItemActionAttack)
		{
			return ((ItemActionAttack)this.Actions[0]).Range;
		}
		return 0f;
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x00141418 File Offset: 0x0013F618
	public virtual bool IsFocusBlockInside()
	{
		bool flag = false;
		if (this.Actions[0] != null)
		{
			flag = this.Actions[0].IsFocusBlockInside();
		}
		bool flag2 = false;
		if (this.Actions[1] != null)
		{
			flag2 = this.Actions[1].IsFocusBlockInside();
		}
		return flag2 && flag;
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x0014145C File Offset: 0x0013F65C
	public virtual bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		bool flag = false;
		if (this.Actions[0] != null)
		{
			flag = this.Actions[0].ConsumeScrollWheel(_data.actionData[0], _scrollWheelInput, _playerInput);
		}
		if (!flag && this.Actions[1] != null)
		{
			flag = this.Actions[1].ConsumeScrollWheel(_data.actionData[1], _scrollWheelInput, _playerInput);
		}
		return flag;
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x001414BC File Offset: 0x0013F6BC
	public virtual bool ConsumeCameraFunction(ItemInventoryData _data)
	{
		bool flag = false;
		if (this.Actions[0] != null)
		{
			flag = this.Actions[0].ConsumeCameraFunction(_data.actionData[0]);
		}
		if (!flag && this.Actions[1] != null)
		{
			flag = this.Actions[1].ConsumeCameraFunction(_data.actionData[1]);
		}
		return flag;
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CheckKeys(ItemInventoryData _data, WorldRayHitInfo _hitInfo)
	{
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x00141517 File Offset: 0x0013F717
	public virtual float GetLifetimeOnDrop()
	{
		return 60f;
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual Block GetBlock()
	{
		return null;
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsBlock()
	{
		return false;
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsProp()
	{
		return false;
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x00141520 File Offset: 0x0013F720
	public virtual ItemClass.EnumCrosshairType GetCrosshairType(ItemInventoryData _holdingData)
	{
		ItemClass.EnumCrosshairType enumCrosshairType = ItemClass.EnumCrosshairType.Plus;
		ItemClass.EnumCrosshairType enumCrosshairType2 = ItemClass.EnumCrosshairType.Plus;
		if (this.Actions[0] != null)
		{
			enumCrosshairType = this.Actions[0].GetCrosshairType(_holdingData.actionData[0]);
		}
		if (this.Actions[1] != null)
		{
			enumCrosshairType2 = this.Actions[1].GetCrosshairType(_holdingData.actionData[1]);
		}
		ItemClass.EnumCrosshairType enumCrosshairType3 = ((enumCrosshairType > enumCrosshairType2) ? enumCrosshairType : enumCrosshairType2);
		string propertyOverride = _holdingData.itemValue.GetPropertyOverride(ItemClass.PropCrosshairOnAim, string.Empty);
		if (propertyOverride.Length > 0)
		{
			this.bShowCrosshairOnAiming = StringParsers.ParseBool(propertyOverride, 0, -1, true);
		}
		if (enumCrosshairType3 == ItemClass.EnumCrosshairType.Crosshair && this.bShowCrosshairOnAiming)
		{
			enumCrosshairType3 = ItemClass.EnumCrosshairType.CrosshairOnAiming;
		}
		return enumCrosshairType3;
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x001415C0 File Offset: 0x0013F7C0
	public virtual void GetIronSights(ItemInventoryData _invData, out float _fov)
	{
		if (this.Actions[0] != null)
		{
			this.Actions[0].GetIronSights(_invData.actionData[0], out _fov);
			if (_fov != 0f)
			{
				return;
			}
		}
		if (this.Actions[1] != null)
		{
			this.Actions[1].GetIronSights(_invData.actionData[1], out _fov);
			if (_fov != 0f)
			{
				return;
			}
		}
		_fov = (float)_invData.holdingEntity.GetCameraFOV();
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x00141638 File Offset: 0x0013F838
	public virtual EnumCameraShake GetCameraShakeType(ItemInventoryData _invData)
	{
		EnumCameraShake enumCameraShake = EnumCameraShake.None;
		EnumCameraShake enumCameraShake2 = EnumCameraShake.None;
		if (this.Actions[0] != null)
		{
			enumCameraShake = this.Actions[0].GetCameraShakeType(_invData.actionData[0]);
		}
		if (this.Actions[1] != null)
		{
			enumCameraShake2 = this.Actions[1].GetCameraShakeType(_invData.actionData[1]);
		}
		if (enumCameraShake > enumCameraShake2)
		{
			return enumCameraShake;
		}
		return enumCameraShake2;
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x00141698 File Offset: 0x0013F898
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		if (this.Actions[0] != null)
		{
			return this.Actions[0].GetControllerTriggerEffectPull();
		}
		if (this.Actions[1] != null)
		{
			return this.Actions[1].GetControllerTriggerEffectPull();
		}
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x001416CF File Offset: 0x0013F8CF
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		if (this.Actions[0] != null)
		{
			return this.Actions[0].GetControllerTriggerEffectShoot();
		}
		if (this.Actions[1] != null)
		{
			return this.Actions[1].GetControllerTriggerEffectShoot();
		}
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x0600315D RID: 12637 RVA: 0x00141706 File Offset: 0x0013F906
	public virtual bool IsActivated(ItemValue _value)
	{
		return _value.Activated > 0;
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x00141711 File Offset: 0x0013F911
	public virtual void SetActivated(ref ItemValue _value, bool _activated)
	{
		_value.Activated = (_activated ? 1 : 0);
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x00141722 File Offset: 0x0013F922
	public virtual Vector3 GetDroppedCorrectionRotation()
	{
		return new Vector3(-90f, 0f, 0f);
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x0004E076 File Offset: 0x0004C276
	public virtual Vector3 GetCorrectionRotation()
	{
		return Vector3.zero;
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x0004E076 File Offset: 0x0004C276
	public virtual Vector3 GetCorrectionPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x0004E076 File Offset: 0x0004C276
	public virtual Vector3 GetCorrectionScale()
	{
		return Vector3.zero;
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnDamagedByExplosion(ItemWorldData _data)
	{
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x00141738 File Offset: 0x0013F938
	public static int GetFuelValue(ItemValue _itemValue)
	{
		ItemClass itemClass = ItemClass.list[_itemValue.type];
		if (itemClass == null)
		{
			return 0;
		}
		if (itemClass.IsBlock())
		{
			return Block.list[_itemValue.type].FuelValue;
		}
		if (itemClass.FuelValue == null)
		{
			return 0;
		}
		return itemClass.FuelValue.Value;
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x00141786 File Offset: 0x0013F986
	public int GetWeight()
	{
		if (this.IsBlock())
		{
			return this.GetBlock().GetWeight();
		}
		if (base.Weight != null)
		{
			return base.Weight.Value;
		}
		return 0;
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x001417B1 File Offset: 0x0013F9B1
	public string GetImageEffect()
	{
		if (base.ImageEffectOnActive != null)
		{
			return base.ImageEffectOnActive.Value;
		}
		return "";
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x001417D2 File Offset: 0x0013F9D2
	public bool GetActive()
	{
		return base.Active != null && base.Active.Value;
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x001417E9 File Offset: 0x0013F9E9
	public string GetSoundOnActive()
	{
		if (base.PlaySoundOnActive != null)
		{
			return base.PlaySoundOnActive.Value;
		}
		return "";
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x0014180A File Offset: 0x0013FA0A
	public void SetWeight(int _w)
	{
		if (this.IsBlock())
		{
			this.GetBlock().Weight = new DataItem<int>(_w);
			return;
		}
		base.Weight = new DataItem<int>(_w);
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x00141832 File Offset: 0x0013FA32
	public void SetImageEffect(string _str)
	{
		base.ImageEffectOnActive = new DataItem<string>(_str);
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x00141840 File Offset: 0x0013FA40
	public void SetActive(bool _bOn)
	{
		base.Active = new DataItem<bool>(_bOn);
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x0014184E File Offset: 0x0013FA4E
	public void SetSoundOnActive(string _str)
	{
		base.PlaySoundOnActive = new DataItem<string>(_str);
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x0014185C File Offset: 0x0013FA5C
	public int AutoCalcWeight(Dictionary<string, List<Recipe>> _recipesByName)
	{
		Block block = (this.IsBlock() ? this.GetBlock() : null);
		if (block != null)
		{
			if (block.Weight != null)
			{
				if (block.Weight.Value != -1)
				{
					return block.Weight.Value;
				}
				return 0;
			}
			else
			{
				block.Weight = new DataItem<int>(-1);
			}
		}
		else if (base.Weight != null)
		{
			if (base.Weight.Value != -1)
			{
				return base.Weight.Value;
			}
			return 0;
		}
		else
		{
			base.Weight = new DataItem<int>(-1);
		}
		int num = 0;
		int num2 = 0;
		List<Recipe> list;
		if (_recipesByName.TryGetValue(this.GetItemName(), out list))
		{
			Recipe recipe = list[0];
			for (int i = 0; i < recipe.ingredients.Count; i++)
			{
				ItemStack itemStack = recipe.ingredients[i];
				ItemClass forId = ItemClass.GetForId(itemStack.itemValue.type);
				ItemClass forId2 = ItemClass.GetForId(recipe.itemValueType);
				if (recipe.materialBasedRecipe)
				{
					if (forId2.MadeOfMaterial.ForgeCategory != null && forId.MadeOfMaterial.ForgeCategory != null && forId2.MadeOfMaterial.ForgeCategory.EqualsCaseInsensitive(forId.MadeOfMaterial.ForgeCategory))
					{
						num += forId.AutoCalcWeight(_recipesByName) * itemStack.count;
						num2++;
						break;
					}
				}
				else if (forId2.MadeOfMaterial.ForgeCategory != null && forId.MadeOfMaterial.ForgeCategory != null && forId2.MadeOfMaterial.ForgeCategory.EqualsCaseInsensitive(forId.MadeOfMaterial.ForgeCategory))
				{
					if (ItemClass.GetForId(itemStack.itemValue.type).GetWeight() > 0)
					{
						num += ItemClass.GetForId(itemStack.itemValue.type).GetWeight() * itemStack.count;
						num2++;
					}
					else
					{
						num += forId.AutoCalcWeight(_recipesByName) * itemStack.count;
						num2++;
					}
				}
			}
			num /= ((num2 > 1) ? recipe.count : 1);
		}
		if (block != null)
		{
			block.Weight = new DataItem<int>(num);
		}
		else
		{
			base.Weight = new DataItem<int>(num);
		}
		return num;
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x00141A7F File Offset: 0x0013FC7F
	public virtual bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ItemTags.Test_AnySet(_tags);
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x00141A8D File Offset: 0x0013FC8D
	public virtual bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ItemTags.Test_AllSet(_tags);
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x00141A9C File Offset: 0x0013FC9C
	public static ItemClass GetItemWithTag(FastTags<TagGroup.Global> _tags)
	{
		if (ItemClass.list != null)
		{
			for (int i = 0; i < ItemClass.list.Length; i++)
			{
				if (ItemClass.list[i] != null && ItemClass.list[i].HasAllTags(_tags))
				{
					return ItemClass.list[i];
				}
			}
		}
		return null;
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x00141AE4 File Offset: 0x0013FCE4
	public static List<ItemClass> GetItemsWithTag(FastTags<TagGroup.Global> _tags)
	{
		List<ItemClass> list = new List<ItemClass>();
		if (ItemClass.list != null)
		{
			for (int i = 0; i < ItemClass.list.Length; i++)
			{
				if (ItemClass.list[i] != null && ItemClass.list[i].HasAllTags(_tags))
				{
					list.Add(ItemClass.list[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanCollect(ItemValue _iv)
	{
		return true;
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x00141B38 File Offset: 0x0013FD38
	public float AutoCalcEcoVal(Dictionary<string, List<Recipe>> _recipesByName, List<string> _recipeCalcStack)
	{
		string itemName = this.GetItemName();
		if (_recipeCalcStack.ContainsWithComparer(itemName, StringComparer.Ordinal))
		{
			return -1f;
		}
		Block block = (this.IsBlock() ? this.GetBlock() : null);
		float num = ((block != null) ? block.EconomicValue : this.EconomicValue);
		if (num > 0f)
		{
			return num;
		}
		if ((double)num < -0.1)
		{
			return 0f;
		}
		_recipeCalcStack.Add(itemName);
		float num2 = 0f;
		int num3 = 0;
		List<Recipe> list;
		if (_recipesByName.TryGetValue(itemName, out list))
		{
			foreach (Recipe recipe in list)
			{
				for (int i = 0; i < recipe.ingredients.Count; i++)
				{
					ItemStack itemStack = recipe.ingredients[i];
					float num4 = ItemClass.GetForId(itemStack.itemValue.type).AutoCalcEcoVal(_recipesByName, _recipeCalcStack);
					if (num4 < 0f)
					{
						num2 = -1f;
						break;
					}
					num2 += (float)itemStack.count * num4;
					num3++;
				}
				if (num2 >= 0f)
				{
					num2 /= (float)((num3 > 1) ? recipe.count : 1);
					break;
				}
			}
		}
		_recipeCalcStack.RemoveAt(_recipeCalcStack.Count - 1);
		if (num2 < 0f)
		{
			return -1f;
		}
		if (num2 == 0f)
		{
			num2 = 1f;
		}
		if (block != null)
		{
			block.EconomicValue = num2;
		}
		this.EconomicValue = num2;
		return num2;
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x00141CC8 File Offset: 0x0013FEC8
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		if (this.Effects != null)
		{
			this.Effects.FireEvent(_eventType, _eventParms);
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06003175 RID: 12661 RVA: 0x00141CDF File Offset: 0x0013FEDF
	public bool IsEatDistraction
	{
		get
		{
			return this.DistractionTags.Test_AnySet(ItemClass.EatDistractionTag);
		}
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x06003176 RID: 12662 RVA: 0x00141CF1 File Offset: 0x0013FEF1
	public bool IsRequireContactDistraction
	{
		get
		{
			return this.DistractionTags.Test_AnySet(ItemClass.RequiresContactDistractionTag);
		}
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x00141D03 File Offset: 0x0013FF03
	public bool HasTrigger(MinEventTypes _eventType)
	{
		return this.Effects != null && this.Effects.HasTrigger(_eventType);
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x00141D1C File Offset: 0x0013FF1C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void assignIdsFromXml()
	{
		Log.Out("ItemIDs from XML");
		foreach (KeyValuePair<string, ItemClass> keyValuePair in ItemClass.nameToItem)
		{
			if (!keyValuePair.Value.IsBlock())
			{
				ItemClass.assignId(keyValuePair.Value, keyValuePair.Value.Id, null);
			}
		}
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x00141D98 File Offset: 0x0013FF98
	[PublicizedFrom(EAccessModifier.Private)]
	public static void assignIdsLinear()
	{
		Log.Out("ItemIDs linear");
		bool[] array = new bool[ItemClass.MAX_ITEMS];
		List<ItemClass> list = new List<ItemClass>(ItemClass.nameToItem.Count);
		ItemClass.nameToItem.CopyValuesTo<string, ItemClass>(list);
		ItemClass.assignLeftOverItems(array, list);
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x00141DDA File Offset: 0x0013FFDA
	[PublicizedFrom(EAccessModifier.Private)]
	public static void assignId(ItemClass _b, int _id, bool[] _usedIds)
	{
		ItemClass.list[_id] = _b;
		_b.SetId(_id);
		if (_usedIds != null)
		{
			_usedIds[_id] = true;
		}
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x00141DF4 File Offset: 0x0013FFF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void assignLeftOverItems(bool[] _usedIds, List<ItemClass> _unassignedItems)
	{
		foreach (KeyValuePair<string, int> keyValuePair in ItemClass.fixedItemIds)
		{
			if (ItemClass.nameToItem.ContainsKey(keyValuePair.Key))
			{
				ItemClass itemClass = ItemClass.nameToItem[keyValuePair.Key];
				if (_unassignedItems.Contains(itemClass))
				{
					_unassignedItems.Remove(itemClass);
					ItemClass.assignId(itemClass, keyValuePair.Value + Block.ItemsStartHere, _usedIds);
				}
			}
		}
		int num = Block.ItemsStartHere;
		foreach (ItemClass itemClass2 in _unassignedItems)
		{
			if (!itemClass2.IsBlock())
			{
				while (_usedIds[++num])
				{
				}
				ItemClass.assignId(itemClass2, num, _usedIds);
			}
		}
		Log.Out("ItemClass assignLeftOverItems {0} of {1}", new object[]
		{
			num,
			ItemClass.MAX_ITEMS
		});
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x00141F08 File Offset: 0x00140108
	[PublicizedFrom(EAccessModifier.Private)]
	public static void assignIdsFromMapping()
	{
		Log.Out("ItemIDs from Mapping");
		List<ItemClass> list = new List<ItemClass>();
		bool[] array = new bool[ItemClass.MAX_ITEMS];
		foreach (KeyValuePair<string, ItemClass> keyValuePair in ItemClass.nameToItem)
		{
			if (!keyValuePair.Value.IsBlock())
			{
				int idForName = ItemClass.nameIdMapping.GetIdForName(keyValuePair.Key);
				if (idForName >= 0)
				{
					ItemClass.assignId(keyValuePair.Value, idForName, array);
				}
				else
				{
					list.Add(keyValuePair.Value);
				}
			}
		}
		ItemClass.assignLeftOverItems(array, list);
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x00141FBC File Offset: 0x001401BC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void createFullMappingForClients()
	{
		NameIdMapping nameIdMapping = new NameIdMapping(null, ItemClass.MAX_ITEMS);
		foreach (KeyValuePair<string, ItemClass> keyValuePair in ItemClass.nameToItem)
		{
			nameIdMapping.AddMapping(keyValuePair.Value.Id, keyValuePair.Key, false);
		}
		ItemClass.fullMappingDataForClients = nameIdMapping.SaveToArray();
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x00142038 File Offset: 0x00140238
	public static void AssignIds()
	{
		if (ItemClass.nameIdMapping != null)
		{
			Log.Out("Item IDs with mapping");
			ItemClass.assignIdsFromMapping();
		}
		else
		{
			Log.Out("Item IDs withOUT mapping");
			ItemClass.assignIdsLinear();
		}
		ItemClass.createFullMappingForClients();
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x0600317F RID: 12671 RVA: 0x00142066 File Offset: 0x00140266
	public static ItemClass MissingItem
	{
		get
		{
			return ItemClass.GetItemClass("missingItem", false);
		}
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x00142074 File Offset: 0x00140274
	public void GSStatsParseXml(XElement _element)
	{
		foreach (XElement xelement in _element.Elements("stats"))
		{
			foreach (XElement xelement2 in xelement.Elements())
			{
				if (xelement2.Name == "stat")
				{
					string attribute = xelement2.GetAttribute("name");
					if (attribute.Length > 0)
					{
						string attribute2 = xelement2.GetAttribute("value");
						if (attribute2.Length > 0)
						{
							string[] array = attribute2.Split(',', StringSplitOptions.None);
							if (array.Length >= 5)
							{
								PassiveEffects passiveEffects = EnumUtils.Parse<PassiveEffects>(attribute, false);
								if (this.Stats == null)
								{
									this.Stats = new Dictionary<PassiveEffects, List<ItemClass.GSStat>>();
								}
								List<ItemClass.GSStat> list;
								if (!this.Stats.TryGetValue(passiveEffects, out list))
								{
									list = new List<ItemClass.GSStat>();
									this.Stats[passiveEffects] = list;
								}
								ItemClass.GSStat gsstat;
								gsstat.quality = short.Parse(array[0]);
								gsstat.gameStage = short.Parse(array[1]);
								gsstat.chance = float.Parse(array[2]);
								float num = float.Parse(array[3]);
								float num2 = float.Parse(array[4]);
								if (Utils.FastAbs(num) < 163.83499f && Utils.FastAbs(num2) < 163.83499f)
								{
									gsstat.min = (short)(num / 0.005f);
									gsstat.max = (short)(num2 / 0.005f);
									list.Add(gsstat);
								}
								else
								{
									Log.Warning("GSStat {0}, qual {1}, gs {2}, chance {3}, {4} to {5} out of range", new object[] { passiveEffects, gsstat.quality, gsstat.gameStage, gsstat.chance, num, num2 });
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x001422B4 File Offset: 0x001404B4
	public void AddGSStats(ItemValue _iv, int _stage, GameRandom _rand)
	{
		if (this.Stats != null)
		{
			_iv.InitStats(this.Stats.Count);
			int quality = (int)_iv.Quality;
			int num = 0;
			foreach (KeyValuePair<PassiveEffects, List<ItemClass.GSStat>> keyValuePair in this.Stats)
			{
				PassiveEffects passiveEffects;
				List<ItemClass.GSStat> list;
				keyValuePair.Deconstruct(out passiveEffects, out list);
				PassiveEffects passiveEffects2 = passiveEffects;
				List<ItemClass.GSStat> list2 = list;
				int num2 = 0;
				int num3 = 0;
				ItemClass.GSStat gsstat = this.FindGSStat(list2, 0, 0, _rand);
				if (gsstat.quality >= 0)
				{
					num2 = (int)((short)_rand.RandomRange((int)gsstat.min, (int)(gsstat.max + 1)));
				}
				ItemClass.GSStat gsstat2 = this.FindGSStat(list2, quality, _stage, _rand);
				if (gsstat2.quality > 0)
				{
					num3 = (int)((short)_rand.RandomRange((int)gsstat2.min, (int)(gsstat2.max + 1)));
				}
				if (num2 + num3 != 0)
				{
					_iv.SetStat(num++, passiveEffects2, num2, num3);
				}
			}
			_iv.RemoveUnusedStats();
		}
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x001423C4 File Offset: 0x001405C4
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass.GSStat FindGSStat(List<ItemClass.GSStat> _list, int _quality, int _gameStage, GameRandom _rand)
	{
		int count = _list.Count;
		if (_gameStage < 0)
		{
			for (int i = 0; i < count; i++)
			{
				if ((int)_list[i].quality == _quality)
				{
					_gameStage = (int)_list[i].gameStage;
					break;
				}
			}
		}
		int num = -1;
		for (int j = count - 1; j >= 0; j--)
		{
			if ((int)_list[j].quality == _quality)
			{
				int gameStage = (int)_list[j].gameStage;
				if (gameStage <= _gameStage)
				{
					if (num < 0)
					{
						num = gameStage;
					}
					else if (gameStage < num)
					{
						break;
					}
					float chance = _list[j].chance;
					if (chance >= 1f || chance > _rand.RandomFloat)
					{
						return _list[j];
					}
				}
			}
		}
		ItemClass.GSStat gsstat = _list[0];
		gsstat.quality = -1;
		return gsstat;
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x0014248C File Offset: 0x0014068C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalculateTechType()
	{
		FastTags<TagGroup.Global> fastTags = (this.IsBlock() ? this.GetBlock().Tags : this.ItemTags);
		if (fastTags.Test_AnySet(this.T3Tag))
		{
			this.ItemTechType = ItemClass.ItemTechTypes.T3;
			return;
		}
		if (fastTags.Test_AnySet(this.T2Tag))
		{
			this.ItemTechType = ItemClass.ItemTechTypes.T2;
			return;
		}
		if (fastTags.Test_AnySet(this.T1Tag))
		{
			this.ItemTechType = ItemClass.ItemTechTypes.T1;
			return;
		}
		if (fastTags.Test_AnySet(this.T0Tag))
		{
			this.ItemTechType = ItemClass.ItemTechTypes.T0;
			return;
		}
		this.ItemTechType = ItemClass.ItemTechTypes.None;
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x00142518 File Offset: 0x00140718
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass GetPreviousTechItem()
	{
		if (this.previousTechItems == null && this.previousTechTypeName != "")
		{
			string[] array = this.previousTechTypeName.Split(',', StringSplitOptions.None);
			this.previousTechItems = new ItemClass[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				this.previousTechItems[i] = ItemClass.GetItemClass(array[i], false);
			}
		}
		if (this.previousTechItems == null || this.previousTechItems.Length == 0)
		{
			return null;
		}
		if (this.previousTechItems.Length > 1)
		{
			return this.previousTechItems[GameManager.Instance.World.GetGameRandom().RandomRange(this.previousTechItems.Length)];
		}
		return this.previousTechItems[0];
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x001425C8 File Offset: 0x001407C8
	public static ItemClass HandleSandboxTechType(ItemClass itemClass)
	{
		if (itemClass.ItemTechType > ItemClass.MaxTechType)
		{
			ItemClass itemClass2 = itemClass.GetPreviousTechItem();
			while (itemClass2 != null && itemClass2.ItemTechType > ItemClass.MaxTechType)
			{
				itemClass2 = itemClass2.GetPreviousTechItem();
			}
			itemClass = itemClass2;
		}
		return itemClass;
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CanEquip()
	{
		return false;
	}

	// Token: 0x0400260C RID: 9740
	public static readonly FastTags<TagGroup.Global> EatDistractionTag = FastTags<TagGroup.Global>.GetTag("eat");

	// Token: 0x0400260D RID: 9741
	public static readonly FastTags<TagGroup.Global> RequiresContactDistractionTag = FastTags<TagGroup.Global>.GetTag("requires_contact");

	// Token: 0x0400260E RID: 9742
	public static string PropSoundIdle = "SoundIdle";

	// Token: 0x0400260F RID: 9743
	public static string PropSoundDestroy = "SoundDestroy";

	// Token: 0x04002610 RID: 9744
	public static string PropSoundJammed = "SoundJammed";

	// Token: 0x04002611 RID: 9745
	public static string PropSoundHolster = "SoundHolster";

	// Token: 0x04002612 RID: 9746
	public static string PropSoundUnholster = "SoundUnholster";

	// Token: 0x04002613 RID: 9747
	public static string PropSoundStick = "SoundStick";

	// Token: 0x04002614 RID: 9748
	public static string PropSoundTick = "SoundTick";

	// Token: 0x04002615 RID: 9749
	public static string PropSoundPickup = "SoundPickup";

	// Token: 0x04002616 RID: 9750
	public static string PropSoundPlace = "SoundPlace";

	// Token: 0x04002617 RID: 9751
	public static string PropSoundImpactHit = "SoundImpactHit";

	// Token: 0x04002618 RID: 9752
	public static string PropSoundImpactGraze = "SoundImpactGraze";

	// Token: 0x04002619 RID: 9753
	public static string PropHasReloadAnim = "HasReloadAnim";

	// Token: 0x0400261A RID: 9754
	public static string PropFuelValue = "FuelValue";

	// Token: 0x0400261B RID: 9755
	public static string PropWeight = "Weight";

	// Token: 0x0400261C RID: 9756
	public static string PropMoldTarget = "MoldTarget";

	// Token: 0x0400261D RID: 9757
	public static string PropSmell = "Smell";

	// Token: 0x0400261E RID: 9758
	public static string PropLightSource = "LightSource";

	// Token: 0x0400261F RID: 9759
	public static string PropLightValue = "LightValue";

	// Token: 0x04002620 RID: 9760
	public static string PropMatEmission = "MatEmission";

	// Token: 0x04002621 RID: 9761
	public static string PropActivateObject = "ActivateObject";

	// Token: 0x04002622 RID: 9762
	public static string PropThrowableDecoy = "ThrowableDecoy";

	// Token: 0x04002623 RID: 9763
	public static string PropGroupName = "Group";

	// Token: 0x04002624 RID: 9764
	public static string PropCritChance = "CritChance";

	// Token: 0x04002625 RID: 9765
	public static string PropCustomIcon = "CustomIcon";

	// Token: 0x04002626 RID: 9766
	public static string PropCustomIconTint = "CustomIconTint";

	// Token: 0x04002627 RID: 9767
	public static string PropPartType = "PartType";

	// Token: 0x04002628 RID: 9768
	public static string PropImageEffectOnActive = "ImageEffectOnActive";

	// Token: 0x04002629 RID: 9769
	public static string PropPlaySoundOnActive = "PlaySoundOnActive";

	// Token: 0x0400262A RID: 9770
	public static string PropActive = "Active";

	// Token: 0x0400262B RID: 9771
	public static string PropAlwaysActive = "AlwaysActive";

	// Token: 0x0400262C RID: 9772
	public static string PropHoldingItemHidden = "HoldingItemHidden";

	// Token: 0x0400262D RID: 9773
	public static string PropVehicleSlotType = "VehicleSlotType";

	// Token: 0x0400262E RID: 9774
	public static string PropGetQualityFromWeapon = "GetQualityFromWeapon";

	// Token: 0x0400262F RID: 9775
	public static string PropAttributes = "Attributes";

	// Token: 0x04002630 RID: 9776
	public static string PropCraftExpValue = "CraftingIngredientExp";

	// Token: 0x04002631 RID: 9777
	public static string PropCraftTimeValue = "CraftingIngredientTime";

	// Token: 0x04002632 RID: 9778
	public static string PropLootExpValue = "LootExpValue";

	// Token: 0x04002633 RID: 9779
	public static string PropEconomicValue = "EconomicValue";

	// Token: 0x04002634 RID: 9780
	public static string PropEconomicSellScale = "EconomicSellScale";

	// Token: 0x04002635 RID: 9781
	public static string PropEconomicBundleSize = "EconomicBundleSize";

	// Token: 0x04002636 RID: 9782
	public static string PropSellableToTrader = "SellableToTrader";

	// Token: 0x04002637 RID: 9783
	public static string PropCraftingSkillExp = "CraftingSkillExp";

	// Token: 0x04002638 RID: 9784
	public static string PropActionSkillExp = "ActionSkillExp";

	// Token: 0x04002639 RID: 9785
	public static string PropInsulation = "Insulation";

	// Token: 0x0400263A RID: 9786
	public static string PropWaterproof = "Waterproof";

	// Token: 0x0400263B RID: 9787
	public static string PropEncumbrance = "Encumbrance";

	// Token: 0x0400263C RID: 9788
	public static string PropDescriptionKey = "DescriptionKey";

	// Token: 0x0400263D RID: 9789
	public static string PropResourceUnit = "ResourceUnit";

	// Token: 0x0400263E RID: 9790
	public static string PropReplaceResourceUnit = "ReplaceResourceUnit";

	// Token: 0x0400263F RID: 9791
	public static string PropMeltTimePerUnit = "MeltTimePerUnit";

	// Token: 0x04002640 RID: 9792
	public static string PropActionSkillGroup = "ActionSkillGroup";

	// Token: 0x04002641 RID: 9793
	public static string PropCraftingSkillGroup = "CraftingSkillGroup";

	// Token: 0x04002642 RID: 9794
	public static string PropCrosshairOnAim = "CrosshairOnAim";

	// Token: 0x04002643 RID: 9795
	public static string PropCrosshairUpAfterShot = "CrosshairUpAfterShot";

	// Token: 0x04002644 RID: 9796
	public static string PropUsableUnderwater = "UsableUnderwater";

	// Token: 0x04002645 RID: 9797
	public static string PropRepairExpMultiplier = "RepairExpMultiplier";

	// Token: 0x04002646 RID: 9798
	public static string PropTags = "Tags";

	// Token: 0x04002647 RID: 9799
	public static string PropShowQuality = "ShowQuality";

	// Token: 0x04002648 RID: 9800
	public static string PropQualityMin = "QualityMin";

	// Token: 0x04002649 RID: 9801
	public static string PropSoundSightIn = "Sound_Sight_In";

	// Token: 0x0400264A RID: 9802
	public static string PropSoundSightOut = "Sound_Sight_Out";

	// Token: 0x0400264B RID: 9803
	public static string PropIgnoreKeystoneSound = "IgnoreKeystoneSound";

	// Token: 0x0400264C RID: 9804
	public static string PropCreativeMode = "CreativeMode";

	// Token: 0x0400264D RID: 9805
	public static string PropCreativeSort1 = "SortOrder1";

	// Token: 0x0400264E RID: 9806
	public static string PropCreativeSort2 = "SortOrder2";

	// Token: 0x0400264F RID: 9807
	public static string PropDistractionTags = "DistractionTags";

	// Token: 0x04002650 RID: 9808
	public static string PropIsSticky = "IsSticky";

	// Token: 0x04002651 RID: 9809
	public static string PropDisplayType = "DisplayType";

	// Token: 0x04002652 RID: 9810
	public static string PropItemTypeIcon = "ItemTypeIcon";

	// Token: 0x04002653 RID: 9811
	public static string PropAltItemTypeIcon = "AltItemTypeIcon";

	// Token: 0x04002654 RID: 9812
	public static string PropAltItemTypeIconColor = "AltItemTypeIconColor";

	// Token: 0x04002655 RID: 9813
	public static string PropUnlockedBy = "UnlockedBy";

	// Token: 0x04002656 RID: 9814
	public static string PropUnlocks = "Unlocks";

	// Token: 0x04002657 RID: 9815
	public static string PropNoScrapping = "NoScrapping";

	// Token: 0x04002658 RID: 9816
	public static string PropScrapTimeOverride = "ScrapTimeOverride";

	// Token: 0x04002659 RID: 9817
	public static string PropNavObject = "NavObject";

	// Token: 0x0400265A RID: 9818
	public static string PropQuestItem = "IsQuestItem";

	// Token: 0x0400265B RID: 9819
	public static string PropTrackerIndexName = "TrackerIndexName";

	// Token: 0x0400265C RID: 9820
	public static string PropTrackerNavObject = "TrackerNavObject";

	// Token: 0x0400265D RID: 9821
	public static string PropTraderStageTemplate = "TraderStageTemplate";

	// Token: 0x0400265E RID: 9822
	public static string PropMaxModsAllowed = "MaxModsAllowed";

	// Token: 0x0400265F RID: 9823
	public static string PropRestrictedMove = "RestrictedMove";

	// Token: 0x04002660 RID: 9824
	public static string PropPreviousItemType = "PreviousTechItem";

	// Token: 0x04002661 RID: 9825
	public static int MAX_ITEMS = Block.MAX_BLOCKS + 16384;

	// Token: 0x04002662 RID: 9826
	public static NameIdMapping nameIdMapping;

	// Token: 0x04002663 RID: 9827
	public static byte[] fullMappingDataForClients;

	// Token: 0x04002664 RID: 9828
	public static ItemClass[] list;

	// Token: 0x04002665 RID: 9829
	public static string[] itemActionNames;

	// Token: 0x04002666 RID: 9830
	public const int cMaxActionNames = 5;

	// Token: 0x04002667 RID: 9831
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Dictionary<string, ItemClass> nameToItem = new Dictionary<string, ItemClass>();

	// Token: 0x04002668 RID: 9832
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Dictionary<string, ItemClass> nameToItemCaseInsensitive = new CaseInsensitiveStringDictionary<ItemClass>();

	// Token: 0x04002669 RID: 9833
	[PublicizedFrom(EAccessModifier.Protected)]
	public static List<string> itemNames = new List<string>();

	// Token: 0x0400266A RID: 9834
	public static readonly ReadOnlyCollection<string> ItemNames = new ReadOnlyCollection<string>(ItemClass.itemNames);

	// Token: 0x0400266B RID: 9835
	public DynamicProperties Properties;

	// Token: 0x0400266C RID: 9836
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x0400266D RID: 9837
	public float Smell;

	// Token: 0x0400266E RID: 9838
	public string MeshFile;

	// Token: 0x0400266F RID: 9839
	public string DropMeshFile;

	// Token: 0x04002670 RID: 9840
	public string HandMeshFile;

	// Token: 0x04002671 RID: 9841
	public string StickyMaterial;

	// Token: 0x04002672 RID: 9842
	public float StickyOffset = 0.15f;

	// Token: 0x04002673 RID: 9843
	public int StickyColliderUp = -1;

	// Token: 0x04002674 RID: 9844
	public float StickyColliderRadius = 0.2f;

	// Token: 0x04002675 RID: 9845
	public float StickyColliderLength = -1f;

	// Token: 0x04002676 RID: 9846
	public bool IsSticky;

	// Token: 0x04002677 RID: 9847
	public const int cActionUpdateCount = 3;

	// Token: 0x04002678 RID: 9848
	public ItemAction[] Actions;

	// Token: 0x04002679 RID: 9849
	public MaterialBlock MadeOfMaterial;

	// Token: 0x0400267A RID: 9850
	public DataItem<int> HoldType = new DataItem<int>(0);

	// Token: 0x0400267B RID: 9851
	public DataItem<int> Stacknumber = new DataItem<int>(500);

	// Token: 0x0400267C RID: 9852
	public DataItem<int> MaxUseTimes = new DataItem<int>(0);

	// Token: 0x0400267D RID: 9853
	public DataItem<bool> MaxUseTimesBreaksAfter = new DataItem<bool>(false);

	// Token: 0x0400267E RID: 9854
	public ItemClass MoldTarget;

	// Token: 0x0400267F RID: 9855
	public DataItem<string> LightSource;

	// Token: 0x04002680 RID: 9856
	public DataItem<string> ActivateObject;

	// Token: 0x04002681 RID: 9857
	public DataItem<bool> ThrowableDecoy;

	// Token: 0x04002682 RID: 9858
	public ItemData.DataItemArrayRepairTools RepairTools;

	// Token: 0x04002683 RID: 9859
	public DataItem<int> RepairAmount;

	// Token: 0x04002684 RID: 9860
	public DataItem<float> RepairTime;

	// Token: 0x04002685 RID: 9861
	public DataItem<float> CritChance;

	// Token: 0x04002686 RID: 9862
	public string[] Groups = new string[] { "Decor/Miscellaneous" };

	// Token: 0x04002687 RID: 9863
	public DataItem<string> CustomIcon;

	// Token: 0x04002688 RID: 9864
	public Color CustomIconTint;

	// Token: 0x04002689 RID: 9865
	public DataItem<bool> UserHidden = new DataItem<bool>(false);

	// Token: 0x0400268A RID: 9866
	public string VehicleSlotType;

	// Token: 0x0400268B RID: 9867
	public bool GetQualityFromWeapon;

	// Token: 0x0400268C RID: 9868
	public string DescriptionKey;

	// Token: 0x0400268D RID: 9869
	public bool IsResourceUnit;

	// Token: 0x0400268E RID: 9870
	public string ReplaceResourceUnit = "";

	// Token: 0x0400268F RID: 9871
	public float MeltTimePerUnit;

	// Token: 0x04002690 RID: 9872
	public string ActionSkillGroup = "";

	// Token: 0x04002691 RID: 9873
	public string CraftingSkillGroup = "";

	// Token: 0x04002692 RID: 9874
	public bool UsableUnderwater = true;

	// Token: 0x04002693 RID: 9875
	public float lightValue;

	// Token: 0x04002694 RID: 9876
	public string soundSightIn = "";

	// Token: 0x04002695 RID: 9877
	public string soundSightOut = "";

	// Token: 0x04002696 RID: 9878
	public bool ignoreKeystoneSound;

	// Token: 0x04002697 RID: 9879
	public bool HoldingItemHidden;

	// Token: 0x04002698 RID: 9880
	public List<int> PartParentId;

	// Token: 0x04002699 RID: 9881
	public PreviewData Preview;

	// Token: 0x0400269A RID: 9882
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameObject renderGameObject;

	// Token: 0x0400269B RID: 9883
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bCanHold;

	// Token: 0x0400269C RID: 9884
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bCanDrop;

	// Token: 0x0400269D RID: 9885
	public bool HasSubItems;

	// Token: 0x0400269E RID: 9886
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] attachments;

	// Token: 0x0400269F RID: 9887
	public bool bCraftingTool;

	// Token: 0x040026A0 RID: 9888
	public float CraftComponentExp = 3f;

	// Token: 0x040026A1 RID: 9889
	public float CraftComponentTime = 1f;

	// Token: 0x040026A2 RID: 9890
	public float LootExp = 1f;

	// Token: 0x040026A3 RID: 9891
	public float EconomicValue;

	// Token: 0x040026A4 RID: 9892
	public float EconomicSellScale = 1f;

	// Token: 0x040026A5 RID: 9893
	public int EconomicBundleSize = 1;

	// Token: 0x040026A6 RID: 9894
	public bool SellableToTrader = true;

	// Token: 0x040026A7 RID: 9895
	public string TraderStageTemplate;

	// Token: 0x040026A8 RID: 9896
	public int MaxModsAllowed = 1;

	// Token: 0x040026A9 RID: 9897
	public int CraftingSkillExp = 10;

	// Token: 0x040026AA RID: 9898
	public int ActionSkillExp = 10;

	// Token: 0x040026AB RID: 9899
	public float RepairExpMultiplier = 10f;

	// Token: 0x040026AC RID: 9900
	public float Insulation;

	// Token: 0x040026AD RID: 9901
	public float WaterProof;

	// Token: 0x040026AE RID: 9902
	public float Encumbrance;

	// Token: 0x040026AF RID: 9903
	public string SoundUnholster = "generic_unholster";

	// Token: 0x040026B0 RID: 9904
	public string SoundHolster = "generic_holster";

	// Token: 0x040026B1 RID: 9905
	public string SoundStick;

	// Token: 0x040026B2 RID: 9906
	public string SoundTick;

	// Token: 0x040026B3 RID: 9907
	public float SoundTickDelay = 1f;

	// Token: 0x040026B4 RID: 9908
	public string SoundPickup = "craft_take_item";

	// Token: 0x040026B5 RID: 9909
	public string SoundPlace = "craft_place_item";

	// Token: 0x040026B6 RID: 9910
	public string SoundImpactHit;

	// Token: 0x040026B7 RID: 9911
	public string SoundImpactGraze;

	// Token: 0x040026B8 RID: 9912
	public bool HasReloadAnim = true;

	// Token: 0x040026B9 RID: 9913
	public bool bShowCrosshairOnAiming;

	// Token: 0x040026BA RID: 9914
	public bool bCrosshairUpAfterShot;

	// Token: 0x040026BB RID: 9915
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bRestrictedMove;

	// Token: 0x040026BC RID: 9916
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ItemStack.StackLocationTypes> restrictedTo = new List<XUiC_ItemStack.StackLocationTypes>();

	// Token: 0x040026BD RID: 9917
	public ItemClass.ItemTechTypes ItemTechType;

	// Token: 0x040026BE RID: 9918
	[PublicizedFrom(EAccessModifier.Private)]
	public string previousTechTypeName = "";

	// Token: 0x040026BF RID: 9919
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] previousTechItems;

	// Token: 0x040026C0 RID: 9920
	public static ItemClass.ItemTechTypes MaxTechType = ItemClass.ItemTechTypes.T3;

	// Token: 0x040026C1 RID: 9921
	public bool IsLimited;

	// Token: 0x040026C2 RID: 9922
	public EnumCreativeMode CreativeMode;

	// Token: 0x040026C3 RID: 9923
	public string SortOrder;

	// Token: 0x040026C4 RID: 9924
	public FastTags<TagGroup.Global> ItemTags;

	// Token: 0x040026C5 RID: 9925
	public MinEffectController Effects;

	// Token: 0x040026C6 RID: 9926
	public FastTags<TagGroup.Global> DistractionTags;

	// Token: 0x040026C7 RID: 9927
	public SDCSUtils.SlotData SDCSData;

	// Token: 0x040026C8 RID: 9928
	public string DisplayType;

	// Token: 0x040026C9 RID: 9929
	public string ItemTypeIcon = "";

	// Token: 0x040026CA RID: 9930
	public string AltItemTypeIcon;

	// Token: 0x040026CB RID: 9931
	public Color AltItemTypeIconColor;

	// Token: 0x040026CC RID: 9932
	public bool ShowQualityBar;

	// Token: 0x040026CD RID: 9933
	public byte QualityMin;

	// Token: 0x040026CE RID: 9934
	public bool NoScrapping;

	// Token: 0x040026CF RID: 9935
	public float ScrapTimeOverride;

	// Token: 0x040026D0 RID: 9936
	public string Unlocks = "";

	// Token: 0x040026D1 RID: 9937
	public string NavObject = "";

	// Token: 0x040026D2 RID: 9938
	public bool IsQuestItem;

	// Token: 0x040026D3 RID: 9939
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> stopBleed = FastTags<TagGroup.Global>.Parse("stopsBleeding");

	// Token: 0x040026D4 RID: 9940
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T0Tag = FastTags<TagGroup.Global>.Parse("T0");

	// Token: 0x040026D5 RID: 9941
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T1Tag = FastTags<TagGroup.Global>.Parse("T1");

	// Token: 0x040026D6 RID: 9942
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T2Tag = FastTags<TagGroup.Global>.Parse("T2");

	// Token: 0x040026D7 RID: 9943
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T3Tag = FastTags<TagGroup.Global>.Parse("T3");

	// Token: 0x040026D8 RID: 9944
	public string TrackerIndexName;

	// Token: 0x040026D9 RID: 9945
	public string TrackerNavObject;

	// Token: 0x040026DA RID: 9946
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData[] unlockedBy;

	// Token: 0x040026DB RID: 9947
	public float TraderQualityMaxMod;

	// Token: 0x040026DC RID: 9948
	public float TraderQualityMinMod;

	// Token: 0x040026DD RID: 9949
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cFilterQualityPrefix = "#";

	// Token: 0x040026DE RID: 9950
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] ActionProfilerNames = new string[] { "action0", "action1", "action2" };

	// Token: 0x040026DF RID: 9951
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, int> fixedItemIds = new Dictionary<string, int>();

	// Token: 0x040026E0 RID: 9952
	public const float cGSStatScale = 0.005f;

	// Token: 0x040026E1 RID: 9953
	public const float cGSStatMax = 163.83499f;

	// Token: 0x040026E2 RID: 9954
	public Dictionary<PassiveEffects, List<ItemClass.GSStat>> Stats;

	// Token: 0x020005EA RID: 1514
	public enum EnumCrosshairType
	{
		// Token: 0x040026E4 RID: 9956
		None,
		// Token: 0x040026E5 RID: 9957
		Plus,
		// Token: 0x040026E6 RID: 9958
		Crosshair,
		// Token: 0x040026E7 RID: 9959
		CrosshairOnAiming,
		// Token: 0x040026E8 RID: 9960
		Damage,
		// Token: 0x040026E9 RID: 9961
		Upgrade,
		// Token: 0x040026EA RID: 9962
		Repair,
		// Token: 0x040026EB RID: 9963
		PowerSource,
		// Token: 0x040026EC RID: 9964
		Heal,
		// Token: 0x040026ED RID: 9965
		PowerItem,
		// Token: 0x040026EE RID: 9966
		Blocked,
		// Token: 0x040026EF RID: 9967
		Dot
	}

	// Token: 0x020005EB RID: 1515
	public enum ItemTechTypes
	{
		// Token: 0x040026F1 RID: 9969
		None,
		// Token: 0x040026F2 RID: 9970
		T0,
		// Token: 0x040026F3 RID: 9971
		T1,
		// Token: 0x040026F4 RID: 9972
		T2,
		// Token: 0x040026F5 RID: 9973
		T3
	}

	// Token: 0x020005EC RID: 1516
	// (Invoke) Token: 0x06003189 RID: 12681
	public delegate bool FilterItem(ItemClass _class, Block _block);

	// Token: 0x020005ED RID: 1517
	public struct GSStat
	{
		// Token: 0x040026F6 RID: 9974
		public short quality;

		// Token: 0x040026F7 RID: 9975
		public short gameStage;

		// Token: 0x040026F8 RID: 9976
		public float chance;

		// Token: 0x040026F9 RID: 9977
		public short min;

		// Token: 0x040026FA RID: 9978
		public short max;
	}
}
