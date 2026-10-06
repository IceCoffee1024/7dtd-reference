using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000C08 RID: 3080
public abstract class WorldBase : IBlockAccess, IChunkAccess
{
	// Token: 0x06005D45 RID: 23877
	public abstract IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x06005D46 RID: 23878
	public abstract IChunk GetChunkSync(long chunkKey);

	// Token: 0x06005D47 RID: 23879 RVA: 0x001B1448 File Offset: 0x001AF648
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005D48 RID: 23880 RVA: 0x001B1453 File Offset: 0x001AF653
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005D49 RID: 23881 RVA: 0x001B145C File Offset: 0x001AF65C
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005D4A RID: 23882 RVA: 0x001B1465 File Offset: 0x001AF665
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005D4B RID: 23883 RVA: 0x001B146E File Offset: 0x001AF66E
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005D4C RID: 23884 RVA: 0x001B1478 File Offset: 0x001AF678
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005D4D RID: 23885 RVA: 0x001B1483 File Offset: 0x001AF683
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005D4E RID: 23886 RVA: 0x001B148C File Offset: 0x001AF68C
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x001B1497 File Offset: 0x001AF697
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005D50 RID: 23888
	public abstract void AddFallingBlocks(IList<Vector3i> _list);

	// Token: 0x06005D51 RID: 23889
	public abstract byte GetStability(int worldX, int worldY, int worldZ);

	// Token: 0x06005D52 RID: 23890
	public abstract byte GetStability(Vector3i _pos);

	// Token: 0x06005D53 RID: 23891
	public abstract void SetStability(int worldX, int worldY, int worldZ, byte stab);

	// Token: 0x06005D54 RID: 23892
	public abstract void SetStability(Vector3i _pos, byte stab);

	// Token: 0x06005D55 RID: 23893
	public abstract byte GetHeight(int worldX, int worldZ);

	// Token: 0x06005D56 RID: 23894
	public abstract bool IsWater(int _x, int _y, int _z);

	// Token: 0x06005D57 RID: 23895
	public abstract bool IsWater(Vector3i _pos);

	// Token: 0x06005D58 RID: 23896
	public abstract bool IsWater(Vector3 _pos);

	// Token: 0x06005D59 RID: 23897
	public abstract bool IsAir(int _x, int _y, int _z);

	// Token: 0x06005D5A RID: 23898
	public abstract IGameManager GetGameManager();

	// Token: 0x06005D5B RID: 23899
	public abstract Manager GetAudioManager();

	// Token: 0x06005D5C RID: 23900
	public abstract AIDirector GetAIDirector();

	// Token: 0x06005D5D RID: 23901
	public abstract void SetBlocksRPC(List<BlockChangeInfo> _blockChangeInfo);

	// Token: 0x06005D5E RID: 23902 RVA: 0x00248358 File Offset: 0x00246558
	public void SetBlockRPC(BlockChangeInfo _info)
	{
		this.SetBlocksRPC(new List<BlockChangeInfo> { _info });
	}

