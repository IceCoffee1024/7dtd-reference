using System;
using Audio;
using UnityEngine;

// Token: 0x0200091A RID: 2330
public class PowerTrigger : PowerConsumer
{
	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x0600430B RID: 17163 RVA: 0x000470B6 File Offset: 0x000452B6
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Trigger;
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x0600430C RID: 17164 RVA: 0x001A43AC File Offset: 0x001A25AC
	// (set) Token: 0x0600430D RID: 17165 RVA: 0x001A43B4 File Offset: 0x001A25B4
	public PowerTrigger.TriggerPowerDelayTypes TriggerPowerDelay
	{
		get
		{
			return this.triggerPowerDelay;
		}
		set
		{
			this.triggerPowerDelay = value;
		}
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x0600430E RID: 17166 RVA: 0x001A43BD File Offset: 0x001A25BD
	// (set) Token: 0x0600430F RID: 17167 RVA: 0x001A43C5 File Offset: 0x001A25C5
	public PowerTrigger.TriggerPowerDurationTypes TriggerPowerDuration
	{
		get
		{
			return this.triggerPowerDuration;
		}
		set
		{
			this.triggerPowerDuration = value;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06004310 RID: 17168 RVA: 0x001A43CE File Offset: 0x001A25CE
	public virtual bool IsActive
	{
		get
		{
			if (this.TriggerType == PowerTrigger.TriggerTypes.Switch)
			{
				return this.isTriggered;
			}
			return this.isActive || this.parentTriggered;
		}
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06004311 RID: 17169 RVA: 0x001A4115 File Offset: 0x001A2315
	// (set) Token: 0x06004312 RID: 17170 RVA: 0x001A43F0 File Offset: 0x001A25F0
	public virtual bool IsTriggered
	{
		get
		{
			return this.isTriggered;
		}
		set
		{
			if (this.TriggerType == PowerTrigger.TriggerTypes.Switch)
			{
				this.lastTriggered = this.isTriggered;
				this.isTriggered = value;
				if (this.isTriggered && !this.lastTriggered)
				{
					this.isActive = true;
				}
				base.SendHasLocalChangesToRoot();
				if (!this.isTriggered && this.lastTriggered)
				{
					this.HandleDisconnectChildren();
					this.isActive = false;
					return;
				}
			}
			else
			{
				this.isTriggered = value;
				if (this.isTriggered && !this.lastTriggered)
				{
					PowerTrigger.TriggerTypes triggerType = this.TriggerType;
					if (triggerType != PowerTrigger.TriggerTypes.Motion)
					{
						if (triggerType == PowerTrigger.TriggerTypes.TripWire)
						{
							Manager.BroadcastPlay(this.Position.ToVector3(), "trip_wire_trigger", 0f);
						}
					}
					else
					{
						Manager.BroadcastPlay(this.Position.ToVector3(), "motion_sensor_trigger", 0f);
					}
					base.SendHasLocalChangesToRoot();
				}
				this.lastTriggered = this.isTriggered;
				if (this.IsPowered && !this.isActive && this.delayStartTime == -1f)
				{
					this.lastPowerTime = Time.time;
					this.delayStartTime = -1f;
					switch (this.TriggerPowerDelay)
					{
					case PowerTrigger.TriggerPowerDelayTypes.OneSecond:
						this.delayStartTime = 1f;
						break;
					case PowerTrigger.TriggerPowerDelayTypes.TwoSecond:
						this.delayStartTime = 2f;
						break;
					case PowerTrigger.TriggerPowerDelayTypes.ThreeSecond:
						this.delayStartTime = 3f;
						break;
					case PowerTrigger.TriggerPowerDelayTypes.FourSecond:
						this.delayStartTime = 4f;
						break;
					case PowerTrigger.TriggerPowerDelayTypes.FiveSecond:
						this.delayStartTime = 5f;
						break;
					}
					if (this.delayStartTime == -1f)
					{
						this.isActive = true;
						this.SetupDurationTime();
					}
				}
				this.parentTriggered = false;
			}
		}
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x001A458C File Offset: 0x001A278C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetupDurationTime()
	{
		this.lastPowerTime = Time.time;
		switch (this.TriggerPowerDuration)
		{
		case PowerTrigger.TriggerPowerDurationTypes.Always:
			this.powerTime = -1f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.Triggered:
			this.powerTime = 0f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.OneSecond:
			this.powerTime = 1f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.TwoSecond:
			this.powerTime = 2f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.ThreeSecond:
			this.powerTime = 3f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.FourSecond:
			this.powerTime = 4f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.FiveSecond:
			this.powerTime = 5f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.SixSecond:
			this.powerTime = 6f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.SevenSecond:
			this.powerTime = 7f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.EightSecond:
			this.powerTime = 8f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.NineSecond:
			this.powerTime = 9f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.TenSecond:
			this.powerTime = 10f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.FifteenSecond:
			this.powerTime = 15f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.ThirtySecond:
			this.powerTime = 30f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.FourtyFiveSecond:
			this.powerTime = 45f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.OneMinute:
			this.powerTime = 60f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.FiveMinute:
			this.powerTime = 300f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.TenMinute:
			this.powerTime = 600f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.ThirtyMinute:
			this.powerTime = 1800f;
			return;
		case PowerTrigger.TriggerPowerDurationTypes.SixtyMinute:
			this.powerTime = 3600f;
			return;
		default:
			return;
		}
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x001A4720 File Offset: 0x001A2920
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
				if (this.Children[i] is PowerTrigger)
				{
					PowerTrigger powerTrigger = this.Children[i] as PowerTrigger;
					this.HandleParentTriggering(powerTrigger);
					if ((this.TriggerType == PowerTrigger.TriggerTypes.Motion || this.TriggerType == PowerTrigger.TriggerTypes.PressurePlate || this.TriggerType == PowerTrigger.TriggerTypes.TripWire) && (powerTrigger.TriggerType == PowerTrigger.TriggerTypes.Motion || powerTrigger.TriggerType == PowerTrigger.TriggerTypes.PressurePlate || powerTrigger.TriggerType == PowerTrigger.TriggerTypes.TripWire))
					{
						powerTrigger.HandlePowerReceived(ref power);
					}
					else if (this.IsActive)
					{
						powerTrigger.HandlePowerReceived(ref power);
					}
				}
				else if (this.IsActive)
				{
					this.Children[i].HandlePowerReceived(ref power);
				}
				if (power <= 0)
				{
					return;
				}
			}
		}
	}

	// Token: 0x06004317 RID: 17175 RVA: 0x001A482F File Offset: 0x001A2A2F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void CheckForActiveChange()
	{
		if (this.powerTime == 0f && this.lastTriggered && !this.isTriggered)
		{
			this.isActive = false;
			this.HandleDisconnectChildren();
			base.SendHasLocalChangesToRoot();
			this.powerTime = -1f;
		}
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x001A486C File Offset: 0x001A2A6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleSingleUseDisable()
	{
		PowerTrigger.TriggerTypes triggerType = this.TriggerType;
		if (triggerType == PowerTrigger.TriggerTypes.PressurePlate || triggerType - PowerTrigger.TriggerTypes.Motion <= 1)
		{
			this.lastTriggered = this.isTriggered;
			this.isTriggered = false;
		}
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleSoundDisable()
	{
	}

	// Token: 0x0600431A RID: 17178 RVA: 0x001A48A0 File Offset: 0x001A2AA0
	public override void HandlePowerUpdate(bool parentIsOn)
	{
		if (this.TileEntity != null)
		{
			((TileEntityPoweredTrigger)this.TileEntity).Activate(this.isPowered && parentIsOn, this.isTriggered);
			this.TileEntity.SetModified();
		}
		for (int i = 0; i < this.Children.Count; i++)
		{
			if (this.Children[i] is PowerTrigger)
			{
				PowerTrigger powerTrigger = this.Children[i] as PowerTrigger;
				this.HandleParentTriggering(powerTrigger);
				this.Children[i].HandlePowerUpdate(this.isPowered && parentIsOn);
			}
			else if (this.IsActive)
			{
				this.Children[i].HandlePowerUpdate(this.isPowered && parentIsOn);
			}
		}
		this.hasChangesLocal = true;
		this.HandleSingleUseDisable();
	}

	// Token: 0x0600431B RID: 17179 RVA: 0x001A496C File Offset: 0x001A2B6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleParentTriggering(PowerTrigger child)
	{
		if (!this.IsActive)
		{
			child.SetTriggeredByParent(false);
			return;
		}
		if ((this.TriggerType == PowerTrigger.TriggerTypes.Motion || this.TriggerType == PowerTrigger.TriggerTypes.PressurePlate || this.TriggerType == PowerTrigger.TriggerTypes.TripWire) && (child.TriggerType == PowerTrigger.TriggerTypes.Motion || child.TriggerType == PowerTrigger.TriggerTypes.PressurePlate || child.TriggerType == PowerTrigger.TriggerTypes.TripWire))
		{
			child.SetTriggeredByParent(true);
			return;
		}
		child.SetTriggeredByParent(false);
	}

	// Token: 0x0600431C RID: 17180 RVA: 0x001A49CE File Offset: 0x001A2BCE
	public void SetTriggeredByParent(bool triggered)
	{
		this.parentTriggered = triggered;
	}

	// Token: 0x0600431D RID: 17181 RVA: 0x001A49D8 File Offset: 0x001A2BD8
	public virtual void CachedUpdateCall()
	{
		PowerTrigger.TriggerTypes triggerType = this.TriggerType;
		if (triggerType == PowerTrigger.TriggerTypes.PressurePlate || triggerType - PowerTrigger.TriggerTypes.Motion <= 1)
		{
			if (!this.hasChangesLocal)
			{
				if (this.isTriggered != this.lastTriggered)
				{
					base.SendHasLocalChangesToRoot();
				}
				this.CheckForActiveChange();
				this.HandleSingleUseDisable();
			}
			if (this.delayStartTime >= 0f)
			{
				if (Time.time - this.lastPowerTime >= this.delayStartTime)
				{
					base.SendHasLocalChangesToRoot();
					this.delayStartTime = -1f;
					this.isActive = true;
					this.SetupDurationTime();
				}
			}
			else if (this.powerTime > 0f && !this.parentTriggered && Time.time - this.lastPowerTime >= this.powerTime)
			{
				this.isActive = false;
				this.HandleDisconnectChildren();
				base.SendHasLocalChangesToRoot();
				this.powerTime = -1f;
			}
			this.hasChangesLocal = false;
			this.HandleSoundDisable();
		}
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x001A4AB8 File Offset: 0x001A2CB8
	public override void read(PooledBinaryReader _br, byte _version)
	{
		base.read(_br, _version);
		this.TriggerType = (PowerTrigger.TriggerTypes)_br.ReadByte();
		if (this.TriggerType == PowerTrigger.TriggerTypes.Switch)
		{
			this.isTriggered = _br.ReadBoolean();
		}
		else
		{
			this.isActive = _br.ReadBoolean();
		}
		if (this.TriggerType != PowerTrigger.TriggerTypes.Switch)
		{
			this.TriggerPowerDelay = (PowerTrigger.TriggerPowerDelayTypes)_br.ReadByte();
			this.TriggerPowerDuration = (PowerTrigger.TriggerPowerDurationTypes)_br.ReadByte();
			this.delayStartTime = _br.ReadSingle();
			this.powerTime = _br.ReadSingle();
		}
		if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
		{
			this.TargetType = (PowerTrigger.TargetTypes)_br.ReadInt32();
		}
	}

	// Token: 0x0600431F RID: 17183 RVA: 0x001A4B48 File Offset: 0x001A2D48
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.TriggerType);
		if (this.TriggerType == PowerTrigger.TriggerTypes.Switch)
		{
			_bw.Write(this.isTriggered);
		}
		else
		{
			_bw.Write(this.isActive);
		}
		if (this.TriggerType != PowerTrigger.TriggerTypes.Switch)
		{
			_bw.Write((byte)this.TriggerPowerDelay);
			_bw.Write((byte)this.TriggerPowerDuration);
			_bw.Write(this.delayStartTime);
			_bw.Write(this.powerTime);
		}
		if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
		{
			_bw.Write((int)this.TargetType);
		}
	}

	// Token: 0x06004320 RID: 17184 RVA: 0x001A4BDC File Offset: 0x001A2DDC
	public virtual void HandleDisconnectChildren()
	{
		this.HandlePowerUpdate(false);
		for (int i = 0; i < this.Children.Count; i++)
		{
			this.Children[i].HandleDisconnect();
		}
	}

	// Token: 0x06004321 RID: 17185 RVA: 0x001A4C18 File Offset: 0x001A2E18
	public override void HandleDisconnect()
	{
		this.parentTriggered = (this.isActive = false);
		base.HandleDisconnect();
	}

	// Token: 0x06004322 RID: 17186 RVA: 0x001A4C3B File Offset: 0x001A2E3B
	public void ResetTrigger()
	{
		this.delayStartTime = -1f;
		this.powerTime = -1f;
		this.isActive = false;
		this.HandleDisconnectChildren();
		base.SendHasLocalChangesToRoot();
	}

	// Token: 0x04003661 RID: 13921
	public PowerTrigger.TriggerTypes TriggerType;

	// Token: 0x04003662 RID: 13922
	public byte Parameter;

	// Token: 0x04003663 RID: 13923
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDelayTypes triggerPowerDelay;

	// Token: 0x04003664 RID: 13924
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDurationTypes triggerPowerDuration = PowerTrigger.TriggerPowerDurationTypes.Triggered;

	// Token: 0x04003665 RID: 13925
	public PowerTrigger.TargetTypes TargetType = PowerTrigger.TargetTypes.Self | PowerTrigger.TargetTypes.Allies;

	// Token: 0x04003666 RID: 13926
	[PublicizedFrom(EAccessModifier.Protected)]
	public float delayStartTime = -1f;

	// Token: 0x04003667 RID: 13927
	[PublicizedFrom(EAccessModifier.Protected)]
	public float powerTime;

	// Token: 0x04003668 RID: 13928
	[PublicizedFrom(EAccessModifier.Protected)]
	public float lastPowerTime = -1f;

	// Token: 0x04003669 RID: 13929
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastTriggered;

	// Token: 0x0400366A RID: 13930
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isTriggered;

	// Token: 0x0400366B RID: 13931
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parentTriggered;

	// Token: 0x0400366C RID: 13932
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isActive;

	// Token: 0x0200091B RID: 2331
	public enum TriggerTypes
	{
		// Token: 0x0400366E RID: 13934
		Switch,
		// Token: 0x0400366F RID: 13935
		PressurePlate,
		// Token: 0x04003670 RID: 13936
		TimerRelay,
		// Token: 0x04003671 RID: 13937
		Motion,
		// Token: 0x04003672 RID: 13938
		TripWire
	}

	// Token: 0x0200091C RID: 2332
	public enum TriggerPowerDelayTypes
	{
		// Token: 0x04003674 RID: 13940
		Instant,
		// Token: 0x04003675 RID: 13941
		OneSecond,
		// Token: 0x04003676 RID: 13942
		TwoSecond,
		// Token: 0x04003677 RID: 13943
		ThreeSecond,
		// Token: 0x04003678 RID: 13944
		FourSecond,
		// Token: 0x04003679 RID: 13945
		FiveSecond
	}

	// Token: 0x0200091D RID: 2333
	public enum TriggerPowerDurationTypes
	{
		// Token: 0x0400367B RID: 13947
		Always,
		// Token: 0x0400367C RID: 13948
		Triggered,
		// Token: 0x0400367D RID: 13949
		OneSecond,
		// Token: 0x0400367E RID: 13950
		TwoSecond,
		// Token: 0x0400367F RID: 13951
		ThreeSecond,
		// Token: 0x04003680 RID: 13952
		FourSecond,
		// Token: 0x04003681 RID: 13953
		FiveSecond,
		// Token: 0x04003682 RID: 13954
		SixSecond,
		// Token: 0x04003683 RID: 13955
		SevenSecond,
		// Token: 0x04003684 RID: 13956
		EightSecond,
		// Token: 0x04003685 RID: 13957
		NineSecond,
		// Token: 0x04003686 RID: 13958
		TenSecond,
		// Token: 0x04003687 RID: 13959
		FifteenSecond,
		// Token: 0x04003688 RID: 13960
		ThirtySecond,
		// Token: 0x04003689 RID: 13961
		FourtyFiveSecond,
		// Token: 0x0400368A RID: 13962
		OneMinute,
		// Token: 0x0400368B RID: 13963
		FiveMinute,
		// Token: 0x0400368C RID: 13964
		TenMinute,
		// Token: 0x0400368D RID: 13965
		ThirtyMinute,
		// Token: 0x0400368E RID: 13966
		SixtyMinute
	}

	// Token: 0x0200091E RID: 2334
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x04003690 RID: 13968
		None = 0,
		// Token: 0x04003691 RID: 13969
		Self = 1,
		// Token: 0x04003692 RID: 13970
		Allies = 2,
		// Token: 0x04003693 RID: 13971
		Strangers = 4,
		// Token: 0x04003694 RID: 13972
		Zombies = 8
	}
}
