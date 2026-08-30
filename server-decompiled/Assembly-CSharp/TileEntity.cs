using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C54 RID: 3156
public abstract class TileEntity : ITileEntity, ILockTarget
{
	// Token: 0x06005FF5 RID: 24565 RVA: 0x0025F2ED File Offset: 0x0025D4ED
	public bool UseLocalVersioning()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
			return false;
		}
		return this.readVersion >= 18;
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x0025F311 File Offset: 0x0025D511
	public int GetLegacyForkVersion()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
		}
		return this.readVersion;
	}

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x06005FF7 RID: 24567 RVA: 0x0025F32C File Offset: 0x0025D52C
	// (set) Token: 0x06005FF8 RID: 24568 RVA: 0x0025F334 File Offset: 0x0025D534
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

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x06005FF9 RID: 24569 RVA: 0x0025F343 File Offset: 0x0025D543
	public BlockValue blockValue
	{
		get
		{
			return this.chunk.GetBlock(this.localChunkPos);
		}
	}

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x06005FFA RID: 24570 RVA: 0x0025F356 File Offset: 0x0025D556
	public Block block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.chunk.GetBlockId(this.localChunkPos.x, this.localChunkPos.y, this.localChunkPos.z)];
		}
	}

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x06005FFB RID: 24571 RVA: 0x0025F38A File Offset: 0x0025D58A
	// (set) Token: 0x06005FFC RID: 24572 RVA: 0x0025F392 File Offset: 0x0025D592
	public bool IsRemoving { get; set; }

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x06005FFD RID: 24573 RVA: 0x0025F39C File Offset: 0x0025D59C
	// (remove) Token: 0x06005FFE RID: 24574 RVA: 0x0025F3D4 File Offset: 0x0025D5D4
	public event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x06005FFF RID: 24575 RVA: 0x0025F409 File Offset: 0x0025D609
	public List<ITileEntityChangedListener> listeners
	{
		get
		{
			return this._listeners;
		}
	}

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x06006000 RID: 24576 RVA: 0x0025F411 File Offset: 0x0025D611
	public bool bWaitingForServerResponse
	{
		get
		{
			return this.lockHandleWaitingFor != byte.MaxValue;
		}
	}

	// Token: 0x06006001 RID: 24577 RVA: 0x0025F423 File Offset: 0x0025D623
	public TileEntity(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x06006002 RID: 24578 RVA: 0x0025F44F File Offset: 0x0025D64F
	public virtual TileEntity Clone()
	{
		throw new NotImplementedException("Clone() not implemented yet");
	}

	// Token: 0x06006003 RID: 24579 RVA: 0x0025F45B File Offset: 0x0025D65B
	public virtual void CopyFrom(TileEntity _other)
	{
		throw new NotImplementedException("CopyFrom() not implemented yet");
	}

	// Token: 0x06006004 RID: 24580 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateTick(World world)
	{
	}

	// Token: 0x06006005 RID: 24581
	public abstract TileEntityType GetTileEntityType();

	// Token: 0x06006006 RID: 24582 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x06006007 RID: 24583 RVA: 0x0025F468 File Offset: 0x0025D668
	public Vector3i ToWorldPos()
	{
		if (this.chunk != null)
		{
			return new Vector3i(this.chunk.X * 16, this.chunk.Y * 256, this.chunk.Z * 16) + this.localChunkPos;
		}
		return Vector3i.zero;
	}

	// Token: 0x06006008 RID: 24584 RVA: 0x0025F4C0 File Offset: 0x0025D6C0
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

	// Token: 0x06006009 RID: 24585 RVA: 0x0025F5FF File Offset: 0x0025D7FF
	public Chunk GetChunk()
	{
		return this.chunk;
	}

	// Token: 0x0600600A RID: 24586 RVA: 0x0025F607 File Offset: 0x0025D807
	public void SetChunk(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x0600600B RID: 24587 RVA: 0x0025F610 File Offset: 0x0025D810
	public static TileEntity InstantiateFromRead(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode, TileEntityType _type, Chunk _chunk, int[] _blockIdMapping, Func<int, int, int, BlockValue> _getBlock)
	{
		TileEntity tileEntity;
		if (TileEntityLegacyUtils.TryReadLegacyType(_br, _eStreamMode, _type, _getBlock, out tileEntity))
		{
			if (tileEntity != null)
			{
				tileEntity.SetChunk(_chunk);
			}
			return tileEntity;
		}
		switch (_type)
		{
		case TileEntityType.Collector:
			tileEntity = new TileEntityCollector(_chunk);
			goto IL_0103;
		case TileEntityType.VendingMachine:
			tileEntity = new TileEntityVendingMachine(_chunk);
			goto IL_0103;
		case TileEntityType.Forge:
			tileEntity = new TileEntityForge(_chunk);
			goto IL_0103;
		case TileEntityType.Workstation:
			tileEntity = new TileEntityWorkstation(_chunk);
			goto IL_0103;
		case TileEntityType.Powered:
			tileEntity = new TileEntityPoweredBlock(_chunk);
			goto IL_0103;
		case TileEntityType.PowerSource:
			tileEntity = new TileEntityPowerSource(_chunk);
			goto IL_0103;
		case TileEntityType.PowerRangeTrap:
			tileEntity = new TileEntityPoweredRangedTrap(_chunk);
			goto IL_0103;
		case TileEntityType.Light:
			tileEntity = new TileEntityLight(_chunk);
			goto IL_0103;
		case TileEntityType.Trigger:
			tileEntity = new TileEntityPoweredTrigger(_chunk);
			goto IL_0103;
		case TileEntityType.Sleeper:
			tileEntity = new TileEntitySleeper(_chunk);
			goto IL_0103;
		case TileEntityType.PowerMeleeTrap:
			tileEntity = new TileEntityPoweredMeleeTrap(_chunk);
			goto IL_0103;
		case TileEntityType.Composite:
			tileEntity = new TileEntityComposite(_chunk);
			goto IL_0103;
		}
		Log.Warning("Dropping TE with unknown/outdated type: " + _type.ToStringCached<TileEntityType>());
		return null;
		IL_0103:
		TileEntityComposite tileEntityComposite = tileEntity as TileEntityComposite;
		if (tileEntityComposite != null)
		{
			tileEntityComposite.read(_br, _eStreamMode, _blockIdMapping);
			return tileEntityComposite;
		}
		tileEntity.read(_br, _eStreamMode);
		return tileEntity;
	}

	// Token: 0x0600600C RID: 24588 RVA: 0x0025F740 File Offset: 0x0025D940
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

	// Token: 0x0600600D RID: 24589 RVA: 0x0025F7AD File Offset: 0x0025D9AD
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

	// Token: 0x0600600E RID: 24590 RVA: 0x0025F7E0 File Offset: 0x0025D9E0
	public override string ToString()
	{
		return string.Format("[TE] " + this.GetTileEntityType().ToStringCached<TileEntityType>() + "/" + this.ToWorldPos().ToString(), Array.Empty<object>());
	}

	// Token: 0x0600600F RID: 24591 RVA: 0x0025F825 File Offset: 0x0025DA25
	public virtual void OnRemove(World world)
	{
		this.OnDestroy();
	}

	// Token: 0x06006010 RID: 24592 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLoad()
	{
	}

	// Token: 0x06006011 RID: 24593 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnload(World world)
	{
	}

	// Token: 0x06006012 RID: 24594 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnReadComplete()
	{
	}

	// Token: 0x06006013 RID: 24595 RVA: 0x0025F82D File Offset: 0x0025DA2D
	public void SetDisableModifiedCheck(bool _b)
	{
		this.bDisableModifiedCheck = _b;
	}

	// Token: 0x06006014 RID: 24596 RVA: 0x0025F836 File Offset: 0x0025DA36
	public void SetModified()
	{
		this.setModified();
	}

	// Token: 0x06006015 RID: 24597 RVA: 0x0025F83E File Offset: 0x0025DA3E
	public void SetChunkModified()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.chunk != null)
		{
			this.chunk.isModified = true;
		}
	}

	// Token: 0x06006016 RID: 24598 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsActive(World world)
	{
		return true;
	}

	// Token: 0x06006017 RID: 24599 RVA: 0x0025F860 File Offset: 0x0025DA60
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsByWater(World _world, Vector3i _blockPos)
	{
		return _world.IsWater(_blockPos.x, _blockPos.y + 1, _blockPos.z) | _world.IsWater(_blockPos.x + 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x - 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z + 1) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z - 1);
	}

	// Token: 0x06006018 RID: 24600 RVA: 0x0025F8F4 File Offset: 0x0025DAF4
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

	// Token: 0x06006019 RID: 24601 RVA: 0x0025F980 File Offset: 0x0025DB80
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
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, TileEntity.StreamModeWrite.ToClient, byte.MaxValue), true, -1, -1, -1, vector, 192, false);
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

	// Token: 0x0600601A RID: 24602 RVA: 0x0025FA74 File Offset: 0x0025DC74
	public override int GetHashCode()
	{
		return this.ToWorldPos().GetHashCode() & int.MaxValue;
	}

	// Token: 0x0600601B RID: 24603 RVA: 0x0020487E File Offset: 0x00202A7E
	public override bool Equals(object obj)
	{
		return base.Equals(obj) && obj.GetHashCode() == this.GetHashCode();
	}

	// Token: 0x0600601C RID: 24604 RVA: 0x0025FA9C File Offset: 0x0025DC9C
	public void NotifyListeners()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnTileEntityChanged(this);
		}
	}

	// Token: 0x0600601D RID: 24605 RVA: 0x0025FAD1 File Offset: 0x0025DCD1
	public virtual void UpgradeDowngradeFrom(TileEntity _other)
	{
		_other.OnDestroy();
	}

	// Token: 0x0600601E RID: 24606 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
	}

	// Token: 0x0600601F RID: 24607 RVA: 0x0025FAD9 File Offset: 0x0025DCD9
	public virtual void SetUserAccessing(bool _bUserAccessing)
	{
		this.bUserAccessing = _bUserAccessing;
	}

	// Token: 0x06006020 RID: 24608 RVA: 0x0025FAE2 File Offset: 0x0025DCE2
	public bool IsUserAccessing()
	{
		return this.bUserAccessing;
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x00010E62 File Offset: 0x0000F062
	public LockTargetType GetLockTargetType()
	{
		return LockTargetType.TileEntity;
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x06006022 RID: 24610 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual LockTargetType LockTargetType
	{
		get
		{
			return LockTargetType.TileEntity;
		}
	}

	// Token: 0x06006023 RID: 24611 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06006025 RID: 24613 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanLockOnServer(int _lockingPlayerId, ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06006026 RID: 24614 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
	}

	// Token: 0x06006029 RID: 24617 RVA: 0x0025FAEA File Offset: 0x0025DCEA
	public virtual void SetHandle(byte _handle)
	{
		if (this.lockHandleWaitingFor != 255 && this.lockHandleWaitingFor == _handle)
		{
			this.lockHandleWaitingFor = byte.MaxValue;
		}
	}

	// Token: 0x0600602A RID: 24618 RVA: 0x0025FB0D File Offset: 0x0025DD0D
	public virtual void OnDestroy()
	{
		if (this.Destroyed != null)
		{
			this.Destroyed(this);
		}
	}

	// Token: 0x0600602B RID: 24619 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset(FastTags<TagGroup.Global> questTags)
	{
	}

	// Token: 0x04004B1D RID: 19229
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 19;

	// Token: 0x04004B1E RID: 19230
	[PublicizedFrom(EAccessModifier.Private)]
	public int readVersion = -1;

	// Token: 0x04004B1F RID: 19231
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i chunkPos;

	// Token: 0x04004B20 RID: 19232
	[PublicizedFrom(EAccessModifier.Protected)]
	public Chunk chunk;

	// Token: 0x04004B21 RID: 19233
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapLastTime;

	// Token: 0x04004B22 RID: 19234
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapUpdateTime;

	// Token: 0x04004B23 RID: 19235
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDisableModifiedCheck;

	// Token: 0x04004B24 RID: 19236
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUserAccessing;

	// Token: 0x04004B27 RID: 19239
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ITileEntityChangedListener> _listeners = new List<ITileEntityChangedListener>();

	// Token: 0x04004B28 RID: 19240
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handleCounter;

	// Token: 0x04004B29 RID: 19241
	[PublicizedFrom(EAccessModifier.Private)]
	public byte lockHandleWaitingFor = byte.MaxValue;

	// Token: 0x02000C55 RID: 3157
	public enum StreamModeRead
	{
		// Token: 0x04004B2B RID: 19243
		Persistency,
		// Token: 0x04004B2C RID: 19244
		FromServer,
		// Token: 0x04004B2D RID: 19245
		FromClient
	}

	// Token: 0x02000C56 RID: 3158
	public enum StreamModeWrite
	{
		// Token: 0x04004B2F RID: 19247
		Persistency,
		// Token: 0x04004B30 RID: 19248
		ToServer,
		// Token: 0x04004B31 RID: 19249
		ToClient
	}
}
