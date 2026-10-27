using System;
using UnityEngine.Scripting;

// Token: 0x02000CBA RID: 3258
[Preserve]
public class VPFuelTank : VehiclePart
{
	// Token: 0x0600633F RID: 25407 RVA: 0x00273CC0 File Offset: 0x00271EC0
	public VPFuelTank()
	{
		this.fuelLevel = 0f;
	}

	// Token: 0x06006340 RID: 25408 RVA: 0x00273CD4 File Offset: 0x00271ED4
	public override void SetProperties(DynamicProperties _properties)
	{
		base.SetProperties(_properties);
		StringParsers.TryParseFloat(base.GetProperty("capacity"), out this.fuelCapacity);
		if (this.fuelCapacity < 1f)
		{
			this.fuelCapacity = 1f;
		}
		this.fuelLevel = this.fuelCapacity;
	}

	// Token: 0x06006341 RID: 25409 RVA: 0x00273D23 File Offset: 0x00271F23
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _amount)
	{
		if (_event == VehiclePart.Event.FuelRemove)
		{
			this.AddFuel(-_amount);
		}
	}

	// Token: 0x06006342 RID: 25410 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsBroken()
	{
		return false;
	}

	// Token: 0x06006343 RID: 25411 RVA: 0x00273D31 File Offset: 0x00271F31
	public float GetFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelLevel;
	}

	// Token: 0x06006344 RID: 25412 RVA: 0x00273D47 File Offset: 0x00271F47
	public float GetMaxFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelCapacity * this.vehicle.EffectFuelMaxPer;
	}

	// Token: 0x06006345 RID: 25413 RVA: 0x00273D6C File Offset: 0x00271F6C
	public float GetFuelLevelPercent()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		float num = this.fuelLevel / (this.fuelCapacity * this.vehicle.EffectFuelMaxPer);
		if (num > 1f)
		{
			num = 1f;
		}
		return num;
	}

	// Token: 0x06006346 RID: 25414 RVA: 0x00273DB0 File Offset: 0x00271FB0
	public void SetFuelLevel(float _fuelLevel)
	{
		if (_fuelLevel <= 0f)
		{
			this.fuelLevel = 0f;
			if (EntityVehicle.VehicleFuelUsageModifier > 0f)
			{
				this.vehicle.FireEvent(VehiclePart.Event.FuelEmpty, this, 0f);
				return;
			}
		}
		else
		{
			float num = this.fuelCapacity * this.vehicle.EffectFuelMaxPer;
			if (_fuelLevel > num)
			{
				_fuelLevel = num;
			}
			this.fuelLevel = _fuelLevel;
		}
	}

	// Token: 0x06006347 RID: 25415 RVA: 0x00273E10 File Offset: 0x00272010
	public void AddFuel(float _amount)
	{
		this.SetFuelLevel(this.fuelLevel + _amount);
	}

	// Token: 0x04004D72 RID: 19826
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelCapacity;

	// Token: 0x04004D73 RID: 19827
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelLevel;
}
