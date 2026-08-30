using System;

// Token: 0x02001114 RID: 4372
public class XUiM_PlayerDefense : XUiModel
{
	// Token: 0x06008A11 RID: 35345 RVA: 0x0034AEF8 File Offset: 0x003490F8
	public static string GetBashing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Bashing) + "%";
	}

	// Token: 0x06008A12 RID: 35346 RVA: 0x0034AF0B File Offset: 0x0034910B
	public static string GetPiercing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Piercing) + "%";
	}

	// Token: 0x06008A13 RID: 35347 RVA: 0x0034AF1E File Offset: 0x0034911E
	public static string GetRadiation(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Radiation) + "%";
	}

	// Token: 0x06008A14 RID: 35348 RVA: 0x0034AF31 File Offset: 0x00349131
	public static string GetWaterproof(EntityPlayer _player)
	{
		return string.Format("{0}%", (int)(_player.equipment.GetTotalWaterproof() * 100f));
	}

	// Token: 0x06008A15 RID: 35349 RVA: 0x0034AF54 File Offset: 0x00349154
	public static string GetFireproof(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Heat) + "%";
	}

	// Token: 0x06008A16 RID: 35350 RVA: 0x0034AF67 File Offset: 0x00349167
	public static string GetElectrical(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Electrical) + "%";
	}

	// Token: 0x06008A17 RID: 35351 RVA: 0x0034AF7B File Offset: 0x0034917B
	public static string GetInsulation(EntityPlayer _player)
	{
		return ValueDisplayFormatters.TemperatureRelative(_player.equipment.GetTotalInsulation(), 0);
	}

	// Token: 0x06008A18 RID: 35352 RVA: 0x0034AF90 File Offset: 0x00349190
	public static string GetWeight(EntityPlayer _player)
	{
		float num = 0f;
		int slotCount = _player.equipment.GetSlotCount();
		for (int i = 0; i < slotCount; i++)
		{
			ItemValue slotItem = _player.equipment.GetSlotItem(i);
			ItemClass itemClass = ((slotItem != null) ? slotItem.ItemClass : null);
			if (itemClass != null)
			{
				num += itemClass.Encumbrance * 100f;
			}
		}
		return num.ToCultureInvariantString() + "%";
	}

	// Token: 0x06008A19 RID: 35353 RVA: 0x0034AFF8 File Offset: 0x003491F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetDefenseFromPlayer(EntityPlayer _player, EnumDamageTypes _armorType)
	{
		float num = 0f;
		if (_armorType != EnumDamageTypes.None && _armorType < EnumDamageTypes.Disease)
		{
			if (_armorType <= EnumDamageTypes.Crushing)
			{
				num = EffectManager.GetValue(PassiveEffects.PhysicalDamageResist, null, 0f, _player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			}
			else if (_armorType <= EnumDamageTypes.Electrical)
			{
				num = EffectManager.GetValue(PassiveEffects.ElementalDamageResist, null, 0f, _player, null, FastTags<TagGroup.Global>.Parse(_armorType.ToStringCached<EnumDamageTypes>()), true, true, true, true, true, 1, true, false);
			}
		}
		return num.ToCultureInvariantString("00");
	}

	// Token: 0x06008A1A RID: 35354 RVA: 0x0034B06D File Offset: 0x0034926D
	public static string GetBashing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Bashing, _xui) + "%";
	}

	// Token: 0x06008A1B RID: 35355 RVA: 0x0034B081 File Offset: 0x00349281
	public static string GetPiercing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Piercing, _xui) + "%";
	}

	// Token: 0x06008A1C RID: 35356 RVA: 0x0034B095 File Offset: 0x00349295
	public static string GetRadiation(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Radiation, _xui) + "%";
	}

	// Token: 0x06008A1D RID: 35357 RVA: 0x0034B0AC File Offset: 0x003492AC
	public static string GetWaterproof(ItemValue _itemValue)
	{
		float num = 0f;
		ItemClass itemClass = _itemValue.ItemClass;
		if (itemClass != null)
		{
			num = itemClass.WaterProof;
		}
		return string.Format("{0}%", (int)(num * 100f));
	}

	// Token: 0x06008A1E RID: 35358 RVA: 0x0034B0E7 File Offset: 0x003492E7
	public static string GetFireproof(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Heat, _xui) + "%";
	}

	// Token: 0x06008A1F RID: 35359 RVA: 0x0034B0FB File Offset: 0x003492FB
	public static string GetElectrical(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Electrical, _xui) + "%";
	}

	// Token: 0x06008A20 RID: 35360 RVA: 0x0034B110 File Offset: 0x00349310
	public static string GetInsulation(ItemValue _itemValue)
	{
		float num = 0f;
		ItemClass itemClass = _itemValue.ItemClass;
		if (itemClass != null)
		{
			num = itemClass.Insulation;
		}
		return ValueDisplayFormatters.TemperatureRelative(num, 0);
	}

	// Token: 0x06008A21 RID: 35361 RVA: 0x0034B13C File Offset: 0x0034933C
	public static string GetWeight(ItemValue _itemValue)
	{
		float num = 0f;
		ItemClass itemClass = _itemValue.ItemClass;
		if (itemClass != null)
		{
			num = itemClass.Encumbrance * 100f;
		}
		return num.ToCultureInvariantString() + "%";
	}

	// Token: 0x06008A22 RID: 35362 RVA: 0x0034B178 File Offset: 0x00349378
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetDefenseFromItemValue(ItemValue _itemValue, EnumDamageTypes _damageType, XUi _xui)
	{
		return (EffectManager.GetValue(PassiveEffects.PhysicalDamageResist, _itemValue, 0f, _xui.playerUI.entityPlayer, null, FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>()), true, true, true, true, true, 1, true, false) * 100f).ToCultureInvariantString("00");
	}
}
