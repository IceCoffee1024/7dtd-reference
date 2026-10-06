using System;
using UnityEngine.Scripting;

// Token: 0x020010C6 RID: 4294
[Preserve]
public class XUiC_VehicleStats : XUiController
{
	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x060088AB RID: 34987 RVA: 0x00345196 File Offset: 0x00343396
	// (set) Token: 0x060088AC RID: 34988 RVA: 0x0034519E File Offset: 0x0034339E
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
		}
	}

	// Token: 0x060088AD RID: 34989 RVA: 0x003451B0 File Offset: 0x003433B0
	public override void Init()
	{
		base.Init();
		this.sprFuelFill = (XUiV_Sprite)base.GetChildById("sprFuelFill").ViewComponent;
		this.sprFillPotential = (XUiV_Sprite)base.GetChildById("sprFillPotential").ViewComponent;
		this.sprFillPotential.Fill = 0f;
		this.btnRefuel = base.GetChildById("btnRefuel");
		this.btnRefuel_Background = (XUiV_Button)this.btnRefuel.GetChildById("clickable").ViewComponent;
		this.btnRefuel_Background.Controller.OnPress += this.BtnRefuel_OnPress;
		this.btnRefuel_Background.Controller.OnHover += this.btnRefuel_OnHover;
		this.IsDirty = true;
	}

	// Token: 0x060088AE RID: 34990 RVA: 0x00345279 File Offset: 0x00343479
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRefuel_OnHover(XUiController _sender, bool _isOver)
	{
		this.RefuelButtonHovered = _isOver;
		base.RefreshBindings();
	}

	// Token: 0x060088AF RID: 34991 RVA: 0x00345288 File Offset: 0x00343488
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRefuel_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.xui.Vehicle.CurrentVehicle.AddFuelFromInventory(this.xui.playerUI.entityPlayer))
		{
			base.RefreshBindings();
		}
	}

	// Token: 0x060088B0 RID: 34992 RVA: 0x003452B8 File Offset: 0x003434B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1246567232U)
		{
			if (num <= 603484716U)
			{
				if (num <= 90731405U)
				{
					if (num != 30055014U)
					{
						if (num == 90731405U)
						{
							if (bindingName == "fuel")
							{
								value = ((EntityVehicle.VehicleFuelUsageModifier == 0f) ? Localization.Get("xuiNA", false, null) : this.fuelFormatter.Format((int)XUiM_Vehicle.GetFuelLevel(this.xui)));
								return true;
							}
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
					if (num == 603484716U)
					{
						if (bindingName == "vehiclename")
						{
							value = Localization.Get(XUiM_Vehicle.GetEntityName(this.xui), false, null);
							return true;
						}
					}
				}
				else if (bindingName == "protection")
				{
					value = this.protectionFormatter.Format((int)XUiM_Vehicle.GetProtection(this.xui));
					return true;
				}
			}
			else if (num <= 659620374U)
			{
				if (num != 609690980U)
				{
					if (num == 659620374U)
					{
						if (bindingName == "passengerstitle")
						{
							value = Localization.Get("xuiSeats", false, null);
							return true;
						}
					}
				}
				else if (bindingName == "vehiclestatstitle")
				{
					value = Localization.Get("xuiStats", false, null);
					return true;
				}
			}
			else if (num != 804398480U)
			{
				if (num == 1246567232U)
				{
					if (bindingName == "fuelfill")
					{
						value = this.fuelFillFormatter.Format(XUiM_Vehicle.GetFuelFill(this.xui));
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
					Vehicle vehicle = this.xui.Vehicle.CurrentVehicle.GetVehicle();
					value = this.potentialFuelFillFormatter.Format(vehicle.GetFuelPercent());
				}
				return true;
			}
		}
		else if (num <= 2420381393U)
		{
			if (num <= 2072037248U)
			{
				if (num != 2015621088U)
				{
					if (num == 2072037248U)
					{
						if (bindingName == "speed")
						{
							value = this.speedFormatter.Format((int)XUiM_Vehicle.GetSpeed(this.xui));
							return true;
						}
					}
				}
				else if (bindingName == "passengers")
				{
					value = this.passengersFormatter.Format(XUiM_Vehicle.GetPassengers(this.xui));
					return true;
				}
			}
			else if (num != 2224025142U)
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
		else if (num <= 2673357751U)
		{
			if (num != 2662421117U)
			{
				if (num == 2673357751U)
				{
					if (bindingName == "speedtext")
					{
						value = XUiM_Vehicle.GetSpeedText(this.xui);
						return true;
					}
				}
			}
			else if (bindingName == "noisetitle")
			{
				value = Localization.Get("xuiNoise", false, null);
				return true;
			}
		}
		else if (num != 2956837708U)
		{
			if (num != 3776886369U)
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
		}
		else if (bindingName == "protectiontitle")
		{
			value = Localization.Get("xuiDefense", false, null);
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060088B1 RID: 34993 RVA: 0x003456DC File Offset: 0x003438DC
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (this.Vehicle == null)
		{
			return;
		}
		base.Update(_dt);
	}

	// Token: 0x060088B2 RID: 34994 RVA: 0x00345710 File Offset: 0x00343910
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.btnRefuel == null)
		{
			this.btnRefuel_Background.IsVisible = (this.btnRefuel.ViewComponent.IsVisible = EntityVehicle.VehicleFuelUsageModifier > 0f);
		}
	}

	// Token: 0x040065D9 RID: 26073
	[PublicizedFrom(EAccessModifier.Private)]
	public bool RefuelButtonHovered;

	// Token: 0x040065DA RID: 26074
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprFuelFill;

	// Token: 0x040065DB RID: 26075
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprFillPotential;

	// Token: 0x040065DC RID: 26076
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnRefuel;

	// Token: 0x040065DD RID: 26077
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRefuel_Background;

	// Token: 0x040065DE RID: 26078
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle vehicle;

	// Token: 0x040065DF RID: 26079
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt speedFormatter = new CachedStringFormatterInt();

	// Token: 0x040065E0 RID: 26080
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt protectionFormatter = new CachedStringFormatterInt();

	// Token: 0x040065E1 RID: 26081
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt fuelFormatter = new CachedStringFormatterInt();

	// Token: 0x040065E2 RID: 26082
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt passengersFormatter = new CachedStringFormatterInt();

	// Token: 0x040065E3 RID: 26083
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat potentialFuelFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040065E4 RID: 26084
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat fuelFillFormatter = new CachedStringFormatterFloat(null);
}
