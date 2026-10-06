using System;
using Audio;
using UnityEngine;

// Token: 0x02000919 RID: 2329
public class PowerTimerRelay : PowerTrigger
{
	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x060042FC RID: 17148 RVA: 0x00080804 File Offset: 0x0007EA04
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Timer;
		}
	}

	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x060042FD RID: 17149 RVA: 0x001A4067 File Offset: 0x001A2267
	// (set) Token: 0x060042FE RID: 17150 RVA: 0x001A4070 File Offset: 0x001A2270
	public byte StartTime
	{
		get
		{
			return this.startTime;
		}
		set
		{
			this.startTime = value;
			int num = (int)(this.startTime / 2);
			bool flag = this.startTime % 2 == 1;
			this.startTimeInTicks = GameUtils.DayTimeToWorldTime(1, num, flag ? 30 : 0);
		}
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x060042FF RID: 17151 RVA: 0x001A40AE File Offset: 0x001A22AE
	// (set) Token: 0x06004300 RID: 17152 RVA: 0x001A40B8 File Offset: 0x001A22B8
	public byte EndTime
	{
		get
		{
			return this.endTime;
		}
		set
		{
			this.endTime = value;
			int num = (int)(this.endTime / 2);
			bool flag = this.endTime % 2 == 1;
			this.endTimeInTicks = GameUtils.DayTimeToWorldTime(1, num, flag ? 30 : 0);
		}
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x001A40F6 File Offset: 0x001A22F6
	public PowerTimerRelay()
	{
		this.StartTime = 0;
		this.EndTime = 24;
	}

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x06004302 RID: 17154 RVA: 0x001A4115 File Offset: 0x001A2315
	// (set) Token: 0x06004303 RID: 17155 RVA: 0x001A4120 File Offset: 0x001A2320
	public override bool IsTriggered
	{
		get
		{
			return this.isTriggered;
		}
		set
		{
			if (this.lastTriggered != value)
			{
				this.lastTriggered = this.isTriggered;
				this.isTriggered = value;
				if (!this.isTriggered && this.lastTriggered)
				{
					if (this.isPowered)
					{
						Manager.BroadcastPlay(this.Position.ToVector3(), "timer_start", 0f);
					}
					this.HandleDisconnect();
					return;
				}
				if (this.isPowered)
				{
					Manager.BroadcastPlay(this.Position.ToVector3(), "timer_stop", 0f);
				}
				this.isActive = true;
				base.SendHasLocalChangesToRoot();
			}
		}
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x001A41B1 File Offset: 0x001A23B1
	public override bool PowerChildren()
	{
		return this.IsTriggered;
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x001A41BC File Offset: 0x001A23BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CheckForActiveChange()
	{
		if (GameManager.Instance.World != null)
		{
			ulong num = GameManager.Instance.World.worldTime % 24000UL;
			if (this.StartTime < this.EndTime)
			{
				this.IsTriggered = this.startTimeInTicks < num && num < this.endTimeInTicks;
				return;
			}
			if (this.EndTime < this.StartTime)
			{
				this.IsTriggered = num > this.startTimeInTicks || num < this.endTimeInTicks;
				return;
			}
			this.IsTriggered = false;
		}
	}

	// Token: 0x06004306 RID: 17158 RVA: 0x001A4247 File Offset: 0x001A2447
	public override void CachedUpdateCall()
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			this.CheckForActiveChange();
		}
	}

	// Token: 0x06004307 RID: 17159 RVA: 0x001A4270 File Offset: 0x001A2470
	public override void HandlePowerReceived(ref ushort power)
	{
		ushort num = (ushort)Mathf.Min((int)this.RequiredPower, (int)power);
		num = (ushort)Mathf.Min((int)num, (int)this.RequiredPower);
		this.isPowered = num == this.RequiredPower;
		power -= num;
		if (power <= 0)
		{
			return;
		}
		this.CheckForActiveChange();
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

	// Token: 0x06004308 RID: 17160 RVA: 0x001A42F4 File Offset: 0x001A24F4
	public override void HandlePowerUpdate(bool parentIsOn)
	{
		if (this.TileEntity != null)
		{
			((TileEntityPoweredTrigger)this.TileEntity).Activate(this.isPowered && parentIsOn, this.isTriggered);
			this.TileEntity.SetModified();
		}
		for (int i = 0; i < this.Children.Count; i++)
		{
			this.Children[i].HandlePowerUpdate(this.isPowered && parentIsOn);
		}
		this.hasChangesLocal = true;
	}

	// Token: 0x06004309 RID: 17161 RVA: 0x001A4369 File Offset: 0x001A2569
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.StartTime = _br.ReadByte();
		this.EndTime = _br.ReadByte();
	}

	// Token: 0x0600430A RID: 17162 RVA: 0x001A438B File Offset: 0x001A258B
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.StartTime);
		_bw.Write(this.EndTime);
	}

	// Token: 0x0400365C RID: 13916
	[PublicizedFrom(EAccessModifier.Private)]
	public byte startTime;

	// Token: 0x0400365D RID: 13917
	[PublicizedFrom(EAccessModifier.Private)]
	public byte endTime = 12;

	// Token: 0x0400365E RID: 13918
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTimeInTicks;

	// Token: 0x0400365F RID: 13919
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong endTimeInTicks;

	// Token: 0x04003660 RID: 13920
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
