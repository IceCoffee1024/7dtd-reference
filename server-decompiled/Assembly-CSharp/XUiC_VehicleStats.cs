using System;
using UnityEngine.Scripting;

// Token: 0x020010D6 RID: 4310
[Preserve]
public class XUiC_VehicleStats : XUiController
{
	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x060088FE RID: 35070 RVA: 0x0034721A File Offset: 0x0034541A
	// (set) Token: 0x060088FF RID: 35071 RVA: 0x00347222 File Offset: 0x00345422
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

	// Token: 0x06008900 RID: 35072 RVA: 0x00347234 File Offset: 0x00345434
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

	// Token: 0x06008901 RID: 35073 RVA: 0x003472FD File Offset: 0x003454FD
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnRefuel_OnHover(XUiController _sender, bool _isOver)
	{
		this.RefuelButtonHovered = _isOver;
		base.RefreshBindings();
	}

	// Token: 0x06008902 RID: 35074 RVA: 0x0034730C File Offset: 0x0034550C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRefuel_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.xui.Vehicle.CurrentVehicle.AddFuelFromInventory(this.xui.playerUI.entityPlayer))
		{
			base.RefreshBindings();
		}
	}

	// Token: 0x06008903 RID: 35075 RVA: 0x0034733C File Offset: 0x0034553C
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

	// Token: 0x06008904 RID: 35076 RVA: 0x00347760 File Offset: 0x00345960
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

	// Token: 0x06008905 RID: 35077 RVA: 0x00347794 File Offset: 0x00345994
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.btnRefuel == null)
		{
			this.btnRefuel_Background.IsVisible = (this.btnRefuel.ViewComponent.IsVisible = EntityVehicle.VehicleFuelUsageModifier > 0f);
		}
	}

	// Token: 0x04006618 RID: 26136
	[PublicizedFrom(EAccessModifier.Private)]
	public bool RefuelButtonHovered;

	// Token: 0x04006619 RID: 26137
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprFuelFill;

	// Token: 0x0400661A RID: 26138
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite sprFillPotential;

	// Token: 0x0400661B RID: 26139
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnRefuel;

	// Token: 0x0400661C RID: 26140
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button btnRefuel_Background;

	// Token: 0x0400661D RID: 26141
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityVehicle vehicle;

	// Token: 0x0400661E RID: 26142
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt speedFormatter = new CachedStringFormatterInt();

	// Token: 0x0400661F RID: 26143
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt protectionFormatter = new CachedStringFormatterInt();

	// Token: 0x04006620 RID: 26144
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt fuelFormatter = new CachedStringFormatterInt();

	// Token: 0x04006621 RID: 26145
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt passengersFormatter = new CachedStringFormatterInt();

	// Token: 0x04006622 RID: 26146
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat potentialFuelFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04006623 RID: 26147
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat fuelFillFormatter = new CachedStringFormatterFloat(null);
}
