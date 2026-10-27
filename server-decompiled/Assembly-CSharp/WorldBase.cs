using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000C13 RID: 3091
public abstract class WorldBase : IBlockAccess, IChunkAccess
{
	// Token: 0x06005D6A RID: 23914
	public abstract IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x06005D6B RID: 23915
	public abstract IChunk GetChunkSync(long chunkKey);

	// Token: 0x06005D6C RID: 23916 RVA: 0x001B24CC File Offset: 0x001B06CC
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005D6D RID: 23917 RVA: 0x001B24D7 File Offset: 0x001B06D7
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005D6E RID: 23918 RVA: 0x001B24E0 File Offset: 0x001B06E0
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005D6F RID: 23919 RVA: 0x001B24E9 File Offset: 0x001B06E9
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005D70 RID: 23920 RVA: 0x001B24F2 File Offset: 0x001B06F2
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x001B24FC File Offset: 0x001B06FC
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005D72 RID: 23922 RVA: 0x001B2507 File Offset: 0x001B0707
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005D73 RID: 23923 RVA: 0x001B2510 File Offset: 0x001B0710
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005D74 RID: 23924 RVA: 0x001B251B File Offset: 0x001B071B
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005D75 RID: 23925
	public abstract void AddFallingBlocks(IList<Vector3i> _list);

	// Token: 0x06005D76 RID: 23926
	public abstract byte GetStability(int worldX, int worldY, int worldZ);

	// Token: 0x06005D77 RID: 23927
	public abstract byte GetStability(Vector3i _pos);

	// Token: 0x06005D78 RID: 23928
	public abstract void SetStability(int worldX, int worldY, int worldZ, byte stab);

	// Token: 0x06005D79 RID: 23929
	public abstract void SetStability(Vector3i _pos, byte stab);

	// Token: 0x06005D7A RID: 23930
	public abstract byte GetHeight(int worldX, int worldZ);

	// Token: 0x06005D7B RID: 23931
	public abstract bool IsWater(int _x, int _y, int _z);

	// Token: 0x06005D7C RID: 23932
	public abstract bool IsWater(Vector3i _pos);

	// Token: 0x06005D7D RID: 23933
	public abstract bool IsWater(Vector3 _pos);

	// Token: 0x06005D7E RID: 23934
	public abstract bool IsAir(int _x, int _y, int _z);

	// Token: 0x06005D7F RID: 23935
	public abstract IGameManager GetGameManager();

	// Token: 0x06005D80 RID: 23936
	public abstract Manager GetAudioManager();

	// Token: 0x06005D81 RID: 23937
	public abstract AIDirector GetAIDirector();

	// Token: 0x06005D82 RID: 23938
	public abstract void SetBlocksRPC(List<BlockChangeInfo> _blockChangeInfo);

	// Token: 0x06005D83 RID: 23939 RVA: 0x00249880 File Offset: 0x00247A80
	public void SetBlockRPC(BlockChangeInfo _info)
	{
		this.SetBlocksRPC(new List<BlockChangeInfo> { _info });
	}