	// Token: 0x06005D5F RID: 23903 RVA: 0x0024836C File Offset: 0x0024656C
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true));
	}

	// Token: 0x06005D60 RID: 23904 RVA: 0x0024837C File Offset: 0x0024657C
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density));
	}

	// Token: 0x06005D61 RID: 23905 RVA: 0x0024838C File Offset: 0x0024658C
	public void SetBlockRPC(BlockValueRef _bvRef, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _density, false));
	}

	// Token: 0x06005D62 RID: 23906 RVA: 0x0024839C File Offset: 0x0024659C
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density, _changingEntityId));
	}

	// Token: 0x06005D63 RID: 23907 RVA: 0x002483AE File Offset: 0x002465AE
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true, _changingEntityId));
	}

	// Token: 0x06005D64 RID: 23908
	public abstract void SetPropsRPC(List<PropChangeInfo> _propChangeInfo);

	// Token: 0x06005D65 RID: 23909 RVA: 0x002483BF File Offset: 0x002465BF
	public void SetPropRPC(PropChangeInfo _change)
	{
		this.SetPropsRPC(new List<PropChangeInfo> { _change });
	}

	// Token: 0x06005D66 RID: 23910
	public abstract BlockValue GetBlock(int _x, int _y, int _z);

	// Token: 0x06005D67 RID: 23911 RVA: 0x001B2790 File Offset: 0x001B0990
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x06005D68 RID: 23912 RVA: 0x001B2799 File Offset: 0x001B0999
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005D69 RID: 23913
	public abstract sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x06005D6A RID: 23914 RVA: 0x002483D3 File Offset: 0x002465D3
	public sbyte GetDensity(Vector3i _blockPos)
	{
		return this.GetDensity(_blockPos.x, _blockPos.y, _blockPos.z);
	}

	// Token: 0x06005D6B RID: 23915 RVA: 0x002483F0 File Offset: 0x002465F0
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

	// Token: 0x06005D6C RID: 23916
	public abstract PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x06005D6D RID: 23917
	public abstract PropValue GetProp(long chunkKey, int propId);

	// Token: 0x06005D6E RID: 23918 RVA: 0x001B2925 File Offset: 0x001B0B25
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005D6F RID: 23919 RVA: 0x001B292F File Offset: 0x001B0B2F
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x06005D70 RID: 23920
	public abstract Entity GetEntity(int _entityId);

	// Token: 0x06005D71 RID: 23921
	public abstract void ChangeClientEntityIdToServer(int _clientEntityId, int _serverEntityId);

	// Token: 0x06005D72 RID: 23922
	public abstract bool IsRemote();

	// Token: 0x06005D73 RID: 23923
	public abstract WorldBlockTicker GetWBT();

	// Token: 0x06005D74 RID: 23924
	public abstract bool IsOpenSkyAbove(int _x, int _y, int _z);

	// Token: 0x06005D75 RID: 23925
	public abstract int GetBlockLightValue(Vector3i blockPos);

	// Token: 0x06005D76 RID: 23926
	public abstract float GetLightBrightness(Vector3i blockPos);

	// Token: 0x06005D77 RID: 23927
	public abstract bool IsEditor();

	// Token: 0x06005D78 RID: 23928
	public abstract TileEntity GetTileEntity(Vector3i _blockPos);

	// Token: 0x06005D79 RID: 23929 RVA: 0x00248430 File Offset: 0x00246630
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

	// Token: 0x06005D7A RID: 23930
	public abstract List<EntityPlayer> GetPlayers();

	// Token: 0x06005D7B RID: 23931
	public abstract EntityPlayerLocal GetPrimaryPlayer();

	// Token: 0x06005D7C RID: 23932
	public abstract void AddLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005D7D RID: 23933
	public abstract void RemoveLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005D7E RID: 23934
	public abstract List<EntityPlayerLocal> GetLocalPlayers();

	// Token: 0x06005D7F RID: 23935
	public abstract bool IsLocalPlayer(int _playerId);

	// Token: 0x06005D80 RID: 23936
	public abstract EntityPlayerLocal GetLocalPlayerFromID(int _playerId);

	// Token: 0x06005D81 RID: 23937
	public abstract EntityPlayerLocal GetClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D82 RID: 23938
	public abstract Vector3 GetVectorToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D83 RID: 23939
	public abstract float GetSquaredDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D84 RID: 23940
	public abstract float GetDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D85 RID: 23941
	public abstract bool CanPlaceLandProtectionBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D86 RID: 23942
	public abstract bool CanPlaceBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005D87 RID: 23943
	public abstract bool CanPickupBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D88 RID: 23944
	public abstract float GetLandProtectionHardnessModifier(Vector3i blockPos, EntityAlive lpRelative, PersistentPlayerData ppData);

	// Token: 0x06005D89 RID: 23945
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D8A RID: 23946
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005D8B RID: 23947
	public abstract EnumLandClaimOwner GetLandClaimOwner(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D8C RID: 23948
	public abstract ulong GetWorldTime();

	// Token: 0x06005D8D RID: 23949
	public abstract WorldCreationData GetWorldCreationData();

	// Token: 0x06005D8E RID: 23950
	public abstract void UnloadEntities(List<Entity> _entityList, bool _forceUnload = false);

	// Token: 0x06005D8F RID: 23951
	public abstract Entity RemoveEntity(int _entityId, EnumRemoveEntityReason _reason);

	// Token: 0x06005D90 RID: 23952
	public abstract int AddSleeperVolume(SleeperVolume _volume);

	// Token: 0x06005D91 RID: 23953
	public abstract int FindSleeperVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005D92 RID: 23954
	public abstract void ResetSleeperVolumes(long chunkKey);

	// Token: 0x06005D93 RID: 23955
	public abstract SleeperVolume GetSleeperVolume(int index);

	// Token: 0x06005D94 RID: 23956
	public abstract int AddTriggerVolume(TriggerVolume _volume);

	// Token: 0x06005D95 RID: 23957
	public abstract int FindTriggerVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005D96 RID: 23958
	public abstract void ResetTriggerVolumes(long chunkKey);

	// Token: 0x06005D97 RID: 23959
	public abstract TriggerVolume GetTriggerVolume(int index);

	// Token: 0x06005D98 RID: 23960
	public abstract int AddWallVolume(WallVolume _volume);

	// Token: 0x06005D99 RID: 23961
	public abstract int FindWallVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005D9A RID: 23962
	public abstract List<ValueTuple<int, WallVolume>> GetAllWallVolumes();

	// Token: 0x06005D9B RID: 23963
	public abstract WallVolume GetWallVolume(int index);

	// Token: 0x06005D9C RID: 23964
	public abstract bool HasWallVolumes(List<int> _wallVolumeIds);

	// Token: 0x06005D9D RID: 23965
	public abstract GameRandom GetGameRandom();

	// Token: 0x06005D9E RID: 23966
	public abstract void AddPendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005D9F RID: 23967
	public abstract bool TryRetrieveAndRemovePendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005DA0 RID: 23968 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public WorldBase()
	{
	}

	// Token: 0x0400486C RID: 18540
	public ChunkCluster ChunkCache;
}
