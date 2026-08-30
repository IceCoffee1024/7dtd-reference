using System;

// Token: 0x0200112D RID: 4397
public class XUiM_PlayerDefense : XUiModel
{
	// Token: 0x06008B03 RID: 35587 RVA: 0x0034F71C File Offset: 0x0034D91C
	public static string GetBashing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Bashing) + "%";
	}

	// Token: 0x06008B04 RID: 35588 RVA: 0x0034F72F File Offset: 0x0034D92F
	public static string GetPiercing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Piercing) + "%";
	}

	// Token: 0x06008B05 RID: 35589 RVA: 0x0034F742 File Offset: 0x0034D942
	public static string GetRadiation(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Radiation) + "%";
	}

	// Token: 0x06008B06 RID: 35590 RVA: 0x0034F755 File Offset: 0x0034D955
	public static string GetWaterproof(EntityPlayer _player)
	{
		return string.Format("{0}%", (int)(_player.equipment.GetTotalWaterproof() * 100f));
	}

	// Token: 0x06008B07 RID: 35591 RVA: 0x0034F778 File Offset: 0x0034D978
	public static string GetFireproof(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Heat) + "%";
	}

	// Token: 0x06008B08 RID: 35592 RVA: 0x0034F78B File Offset: 0x0034D98B
	public static string GetElectrical(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Electrical) + "%";
	}

	// Token: 0x06008B09 RID: 35593 RVA: 0x0034F79F File Offset: 0x0034D99F
	public static string GetInsulation(EntityPlayer _player)
	{
		return ValueDisplayFormatters.TemperatureRelative(_player.equipment.GetTotalInsulation(), 0);
	}

	// Token: 0x06008B0A RID: 35594 RVA: 0x0034F7B4 File Offset: 0x0034D9B4
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

	// Token: 0x06008B0B RID: 35595 RVA: 0x0034F81C File Offset: 0x0034DA1C
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

	// Token: 0x06008B0C RID: 35596 RVA: 0x0034F891 File Offset: 0x0034DA91
	public static string GetBashing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Bashing, _xui) + "%";
	}

	// Token: 0x06008B0D RID: 35597 RVA: 0x0034F8A5 File Offset: 0x0034DAA5
	public static string GetPiercing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Piercing, _xui) + "%";
	}

	// Token: 0x06008B0E RID: 35598 RVA: 0x0034F8B9 File Offset: 0x0034DAB9
	public static string GetRadiation(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Radiation, _xui) + "%";
	}

	// Token: 0x06008B0F RID: 35599 RVA: 0x0034F8D0 File Offset: 0x0034DAD0
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

	// Token: 0x06008B10 RID: 35600 RVA: 0x0034F90B File Offset: 0x0034DB0B
	public static string GetFireproof(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Heat, _xui) + "%";
	}

	// Token: 0x06008B11 RID: 35601 RVA: 0x0034F91F File Offset: 0x0034DB1F
	public static string GetElectrical(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Electrical, _xui) + "%";
	}

	// Token: 0x06008B12 RID: 35602 RVA: 0x0034F934 File Offset: 0x0034DB34
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

	// Token: 0x06008B13 RID: 35603 RVA: 0x0034F960 File Offset: 0x0034DB60
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

	// Token: 0x06008B14 RID: 35604 RVA: 0x0034F99C File Offset: 0x0034DB9C
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetDefenseFromItemValue(ItemValue _itemValue, EnumDamageTypes _damageType, XUi _xui)
	{
		return (EffectManager.GetValue(PassiveEffects.PhysicalDamageResist, _itemValue, 0f, _xui.playerUI.entityPlayer, null, FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>()), true, true, true, true, true, 1, true, false) * 100f).ToCultureInvariantString("00");
	}
}
