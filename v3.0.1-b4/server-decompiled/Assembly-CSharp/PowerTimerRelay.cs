using System;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public class PowerTimerRelay : PowerTrigger
{
	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06004305 RID: 17157 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Timer;
		}
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06004306 RID: 17158 RVA: 0x001A1E8B File Offset: 0x001A008B
	// (set) Token: 0x06004307 RID: 17159 RVA: 0x001A1E94 File Offset: 0x001A0094
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

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06004308 RID: 17160 RVA: 0x001A1ED2 File Offset: 0x001A00D2
	// (set) Token: 0x06004309 RID: 17161 RVA: 0x001A1EDC File Offset: 0x001A00DC
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

	// Token: 0x0600430A RID: 17162 RVA: 0x001A1F1A File Offset: 0x001A011A
	public PowerTimerRelay()
	{
		this.StartTime = 0;
		this.EndTime = 24;
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x0600430B RID: 17163 RVA: 0x001A1F39 File Offset: 0x001A0139
	// (set) Token: 0x0600430C RID: 17164 RVA: 0x001A1F44 File Offset: 0x001A0144
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

	// Token: 0x0600430D RID: 17165 RVA: 0x001A1FD5 File Offset: 0x001A01D5
	public override bool PowerChildren()
	{
		return this.IsTriggered;
	}

	// Token: 0x0600430E RID: 17166 RVA: 0x001A1FE0 File Offset: 0x001A01E0
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

	// Token: 0x0600430F RID: 17167 RVA: 0x001A206B File Offset: 0x001A026B
	public override void CachedUpdateCall()
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 1f;
			this.CheckForActiveChange();
		}
	}

	// Token: 0x06004310 RID: 17168 RVA: 0x001A2094 File Offset: 0x001A0294
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

	// Token: 0x06004311 RID: 17169 RVA: 0x001A2118 File Offset: 0x001A0318
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

	// Token: 0x06004312 RID: 17170 RVA: 0x001A218D File Offset: 0x001A038D
	public override void read(BinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.StartTime = _br.ReadByte();
		this.EndTime = _br.ReadByte();
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x001A21AF File Offset: 0x001A03AF
	public override void write(BinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.StartTime);
		_bw.Write(this.EndTime);
	}

	// Token: 0x040035E7 RID: 13799
	[PublicizedFrom(EAccessModifier.Private)]
	public byte startTime;

	// Token: 0x040035E8 RID: 13800
	[PublicizedFrom(EAccessModifier.Private)]
	public byte endTime = 12;

	// Token: 0x040035E9 RID: 13801
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTimeInTicks;

	// Token: 0x040035EA RID: 13802
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong endTimeInTicks;

	// Token: 0x040035EB RID: 13803
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;
}