	// Token: 0x06005D84 RID: 23940 RVA: 0x00249894 File Offset: 0x00247A94
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true));
	}

	// Token: 0x06005D85 RID: 23941 RVA: 0x002498A4 File Offset: 0x00247AA4
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density));
	}

	// Token: 0x06005D86 RID: 23942 RVA: 0x002498B4 File Offset: 0x00247AB4
	public void SetBlockRPC(BlockValueRef _bvRef, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _density, false));
	}

	// Token: 0x06005D87 RID: 23943 RVA: 0x002498C4 File Offset: 0x00247AC4
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density, _changingEntityId));
	}

	// Token: 0x06005D88 RID: 23944 RVA: 0x002498D6 File Offset: 0x00247AD6
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true, _changingEntityId));
	}

	// Token: 0x06005D89 RID: 23945
	public abstract void SetPropsRPC(List<PropChangeInfo> _propChangeInfo);

	// Token: 0x06005D8A RID: 23946 RVA: 0x002498E7 File Offset: 0x00247AE7
	public void SetPropRPC(PropChangeInfo _change)
	{
		this.SetPropsRPC(new List<PropChangeInfo> { _change });
	}

	// Token: 0x06005D8B RID: 23947
	public abstract BlockValue GetBlock(int _x, int _y, int _z);

	// Token: 0x06005D8C RID: 23948 RVA: 0x001B3814 File Offset: 0x001B1A14
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x06005D8D RID: 23949 RVA: 0x001B381D File Offset: 0x001B1A1D
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005D8E RID: 23950
	public abstract sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x06005D8F RID: 23951 RVA: 0x002498FB File Offset: 0x00247AFB
	public sbyte GetDensity(Vector3i _blockPos)
	{
		return this.GetDensity(_blockPos.x, _blockPos.y, _blockPos.z);
	}

	// Token: 0x06005D90 RID: 23952 RVA: 0x00249918 File Offset: 0x00247B18
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

	// Token: 0x06005D91 RID: 23953
	public abstract PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x06005D92 RID: 23954
	public abstract PropValue GetProp(long chunkKey, int propId);

	// Token: 0x06005D93 RID: 23955 RVA: 0x001B39A9 File Offset: 0x001B1BA9
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005D94 RID: 23956 RVA: 0x001B39B3 File Offset: 0x001B1BB3
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x06005D95 RID: 23957
	public abstract Entity GetEntity(int _entityId);

	// Token: 0x06005D96 RID: 23958
	public abstract void ChangeClientEntityIdToServer(int _clientEntityId, int _serverEntityId);

	// Token: 0x06005D97 RID: 23959
	public abstract bool IsRemote();

	// Token: 0x06005D98 RID: 23960
	public abstract WorldBlockTicker GetWBT();

	// Token: 0x06005D99 RID: 23961
	public abstract bool IsOpenSkyAbove(int _x, int _y, int _z);

	// Token: 0x06005D9A RID: 23962
	public abstract int GetBlockLightValue(Vector3i blockPos);

	// Token: 0x06005D9B RID: 23963
	public abstract float GetLightBrightness(Vector3i blockPos);

	// Token: 0x06005D9C RID: 23964
	public abstract bool IsEditor();

	// Token: 0x06005D9D RID: 23965
	public abstract TileEntity GetTileEntity(Vector3i _blockPos);

	// Token: 0x06005D9E RID: 23966 RVA: 0x00249958 File Offset: 0x00247B58
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

	// Token: 0x06005D9F RID: 23967
	public abstract List<EntityPlayer> GetPlayers();

	// Token: 0x06005DA0 RID: 23968
	public abstract EntityPlayerLocal GetPrimaryPlayer();

	// Token: 0x06005DA1 RID: 23969
	public abstract void AddLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005DA2 RID: 23970
	public abstract void RemoveLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005DA3 RID: 23971
	public abstract List<EntityPlayerLocal> GetLocalPlayers();

	// Token: 0x06005DA4 RID: 23972
	public abstract bool IsLocalPlayer(int _playerId);

	// Token: 0x06005DA5 RID: 23973
	public abstract EntityPlayerLocal GetLocalPlayerFromID(int _playerId);

	// Token: 0x06005DA6 RID: 23974
	public abstract EntityPlayerLocal GetClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005DA7 RID: 23975
	public abstract Vector3 GetVectorToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005DA8 RID: 23976
	public abstract float GetSquaredDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005DA9 RID: 23977
	public abstract float GetDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005DAA RID: 23978
	public abstract bool CanPlaceLandProtectionBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005DAB RID: 23979
	public abstract bool CanPlaceBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005DAC RID: 23980
	public abstract bool CanPickupBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005DAD RID: 23981
	public abstract float GetLandProtectionHardnessModifier(Vector3i blockPos, EntityAlive lpRelative, PersistentPlayerData ppData);

	// Token: 0x06005DAE RID: 23982
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005DAF RID: 23983
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005DB0 RID: 23984
	public abstract EnumLandClaimOwner GetLandClaimOwner(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005DB1 RID: 23985
	public abstract ulong GetWorldTime();

	// Token: 0x06005DB2 RID: 23986
	public abstract WorldCreationData GetWorldCreationData();

	// Token: 0x06005DB3 RID: 23987
	public abstract void UnloadEntities(List<Entity> _entityList, bool _forceUnload = false);

	// Token: 0x06005DB4 RID: 23988
	public abstract Entity RemoveEntity(int _entityId, EnumRemoveEntityReason _reason);

	// Token: 0x06005DB5 RID: 23989
	public abstract int AddSleeperVolume(SleeperVolume _volume);

	// Token: 0x06005DB6 RID: 23990
	public abstract int FindSleeperVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005DB7 RID: 23991
	public abstract void ResetSleeperVolumes(long chunkKey);

	// Token: 0x06005DB8 RID: 23992
	public abstract SleeperVolume GetSleeperVolume(int index);

	// Token: 0x06005DB9 RID: 23993
	public abstract int AddTriggerVolume(TriggerVolume _volume);

	// Token: 0x06005DBA RID: 23994
	public abstract int FindTriggerVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005DBB RID: 23995
	public abstract void ResetTriggerVolumes(long chunkKey);

	// Token: 0x06005DBC RID: 23996
	public abstract TriggerVolume GetTriggerVolume(int index);

	// Token: 0x06005DBD RID: 23997
	public abstract int AddWallVolume(WallVolume _volume);

	// Token: 0x06005DBE RID: 23998
	public abstract int FindWallVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005DBF RID: 23999
	public abstract List<ValueTuple<int, WallVolume>> GetAllWallVolumes();

	// Token: 0x06005DC0 RID: 24000
	public abstract WallVolume GetWallVolume(int index);

	// Token: 0x06005DC1 RID: 24001
	public abstract bool HasWallVolumes(List<int> _wallVolumeIds);

	// Token: 0x06005DC2 RID: 24002
	public abstract GameRandom GetGameRandom();

	// Token: 0x06005DC3 RID: 24003
	public abstract void AddPendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005DC4 RID: 24004
	public abstract bool TryRetrieveAndRemovePendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005DC5 RID: 24005 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public WorldBase()
	{
	}

	// Token: 0x04004891 RID: 18577
	public ChunkCluster ChunkCache;
}
