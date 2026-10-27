using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C82 RID: 3202
public abstract class TileEntity : ITileEntity, ILockTarget, IHasWorldAddress
{
	// Token: 0x060060A2 RID: 24738 RVA: 0x002669B1 File Offset: 0x00264BB1
	public bool UseLocalVersioning()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
			return false;
		}
		return this.readVersion >= 18;
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x002669D5 File Offset: 0x00264BD5
	public int GetLegacyForkVersion()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
		}
		return this.readVersion;
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x060060A4 RID: 24740 RVA: 0x002669F0 File Offset: 0x00264BF0
	// (set) Token: 0x060060A5 RID: 24741 RVA: 0x002669F8 File Offset: 0x00264BF8
	public Vector3i localChunkPos
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.chunkPos;
		}
		set
		{
			this.chunkPos = value;
			this.OnSetLocalChunkPosition();
		}
	}

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x060060A6 RID: 24742 RVA: 0x00266A07 File Offset: 0x00264C07
	public BlockValue blockValue
	{
		get
		{
			return this.chunk.GetBlock(this.localChunkPos);
		}
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x060060A7 RID: 24743 RVA: 0x00266A1A File Offset: 0x00264C1A
	public Block block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.chunk.GetBlockId(this.localChunkPos.x, this.localChunkPos.y, this.localChunkPos.z)];
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x060060A8 RID: 24744 RVA: 0x00266A4E File Offset: 0x00264C4E
	// (set) Token: 0x060060A9 RID: 24745 RVA: 0x00266A56 File Offset: 0x00264C56
	public bool IsRemoving { get; set; }

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x060060AA RID: 24746 RVA: 0x00266A60 File Offset: 0x00264C60
	// (remove) Token: 0x060060AB RID: 24747 RVA: 0x00266A98 File Offset: 0x00264C98
	public event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x060060AC RID: 24748 RVA: 0x00266ACD File Offset: 0x00264CCD
	public List<ITileEntityChangedListener> listeners
	{
		get
		{
			return this._listeners;
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x060060AD RID: 24749 RVA: 0x00266AD5 File Offset: 0x00264CD5
	public bool bWaitingForServerResponse
	{
		get
		{
			return this.lockHandleWaitingFor != byte.MaxValue;
		}
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x00266AE7 File Offset: 0x00264CE7
	public TileEntity(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x00266B13 File Offset: 0x00264D13
	public virtual TileEntity Clone()
	{
		throw new NotImplementedException("Clone() not implemented yet");
	}

	// Token: 0x060060B0 RID: 24752 RVA: 0x00266B1F File Offset: 0x00264D1F
	public virtual void CopyFrom(TileEntity _other)
	{
		throw new NotImplementedException("CopyFrom() not implemented yet");
	}

	// Token: 0x060060B1 RID: 24753 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateTick(World world)
	{
	}

	// Token: 0x060060B2 RID: 24754
	public abstract TileEntityType GetTileEntityType();

	// Token: 0x060060B3 RID: 24755 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x060060B4 RID: 24756 RVA: 0x00266B2C File Offset: 0x00264D2C
	public Vector3i ToWorldPos()
	{
		if (this.chunk != null)
		{
			return new Vector3i(this.chunk.X * 16, this.chunk.Y * 256, this.chunk.Z * 16) + this.localChunkPos;
		}
		return Vector3i.zero;
	}

	// Token: 0x060060B5 RID: 24757 RVA: 0x00266B84 File Offset: 0x00264D84
	public Vector3 ToWorldCenterPos()
	{
		if (this.chunk != null)
		{
			BlockValue blockNoDamage = this.chunk.GetBlockNoDamage(this.chunkPos.x, this.chunkPos.y, this.chunkPos.z);
			Block block = blockNoDamage.Block;
			Vector3 vector;
			vector.x = (float)(this.chunk.X * 16 + this.chunkPos.x);
			vector.y = (float)(this.chunk.Y * 256 + this.chunkPos.y);
			vector.z = (float)(this.chunk.Z * 16 + this.chunkPos.z);
			if (!block.isMultiBlock)
			{
				vector.x += 0.5f;
				vector.y += 0.5f;
				vector.z += 0.5f;
			}
			else
			{
				BlockShapeModelEntity blockShapeModelEntity = block.shape as BlockShapeModelEntity;
				if (blockShapeModelEntity != null)
				{
					Quaternion rotation = blockShapeModelEntity.GetRotation(blockNoDamage);
					vector += blockShapeModelEntity.GetRotatedOffset(block, rotation);
					vector.x += 0.5f;
					vector.z += 0.5f;
				}
			}
			return vector;
		}
		return Vector3.zero;
	}

	// Token: 0x060060B6 RID: 24758 RVA: 0x00266CC3 File Offset: 0x00264EC3
	public Chunk GetChunk()
	{
		return this.chunk;
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x00266CCB File Offset: 0x00264ECB
	public void SetChunk(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x00266CD4 File Offset: 0x00264ED4
	public static TileEntity InstantiateFromRead(PooledBinaryReader _br, StreamModeRead _eStreamMode, TileEntityType _type, Chunk _chunk, int[] _blockIdMapping, Func<int, int, int, BlockValue> _getBlock)
	{
		TileEntity tileEntity;
		if (TileEntityLegacyUtils.TryReadLegacyType(_br, _eStreamMode, _type, _chunk, _getBlock, out tileEntity))
		{
			return tileEntity;
		}
		switch (_type)
		{
		case TileEntityType.Collector:
			tileEntity = new TileEntityCollector(_chunk);
			goto IL_00FA;
		case TileEntityType.VendingMachine:
			tileEntity = new TileEntityVendingMachine(_chunk);
			goto IL_00FA;
		case TileEntityType.Forge:
			tileEntity = new TileEntityForge(_chunk);
			goto IL_00FA;
		case TileEntityType.Workstation:
			tileEntity = new TileEntityWorkstation(_chunk);
			goto IL_00FA;
		case TileEntityType.Powered:
			tileEntity = new TileEntityPoweredBlock(_chunk);
			goto IL_00FA;
		case TileEntityType.PowerSource:
			tileEntity = new TileEntityPowerSource(_chunk);
			goto IL_00FA;
		case TileEntityType.PowerRangeTrap:
			tileEntity = new TileEntityPoweredRangedTrap(_chunk);
			goto IL_00FA;
		case TileEntityType.Light:
			tileEntity = new TileEntityLight(_chunk);
			goto IL_00FA;
		case TileEntityType.Trigger:
			tileEntity = new TileEntityPoweredTrigger(_chunk);
			goto IL_00FA;
		case TileEntityType.Sleeper:
			tileEntity = new TileEntitySleeper(_chunk);
			goto IL_00FA;
		case TileEntityType.PowerMeleeTrap:
			tileEntity = new TileEntityPoweredMeleeTrap(_chunk);
			goto IL_00FA;
		case TileEntityType.Composite:
			tileEntity = new TileEntityComposite(_chunk);
			goto IL_00FA;
		}
		Log.Warning("Dropping TE with unknown/outdated type: " + _type.ToStringCached<TileEntityType>());
		return null;
		IL_00FA:
		TileEntityComposite tileEntityComposite = tileEntity as TileEntityComposite;
		if (tileEntityComposite != null)
		{
			tileEntityComposite.read(_br, _eStreamMode, _blockIdMapping);
			return tileEntityComposite;
		}
		tileEntity.read(_br, _eStreamMode);
		return tileEntity;
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x00266DFC File Offset: 0x00264FFC
	public virtual void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		if (_eStreamMode == StreamModeRead.Persistency)
		{
			this.readVersion = (int)_br.ReadUInt16();
			this.chunkPos = StreamUtils.ReadVector3i(_br);
			if (this.readVersion <= 18)
			{
				_br.ReadInt32();
			}
			if (this.readVersion > 1)
			{
				this.heapMapUpdateTime = _br.ReadUInt64();
				this.heapMapLastTime = this.heapMapUpdateTime - AIDirector.GetActivityWorldTimeDelay();
				return;
			}
		}
		else
		{
			this.chunkPos = StreamUtils.ReadVector3i(_br);
		}
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x00266E69 File Offset: 0x00265069
	public virtual void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(19);
			StreamUtils.Write(_bw, this.chunkPos);
			_bw.Write(this.heapMapUpdateTime);
			return;
		}
		StreamUtils.Write(_bw, this.chunkPos);
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x00266E9C File Offset: 0x0026509C
	public override string ToString()
	{
		return string.Format("[TE] " + this.GetTileEntityType().ToStringCached<TileEntityType>() + "/" + this.ToWorldPos().ToString(), Array.Empty<object>());
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x00266EE1 File Offset: 0x002650E1
	public virtual void OnRemove(World world)
	{
		this.OnDestroy();
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLoad()
	{
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnload(World world)
	{
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnReadComplete()
	{
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x00266EE9 File Offset: 0x002650E9
	public void SetDisableModifiedCheck(bool _b)
	{
		this.bDisableModifiedCheck = _b;
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x00266EF2 File Offset: 0x002650F2
	public void SetModified()
	{
		this.setModified();
	}

	// Token: 0x060060C2 RID: 24770 RVA: 0x00266EFA File Offset: 0x002650FA
	public void SetChunkModified()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.chunk != null)
		{
			this.chunk.isModified = true;
		}
	}

	// Token: 0x060060C3 RID: 24771 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsActive(World world)
	{
		return true;
	}

	// Token: 0x060060C4 RID: 24772 RVA: 0x00266F1C File Offset: 0x0026511C
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsByWater(World _world, Vector3i _blockPos)
	{
		return _world.IsWater(_blockPos.x, _blockPos.y + 1, _blockPos.z) | _world.IsWater(_blockPos.x + 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x - 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z + 1) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z - 1);
	}

	// Token: 0x060060C5 RID: 24773 RVA: 0x00266FB0 File Offset: 0x002651B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void emitHeatMapEvent(World world, EnumAIDirectorChunkEvent eventType)
	{
		if (world.worldTime < this.heapMapLastTime)
		{
			this.heapMapUpdateTime = 0UL;
		}
		if (world.worldTime >= this.heapMapUpdateTime && world.aiDirector != null)
		{
			Vector3i vector3i = this.ToWorldPos();
			Block block = world.GetBlock(vector3i).Block;
			if (block != null)
			{
				world.aiDirector.NotifyActivity(eventType, vector3i, block.HeatMapStrength, 720f);
				this.heapMapLastTime = world.worldTime;
				this.heapMapUpdateTime = world.worldTime + AIDirector.GetActivityWorldTimeDelay();
			}
		}
	}

	// Token: 0x060060C6 RID: 24774 RVA: 0x0026703C File Offset: 0x0026523C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void setModified()
	{
		if (this.bDisableModifiedCheck)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.SetChunkModified();
			Vector3? vector = new Vector3?(this.ToWorldCenterPos());
			if (vector.Value == Vector3.zero)
			{
				vector = null;
			}
			if (!this.chunk.NeedsLightCalculation)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, StreamModeWrite.ToClient, byte.MaxValue), true, -1, -1, -1, vector, 192, false);
			}
		}
		else
		{
			byte b = this.handleCounter + 1;
			this.handleCounter = b;
			if (b == 255)
			{
				this.handleCounter = 0;
			}
			this.lockHandleWaitingFor = this.handleCounter;
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, StreamModeWrite.ToServer, this.lockHandleWaitingFor), false);
		}
		this.NotifyListeners();
		if (GameManager.Instance.IsEditMode() && this.chunk != null && this.block.IsTileEntitySavedInPrefab())
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x060060C7 RID: 24775 RVA: 0x00267140 File Offset: 0x00265340
	public override int GetHashCode()
	{
		return this.ToWorldPos().GetHashCode() & int.MaxValue;
	}

	// Token: 0x060060C8 RID: 24776 RVA: 0x0020BCDA File Offset: 0x00209EDA
	public override bool Equals(object obj)
	{
		return base.Equals(obj) && obj.GetHashCode() == this.GetHashCode();
	}

	// Token: 0x060060C9 RID: 24777 RVA: 0x00267168 File Offset: 0x00265368
	public void NotifyListeners()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnTileEntityChanged(this);
		}
	}

	// Token: 0x060060CA RID: 24778 RVA: 0x0026719D File Offset: 0x0026539D
	public virtual void UpgradeDowngradeFrom(TileEntity _other)
	{
		_other.OnDestroy();
	}

	// Token: 0x060060CB RID: 24779 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
	}

	// Token: 0x060060CC RID: 24780 RVA: 0x002671A5 File Offset: 0x002653A5
	public virtual void SetUserAccessing(bool _bUserAccessing)
	{
		this.bUserAccessing = _bUserAccessing;
	}

	// Token: 0x060060CD RID: 24781 RVA: 0x002671AE File Offset: 0x002653AE
	public bool IsUserAccessing()
	{
		return this.bUserAccessing;
	}

	// Token: 0x060060CE RID: 24782 RVA: 0x002671B6 File Offset: 0x002653B6
	public virtual WorldAddress GetAddress()
	{
		return WorldAddress.CreateForTileEntity(this.ToWorldPos());
	}

	// Token: 0x060060CF RID: 24783 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x060060D0 RID: 24784 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockRequestLocal(PooledBinaryWriter _bwLocalContext, ushort _channel)
	{
	}

	// Token: 0x060060D1 RID: 24785 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool OnLockRequestServer(int _lockingPlayerID, PooledBinaryReader _brLocalContext, ushort _channel)
	{
		return true;
	}

	// Token: 0x060060D2 RID: 24786 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
	}

	// Token: 0x060060D3 RID: 24787 RVA: 0x00045D74 File Offset: 0x00043F74
	public virtual bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x060060D4 RID: 24788 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
	}

	// Token: 0x060060D5 RID: 24789 RVA: 0x002671C3 File Offset: 0x002653C3
	public virtual void SetHandle(byte _handle)
	{
		if (this.lockHandleWaitingFor != 255 && this.lockHandleWaitingFor == _handle)
		{
			this.lockHandleWaitingFor = byte.MaxValue;
		}
	}

	// Token: 0x060060D6 RID: 24790 RVA: 0x002671E6 File Offset: 0x002653E6
	public virtual void OnDestroy()
	{
		if (this.Destroyed != null)
		{
			this.Destroyed(this);
		}
	}

	// Token: 0x060060D7 RID: 24791 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset(FastTags<TagGroup.Global> questTags)
	{
	}

	// Token: 0x04004C1E RID: 19486
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 19;

	// Token: 0x04004C1F RID: 19487
	[PublicizedFrom(EAccessModifier.Private)]
	public int readVersion = -1;

	// Token: 0x04004C20 RID: 19488
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i chunkPos;

	// Token: 0x04004C21 RID: 19489
	[PublicizedFrom(EAccessModifier.Protected)]
	public Chunk chunk;

	// Token: 0x04004C22 RID: 19490
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapLastTime;

	// Token: 0x04004C23 RID: 19491
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapUpdateTime;

	// Token: 0x04004C24 RID: 19492
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDisableModifiedCheck;

	// Token: 0x04004C25 RID: 19493
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUserAccessing;

	// Token: 0x04004C28 RID: 19496
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ITileEntityChangedListener> _listeners = new List<ITileEntityChangedListener>();

	// Token: 0x04004C29 RID: 19497
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handleCounter;

	// Token: 0x04004C2A RID: 19498
	[PublicizedFrom(EAccessModifier.Private)]
	public byte lockHandleWaitingFor = byte.MaxValue;
}
