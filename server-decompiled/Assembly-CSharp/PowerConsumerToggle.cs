using System;
using UnityEngine;

// Token: 0x0200091A RID: 2330
public class PowerConsumerToggle : PowerConsumer
{
	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x060042B5 RID: 17077 RVA: 0x0002F260 File Offset: 0x0002D460
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.ConsumerToggle;
		}
	}

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x060042B6 RID: 17078 RVA: 0x001A377B File Offset: 0x001A197B
	// (set) Token: 0x060042B7 RID: 17079 RVA: 0x001A3783 File Offset: 0x001A1983
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

	// Token: 0x060042B8 RID: 17080 RVA: 0x001A3794 File Offset: 0x001A1994
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

	// Token: 0x060042B9 RID: 17081 RVA: 0x001A381C File Offset: 0x001A1A1C
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

	// Token: 0x060042BA RID: 17082 RVA: 0x001A38AF File Offset: 0x001A1AAF
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.isToggled = _br.ReadBoolean();
	}

	// Token: 0x060042BB RID: 17083 RVA: 0x001A38C5 File Offset: 0x001A1AC5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.isToggled);
	}

	// Token: 0x0400363B RID: 13883
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isToggled = true;
}
