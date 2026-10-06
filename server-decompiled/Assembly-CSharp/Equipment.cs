using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using BhvrAnalyticsServices.Interfaces;
using Services;
using Services.Analytics;
using Services.Analytics.Events;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class Equipment
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06002AF1 RID: 10993 RVA: 0x001102DC File Offset: 0x0010E4DC
	// (remove) Token: 0x06002AF2 RID: 10994 RVA: 0x00110314 File Offset: 0x0010E514
	public event Action OnChanged;

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x00110349 File Offset: 0x0010E549
	public ItemStackGrid ItemGrid
	{
		get
		{
			return this.itemGrid;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x00110351 File Offset: 0x0010E551
	// (set) Token: 0x06002AF5 RID: 10997 RVA: 0x00110359 File Offset: 0x0010E559
	public ItemClass[] CosmeticSlots
	{
		get
		{
			return this.m_cosmeticSlots;
		}
		set
		{
			this.m_cosmeticSlots = value;
		}
	}

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06002AF6 RID: 10998 RVA: 0x00110364 File Offset: 0x0010E564
	// (remove) Token: 0x06002AF7 RID: 10999 RVA: 0x0011039C File Offset: 0x0010E59C
	public event Equipment_CosmeticUnlocked CosmeticUnlocked;

	// Token: 0x06002AF8 RID: 11000 RVA: 0x001103D1 File Offset: 0x0010E5D1
	public Equipment()
		: this(null)
	{
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x001103DC File Offset: 0x0010E5DC
	public Equipment(EntityAlive _entity)
	{
		this.m_entity = _entity;
		int num = 13;
		this.itemGrid = ItemStackGrid.Create(new Vector2i(num, 1), XUiC_ItemStack.StackLocationTypes.Equipment, false, false, _entity);
		this.CosmeticSlots = new ItemClass[num];
		this.preferredItemSlots = new int[num];
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x0011044F File Offset: 0x0010E64F
	public ItemClass GetCosmeticSlot(int index, bool useTemporary)
	{
		if (index >= this.CosmeticSlots.Length)
		{
			return null;
		}
		if (useTemporary && this.tempCosmeticSlotIndex == index)
		{
			return this.tempCosmeticSlot;
		}
		return this.CosmeticSlots[index];
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x0011047C File Offset: 0x0010E67C
	public void SetCosmeticSlot(ItemClassArmor itemClass)
	{
		if (itemClass.EquipSlot.SlotType() == EquipmentSlotType.Armor)
		{
			if (!this.HasCosmeticUnlocked(itemClass).Item1)
			{
				return;
			}
			int equipSlot = (int)itemClass.EquipSlot;
			ItemClassArmor itemClassArmor = this.CosmeticSlots[equipSlot] as ItemClassArmor;
			if (itemClassArmor != null && itemClassArmor.ArmorGroup[0] == itemClass.ArmorGroup[0])
			{
				return;
			}
			this.CosmeticSlots[equipSlot] = itemClass;
			if (this.m_entity && !this.m_entity.isEntityRemote)
			{
				this.m_entity.bPlayerEquipmentChanged = true;
			}
		}
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x00110508 File Offset: 0x0010E708
	public void SetCosmeticSlot(int slotID, int id)
	{
		if (id == 0)
		{
			this.CosmeticSlots[slotID] = null;
			if (this.m_entity && !this.m_entity.isEntityRemote)
			{
				this.m_entity.bPlayerEquipmentChanged = true;
				return;
			}
		}
		else
		{
			ItemClass itemClass = ItemClass.GetItemClass(Equipment.CosmeticMappingIDString[id], false);
			ItemClassArmor itemClassArmor = itemClass as ItemClassArmor;
			if (itemClassArmor != null)
			{
				if (itemClassArmor.EquipSlot.SlotType() == EquipmentSlotType.Armor)
				{
					this.CosmeticSlots[(int)itemClassArmor.EquipSlot] = itemClass;
					if (this.m_entity && !this.m_entity.isEntityRemote)
					{
						this.m_entity.bPlayerEquipmentChanged = true;
						return;
					}
				}
			}
			else
			{
				this.CosmeticSlots[slotID] = itemClass;
				if (this.m_entity && !this.m_entity.isEntityRemote)
				{
					this.m_entity.bPlayerEquipmentChanged = true;
				}
			}
		}
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x001105DC File Offset: 0x0010E7DC
	public int[] GetCosmeticIDs()
	{
		int num = 13;
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			if (this.m_cosmeticSlots[i] == null)
			{
				array[i] = 0;
			}
			else
			{
				array[i] = Equipment.CosmeticMappingStringID[this.m_cosmeticSlots[i].GetItemName()];
			}
		}
		return array;
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x0011062C File Offset: 0x0010E82C
	public void ClearCosmeticSlots()
	{
		for (int i = 0; i < this.m_cosmeticSlots.Length; i++)
		{
			this.m_cosmeticSlots[i] = null;
		}
		if (this.m_entity && !this.m_entity.isEntityRemote)
		{
			this.m_entity.bPlayerEquipmentChanged = true;
		}
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x0011067C File Offset: 0x0010E87C
	public void UnlockCosmeticItem(ItemClass itemClass)
	{
		if (itemClass == null)
		{
			return;
		}
		string itemName = itemClass.GetItemName();
		if (Equipment.CosmeticMappingStringID.ContainsKey(itemName))
		{
			int num = Equipment.CosmeticMappingStringID[itemName];
			if (!this.m_unlockedCosmetics.Contains(num))
			{
				this.m_unlockedCosmetics.Add(num);
				if (this.CosmeticUnlocked != null)
				{
					this.CosmeticUnlocked(itemName);
				}
			}
		}
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x001106DC File Offset: 0x0010E8DC
	public void ModifyValue(ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags, bool _useDurability = false)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty() && !itemValue.Equals(_originalItemValue) && itemValue.ItemClass != null)
			{
				itemValue.ModifyValue(this.m_entity, _originalItemValue, _passiveEffect, ref _base_val, ref _perc_val, tags, true, _useDurability);
			}
		}
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x00110740 File Offset: 0x0010E940
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty() && !itemValue.Equals(_originalItemValue) && itemValue.ItemClass != null)
			{
				itemValue.GetModifiedValueData(_modValueSources, _sourceType, this.m_entity, _originalItemValue, _passiveEffect, ref _base_val, ref _perc_val, tags);
			}
		}
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x001107A8 File Offset: 0x0010E9A8
	[return: TupleElementNames(new string[] { "isUnlocked", "set" })]
	public ValueTuple<bool, EntitlementSetEnum> HasCosmeticUnlocked(ItemClass itemClass)
	{
		if (itemClass == null)
		{
			return new ValueTuple<bool, EntitlementSetEnum>(false, EntitlementSetEnum.None);
		}
		string itemName = itemClass.GetItemName();
		if (!Equipment.CosmeticMappingStringID.ContainsKey(itemName))
		{
			return new ValueTuple<bool, EntitlementSetEnum>(false, EntitlementSetEnum.None);
		}
		EntitlementSetEnum entitlementSetEnum = EntitlementSetEnum.None;
		if (itemClass.SDCSData != null)
		{
			entitlementSetEnum = EntitlementManager.Instance.GetSetForAsset(itemClass.SDCSData.PrefabName);
			if (entitlementSetEnum != EntitlementSetEnum.None && EntitlementManager.Instance.HasEntitlement(entitlementSetEnum))
			{
				return new ValueTuple<bool, EntitlementSetEnum>(true, entitlementSetEnum);
			}
		}
		return new ValueTuple<bool, EntitlementSetEnum>(this.m_unlockedCosmetics.Contains(Equipment.CosmeticMappingStringID[itemClass.GetItemName()]), entitlementSetEnum);
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x00110838 File Offset: 0x0010EA38
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty() && itemValue.ItemClass != null)
			{
				itemValue.FireEvent(_eventType, _params);
			}
		}
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x00110888 File Offset: 0x0010EA88
	public void DropItems()
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty())
			{
				this.DropItemOnGround(itemValue);
				this.SetSlotItem(i, ItemValue.None);
			}
		}
		this.updateInsulation();
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x001108E0 File Offset: 0x0010EAE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateInsulation()
	{
		this.waterProof = 0f;
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty())
			{
				this.waterProof += itemValue.ItemClass.WaterProof;
			}
		}
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x00110940 File Offset: 0x0010EB40
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DropItemOnGround(ItemValue _itemValue)
	{
		this.m_entity.world.GetGameManager().ItemDropServer(new ItemStack(_itemValue, 1), this.m_entity.GetPosition(), new Vector3(0.5f, 0f, 0.5f), this.m_entity.belongsPlayerId, 60f, false);
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x00110999 File Offset: 0x0010EB99
	public float GetTotalInsulation()
	{
		return this.insulation;
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x001109A1 File Offset: 0x0010EBA1
	public float GetTotalWaterproof()
	{
		return this.waterProof;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x001109A9 File Offset: 0x0010EBA9
	public int GetSlotCount()
	{
		return this.itemGrid.Length;
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x001109B6 File Offset: 0x0010EBB6
	public ItemValue GetSlotItem(int index)
	{
		if (index >= this.itemGrid.Length)
		{
			return ItemValue.None;
		}
		return this.itemGrid[index].itemValue;
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x001109E0 File Offset: 0x0010EBE0
	public bool HasAnyItems()
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			if (!this.itemGrid[i].IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x00110A1C File Offset: 0x0010EC1C
	public void PerformActionOnSlots(Action<ItemValue> _action)
	{
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty())
			{
				_action(itemValue);
			}
		}
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x00110A60 File Offset: 0x0010EC60
	public void SetTempCosmeticSlot(int index, ItemClass itemClass)
	{
		this.tempCosmeticSlotIndex = index;
		this.tempCosmeticSlot = itemClass;
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x00110A70 File Offset: 0x0010EC70
	public void ClearTempCosmeticSlot()
	{
		this.tempCosmeticSlotIndex = -1;
		this.tempCosmeticSlot = null;
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x00110A80 File Offset: 0x0010EC80
	public void ApplyTempCosmeticSlot()
	{
		if (this.tempCosmeticSlotIndex != -1)
		{
			this.m_cosmeticSlots[this.tempCosmeticSlotIndex] = this.tempCosmeticSlot;
			if (this.m_entity && !this.m_entity.isEntityRemote)
			{
				this.m_entity.bPlayerEquipmentChanged = true;
			}
		}
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x00110AD0 File Offset: 0x0010ECD0
	public void CalcDamage(ref int entityDamageTaken, ref int armorDamageTaken, FastTags<TagGroup.Global> damageTypeTag, EntityAlive attacker, ItemValue attackingItem)
	{
		armorDamageTaken = entityDamageTaken;
		if (damageTypeTag.Test_AnySet(Equipment.physicalDamageTypes))
		{
			if (entityDamageTaken > 0)
			{
				float num = this.GetTotalPhysicalArmorRating(attacker, attackingItem) / 100f;
				armorDamageTaken = Utils.FastMax((num > 0f) ? 1 : 0, Utils.FastRoundToInt((float)entityDamageTaken * num));
				entityDamageTaken -= armorDamageTaken;
				return;
			}
		}
		else
		{
			entityDamageTaken = Utils.FastRoundToInt(Utils.FastMax(0f, (float)entityDamageTaken * (1f - EffectManager.GetValue(PassiveEffects.ElementalDamageResist, null, 0f, this.m_entity, null, damageTypeTag, true, true, true, true, true, 1, true, false) / 100f)));
			armorDamageTaken = Utils.FastRoundToInt((float)Utils.FastMax(0, armorDamageTaken - entityDamageTaken));
		}
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x00110B80 File Offset: 0x0010ED80
	public float GetTotalPhysicalArmorRating(EntityAlive attacker, ItemValue attackingItem)
	{
		FastTags<TagGroup.Global> fastTags = Equipment.coreDamageResist;
		if (attackingItem != null)
		{
			fastTags |= attackingItem.ItemClassOrMissing.ItemTags;
		}
		float value = EffectManager.GetValue(PassiveEffects.PhysicalDamageResist, null, 0f, this.m_entity, null, fastTags, true, true, true, true, true, 1, true, true);
		return EffectManager.GetValue(PassiveEffects.TargetArmor, attackingItem, value, attacker, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x00110BE8 File Offset: 0x0010EDE8
	public List<ItemValue> GetArmor()
	{
		List<ItemValue> list = new List<ItemValue>();
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty())
			{
				float num = 0f;
				float num2 = 1f;
				itemValue.ModifyValue(this.m_entity, null, PassiveEffects.PhysicalDamageResist, ref num, ref num2, FastTags<TagGroup.Global>.all, true, false);
				if (num != 0f)
				{
					list.Add(itemValue);
				}
			}
		}
		return list;
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x00110C64 File Offset: 0x0010EE64
	public bool CheckBreakUseItems()
	{
		bool flag = false;
		this.CurrentLowestDurability = 1f;
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty())
			{
				ItemClass forId = ItemClass.GetForId(itemValue.type);
				float percentUsesLeft = itemValue.PercentUsesLeft;
				if (percentUsesLeft < this.CurrentLowestDurability)
				{
					this.CurrentLowestDurability = percentUsesLeft;
				}
				if (forId != null && itemValue.MaxUseTimes > 0 && forId.MaxUseTimesBreaksAfter.Value && itemValue.UseTimes > (float)itemValue.MaxUseTimes)
				{
					this.SetSlotItem(i, ItemValue.None);
					if (this.m_entity != null && forId.Properties.Values.ContainsKey(ItemClass.PropSoundDestroy))
					{
						Manager.BroadcastPlay(this.m_entity, forId.Properties.Values[ItemClass.PropSoundDestroy], false, 1f);
					}
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x00110D60 File Offset: 0x0010EF60
	public void SetSlotItem(int index, ItemValue value)
	{
		if (value == null)
		{
			value = ItemValue.None;
		}
		ItemValue itemValue = this.itemGrid[index].itemValue.Clone();
		if (value.IsEmpty() && itemValue.IsEmpty())
		{
			return;
		}
		this.m_entity.IsEquipping = true;
		if (!itemValue.IsEmpty() && itemValue.Equals(value))
		{
			this.itemGrid[index] = new ItemStack(value, 1);
			ItemValue itemValue2 = this.itemGrid[index].itemValue;
			this.m_entity.MinEventContext.ItemValue = itemValue2;
			itemValue2.FireEvent(MinEventTypes.onSelfEquipStart, this.m_entity.MinEventContext);
		}
		else
		{
			this.TeardownSlot(index);
			this.itemGrid[index] = (value.IsEmpty() ? ItemStack.Empty : new ItemStack(value, 1));
			this.BuildSlot(index);
			this.OnEquipmentChanged();
			if (this.m_entity && !this.m_entity.isEntityRemote)
			{
				this.LogEquipmentUsageEvent(itemValue, value);
			}
		}
		this.m_entity.IsEquipping = false;
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x00110E6C File Offset: 0x0010F06C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TeardownSlot(int _index)
	{
		ItemValue itemValue = this.itemGrid[_index].itemValue;
		if (itemValue.IsEmpty())
		{
			return;
		}
		if (itemValue.ItemClass.HasTrigger(MinEventTypes.onSelfItemActivate) && itemValue.Activated)
		{
			this.m_entity.MinEventContext.ItemValue = itemValue;
			itemValue.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
			itemValue.Activated = false;
		}
		for (int i = 0; i < itemValue.ModificationCount; i++)
		{
			ItemValue modification = itemValue.GetModification(i);
			if (modification != null)
			{
				ItemClass itemClass = modification.ItemClass;
				if (itemClass != null && itemClass.HasTrigger(MinEventTypes.onSelfItemActivate) && modification.Activated)
				{
					this.m_entity.MinEventContext.ItemValue = modification;
					modification.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
					modification.Activated = false;
				}
			}
		}
		this.m_entity.MinEventContext.ItemValue = itemValue;
		itemValue.FireEvent(MinEventTypes.onSelfEquipStop, this.m_entity.MinEventContext);
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x00110F60 File Offset: 0x0010F160
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildSlot(int _index)
	{
		this.preferredItemSlots[_index] = this.itemGrid[_index].itemValue.type;
		this.slotsSetFlags |= 1 << _index;
		this.slotsChangedFlags |= 1 << _index;
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x00110FB1 File Offset: 0x0010F1B1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEquipmentChanged()
	{
		if (this.m_entity && !this.m_entity.isEntityRemote)
		{
			this.m_entity.bPlayerEquipmentChanged = true;
		}
		this.ResetArmorGroups();
		Action onChanged = this.OnChanged;
		if (onChanged == null)
		{
			return;
		}
		onChanged();
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x00110FF0 File Offset: 0x0010F1F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogEquipmentUsageEvent(ItemValue previousItem, ItemValue currentItem)
	{
		if (Helper.IsEditingOrPlaytesting() || !GameManager.Instance.GameHasStarted)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = this.m_entity as EntityPlayerLocal;
		if (entityPlayerLocal == null)
		{
			return;
		}
		List<EquipmentUsageEventData.ModificationData> list = new List<EquipmentUsageEventData.ModificationData>();
		int num = 0;
		while (currentItem != null && num < currentItem.ModificationCount)
		{
			ItemValue modification = currentItem.GetModification(num);
			if (modification != null && !modification.IsEmpty())
			{
				List<EquipmentUsageEventData.ModificationData> list2 = list;
				ItemClass itemClass = modification.ItemClass;
				list2.Add(new EquipmentUsageEventData.ModificationData((itemClass != null) ? itemClass.Name : null, (modification.Quality != 0) ? new int?((int)modification.Quality) : null));
			}
			num++;
		}
		Dictionary<PassiveEffects, float> dictionary = new Dictionary<PassiveEffects, float>();
		MinEffectController minEffectController;
		if (currentItem == null)
		{
			minEffectController = null;
		}
		else
		{
			ItemClass itemClass2 = currentItem.ItemClass;
			minEffectController = ((itemClass2 != null) ? itemClass2.Effects : null);
		}
		MinEffectController minEffectController2 = minEffectController;
		if (minEffectController2 != null)
		{
			foreach (MinEffectGroup minEffectGroup in minEffectController2.EffectGroups)
			{
				foreach (PassiveEffect passiveEffect in minEffectGroup.PassiveEffects)
				{
					if (!dictionary.ContainsKey(passiveEffect.Type))
					{
						dictionary.Add(passiveEffect.Type, (float)Math.Round((double)EffectManager.GetValue(passiveEffect.Type, currentItem, 0f, null, null, passiveEffect.Tags, true, true, true, true, true, 1, true, false), 3));
					}
				}
			}
		}
		EquipmentUsageEventData equipmentUsageEventData = new EquipmentUsageEventData();
		equipmentUsageEventData.ServerId = Helper.GetServerId();
		equipmentUsageEventData.SaveId = Helper.GetSaveId();
		equipmentUsageEventData.EquipmentChangeTimestamp = DateTime.UtcNow.ToString("O");
		equipmentUsageEventData.PlayerLevel = entityPlayerLocal.Progression.GetLevel();
		equipmentUsageEventData.PersonalGameStage = entityPlayerLocal.gameStage;
		string text;
		if (currentItem == null)
		{
			text = null;
		}
		else
		{
			ItemClass itemClass3 = currentItem.ItemClass;
			text = ((itemClass3 != null) ? itemClass3.Name : null);
		}
		equipmentUsageEventData.EquipmentName = text;
		ItemClassArmor itemClassArmor = ((currentItem != null) ? currentItem.ItemClass : null) as ItemClassArmor;
		equipmentUsageEventData.EquipmentSlot = ((itemClassArmor != null) ? itemClassArmor.EquipSlot.ToStringCached<EquipmentSlots>() : null);
		object obj;
		if (currentItem == null)
		{
			obj = null;
		}
		else
		{
			ItemClass itemClass4 = currentItem.ItemClass;
			obj = ((itemClass4 != null) ? itemClass4.ItemTags.GetTagNames() : null);
		}
		equipmentUsageEventData.EquipmentTags = obj;
		ushort? num2 = ((currentItem != null) ? new ushort?(currentItem.Quality) : null);
		int? num3 = ((num2 != null) ? new int?((int)num2.GetValueOrDefault()) : null);
		int num4 = 0;
		ushort? num5 = ((!((num3.GetValueOrDefault() == num4) & (num3 != null))) ? ((currentItem != null) ? new ushort?(currentItem.Quality) : null) : null);
		equipmentUsageEventData.Quality = ((num5 != null) ? new int?((int)num5.GetValueOrDefault()) : null);
		equipmentUsageEventData.ModSlots = ((list.Count != 0) ? list : null);
		equipmentUsageEventData.PassiveEffects = ((dictionary.Count != 0) ? dictionary : null);
		equipmentUsageEventData.Stats = ((currentItem != null) ? currentItem.Stats : null);
		equipmentUsageEventData.CurrentDegradation = ((currentItem != null) ? new float?(currentItem.UseTimes) : null);
		ItemClass itemClass5 = previousItem.ItemClass;
		equipmentUsageEventData.PreviousEquipment = ((itemClass5 != null) ? itemClass5.Name : null);
		EquipmentUsageEventData equipmentUsageEventData2 = equipmentUsageEventData;
		ServiceProvider.Instance.Get<IAnalyticsService>().LogEvent(equipmentUsageEventData2);
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x00111360 File Offset: 0x0010F560
	public void FireEventsForSlots(MinEventTypes _event, int _flags = -1)
	{
		this.m_entity.MinEventContext.Self = this.m_entity;
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			if ((_flags & (1 << i)) > 0)
			{
				ItemValue itemValue = this.itemGrid[i].itemValue;
				if (!itemValue.IsEmpty() && itemValue.ItemClass != null)
				{
					this.m_entity.MinEventContext.ItemValue = itemValue;
					itemValue.FireEvent(_event, this.m_entity.MinEventContext);
				}
			}
		}
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x001113E9 File Offset: 0x0010F5E9
	public void FireEventsForSetSlots()
	{
		this.FireEventsForSlots(MinEventTypes.onSelfEquipStart, this.slotsSetFlags);
		this.slotsSetFlags = 0;
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x00111400 File Offset: 0x0010F600
	public void FireEventsForChangedSlots()
	{
		if (this.slotsChangedFlags == 0)
		{
			return;
		}
		this.m_entity.IsEquipping = true;
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			if ((this.slotsChangedFlags & (1 << i)) > 0)
			{
				ItemValue itemValue = this.itemGrid[i].itemValue;
				if (!itemValue.IsEmpty())
				{
					ItemClass itemClass = itemValue.ItemClass;
					if (itemClass != null)
					{
						this.m_entity.MinEventContext.Self = this.m_entity;
						this.m_entity.MinEventContext.ItemValue = itemValue;
						itemValue.FireEvent(MinEventTypes.onSelfEquipChanged, this.m_entity.MinEventContext);
						if (itemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
						{
							if (!itemValue.Activated)
							{
								itemValue.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
							}
							else
							{
								itemValue.FireEvent(MinEventTypes.onSelfItemActivate, this.m_entity.MinEventContext);
							}
						}
						for (int j = 0; j < itemValue.ModificationCount; j++)
						{
							ItemValue modification = itemValue.GetModification(j);
							if (modification != null && modification.ItemClass != null && modification.ItemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
							{
								this.m_entity.MinEventContext.ItemValue = modification;
								if (!modification.Activated)
								{
									modification.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
								}
								else
								{
									modification.FireEvent(MinEventTypes.onSelfItemActivate, this.m_entity.MinEventContext);
								}
							}
						}
					}
				}
			}
		}
		this.slotsChangedFlags = 0;
		this.m_entity.IsEquipping = false;
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x00111580 File Offset: 0x0010F780
	public void Update()
	{
		if (Time.time - this.lastUpdateTime >= 1f)
		{
			for (int i = 0; i < this.itemGrid.Length; i++)
			{
				ItemValue itemValue = this.itemGrid[i].itemValue;
				if (!itemValue.IsEmpty())
				{
					this.m_entity.MinEventContext.ItemValue = itemValue;
					itemValue.FireEvent(MinEventTypes.onSelfEquipUpdate, this.m_entity.MinEventContext);
				}
			}
			this.lastUpdateTime = Time.time;
		}
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x001115FF File Offset: 0x0010F7FF
	public void SetPreferredItemSlot(int _slot, ItemValue _itemValue)
	{
		if (_slot >= 0 && _slot < this.preferredItemSlots.Length)
		{
			this.preferredItemSlots[_slot] = _itemValue.type;
		}
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x00111620 File Offset: 0x0010F820
	public int PreferredItemSlot(ItemValue _itemValue)
	{
		for (int i = 0; i < this.preferredItemSlots.Length; i++)
		{
			if (_itemValue.type == this.preferredItemSlots[i])
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x00111654 File Offset: 0x0010F854
	public int PreferredItemSlot(ItemStack _itemStack)
	{
		for (int i = 0; i < this.preferredItemSlots.Length; i++)
		{
			if (_itemStack.itemValue.type == this.preferredItemSlots[i])
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x0011168C File Offset: 0x0010F88C
	public void Write(PooledBinaryWriter writer, StreamModeWrite _eStreamMode)
	{
		writer.Write(6);
		this.itemGrid.Write(writer, _eStreamMode);
		for (int i = 0; i < this.m_cosmeticSlots.Length; i++)
		{
			if (this.m_cosmeticSlots[i] == null)
			{
				writer.Write(0);
			}
			else
			{
				writer.Write(Equipment.CosmeticMappingStringID[this.m_cosmeticSlots[i].GetItemName()]);
			}
		}
		writer.Write(this.m_unlockedCosmetics.Count);
		for (int j = 0; j < this.m_unlockedCosmetics.Count; j++)
		{
			writer.Write(this.m_unlockedCosmetics[j]);
		}
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x00111729 File Offset: 0x0010F929
	public static Equipment Read(PooledBinaryReader reader, StreamModeRead _eStreamMode)
	{
		Equipment equipment = new Equipment();
		equipment.ReadInto(reader, _eStreamMode);
		return equipment;
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x00111738 File Offset: 0x0010F938
	public void ReadInto(PooledBinaryReader reader, StreamModeRead _eStreamMode)
	{
		int num = (int)reader.ReadByte();
		bool flag = this.m_entity;
		if (flag)
		{
			this.m_entity.IsEquipping = true;
			for (int i = 0; i < this.itemGrid.Length; i++)
			{
				this.TeardownSlot(i);
			}
		}
		int num2 = this.itemGrid.Length;
		if (num <= 2)
		{
			num2 = 5;
		}
		else if (num <= 3)
		{
			num2 = 8;
		}
		else if (num <= 5)
		{
			num2 = 12;
		}
		if (num >= 5)
		{
			this.itemGrid.ReadInto(reader, _eStreamMode);
			List<ItemStack> list = this.itemGrid.Resize(new Vector2i(13, 1));
			if (list != null)
			{
				Log.Warning(string.Format("[Equipment] Save holds {0} slot(s) past EquipmentSlots.Count; contents discarded.", list.Count));
			}
		}
		else
		{
			ItemStack[] array = new ItemStack[this.itemGrid.Length];
			for (int j = 0; j < num2; j++)
			{
				ItemValue itemValue = ItemValue.ReadOrNull(reader);
				if (itemValue != null && !itemValue.IsEmpty())
				{
					array[j] = new ItemStack(itemValue, 1);
				}
			}
			this.itemGrid.SetItems(array, false);
		}
		if (num >= 2)
		{
			for (int k = 0; k < num2; k++)
			{
				int num3 = reader.ReadInt32();
				if (num3 == 0)
				{
					this.m_cosmeticSlots[k] = null;
				}
				else
				{
					this.m_cosmeticSlots[k] = ItemClass.GetItemClass(Equipment.CosmeticMappingIDString[num3], false);
				}
			}
			this.m_unlockedCosmetics.Clear();
			int num4 = reader.ReadInt32();
			for (int l = 0; l < num4; l++)
			{
				this.m_unlockedCosmetics.Add(reader.ReadInt32());
			}
		}
		if (!flag)
		{
			return;
		}
		for (int m = 0; m < this.itemGrid.Length; m++)
		{
			this.BuildSlot(m);
		}
		this.OnEquipmentChanged();
		this.m_entity.IsEquipping = false;
		this.FireEventsForSetSlots();
		EModelSDCS emodelSDCS = this.m_entity.emodel as EModelSDCS;
		if (emodelSDCS != null)
		{
			emodelSDCS.UpdateEquipment();
		}
		this.FireEventsForChangedSlots();
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x00111920 File Offset: 0x0010FB20
	public virtual bool ReturnItem(ItemStack _itemStack)
	{
		int num = this.PreferredItemSlot(_itemStack);
		if (num < 0 || num >= this.itemGrid.Length)
		{
			return false;
		}
		if (this.itemGrid[num].IsEmpty())
		{
			this.SetSlotItem(num, _itemStack.itemValue);
			return true;
		}
		return false;
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x0011196C File Offset: 0x0010FB6C
	public void InitializeEquipmentTransforms()
	{
		this.FireEventsForSlots(MinEventTypes.onSelfEquipStop, -1);
		this.FireEventsForSlots(MinEventTypes.onSelfEquipStart, -1);
		this.slotsSetFlags = 0;
		this.slotsChangedFlags = -1;
		this.FireEventsForChangedSlots();
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x00111994 File Offset: 0x0010FB94
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddArmorGroup(string armorGroup, int quality)
	{
		if (this.ArmorGroupEquipped.ContainsKey(armorGroup))
		{
			Equipment.ArmorGroupInfo armorGroupInfo = this.ArmorGroupEquipped[armorGroup];
			armorGroupInfo.Count++;
			if (armorGroupInfo.LowestQuality > quality)
			{
				armorGroupInfo.LowestQuality = quality;
				return;
			}
		}
		else
		{
			this.ArmorGroupEquipped[armorGroup] = new Equipment.ArmorGroupInfo
			{
				Count = 1,
				LowestQuality = quality
			};
		}
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x001119FC File Offset: 0x0010FBFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetArmorGroups()
	{
		this.ArmorGroupEquipped.Clear();
		for (int i = 0; i < this.itemGrid.Length; i++)
		{
			ItemValue itemValue = this.itemGrid[i].itemValue;
			if (!itemValue.IsEmpty())
			{
				ItemClassArmor itemClassArmor = itemValue.ItemClass as ItemClassArmor;
				if (itemClassArmor != null)
				{
					for (int j = 0; j < itemClassArmor.ArmorGroup.Length; j++)
					{
						this.AddArmorGroup(itemClassArmor.ArmorGroup[j], (int)itemValue.Quality);
					}
				}
			}
		}
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x00111A7A File Offset: 0x0010FC7A
	public int GetArmorGroupCount(string armorGroup)
	{
		if (this.ArmorGroupEquipped.ContainsKey(armorGroup))
		{
			return this.ArmorGroupEquipped[armorGroup].Count;
		}
		return 0;
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x00111A9D File Offset: 0x0010FC9D
	public int GetArmorGroupLowestQuality(string armorGroup)
	{
		if (this.ArmorGroupEquipped.ContainsKey(armorGroup))
		{
			return this.ArmorGroupEquipped[armorGroup].LowestQuality;
		}
		return 0;
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x00111AC0 File Offset: 0x0010FCC0
	public static void AddCosmeticMapping(int ID, string name)
	{
		Equipment.CosmeticMappingIDString.Add(ID, name);
		Equipment.CosmeticMappingStringID.Add(name, ID);
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x00111ADC File Offset: 0x0010FCDC
	public static void SetupCosmeticMapping()
	{
		Equipment.CosmeticMappingIDString.Clear();
		Equipment.CosmeticMappingStringID.Clear();
		Equipment.AddCosmeticMapping(-1, "missingItem");
		Equipment.AddCosmeticMapping(1, "armorPrimitiveHelmet");
		Equipment.AddCosmeticMapping(2, "armorPrimitiveOutfit");
		Equipment.AddCosmeticMapping(3, "armorPrimitiveGloves");
		Equipment.AddCosmeticMapping(4, "armorPrimitiveBoots");
		Equipment.AddCosmeticMapping(5, "armorLumberjackHelmet");
		Equipment.AddCosmeticMapping(6, "armorLumberjackOutfit");
		Equipment.AddCosmeticMapping(7, "armorLumberjackGloves");
		Equipment.AddCosmeticMapping(8, "armorLumberjackBoots");
		Equipment.AddCosmeticMapping(9, "armorPreacherHelmet");
		Equipment.AddCosmeticMapping(10, "armorPreacherOutfit");
		Equipment.AddCosmeticMapping(11, "armorPreacherGloves");
		Equipment.AddCosmeticMapping(12, "armorPreacherBoots");
		Equipment.AddCosmeticMapping(13, "armorRogueHelmet");
		Equipment.AddCosmeticMapping(14, "armorRogueOutfit");
		Equipment.AddCosmeticMapping(15, "armorRogueGloves");
		Equipment.AddCosmeticMapping(16, "armorRogueBoots");
		Equipment.AddCosmeticMapping(17, "armorAthleticHelmet");
		Equipment.AddCosmeticMapping(18, "armorAthleticOutfit");
		Equipment.AddCosmeticMapping(19, "armorAthleticGloves");
		Equipment.AddCosmeticMapping(20, "armorAthleticBoots");
		Equipment.AddCosmeticMapping(21, "armorEnforcerHelmet");
		Equipment.AddCosmeticMapping(22, "armorEnforcerOutfit");
		Equipment.AddCosmeticMapping(23, "armorEnforcerGloves");
		Equipment.AddCosmeticMapping(24, "armorEnforcerBoots");
		Equipment.AddCosmeticMapping(25, "armorFarmerHelmet");
		Equipment.AddCosmeticMapping(26, "armorFarmerOutfit");
		Equipment.AddCosmeticMapping(27, "armorFarmerGloves");
		Equipment.AddCosmeticMapping(28, "armorFarmerBoots");
		Equipment.AddCosmeticMapping(29, "armorBikerHelmet");
		Equipment.AddCosmeticMapping(30, "armorBikerOutfit");
		Equipment.AddCosmeticMapping(31, "armorBikerGloves");
		Equipment.AddCosmeticMapping(32, "armorBikerBoots");
		Equipment.AddCosmeticMapping(33, "armorScavengerHelmet");
		Equipment.AddCosmeticMapping(34, "armorScavengerOutfit");
		Equipment.AddCosmeticMapping(35, "armorScavengerGloves");
		Equipment.AddCosmeticMapping(36, "armorScavengerBoots");
		Equipment.AddCosmeticMapping(37, "armorRangerHelmet");
		Equipment.AddCosmeticMapping(38, "armorRangerOutfit");
		Equipment.AddCosmeticMapping(39, "armorRangerGloves");
		Equipment.AddCosmeticMapping(40, "armorRangerBoots");
		Equipment.AddCosmeticMapping(41, "armorCommandoHelmet");
		Equipment.AddCosmeticMapping(42, "armorCommandoOutfit");
		Equipment.AddCosmeticMapping(43, "armorCommandoGloves");
		Equipment.AddCosmeticMapping(44, "armorCommandoBoots");
		Equipment.AddCosmeticMapping(45, "armorAssassinHelmet");
		Equipment.AddCosmeticMapping(46, "armorAssassinOutfit");
		Equipment.AddCosmeticMapping(47, "armorAssassinGloves");
		Equipment.AddCosmeticMapping(48, "armorAssassinBoots");
		Equipment.AddCosmeticMapping(49, "armorMinerHelmet");
		Equipment.AddCosmeticMapping(50, "armorMinerOutfit");
		Equipment.AddCosmeticMapping(51, "armorMinerGloves");
		Equipment.AddCosmeticMapping(52, "armorMinerBoots");
		Equipment.AddCosmeticMapping(53, "armorNomadHelmet");
		Equipment.AddCosmeticMapping(54, "armorNomadOutfit");
		Equipment.AddCosmeticMapping(55, "armorNomadGloves");
		Equipment.AddCosmeticMapping(56, "armorNomadBoots");
		Equipment.AddCosmeticMapping(57, "armorNerdHelmet");
		Equipment.AddCosmeticMapping(58, "armorNerdOutfit");
		Equipment.AddCosmeticMapping(59, "armorNerdGloves");
		Equipment.AddCosmeticMapping(60, "armorNerdBoots");
		Equipment.AddCosmeticMapping(61, "armorRaiderHelmet");
		Equipment.AddCosmeticMapping(62, "armorRaiderOutfit");
		Equipment.AddCosmeticMapping(63, "armorRaiderGloves");
		Equipment.AddCosmeticMapping(64, "armorRaiderBoots");
		Equipment.AddCosmeticMapping(65, "armorDesertHelmet");
		Equipment.AddCosmeticMapping(66, "armorDesertOutfit");
		Equipment.AddCosmeticMapping(67, "armorDesertGloves");
		Equipment.AddCosmeticMapping(68, "armorDesertBoots");
		Equipment.AddCosmeticMapping(69, "armorHoarderHelmet");
		Equipment.AddCosmeticMapping(70, "armorHoarderOutfit");
		Equipment.AddCosmeticMapping(71, "armorHoarderGloves");
		Equipment.AddCosmeticMapping(72, "armorHoarderBoots");
		Equipment.AddCosmeticMapping(73, "armorMarauderHelmet");
		Equipment.AddCosmeticMapping(74, "armorMarauderOutfit");
		Equipment.AddCosmeticMapping(75, "armorMarauderGloves");
		Equipment.AddCosmeticMapping(76, "armorMarauderBoots");
		Equipment.AddCosmeticMapping(77, "armorCrimsonWarlordHelmet");
		Equipment.AddCosmeticMapping(78, "armorCrimsonWarlordOutfit");
		Equipment.AddCosmeticMapping(79, "armorCrimsonWarlordGloves");
		Equipment.AddCosmeticMapping(80, "armorCrimsonWarlordBoots");
		Equipment.AddCosmeticMapping(81, "armorSamuraiHelmet");
		Equipment.AddCosmeticMapping(82, "armorSamuraiOutfit");
		Equipment.AddCosmeticMapping(83, "armorSamuraiGloves");
		Equipment.AddCosmeticMapping(84, "armorSamuraiBoots");
		Equipment.AddCosmeticMapping(85, "armorPimpHatBlueHelmet");
		Equipment.AddCosmeticMapping(86, "armorPimpHatPurpleHelmet");
		Equipment.AddCosmeticMapping(87, "armorWatcherHelmet");
		Equipment.AddCosmeticMapping(88, "armorWatcherOutfit");
		Equipment.AddCosmeticMapping(89, "armorWatcherGloves");
		Equipment.AddCosmeticMapping(90, "armorWatcherBoots");
		Equipment.AddCosmeticMapping(91, "armorCrackABookHelmet");
		Equipment.AddCosmeticMapping(92, "armorCrackABookOutfit");
		Equipment.AddCosmeticMapping(93, "armorMoPowerHelmet");
		Equipment.AddCosmeticMapping(94, "armorMoPowerOutfit");
		Equipment.AddCosmeticMapping(95, "armorPassNGasHelmet");
		Equipment.AddCosmeticMapping(96, "armorPassNGasOutfit");
		Equipment.AddCosmeticMapping(97, "armorPopNPillsHelmet");
		Equipment.AddCosmeticMapping(98, "armorPopNPillsOutfit");
		Equipment.AddCosmeticMapping(99, "armorSavageCountryHelmet");
		Equipment.AddCosmeticMapping(100, "armorSavageCountryOutfit");
		Equipment.AddCosmeticMapping(101, "armorShamwayHelmet");
		Equipment.AddCosmeticMapping(102, "armorShamwayOutfit");
		Equipment.AddCosmeticMapping(103, "armorShotgunMessiahHelmet");
		Equipment.AddCosmeticMapping(104, "armorShotgunMessiahOutfit");
		Equipment.AddCosmeticMapping(105, "armorWorkingStiffsHelmet");
		Equipment.AddCosmeticMapping(106, "armorWorkingStiffsOutfit");
		Equipment.AddCosmeticMapping(107, "armorPirateHelmet");
		Equipment.AddCosmeticMapping(108, "armorPirateOutfit");
		Equipment.AddCosmeticMapping(109, "armorPirateGloves");
		Equipment.AddCosmeticMapping(110, "armorPirateBoots");
		Equipment.AddCosmeticMapping(111, "armorHellreaverHelmet");
		Equipment.AddCosmeticMapping(112, "armorHellreaverOutfit");
		Equipment.AddCosmeticMapping(113, "armorHellreaverGloves");
		Equipment.AddCosmeticMapping(114, "armorHellreaverBoots");
		Equipment.AddCosmeticMapping(115, "armorMonsterMaskHelmet");
		Equipment.AddCosmeticMapping(116, "armorVampireMaskHelmet");
		Equipment.AddCosmeticMapping(117, "armorWerewolfMaskHelmet");
		Equipment.AddCosmeticMapping(118, "armorZombieMaskHelmet");
		Equipment.AddCosmeticMapping(119, "armorInspectorMaskHelmet");
		Equipment.AddCosmeticMapping(120, "armorNobleThiefMaskHelmet");
		Equipment.AddCosmeticMapping(121, "armorPennyPincherMaskHelmet");
		Equipment.AddCosmeticMapping(122, "armorTheHatterMaskHelmet");
		Equipment.AddCosmeticMapping(123, "armorClassicSurvivorHelmet");
		Equipment.AddCosmeticMapping(124, "armorClassicSurvivorOutfit");
		Equipment.AddCosmeticMapping(125, "armorClassicSurvivorBoots");
		Equipment.AddCosmeticMapping(126, "armorSantaHatHelmet");
		Equipment.AddCosmeticMapping(127, "armorButcherHelmet");
		Equipment.AddCosmeticMapping(128, "armorButcherOutfit");
		Equipment.AddCosmeticMapping(129, "armorButcherGloves");
		Equipment.AddCosmeticMapping(130, "armorButcherBoots");
		Equipment.AddCosmeticMapping(131, "armorElfHatHelmet");
		Equipment.AddCosmeticMapping(132, "armorReindeerHatHelmet");
		Equipment.AddCosmeticMapping(133, "armorSnowmanHatHelmet");
		Equipment.AddCosmeticMapping(134, "armorTreeHatHelmet");
		Equipment.AddCosmeticMapping(135, "armorWinterHelmet");
		Equipment.AddCosmeticMapping(136, "armorWinterOutfit");
		Equipment.AddCosmeticMapping(137, "armorWinterGloves");
		Equipment.AddCosmeticMapping(138, "armorWinterBoots");
		Equipment.AddCosmeticMapping(139, "armorWorkingStiffHelmet");
		Equipment.AddCosmeticMapping(140, "armorWorkingStiffOutfit");
		Equipment.AddCosmeticMapping(141, "armorWorkingStiffGloves");
		Equipment.AddCosmeticMapping(142, "armorWorkingStiffBoots");
		Equipment.AddCosmeticMapping(143, "armorCasualOutfit");
		Equipment.AddCosmeticMapping(144, "armorCasualGloves");
		Equipment.AddCosmeticMapping(145, "armorCasualBoots");
		Equipment.AddCosmeticMapping(146, "armorBeachHelmet");
		Equipment.AddCosmeticMapping(147, "armorBeachOutfit");
		Equipment.AddCosmeticMapping(148, "armorBeachGloves");
		Equipment.AddCosmeticMapping(149, "armorBeachBoots");
		Equipment.AddCosmeticMapping(150, "armorBeachHelmet02");
		Equipment.AddCosmeticMapping(151, "armorBigbeakHelmet");
		Equipment.AddCosmeticMapping(152, "armorBigbeakOutfit");
		Equipment.AddCosmeticMapping(153, "armorBigbeakGloves");
		Equipment.AddCosmeticMapping(154, "armorBigbeakBoots");
		Equipment.AddCosmeticMapping(155, "armorKegBreakerHelmet");
		Equipment.AddCosmeticMapping(156, "armorKegBreakerOutfit");
		Equipment.AddCosmeticMapping(157, "armorKegBreakerGloves");
		Equipment.AddCosmeticMapping(158, "armorKegBreakerBoots");
		Equipment.AddCosmeticMapping(159, "armorWraithWoodWitchHelmet");
		Equipment.AddCosmeticMapping(160, "armorWraithWoodWitchOutfit");
		Equipment.AddCosmeticMapping(161, "armorWraithWoodWitchGloves");
		Equipment.AddCosmeticMapping(162, "armorWraithWoodWitchBoots");
	}

	// Token: 0x040020FD RID: 8445
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 6;

	// Token: 0x040020FF RID: 8447
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid itemGrid;

	// Token: 0x04002100 RID: 8448
	[PublicizedFrom(EAccessModifier.Private)]
	public int slotsSetFlags;

	// Token: 0x04002101 RID: 8449
	[PublicizedFrom(EAccessModifier.Private)]
	public int slotsChangedFlags;

	// Token: 0x04002102 RID: 8450
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] preferredItemSlots;

	// Token: 0x04002103 RID: 8451
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] m_cosmeticSlots;

	// Token: 0x04002104 RID: 8452
	public List<int> m_unlockedCosmetics = new List<int>();

	// Token: 0x04002105 RID: 8453
	public int tempCosmeticSlotIndex = -1;

	// Token: 0x04002106 RID: 8454
	public ItemClass tempCosmeticSlot;

	// Token: 0x04002107 RID: 8455
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive m_entity;

	// Token: 0x04002108 RID: 8456
	[PublicizedFrom(EAccessModifier.Private)]
	public float insulation;

	// Token: 0x04002109 RID: 8457
	[PublicizedFrom(EAccessModifier.Private)]
	public float waterProof;

	// Token: 0x0400210A RID: 8458
	public float CurrentLowestDurability = 1f;

	// Token: 0x0400210B RID: 8459
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, Equipment.ArmorGroupInfo> ArmorGroupEquipped = new Dictionary<string, Equipment.ArmorGroupInfo>();

	// Token: 0x0400210C RID: 8460
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastUpdateTime;

	// Token: 0x0400210E RID: 8462
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> physicalDamageTypes = FastTags<TagGroup.Global>.Parse("piercing,bashing,slashing,crushing,none,corrosive");

	// Token: 0x0400210F RID: 8463
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> coreDamageResist = FastTags<TagGroup.Global>.Parse("coredamageresist");

	// Token: 0x04002110 RID: 8464
	public static Dictionary<int, string> CosmeticMappingIDString = new Dictionary<int, string>();

	// Token: 0x04002111 RID: 8465
	public static Dictionary<string, int> CosmeticMappingStringID = new Dictionary<string, int>();

	// Token: 0x02000525 RID: 1317
	[PublicizedFrom(EAccessModifier.Private)]
	public class ArmorGroupInfo
	{
		// Token: 0x04002112 RID: 8466
		public int Count;

		// Token: 0x04002113 RID: 8467
		public int LowestQuality;
	}
}
