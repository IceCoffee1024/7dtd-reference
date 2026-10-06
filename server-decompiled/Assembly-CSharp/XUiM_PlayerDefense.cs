using System;

// Token: 0x02001145 RID: 4421
public class XUiM_PlayerDefense : XUiModel
{
	// Token: 0x06008B6E RID: 35694 RVA: 0x00353B34 File Offset: 0x00351D34
	public static string GetBashing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Bashing) + "%";
	}

	// Token: 0x06008B6F RID: 35695 RVA: 0x00353B47 File Offset: 0x00351D47
	public static string GetPiercing(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Piercing) + "%";
	}

	// Token: 0x06008B70 RID: 35696 RVA: 0x00353B5A File Offset: 0x00351D5A
	public static string GetRadiation(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Radiation) + "%";
	}

	// Token: 0x06008B71 RID: 35697 RVA: 0x00353B6D File Offset: 0x00351D6D
	public static string GetWaterproof(EntityPlayer _player)
	{
		return string.Format("{0}%", (int)(_player.equipment.GetTotalWaterproof() * 100f));
	}

	// Token: 0x06008B72 RID: 35698 RVA: 0x00353B90 File Offset: 0x00351D90
	public static string GetFireproof(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Heat) + "%";
	}

	// Token: 0x06008B73 RID: 35699 RVA: 0x00353BA3 File Offset: 0x00351DA3
	public static string GetElectrical(EntityPlayer _player)
	{
		return XUiM_PlayerDefense.GetDefenseFromPlayer(_player, EnumDamageTypes.Electrical) + "%";
	}

	// Token: 0x06008B74 RID: 35700 RVA: 0x00353BB7 File Offset: 0x00351DB7
	public static string GetInsulation(EntityPlayer _player)
	{
		return ValueDisplayFormatters.TemperatureRelative(_player.equipment.GetTotalInsulation(), 0);
	}

	// Token: 0x06008B75 RID: 35701 RVA: 0x00353BCC File Offset: 0x00351DCC
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

	// Token: 0x06008B76 RID: 35702 RVA: 0x00353C34 File Offset: 0x00351E34
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

	// Token: 0x06008B77 RID: 35703 RVA: 0x00353CA9 File Offset: 0x00351EA9
	public static string GetBashing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Bashing, _xui) + "%";
	}

	// Token: 0x06008B78 RID: 35704 RVA: 0x00353CBD File Offset: 0x00351EBD
	public static string GetPiercing(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Piercing, _xui) + "%";
	}

	// Token: 0x06008B79 RID: 35705 RVA: 0x00353CD1 File Offset: 0x00351ED1
	public static string GetRadiation(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Radiation, _xui) + "%";
	}

	// Token: 0x06008B7A RID: 35706 RVA: 0x00353CE8 File Offset: 0x00351EE8
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

	// Token: 0x06008B7B RID: 35707 RVA: 0x00353D23 File Offset: 0x00351F23
	public static string GetFireproof(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Heat, _xui) + "%";
	}

	// Token: 0x06008B7C RID: 35708 RVA: 0x00353D37 File Offset: 0x00351F37
	public static string GetElectrical(ItemValue _itemValue, XUi _xui)
	{
		return XUiM_PlayerDefense.GetDefenseFromItemValue(_itemValue, EnumDamageTypes.Electrical, _xui) + "%";
	}

	// Token: 0x06008B7D RID: 35709 RVA: 0x00353D4C File Offset: 0x00351F4C
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

	// Token: 0x06008B7E RID: 35710 RVA: 0x00353D78 File Offset: 0x00351F78
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

	// Token: 0x06008B7F RID: 35711 RVA: 0x00353DB4 File Offset: 0x00351FB4
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetDefenseFromItemValue(ItemValue _itemValue, EnumDamageTypes _damageType, XUi _xui)
	{
		return (EffectManager.GetValue(PassiveEffects.PhysicalDamageResist, _itemValue, 0f, _xui.playerUI.entityPlayer, null, FastTags<TagGroup.Global>.Parse(_damageType.ToStringCached<EnumDamageTypes>()), true, true, true, true, true, 1, true, false) * 100f).ToCultureInvariantString("00");
	}
}
