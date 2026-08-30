using System;
using Audio;

// Token: 0x02001142 RID: 4418
public class XUiM_Vehicle : XUiModel
{
	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x06008BAC RID: 35756 RVA: 0x0035291E File Offset: 0x00350B1E
	// (set) Token: 0x06008BAD RID: 35757 RVA: 0x00352926 File Offset: 0x00350B26
	public EntityVehicle CurrentVehicle { get; set; }

	// Token: 0x06008BAE RID: 35758 RVA: 0x0035292F File Offset: 0x00350B2F
	public static string GetEntityName(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return "";
		}
		return _xui.Vehicle.CurrentVehicle.EntityName;
	}

	// Token: 0x06008BAF RID: 35759 RVA: 0x0035295A File Offset: 0x00350B5A
	public static float GetSpeed(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().MaxPossibleSpeed;
	}

	// Token: 0x06008BB0 RID: 35760 RVA: 0x0035298C File Offset: 0x00350B8C
	public static string GetNoise(XUi _xui)
	{
		EntityVehicle currentVehicle = _xui.Vehicle.CurrentVehicle;
		if (currentVehicle == null)
		{
			return "";
		}
		float noise = currentVehicle.GetVehicle().GetNoise();
		string text;
		if (noise > 0.33f)
		{
			if (noise > 0.66f)
			{
				text = Localization.Get("xuiVehicleNoiseLoud", false, null);
			}
			else
			{
				text = Localization.Get("xuiVehicleNoiseModerate", false, null);
			}
		}
		else
		{
			text = Localization.Get("xuiVehicleNoiseSoft", false, null);
		}
		return text;
	}

	// Token: 0x06008BB1 RID: 35761 RVA: 0x00352A00 File Offset: 0x00350C00
	public static float GetProtection(XUi _xui)
	{
		EntityVehicle currentVehicle = _xui.Vehicle.CurrentVehicle;
		if (currentVehicle == null)
		{
			return 0f;
		}
		return (1f - currentVehicle.GetVehicle().GetPlayerDamagePercent()) * 100f;
	}

	// Token: 0x06008BB2 RID: 35762 RVA: 0x00352A3F File Offset: 0x00350C3F
	public static float GetFuelLevel(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent() * 100f;
	}

	// Token: 0x06008BB3 RID: 35763 RVA: 0x00352A75 File Offset: 0x00350C75
	public static float GetFuelFill(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent();
	}

	// Token: 0x06008BB4 RID: 35764 RVA: 0x00352AA5 File Offset: 0x00350CA5
	public static int GetPassengers(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 1;
		}
		return _xui.Vehicle.CurrentVehicle.GetAttachMaxCount();
	}

	// Token: 0x06008BB5 RID: 35765 RVA: 0x00352ACC File Offset: 0x00350CCC
	public static string GetSpeedText(XUi _xui)
	{
		float num = ((_xui.Vehicle.CurrentVehicle != null) ? _xui.Vehicle.CurrentVehicle.GetVehicle().MaxPossibleSpeed : 0f);
		string text;
		if (num <= 9f)
		{
			if (num > 0f)
			{
				text = Localization.Get("xuiVehicleSpeedSlow", false, null);
			}
			else
			{
				text = Localization.Get("xuiVehicleSpeedNone", false, null);
			}
		}
		else if (num > 12f)
		{
			text = Localization.Get("xuiVehicleSpeedFast", false, null);
		}
		else
		{
			text = Localization.Get("xuiVehicleSpeedNormal", false, null);
		}
		return text;
	}

	// Token: 0x06008BB6 RID: 35766 RVA: 0x00352B60 File Offset: 0x00350D60
	public bool SetPart(XUi _xui, string _vehicleSlotName, ItemStack _stack, out ItemStack _resultStack)
	{
		Log.Warning("XUiM_Vehicle SetPart {0}", new object[] { _vehicleSlotName });
		this.CurrentVehicle == null;
		_resultStack = _stack;
		return false;
	}

	// Token: 0x06008BB7 RID: 35767 RVA: 0x000027FC File Offset: 0x000009FC
	public void RefreshVehicle()
	{
	}

	// Token: 0x06008BB8 RID: 35768 RVA: 0x00352B88 File Offset: 0x00350D88
	public static bool RepairVehicle(XUi _xui, Vehicle _vehicle = null)
	{
		if (_vehicle == null)
		{
			_vehicle = _xui.Vehicle.CurrentVehicle.GetVehicle();
		}
		ItemValue item = ItemClass.GetItem("resourceRepairKit", false);
		if (item.ItemClass == null)
		{
			return false;
		}
		EntityPlayerLocal entityPlayer = _xui.playerUI.entityPlayer;
		LocalPlayerUI playerUI = _xui.playerUI;
		int itemCount = entityPlayer.bag.GetItemCount(item, -1, -1, true);
		int itemCount2 = entityPlayer.inventory.GetItemCount(item, false, -1, -1, true);
		int repairAmountNeeded = _vehicle.GetRepairAmountNeeded();
		if (itemCount + itemCount2 <= 0 || repairAmountNeeded <= 0)
		{
			if (repairAmountNeeded > itemCount + itemCount2)
			{
				Manager.PlayInsidePlayerHead("misc/missingitemtorepair", -1, 0f, false, false);
			}
			return false;
		}
		float num = 0f;
		ProgressionValue progressionValue = entityPlayer.Progression.GetProgressionValue("perkGreaseMonkey");
		if (progressionValue != null)
		{
			num += (float)progressionValue.Level * 0.1f;
		}
		_vehicle.RepairParts(1000, num);
		if (itemCount2 > 0)
		{
			entityPlayer.inventory.DecItem(item, 1, false, null);
		}
		else
		{
			entityPlayer.bag.DecItem(item, 1, false, null);
		}
		playerUI.xui.CollectedItemList.RemoveItemStack(new ItemStack(item, 1));
		Manager.PlayInsidePlayerHead("craft_complete_item", -1, 0f, false, false);
		return true;
	}

	// Token: 0x0400674C RID: 26444
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRepairBase = 1000;

	// Token: 0x0400674D RID: 26445
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPercent = 0f;

	// Token: 0x0400674E RID: 26446
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPerkPercent = 0.1f;
}
