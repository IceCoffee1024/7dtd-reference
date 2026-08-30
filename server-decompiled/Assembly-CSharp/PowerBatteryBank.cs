using System;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public class PowerBatteryBank : PowerSource
{
	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x060042E8 RID: 17128 RVA: 0x000601DD File Offset: 0x0005E3DD
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.BatteryBank;
		}
	}

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x060042E9 RID: 17129 RVA: 0x001A1D93 File Offset: 0x0019FF93
	public override string OnSound
	{
		get
		{
			return "batterybank_start";
		}
	}

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x060042EA RID: 17130 RVA: 0x001A1D9A File Offset: 0x0019FF9A
	public override string OffSound
	{
		get
		{
			return "batterybank_stop";
		}
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool CanParent(PowerItem parent)
	{
		return true;
	}

	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x060042ED RID: 17133 RVA: 0x001A1DA9 File Offset: 0x0019FFA9
	public override bool IsPowered
	{
		get
		{
			return this.isOn || this.isPowered;
		}
	}

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x060042EE RID: 17134 RVA: 0x001A1DBC File Offset: 0x0019FFBC
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

	// Token: 0x060042EF RID: 17135 RVA: 0x001A1E53 File Offset: 0x001A0053
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

	// Token: 0x060042F0 RID: 17136 RVA: 0x001A1E8C File Offset: 0x001A008C
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

	// Token: 0x060042F1 RID: 17137 RVA: 0x001A1F9C File Offset: 0x001A019C
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

	// Token: 0x060042F2 RID: 17138 RVA: 0x001A205C File Offset: 0x001A025C
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

	// Token: 0x060042F3 RID: 17139 RVA: 0x001A210C File Offset: 0x001A030C
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

	// Token: 0x060042F4 RID: 17140 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x001A21B8 File Offset: 0x001A03B8
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

	// Token: 0x060042F6 RID: 17142 RVA: 0x001A2208 File Offset: 0x001A0408
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

	// Token: 0x060042F7 RID: 17143 RVA: 0x001A227C File Offset: 0x001A047C
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

	// Token: 0x0400361E RID: 13854
	public ushort LastInputAmount;

	// Token: 0x0400361F RID: 13855
	public ushort LastPowerReceived;

	// Token: 0x04003620 RID: 13856
	public ushort InputPerTick;

	// Token: 0x04003621 RID: 13857
	public ushort ChargePerInput;

	// Token: 0x04003622 RID: 13858
	public ushort OutputPerCharge;
}
