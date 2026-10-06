using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Linq;
using AssetManagement;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;
using XMLData.Item;

// Token: 0x020005EF RID: 1519
[Preserve]
public class ItemClass : ItemData
{
	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06003185 RID: 12677 RVA: 0x001408B8 File Offset: 0x0013EAB8
	public string[] Attachments
	{
		get
		{
			return this.attachments;
		}
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06003186 RID: 12678 RVA: 0x001408C0 File Offset: 0x0013EAC0
	public int MaxCount
	{
		get
		{
			if (ItemClass.MaxStackSizeModifier == 1f || this.HasQuality || !this.CanStack())
			{
				return this.Stacknumber.Value;
			}
			return Utils.FastMin(Utils.FastRoundToInt((float)this.Stacknumber.Value * ItemClass.MaxStackSizeModifier), 30000);
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06003187 RID: 12679 RVA: 0x00140916 File Offset: 0x0013EB16
	public bool HasQuality
	{
		get
		{
			return this.Effects != null && this.Effects.IsOwnerTiered();
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06003188 RID: 12680 RVA: 0x0014092D File Offset: 0x0013EB2D
	public virtual bool IsLimitedItem
	{
		get
		{
			return this.IsLimited;
		}
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x00140938 File Offset: 0x0013EB38
	public virtual bool CanMoveToLocation(XUiC_ItemStack.StackLocationTypes locationType, int slotNumber)
	{
		bool flag = true;
		if (slotNumber >= 0)
		{
			flag = this.CanMoveToSlot(locationType, slotNumber);
		}
		bool flag2 = true;
		if (this.bRestrictedMove)
		{
			flag2 = false;
			using (List<XUiC_ItemStack.StackLocationTypes>.Enumerator enumerator = this.restrictedTo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == locationType)
					{
						flag2 = true;
						break;
					}
				}
			}
		}
		return flag2 && flag;
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanMoveToSlot(XUiC_ItemStack.StackLocationTypes locationType, int slot)
	{
		return true;
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x0600318B RID: 12683 RVA: 0x001409A8 File Offset: 0x0013EBA8
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

	// Token: 0x0600318C RID: 12684 RVA: 0x00140A38 File Offset: 0x0013EC38
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

	// Token: 0x0600318D RID: 12685 RVA: 0x00140C75 File Offset: 0x0013EE75
	public void SetId(int _id)
	{
		this.pId = _id;
		if (this.Effects != null)
		{
			this.Effects.ParentPointer = _id;
		}
	}

	// Token: 0x0600318E RID: 12686 RVA: 0x00140C98 File Offset: 0x0013EE98
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

	// Token: 0x0600318F RID: 12687 RVA: 0x00141D20 File Offset: 0x0013FF20
	public void LateInit()
	{
		if (this.HasQuality)
		{
			this.Stacknumber.Value = 1;
		}
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x00141D38 File Offset: 0x0013FF38
	public static void InitStatic()
	{
		ItemClass.list = new ItemClass[ItemClass.MAX_ITEMS];
		ItemClass.itemActionNames = new string[5];
		for (int i = 0; i < 5; i++)
		{
			ItemClass.itemActionNames[i] = "Action" + i.ToString();
		}
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x00141D84 File Offset: 0x0013FF84
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

	// Token: 0x06003192 RID: 12690 RVA: 0x00141DB6 File Offset: 0x0013FFB6
	public static void Cleanup()
	{
		ItemClass.list = null;
		ItemClass.nameToItem.Clear();
		ItemClass.nameToItemCaseInsensitive.Clear();
		ItemClass.itemNames.Clear();
		ItemClass.itemActionNames = null;
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x00141DE2 File Offset: 0x0013FFE2
	public virtual int GetInitialMetadata(ItemValue _itemValue)
	{
		if (this.Actions[0] == null)
		{
			return 0;
		}
		return this.Actions[0].GetInitialMeta(_itemValue);
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x00141DFE File Offset: 0x0013FFFE
	public static ItemClass GetForId(int _id)
	{
		if (ItemClass.list == null || (ulong)_id >= (ulong)((long)ItemClass.list.Length))
		{
			return null;
		}
		return ItemClass.list[_id];
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x00141E1C File Offset: 0x0014001C
	public static ItemValue GetItem(string _itemName, bool _caseInsensitive = false)
	{
		ItemClass itemClass = ItemClass.GetItemClass(_itemName, _caseInsensitive);
		if (itemClass != null)
		{
			return new ItemValue(itemClass.Id, false);
		}
		return ItemValue.None;
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x00141E48 File Offset: 0x00140048
	public static ItemValue CreateItemValue(string _itemName, int _quality, bool _caseInsensitive = false)
	{
		ItemClass itemClass = ItemClass.GetItemClass(_itemName, _caseInsensitive);
		if (itemClass != null)
		{
			return new ItemValue(itemClass.Id, _quality, _quality, false, null, 1f);
		}
		return ItemValue.None;
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x00141E7C File Offset: 0x0014007C
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

	// Token: 0x06003198 RID: 12696 RVA: 0x00141EAC File Offset: 0x001400AC
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

	// Token: 0x06003199 RID: 12697 RVA: 0x00142060 File Offset: 0x00140260
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
			ItemStack itemStack = new ItemStack(itemValue, itemClass.MaxCount);
			_targetList.Add(itemStack);
		}
		GameRandomManager.Instance.FreeGameRandom(gameRandom);
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x0014216C File Offset: 0x0014036C
	public virtual bool IsHUDDisabled(ItemInventoryData _data)
	{
		return (this.Actions[0] != null && this.Actions[0].IsHUDDisabled((_data != null) ? _data.actionData[0] : null)) || (this.Actions[1] != null && this.Actions[1].IsHUDDisabled((_data != null) ? _data.actionData[1] : null));
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x001421D4 File Offset: 0x001403D4
	public virtual bool IsLightSource()
	{
		return this.LightSource != null;
	}

	// Token: 0x0600319C RID: 12700 RVA: 0x001421E4 File Offset: 0x001403E4
	public virtual InstanceHandle<GameObject> CloneModel(ItemValue _itemValue, Transform _parent, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
		AssetHandle<GameObject> modelHandle = this.GetModelHandle(_purpose, _itemValue);
		if (modelHandle.Asset == null)
		{
			return default(InstanceHandle<GameObject>);
		}
		InstanceHandle<GameObject> instanceHandle = modelHandle.CreateInstance();
		modelHandle.Release();
		if (!this.InitializeModel(instanceHandle.Instance, _itemValue, _parent, _purpose))
		{
			instanceHandle.Destroy();
			return default(InstanceHandle<GameObject>);
		}
		return instanceHandle;
	}

	// Token: 0x0600319D RID: 12701 RVA: 0x00142244 File Offset: 0x00140444
	[PublicizedFrom(EAccessModifier.Private)]
	public AssetHandle<GameObject> GetModelHandle(BlockShape.MeshPurpose _purpose, ItemValue _itemValue)
	{
		string meshfilePath = this.GetMeshfilePath(_purpose, _itemValue);
		if (!string.IsNullOrEmpty(meshfilePath))
		{
			AssetHandle<GameObject> assetHandle = AssetManagers.Items.AcquireAsset<GameObject>(meshfilePath, false);
			if (!assetHandle.IsValid() || assetHandle.Asset == null)
			{
				assetHandle = AssetManagers.Items.AcquireAsset<GameObject>("@:Other/Items/Crafting/leather.fbx", false);
			}
			return assetHandle;
		}
		return null;
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x0014229C File Offset: 0x0014049C
	public GameObject CreateModelInstance(ItemValue _itemValue, Transform _parent, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(DataLoader.LoadAsset<GameObject>(this.GetMeshfilePath(_purpose, _itemValue), false));
		if (!this.InitializeModel(gameObject, _itemValue, _parent, _purpose))
		{
			return null;
		}
		return gameObject;
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x001422CC File Offset: 0x001404CC
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetMeshfilePath(BlockShape.MeshPurpose _purpose, ItemValue _itemValue)
	{
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
			return text;
		}
		return null;
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x00142328 File Offset: 0x00140528
	[PublicizedFrom(EAccessModifier.Private)]
	public bool InitializeModel(GameObject _instance, ItemValue _itemValue, Transform _parent, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
		try
		{
			Transform transform = _instance.transform;
			transform.SetParent(_parent, false);
			transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			_instance.SetActive(false);
			if (_purpose == BlockShape.MeshPurpose.Hold)
			{
				Collider[] componentsInChildren = _instance.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				if (_instance != null)
				{
					Animator[] componentsInChildren2 = _instance.GetComponentsInChildren<Animator>();
					for (int i = 0; i < componentsInChildren2.Length; i++)
					{
						componentsInChildren2[i].writeDefaultValuesOnDisable = true;
					}
				}
			}
			UpdateLightOnAllMaterials updateLightOnAllMaterials = _instance.GetComponent<UpdateLightOnAllMaterials>();
			if (updateLightOnAllMaterials == null)
			{
				updateLightOnAllMaterials = _instance.AddComponent<UpdateLightOnAllMaterials>();
			}
			string text = "255,255,255";
			this.Properties.ParseString(Block.PropTintColor, ref text);
			Vector3 vector = Block.StringToVector3(_itemValue.GetPropertyOverride(Block.PropTintColor, text));
			updateLightOnAllMaterials.SetTintColorForItem(vector);
			return true;
		}
		catch (Exception ex)
		{
			Log.Error("Instantiate of '" + this.MeshFile + "' led to error: " + ex.Message);
			Log.Error(ex.StackTrace);
		}
		return false;
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x00142444 File Offset: 0x00140644
	public void setLocalizedItemName(string _localizedName)
	{
		this.localizedName = _localizedName;
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x0014244D File Offset: 0x0014064D
	public virtual string GetLocalizedItemName()
	{
		return this.localizedName;
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x00142455 File Offset: 0x00140655
	public void SetName(string _name)
	{
		this.pName = _name;
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x0014245E File Offset: 0x0014065E
	public virtual string GetItemName()
	{
		return base.Name;
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x00142466 File Offset: 0x00140666
	public virtual string GetItemDescriptionKey()
	{
		return this.DescriptionKey;
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x0014246E File Offset: 0x0014066E
	public virtual string GetIconName()
	{
		if (this.CustomIcon != null && this.CustomIcon.Value.Length > 0)
		{
			return this.CustomIcon.Value;
		}
		return base.Name;
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x001424A4 File Offset: 0x001406A4
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

	// Token: 0x060031A8 RID: 12712 RVA: 0x001424DD File Offset: 0x001406DD
	public virtual bool IsGun()
	{
		return this.Actions[0] is ItemActionAttack;
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x001424EF File Offset: 0x001406EF
	public virtual bool IsDynamicMelee()
	{
		return this.Actions[0] is ItemActionDynamic;
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x00142501 File Offset: 0x00140701
	public virtual bool CanStack()
	{
		return this.Stacknumber.Value > 1;
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x00142511 File Offset: 0x00140711
	public void SetCanHold(bool _b)
	{
		this.bCanHold = _b;
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x0014251A File Offset: 0x0014071A
	public virtual bool CanHold()
	{
		return this.bCanHold;
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x00142522 File Offset: 0x00140722
	public void SetCanDrop(bool _b)
	{
		this.bCanDrop = _b;
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x0014252B File Offset: 0x0014072B
	public virtual bool CanDrop(ItemValue _iv = null)
	{
		return this.bCanDrop;
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x00142533 File Offset: 0x00140733
	public virtual bool RestrictedMove(ItemValue _iv = null)
	{
		return this.bRestrictedMove;
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Deactivate(ItemValue _iv)
	{
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool KeepOnDeath()
	{
		return false;
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanPlaceInContainer()
	{
		return true;
	}

	// Token: 0x060031B3 RID: 12723 RVA: 0x0014253C File Offset: 0x0014073C
	public virtual string CanInteract(ItemInventoryData _data)
	{
		ItemAction itemAction = this.Actions[2];
		if (itemAction == null)
		{
			return null;
		}
		return itemAction.CanInteract(_data.actionData[2]);
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x00142569 File Offset: 0x00140769
	public void Interact(ItemInventoryData _data)
	{
		this.ExecuteAction(2, _data, false, null);
		this.ExecuteAction(2, _data, true, null);
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x00142580 File Offset: 0x00140780
	public bool CanExecuteAction(int actionIdx, EntityAlive holdingEntity, ItemValue itemValue)
	{
		if (this.Actions[actionIdx] == null || this.Actions[actionIdx].ExecutionRequirements == null)
		{
			return true;
		}
		holdingEntity.MinEventContext.ItemValue = itemValue;
		return this.Actions[actionIdx].ExecutionRequirements.IsValid(holdingEntity.MinEventContext);
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x001425CC File Offset: 0x001407CC
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

	// Token: 0x060031B7 RID: 12727 RVA: 0x00142B20 File Offset: 0x00140D20
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

	// Token: 0x060031B8 RID: 12728 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingItemActivated(ItemInventoryData _data)
	{
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnPlacedAsCatalyst(ItemInventoryData _data)
	{
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x00142B5C File Offset: 0x00140D5C
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

	// Token: 0x060031BB RID: 12731 RVA: 0x00142BB0 File Offset: 0x00140DB0
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

	// Token: 0x060031BC RID: 12732 RVA: 0x00142BEC File Offset: 0x00140DEC
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

	// Token: 0x060031BD RID: 12733 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingReset(ItemInventoryData _data)
	{
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x00142D14 File Offset: 0x00140F14
	public void StopHoldingAudio(ItemInventoryData _data)
	{
		if (this.Properties.GetValue(ItemClass.PropSoundIdle) != null && _data.holdingEntitySoundID == 0)
		{
			Manager.BroadcastStop(_data.holdingEntity.entityId, this.Properties.GetValue(ItemClass.PropSoundIdle));
		}
		_data.holdingEntitySoundID = -2;
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x00142D64 File Offset: 0x00140F64
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

	// Token: 0x060031C0 RID: 12736 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnMeshCreated(ItemWorldData _data)
	{
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnDroppedUpdate(ItemWorldData _data)
	{
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x0004E4F3 File Offset: 0x0004C6F3
	public virtual BlockValue OnConvertToBlockValue(ItemValue _itemValue, BlockValue _blueprintBlockValue)
	{
		return _blueprintBlockValue;
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x00142DD5 File Offset: 0x00140FD5
	public virtual ItemWorldData CreateWorldData(IGameManager _gm, EntityItem _entityItem, ItemValue _itemValue, int _belongsEntityId)
	{
		return new ItemWorldData(_gm, _itemValue, _entityItem, _belongsEntityId);
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x00142DE4 File Offset: 0x00140FE4
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

	// Token: 0x060031C5 RID: 12741 RVA: 0x00142E3C File Offset: 0x0014103C
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

	// Token: 0x060031C6 RID: 12742 RVA: 0x00142E90 File Offset: 0x00141090
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

	// Token: 0x060031C7 RID: 12743 RVA: 0x00142F06 File Offset: 0x00141106
	public virtual float GetFocusRange()
	{
		if (this.Actions[0] != null && this.Actions[0] is ItemActionAttack)
		{
			return ((ItemActionAttack)this.Actions[0]).Range;
		}
		return 0f;
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x00142F3C File Offset: 0x0014113C
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

	// Token: 0x060031C9 RID: 12745 RVA: 0x00142F80 File Offset: 0x00141180
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

	// Token: 0x060031CA RID: 12746 RVA: 0x00142FE0 File Offset: 0x001411E0
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

	// Token: 0x060031CB RID: 12747 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CheckKeys(ItemInventoryData _data, WorldRayHitInfo _hitInfo)
	{
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x0014303B File Offset: 0x0014123B
	public virtual float GetLifetimeOnDrop()
	{
		return 60f;
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual Block GetBlock()
	{
		return null;
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsBlock()
	{
		return false;
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsProp()
	{
		return false;
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x00143044 File Offset: 0x00141244
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

	// Token: 0x060031D1 RID: 12753 RVA: 0x001430E4 File Offset: 0x001412E4
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

	// Token: 0x060031D2 RID: 12754 RVA: 0x0014315C File Offset: 0x0014135C
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

	// Token: 0x060031D3 RID: 12755 RVA: 0x001431BC File Offset: 0x001413BC
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

	// Token: 0x060031D4 RID: 12756 RVA: 0x001431F3 File Offset: 0x001413F3
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

	// Token: 0x060031D5 RID: 12757 RVA: 0x0014322A File Offset: 0x0014142A
	public virtual bool IsActivated(ItemValue _value)
	{
		return _value.Activated;
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x00143232 File Offset: 0x00141432
	public virtual void SetActivated(ref ItemValue _value, bool _activated)
	{
		_value.Activated = _activated;
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x0014323C File Offset: 0x0014143C
	public virtual Vector3 GetDroppedCorrectionRotation()
	{
		return new Vector3(-90f, 0f, 0f);
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x0004E492 File Offset: 0x0004C692
	public virtual Vector3 GetCorrectionRotation()
	{
		return Vector3.zero;
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x0004E492 File Offset: 0x0004C692
	public virtual Vector3 GetCorrectionPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x0004E492 File Offset: 0x0004C692
	public virtual Vector3 GetCorrectionScale()
	{
		return Vector3.zero;
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnDamagedByExplosion(ItemWorldData _data)
	{
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x00143254 File Offset: 0x00141454
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

	// Token: 0x060031DD RID: 12765 RVA: 0x001432A2 File Offset: 0x001414A2
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

	// Token: 0x060031DE RID: 12766 RVA: 0x001432CD File Offset: 0x001414CD
	public string GetImageEffect()
	{
		if (base.ImageEffectOnActive != null)
		{
			return base.ImageEffectOnActive.Value;
		}
		return "";
	}

	// Token: 0x060031DF RID: 12767 RVA: 0x001432EE File Offset: 0x001414EE
	public bool GetActive()
	{
		return base.Active != null && base.Active.Value;
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x00143305 File Offset: 0x00141505
	public string GetSoundOnActive()
	{
		if (base.PlaySoundOnActive != null)
		{
			return base.PlaySoundOnActive.Value;
		}
		return "";
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x00143326 File Offset: 0x00141526
	public void SetWeight(int _w)
	{
		if (this.IsBlock())
		{
			this.GetBlock().Weight = new DataItem<int>(_w);
			return;
		}
		base.Weight = new DataItem<int>(_w);
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x0014334E File Offset: 0x0014154E
	public void SetImageEffect(string _str)
	{
		base.ImageEffectOnActive = new DataItem<string>(_str);
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x0014335C File Offset: 0x0014155C
	public void SetActive(bool _bOn)
	{
		base.Active = new DataItem<bool>(_bOn);
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x0014336A File Offset: 0x0014156A
	public void SetSoundOnActive(string _str)
	{
		base.PlaySoundOnActive = new DataItem<string>(_str);
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x00143378 File Offset: 0x00141578
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

	// Token: 0x060031E6 RID: 12774 RVA: 0x0014359B File Offset: 0x0014179B
	public virtual bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ItemTags.Test_AnySet(_tags);
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x001435A9 File Offset: 0x001417A9
	public virtual bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.ItemTags.Test_AllSet(_tags);
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x001435B8 File Offset: 0x001417B8
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

	// Token: 0x060031E9 RID: 12777 RVA: 0x00143600 File Offset: 0x00141800
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

	// Token: 0x060031EA RID: 12778 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanCollect(ItemValue _iv)
	{
		return true;
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x00143654 File Offset: 0x00141854
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

	// Token: 0x060031EC RID: 12780 RVA: 0x001437E4 File Offset: 0x001419E4
	public void FireEvent(MinEventTypes _eventType, MinEventParams _eventParms)
	{
		if (this.Effects != null)
		{
			this.Effects.FireEvent(_eventType, _eventParms);
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x060031ED RID: 12781 RVA: 0x001437FB File Offset: 0x001419FB
	public bool IsEatDistraction
	{
		get
		{
			return this.DistractionTags.Test_AnySet(ItemClass.EatDistractionTag);
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x060031EE RID: 12782 RVA: 0x0014380D File Offset: 0x00141A0D
	public bool IsRequireContactDistraction
	{
		get
		{
			return this.DistractionTags.Test_AnySet(ItemClass.RequiresContactDistractionTag);
		}
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x0014381F File Offset: 0x00141A1F
	public bool HasTrigger(MinEventTypes _eventType)
	{
		return this.Effects != null && this.Effects.HasTrigger(_eventType);
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x00143838 File Offset: 0x00141A38
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

	// Token: 0x060031F1 RID: 12785 RVA: 0x001438B4 File Offset: 0x00141AB4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void assignIdsLinear()
	{
		Log.Out("ItemIDs linear");
		bool[] array = new bool[ItemClass.MAX_ITEMS];
		List<ItemClass> list = new List<ItemClass>(ItemClass.nameToItem.Count);
		ItemClass.nameToItem.CopyValuesTo<string, ItemClass>(list);
		ItemClass.assignLeftOverItems(array, list);
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x001438F6 File Offset: 0x00141AF6
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

	// Token: 0x060031F3 RID: 12787 RVA: 0x00143910 File Offset: 0x00141B10
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

	// Token: 0x060031F4 RID: 12788 RVA: 0x00143A24 File Offset: 0x00141C24
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

	// Token: 0x060031F5 RID: 12789 RVA: 0x00143AD8 File Offset: 0x00141CD8
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

	// Token: 0x060031F6 RID: 12790 RVA: 0x00143B54 File Offset: 0x00141D54
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

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x060031F7 RID: 12791 RVA: 0x00143B82 File Offset: 0x00141D82
	public static ItemClass MissingItem
	{
		get
		{
			return ItemClass.GetItemClass("missingItem", false);
		}
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x00143B90 File Offset: 0x00141D90
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

	// Token: 0x060031F9 RID: 12793 RVA: 0x00143DD0 File Offset: 0x00141FD0
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

	// Token: 0x060031FA RID: 12794 RVA: 0x00143EE0 File Offset: 0x001420E0
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

	// Token: 0x060031FB RID: 12795 RVA: 0x00143FA8 File Offset: 0x001421A8
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

	// Token: 0x060031FC RID: 12796 RVA: 0x00144034 File Offset: 0x00142234
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

	// Token: 0x060031FD RID: 12797 RVA: 0x001440E4 File Offset: 0x001422E4
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

	// Token: 0x060031FE RID: 12798 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CanEquip()
	{
		return false;
	}

	// Token: 0x0400263A RID: 9786
	public static readonly FastTags<TagGroup.Global> EatDistractionTag = FastTags<TagGroup.Global>.GetTag("eat");

	// Token: 0x0400263B RID: 9787
	public static readonly FastTags<TagGroup.Global> RequiresContactDistractionTag = FastTags<TagGroup.Global>.GetTag("requires_contact");

	// Token: 0x0400263C RID: 9788
	public static string PropSoundIdle = "SoundIdle";

	// Token: 0x0400263D RID: 9789
	public static string PropSoundDestroy = "SoundDestroy";

	// Token: 0x0400263E RID: 9790
	public static string PropSoundJammed = "SoundJammed";

	// Token: 0x0400263F RID: 9791
	public static string PropSoundHolster = "SoundHolster";

	// Token: 0x04002640 RID: 9792
	public static string PropSoundUnholster = "SoundUnholster";

	// Token: 0x04002641 RID: 9793
	public static string PropSoundStick = "SoundStick";

	// Token: 0x04002642 RID: 9794
	public static string PropSoundTick = "SoundTick";

	// Token: 0x04002643 RID: 9795
	public static string PropSoundPickup = "SoundPickup";

	// Token: 0x04002644 RID: 9796
	public static string PropSoundPlace = "SoundPlace";

	// Token: 0x04002645 RID: 9797
	public static string PropSoundImpactHit = "SoundImpactHit";

	// Token: 0x04002646 RID: 9798
	public static string PropSoundImpactGraze = "SoundImpactGraze";

	// Token: 0x04002647 RID: 9799
	public static string PropHasReloadAnim = "HasReloadAnim";

	// Token: 0x04002648 RID: 9800
	public static string PropFuelValue = "FuelValue";

	// Token: 0x04002649 RID: 9801
	public static string PropWeight = "Weight";

	// Token: 0x0400264A RID: 9802
	public static string PropMoldTarget = "MoldTarget";

	// Token: 0x0400264B RID: 9803
	public static string PropSmell = "Smell";

	// Token: 0x0400264C RID: 9804
	public static string PropLightSource = "LightSource";

	// Token: 0x0400264D RID: 9805
	public static string PropLightValue = "LightValue";

	// Token: 0x0400264E RID: 9806
	public static string PropMatEmission = "MatEmission";

	// Token: 0x0400264F RID: 9807
	public static string PropActivateObject = "ActivateObject";

	// Token: 0x04002650 RID: 9808
	public static string PropThrowableDecoy = "ThrowableDecoy";

	// Token: 0x04002651 RID: 9809
	public static string PropGroupName = "Group";

	// Token: 0x04002652 RID: 9810
	public static string PropCritChance = "CritChance";

	// Token: 0x04002653 RID: 9811
	public static string PropCustomIcon = "CustomIcon";

	// Token: 0x04002654 RID: 9812
	public static string PropCustomIconTint = "CustomIconTint";

	// Token: 0x04002655 RID: 9813
	public static string PropPartType = "PartType";

	// Token: 0x04002656 RID: 9814
	public static string PropImageEffectOnActive = "ImageEffectOnActive";

	// Token: 0x04002657 RID: 9815
	public static string PropPlaySoundOnActive = "PlaySoundOnActive";

	// Token: 0x04002658 RID: 9816
	public static string PropActive = "Active";

	// Token: 0x04002659 RID: 9817
	public static string PropAlwaysActive = "AlwaysActive";

	// Token: 0x0400265A RID: 9818
	public static string PropHoldingItemHidden = "HoldingItemHidden";

	// Token: 0x0400265B RID: 9819
	public static string PropVehicleSlotType = "VehicleSlotType";

	// Token: 0x0400265C RID: 9820
	public static string PropGetQualityFromWeapon = "GetQualityFromWeapon";

	// Token: 0x0400265D RID: 9821
	public static string PropAttributes = "Attributes";

	// Token: 0x0400265E RID: 9822
	public static string PropCraftExpValue = "CraftingIngredientExp";

	// Token: 0x0400265F RID: 9823
	public static string PropCraftTimeValue = "CraftingIngredientTime";

	// Token: 0x04002660 RID: 9824
	public static string PropLootExpValue = "LootExpValue";

	// Token: 0x04002661 RID: 9825
	public static string PropEconomicValue = "EconomicValue";

	// Token: 0x04002662 RID: 9826
	public static string PropEconomicSellScale = "EconomicSellScale";

	// Token: 0x04002663 RID: 9827
	public static string PropEconomicBundleSize = "EconomicBundleSize";

	// Token: 0x04002664 RID: 9828
	public static string PropSellableToTrader = "SellableToTrader";

	// Token: 0x04002665 RID: 9829
	public static string PropCraftingSkillExp = "CraftingSkillExp";

	// Token: 0x04002666 RID: 9830
	public static string PropActionSkillExp = "ActionSkillExp";

	// Token: 0x04002667 RID: 9831
	public static string PropInsulation = "Insulation";

	// Token: 0x04002668 RID: 9832
	public static string PropWaterproof = "Waterproof";

	// Token: 0x04002669 RID: 9833
	public static string PropEncumbrance = "Encumbrance";

	// Token: 0x0400266A RID: 9834
	public static string PropDescriptionKey = "DescriptionKey";

	// Token: 0x0400266B RID: 9835
	public static string PropResourceUnit = "ResourceUnit";

	// Token: 0x0400266C RID: 9836
	public static string PropReplaceResourceUnit = "ReplaceResourceUnit";

	// Token: 0x0400266D RID: 9837
	public static string PropMeltTimePerUnit = "MeltTimePerUnit";

	// Token: 0x0400266E RID: 9838
	public static string PropActionSkillGroup = "ActionSkillGroup";

	// Token: 0x0400266F RID: 9839
	public static string PropCraftingSkillGroup = "CraftingSkillGroup";

	// Token: 0x04002670 RID: 9840
	public static string PropCrosshairOnAim = "CrosshairOnAim";

	// Token: 0x04002671 RID: 9841
	public static string PropCrosshairUpAfterShot = "CrosshairUpAfterShot";

	// Token: 0x04002672 RID: 9842
	public static string PropUsableUnderwater = "UsableUnderwater";

	// Token: 0x04002673 RID: 9843
	public static string PropRepairExpMultiplier = "RepairExpMultiplier";

	// Token: 0x04002674 RID: 9844
	public static string PropTags = "Tags";

	// Token: 0x04002675 RID: 9845
	public static string PropShowQuality = "ShowQuality";

	// Token: 0x04002676 RID: 9846
	public static string PropQualityMin = "QualityMin";

	// Token: 0x04002677 RID: 9847
	public static string PropSoundSightIn = "Sound_Sight_In";

	// Token: 0x04002678 RID: 9848
	public static string PropSoundSightOut = "Sound_Sight_Out";

	// Token: 0x04002679 RID: 9849
	public static string PropIgnoreKeystoneSound = "IgnoreKeystoneSound";

	// Token: 0x0400267A RID: 9850
	public static string PropCreativeMode = "CreativeMode";

	// Token: 0x0400267B RID: 9851
	public static string PropCreativeSort1 = "SortOrder1";

	// Token: 0x0400267C RID: 9852
	public static string PropCreativeSort2 = "SortOrder2";

	// Token: 0x0400267D RID: 9853
	public static string PropDistractionTags = "DistractionTags";

	// Token: 0x0400267E RID: 9854
	public static string PropIsSticky = "IsSticky";

	// Token: 0x0400267F RID: 9855
	public static string PropDisplayType = "DisplayType";

	// Token: 0x04002680 RID: 9856
	public static string PropItemTypeIcon = "ItemTypeIcon";

	// Token: 0x04002681 RID: 9857
	public static string PropAltItemTypeIcon = "AltItemTypeIcon";

	// Token: 0x04002682 RID: 9858
	public static string PropAltItemTypeIconColor = "AltItemTypeIconColor";

	// Token: 0x04002683 RID: 9859
	public static string PropUnlockedBy = "UnlockedBy";

	// Token: 0x04002684 RID: 9860
	public static string PropUnlocks = "Unlocks";

	// Token: 0x04002685 RID: 9861
	public static string PropNoScrapping = "NoScrapping";

	// Token: 0x04002686 RID: 9862
	public static string PropScrapTimeOverride = "ScrapTimeOverride";

	// Token: 0x04002687 RID: 9863
	public static string PropNavObject = "NavObject";

	// Token: 0x04002688 RID: 9864
	public static string PropQuestItem = "IsQuestItem";

	// Token: 0x04002689 RID: 9865
	public static string PropTrackerIndexName = "TrackerIndexName";

	// Token: 0x0400268A RID: 9866
	public static string PropTrackerNavObject = "TrackerNavObject";

	// Token: 0x0400268B RID: 9867
	public static string PropTraderStageTemplate = "TraderStageTemplate";

	// Token: 0x0400268C RID: 9868
	public static string PropMaxModsAllowed = "MaxModsAllowed";

	// Token: 0x0400268D RID: 9869
	public static string PropRestrictedMove = "RestrictedMove";

	// Token: 0x0400268E RID: 9870
	public static string PropPreviousItemType = "PreviousTechItem";

	// Token: 0x0400268F RID: 9871
	public static int MAX_ITEMS = Block.MAX_BLOCKS + 16384;

	// Token: 0x04002690 RID: 9872
	public static NameIdMapping nameIdMapping;

	// Token: 0x04002691 RID: 9873
	public static byte[] fullMappingDataForClients;

	// Token: 0x04002692 RID: 9874
	public static ItemClass[] list;

	// Token: 0x04002693 RID: 9875
	public static string[] itemActionNames;

	// Token: 0x04002694 RID: 9876
	public const int cMaxActionNames = 5;

	// Token: 0x04002695 RID: 9877
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Dictionary<string, ItemClass> nameToItem = new Dictionary<string, ItemClass>();

	// Token: 0x04002696 RID: 9878
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Dictionary<string, ItemClass> nameToItemCaseInsensitive = new CaseInsensitiveStringDictionary<ItemClass>();

	// Token: 0x04002697 RID: 9879
	[PublicizedFrom(EAccessModifier.Protected)]
	public static List<string> itemNames = new List<string>();

	// Token: 0x04002698 RID: 9880
	public static readonly ReadOnlyCollection<string> ItemNames = new ReadOnlyCollection<string>(ItemClass.itemNames);

	// Token: 0x04002699 RID: 9881
	public DynamicProperties Properties;

	// Token: 0x0400269A RID: 9882
	[PublicizedFrom(EAccessModifier.Private)]
	public string localizedName;

	// Token: 0x0400269B RID: 9883
	public float Smell;

	// Token: 0x0400269C RID: 9884
	public string MeshFile;

	// Token: 0x0400269D RID: 9885
	public string DropMeshFile;

	// Token: 0x0400269E RID: 9886
	public string HandMeshFile;

	// Token: 0x0400269F RID: 9887
	public string StickyMaterial;

	// Token: 0x040026A0 RID: 9888
	public float StickyOffset = 0.15f;

	// Token: 0x040026A1 RID: 9889
	public int StickyColliderUp = -1;

	// Token: 0x040026A2 RID: 9890
	public float StickyColliderRadius = 0.2f;

	// Token: 0x040026A3 RID: 9891
	public float StickyColliderLength = -1f;

	// Token: 0x040026A4 RID: 9892
	public bool IsSticky;

	// Token: 0x040026A5 RID: 9893
	public const int cActionUpdateCount = 3;

	// Token: 0x040026A6 RID: 9894
	public ItemAction[] Actions;

	// Token: 0x040026A7 RID: 9895
	public MaterialBlock MadeOfMaterial;

	// Token: 0x040026A8 RID: 9896
	public DataItem<int> HoldType = new DataItem<int>(0);

	// Token: 0x040026A9 RID: 9897
	public DataItem<int> Stacknumber = new DataItem<int>(500);

	// Token: 0x040026AA RID: 9898
	public DataItem<int> MaxUseTimes = new DataItem<int>(0);

	// Token: 0x040026AB RID: 9899
	public DataItem<bool> MaxUseTimesBreaksAfter = new DataItem<bool>(false);

	// Token: 0x040026AC RID: 9900
	public ItemClass MoldTarget;

	// Token: 0x040026AD RID: 9901
	public DataItem<string> LightSource;

	// Token: 0x040026AE RID: 9902
	public DataItem<string> ActivateObject;

	// Token: 0x040026AF RID: 9903
	public DataItem<bool> ThrowableDecoy;

	// Token: 0x040026B0 RID: 9904
	public ItemData.DataItemArrayRepairTools RepairTools;

	// Token: 0x040026B1 RID: 9905
	public DataItem<int> RepairAmount;

	// Token: 0x040026B2 RID: 9906
	public DataItem<float> RepairTime;

	// Token: 0x040026B3 RID: 9907
	public DataItem<float> CritChance;

	// Token: 0x040026B4 RID: 9908
	public string[] Groups = new string[] { "Decor/Miscellaneous" };

	// Token: 0x040026B5 RID: 9909
	public DataItem<string> CustomIcon;

	// Token: 0x040026B6 RID: 9910
	public Color CustomIconTint;

	// Token: 0x040026B7 RID: 9911
	public DataItem<bool> UserHidden = new DataItem<bool>(false);

	// Token: 0x040026B8 RID: 9912
	public string VehicleSlotType;

	// Token: 0x040026B9 RID: 9913
	public bool GetQualityFromWeapon;

	// Token: 0x040026BA RID: 9914
	public string DescriptionKey;

	// Token: 0x040026BB RID: 9915
	public bool IsResourceUnit;

	// Token: 0x040026BC RID: 9916
	public string ReplaceResourceUnit = "";

	// Token: 0x040026BD RID: 9917
	public float MeltTimePerUnit;

	// Token: 0x040026BE RID: 9918
	public string ActionSkillGroup = "";

	// Token: 0x040026BF RID: 9919
	public string CraftingSkillGroup = "";

	// Token: 0x040026C0 RID: 9920
	public bool UsableUnderwater = true;

	// Token: 0x040026C1 RID: 9921
	public float lightValue;

	// Token: 0x040026C2 RID: 9922
	public string soundSightIn = "";

	// Token: 0x040026C3 RID: 9923
	public string soundSightOut = "";

	// Token: 0x040026C4 RID: 9924
	public bool ignoreKeystoneSound;

	// Token: 0x040026C5 RID: 9925
	public bool HoldingItemHidden;

	// Token: 0x040026C6 RID: 9926
	public List<int> PartParentId;

	// Token: 0x040026C7 RID: 9927
	public PreviewData Preview;

	// Token: 0x040026C8 RID: 9928
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bCanHold;

	// Token: 0x040026C9 RID: 9929
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bCanDrop;

	// Token: 0x040026CA RID: 9930
	public bool HasSubItems;

	// Token: 0x040026CB RID: 9931
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] attachments;

	// Token: 0x040026CC RID: 9932
	public static float MaxStackSizeModifier = 1f;

	// Token: 0x040026CD RID: 9933
	public bool bCraftingTool;

	// Token: 0x040026CE RID: 9934
	public float CraftComponentExp = 3f;

	// Token: 0x040026CF RID: 9935
	public float CraftComponentTime = 1f;

	// Token: 0x040026D0 RID: 9936
	public float LootExp = 1f;

	// Token: 0x040026D1 RID: 9937
	public float EconomicValue;

	// Token: 0x040026D2 RID: 9938
	public float EconomicSellScale = 1f;

	// Token: 0x040026D3 RID: 9939
	public int EconomicBundleSize = 1;

	// Token: 0x040026D4 RID: 9940
	public bool SellableToTrader = true;

	// Token: 0x040026D5 RID: 9941
	public string TraderStageTemplate;

	// Token: 0x040026D6 RID: 9942
	public int MaxModsAllowed = 1;

	// Token: 0x040026D7 RID: 9943
	public int CraftingSkillExp = 10;

	// Token: 0x040026D8 RID: 9944
	public int ActionSkillExp = 10;

	// Token: 0x040026D9 RID: 9945
	public float RepairExpMultiplier = 10f;

	// Token: 0x040026DA RID: 9946
	public float Insulation;

	// Token: 0x040026DB RID: 9947
	public float WaterProof;

	// Token: 0x040026DC RID: 9948
	public float Encumbrance;

	// Token: 0x040026DD RID: 9949
	public string SoundUnholster = "generic_unholster";

	// Token: 0x040026DE RID: 9950
	public string SoundHolster = "generic_holster";

	// Token: 0x040026DF RID: 9951
	public string SoundStick;

	// Token: 0x040026E0 RID: 9952
	public string SoundTick;

	// Token: 0x040026E1 RID: 9953
	public float SoundTickDelay = 1f;

	// Token: 0x040026E2 RID: 9954
	public string SoundPickup = "craft_take_item";

	// Token: 0x040026E3 RID: 9955
	public string SoundPlace = "craft_place_item";

	// Token: 0x040026E4 RID: 9956
	public string SoundImpactHit;

	// Token: 0x040026E5 RID: 9957
	public string SoundImpactGraze;

	// Token: 0x040026E6 RID: 9958
	public bool HasReloadAnim = true;

	// Token: 0x040026E7 RID: 9959
	public bool bShowCrosshairOnAiming;

	// Token: 0x040026E8 RID: 9960
	public bool bCrosshairUpAfterShot;

	// Token: 0x040026E9 RID: 9961
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bRestrictedMove;

	// Token: 0x040026EA RID: 9962
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<XUiC_ItemStack.StackLocationTypes> restrictedTo = new List<XUiC_ItemStack.StackLocationTypes>();

	// Token: 0x040026EB RID: 9963
	public ItemClass.ItemTechTypes ItemTechType;

	// Token: 0x040026EC RID: 9964
	[PublicizedFrom(EAccessModifier.Private)]
	public string previousTechTypeName = "";

	// Token: 0x040026ED RID: 9965
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] previousTechItems;

	// Token: 0x040026EE RID: 9966
	public static ItemClass.ItemTechTypes MaxTechType = ItemClass.ItemTechTypes.T3;

	// Token: 0x040026EF RID: 9967
	public bool IsLimited;

	// Token: 0x040026F0 RID: 9968
	public EnumCreativeMode CreativeMode;

	// Token: 0x040026F1 RID: 9969
	public string SortOrder;

	// Token: 0x040026F2 RID: 9970
	public FastTags<TagGroup.Global> ItemTags;

	// Token: 0x040026F3 RID: 9971
	public MinEffectController Effects;

	// Token: 0x040026F4 RID: 9972
	public FastTags<TagGroup.Global> DistractionTags;

	// Token: 0x040026F5 RID: 9973
	public SDCSUtils.SlotData SDCSData;

	// Token: 0x040026F6 RID: 9974
	public string DisplayType;

	// Token: 0x040026F7 RID: 9975
	public string ItemTypeIcon = "";

	// Token: 0x040026F8 RID: 9976
	public string AltItemTypeIcon;

	// Token: 0x040026F9 RID: 9977
	public Color AltItemTypeIconColor;

	// Token: 0x040026FA RID: 9978
	public bool ShowQualityBar;

	// Token: 0x040026FB RID: 9979
	public byte QualityMin;

	// Token: 0x040026FC RID: 9980
	public bool NoScrapping;

	// Token: 0x040026FD RID: 9981
	public float ScrapTimeOverride;

	// Token: 0x040026FE RID: 9982
	public string Unlocks = "";

	// Token: 0x040026FF RID: 9983
	public string NavObject = "";

	// Token: 0x04002700 RID: 9984
	public bool IsQuestItem;

	// Token: 0x04002701 RID: 9985
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> stopBleed = FastTags<TagGroup.Global>.Parse("stopsBleeding");

	// Token: 0x04002702 RID: 9986
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T0Tag = FastTags<TagGroup.Global>.Parse("T0");

	// Token: 0x04002703 RID: 9987
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T1Tag = FastTags<TagGroup.Global>.Parse("T1");

	// Token: 0x04002704 RID: 9988
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T2Tag = FastTags<TagGroup.Global>.Parse("T2");

	// Token: 0x04002705 RID: 9989
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> T3Tag = FastTags<TagGroup.Global>.Parse("T3");

	// Token: 0x04002706 RID: 9990
	public string TrackerIndexName;

	// Token: 0x04002707 RID: 9991
	public string TrackerNavObject;

	// Token: 0x04002708 RID: 9992
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData[] unlockedBy;

	// Token: 0x04002709 RID: 9993
	public float TraderQualityMaxMod;

	// Token: 0x0400270A RID: 9994
	public float TraderQualityMinMod;

	// Token: 0x0400270B RID: 9995
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cFilterQualityPrefix = "#";

	// Token: 0x0400270C RID: 9996
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] ActionProfilerNames = new string[] { "action0", "action1", "action2" };

	// Token: 0x0400270D RID: 9997
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, int> fixedItemIds = new Dictionary<string, int>();

	// Token: 0x0400270E RID: 9998
	public const float cGSStatScale = 0.005f;

	// Token: 0x0400270F RID: 9999
	public const float cGSStatMax = 163.83499f;

	// Token: 0x04002710 RID: 10000
	public Dictionary<PassiveEffects, List<ItemClass.GSStat>> Stats;

	// Token: 0x04002711 RID: 10001
	public static int MaxQualityTier;

	// Token: 0x020005F0 RID: 1520
	public enum EnumCrosshairType
	{
		// Token: 0x04002713 RID: 10003
		None,
		// Token: 0x04002714 RID: 10004
		Plus,
		// Token: 0x04002715 RID: 10005
		Crosshair,
		// Token: 0x04002716 RID: 10006
		CrosshairOnAiming,
		// Token: 0x04002717 RID: 10007
		Damage,
		// Token: 0x04002718 RID: 10008
		Upgrade,
		// Token: 0x04002719 RID: 10009
		Repair,
		// Token: 0x0400271A RID: 10010
		PowerSource,
		// Token: 0x0400271B RID: 10011
		Heal,
		// Token: 0x0400271C RID: 10012
		PowerItem,
		// Token: 0x0400271D RID: 10013
		Blocked,
		// Token: 0x0400271E RID: 10014
		Dot
	}

	// Token: 0x020005F1 RID: 1521
	public enum ItemTechTypes
	{
		// Token: 0x04002720 RID: 10016
		None,
		// Token: 0x04002721 RID: 10017
		T0,
		// Token: 0x04002722 RID: 10018
		T1,
		// Token: 0x04002723 RID: 10019
		T2,
		// Token: 0x04002724 RID: 10020
		T3
	}

	// Token: 0x020005F2 RID: 1522
	// (Invoke) Token: 0x06003201 RID: 12801
	public delegate bool FilterItem(ItemClass _class, Block _block);

	// Token: 0x020005F3 RID: 1523
	public struct GSStat
	{
		// Token: 0x04002725 RID: 10021
		public short quality;

		// Token: 0x04002726 RID: 10022
		public short gameStage;

		// Token: 0x04002727 RID: 10023
		public float chance;

		// Token: 0x04002728 RID: 10024
		public short min;

		// Token: 0x04002729 RID: 10025
		public short max;
	}
}
