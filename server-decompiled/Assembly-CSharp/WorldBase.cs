using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000BF6 RID: 3062
public abstract class WorldBase : IBlockAccess, IChunkAccess
{
	// Token: 0x06005D2C RID: 23852
	public abstract IChunk GetChunkSync(int chunkX, int chunkZ);

	// Token: 0x06005D2D RID: 23853
	public abstract IChunk GetChunkSync(long chunkKey);

	// Token: 0x06005D2E RID: 23854 RVA: 0x001B1240 File Offset: 0x001AF440
	public IChunk GetChunkSync(int chunkX, int chunkY, int chunkZ)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkX, chunkY, chunkZ);
	}

	// Token: 0x06005D2F RID: 23855 RVA: 0x001B124B File Offset: 0x001AF44B
	public IChunk GetChunkSync(Vector2i chunkPos)
	{
		return IChunkAccess.DefaultGetChunkSync(this, chunkPos);
	}

	// Token: 0x06005D30 RID: 23856 RVA: 0x001B1254 File Offset: 0x001AF454
	public IChunk GetChunkSync(PropRef propRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, propRef);
	}

	// Token: 0x06005D31 RID: 23857 RVA: 0x001B125D File Offset: 0x001AF45D
	public IChunk GetChunkSync(BlockValueRef bvRef)
	{
		return IChunkAccess.DefaultGetChunkSync(this, bvRef);
	}

	// Token: 0x06005D32 RID: 23858 RVA: 0x001B1266 File Offset: 0x001AF466
	public IChunk GetChunkFromWorldPos(int x, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z);
	}

	// Token: 0x06005D33 RID: 23859 RVA: 0x001B1270 File Offset: 0x001AF470
	public IChunk GetChunkFromWorldPos(int x, int y, int z)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, y, z);
	}

	// Token: 0x06005D34 RID: 23860 RVA: 0x001B127B File Offset: 0x001AF47B
	public IChunk GetChunkFromWorldPos(Vector3i blockPos)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos);
	}

	// Token: 0x06005D35 RID: 23861 RVA: 0x001B1284 File Offset: 0x001AF484
	public bool GetChunkFromWorldPos(int x, int z, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, x, z, ref chunk);
	}

	// Token: 0x06005D36 RID: 23862 RVA: 0x001B128F File Offset: 0x001AF48F
	public bool GetChunkFromWorldPos(Vector3i blockPos, ref IChunk chunk)
	{
		return IChunkAccess.DefaultGetChunkFromWorldPos(this, blockPos, ref chunk);
	}

	// Token: 0x06005D37 RID: 23863
	public abstract void AddFallingBlocks(IList<Vector3i> _list);

	// Token: 0x06005D38 RID: 23864
	public abstract byte GetStability(int worldX, int worldY, int worldZ);

	// Token: 0x06005D39 RID: 23865
	public abstract byte GetStability(Vector3i _pos);

	// Token: 0x06005D3A RID: 23866
	public abstract void SetStability(int worldX, int worldY, int worldZ, byte stab);

	// Token: 0x06005D3B RID: 23867
	public abstract void SetStability(Vector3i _pos, byte stab);

	// Token: 0x06005D3C RID: 23868
	public abstract byte GetHeight(int worldX, int worldZ);

	// Token: 0x06005D3D RID: 23869
	public abstract bool IsWater(int _x, int _y, int _z);

	// Token: 0x06005D3E RID: 23870
	public abstract bool IsWater(Vector3i _pos);

	// Token: 0x06005D3F RID: 23871
	public abstract bool IsWater(Vector3 _pos);

	// Token: 0x06005D40 RID: 23872
	public abstract bool IsAir(int _x, int _y, int _z);

	// Token: 0x06005D41 RID: 23873
	public abstract IGameManager GetGameManager();

	// Token: 0x06005D42 RID: 23874
	public abstract Manager GetAudioManager();

	// Token: 0x06005D43 RID: 23875
	public abstract AIDirector GetAIDirector();

	// Token: 0x06005D44 RID: 23876
	public abstract void SetBlocksRPC(List<BlockChangeInfo> _blockChangeInfo);

	// Token: 0x06005D45 RID: 23877 RVA: 0x0024505C File Offset: 0x0024325C
	public void SetBlockRPC(BlockChangeInfo _info)
	{
		this.SetBlocksRPC(new List<BlockChangeInfo> { _info });
	}

	// Token: 0x06005D46 RID: 23878 RVA: 0x00245070 File Offset: 0x00243270
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true));
	}

	// Token: 0x06005D47 RID: 23879 RVA: 0x00245080 File Offset: 0x00243280
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density));
	}

	// Token: 0x06005D48 RID: 23880 RVA: 0x00245090 File Offset: 0x00243290
	public void SetBlockRPC(BlockValueRef _bvRef, sbyte _density)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _density, false));
	}

	// Token: 0x06005D49 RID: 23881 RVA: 0x002450A0 File Offset: 0x002432A0
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, sbyte _density, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, _density, _changingEntityId));
	}

	// Token: 0x06005D4A RID: 23882 RVA: 0x002450B2 File Offset: 0x002432B2
	public void SetBlockRPC(BlockValueRef _bvRef, BlockValue _blockValue, int _changingEntityId)
	{
		this.SetBlockRPC(new BlockChangeInfo(_bvRef, _blockValue, true, _changingEntityId));
	}

	// Token: 0x06005D4B RID: 23883
	public abstract void SetPropsRPC(List<PropChangeInfo> _propChangeInfo);

	// Token: 0x06005D4C RID: 23884 RVA: 0x002450C3 File Offset: 0x002432C3
	public void SetPropRPC(PropChangeInfo _change)
	{
		this.SetPropsRPC(new List<PropChangeInfo> { _change });
	}

	// Token: 0x06005D4D RID: 23885
	public abstract BlockValue GetBlock(int _x, int _y, int _z);

	// Token: 0x06005D4E RID: 23886 RVA: 0x001B233C File Offset: 0x001B053C
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x001B2345 File Offset: 0x001B0545
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005D50 RID: 23888
	public abstract sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x06005D51 RID: 23889 RVA: 0x002450D7 File Offset: 0x002432D7
	public sbyte GetDensity(Vector3i _blockPos)
	{
		return this.GetDensity(_blockPos.x, _blockPos.y, _blockPos.z);
	}

	// Token: 0x06005D52 RID: 23890 RVA: 0x002450F4 File Offset: 0x002432F4
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

	// Token: 0x06005D53 RID: 23891
	public abstract PropValue GetProp(int chunkX, int chunkZ, int propId);

	// Token: 0x06005D54 RID: 23892
	public abstract PropValue GetProp(long chunkKey, int propId);

	// Token: 0x06005D55 RID: 23893 RVA: 0x001B24D1 File Offset: 0x001B06D1
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005D56 RID: 23894 RVA: 0x001B24DB File Offset: 0x001B06DB
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x06005D57 RID: 23895
	public abstract Entity GetEntity(int _entityId);

	// Token: 0x06005D58 RID: 23896
	public abstract void ChangeClientEntityIdToServer(int _clientEntityId, int _serverEntityId);

	// Token: 0x06005D59 RID: 23897
	public abstract bool IsRemote();

	// Token: 0x06005D5A RID: 23898
	public abstract WorldBlockTicker GetWBT();

	// Token: 0x06005D5B RID: 23899
	public abstract bool IsOpenSkyAbove(int _x, int _y, int _z);

	// Token: 0x06005D5C RID: 23900
	public abstract int GetBlockLightValue(Vector3i blockPos);

	// Token: 0x06005D5D RID: 23901
	public abstract float GetLightBrightness(Vector3i blockPos);

	// Token: 0x06005D5E RID: 23902
	public abstract bool IsEditor();

	// Token: 0x06005D5F RID: 23903
	public abstract TileEntity GetTileEntity(Vector3i _blockPos);

	// Token: 0x06005D60 RID: 23904 RVA: 0x00245134 File Offset: 0x00243334
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

	// Token: 0x06005D61 RID: 23905
	public abstract List<EntityPlayer> GetPlayers();

	// Token: 0x06005D62 RID: 23906
	public abstract EntityPlayerLocal GetPrimaryPlayer();

	// Token: 0x06005D63 RID: 23907
	public abstract void AddLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005D64 RID: 23908
	public abstract void RemoveLocalPlayer(EntityPlayerLocal _localPlayer);

	// Token: 0x06005D65 RID: 23909
	public abstract List<EntityPlayerLocal> GetLocalPlayers();

	// Token: 0x06005D66 RID: 23910
	public abstract bool IsLocalPlayer(int _playerId);

	// Token: 0x06005D67 RID: 23911
	public abstract EntityPlayerLocal GetLocalPlayerFromID(int _playerId);

	// Token: 0x06005D68 RID: 23912
	public abstract EntityPlayerLocal GetClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D69 RID: 23913
	public abstract Vector3 GetVectorToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D6A RID: 23914
	public abstract float GetSquaredDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D6B RID: 23915
	public abstract float GetDistanceToClosestLocalPlayer(Vector3 _position);

	// Token: 0x06005D6C RID: 23916
	public abstract bool CanPlaceLandProtectionBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D6D RID: 23917
	public abstract bool CanPlaceBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005D6E RID: 23918
	public abstract bool CanPickupBlockAt(Vector3i blockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D6F RID: 23919
	public abstract float GetLandProtectionHardnessModifier(Vector3i blockPos, EntityAlive lpRelative, PersistentPlayerData ppData);

	// Token: 0x06005D70 RID: 23920
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D71 RID: 23921
	public abstract bool IsMyLandProtectedBlock(Vector3i worldBlockPos, PersistentPlayerData lpRelative, bool traderAllowed = false);

	// Token: 0x06005D72 RID: 23922
	public abstract EnumLandClaimOwner GetLandClaimOwner(Vector3i worldBlockPos, PersistentPlayerData lpRelative);

	// Token: 0x06005D73 RID: 23923
	public abstract ulong GetWorldTime();

	// Token: 0x06005D74 RID: 23924
	public abstract WorldCreationData GetWorldCreationData();

	// Token: 0x06005D75 RID: 23925
	public abstract void UnloadEntities(List<Entity> _entityList, bool _forceUnload = false);

	// Token: 0x06005D76 RID: 23926
	public abstract Entity RemoveEntity(int _entityId, EnumRemoveEntityReason _reason);

	// Token: 0x06005D77 RID: 23927
	public abstract int AddSleeperVolume(SleeperVolume _volume);

	// Token: 0x06005D78 RID: 23928
	public abstract int FindSleeperVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005D79 RID: 23929
	public abstract void ResetSleeperVolumes(long chunkKey);

	// Token: 0x06005D7A RID: 23930
	public abstract SleeperVolume GetSleeperVolume(int index);

	// Token: 0x06005D7B RID: 23931
	public abstract int AddTriggerVolume(TriggerVolume _volume);

	// Token: 0x06005D7C RID: 23932
	public abstract int FindTriggerVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005D7D RID: 23933
	public abstract void ResetTriggerVolumes(long chunkKey);

	// Token: 0x06005D7E RID: 23934
	public abstract TriggerVolume GetTriggerVolume(int index);

	// Token: 0x06005D7F RID: 23935
	public abstract int AddWallVolume(WallVolume _volume);

	// Token: 0x06005D80 RID: 23936
	public abstract int FindWallVolume(Vector3i mins, Vector3i maxs);

	// Token: 0x06005D81 RID: 23937
	public abstract List<ValueTuple<int, WallVolume>> GetAllWallVolumes();

	// Token: 0x06005D82 RID: 23938
	public abstract WallVolume GetWallVolume(int index);

	// Token: 0x06005D83 RID: 23939
	public abstract bool HasWallVolumes(List<int> _wallVolumeIds);

	// Token: 0x06005D84 RID: 23940
	public abstract GameRandom GetGameRandom();

	// Token: 0x06005D85 RID: 23941
	public abstract void AddPendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005D86 RID: 23942
	public abstract bool TryRetrieveAndRemovePendingDowngradeBlock(BlockValueRef _bvRef);

	// Token: 0x06005D87 RID: 23943 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public WorldBase()
	{
	}

	// Token: 0x0400482D RID: 18477
	public ChunkCluster ChunkCache;
}
