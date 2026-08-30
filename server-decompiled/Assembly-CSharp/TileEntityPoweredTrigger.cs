using System;

// Token: 0x02000C77 RID: 3191
public class TileEntityPoweredTrigger : TileEntityPowered
{
	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x060061A1 RID: 24993 RVA: 0x00268363 File Offset: 0x00266563
	// (set) Token: 0x060061A2 RID: 24994 RVA: 0x00268378 File Offset: 0x00266578
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

	// Token: 0x060061A3 RID: 24995 RVA: 0x002683AD File Offset: 0x002665AD
	public TileEntityPoweredTrigger(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x060061A4 RID: 24996 RVA: 0x002683C1 File Offset: 0x002665C1
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061A5 RID: 24997 RVA: 0x002683D4 File Offset: 0x002665D4
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061A6 RID: 24998 RVA: 0x002683DC File Offset: 0x002665DC
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		base.SetModified();
	}

	// Token: 0x060061A7 RID: 24999 RVA: 0x002683EC File Offset: 0x002665EC
	public bool Activate(bool activated, bool isOn)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, isOn, activated);
	}

	// Token: 0x060061A8 RID: 25000 RVA: 0x0026842C File Offset: 0x0026662C
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

	// Token: 0x060061A9 RID: 25001 RVA: 0x00268534 File Offset: 0x00266734
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

	// Token: 0x060061AA RID: 25002 RVA: 0x002686D9 File Offset: 0x002668D9
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

	// Token: 0x060061AB RID: 25003 RVA: 0x00268714 File Offset: 0x00266914
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

	// Token: 0x060061AC RID: 25004 RVA: 0x00048AAD File Offset: 0x00046CAD
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x060061AD RID: 25005 RVA: 0x002688A8 File Offset: 0x00266AA8
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

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x060061AE RID: 25006 RVA: 0x00268920 File Offset: 0x00266B20
	// (set) Token: 0x060061AF RID: 25007 RVA: 0x00268970 File Offset: 0x00266B70
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

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x060061B0 RID: 25008 RVA: 0x002689C4 File Offset: 0x00266BC4
	// (set) Token: 0x060061B1 RID: 25009 RVA: 0x00268A14 File Offset: 0x00266C14
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

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x060061B2 RID: 25010 RVA: 0x00268A66 File Offset: 0x00266C66
	// (set) Token: 0x060061B3 RID: 25011 RVA: 0x00268A90 File Offset: 0x00266C90
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

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x060061B4 RID: 25012 RVA: 0x00268ABC File Offset: 0x00266CBC
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

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x060061B5 RID: 25013 RVA: 0x00268B10 File Offset: 0x00266D10
	public bool TargetSelf
	{
		get
		{
			return (this.TargetType & 1) == 1;
		}
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x060061B6 RID: 25014 RVA: 0x00268B1D File Offset: 0x00266D1D
	public bool TargetAllies
	{
		get
		{
			return (this.TargetType & 2) == 2;
		}
	}

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x060061B7 RID: 25015 RVA: 0x00268B2A File Offset: 0x00266D2A
	public bool TargetStrangers
	{
		get
		{
			return (this.TargetType & 4) == 4;
		}
	}

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x060061B8 RID: 25016 RVA: 0x00268B37 File Offset: 0x00266D37
	public bool TargetZombies
	{
		get
		{
			return (this.TargetType & 8) == 8;
		}
	}

	// Token: 0x060061B9 RID: 25017 RVA: 0x000ED7F0 File Offset: 0x000EB9F0
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Trigger;
	}

	// Token: 0x04004C3E RID: 19518
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C3F RID: 19519
	public PowerTrigger.TriggerTypes TriggerType;

	// Token: 0x04004C40 RID: 19520
	public TileEntityPoweredTrigger.ClientTriggerData ClientData = new TileEntityPoweredTrigger.ClientTriggerData();

	// Token: 0x04004C41 RID: 19521
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x02000C78 RID: 3192
	public class ClientTriggerData
	{
		// Token: 0x04004C42 RID: 19522
		public byte Property1;

		// Token: 0x04004C43 RID: 19523
		public byte Property2;

		// Token: 0x04004C44 RID: 19524
		public int TargetType = 3;

		// Token: 0x04004C45 RID: 19525
		public bool ShowTriggerOptions;

		// Token: 0x04004C46 RID: 19526
		public bool ResetTrigger;

		// Token: 0x04004C47 RID: 19527
		public bool HasChanges;
	}
}
