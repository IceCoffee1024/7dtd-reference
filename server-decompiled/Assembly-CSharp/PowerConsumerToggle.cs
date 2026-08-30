using System;
using System.IO;
using UnityEngine;

// Token: 0x0200090B RID: 2315
public class PowerConsumerToggle : PowerConsumer
{
	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06004300 RID: 17152 RVA: 0x0002F168 File Offset: 0x0002D368
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06004301 RID: 17153 RVA: 0x001A25CF File Offset: 0x001A07CF
	// (set) Token: 0x06004302 RID: 17154 RVA: 0x001A25D7 File Offset: 0x001A07D7
	public bool IsToggled
	{
		get
		{
			return this.isToggled;
		}
		set
		{
			this.isToggled = value;
			base.SendHasLocalChangesToRoot();
		}
	}

	// Token: 0x06004303 RID: 17155 RVA: 0x001A25E8 File Offset: 0x001A07E8
	public override void HandlePowerUpdate(bool isOn)
	{
		bool flag = this.isPowered && isOn && this.isToggled;
		if (this.TileEntity != null)
		{
			this.TileEntity.Activate(flag);
			if (flag && this.lastActivate != flag)
			{
				this.TileEntity.ActivateOnce();
			}
		}
		this.lastActivate = flag;
		if (this.PowerChildren())
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandlePowerUpdate(isOn);
			}
		}
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x001A2670 File Offset: 0x001A0870
	public override void HandlePowerReceived(ref ushort power)
	{
		ushort num = (ushort)Mathf.Min((int)this.RequiredPower, (int)power);
		bool flag = num == this.RequiredPower;
		if (flag != this.isPowered)
		{
			this.isPowered = flag;
			this.IsPoweredChanged(flag);
			if (this.TileEntity != null)
			{
				this.TileEntity.SetModified();
			}
		}
		power -= num;
		if (power <= 0)
		{
			return;
		}
		if (this.PowerChildren())
		{
			for (int i = 0; i < this.Children.Count; i++)
			{
				this.Children[i].HandlePowerReceived(ref power);
				if (power <= 0)
				{
					return;
				}
			}
		}
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x001A2703 File Offset: 0x001A0903
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isToggled = _br.ReadBoolean();
	}

	// Token: 0x06004306 RID: 17158 RVA: 0x001A2719 File Offset: 0x001A0919
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isToggled);
	}

	// Token: 0x04003627 RID: 13863
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isToggled = true;
}
