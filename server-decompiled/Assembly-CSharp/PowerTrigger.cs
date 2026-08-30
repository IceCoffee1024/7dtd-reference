using System;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x02000909 RID: 2313
public class PowerTrigger : PowerConsumer
{
	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06004314 RID: 17172 RVA: 0x00046C7A File Offset: 0x00044E7A
	public override PowerItem.PowerItemTypes PowerItemType
	{
		get
		{
			return PowerItem.PowerItemTypes.Trigger;
		}
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06004315 RID: 17173 RVA: 0x001A21D0 File Offset: 0x001A03D0
	// (set) Token: 0x06004316 RID: 17174 RVA: 0x001A21D8 File Offset: 0x001A03D8
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

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06004317 RID: 17175 RVA: 0x001A21E1 File Offset: 0x001A03E1
	// (set) Token: 0x06004318 RID: 17176 RVA: 0x001A21E9 File Offset: 0x001A03E9
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

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06004319 RID: 17177 RVA: 0x001A21F2 File Offset: 0x001A03F2
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

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x0600431A RID: 17178 RVA: 0x001A1F39 File Offset: 0x001A0139
	// (set) Token: 0x0600431B RID: 17179 RVA: 0x001A2214 File Offset: 0x001A0414
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

	// Token: 0x0600431C RID: 17180 RVA: 0x001A23B0 File Offset: 0x001A05B0
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

	// Token: 0x0600431E RID: 17182 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool PowerChildren()
	{
		return true;
	}

	// Token: 0x0600431F RID: 17183 RVA: 0x001A2544 File Offset: 0x001A0744
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

	// Token: 0x06004320 RID: 17184 RVA: 0x001A2653 File Offset: 0x001A0853
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

	// Token: 0x06004321 RID: 17185 RVA: 0x001A2690 File Offset: 0x001A0890
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

	// Token: 0x06004322 RID: 17186 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleSoundDisable()
	{
	}

	// Token: 0x06004323 RID: 17187 RVA: 0x001A26C4 File Offset: 0x001A08C4
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

	// Token: 0x06004324 RID: 17188 RVA: 0x001A2790 File Offset: 0x001A0990
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

	// Token: 0x06004325 RID: 17189 RVA: 0x001A27F2 File Offset: 0x001A09F2
	public void SetTriggeredByParent(bool triggered)
	{
		this.parentTriggered = triggered;
	}

	// Token: 0x06004326 RID: 17190 RVA: 0x001A27FC File Offset: 0x001A09FC
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

	// Token: 0x06004327 RID: 17191 RVA: 0x001A28DC File Offset: 0x001A0ADC
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

	// Token: 0x06004328 RID: 17192 RVA: 0x001A296C File Offset: 0x001A0B6C
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

	// Token: 0x06004329 RID: 17193 RVA: 0x001A2A00 File Offset: 0x001A0C00
	public virtual void HandleDisconnectChildren()
	{
		this.HandlePowerUpdate(false);
		for (int i = 0; i < this.Children.Count; i++)
		{
			this.Children[i].HandleDisconnect();
		}
	}

	// Token: 0x0600432A RID: 17194 RVA: 0x001A2A3C File Offset: 0x001A0C3C
	public override void HandleDisconnect()
	{
		this.parentTriggered = (this.isActive = false);
		base.HandleDisconnect();
	}

	// Token: 0x0600432B RID: 17195 RVA: 0x001A2A5F File Offset: 0x001A0C5F
	public void ResetTrigger()
	{
		this.delayStartTime = -1f;
		this.powerTime = -1f;
		this.isActive = false;
		this.HandleDisconnectChildren();
		base.SendHasLocalChangesToRoot();
	}

	// Token: 0x040035EC RID: 13804
	public PowerTrigger.TriggerTypes TriggerType;

	// Token: 0x040035ED RID: 13805
	public byte Parameter;

	// Token: 0x040035EE RID: 13806
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDelayTypes triggerPowerDelay;

	// Token: 0x040035EF RID: 13807
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerTrigger.TriggerPowerDurationTypes triggerPowerDuration = PowerTrigger.TriggerPowerDurationTypes.Triggered;

	// Token: 0x040035F0 RID: 13808
	public PowerTrigger.TargetTypes TargetType = PowerTrigger.TargetTypes.Self | PowerTrigger.TargetTypes.Allies;

	// Token: 0x040035F1 RID: 13809
	[PublicizedFrom(EAccessModifier.Protected)]
	public float delayStartTime = -1f;

	// Token: 0x040035F2 RID: 13810
	[PublicizedFrom(EAccessModifier.Protected)]
	public float powerTime;

	// Token: 0x040035F3 RID: 13811
	[PublicizedFrom(EAccessModifier.Protected)]
	public float lastPowerTime = -1f;

	// Token: 0x040035F4 RID: 13812
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lastTriggered;

	// Token: 0x040035F5 RID: 13813
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isTriggered;

	// Token: 0x040035F6 RID: 13814
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parentTriggered;

	// Token: 0x040035F7 RID: 13815
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isActive;

	// Token: 0x0200090A RID: 2314
	public enum TriggerTypes
	{
		// Token: 0x040035F9 RID: 13817
		Switch,
		// Token: 0x040035FA RID: 13818
		PressurePlate,
		// Token: 0x040035FB RID: 13819
		TimerRelay,
		// Token: 0x040035FC RID: 13820
		Motion,
		// Token: 0x040035FD RID: 13821
		TripWire
	}

	// Token: 0x0200090B RID: 2315
	public enum TriggerPowerDelayTypes
	{
		// Token: 0x040035FF RID: 13823
		Instant,
		// Token: 0x04003600 RID: 13824
		OneSecond,
		// Token: 0x04003601 RID: 13825
		TwoSecond,
		// Token: 0x04003602 RID: 13826
		ThreeSecond,
		// Token: 0x04003603 RID: 13827
		FourSecond,
		// Token: 0x04003604 RID: 13828
		FiveSecond
	}

	// Token: 0x0200090C RID: 2316
	public enum TriggerPowerDurationTypes
	{
		// Token: 0x04003606 RID: 13830
		Always,
		// Token: 0x04003607 RID: 13831
		Triggered,
		// Token: 0x04003608 RID: 13832
		OneSecond,
		// Token: 0x04003609 RID: 13833
		TwoSecond,
		// Token: 0x0400360A RID: 13834
		ThreeSecond,
		// Token: 0x0400360B RID: 13835
		FourSecond,
		// Token: 0x0400360C RID: 13836
		FiveSecond,
		// Token: 0x0400360D RID: 13837
		SixSecond,
		// Token: 0x0400360E RID: 13838
		SevenSecond,
		// Token: 0x0400360F RID: 13839
		EightSecond,
		// Token: 0x04003610 RID: 13840
		NineSecond,
		// Token: 0x04003611 RID: 13841
		TenSecond,
		// Token: 0x04003612 RID: 13842
		FifteenSecond,
		// Token: 0x04003613 RID: 13843
		ThirtySecond,
		// Token: 0x04003614 RID: 13844
		FourtyFiveSecond,
		// Token: 0x04003615 RID: 13845
		OneMinute,
		// Token: 0x04003616 RID: 13846
		FiveMinute,
		// Token: 0x04003617 RID: 13847
		TenMinute,
		// Token: 0x04003618 RID: 13848
		ThirtyMinute,
		// Token: 0x04003619 RID: 13849
		SixtyMinute
	}

	// Token: 0x0200090D RID: 2317
	[Flags]
	public enum TargetTypes
	{
		// Token: 0x0400361B RID: 13851
		None = 0,
		// Token: 0x0400361C RID: 13852
		Self = 1,
		// Token: 0x0400361D RID: 13853
		Allies = 2,
		// Token: 0x0400361E RID: 13854
		Strangers = 4,
		// Token: 0x0400361F RID: 13855
		Zombies = 8
	}
}
