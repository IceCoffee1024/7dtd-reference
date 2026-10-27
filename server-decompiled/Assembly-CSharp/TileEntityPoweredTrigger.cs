using System;

// Token: 0x02000C91 RID: 3217
public class TileEntityPoweredTrigger : TileEntityPowered
{
	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x060061C1 RID: 25025 RVA: 0x0026CD2F File Offset: 0x0026AF2F
	// (set) Token: 0x060061C2 RID: 25026 RVA: 0x0026CD44 File Offset: 0x0026AF44
	public bool IsTriggered
	{
		get
		{
			return ((PowerTrigger)this.PowerItem).IsTriggered;
		}
		set
		{
			PowerTrigger powerTrigger = this.PowerItem as PowerTrigger;
			powerTrigger.IsTriggered = value;
			if (powerTrigger.TriggerType == PowerTrigger.TriggerTypes.PressurePlate)
			{
				(powerTrigger as PowerPressurePlate).Pressed = true;
			}
		}
	}

	// Token: 0x060061C3 RID: 25027 RVA: 0x0026CD79 File Offset: 0x0026AF79
	public TileEntityPoweredTrigger(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x060061C4 RID: 25028 RVA: 0x0026CD8D File Offset: 0x0026AF8D
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061C5 RID: 25029 RVA: 0x0026CDA0 File Offset: 0x0026AFA0
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061C6 RID: 25030 RVA: 0x0026CDA8 File Offset: 0x0026AFA8
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		base.SetModified();
	}

