using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000BE4 RID: 3044
public abstract class WorldBase : IBlockAccess, IChunkAccess
{
	// Token: 0x06005CA2 RID: 23714
	public abstract IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x06005CA3 RID: 23715
	public abstract IChunk GetChunkSync(long chunkKey);

	// Token: 0x06005CA4 RID: 23716 RVA: 0x001AEC68 File Offset: 0x001ACE68
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005CA5 RID: 23717 RVA: 0x001AEC73 File Offset: 0x001ACE73
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005CA6 RID: 23718 RVA: 0x001AEC7C File Offset: 0x001ACE7C
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005CA7 RID: 23719 RVA: 0x001AEC85 File Offset: 0x001ACE85
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005CA8 RID: 23720 RVA: 0x001AEC8E File Offset: 0x001ACE8E
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005CA9 RID: 23721 RVA: 0x001AEC98 File Offset: 0x001ACE98
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005CAA RID: 23722 RVA: 0x001AECA3 File Offset: 0x001ACEA3
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005CAB RID: 23723 RVA: 0x001AECAC File Offset: 0x001ACEAC
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005CAC RID: 23724 RVA: 0x001AECB7 File Offset: 0x001ACEB7
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005CAD RID: 23725
	public abstract void AddFallingBlocks(IList<Vector3i> _list);

	// Token: 0x06005CAE RID: 23726
	public abstract byte GetStability(int worldX, int worldY, int worldZ);

	// Token: 0x06005CAF RID: 23727
	public abstract byte GetStability(Vector3i _pos);

	// Token: 0x06005CB0 RID: 23728
	public abstract void SetStability(int worldX, int worldY, int worldZ, byte stab);

	// Token: 0x06005CB1 RID: 23729
	public abstract void SetStability(Vector3i _pos, byte stab);

	// Token: 0x06005CB2 RID: 23730
	public abstract byte GetHeight(int worldX, int worldZ);

	// Token: 0x06005CB3 RID: 23731
	public abstract bool IsWater(int _x, int _y, int _z);

	// Token: 0x06005CB4 RID: 23732
	public abstract bool IsWater(Vector3i _pos);

	// Token: 0x06005CB5 RID: 23733
	public abstract bool IsWater(Vector3 _pos);

	// Token: 0x06005CB6 RID: 23734
	public abstract bool IsAir(int _x, int _y, int _z);

	// Token: 0x06005CB7 RID: 23735
	public abstract IGameManager GetGameManager();

	// Token: 0x06005CB8 RID: 23736
	public abstract Manager GetAudioManager();

	// Token: 0x06005CB9 RID: 23737
	public abstract AIDirector GetAIDirector();

	// Token: 0x06005CBA RID: 23738
	public abstract void SetBlocksRPC(List<BlockChangeInfo> _blockChangeInfo);

	// Token: 0x06005CBB RID: 23739 RVA: 0x002422B8 File Offset: 0x002404B8
	public void SetBlockRPC(BlockChangeInfo _info)
	{
		this.SetBlocksRPC(new List<BlockChangeInfo> { _info });
	}

