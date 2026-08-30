using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;

// Token: 0x02000519 RID: 1305
public class Equipment
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06002AD2 RID: 10962 RVA: 0x0010E9DC File Offset: 0x0010CBDC
	// (remove) Token: 0x06002AD3 RID: 10963 RVA: 0x0010EA14 File Offset: 0x0010CC14
	public event Action OnChanged;

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06002AD4 RID: 10964 RVA: 0x0010EA49 File Offset: 0x0010CC49
	// (set) Token: 0x06002AD5 RID: 10965 RVA: 0x0010EA51 File Offset: 0x0010CC51
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
	// (add) Token: 0x06002AD6 RID: 10966 RVA: 0x0010EA5C File Offset: 0x0010CC5C
	// (remove) Token: 0x06002AD7 RID: 10967 RVA: 0x0010EA94 File Offset: 0x0010CC94
	public event Equipment_CosmeticUnlocked CosmeticUnlocked;

	// Token: 0x06002AD8 RID: 10968 RVA: 0x0010EACC File Offset: 0x0010CCCC
	public Equipment()
	{
		int num = 12;
		this.m_slots = new ItemValue[num];
		this.CosmeticSlots = new ItemClass[num];
		this.preferredItemSlots = new int[num];
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x0010EB2E File Offset: 0x0010CD2E
	public Equipment(EntityAlive _entity)
		: this()
	{
		this.m_entity = _entity;
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x0010EB3D File Offset: 0x0010CD3D
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

	// Token: 0x06002ADB RID: 10971 RVA: 0x0010EB68 File Offset: 0x0010CD68
	public void SetCosmeticSlot(ItemClassArmor itemClass)
	{
		if (itemClass.EquipSlot < EquipmentSlots.BiomeBadge)
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

	// Token: 0x06002ADC RID: 10972 RVA: 0x0010EBEC File Offset: 0x0010CDEC
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
				if (itemClassArmor.EquipSlot < EquipmentSlots.BiomeBadge)
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

	// Token: 0x06002ADD RID: 10973 RVA: 0x0010ECBC File Offset: 0x0010CEBC
	public int[] GetCosmeticIDs()
	{
		int num = 12;
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

	// Token: 0x06002ADE RID: 10974 RVA: 0x0010ED0C File Offset: 0x0010CF0C
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

	// Token: 0x06002ADF RID: 10975 RVA: 0x0010ED5C File Offset: 0x0010CF5C
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

	// Token: 0x06002AE0 RID: 10976 RVA: 0x0010EDBC File Offset: 0x0010CFBC
	public void ModifyValue(ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags, bool _useDurability = false)
	{
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null && !itemValue.Equals(_originalItemValue) && itemValue.ItemClass != null)
			{
				itemValue.ModifyValue(this.m_entity, _originalItemValue, _passiveEffect, ref _base_val, ref _perc_val, tags, true, _useDurability);
			}
		}
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x0010EE10 File Offset: 0x0010D010
	public void GetModifiedValueData(List<EffectManager.ModifierValuesAndSources> _modValueSources, EffectManager.ModifierValuesAndSources.ValueSourceType _sourceType, ItemValue _originalItemValue, PassiveEffects _passiveEffect, ref float _base_val, ref float _perc_val, FastTags<TagGroup.Global> tags)
	{
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null && !itemValue.Equals(_originalItemValue) && itemValue.ItemClass != null)
			{
				itemValue.GetModifiedValueData(_modValueSources, _sourceType, this.m_entity, _originalItemValue, _passiveEffect, ref _base_val, ref _perc_val, tags);
			}
		}
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x0010EE64 File Offset: 0x0010D064
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

	// Token: 0x06002AE3 RID: 10979 RVA: 0x0010EEF4 File Offset: 0x0010D0F4
	public void FireEvent(MinEventTypes _eventType, MinEventParams _params)
	{
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null && itemValue.ItemClass != null)
			{
				itemValue.FireEvent(_eventType, _params);
			}
		}
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x0010EF30 File Offset: 0x0010D130
	public void DropItems()
	{
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null)
			{
				this.DropItemOnGround(itemValue);
				this.SetSlotItem(i, null, true);
			}
		}
		this.updateInsulation();
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x0010EF74 File Offset: 0x0010D174
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateInsulation()
	{
		this.waterProof = 0f;
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null)
			{
				this.waterProof += itemValue.ItemClass.WaterProof;
			}
		}
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x0010EFC4 File Offset: 0x0010D1C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DropItemOnGround(ItemValue _itemValue)
	{
		this.m_entity.world.GetGameManager().ItemDropServer(new ItemStack(_itemValue, 1), this.m_entity.GetPosition(), new Vector3(0.5f, 0f, 0.5f), this.m_entity.belongsPlayerId, 60f, false);
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x0010F01D File Offset: 0x0010D21D
	public float GetTotalInsulation()
	{
		return this.insulation;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x0010F025 File Offset: 0x0010D225
	public float GetTotalWaterproof()
	{
		return this.waterProof;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x0010F02D File Offset: 0x0010D22D
	public int GetSlotCount()
	{
		return this.m_slots.Length;
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x0010F037 File Offset: 0x0010D237
	public ItemValue[] GetItems()
	{
		return this.m_slots;
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x0010F03F File Offset: 0x0010D23F
	public ItemValue GetSlotItem(int index)
	{
		if (index >= this.m_slots.Length)
		{
			return null;
		}
		return this.m_slots[index];
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x0010F058 File Offset: 0x0010D258
	public ItemValue GetSlotItemOrNone(int index)
	{
		if (index >= this.m_slots.Length)
		{
			return ItemValue.None;
		}
		ItemValue itemValue = this.m_slots[index];
		if (itemValue == null)
		{
			return ItemValue.None;
		}
		return itemValue;
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x0010F08C File Offset: 0x0010D28C
	public bool HasAnyItems()
	{
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			if (this.m_slots[i] != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x0010F0BC File Offset: 0x0010D2BC
	public void PerformActionOnSlots(Action<ItemValue> _action)
	{
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null)
			{
				_action(itemValue);
			}
		}
	}

	// Token: 0x06002AEF RID: 10991 RVA: 0x0010F0EF File Offset: 0x0010D2EF
	public void SetTempCosmeticSlot(int index, ItemClass itemClass)
	{
		this.tempCosmeticSlotIndex = index;
		this.tempCosmeticSlot = itemClass;
	}

	// Token: 0x06002AF0 RID: 10992 RVA: 0x0010F0FF File Offset: 0x0010D2FF
	public void ClearTempCosmeticSlot()
	{
		this.tempCosmeticSlotIndex = -1;
		this.tempCosmeticSlot = null;
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x0010F110 File Offset: 0x0010D310
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

	// Token: 0x06002AF2 RID: 10994 RVA: 0x0010F160 File Offset: 0x0010D360
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

	// Token: 0x06002AF3 RID: 10995 RVA: 0x0010F210 File Offset: 0x0010D410
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

	// Token: 0x06002AF4 RID: 10996 RVA: 0x0010F278 File Offset: 0x0010D478
	public List<ItemValue> GetArmor()
	{
		List<ItemValue> list = new List<ItemValue>();
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null)
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

	// Token: 0x06002AF5 RID: 10997 RVA: 0x0010F2E4 File Offset: 0x0010D4E4
	public bool CheckBreakUseItems()
	{
		bool flag = false;
		this.CurrentLowestDurability = 1f;
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null)
			{
				ItemClass forId = ItemClass.GetForId(itemValue.type);
				float percentUsesLeft = itemValue.PercentUsesLeft;
				if (percentUsesLeft < this.CurrentLowestDurability)
				{
					this.CurrentLowestDurability = percentUsesLeft;
				}
				if (forId != null && itemValue.MaxUseTimes > 0 && forId.MaxUseTimesBreaksAfter.Value && itemValue.UseTimes > (float)itemValue.MaxUseTimes)
				{
					this.SetSlotItem(i, null, true);
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

	// Token: 0x06002AF6 RID: 10998 RVA: 0x0010F3CC File Offset: 0x0010D5CC
	public void SetSlotItem(int index, ItemValue value, bool isLocal = true)
	{
		if (value != null && value.IsEmpty())
		{
			value = null;
		}
		ItemValue itemValue = this.m_slots[index];
		if (value == null && itemValue == null)
		{
			return;
		}
		this.m_entity.IsEquipping = true;
		bool flag = false;
		if (itemValue != null && itemValue.Equals(value))
		{
			this.m_slots[index] = value;
			this.m_entity.MinEventContext.ItemValue = value;
			value.FireEvent(MinEventTypes.onSelfEquipStart, this.m_entity.MinEventContext);
		}
		else
		{
			flag = true;
			if (itemValue != null)
			{
				if (itemValue.ItemClass.HasTrigger(MinEventTypes.onSelfItemActivate) && itemValue.Activated != 0)
				{
					this.m_entity.MinEventContext.ItemValue = itemValue;
					itemValue.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
					itemValue.Activated = 0;
				}
				for (int i = 0; i < itemValue.Modifications.Length; i++)
				{
					ItemValue itemValue2 = itemValue.Modifications[i];
					if (itemValue2 != null)
					{
						ItemClass itemClass = itemValue2.ItemClass;
						if (itemClass != null && itemClass.HasTrigger(MinEventTypes.onSelfItemActivate) && itemValue2.Activated != 0)
						{
							this.m_entity.MinEventContext.ItemValue = itemValue2;
							itemValue2.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
							itemValue2.Activated = 0;
						}
					}
				}
				this.m_entity.MinEventContext.ItemValue = itemValue;
				itemValue.FireEvent(MinEventTypes.onSelfEquipStop, this.m_entity.MinEventContext);
			}
			this.preferredItemSlots[index] = ((value == null) ? 0 : value.type);
			this.m_slots[index] = value;
			this.slotsSetFlags |= 1 << index;
			this.slotsChangedFlags |= 1 << index;
		}
		if (flag)
		{
			if (this.m_entity && !this.m_entity.isEntityRemote)
			{
				this.m_entity.bPlayerEquipmentChanged = true;
			}
			this.ResetArmorGroups();
			if (this.OnChanged != null)
			{
				this.OnChanged();
			}
		}
		this.m_entity.IsEquipping = false;
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x0010F5AB File Offset: 0x0010D7AB
	public void SetSlotItemRaw(int index, ItemValue _iv)
	{
		if (_iv != null && _iv.IsEmpty())
		{
			_iv = null;
		}
		this.m_slots[index] = _iv;
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x0010F5C4 File Offset: 0x0010D7C4
	public void FireEventsForSlots(MinEventTypes _event, int _flags = -1)
	{
		this.m_entity.MinEventContext.Self = this.m_entity;
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			if ((_flags & (1 << i)) > 0)
			{
				ItemValue itemValue = this.m_slots[i];
				if (itemValue != null && itemValue.ItemClass != null)
				{
					this.m_entity.MinEventContext.ItemValue = itemValue;
					itemValue.FireEvent(_event, this.m_entity.MinEventContext);
				}
			}
		}
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x0010F63C File Offset: 0x0010D83C
	public void FireEventsForSetSlots()
	{
		this.FireEventsForSlots(MinEventTypes.onSelfEquipStart, this.slotsSetFlags);
		this.slotsSetFlags = 0;
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x0010F654 File Offset: 0x0010D854
	public void FireEventsForChangedSlots()
	{
		if (this.slotsChangedFlags == 0)
		{
			return;
		}
		this.m_entity.IsEquipping = true;
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			if ((this.slotsChangedFlags & (1 << i)) > 0)
			{
				ItemValue itemValue = this.m_slots[i];
				if (itemValue != null)
				{
					ItemClass itemClass = itemValue.ItemClass;
					if (itemClass != null)
					{
						this.m_entity.MinEventContext.Self = this.m_entity;
						this.m_entity.MinEventContext.ItemValue = itemValue;
						itemValue.FireEvent(MinEventTypes.onSelfEquipChanged, this.m_entity.MinEventContext);
						if (itemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
						{
							if (itemValue.Activated == 0)
							{
								itemValue.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
							}
							else
							{
								itemValue.FireEvent(MinEventTypes.onSelfItemActivate, this.m_entity.MinEventContext);
							}
						}
						for (int j = 0; j < itemValue.Modifications.Length; j++)
						{
							ItemValue itemValue2 = itemValue.Modifications[j];
							if (itemValue2 != null && itemValue2.ItemClass != null && itemValue2.ItemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
							{
								this.m_entity.MinEventContext.ItemValue = itemValue2;
								if (itemValue2.Activated == 0)
								{
									itemValue2.FireEvent(MinEventTypes.onSelfItemDeactivate, this.m_entity.MinEventContext);
								}
								else
								{
									itemValue2.FireEvent(MinEventTypes.onSelfItemActivate, this.m_entity.MinEventContext);
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

	// Token: 0x06002AFB RID: 11003 RVA: 0x0010F7C4 File Offset: 0x0010D9C4
	public void Update()
	{
		if (Time.time - this.lastUpdateTime >= 1f)
		{
			for (int i = 0; i < this.m_slots.Length; i++)
			{
				ItemValue itemValue = this.m_slots[i];
				if (itemValue != null)
				{
					this.m_entity.MinEventContext.ItemValue = itemValue;
					itemValue.FireEvent(MinEventTypes.onSelfEquipUpdate, this.m_entity.MinEventContext);
				}
			}
			this.lastUpdateTime = Time.time;
		}
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x0010F832 File Offset: 0x0010DA32
	public void SetPreferredItemSlot(int _slot, ItemValue _itemValue)
	{
		if (_slot >= 0 && _slot < this.preferredItemSlots.Length)
		{
			this.preferredItemSlots[_slot] = _itemValue.type;
		}
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x0010F854 File Offset: 0x0010DA54
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

	// Token: 0x06002AFE RID: 11006 RVA: 0x0010F888 File Offset: 0x0010DA88
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

	// Token: 0x06002AFF RID: 11007 RVA: 0x0010F8C0 File Offset: 0x0010DAC0
	public void Write(BinaryWriter writer)
	{
		writer.Write(4);
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue.Write(this.m_slots[i], writer);
		}
		for (int j = 0; j < this.m_cosmeticSlots.Length; j++)
		{
			if (this.m_cosmeticSlots[j] == null)
			{
				writer.Write(0);
			}
			else
			{
				writer.Write(Equipment.CosmeticMappingStringID[this.m_cosmeticSlots[j].GetItemName()]);
			}
		}
		writer.Write(this.m_unlockedCosmetics.Count);
		for (int k = 0; k < this.m_unlockedCosmetics.Count; k++)
		{
			writer.Write(this.m_unlockedCosmetics[k]);
		}
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x0010F974 File Offset: 0x0010DB74
	public static Equipment Read(BinaryReader reader)
	{
		int num = (int)reader.ReadByte();
		Equipment equipment = new Equipment();
		int num2 = equipment.m_slots.Length;
		if (num <= 2)
		{
			num2 = 5;
		}
		else if (num <= 3)
		{
			num2 = 8;
		}
		for (int i = 0; i < num2; i++)
		{
			equipment.m_slots[i] = ItemValue.ReadOrNull(reader);
		}
		if (num >= 2)
		{
			for (int j = 0; j < num2; j++)
			{
				int num3 = reader.ReadInt32();
				if (num3 == 0)
				{
					equipment.m_cosmeticSlots[j] = null;
				}
				else
				{
					equipment.m_cosmeticSlots[j] = ItemClass.GetItemClass(Equipment.CosmeticMappingIDString[num3], false);
				}
			}
			equipment.m_unlockedCosmetics.Clear();
			int num4 = reader.ReadInt32();
			for (int k = 0; k < num4; k++)
			{
				equipment.m_unlockedCosmetics.Add(reader.ReadInt32());
			}
		}
		return equipment;
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x0010FA3C File Offset: 0x0010DC3C
	public virtual bool ReturnItem(ItemStack _itemStack, bool isLocal = true)
	{
		int num = this.PreferredItemSlot(_itemStack);
		if (num < 0 || num >= this.m_slots.Length)
		{
			return false;
		}
		if (this.m_slots[num] == null)
		{
			this.SetSlotItem(num, _itemStack.itemValue, isLocal);
			return true;
		}
		return false;
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x0010FA80 File Offset: 0x0010DC80
	public void Apply(Equipment eq, bool isLocal = true)
	{
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			this.SetSlotItem(i, eq.m_slots[i], isLocal);
		}
		for (int j = 0; j < this.m_cosmeticSlots.Length; j++)
		{
			this.m_cosmeticSlots[j] = eq.m_cosmeticSlots[j];
		}
		this.m_unlockedCosmetics.Clear();
		for (int k = 0; k < eq.m_unlockedCosmetics.Count; k++)
		{
			this.m_unlockedCosmetics.Add(eq.m_unlockedCosmetics[k]);
		}
		this.FireEventsForSetSlots();
		EModelSDCS emodelSDCS = this.m_entity.emodel as EModelSDCS;
		if (emodelSDCS != null)
		{
			emodelSDCS.UpdateEquipment();
		}
		this.FireEventsForChangedSlots();
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x0010FB31 File Offset: 0x0010DD31
	public void InitializeEquipmentTransforms()
	{
		this.FireEventsForSlots(MinEventTypes.onSelfEquipStop, -1);
		this.FireEventsForSlots(MinEventTypes.onSelfEquipStart, -1);
		this.slotsSetFlags = 0;
		this.slotsChangedFlags = -1;
		this.FireEventsForChangedSlots();
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x0010FB5C File Offset: 0x0010DD5C
	public Equipment Clone()
	{
		Equipment equipment = new Equipment();
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			if (this.m_slots[i] != null)
			{
				equipment.m_slots[i] = this.m_slots[i].Clone();
			}
		}
		for (int j = 0; j < this.m_cosmeticSlots.Length; j++)
		{
			ItemClass itemClass = this.m_cosmeticSlots[j];
			equipment.m_cosmeticSlots[j] = this.m_cosmeticSlots[j];
		}
		for (int k = 0; k < this.m_unlockedCosmetics.Count; k++)
		{
			if (equipment.m_unlockedCosmetics == null)
			{
				equipment.m_unlockedCosmetics = new List<int>();
			}
			equipment.m_unlockedCosmetics.Add(this.m_unlockedCosmetics[k]);
		}
		return equipment;
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x0010FC10 File Offset: 0x0010DE10
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

	// Token: 0x06002B06 RID: 11014 RVA: 0x0010FC78 File Offset: 0x0010DE78
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetArmorGroups()
	{
		this.ArmorGroupEquipped.Clear();
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			ItemValue itemValue = this.m_slots[i];
			if (itemValue != null)
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

	// Token: 0x06002B07 RID: 11015 RVA: 0x0010FCE5 File Offset: 0x0010DEE5
	public int GetArmorGroupCount(string armorGroup)
	{
		if (this.ArmorGroupEquipped.ContainsKey(armorGroup))
		{
			return this.ArmorGroupEquipped[armorGroup].Count;
		}
		return 0;
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x0010FD08 File Offset: 0x0010DF08
	public int GetArmorGroupLowestQuality(string armorGroup)
	{
		if (this.ArmorGroupEquipped.ContainsKey(armorGroup))
		{
			return this.ArmorGroupEquipped[armorGroup].LowestQuality;
		}
		return 0;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x0010FD2B File Offset: 0x0010DF2B
	public static void AddCosmeticMapping(int ID, string name)
	{
		Equipment.CosmeticMappingIDString.Add(ID, name);
		Equipment.CosmeticMappingStringID.Add(name, ID);
	}

	// Token: 0x06002B0A RID: 11018 RVA: 0x0010FD48 File Offset: 0x0010DF48
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
	}

	// Token: 0x040020C6 RID: 8390
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 4;

	// Token: 0x040020C8 RID: 8392
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue[] m_slots;

	// Token: 0x040020C9 RID: 8393
	[PublicizedFrom(EAccessModifier.Private)]
	public int slotsSetFlags;

	// Token: 0x040020CA RID: 8394
	[PublicizedFrom(EAccessModifier.Private)]
	public int slotsChangedFlags;

	// Token: 0x040020CB RID: 8395
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] preferredItemSlots;

	// Token: 0x040020CC RID: 8396
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] m_cosmeticSlots;

	// Token: 0x040020CD RID: 8397
	public List<int> m_unlockedCosmetics = new List<int>();

	// Token: 0x040020CE RID: 8398
	public int tempCosmeticSlotIndex = -1;

	// Token: 0x040020CF RID: 8399
	public ItemClass tempCosmeticSlot;

	// Token: 0x040020D0 RID: 8400
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive m_entity;

	// Token: 0x040020D1 RID: 8401
	[PublicizedFrom(EAccessModifier.Private)]
	public float insulation;

	// Token: 0x040020D2 RID: 8402
	[PublicizedFrom(EAccessModifier.Private)]
	public float waterProof;

	// Token: 0x040020D3 RID: 8403
	public float CurrentLowestDurability = 1f;

	// Token: 0x040020D4 RID: 8404
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, Equipment.ArmorGroupInfo> ArmorGroupEquipped = new Dictionary<string, Equipment.ArmorGroupInfo>();

	// Token: 0x040020D5 RID: 8405
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastUpdateTime;

	// Token: 0x040020D7 RID: 8407
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> physicalDamageTypes = FastTags<TagGroup.Global>.Parse("piercing,bashing,slashing,crushing,none,corrosive");

	// Token: 0x040020D8 RID: 8408
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> coreDamageResist = FastTags<TagGroup.Global>.Parse("coredamageresist");

	// Token: 0x040020D9 RID: 8409
	public static Dictionary<int, string> CosmeticMappingIDString = new Dictionary<int, string>();

	// Token: 0x040020DA RID: 8410
	public static Dictionary<string, int> CosmeticMappingStringID = new Dictionary<string, int>();

	// Token: 0x0200051A RID: 1306
	[PublicizedFrom(EAccessModifier.Private)]
	public class ArmorGroupInfo
	{
		// Token: 0x040020DB RID: 8411
		public int Count;

		// Token: 0x040020DC RID: 8412
		public int LowestQuality;
	}
}
