using System;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x02000914 RID: 2324
public class PowerTimerRelay : PowerTrigger
{
	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x0600436A RID: 17258 RVA: 0x00080550 File Offset: 0x0007E750
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Timer;
		}
	}

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x0600436B RID: 17259 RVA: 0x001A3F63 File Offset: 0x001A2163
	// (set) Token: 0x0600436C RID: 17260 RVA: 0x001A3F6C File Offset: 0x001A216C
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

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x0600436D RID: 17261 RVA: 0x001A3FAA File Offset: 0x001A21AA
	// (set) Token: 0x0600436E RID: 17262 RVA: 0x001A3FB4 File Offset: 0x001A21B4
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

	// Token: 0x0600436F RID: 17263 RVA: 0x001A3FF2 File Offset: 0x001A21F2
	public PowerTimerRelay()
	{
		this.StartTime = 0;
		this.EndTime = 24;
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06004370 RID: 17264 RVA: 0x001A4011 File Offset: 0x001A2211
	// (set) Token: 0x06004371 RID: 17265 RVA: 0x001A401C File Offset: 0x001A221C
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

	// Token: 0x06004372 RID: 17266 RVA: 0x001A40AD File Offset: 0x001A22AD
	public override bool PowerChildren()
	{
		return this.IsTriggered;
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x001A40B8 File Offset: 0x001A22B8
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

	// Token: 0x06004374 RID: 17268 RVA: 0x001A4143 File Offset: 0x001A2343
	public override void CachedUpdateCall()
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			this.CheckForActiveChange();
		}
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x001A416C File Offset: 0x001A236C
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

	// Token: 0x06004376 RID: 17270 RVA: 0x001A41F0 File Offset: 0x001A23F0
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

	// Token: 0x06004377 RID: 17271 RVA: 0x001A4265 File Offset: 0x001A2465
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.StartTime = _br.ReadByte();
		this.EndTime = _br.ReadByte();
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x001A4287 File Offset: 0x001A2487
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.StartTime);
		_bw.Write(this.EndTime);
	}

	// Token: 0x04003668 RID: 13928
	[PublicizedFrom(EAccessModifier.Private)]
	public byte startTime;

	// Token: 0x04003669 RID: 13929
	[PublicizedFrom(EAccessModifier.Private)]
	public byte endTime = 12;

	// Token: 0x0400366A RID: 13930
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTimeInTicks;

	// Token: 0x0400366B RID: 13931
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong endTimeInTicks;

	// Token: 0x0400366C RID: 13932
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
