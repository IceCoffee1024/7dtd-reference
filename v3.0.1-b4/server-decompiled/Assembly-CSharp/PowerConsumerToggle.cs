using System;
using System.IO;
using UnityEngine;

// Token: 0x020008FF RID: 2303
public class PowerConsumerToggle : PowerConsumer
{
	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x0600429B RID: 17051 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x0600429C RID: 17052 RVA: 0x001A04F7 File Offset: 0x0019E6F7
	// (set) Token: 0x0600429D RID: 17053 RVA: 0x001A04FF File Offset: 0x0019E6FF
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

	// Token: 0x0600429E RID: 17054 RVA: 0x001A0510 File Offset: 0x0019E710
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

	// Token: 0x0600429F RID: 17055 RVA: 0x001A0598 File Offset: 0x0019E798
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

	// Token: 0x060042A0 RID: 17056 RVA: 0x001A062B File Offset: 0x0019E82B
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isToggled = _br.ReadBoolean();
	}

	// Token: 0x060042A1 RID: 17057 RVA: 0x001A0641 File Offset: 0x0019E841
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isToggled);
	}

	// Token: 0x040035A6 RID: 13734
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isToggled = true;
}
