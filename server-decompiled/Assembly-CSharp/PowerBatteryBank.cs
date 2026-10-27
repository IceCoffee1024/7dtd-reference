using System;
using UnityEngine;

// Token: 0x02000917 RID: 2327
public class PowerBatteryBank : PowerSource
{
	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x0600429D RID: 17053 RVA: 0x00060491 File Offset: 0x0005E691
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.BatteryBank;
		}
	}

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x0600429E RID: 17054 RVA: 0x001A2F3F File Offset: 0x001A113F
	public override string OnSound
	{
		get
		{
			return "batterybank_start";
		}
	}

	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x0600429F RID: 17055 RVA: 0x001A2F46 File Offset: 0x001A1146
	public override string OffSound
	{
		get
		{
			return "batterybank_stop";
		}
	}

	// Token: 0x060042A1 RID: 17057 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool CanParent(PowerItem parent)
	{
		return true;
	}

	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x060042A2 RID: 17058 RVA: 0x001A2F55 File Offset: 0x001A1155
	public override bool IsPowered
	{
		get
		{
			return this.isOn || this.isPowered;
		}
	}

	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x060042A3 RID: 17059 RVA: 0x001A2F68 File Offset: 0x001A1168
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

	// Token: 0x060042A4 RID: 17060 RVA: 0x001A2FFF File Offset: 0x001A11FF
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

	// Token: 0x060042A5 RID: 17061 RVA: 0x001A3038 File Offset: 0x001A1238
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

	// Token: 0x060042A6 RID: 17062 RVA: 0x001A3148 File Offset: 0x001A1348
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

	// Token: 0x060042A7 RID: 17063 RVA: 0x001A3208 File Offset: 0x001A1408
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

	// Token: 0x060042A8 RID: 17064 RVA: 0x001A32B8 File Offset: 0x001A14B8
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

	// Token: 0x060042A9 RID: 17065 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x001A3364 File Offset: 0x001A1564
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

	// Token: 0x060042AB RID: 17067 RVA: 0x001A33B4 File Offset: 0x001A15B4
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

	// Token: 0x060042AC RID: 17068 RVA: 0x001A3428 File Offset: 0x001A1628
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

	// Token: 0x04003632 RID: 13874
	public ushort LastInputAmount;

	// Token: 0x04003633 RID: 13875
	public ushort LastPowerReceived;

	// Token: 0x04003634 RID: 13876
	public ushort InputPerTick;

	// Token: 0x04003635 RID: 13877
	public ushort ChargePerInput;

	// Token: 0x04003636 RID: 13878
	public ushort OutputPerCharge;
}
