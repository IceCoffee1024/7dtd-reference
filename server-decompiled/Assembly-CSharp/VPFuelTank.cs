using System;
using UnityEngine.Scripting;

// Token: 0x02000CAF RID: 3247
[Preserve]
public class VPFuelTank : VehiclePart
{
	// Token: 0x0600631A RID: 25370 RVA: 0x00272798 File Offset: 0x00270998
	public VPFuelTank()
	{
		this.fuelLevel = 0f;
	}

	// Token: 0x0600631B RID: 25371 RVA: 0x002727AC File Offset: 0x002709AC
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

	// Token: 0x0600631C RID: 25372 RVA: 0x002727FB File Offset: 0x002709FB
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _amount)
	{
		if (_event == VehiclePart.Event.FuelRemove)
		{
			this.AddFuel(-_amount);
		}
	}

	// Token: 0x0600631D RID: 25373 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsBroken()
	{
		return false;
	}

	// Token: 0x0600631E RID: 25374 RVA: 0x00272809 File Offset: 0x00270A09
	public float GetFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelLevel;
	}

	// Token: 0x0600631F RID: 25375 RVA: 0x0027281F File Offset: 0x00270A1F
	public float GetMaxFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelCapacity * this.vehicle.EffectFuelMaxPer;
	}

	// Token: 0x06006320 RID: 25376 RVA: 0x00272844 File Offset: 0x00270A44
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

	// Token: 0x06006321 RID: 25377 RVA: 0x00272888 File Offset: 0x00270A88
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

	// Token: 0x06006322 RID: 25378 RVA: 0x002728E8 File Offset: 0x00270AE8
	public void AddFuel(float _amount)
	{
		this.SetFuelLevel(this.fuelLevel + _amount);
	}

	// Token: 0x04004D4D RID: 19789
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelCapacity;

	// Token: 0x04004D4E RID: 19790
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelLevel;
}
