using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010B1 RID: 4273
[Preserve]
public class XUiC_VehicleFrameWindow : XUiC_AssembleWindow
{
	// Token: 0x17000FE5 RID: 4069
	// (get) Token: 0x06008844 RID: 34884 RVA: 0x003409C9 File Offset: 0x0033EBC9
	// (set) Token: 0x06008845 RID: 34885 RVA: 0x003409D1 File Offset: 0x0033EBD1
	public EntityVehicle Vehicle
	{
		get
		{
			return this.vehicle;
		}
		set
		{
			this.vehicle = value;
			base.RefreshBindings();
			this.IsDirty = true;
		}
	}

	// Token: 0x06008846 RID: 34886 RVA: 0x003409E8 File Offset: 0x0033EBE8
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("btnRepair");
		if (childById != null)
		{
			this.btnRepair_Background = (XUiV_Button)childById.GetChildById("clickable").ViewComponent;
			this.btnRepair_Background.Controller.OnPress += this.BtnRepair_OnPress;
		}
		XUiController childById2 = base.GetChildById("btnRefuel");
		if (childById2 != null)
		{
			this.btnRefuel_Background = (XUiV_Button)childById2.GetChildById("clickable").ViewComponent;
			this.btnRefuel_Background.Controller.OnPress += this.BtnRefuel_OnPress;
			this.btnRefuel_Background.Controller.OnHover += this.btnRefuel_OnHover;
		}
	}

	// Token: 0x06008847 RID: 34887 RVA: 0x00340AA3 File Offset: 0x0033ECA3
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRepair_OnPress(XUiController _sender, int _mouseButton)
	{
		if (XUiM_Vehicle.RepairVehicle(this.xui, null))
		{
			base.RefreshBindings();
			this.IsDirty = true;
			Manager.PlayInsidePlayerHead("crafting/craft_repair_item", -1, 0f, false, false);
		}
	}

	// Token: 0x06008848 RID: 34888 RVA: 0x00340AD2 File Offset: 0x0033ECD2
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRefuel_OnHover(XUiController _sender, bool _isOver)
	{
		if (this.Vehicle != null && !this.Vehicle.GetVehicle().HasEnginePart())
		{
			return;
		}
		this.RefuelButtonHovered = _isOver;
		base.RefreshBindings();
	}

	// Token: 0x06008849 RID: 34889 RVA: 0x00340B04 File Offset: 0x0033ED04
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRefuel_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.Vehicle != null && !this.Vehicle.GetVehicle().HasEnginePart())
		{
			return;
		}
		if (this.xui.Vehicle.CurrentVehicle.AddFuelFromInventory(this.xui.playerUI.entityPlayer))
		{
			base.RefreshBindings();
		}
	}

	// Token: 0x0600884A RID: 34890 RVA: 0x00340B60 File Offset: 0x0033ED60
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		Vehicle vehicle = ((this.vehicle != null) ? this.vehicle.GetVehicle() : null);
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2015621088U)
		{
			if (num <= 609690980U)
			{
				if (num <= 170644062U)
				{
					if (num != 30055014U)
					{
						if (num != 90731405U)
						{
							if (num == 170644062U)
							{
								if (bindingName == "vehicleicon")
								{
									value = ((this.vehicle != null) ? this.vehicle.GetMapIcon() : "");
									return true;
								}
							}
						}
						else if (bindingName == "fuel")
						{
							value = ((EntityVehicle.VehicleFuelUsageModifier == 0f) ? Localization.Get("xuiNA", false, null) : this.fuelFormatter.Format((int)XUiM_Vehicle.GetFuelLevel(this.xui)));
							return true;
						}
					}
					else if (bindingName == "storage")
					{
						value = "BASKET";
						return true;
					}
				}
				else if (num != 461899258U)
				{
					if (num != 603484716U)
					{
						if (num == 609690980U)
						{
							if (bindingName == "vehiclestatstitle")
							{
								value = Localization.Get("xuiStats", false, null);
								return true;
							}
						}
					}
					else if (bindingName == "vehiclename")
					{
						value = Localization.Get(XUiM_Vehicle.GetEntityName(this.xui), false, null);
						return true;
					}
				}
				else if (bindingName == "protection")
				{
					value = this.protectionFormatter.Format((int)XUiM_Vehicle.GetProtection(this.xui));
					return true;
				}
			}
			else if (num <= 1054004131U)
			{
				if (num != 659620374U)
				{
					if (num != 804398480U)
					{
						if (num == 1054004131U)
						{
							if (bindingName == "refueltext")
							{
								value = ((this.Vehicle != null && this.Vehicle.GetVehicle().HasEnginePart()) ? Localization.Get("xuiRefuel", false, null) : Localization.Get("xuiRefuelNotAllowed", false, null));
								return true;
							}
						}
					}
					else if (bindingName == "potentialfuelfill")
					{
						if (!this.RefuelButtonHovered)
						{
							value = "0";
						}
						else
						{
							value = this.potentialFuelFillFormatter.Format(vehicle.GetFuelPercent());
						}
						return true;
					}
				}
				else if (bindingName == "passengerstitle")
				{
					value = Localization.Get("xuiSeats", false, null);
					return true;
				}
			}
			else if (num <= 1397227906U)
			{
				if (num != 1246567232U)
				{
					if (num == 1397227906U)
					{
						if (bindingName == "vehiclequalitytitle")
						{
							value = "";
							return true;
						}
					}
				}
				else if (bindingName == "fuelfill")
				{
					value = this.fuelFillFormatter.Format(XUiM_Vehicle.GetFuelFill(this.xui));
					return true;
				}
			}
			else if (num != 1492025581U)
			{
				if (num == 2015621088U)
				{
					if (bindingName == "passengers")
					{
						value = this.passengersFormatter.Format(XUiM_Vehicle.GetPassengers(this.xui));
						return true;
					}
				}
			}
			else if (bindingName == "vehiclenamequality")
			{
				value = "";
				return true;
			}
		}
		else if (num <= 2759863796U)
		{
			if (num <= 2420381393U)
			{
				if (num != 2072037248U)
				{
					if (num != 2224025142U)
					{
						if (num == 2420381393U)
						{
							if (bindingName == "noise")
							{
								value = XUiM_Vehicle.GetNoise(this.xui);
								return true;
							}
						}
					}
					else if (bindingName == "speedtitle")
					{
						value = Localization.Get("xuiSpeed", false, null);
						return true;
					}
				}
				else if (bindingName == "speed")
				{
					value = this.speedFormatter.Format((int)XUiM_Vehicle.GetSpeed(this.xui));
					return true;
				}
			}
			else if (num != 2662421117U)
			{
				if (num != 2673357751U)
				{
					if (num == 2759863796U)
					{
						if (bindingName == "vehiclequality")
						{
							value = "";
							return true;
						}
					}
				}
				else if (bindingName == "speedtext")
				{
					value = XUiM_Vehicle.GetSpeedText(this.xui);
					return true;
				}
			}
			else if (bindingName == "noisetitle")
			{
				value = Localization.Get("xuiNoise", false, null);
				return true;
			}
		}
		else if (num <= 3243084443U)
		{
			if (num != 2885085424U)
			{
				if (num != 2956837708U)
				{
					if (num == 3243084443U)
					{
						if (bindingName == "vehiclequalitycolor")
						{
							if (this.vehicle != null)
							{
								Color32 color = QualityInfo.GetQualityColor(vehicle.GetVehicleQuality());
								value = this.vehicleQualityColorFormatter.Format(color);
							}
							return true;
						}
					}
				}
				else if (bindingName == "protectiontitle")
				{
					value = Localization.Get("xuiDefense", false, null);
					return true;
				}
			}
			else if (bindingName == "vehicledurability")
			{
				value = ((vehicle != null) ? this.vehicleDurabilityFormatter.Format(vehicle.GetHealth(), vehicle.GetMaxHealth()) : "");
				return true;
			}
		}
		else if (num <= 3720998534U)
		{
			if (num != 3392664512U)
			{
				if (num == 3720998534U)
				{
					if (bindingName == "vehicledurabilitytitle")
					{
						value = Localization.Get("xuiDurability", false, null);
						return true;
					}
				}
			}
			else if (bindingName == "showfuel")
			{
				value = (this.Vehicle != null && this.Vehicle.GetVehicle().HasEnginePart() && EntityVehicle.VehicleFuelUsageModifier > 0f).ToString();
				return true;
			}
		}
		else if (num != 3776886369U)
		{
			if (num == 4165243794U)
			{
				if (bindingName == "locktype")
				{
					value = Localization.Get("none", false, null);
					return true;
				}
			}
		}
		else if (bindingName == "fueltitle")
		{
			value = Localization.Get("xuiGas", false, null);
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600884B RID: 34891 RVA: 0x003411E4 File Offset: 0x0033F3E4
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.btnRefuel_Background.Enabled = this.Vehicle != null && this.Vehicle.GetVehicle().HasEnginePart() && EntityVehicle.VehicleFuelUsageModifier > 0f;
		}
		base.Update(_dt);
	}

	// Token: 0x0600884C RID: 34892 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x0600884D RID: 34893 RVA: 0x002CCDB9 File Offset: 0x002CAFB9
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x17000FE6 RID: 4070
	// (set) Token: 0x0600884E RID: 34894 RVA: 0x0034123A File Offset: 0x0033F43A
	public override ItemStack ItemStack
	{
		set
		{
			this.vehicle.GetVehicle().SetItemValueMods(value.itemValue);
			base.ItemStack = value;
		}
	}

	// Token: 0x0600884F RID: 34895 RVA: 0x00341259 File Offset: 0x0033F459
	public override void OnChanged()
	{
		this.group.OnItemChanged(this.ItemStack);
		this.IsDirty = true;
	}

	// Token: 0x04006583 RID: 25987
	public XUiC_VehicleWindowGroup group;

	// Token: 0x04006584 RID: 25988
	[PublicizedFrom(EAccessModifier.Private)]
	public bool RefuelButtonHovered;

	// Token: 0x04006585 RID: 25989
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRepair_Background;

	// Token: 0x04006586 RID: 25990
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRefuel_Background;

	// Token: 0x04006587 RID: 25991
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle vehicle;

	// Token: 0x04006588 RID: 25992
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> vehicleNameQualityFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format(_s1, _s2));

	// Token: 0x04006589 RID: 25993
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt vehicleQualityFormatter = new CachedStringFormatterInt();

	// Token: 0x0400658A RID: 25994
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor vehicleQualityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400658B RID: 25995
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> vehicleDurabilityFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => string.Format("{0}/{1}", _i1, _i2));

	// Token: 0x0400658C RID: 25996
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt speedFormatter = new CachedStringFormatterInt();

	// Token: 0x0400658D RID: 25997
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt protectionFormatter = new CachedStringFormatterInt();

	// Token: 0x0400658E RID: 25998
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> fuelFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + "%");

	// Token: 0x0400658F RID: 25999
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt passengersFormatter = new CachedStringFormatterInt();

	// Token: 0x04006590 RID: 26000
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat potentialFuelFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006591 RID: 26001
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat fuelFillFormatter = new CachedStringFormatterFloat(null);
}
