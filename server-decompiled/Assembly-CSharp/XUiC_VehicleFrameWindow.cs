using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001098 RID: 4248
[Preserve]
public class XUiC_VehicleFrameWindow : XUiC_AssembleWindow
{
	// Token: 0x17000FAC RID: 4012
	// (get) Token: 0x06008752 RID: 34642 RVA: 0x0033C1A1 File Offset: 0x0033A3A1
	// (set) Token: 0x06008753 RID: 34643 RVA: 0x0033C1A9 File Offset: 0x0033A3A9
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

	// Token: 0x06008754 RID: 34644 RVA: 0x0033C1C0 File Offset: 0x0033A3C0
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

	// Token: 0x06008755 RID: 34645 RVA: 0x0033C27B File Offset: 0x0033A47B
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

	// Token: 0x06008756 RID: 34646 RVA: 0x0033C2AA File Offset: 0x0033A4AA
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

	// Token: 0x06008757 RID: 34647 RVA: 0x0033C2DC File Offset: 0x0033A4DC
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

	// Token: 0x06008758 RID: 34648 RVA: 0x0033C338 File Offset: 0x0033A538
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

	// Token: 0x06008759 RID: 34649 RVA: 0x0033C9BC File Offset: 0x0033ABBC
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.btnRefuel_Background.Enabled = this.Vehicle != null && this.Vehicle.GetVehicle().HasEnginePart() && EntityVehicle.VehicleFuelUsageModifier > 0f;
		}
		base.Update(_dt);
	}

	// Token: 0x0600875A RID: 34650 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x0600875B RID: 34651 RVA: 0x002C9895 File Offset: 0x002C7A95
	public override void OnClose()
	{
		base.OnClose();
	}

	// Token: 0x17000FAD RID: 4013
	// (set) Token: 0x0600875C RID: 34652 RVA: 0x0033CA12 File Offset: 0x0033AC12
	public override ItemStack ItemStack
	{
		set
		{
			this.vehicle.GetVehicle().SetItemValueMods(value.itemValue);
			base.ItemStack = value;
		}
	}

	// Token: 0x0600875D RID: 34653 RVA: 0x0033CA31 File Offset: 0x0033AC31
	public override void OnChanged()
	{
		this.group.OnItemChanged(this.ItemStack);
		this.IsDirty = true;
	}

	// Token: 0x040064BC RID: 25788
	public XUiC_VehicleWindowGroup group;

	// Token: 0x040064BD RID: 25789
	[PublicizedFrom(EAccessModifier.Private)]
	public bool RefuelButtonHovered;

	// Token: 0x040064BE RID: 25790
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRepair_Background;

	// Token: 0x040064BF RID: 25791
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRefuel_Background;

	// Token: 0x040064C0 RID: 25792
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle vehicle;

	// Token: 0x040064C1 RID: 25793
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> vehicleNameQualityFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => string.Format(_s1, _s2));

	// Token: 0x040064C2 RID: 25794
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt vehicleQualityFormatter = new CachedStringFormatterInt();

	// Token: 0x040064C3 RID: 25795
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor vehicleQualityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040064C4 RID: 25796
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int, int> vehicleDurabilityFormatter = new CachedStringFormatter<int, int>((int _i1, int _i2) => string.Format("{0}/{1}", _i1, _i2));

	// Token: 0x040064C5 RID: 25797
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt speedFormatter = new CachedStringFormatterInt();

	// Token: 0x040064C6 RID: 25798
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt protectionFormatter = new CachedStringFormatterInt();

	// Token: 0x040064C7 RID: 25799
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> fuelFormatter = new CachedStringFormatter<int>((int _i) => _i.ToString() + "%");

	// Token: 0x040064C8 RID: 25800
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt passengersFormatter = new CachedStringFormatterInt();

	// Token: 0x040064C9 RID: 25801
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat potentialFuelFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040064CA RID: 25802
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat fuelFillFormatter = new CachedStringFormatterFloat(null);
}
