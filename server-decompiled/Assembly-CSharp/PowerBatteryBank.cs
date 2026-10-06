using System;
using UnityEngine;

// Token: 0x0200090D RID: 2317
public class PowerBatteryBank : PowerSource
{
	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x0600427A RID: 17018 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.BatteryBank;
		}
	}

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x0600427B RID: 17019 RVA: 0x001A1E97 File Offset: 0x001A0097
	public override string OnSound
	{
		get
		{
			return "batterybank_start";
		}
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x0600427C RID: 17020 RVA: 0x001A1E9E File Offset: 0x001A009E
	public override string OffSound
	{
		get
		{
			return "batterybank_stop";
		}
	}

	// Token: 0x0600427E RID: 17022 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool CanParent(PowerItem parent)
	{
		return true;
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x0600427F RID: 17023 RVA: 0x001A1EAD File Offset: 0x001A00AD
	public override bool IsPowered
	{
		get
		{
			return this.isOn || this.isPowered;
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x06004280 RID: 17024 RVA: 0x001A1EC0 File Offset: 0x001A00C0
	public bool ParentPowering
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.Parent == null)
			{
				return false;
			}
			if (this.Parent is PowerSolarPanel)
			{
				PowerSolarPanel powerSolarPanel = this.Parent as PowerSolarPanel;
				return powerSolarPanel.HasLight && powerSolarPanel.IsOn;
			}
			if (this.Parent is PowerSource)
			{
				return (this.Parent as PowerSource).IsOn;
			}
			if (this.Parent is PowerTrigger)
			{
				return this.Parent.IsPowered && (this.Parent as PowerTrigger).IsActive;
			}
			return this.Parent.IsPowered;
		}
	}

	// Token: 0x06004281 RID: 17025 RVA: 0x001A1F57 File Offset: 0x001A0157
	public override void Update()
	{
		if (this.Parent != null && this.LastPowerReceived > 0)
		{
			if (this.LastInputAmount > 0 && base.IsOn)
			{
				this.AddPowerToBatteries((int)this.LastInputAmount);
			}
			return;
		}
		base.Update();
	}

	// Token: 0x06004282 RID: 17026 RVA: 0x001A1F90 File Offset: 0x001A0190
	public override void HandleSendPower()
	{
		if (base.IsOn && !this.ParentPowering)
		{
			if (this.CurrentPower < this.MaxPower)
			{
				this.TickPowerGeneration();
			}
			else if (this.CurrentPower > this.MaxPower)
			{
				this.CurrentPower = this.MaxPower;
			}
			if (this.CurrentPower <= 0)
			{
				this.CurrentPower = 0;
				if (this.isPowered)
				{
					this.HandleDisconnect();
					this.hasChangesLocal = true;
				}
			}
			else
			{
				this.isPowered = true;
			}
			if (this.hasChangesLocal)
			{
				this.LastPowerUsed = 0;
				ushort num = (ushort)Mathf.Min((int)this.MaxOutput, (int)this.CurrentPower);
				ushort num2 = num;
				World world = GameManager.Instance.World;
				for (int i = 0; i < this.Children.Count; i++)
				{
					num = num2;
					this.Children[i].HandlePowerReceived(ref num2);
					this.LastPowerUsed += num - num2;
				}
			}
			this.CurrentPower -= (ushort)Mathf.Min((int)this.CurrentPower, (int)this.LastPowerUsed);
		}
	}

	// Token: 0x06004283 RID: 17027 RVA: 0x001A20A0 File Offset: 0x001A02A0
	public override void HandlePowerReceived(ref ushort power)
	{
		this.LastPowerUsed = 0;
		if (this.LastPowerReceived != power)
		{
			this.LastPowerReceived = power;
			this.hasChangesLocal = true;
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandleDisconnect();
			}
		}
		if (power <= 0)
		{
			return;
		}
		if (base.IsOn && power > 0)
		{
			ushort num = (ushort)Mathf.Min((int)this.InputPerTick, (int)power);
			this.AddPowerToBatteries((int)num);
			power -= this.LastInputAmount;
		}
		if (this.PowerChildren())
		{
			for (int j = 0; j < this.Children.Count; j++)
			{
				this.Children[j].HandlePowerReceived(ref power);
				if (power <= 0)
				{
					return;
				}
			}
		}
	}

	// Token: 0x06004284 RID: 17028 RVA: 0x001A2160 File Offset: 0x001A0360
	[PublicizedFrom(EAccessModifier.Protected)]
	public void AddPowerToBatteries(int power)
	{
		int num = power;
		int num2 = power / (int)this.InputPerTick * (int)this.ChargePerInput;
		for (int i = this.Stacks.Length - 1; i >= 0; i--)
		{
			if (!this.Stacks[i].IsEmpty())
			{
				int num3 = (int)this.Stacks[i].itemValue.UseTimes;
				if (num3 > 0)
				{
					ushort num4 = (ushort)Mathf.Min(num3, num2);
					num -= (int)(num4 * this.InputPerTick);
					this.Stacks[i].itemValue.UseTimes -= (float)num4;
				}
				if (num == 0)
				{
					break;
				}
			}
		}
		int num5 = power - num;
		if (this.LastInputAmount != (ushort)num5)
		{
			base.SendHasLocalChangesToRoot();
			this.LastInputAmount = (ushort)num5;
		}
	}

	// Token: 0x06004285 RID: 17029 RVA: 0x001A2210 File Offset: 0x001A0410
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TickPowerGeneration()
	{
		base.TickPowerGeneration();
		ushort num = this.MaxPower - this.CurrentPower;
		ushort num2 = num / this.OutputPerCharge;
		if (num >= this.OutputPerCharge)
		{
			for (int i = 0; i < this.Stacks.Length; i++)
			{
				int num3 = (int)Mathf.Min((float)this.Stacks[i].itemValue.MaxUseTimes - this.Stacks[i].itemValue.UseTimes, (float)num2);
				if (num3 > 0)
				{
					this.Stacks[i].itemValue.UseTimes += (float)num3;
					this.CurrentPower += (ushort)(num3 * (int)this.OutputPerCharge);
					return;
				}
			}
		}
	}

	// Token: 0x06004286 RID: 17030 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x06004287 RID: 17031 RVA: 0x001A22BC File Offset: 0x001A04BC
	public override void HandlePowerUpdate(bool isOn)
	{
		if (this.Parent != null && this.LastPowerReceived > 0 && this.PowerChildren())
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandlePowerUpdate(isOn);
			}
		}
	}

	// Token: 0x06004288 RID: 17032 RVA: 0x001A230C File Offset: 0x001A050C
	public override void HandleDisconnect()
	{
		if (this.isPowered)
		{
			this.IsPoweredChanged(false);
		}
		this.isPowered = false;
		this.HandlePowerUpdate(false);
		for (int i = 0; i < this.Children.Count; i++)
		{
			this.Children[i].HandleDisconnect();
		}
		this.LastInputAmount = 0;
		this.LastPowerReceived = 0;
		if (this.TileEntity != null)
		{
			this.TileEntity.SetModified();
		}
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x001A2380 File Offset: 0x001A0580
	public override void SetValuesFromBlock()
	{
		base.SetValuesFromBlock();
		Block block = Block.list[(int)this.BlockID];
		if (block.Properties.Values.ContainsKey("InputPerTick"))
		{
			this.InputPerTick = ushort.Parse(block.Properties.Values["InputPerTick"]);
		}
		if (block.Properties.Values.ContainsKey("ChargePerInput"))
		{
			this.ChargePerInput = ushort.Parse(block.Properties.Values["ChargePerInput"]);
		}
		if (block.Properties.Values.ContainsKey("OutputPerCharge"))
		{
			this.OutputPerCharge = ushort.Parse(block.Properties.Values["OutputPerCharge"]);
		}
		if (block.Properties.Values.ContainsKey("MaxPower"))
		{
			this.MaxPower = ushort.Parse(block.Properties.Values["MaxPower"]);
		}
	}

	// Token: 0x04003612 RID: 13842
	public ushort LastInputAmount;

	// Token: 0x04003613 RID: 13843
	public ushort LastPowerReceived;

	// Token: 0x04003614 RID: 13844
	public ushort InputPerTick;

	// Token: 0x04003615 RID: 13845
	public ushort ChargePerInput;

	// Token: 0x04003616 RID: 13846
	public ushort OutputPerCharge;
}
