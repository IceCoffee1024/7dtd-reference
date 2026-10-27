using System;
using Audio;

// Token: 0x0200116A RID: 4458
public class XUiM_Vehicle : XUiModel
{
	// Token: 0x1700105F RID: 4191
	// (get) Token: 0x06008C6B RID: 35947 RVA: 0x00358DEB File Offset: 0x00356FEB
	// (set) Token: 0x06008C6C RID: 35948 RVA: 0x00358DF3 File Offset: 0x00356FF3
	public EntityVehicle CurrentVehicle { get; set; }

	// Token: 0x06008C6D RID: 35949 RVA: 0x00358DFC File Offset: 0x00356FFC
	public static string GetEntityName(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return "";
		}
		return _xui.Vehicle.CurrentVehicle.EntityName;
	}

	// Token: 0x06008C6E RID: 35950 RVA: 0x00358E27 File Offset: 0x00357027
	public static float GetSpeed(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().MaxPossibleSpeed;
	}

	// Token: 0x06008C6F RID: 35951 RVA: 0x00358E58 File Offset: 0x00357058
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

	// Token: 0x06008C70 RID: 35952 RVA: 0x00358ECC File Offset: 0x003570CC
	public static float GetProtection(XUi _xui)
	{
		EntityVehicle currentVehicle = _xui.Vehicle.CurrentVehicle;
		if (currentVehicle == null)
		{
			return 0f;
		}
		return (1f - currentVehicle.GetVehicle().GetPlayerDamagePercent()) * 100f;
	}

	// Token: 0x06008C71 RID: 35953 RVA: 0x00358F0B File Offset: 0x0035710B
	public static float GetFuelLevel(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent() * 100f;
	}

	// Token: 0x06008C72 RID: 35954 RVA: 0x00358F41 File Offset: 0x00357141
	public static float GetFuelFill(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent();
	}

	// Token: 0x06008C73 RID: 35955 RVA: 0x00358F71 File Offset: 0x00357171
	public static int GetPassengers(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 1;
		}
		return _xui.Vehicle.CurrentVehicle.GetAttachMaxCount();
	}

	// Token: 0x06008C74 RID: 35956 RVA: 0x00358F98 File Offset: 0x00357198
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

	// Token: 0x06008C75 RID: 35957 RVA: 0x0035902C File Offset: 0x0035722C
	public bool SetPart(XUi _xui, string _vehicleSlotName, ItemStack _stack, out ItemStack _resultStack)
	{
		Log.Warning("XUiM_Vehicle SetPart {0}", new object[] { _vehicleSlotName });
		this.CurrentVehicle == null;
		_resultStack = _stack;
		return false;
	}

	// Token: 0x06008C76 RID: 35958 RVA: 0x000027FC File Offset: 0x000009FC
	public void RefreshVehicle()
	{
	}

	// Token: 0x06008C77 RID: 35959 RVA: 0x00359054 File Offset: 0x00357254
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

	// Token: 0x040067E1 RID: 26593
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRepairBase = 1000;

	// Token: 0x040067E2 RID: 26594
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPercent = 0f;

	// Token: 0x040067E3 RID: 26595
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPerkPercent = 0.1f;
}
