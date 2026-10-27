using System;

// Token: 0x02001155 RID: 4437
public class XUiM_PlayerDefense : XUiModel
{
	// Token: 0x06008BC2 RID: 35778 RVA: 0x00355BE4 File Offset: 0x00353DE4
	public static string GetBashing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Bashing) + "%";
	}

	// Token: 0x06008BC3 RID: 35779 RVA: 0x00355BF7 File Offset: 0x00353DF7
	public static string GetPiercing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Piercing) + "%";
	}

	// Token: 0x06008BC4 RID: 35780 RVA: 0x00355C0A File Offset: 0x00353E0A
	public static string GetRadiation(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Radiation) + "%";
	}

	// Token: 0x06008BC5 RID: 35781 RVA: 0x00355C1D File Offset: 0x00353E1D
	public static string GetWaterproof(EntityPlayer _player)
	{
		return string.Format("{0}%", (int)(_player.equipment.GetTotalWaterproof() * 100f));
	}

	// Token: 0x06008BC6 RID: 35782 RVA: 0x00355C40 File Offset: 0x00353E40
	public static string GetFireproof(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Heat) + "%";
	}

	// Token: 0x06008BC7 RID: 35783 RVA: 0x00355C53 File Offset: 0x00353E53
	public static string GetElectrical(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Electrical) + "%";
	}

	// Token: 0x06008BC8 RID: 35784 RVA: 0x00355C67 File Offset: 0x00353E67
	public static string GetInsulation(EntityPlayer _player)
	{
		return ValueDisplayFormatters.TemperatureRelative(_player.equipment.GetTotalInsulation(), 0);
	}

	// Token: 0x06008BC9 RID: 35785 RVA: 0x00355C7C File Offset: 0x00353E7C
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

	// Token: 0x06008BCA RID: 35786 RVA: 0x00355CE4 File Offset: 0x00353EE4
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

	// Token: 0x06008BCB RID: 35787 RVA: 0x00355D59 File Offset: 0x00353F59
	public static string GetBashing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Bashing, _xui) + "%";
	}

	// Token: 0x06008BCC RID: 35788 RVA: 0x00355D6D File Offset: 0x00353F6D
	public static string GetPiercing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Piercing, _xui) + "%";
	}

	// Token: 0x06008BCD RID: 35789 RVA: 0x00355D81 File Offset: 0x00353F81
	public static string GetRadiation(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Radiation, _xui) + "%";
	}

	// Token: 0x06008BCE RID: 35790 RVA: 0x00355D98 File Offset: 0x00353F98
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

	// Token: 0x06008BCF RID: 35791 RVA: 0x00355DD3 File Offset: 0x00353FD3
	public static string GetFireproof(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Heat, _xui) + "%";
	}

	// Token: 0x06008BD0 RID: 35792 RVA: 0x00355DE7 File Offset: 0x00353FE7
	public static string GetElectrical(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Electrical, _xui) + "%";
	}

	// Token: 0x06008BD1 RID: 35793 RVA: 0x00355DFC File Offset: 0x00353FFC
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

	// Token: 0x06008BD2 RID: 35794 RVA: 0x00355E28 File Offset: 0x00354028
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

	// Token: 0x06008BD3 RID: 35795 RVA: 0x00355E64 File Offset: 0x00354064
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetDefenseFromItemValue(ItemValue _itemValue, EnumDamageTypes _damageType, XUi _xui)
	{
		return (EffectManager.GetValue(PassiveEffects.PhysicalDamageResist, _itemValue, 0f, _xui.playerUI.entityPlayer, null, FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>()), true, true, true, true, true, 1, true, false) * 100f).ToCultureInvariantString("00");
	}
}
