using System;
using Audio;

// Token: 0x0200115A RID: 4442
public class XUiM_Vehicle : XUiModel
{
	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x06008C17 RID: 35863 RVA: 0x00356D3F File Offset: 0x00354F3F
	// (set) Token: 0x06008C18 RID: 35864 RVA: 0x00356D47 File Offset: 0x00354F47
	public EntityVehicle CurrentVehicle { get; set; }

	// Token: 0x06008C19 RID: 35865 RVA: 0x00356D50 File Offset: 0x00354F50
	public static string GetEntityName(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return "";
		}
		return _xui.Vehicle.CurrentVehicle.EntityName;
	}

	// Token: 0x06008C1A RID: 35866 RVA: 0x00356D7B File Offset: 0x00354F7B
	public static float GetSpeed(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().MaxPossibleSpeed;
	}

	// Token: 0x06008C1B RID: 35867 RVA: 0x00356DAC File Offset: 0x00354FAC
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

	// Token: 0x06008C1C RID: 35868 RVA: 0x00356E20 File Offset: 0x00355020
	public static float GetProtection(XUi _xui)
	{
		EntityVehicle currentVehicle = _xui.Vehicle.CurrentVehicle;
		if (currentVehicle == null)
		{
			return 0f;
		}
		return (1f - currentVehicle.GetVehicle().GetPlayerDamagePercent()) * 100f;
	}

	// Token: 0x06008C1D RID: 35869 RVA: 0x00356E5F File Offset: 0x0035505F
	public static float GetFuelLevel(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent() * 100f;
	}

	// Token: 0x06008C1E RID: 35870 RVA: 0x00356E95 File Offset: 0x00355095
	public static float GetFuelFill(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 0f;
		}
		return _xui.Vehicle.CurrentVehicle.GetVehicle().GetFuelPercent();
	}

	// Token: 0x06008C1F RID: 35871 RVA: 0x00356EC5 File Offset: 0x003550C5
	public static int GetPassengers(XUi _xui)
	{
		if (!(_xui.Vehicle.CurrentVehicle != null))
		{
			return 1;
		}
		return _xui.Vehicle.CurrentVehicle.GetAttachMaxCount();
	}

	// Token: 0x06008C20 RID: 35872 RVA: 0x00356EEC File Offset: 0x003550EC
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

	// Token: 0x06008C21 RID: 35873 RVA: 0x00356F80 File Offset: 0x00355180
	public bool SetPart(XUi _xui, string _vehicleSlotName, ItemStack _stack, out ItemStack _resultStack)
	{
		Log.Warning("XUiM_Vehicle SetPart {0}", new object[] { _vehicleSlotName });
		this.CurrentVehicle == null;
		_resultStack = _stack;
		return false;
	}

	// Token: 0x06008C22 RID: 35874 RVA: 0x000027FC File Offset: 0x000009FC
	public void RefreshVehicle()
	{
	}

	// Token: 0x06008C23 RID: 35875 RVA: 0x00356FA8 File Offset: 0x003551A8
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

	// Token: 0x040067A1 RID: 26529
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRepairBase = 1000;

	// Token: 0x040067A2 RID: 26530
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPercent = 0f;

	// Token: 0x040067A3 RID: 26531
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cRepairPerkPercent = 0.1f;
}
