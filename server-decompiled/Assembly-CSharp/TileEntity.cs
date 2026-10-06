using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C77 RID: 3191
public abstract class TileEntity : ITileEntity, ILockTarget, IHasWorldAddress
{
	// Token: 0x0600607D RID: 24701 RVA: 0x00265489 File Offset: 0x00263689
	public bool UseLocalVersioning()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
			return false;
		}
		return this.readVersion >= 18;
	}

	// Token: 0x0600607E RID: 24702 RVA: 0x002654AD File Offset: 0x002636AD
	public int GetLegacyForkVersion()
	{
		if (this.readVersion == -1)
		{
			Log.Error("[TileEntity] read must be called before using this.");
		}
		return this.readVersion;
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x0600607F RID: 24703 RVA: 0x002654C8 File Offset: 0x002636C8
	// (set) Token: 0x06006080 RID: 24704 RVA: 0x002654D0 File Offset: 0x002636D0
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

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06006081 RID: 24705 RVA: 0x002654DF File Offset: 0x002636DF
	public BlockValue blockValue
	{
		get
		{
			return this.chunk.GetBlock(this.localChunkPos);
		}
	}

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06006082 RID: 24706 RVA: 0x002654F2 File Offset: 0x002636F2
	public Block block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return Block.list[this.chunk.GetBlockId(this.localChunkPos.x, this.localChunkPos.y, this.localChunkPos.z)];
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06006083 RID: 24707 RVA: 0x00265526 File Offset: 0x00263726
	// (set) Token: 0x06006084 RID: 24708 RVA: 0x0026552E File Offset: 0x0026372E
	public bool IsRemoving { get; set; }

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x06006085 RID: 24709 RVA: 0x00265538 File Offset: 0x00263738
	// (remove) Token: 0x06006086 RID: 24710 RVA: 0x00265570 File Offset: 0x00263770
	public event XUiEvent_TileEntityDestroyed Destroyed;

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06006087 RID: 24711 RVA: 0x002655A5 File Offset: 0x002637A5
	public List<ITileEntityChangedListener> listeners
	{
		get
		{
			return this._listeners;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06006088 RID: 24712 RVA: 0x002655AD File Offset: 0x002637AD
	public bool bWaitingForServerResponse
	{
		get
		{
			return this.lockHandleWaitingFor != byte.MaxValue;
		}
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x002655BF File Offset: 0x002637BF
	public TileEntity(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x002655EB File Offset: 0x002637EB
	public virtual TileEntity Clone()
	{
		throw new NotImplementedException("Clone() not implemented yet");
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x002655F7 File Offset: 0x002637F7
	public virtual void CopyFrom(TileEntity _other)
	{
		throw new NotImplementedException("CopyFrom() not implemented yet");
	}

	// Token: 0x0600608C RID: 24716 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateTick(World world)
	{
	}

	// Token: 0x0600608D RID: 24717
	public abstract TileEntityType GetTileEntityType();

	// Token: 0x0600608E RID: 24718 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x0600608F RID: 24719 RVA: 0x00265604 File Offset: 0x00263804
	public Vector3i ToWorldPos()
	{
		if (this.chunk != null)
		{
			return new Vector3i(this.chunk.X * 16, this.chunk.Y * 256, this.chunk.Z * 16) + this.localChunkPos;
		}
		return Vector3i.zero;
	}

	// Token: 0x06006090 RID: 24720 RVA: 0x0026565C File Offset: 0x0026385C
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

	// Token: 0x06006091 RID: 24721 RVA: 0x0026579B File Offset: 0x0026399B
	public Chunk GetChunk()
	{
		return this.chunk;
	}

	// Token: 0x06006092 RID: 24722 RVA: 0x002657A3 File Offset: 0x002639A3
	public void SetChunk(Chunk _chunk)
	{
		this.chunk = _chunk;
	}

	// Token: 0x06006093 RID: 24723 RVA: 0x002657AC File Offset: 0x002639AC
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

	// Token: 0x06006094 RID: 24724 RVA: 0x002658D4 File Offset: 0x00263AD4
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

	// Token: 0x06006095 RID: 24725 RVA: 0x00265941 File Offset: 0x00263B41
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

	// Token: 0x06006096 RID: 24726 RVA: 0x00265974 File Offset: 0x00263B74
	public override string ToString()
	{
		return string.Format("[TE] " + this.GetTileEntityType().ToStringCached<TileEntityType>() + "/" + this.ToWorldPos().ToString(), Array.Empty<object>());
	}

	// Token: 0x06006097 RID: 24727 RVA: 0x002659B9 File Offset: 0x00263BB9
	public virtual void OnRemove(World world)
	{
		this.OnDestroy();
	}

	// Token: 0x06006098 RID: 24728 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLoad()
	{
	}

	// Token: 0x06006099 RID: 24729 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnload(World world)
	{
	}

	// Token: 0x0600609A RID: 24730 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnReadComplete()
	{
	}

	// Token: 0x0600609B RID: 24731 RVA: 0x002659C1 File Offset: 0x00263BC1
	public void SetDisableModifiedCheck(bool _b)
	{
		this.bDisableModifiedCheck = _b;
	}

	// Token: 0x0600609C RID: 24732 RVA: 0x002659CA File Offset: 0x00263BCA
	public void SetModified()
	{
		this.setModified();
	}

	// Token: 0x0600609D RID: 24733 RVA: 0x002659D2 File Offset: 0x00263BD2
	public void SetChunkModified()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.chunk != null)
		{
			this.chunk.isModified = true;
		}
	}

	// Token: 0x0600609E RID: 24734 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsActive(World world)
	{
		return true;
	}

	// Token: 0x0600609F RID: 24735 RVA: 0x002659F4 File Offset: 0x00263BF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsByWater(World _world, Vector3i _blockPos)
	{
		return _world.IsWater(_blockPos.x, _blockPos.y + 1, _blockPos.z) | _world.IsWater(_blockPos.x + 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x - 1, _blockPos.y, _blockPos.z) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z + 1) | _world.IsWater(_blockPos.x, _blockPos.y, _blockPos.z - 1);
	}

	// Token: 0x060060A0 RID: 24736 RVA: 0x00265A88 File Offset: 0x00263C88
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

	// Token: 0x060060A1 RID: 24737 RVA: 0x00265B14 File Offset: 0x00263D14
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

	// Token: 0x060060A2 RID: 24738 RVA: 0x00265C18 File Offset: 0x00263E18
	public override int GetHashCode()
	{
		return this.ToWorldPos().GetHashCode() & int.MaxValue;
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x0020A75E File Offset: 0x0020895E
	public override bool Equals(object obj)
	{
		return base.Equals(obj) && obj.GetHashCode() == this.GetHashCode();
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x00265C40 File Offset: 0x00263E40
	public void NotifyListeners()
	{
		for (int i = 0; i < this.listeners.Count; i++)
		{
			this.listeners[i].OnTileEntityChanged(this);
		}
	}

	// Token: 0x060060A5 RID: 24741 RVA: 0x00265C75 File Offset: 0x00263E75
	public virtual void UpgradeDowngradeFrom(TileEntity _other)
	{
		_other.OnDestroy();
	}

	// Token: 0x060060A6 RID: 24742 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x00265C7D File Offset: 0x00263E7D
	public virtual void SetUserAccessing(bool _bUserAccessing)
	{
		this.bUserAccessing = _bUserAccessing;
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x00265C86 File Offset: 0x00263E86
	public bool IsUserAccessing()
	{
		return this.bUserAccessing;
	}

	// Token: 0x060060A9 RID: 24745 RVA: 0x00265C8E File Offset: 0x00263E8E
	public virtual WorldAddress GetAddress()
	{
		return WorldAddress.CreateForTileEntity(this.ToWorldPos());
	}

	// Token: 0x060060AA RID: 24746 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x060060AB RID: 24747 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockRequestLocal(PooledBinaryWriter _bwLocalContext, ushort _channel)
	{
	}

	// Token: 0x060060AC RID: 24748 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool OnLockRequestServer(int _lockingPlayerID, PooledBinaryReader _brLocalContext, ushort _channel)
	{
		return true;
	}

	// Token: 0x060060AD RID: 24749 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x00045DB4 File Offset: 0x00043FB4
	public virtual bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
	}

	// Token: 0x060060B0 RID: 24752 RVA: 0x00265C9B File Offset: 0x00263E9B
	public virtual void SetHandle(byte _handle)
	{
		if (this.lockHandleWaitingFor != 255 && this.lockHandleWaitingFor == _handle)
		{
			this.lockHandleWaitingFor = byte.MaxValue;
		}
	}

	// Token: 0x060060B1 RID: 24753 RVA: 0x00265CBE File Offset: 0x00263EBE
	public virtual void OnDestroy()
	{
		if (this.Destroyed != null)
		{
			this.Destroyed(this);
		}
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Reset(FastTags<TagGroup.Global> questTags)
	{
	}

	// Token: 0x04004BF9 RID: 19449
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 19;

	// Token: 0x04004BFA RID: 19450
	[PublicizedFrom(EAccessModifier.Private)]
	public int readVersion = -1;

	// Token: 0x04004BFB RID: 19451
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i chunkPos;

	// Token: 0x04004BFC RID: 19452
	[PublicizedFrom(EAccessModifier.Protected)]
	public Chunk chunk;

	// Token: 0x04004BFD RID: 19453
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapLastTime;

	// Token: 0x04004BFE RID: 19454
	[PublicizedFrom(EAccessModifier.Protected)]
	public ulong heapMapUpdateTime;

	// Token: 0x04004BFF RID: 19455
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bDisableModifiedCheck;

	// Token: 0x04004C00 RID: 19456
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUserAccessing;

	// Token: 0x04004C03 RID: 19459
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ITileEntityChangedListener> _listeners = new List<ITileEntityChangedListener>();

	// Token: 0x04004C04 RID: 19460
	[PublicizedFrom(EAccessModifier.Private)]
	public byte handleCounter;

	// Token: 0x04004C05 RID: 19461
	[PublicizedFrom(EAccessModifier.Private)]
	public byte lockHandleWaitingFor = byte.MaxValue;
}
