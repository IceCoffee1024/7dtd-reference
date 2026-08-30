using System;
using Audio;

// Token: 0x02001129 RID: 4393
public class XUiM_Vehicle : XUiModel
{
	// Token: 0x17000FE6 RID: 4070
	// (get) Token: 0x06008ABA RID: 35514 RVA: 0x0034E0EA File Offset: 0x0034C2EA
	// (set) Token: 0x06008ABB RID: 35515 RVA: 0x0034E0F2 File Offset: 0x0034C2F2
	public EntityVehicle CurrentVehicle { get; set; }

	// Token: 0x06008ABC RID: 35516 RVA: 0x0034E0FB File Offset: 0x0034C2FB
	public static string GetEntityName(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return "";
		}
		return _xui.Vehicle.CurrentVehicle.EntityName;
	}

	// Token: 0x06008ABD RID: 35517 RVA: 0x0034E126 File Offset: 0x0034C326
	public static float GetSpeed(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().MaxPossibleSpeed;
	}

	// Token: 0x06008ABE RID: 35518 RVA: 0x0034E158 File Offset: 0x0034C358
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

	// Token: 0x06008ABF RID: 35519 RVA: 0x0034E1CC File Offset: 0x0034C3CC
	public static float GetProtection(XUi _xui)
	{
		EntityVehicle currentVehicle = _xui.Vehicle.CurrentVehicle;
		if (currentVehicle == null)
		{
			return 0f;
		}
		return (1f - currentVehicle.GetVehicle().GetPlayerDamagePercent()) * 100f;
	}

	// Token: 0x06008AC0 RID: 35520 RVA: 0x0034E20B File Offset: 0x0034C40B
	public static float GetFuelLevel(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent() * 100f;
	}

	// Token: 0x06008AC1 RID: 35521 RVA: 0x0034E241 File Offset: 0x0034C441
	public static float GetFuelFill(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent();
	}

	// Token: 0x06008AC2 RID: 35522 RVA: 0x0034E271 File Offset: 0x0034C471
	public static int GetPassengers(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 1;
		}
		return _xui.Vehicle.CurrentVehicle.GetAttachMaxCount();
	}

	// Token: 0x06008AC3 RID: 35523 RVA: 0x0034E298 File Offset: 0x0034C498
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

	// Token: 0x06008AC4 RID: 35524 RVA: 0x0034E32C File Offset: 0x0034C52C
	public bool SetPart(XUi _xui, string _vehicleSlotName, ItemStack _stack, out ItemStack _resultStack)
	{
		Log.Warning("XUiM_Vehicle SetPart {0}", new object[] { _vehicleSlotName });
		this.CurrentVehicle == null;
		_resultStack = _stack;
		return false;
	}

	// Token: 0x06008AC5 RID: 35525 RVA: 0x000027FC File Offset: 0x000009FC
	public void RefreshVehicle()
	{
	}

	// Token: 0x06008AC6 RID: 35526 RVA: 0x0034E354 File Offset: 0x0034C554
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

	// Token: 0x04006682 RID: 26242
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRepairBase = 1000;

	// Token: 0x04006683 RID: 26243
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPercent = 0f;

	// Token: 0x04006684 RID: 26244
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPerkPercent = 0.1f;
}