	// Token: 0x06005CBC RID: 23740 RVA: 0x002422CC File Offset: 0x002404CC
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true));
	}

	// Token: 0x06005CBD RID: 23741 RVA: 0x002422DC File Offset: 0x002404DC
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density));
	}

	// Token: 0x06005CBE RID: 23742 RVA: 0x002422EC File Offset: 0x002404EC
	public void SetBlockRPC(BlockValueRef _bvRef, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _density, false));
	}

	// Token: 0x06005CBF RID: 23743 RVA: 0x002422FC File Offset: 0x002404FC
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density, _changingEntityId));
	}

	// Token: 0x06005CC0 RID: 23744 RVA: 0x0024230E File Offset: 0x0024050E
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true, _changingEntityId));
	}

	// Token: 0x06005CC1 RID: 23745
	public abstract void SetPropsRPC(List<PropChangeInfo> _propChangeInfo);

	// Token: 0x06005CC2 RID: 23746 RVA: 0x0024231F File Offset: 0x0024051F
	public void SetPropRPC(PropChangeInfo _change)
	{
		this.SetPropsRPC(new List<PropChangeInfo> { _change });
	}

	// Token: 0x06005CC3 RID: 23747
	public abstract BlockValue GetBlock(int _x, int _y, int _z);

	// Token: 0x06005CC4 RID: 23748 RVA: 0x001AFD64 File Offset: 0x001ADF64
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x06005CC5 RID: 23749 RVA: 0x001AFD6D File Offset: 0x001ADF6D
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005CC6 RID: 23750
	public abstract sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x06005CC7 RID: 23751 RVA: 0x00242333 File Offset: 0x00240533
	public sbyte GetDensity(Vector3i _blockPos)
	{
		return this.GetDensity(_blockPos.x, _blockPos.y, _blockPos.z);
	}

	// Token: 0x06005CC8 RID: 23752 RVA: 0x00242350 File Offset: 0x00240550
	public sbyte GetDensity(BlockValueRef _bvRef)
	{
		switch (_bvRef.Type)
		{
		case BlockValueRefType.None:
			return 0;
		case BlockValueRefType.Block:
			return this.GetDensity(_bvRef.BlockPosition);
		case BlockValueRefType.Prop:
			return 0;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06005CC9 RID: 23753
	public abstract PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x06005CCA RID: 23754
	public abstract PropValue GetProp(long chunkKey, int propId);

	// Token: 0x06005CCB RID: 23755 RVA: 0x001AFEF9 File Offset: 0x001AE0F9
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005CCC RID: 23756 RVA: 0x001AFF03 File Offset: 0x001AE103
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x06005CCD RID: 23757
	public abstract Entity GetEntity(int _entityId);

	// Token: 0x06005CCE RID: 23758
	public abstract void ChangeClientEntityIdToServer(int _clientEntityId, int _serverEntityId);

	// Token: 0x06005CCF RID: 23759
	public abstract bool IsRemote();

	// Token: 0x06005CD0 RID: 23760
	public abstract WorldBlockTicker GetWBT();

	// Token: 0x06005CD1 RID: 23761
	public abstract bool IsOpenSkyAbove(int _x, int _y, int _z);

	// Token: 0x06005CD2 RID: 23762
	public abstract int GetBlockLightValue(Vector3i blockPos);

	// Token: 0x06005CD3 RID: 23763
	public abstract float GetLightBrightness(Vector3i blockPos);

	// Token: 0x06005CD4 RID: 23764
	public abstract bool IsEditor();

	// Token: 0x06005CD5 RID: 23765
	public abstract TileEntity GetTileEntity(Vector3i _blockPos);

	// Token: 0x06005CD6 RID: 23766 RVA: 0x00242390 File Offset: 0x00240590
	public TileEntity GetTileEntity(BlockValueRef _bvRef)
	{
		switch (_bvRef.Type)
		{
		case BlockValueRefType.None:
			return null;
		case BlockValueRefType.Block:
			return this.GetTileEntity(_bvRef.BlockPosition);
		case BlockValueRefType.Prop:
			return null;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06005CD7 RID: 23767
	public abstract List<EntityPlayer> GetPlayers();

	// Token: 0x06005CD8 RID: 23768
	public abstract EntityPlayerLocal GetPrimaryPlayer();

	// Token: 0x06005CD9 RID: 23769
	public abstract void AddLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005CDA RID: 23770
	public abstract void RemoveLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005CDB RID: 23771
	public abstract List<EntityPlayerLocal> GetLocalPlayers();

	// Token: 0x06005CDC RID: 23772
	public abstract bool IsLocalPlayer(int _playerId);

	// Token: 0x06005CDD RID: 23773
	public abstract EntityPlayerLocal GetLocalPlayerFromID(int _playerId);

	// Token: 0x06005CDE RID: 23774
	public abstract EntityPlayerLocal GetClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005CDF RID: 23775
	public abstract Vector3 GetVectorToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005CE0 RID: 23776
	public abstract float GetSquaredDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005CE1 RID: 23777
	public abstract float GetDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005CE2 RID: 23778
	public abstract bool CanPlaceLandProtectionBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005CE3 RID: 23779
	public abstract bool CanPlaceBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005CE4 RID: 23780
	public abstract bool CanPickupBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005CE5 RID: 23781
	public abstract float GetLandProtectionHardnessModifier(Vector3i blockPos, EntityAlive lpRelative, PersistentPlayerData ppData);

	// Token: 0x06005CE6 RID: 23782
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005CE7 RID: 23783
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005CE8 RID: 23784
	public abstract EnumLandClaimOwner GetLandClaimOwner(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005CE9 RID: 23785
	public abstract ulong GetWorldTime();

	// Token: 0x06005CEA RID: 23786
	public abstract WorldCreationData GetWorldCreationData();

	// Token: 0x06005CEB RID: 23787
	public abstract void UnloadEntities(List<Entity> _entityList, bool _forceUnload = false);

	// Token: 0x06005CEC RID: 23788
	public abstract Entity RemoveEntity(int _entityId, EnumRemoveEntityReason _reason);

	// Token: 0x06005CED RID: 23789
	public abstract int AddSleeperVolume(SleeperVolume _volume);

	// Token: 0x06005CEE RID: 23790
	public abstract int FindSleeperVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005CEF RID: 23791
	public abstract void ResetSleeperVolumes(long chunkKey);

	// Token: 0x06005CF0 RID: 23792
	public abstract SleeperVolume GetSleeperVolume(int index);

	// Token: 0x06005CF1 RID: 23793
	public abstract int AddTriggerVolume(TriggerVolume _volume);

	// Token: 0x06005CF2 RID: 23794
	public abstract int FindTriggerVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005CF3 RID: 23795
	public abstract void ResetTriggerVolumes(long chunkKey);

	// Token: 0x06005CF4 RID: 23796
	public abstract TriggerVolume GetTriggerVolume(int index);

	// Token: 0x06005CF5 RID: 23797
	public abstract int AddWallVolume(WallVolume _volume);

	// Token: 0x06005CF6 RID: 23798
	public abstract int FindWallVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005CF7 RID: 23799
	public abstract List<ValueTuple<int, WallVolume>> GetAllWallVolumes();

	// Token: 0x06005CF8 RID: 23800
	public abstract WallVolume GetWallVolume(int index);

	// Token: 0x06005CF9 RID: 23801
	public abstract bool HasWallVolumes(List<int> _wallVolumeIds);

	// Token: 0x06005CFA RID: 23802
	public abstract GameRandom GetGameRandom();

	// Token: 0x06005CFB RID: 23803
	public abstract void AddPendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005CFC RID: 23804
	public abstract bool TryRetrieveAndRemovePendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005CFD RID: 23805 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public WorldBase()
	{
	}

	// Token: 0x04004791 RID: 18321
	public ChunkCluster ChunkCache;
}
