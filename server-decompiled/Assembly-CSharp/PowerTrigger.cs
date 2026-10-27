using System;
using Audio;
using UnityEngine;

// Token: 0x02000924 RID: 2340
public class PowerTrigger : PowerConsumer
{
	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x0600432E RID: 17198 RVA: 0x00047076 File Offset: 0x00045276
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Trigger;
		}
	}

	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x0600432F RID: 17199 RVA: 0x001A5454 File Offset: 0x001A3654
	// (set) Token: 0x06004330 RID: 17200 RVA: 0x001A545C File Offset: 0x001A365C
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

	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x06004331 RID: 17201 RVA: 0x001A5465 File Offset: 0x001A3665
	// (set) Token: 0x06004332 RID: 17202 RVA: 0x001A546D File Offset: 0x001A366D
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

	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x06004333 RID: 17203 RVA: 0x001A5476 File Offset: 0x001A3676
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

	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x06004334 RID: 17204 RVA: 0x001A51BD File Offset: 0x001A33BD
	// (set) Token: 0x06004335 RID: 17205 RVA: 0x001A5498 File Offset: 0x001A3698
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

	// Token: 0x06004336 RID: 17206 RVA: 0x001A5634 File Offset: 0x001A3834
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

	// Token: 0x06004338 RID: 17208 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x001A57C8 File Offset: 0x001A39C8
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

	// Token: 0x0600433A RID: 17210 RVA: 0x001A58D7 File Offset: 0x001A3AD7
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

	// Token: 0x0600433B RID: 17211 RVA: 0x001A5914 File Offset: 0x001A3B14
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

	// Token: 0x0600433C RID: 17212 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleSoundDisable()
	{
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x001A5948 File Offset: 0x001A3B48
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

	// Token: 0x0600433E RID: 17214 RVA: 0x001A5A14 File Offset: 0x001A3C14
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

	// Token: 0x0600433F RID: 17215 RVA: 0x001A5A76 File Offset: 0x001A3C76
	public void SetTriggeredByParent(bool triggered)
	{
		this.parentTriggered = triggered;
	}

	// Token: 0x06004340 RID: 17216 RVA: 0x001A5A80 File Offset: 0x001A3C80
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

	// Token: 0x06004341 RID: 17217 RVA: 0x001A5B60 File Offset: 0x001A3D60
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

	// Token: 0x06004342 RID: 17218 RVA: 0x001A5BF0 File Offset: 0x001A3DF0
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

	// Token: 0x06004343 RID: 17219 RVA: 0x001A5C84 File Offset: 0x001A3E84
	public virtual void HandleDisconnectChildren()
	{
		this.HandlePowerUpdate(false);
		for (int i = 0; i < this.Children.Count; i++)
		{
			this.Children[i].HandleDisconnect();
		}
	}

	// Token: 0x06004344 RID: 17220 RVA: 0x001A5CC0 File Offset: 0x001A3EC0
	public override void HandleDisconnect()
	{
		this.parentTriggered = (this.isActive = false);
		base.HandleDisconnect();
	}

	// Token: 0x06004345 RID: 17221 RVA: 0x001A5CE3 File Offset: 0x001A3EE3
	public void ResetTrigger()
	{
		this.delayStartTime = -1f;
		this.powerTime = -1f;
		this.isActive = false;
		this.HandleDisconnectChildren();
		base.SendHasLocalChangesToRoot();
	}

	// Token: 0x04003681 RID: 13953
	public PowerTrigger.TriggerTypes TriggerType;

	// Token: 0x04003682 RID: 13954
	public byte Parameter;

	// Token: 0x04003683 RID: 13955
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDelayTypes triggerPowerDelay;

	// Token: 0x04003684 RID: 13956
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDurationTypes triggerPowerDuration = PowerTrigger.TriggerPowerDurationTypes.Triggered;

	// Token: 0x04003685 RID: 13957
	public PowerTrigger.TargetTypes TargetType = PowerTrigger.TargetTypes.Self | PowerTrigger.TargetTypes.Allies;

	// Token: 0x04003686 RID: 13958
	[PublicizedFrom(EAccessModifier.Protected)]
	public float delayStartTime = -1f;

	// Token: 0x04003687 RID: 13959
	[PublicizedFrom(EAccessModifier.Protected)]
	public float powerTime;

	// Token: 0x04003688 RID: 13960
	[PublicizedFrom(EAccessModifier.Protected)]
	public float lastPowerTime = -1f;

	// Token: 0x04003689 RID: 13961
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastTriggered;

	// Token: 0x0400368A RID: 13962
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isTriggered;

	// Token: 0x0400368B RID: 13963
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parentTriggered;

	// Token: 0x0400368C RID: 13964
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isActive;

	// Token: 0x02000925 RID: 2341
	public enum TriggerTypes
	{
		// Token: 0x0400368E RID: 13966
		Switch,
		// Token: 0x0400368F RID: 13967
		PressurePlate,
		// Token: 0x04003690 RID: 13968
		TimerRelay,
		// Token: 0x04003691 RID: 13969
		Motion,
		// Token: 0x04003692 RID: 13970
		TripWire
	}

	// Token: 0x02000926 RID: 2342
	public enum TriggerPowerDelayTypes
	{
		// Token: 0x04003694 RID: 13972
		Instant,
		// Token: 0x04003695 RID: 13973
		OneSecond,
		// Token: 0x04003696 RID: 13974
		TwoSecond,
		// Token: 0x04003697 RID: 13975
		ThreeSecond,
		// Token: 0x04003698 RID: 13976
		FourSecond,
		// Token: 0x04003699 RID: 13977
		FiveSecond
	}

	// Token: 0x02000927 RID: 2343
	public enum TriggerPowerDurationTypes
	{
		// Token: 0x0400369B RID: 13979
		Always,
		// Token: 0x0400369C RID: 13980
		Triggered,
		// Token: 0x0400369D RID: 13981
		OneSecond,
		// Token: 0x0400369E RID: 13982
		TwoSecond,
		// Token: 0x0400369F RID: 13983
		ThreeSecond,
		// Token: 0x040036A0 RID: 13984
		FourSecond,
		// Token: 0x040036A1 RID: 13985
		FiveSecond,
		// Token: 0x040036A2 RID: 13986
		SixSecond,
		// Token: 0x040036A3 RID: 13987
		SevenSecond,
		// Token: 0x040036A4 RID: 13988
		EightSecond,
		// Token: 0x040036A5 RID: 13989
		NineSecond,
		// Token: 0x040036A6 RID: 13990
		TenSecond,
		// Token: 0x040036A7 RID: 13991
		FifteenSecond,
		// Token: 0x040036A8 RID: 13992
		ThirtySecond,
		// Token: 0x040036A9 RID: 13993
		FourtyFiveSecond,
		// Token: 0x040036AA RID: 13994
		OneMinute,
		// Token: 0x040036AB RID: 13995
		FiveMinute,
		// Token: 0x040036AC RID: 13996
		TenMinute,
		// Token: 0x040036AD RID: 13997
		ThirtyMinute,
		// Token: 0x040036AE RID: 13998
		SixtyMinute
	}

	// Token: 0x02000928 RID: 2344
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x040036B0 RID: 14000
		None = 0,
		// Token: 0x040036B1 RID: 14001
		Self = 1,
		// Token: 0x040036B2 RID: 14002
		Allies = 2,
		// Token: 0x040036B3 RID: 14003
		Strangers = 4,
		// Token: 0x040036B4 RID: 14004
		Zombies = 8
	}
}
