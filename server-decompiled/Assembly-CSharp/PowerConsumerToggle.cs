using System;
using UnityEngine;

// Token: 0x02000910 RID: 2320
public class PowerConsumerToggle : PowerConsumer
{
	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06004292 RID: 17042 RVA: 0x0002F260 File Offset: 0x0002D460
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x06004293 RID: 17043 RVA: 0x001A26D3 File Offset: 0x001A08D3
	// (set) Token: 0x06004294 RID: 17044 RVA: 0x001A26DB File Offset: 0x001A08DB
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

	// Token: 0x06004295 RID: 17045 RVA: 0x001A26EC File Offset: 0x001A08EC
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

	// Token: 0x06004296 RID: 17046 RVA: 0x001A2774 File Offset: 0x001A0974
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

	// Token: 0x06004297 RID: 17047 RVA: 0x001A2807 File Offset: 0x001A0A07
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isToggled = _br.ReadBoolean();
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x001A281D File Offset: 0x001A0A1D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isToggled);
	}

	// Token: 0x0400361B RID: 13851
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isToggled = true;
}
