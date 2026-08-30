using System;
using UnityEngine.Scripting;

// Token: 0x02000C8F RID: 3215
[Preserve]
public class VPFuelTank : VehiclePart
{
	// Token: 0x0600629C RID: 25244 RVA: 0x0026C47C File Offset: 0x0026A67C
	public VPFuelTank()
	{
		this.fuelLevel = 0f;
	}

	// Token: 0x0600629D RID: 25245 RVA: 0x0026C490 File Offset: 0x0026A690
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

	// Token: 0x0600629E RID: 25246 RVA: 0x0026C4DF File Offset: 0x0026A6DF
	public override void HandleEvent(VehiclePart.Event _event, VehiclePart _part, float _amount)
	{
		if (_event == VehiclePart.Event.FuelRemove)
		{
			this.AddFuel(-_amount);
		}
	}

	// Token: 0x0600629F RID: 25247 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsBroken()
	{
		return false;
	}

	// Token: 0x060062A0 RID: 25248 RVA: 0x0026C4ED File Offset: 0x0026A6ED
	public float GetFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelLevel;
	}

	// Token: 0x060062A1 RID: 25249 RVA: 0x0026C503 File Offset: 0x0026A703
	public float GetMaxFuelLevel()
	{
		if (this.IsBroken())
		{
			return 0f;
		}
		return this.fuelCapacity * this.vehicle.EffectFuelMaxPer;
	}

	// Token: 0x060062A2 RID: 25250 RVA: 0x0026C528 File Offset: 0x0026A728
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

	// Token: 0x060062A3 RID: 25251 RVA: 0x0026C56C File Offset: 0x0026A76C
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

	// Token: 0x060062A4 RID: 25252 RVA: 0x0026C5CC File Offset: 0x0026A7CC
	public void AddFuel(float _amount)
	{
		this.SetFuelLevel(this.fuelLevel + _amount);
	}

	// Token: 0x04004C7A RID: 19578
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelCapacity;

	// Token: 0x04004C7B RID: 19579
	[PublicizedFrom(EAccessModifier.Private)]
	public float fuelLevel;
}
