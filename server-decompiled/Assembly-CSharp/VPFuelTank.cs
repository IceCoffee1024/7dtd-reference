using System;
using UnityEngine.Scripting;

// Token: 0x02000CA1 RID: 3233
[Preserve]
public class VPFuelTank : VehiclePart
{
	// Token: 0x06006327 RID: 25383 RVA: 0x0026F354 File Offset: 0x0026D554
	public VPFuelTank()
	{
		this.fuelLevel = 0f;
	}

	// Token: 0x06006328 RID: 25384 RVA: 0x0026F368 File Offset: 0x0026D568
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

	// Token: 0x06006329 RID: 25385 RVA: 0x0026F3B7 File Offset: 0x0026D5B7
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _amount)
	{
		if (_event == VehiclePart.Event.FuelRemove)
		{
			this.AddFuel(-_amount);
		}
	}

	// Token: 0x0600632A RID: 25386 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsBroken()
	{
		return false;
	}

	// Token: 0x0600632B RID: 25387 RVA: 0x0026F3C5 File Offset: 0x0026D5C5
	public float GetFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelLevel;
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x0026F3DB File Offset: 0x0026D5DB
	public float GetMaxFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelCapacity * this.vehicle.EffectFuelMaxPer;
	}

	// Token: 0x0600632D RID: 25389 RVA: 0x0026F400 File Offset: 0x0026D600
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

	// Token: 0x0600632E RID: 25390 RVA: 0x0026F444 File Offset: 0x0026D644
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

	// Token: 0x0600632F RID: 25391 RVA: 0x0026F4A4 File Offset: 0x0026D6A4
	public void AddFuel(float _amount)
	{
		this.SetFuelLevel(this.fuelLevel + _amount);
	}

	// Token: 0x04004D17 RID: 19735
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelCapacity;

	// Token: 0x04004D18 RID: 19736
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelLevel;
}
