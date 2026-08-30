using System;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x02000915 RID: 2325
public class PowerTrigger : PowerConsumer
{
	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06004379 RID: 17273 RVA: 0x00046EDA File Offset: 0x000450DA
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Trigger;
		}
	}

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x0600437A RID: 17274 RVA: 0x001A42A8 File Offset: 0x001A24A8
	// (set) Token: 0x0600437B RID: 17275 RVA: 0x001A42B0 File Offset: 0x001A24B0
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

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x0600437C RID: 17276 RVA: 0x001A42B9 File Offset: 0x001A24B9
	// (set) Token: 0x0600437D RID: 17277 RVA: 0x001A42C1 File Offset: 0x001A24C1
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

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x0600437E RID: 17278 RVA: 0x001A42CA File Offset: 0x001A24CA
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

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x0600437F RID: 17279 RVA: 0x001A4011 File Offset: 0x001A2211
	// (set) Token: 0x06004380 RID: 17280 RVA: 0x001A42EC File Offset: 0x001A24EC
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

	// Token: 0x06004381 RID: 17281 RVA: 0x001A4488 File Offset: 0x001A2688
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

	// Token: 0x06004383 RID: 17283 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x06004384 RID: 17284 RVA: 0x001A461C File Offset: 0x001A281C
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

	// Token: 0x06004385 RID: 17285 RVA: 0x001A472B File Offset: 0x001A292B
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

	// Token: 0x06004386 RID: 17286 RVA: 0x001A4768 File Offset: 0x001A2968
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

	// Token: 0x06004387 RID: 17287 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleSoundDisable()
	{
	}

	// Token: 0x06004388 RID: 17288 RVA: 0x001A479C File Offset: 0x001A299C
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

	// Token: 0x06004389 RID: 17289 RVA: 0x001A4868 File Offset: 0x001A2A68
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

	// Token: 0x0600438A RID: 17290 RVA: 0x001A48CA File Offset: 0x001A2ACA
	public void SetTriggeredByParent(bool triggered)
	{
		this.parentTriggered = triggered;
	}

	// Token: 0x0600438B RID: 17291 RVA: 0x001A48D4 File Offset: 0x001A2AD4
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

	// Token: 0x0600438C RID: 17292 RVA: 0x001A49B4 File Offset: 0x001A2BB4
	public override void read(BinaryReader _br, byte _version)
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

	// Token: 0x0600438D RID: 17293 RVA: 0x001A4A44 File Offset: 0x001A2C44
	public override void write(BinaryWriter _bw)
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

	// Token: 0x0600438E RID: 17294 RVA: 0x001A4AD8 File Offset: 0x001A2CD8
	public virtual void HandleDisconnectChildren()
	{
		this.HandlePowerUpdate(false);
		for (int i = 0; i < this.Children.Count; i++)
		{
			this.Children[i].HandleDisconnect();
		}
	}

	// Token: 0x0600438F RID: 17295 RVA: 0x001A4B14 File Offset: 0x001A2D14
	public override void HandleDisconnect()
	{
		this.parentTriggered = (this.isActive = false);
		base.HandleDisconnect();
	}

	// Token: 0x06004390 RID: 17296 RVA: 0x001A4B37 File Offset: 0x001A2D37
	public void ResetTrigger()
	{
		this.delayStartTime = -1f;
		this.powerTime = -1f;
		this.isActive = false;
		this.HandleDisconnectChildren();
		base.SendHasLocalChangesToRoot();
	}

	// Token: 0x0400366D RID: 13933
	public PowerTrigger.TriggerTypes TriggerType;

	// Token: 0x0400366E RID: 13934
	public byte Parameter;

	// Token: 0x0400366F RID: 13935
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDelayTypes triggerPowerDelay;

	// Token: 0x04003670 RID: 13936
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDurationTypes triggerPowerDuration = PowerTrigger.TriggerPowerDurationTypes.Triggered;

	// Token: 0x04003671 RID: 13937
	public PowerTrigger.TargetTypes TargetType = PowerTrigger.TargetTypes.Self | PowerTrigger.TargetTypes.Allies;

	// Token: 0x04003672 RID: 13938
	[PublicizedFrom(EAccessModifier.Protected)]
	public float delayStartTime = -1f;

	// Token: 0x04003673 RID: 13939
	[PublicizedFrom(EAccessModifier.Protected)]
	public float powerTime;

	// Token: 0x04003674 RID: 13940
	[PublicizedFrom(EAccessModifier.Protected)]
	public float lastPowerTime = -1f;

	// Token: 0x04003675 RID: 13941
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastTriggered;

	// Token: 0x04003676 RID: 13942
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isTriggered;

	// Token: 0x04003677 RID: 13943
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parentTriggered;

	// Token: 0x04003678 RID: 13944
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isActive;

	// Token: 0x02000916 RID: 2326
	public enum TriggerTypes
	{
		// Token: 0x0400367A RID: 13946
		Switch,
		// Token: 0x0400367B RID: 13947
		PressurePlate,
		// Token: 0x0400367C RID: 13948
		TimerRelay,
		// Token: 0x0400367D RID: 13949
		Motion,
		// Token: 0x0400367E RID: 13950
		TripWire
	}

	// Token: 0x02000917 RID: 2327
	public enum TriggerPowerDelayTypes
	{
		// Token: 0x04003680 RID: 13952
		Instant,
		// Token: 0x04003681 RID: 13953
		OneSecond,
		// Token: 0x04003682 RID: 13954
		TwoSecond,
		// Token: 0x04003683 RID: 13955
		ThreeSecond,
		// Token: 0x04003684 RID: 13956
		FourSecond,
		// Token: 0x04003685 RID: 13957
		FiveSecond
	}

	// Token: 0x02000918 RID: 2328
	public enum TriggerPowerDurationTypes
	{
		// Token: 0x04003687 RID: 13959
		Always,
		// Token: 0x04003688 RID: 13960
		Triggered,
		// Token: 0x04003689 RID: 13961
		OneSecond,
		// Token: 0x0400368A RID: 13962
		TwoSecond,
		// Token: 0x0400368B RID: 13963
		ThreeSecond,
		// Token: 0x0400368C RID: 13964
		FourSecond,
		// Token: 0x0400368D RID: 13965
		FiveSecond,
		// Token: 0x0400368E RID: 13966
		SixSecond,
		// Token: 0x0400368F RID: 13967
		SevenSecond,
		// Token: 0x04003690 RID: 13968
		EightSecond,
		// Token: 0x04003691 RID: 13969
		NineSecond,
		// Token: 0x04003692 RID: 13970
		TenSecond,
		// Token: 0x04003693 RID: 13971
		FifteenSecond,
		// Token: 0x04003694 RID: 13972
		ThirtySecond,
		// Token: 0x04003695 RID: 13973
		FourtyFiveSecond,
		// Token: 0x04003696 RID: 13974
		OneMinute,
		// Token: 0x04003697 RID: 13975
		FiveMinute,
		// Token: 0x04003698 RID: 13976
		TenMinute,
		// Token: 0x04003699 RID: 13977
		ThirtyMinute,
		// Token: 0x0400369A RID: 13978
		SixtyMinute
	}

	// Token: 0x02000919 RID: 2329
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x0400369C RID: 13980
		None = 0,
		// Token: 0x0400369D RID: 13981
		Self = 1,
		// Token: 0x0400369E RID: 13982
		Allies = 2,
		// Token: 0x0400369F RID: 13983
		Strangers = 4,
		// Token: 0x040036A0 RID: 13984
		Zombies = 8
	}
}