	// Token: 0x060061C7 RID: 25031 RVA: 0x0026CDB8 File Offset: 0x0026AFB8
	public bool Activate(bool activated, bool isOn)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, isOn, activated);
	}

	// Token: 0x060061C8 RID: 25032 RVA: 0x0026CDF8 File Offset: 0x0026AFF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override PowerItem CreatePowerItem()
	{
		BlockPowered blockPowered = (BlockPowered)this.chunk.GetBlock(base.localChunkPos).Block;
		if (blockPowered is BlockPressurePlate)
		{
			this.TriggerType = PowerTrigger.TriggerTypes.PressurePlate;
		}
		else if (blockPowered is BlockMotionSensor)
		{
			this.TriggerType = PowerTrigger.TriggerTypes.Motion;
		}
		else if (blockPowered is BlockTripWire)
		{
			this.TriggerType = PowerTrigger.TriggerTypes.TripWire;
		}
		else if (blockPowered is BlockTimerRelay)
		{
			this.TriggerType = PowerTrigger.TriggerTypes.TimerRelay;
		}
		else if (blockPowered is BlockSwitch)
		{
			this.TriggerType = PowerTrigger.TriggerTypes.Switch;
		}
		if (this.TriggerType == PowerTrigger.TriggerTypes.TimerRelay)
		{
			return new PowerTimerRelay
			{
				TriggerType = this.TriggerType
			};
		}
		if (this.TriggerType == PowerTrigger.TriggerTypes.TripWire)
		{
			return new PowerTripWireRelay
			{
				TriggerType = PowerTrigger.TriggerTypes.TripWire
			};
		}
		if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
		{
			return new PowerTrigger
			{
				TriggerType = this.TriggerType,
				TriggerPowerDuration = PowerTrigger.TriggerPowerDurationTypes.Triggered,
				TriggerPowerDelay = PowerTrigger.TriggerPowerDelayTypes.Instant
			};
		}
		if (this.TriggerType == PowerTrigger.TriggerTypes.PressurePlate)
		{
			return new PowerPressurePlate
			{
				TriggerType = this.TriggerType
			};
		}
		return new PowerTrigger
		{
			TriggerType = this.TriggerType
		};
	}

	// Token: 0x060061C9 RID: 25033 RVA: 0x0026CF00 File Offset: 0x0026B100
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
		{
			if (base.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.GetLegacyForkVersion();
			}
		}
		this.TriggerType = (PowerTrigger.TriggerTypes)_br.ReadByte();
		if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
		{
			this.ownerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		}
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			if (_eStreamMode == StreamModeRead.FromClient)
			{
				if (this.PowerItem == null)
				{
					this.PowerItem = base.CreatePowerItemForTileEntity((ushort)this.chunk.GetBlock(base.localChunkPos).type);
				}
				if (this.TriggerType == PowerTrigger.TriggerTypes.TimerRelay)
				{
					(this.PowerItem as PowerTimerRelay).StartTime = _br.ReadByte();
					(this.PowerItem as PowerTimerRelay).EndTime = _br.ReadByte();
				}
				else if (this.TriggerType != PowerTrigger.TriggerTypes.Switch)
				{
					(this.PowerItem as PowerTrigger).TriggerPowerDelay = (PowerTrigger.TriggerPowerDelayTypes)_br.ReadByte();
					(this.PowerItem as PowerTrigger).TriggerPowerDuration = (PowerTrigger.TriggerPowerDurationTypes)_br.ReadByte();
					if (_br.ReadBoolean())
					{
						(this.PowerItem as PowerTrigger).ResetTrigger();
					}
				}
				if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
				{
					this.TargetType = _br.ReadInt32();
					return;
				}
			}
			else
			{
				if (this.TriggerType == PowerTrigger.TriggerTypes.TripWire)
				{
					this.ClientData.ShowTriggerOptions = _br.ReadBoolean();
					this.ClientData.HasChanges = true;
				}
				if (this.TriggerType != PowerTrigger.TriggerTypes.Switch)
				{
					this.ClientData.Property1 = _br.ReadByte();
					this.ClientData.Property2 = _br.ReadByte();
					this.ClientData.HasChanges = true;
				}
				if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
				{
					int num = _br.ReadInt32();
					if (!this.bUserAccessing)
					{
						this.TargetType = num;
					}
				}
			}
		}
	}

	// Token: 0x060061CA RID: 25034 RVA: 0x0026D0A5 File Offset: 0x0026B2A5
	public void ResetTrigger()
	{
		if (this.TriggerType != PowerTrigger.TriggerTypes.TimerRelay)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				(this.PowerItem as PowerTrigger).ResetTrigger();
				return;
			}
			this.ClientData.ResetTrigger = true;
			base.SetModified();
		}
	}

	// Token: 0x060061CB RID: 25035 RVA: 0x0026D0E0 File Offset: 0x0026B2E0
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write((byte)this.TriggerType);
		if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
		{
			this.ownerID.ToStream(_bw, false);
		}
		if (_eStreamMode != StreamModeWrite.Persistency)
		{
			if (_eStreamMode == StreamModeWrite.ToServer)
			{
				if (this.TriggerType != PowerTrigger.TriggerTypes.Switch)
				{
					_bw.Write(this.ClientData.Property1);
					_bw.Write(this.ClientData.Property2);
					_bw.Write(this.ClientData.ResetTrigger);
					this.ClientData.ResetTrigger = false;
				}
				if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
				{
					_bw.Write(this.TargetType);
					return;
				}
			}
			else
			{
				if (this.PowerItem == null)
				{
					this.PowerItem = base.CreatePowerItemForTileEntity((ushort)this.chunk.GetBlock(base.localChunkPos).type);
				}
				if (this.TriggerType == PowerTrigger.TriggerTypes.TripWire)
				{
					PowerTripWireRelay powerTripWireRelay = this.PowerItem as PowerTripWireRelay;
					_bw.Write(powerTripWireRelay.Parent != null && powerTripWireRelay.Parent is PowerTripWireRelay);
				}
				if (this.TriggerType == PowerTrigger.TriggerTypes.TimerRelay)
				{
					_bw.Write((this.PowerItem as PowerTimerRelay).StartTime);
					_bw.Write((this.PowerItem as PowerTimerRelay).EndTime);
				}
				else if (this.TriggerType != PowerTrigger.TriggerTypes.Switch)
				{
					_bw.Write((byte)(this.PowerItem as PowerTrigger).TriggerPowerDelay);
					_bw.Write((byte)(this.PowerItem as PowerTrigger).TriggerPowerDuration);
				}
				if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
				{
					_bw.Write(this.TargetType);
				}
			}
		}
	}

	// Token: 0x060061CC RID: 25036 RVA: 0x0026D274 File Offset: 0x0026B474
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		((XUiC_PowerTriggerWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("powertrigger")).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open("powertrigger", true);
		return true;
	}

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060061CD RID: 25037 RVA: 0x0026D2EC File Offset: 0x0026B4EC
	// (set) Token: 0x060061CE RID: 25038 RVA: 0x0026D33C File Offset: 0x0026B53C
	public byte Property1
	{
		get
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return this.ClientData.Property1;
			}
			if (this.TriggerType == PowerTrigger.TriggerTypes.TimerRelay)
			{
				return (this.PowerItem as PowerTimerRelay).StartTime;
			}
			return (byte)(this.PowerItem as PowerTrigger).TriggerPowerDelay;
		}
		set
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.ClientData.Property1 = value;
				return;
			}
			if (this.TriggerType == PowerTrigger.TriggerTypes.TimerRelay)
			{
				(this.PowerItem as PowerTimerRelay).StartTime = value;
				return;
			}
			(this.PowerItem as PowerTrigger).TriggerPowerDelay = (PowerTrigger.TriggerPowerDelayTypes)value;
		}
	}

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x060061CF RID: 25039 RVA: 0x0026D390 File Offset: 0x0026B590
	// (set) Token: 0x060061D0 RID: 25040 RVA: 0x0026D3E0 File Offset: 0x0026B5E0
	public byte Property2
	{
		get
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return this.ClientData.Property2;
			}
			if (this.TriggerType == PowerTrigger.TriggerTypes.TimerRelay)
			{
				return (this.PowerItem as PowerTimerRelay).EndTime;
			}
			return (byte)(this.PowerItem as PowerTrigger).TriggerPowerDuration;
		}
		set
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.ClientData.Property2 = value;
				return;
			}
			if (this.TriggerType == PowerTrigger.TriggerTypes.TimerRelay)
			{
				(this.PowerItem as PowerTimerRelay).EndTime = value;
				return;
			}
			(this.PowerItem as PowerTrigger).TriggerPowerDuration = (PowerTrigger.TriggerPowerDurationTypes)value;
		}
	}

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060061D1 RID: 25041 RVA: 0x0026D432 File Offset: 0x0026B632
	// (set) Token: 0x060061D2 RID: 25042 RVA: 0x0026D45C File Offset: 0x0026B65C
	public int TargetType
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (int)(this.PowerItem as PowerTrigger).TargetType;
			}
			return this.ClientData.TargetType;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				(this.PowerItem as PowerTrigger).TargetType = (PowerTrigger.TargetTypes)value;
				return;
			}
			this.ClientData.TargetType = value;
		}
	}

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060061D3 RID: 25043 RVA: 0x0026D488 File Offset: 0x0026B688
	public bool ShowTriggerOptions
	{
		get
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return this.ClientData.ShowTriggerOptions;
			}
			if (this.TriggerType == PowerTrigger.TriggerTypes.TripWire)
			{
				PowerTripWireRelay powerTripWireRelay = this.PowerItem as PowerTripWireRelay;
				return powerTripWireRelay.Parent != null && powerTripWireRelay.Parent is PowerTripWireRelay;
			}
			return true;
		}
	}

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x060061D4 RID: 25044 RVA: 0x0026D4DC File Offset: 0x0026B6DC
	public bool TargetSelf
	{
		get
		{
			return (this.TargetType & 1) == 1;
		}
	}

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x060061D5 RID: 25045 RVA: 0x0026D4E9 File Offset: 0x0026B6E9
	public bool TargetAllies
	{
		get
		{
			return (this.TargetType & 2) == 2;
		}
	}

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x060061D6 RID: 25046 RVA: 0x0026D4F6 File Offset: 0x0026B6F6
	public bool TargetStrangers
	{
		get
		{
			return (this.TargetType & 4) == 4;
		}
	}

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x060061D7 RID: 25047 RVA: 0x0026D503 File Offset: 0x0026B703
	public bool TargetZombies
	{
		get
		{
			return (this.TargetType & 8) == 8;
		}
	}

	// Token: 0x060061D8 RID: 25048 RVA: 0x000EDD14 File Offset: 0x000EBF14
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Trigger;
	}

	// Token: 0x04004C9A RID: 19610
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C9B RID: 19611
	public PowerTrigger.TriggerTypes TriggerType;

	// Token: 0x04004C9C RID: 19612
	public TileEntityPoweredTrigger.ClientTriggerData ClientData = new TileEntityPoweredTrigger.ClientTriggerData();

	// Token: 0x04004C9D RID: 19613
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x02000C92 RID: 3218
	public class ClientTriggerData
	{
		// Token: 0x04004C9E RID: 19614
		public byte Property1;

		// Token: 0x04004C9F RID: 19615
		public byte Property2;

		// Token: 0x04004CA0 RID: 19616
		public int TargetType = 3;

		// Token: 0x04004CA1 RID: 19617
		public bool ShowTriggerOptions;

		// Token: 0x04004CA2 RID: 19618
		public bool ResetTrigger;

		// Token: 0x04004CA3 RID: 19619
		public bool HasChanges;
	}
}
