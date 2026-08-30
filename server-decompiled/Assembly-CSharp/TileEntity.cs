using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C66 RID: 3174
public abstract class TileEntity : ITileEntity, ILockTarget
{
	// Token: 0x0600607F RID: 24703 RVA: 0x0026218D File Offset: 0x0026038D
	public bool UseLocalVersioning()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
			return false;
		}
		return this.readVersion >= 18;
	}

	// Token: 0x06006080 RID: 24704 RVA: 0x002621B1 File Offset: 0x002603B1
	public int GetLegacyForkVersion()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
		}
		return this.readVersion;
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x06006081 RID: 24705 RVA: 0x002621CC File Offset: 0x002603CC
	// (set) Token: 0x06006082 RID: 24706 RVA: 0x002621D4 File Offset: 0x002603D4
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

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x06006083 RID: 24707 RVA: 0x002621E3 File Offset: 0x002603E3
	public BlockValue blockValue
	{
		get
		{
			return this.chunk.GetBlock(this.localChunkPos);
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x06006084 RID: 24708 RVA: 0x002621F6 File Offset: 0x002603F6
	public Block block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.chunk.GetBlockId(this.localChunkPos.x, this.localChunkPos.y, this.localChunkPos.z)];
		}
	}

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x06006085 RID: 24709 RVA: 0x0026222A File Offset: 0x0026042A
	// (set) Token: 0x06006086 RID: 24710 RVA: 0x00262232 File Offset: 0x00260432
	public bool IsRemoving { get; set; }

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x06006087 RID: 24711 RVA: 0x0026223C File Offset: 0x0026043C
	// (remove) Token: 0x06006088 RID: 24712 RVA: 0x00262274 File Offset: 0x00260474
	public event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x06006089 RID: 24713 RVA: 0x002622A9 File Offset: 0x002604A9
	public List<ITileEntityChangedListener> listeners
	{
		get
		{
			return this._listeners;
		}
	}

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x0600608A RID: 24714 RVA: 0x002622B1 File Offset: 0x002604B1
	public bool bWaitingForServerResponse
	{
		get
		{
			return this.lockHandleWaitingFor != byte.MaxValue;
		}
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x002622C3 File Offset: 0x002604C3
	public TileEntity(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x0600608C RID: 24716 RVA: 0x002622EF File Offset: 0x002604EF
	public virtual TileEntity Clone()
	{
		throw new NotImplementedException("Clone() not implemented yet");
	}

	// Token: 0x0600608D RID: 24717 RVA: 0x002622FB File Offset: 0x002604FB
	public virtual void CopyFrom(TileEntity _other)
	{
		throw new NotImplementedException("CopyFrom() not implemented yet");
	}

	// Token: 0x0600608E RID: 24718 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateTick(World world)
	{
	}

	// Token: 0x0600608F RID: 24719
	public abstract TileEntityType GetTileEntityType();

	// Token: 0x06006090 RID: 24720 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x06006091 RID: 24721 RVA: 0x00262308 File Offset: 0x00260508
	public Vector3i ToWorldPos()
	{
		if (this.chunk != null)
		{
			return new Vector3i(this.chunk.X * 16, this.chunk.Y * 256, this.chunk.Z * 16) + this.localChunkPos;
		}
		return Vector3i.zero;
	}

	// Token: 0x06006092 RID: 24722 RVA: 0x00262360 File Offset: 0x00260560
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

	// Token: 0x06006093 RID: 24723 RVA: 0x0026249F File Offset: 0x0026069F
	public Chunk GetChunk()
	{
		return this.chunk;
	}

	// Token: 0x06006094 RID: 24724 RVA: 0x002624A7 File Offset: 0x002606A7
	public void SetChunk(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x06006095 RID: 24725 RVA: 0x002624B0 File Offset: 0x002606B0
	public static TileEntity InstantiateFromRead(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode, TileEntityType _type, Chunk _chunk, int[] _blockIdMapping, Func<int, int, int, BlockValue> _getBlock)
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

	// Token: 0x06006096 RID: 24726 RVA: 0x002625D8 File Offset: 0x002607D8
	public virtual void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
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

	// Token: 0x06006097 RID: 24727 RVA: 0x00262645 File Offset: 0x00260845
	public virtual void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(19);
			StreamUtils.Write(_bw, this.chunkPos);
			_bw.Write(this.heapMapUpdateTime);
			return;
		}
		StreamUtils.Write(_bw, this.chunkPos);
	}

	// Token: 0x06006098 RID: 24728 RVA: 0x00262678 File Offset: 0x00260878
	public override string ToString()
	{
		return string.Format("[TE] " + this.GetTileEntityType().ToStringCached<TileEntityType>() + "/" + this.ToWorldPos().ToString(), Array.Empty<object>());
	}

	// Token: 0x06006099 RID: 24729 RVA: 0x002626BD File Offset: 0x002608BD
	public virtual void OnRemove(World world)
	{
		this.OnDestroy();
	}

	// Token: 0x0600609A RID: 24730 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLoad()
	{
	}

	// Token: 0x0600609B RID: 24731 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnload(World world)
	{
	}

	// Token: 0x0600609C RID: 24732 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnReadComplete()
	{
	}

	// Token: 0x0600609D RID: 24733 RVA: 0x002626C5 File Offset: 0x002608C5
	public void SetDisableModifiedCheck(bool _b)
	{
		this.bDisableModifiedCheck = _b;
	}

	// Token: 0x0600609E RID: 24734 RVA: 0x002626CE File Offset: 0x002608CE
	public void SetModified()
	{
		this.setModified();
	}

	// Token: 0x0600609F RID: 24735 RVA: 0x002626D6 File Offset: 0x002608D6
	public void SetChunkModified()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.chunk != null)
		{
			this.chunk.isModified = true;
		}
	}

	// Token: 0x060060A0 RID: 24736 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool IsActive(World world)
	{
		return true;
	}

	// Token: 0x060060A1 RID: 24737 RVA: 0x002626F8 File Offset: 0x002608F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsByWater(World _world, Vector3i _blockPos)
	{
		return _world.IsWater(_blockPos.x, _blockPos.y + 1, _blockPos.z) | _world.IsWater(_blockPos.x + 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x - 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z + 1) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z - 1);
	}

	// Token: 0x060060A2 RID: 24738 RVA: 0x0026278C File Offset: 0x0026098C
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

	// Token: 0x060060A3 RID: 24739 RVA: 0x00262818 File Offset: 0x00260A18
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
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, TileEntity.StreamModeWrite.ToClient, byte.MaxValue), true, -1, -1, -1, vector, 192, false);
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
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, TileEntity.StreamModeWrite.ToServer, this.lockHandleWaitingFor), false);
		}
		this.NotifyListeners();
		if (GameManager.Instance.IsEditMode() && this.chunk != null && this.block.IsTileEntitySavedInPrefab())
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x0026291C File Offset: 0x00260B1C
	public override int GetHashCode()
	{
		return this.ToWorldPos().GetHashCode() & int.MaxValue;
	}

	// Token: 0x060060A5 RID: 24741 RVA: 0x002074EE File Offset: 0x002056EE
	public override bool Equals(object obj)
	{
		return base.Equals(obj) && obj.GetHashCode() == this.GetHashCode();
	}

	// Token: 0x060060A6 RID: 24742 RVA: 0x00262944 File Offset: 0x00260B44
	public void NotifyListeners()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnTileEntityChanged(this);
		}
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x00262979 File Offset: 0x00260B79
	public virtual void UpgradeDowngradeFrom(TileEntity _other)
	{
		_other.OnDestroy();
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
	}

	// Token: 0x060060A9 RID: 24745 RVA: 0x00262981 File Offset: 0x00260B81
	public virtual void SetUserAccessing(bool _bUserAccessing)
	{
		this.bUserAccessing = _bUserAccessing;
	}

	// Token: 0x060060AA RID: 24746 RVA: 0x0026298A File Offset: 0x00260B8A
	public bool IsUserAccessing()
	{
		return this.bUserAccessing;
	}

	// Token: 0x060060AB RID: 24747 RVA: 0x00010E62 File Offset: 0x0000F062
	public LockTargetType GetLockTargetType()
	{
		return LockTargetType.TileEntity;
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x060060AC RID: 24748 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual LockTargetType LockTargetType
	{
		get
		{
			return LockTargetType.TileEntity;
		}
	}

	// Token: 0x060060AD RID: 24749 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool CanLockOnServer(int _lockingPlayerId, ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x060060B0 RID: 24752 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x060060B1 RID: 24753 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
	}

	// Token: 0x060060B3 RID: 24755 RVA: 0x00262992 File Offset: 0x00260B92
	public virtual void SetHandle(byte _handle)
	{
		if (this.lockHandleWaitingFor != 255 && this.lockHandleWaitingFor == _handle)
		{
			this.lockHandleWaitingFor = byte.MaxValue;
		}
	}

	// Token: 0x060060B4 RID: 24756 RVA: 0x002629B5 File Offset: 0x00260BB5
	public virtual void OnDestroy()
	{
		if (this.Destroyed != null)
		{
			this.Destroyed(this);
		}
	}

	// Token: 0x060060B5 RID: 24757 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset(FastTags<TagGroup.Global> questTags)
	{
	}

	// Token: 0x04004BBA RID: 19386
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 19;

	// Token: 0x04004BBB RID: 19387
	[PublicizedFrom(EAccessModifier.Private)]
	public int readVersion = -1;

	// Token: 0x04004BBC RID: 19388
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i chunkPos;

	// Token: 0x04004BBD RID: 19389
	[PublicizedFrom(EAccessModifier.Protected)]
	public Chunk chunk;

	// Token: 0x04004BBE RID: 19390
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapLastTime;

	// Token: 0x04004BBF RID: 19391
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapUpdateTime;

	// Token: 0x04004BC0 RID: 19392
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDisableModifiedCheck;

	// Token: 0x04004BC1 RID: 19393
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUserAccessing;

	// Token: 0x04004BC4 RID: 19396
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ITileEntityChangedListener> _listeners = new List<ITileEntityChangedListener>();

	// Token: 0x04004BC5 RID: 19397
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handleCounter;

	// Token: 0x04004BC6 RID: 19398
	[PublicizedFrom(EAccessModifier.Private)]
	public byte lockHandleWaitingFor = byte.MaxValue;

	// Token: 0x02000C67 RID: 3175
	public enum StreamModeRead
	{
		// Token: 0x04004BC8 RID: 19400
		Persistency,
		// Token: 0x04004BC9 RID: 19401
		FromServer,
		// Token: 0x04004BCA RID: 19402
		FromClient
	}

	// Token: 0x02000C68 RID: 3176
	public enum StreamModeWrite
	{
		// Token: 0x04004BCC RID: 19404
		Persistency,
		// Token: 0x04004BCD RID: 19405
		ToServer,
		// Token: 0x04004BCE RID: 19406
		ToClient
	}
}
