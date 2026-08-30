using System;

// Token: 0x02000C65 RID: 3173
public class TileEntityPoweredTrigger : TileEntityPowered
{
	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x06006116 RID: 24854 RVA: 0x00265477 File Offset: 0x00263677
	// (set) Token: 0x06006117 RID: 24855 RVA: 0x0026548C File Offset: 0x0026368C
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

	// Token: 0x06006118 RID: 24856 RVA: 0x002654C1 File Offset: 0x002636C1
	public TileEntityPoweredTrigger(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x002654D5 File Offset: 0x002636D5
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x0600611A RID: 24858 RVA: 0x002654E8 File Offset: 0x002636E8
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x0600611B RID: 24859 RVA: 0x002654F0 File Offset: 0x002636F0
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		base.SetModified();
	}

	// Token: 0x0600611C RID: 24860 RVA: 0x00265500 File Offset: 0x00263700
	public bool Activate(bool activated, bool isOn)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, isOn, activated);
	}

	// Token: 0x0600611D RID: 24861 RVA: 0x00265540 File Offset: 0x00263740
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

	// Token: 0x0600611E RID: 24862 RVA: 0x00265648 File Offset: 0x00263848
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
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
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			if (_eStreamMode == TileEntity.StreamModeRead.FromClient)
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

	// Token: 0x0600611F RID: 24863 RVA: 0x002657ED File Offset: 0x002639ED
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

	// Token: 0x06006120 RID: 24864 RVA: 0x00265828 File Offset: 0x00263A28
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write((byte)this.TriggerType);
		if (this.TriggerType == PowerTrigger.TriggerTypes.Motion)
		{
			this.ownerID.ToStream(_bw, false);
		}
		if (_eStreamMode != TileEntity.StreamModeWrite.Persistency)
		{
			if (_eStreamMode == TileEntity.StreamModeWrite.ToServer)
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

	// Token: 0x06006121 RID: 24865 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06006122 RID: 24866 RVA: 0x002659BC File Offset: 0x00263BBC
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		((XUiC_PowerTriggerWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("powertrigger")).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open("powertrigger", true);
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x06006123 RID: 24867 RVA: 0x00265A34 File Offset: 0x00263C34
	// (set) Token: 0x06006124 RID: 24868 RVA: 0x00265A84 File Offset: 0x00263C84
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

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x06006125 RID: 24869 RVA: 0x00265AD8 File Offset: 0x00263CD8
	// (set) Token: 0x06006126 RID: 24870 RVA: 0x00265B28 File Offset: 0x00263D28
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

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x06006127 RID: 24871 RVA: 0x00265B7A File Offset: 0x00263D7A
	// (set) Token: 0x06006128 RID: 24872 RVA: 0x00265BA4 File Offset: 0x00263DA4
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

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06006129 RID: 24873 RVA: 0x00265BD0 File Offset: 0x00263DD0
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

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x0600612A RID: 24874 RVA: 0x00265C24 File Offset: 0x00263E24
	public bool TargetSelf
	{
		get
		{
			return (this.TargetType & 1) == 1;
		}
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x0600612B RID: 24875 RVA: 0x00265C31 File Offset: 0x00263E31
	public bool TargetAllies
	{
		get
		{
			return (this.TargetType & 2) == 2;
		}
	}

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x0600612C RID: 24876 RVA: 0x00265C3E File Offset: 0x00263E3E
	public bool TargetStrangers
	{
		get
		{
			return (this.TargetType & 4) == 4;
		}
	}

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x0600612D RID: 24877 RVA: 0x00265C4B File Offset: 0x00263E4B
	public bool TargetZombies
	{
		get
		{
			return (this.TargetType & 8) == 8;
		}
	}

	// Token: 0x0600612E RID: 24878 RVA: 0x000ECB9C File Offset: 0x000EAD9C
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Trigger;
	}

	// Token: 0x04004BA1 RID: 19361
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004BA2 RID: 19362
	public PowerTrigger.TriggerTypes TriggerType;

	// Token: 0x04004BA3 RID: 19363
	public TileEntityPoweredTrigger.ClientTriggerData ClientData = new TileEntityPoweredTrigger.ClientTriggerData();

	// Token: 0x04004BA4 RID: 19364
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x02000C66 RID: 3174
	public class ClientTriggerData
	{
		// Token: 0x04004BA5 RID: 19365
		public byte Property1;

		// Token: 0x04004BA6 RID: 19366
		public byte Property2;

		// Token: 0x04004BA7 RID: 19367
		public int TargetType = 3;

		// Token: 0x04004BA8 RID: 19368
		public bool ShowTriggerOptions;

		// Token: 0x04004BA9 RID: 19369
		public bool ResetTrigger;

		// Token: 0x04004BAA RID: 19370
		public bool HasChanges;
	}
}
