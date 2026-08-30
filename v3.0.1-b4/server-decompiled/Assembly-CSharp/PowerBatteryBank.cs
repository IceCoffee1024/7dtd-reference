using System;
using UnityEngine;

// Token: 0x020008FC RID: 2300
public class PowerBatteryBank : PowerSource
{
	// Token: 0x17000701 RID: 1793
	// (get) Token: 0x06004283 RID: 17027 RVA: 0x0005FC95 File Offset: 0x0005DE95
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.BatteryBank;
		}
	}

	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x06004284 RID: 17028 RVA: 0x0019FCBB File Offset: 0x0019DEBB
	public override string OnSound
	{
		get
		{
			return "batterybank_start";
		}
	}

	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06004285 RID: 17029 RVA: 0x0019FCC2 File Offset: 0x0019DEC2
	public override string OffSound
	{
		get
		{
			return "batterybank_stop";
		}
	}

	// Token: 0x06004287 RID: 17031 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool CanParent(PowerItem parent)
	{
		return true;
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06004288 RID: 17032 RVA: 0x0019FCD1 File Offset: 0x0019DED1
	public override bool IsPowered
	{
		get
		{
			return this.isOn || this.isPowered;
		}
	}

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06004289 RID: 17033 RVA: 0x0019FCE4 File Offset: 0x0019DEE4
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

	// Token: 0x0600428A RID: 17034 RVA: 0x0019FD7B File Offset: 0x0019DF7B
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

	// Token: 0x0600428B RID: 17035 RVA: 0x0019FDB4 File Offset: 0x0019DFB4
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

	// Token: 0x0600428C RID: 17036 RVA: 0x0019FEC4 File Offset: 0x0019E0C4
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

	// Token: 0x0600428D RID: 17037 RVA: 0x0019FF84 File Offset: 0x0019E184
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

	// Token: 0x0600428E RID: 17038 RVA: 0x001A0034 File Offset: 0x0019E234
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

	// Token: 0x0600428F RID: 17039 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x06004290 RID: 17040 RVA: 0x001A00E0 File Offset: 0x0019E2E0
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

	// Token: 0x06004291 RID: 17041 RVA: 0x001A0130 File Offset: 0x0019E330
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

	// Token: 0x06004292 RID: 17042 RVA: 0x001A01A4 File Offset: 0x0019E3A4
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

	// Token: 0x0400359D RID: 13725
	public ushort LastInputAmount;

	// Token: 0x0400359E RID: 13726
	public ushort LastPowerReceived;

	// Token: 0x0400359F RID: 13727
	public ushort InputPerTick;

	// Token: 0x040035A0 RID: 13728
	public ushort ChargePerInput;

	// Token: 0x040035A1 RID: 13729
	public ushort OutputPerCharge;
}
