using System;
using Audio;
using UnityEngine;

// Token: 0x02000923 RID: 2339
public class PowerTimerRelay : PowerTrigger
{
	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x0600431F RID: 17183 RVA: 0x000809D4 File Offset: 0x0007EBD4
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Timer;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06004320 RID: 17184 RVA: 0x001A510F File Offset: 0x001A330F
	// (set) Token: 0x06004321 RID: 17185 RVA: 0x001A5118 File Offset: 0x001A3318
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

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06004322 RID: 17186 RVA: 0x001A5156 File Offset: 0x001A3356
	// (set) Token: 0x06004323 RID: 17187 RVA: 0x001A5160 File Offset: 0x001A3360
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

	// Token: 0x06004324 RID: 17188 RVA: 0x001A519E File Offset: 0x001A339E
	public PowerTimerRelay()
	{
		this.StartTime = 0;
		this.EndTime = 24;
	}

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x06004325 RID: 17189 RVA: 0x001A51BD File Offset: 0x001A33BD
	// (set) Token: 0x06004326 RID: 17190 RVA: 0x001A51C8 File Offset: 0x001A33C8
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

	// Token: 0x06004327 RID: 17191 RVA: 0x001A5259 File Offset: 0x001A3459
	public override bool PowerChildren()
	{
		return this.IsTriggered;
	}

	// Token: 0x06004328 RID: 17192 RVA: 0x001A5264 File Offset: 0x001A3464
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

	// Token: 0x06004329 RID: 17193 RVA: 0x001A52EF File Offset: 0x001A34EF
	public override void CachedUpdateCall()
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			this.CheckForActiveChange();
		}
	}

	// Token: 0x0600432A RID: 17194 RVA: 0x001A5318 File Offset: 0x001A3518
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

	// Token: 0x0600432B RID: 17195 RVA: 0x001A539C File Offset: 0x001A359C
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

	// Token: 0x0600432C RID: 17196 RVA: 0x001A5411 File Offset: 0x001A3611
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.StartTime = _br.ReadByte();
		this.EndTime = _br.ReadByte();
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x001A5433 File Offset: 0x001A3633
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.StartTime);
		_bw.Write(this.EndTime);
	}

	// Token: 0x0400367C RID: 13948
	[PublicizedFrom(EAccessModifier.Private)]
	public byte startTime;

	// Token: 0x0400367D RID: 13949
	[PublicizedFrom(EAccessModifier.Private)]
	public byte endTime = 12;

	// Token: 0x0400367E RID: 13950
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTimeInTicks;

	// Token: 0x0400367F RID: 13951
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong endTimeInTicks;

	// Token: 0x04003680 RID: 13952
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
