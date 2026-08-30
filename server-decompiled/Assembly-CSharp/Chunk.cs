using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Platform;
using UnityEngine;

// Token: 0x02000B01 RID: 2817
public class Chunk : IChunk, IBlockAccess, IMemoryPoolableObject
{
	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x06005400 RID: 21504 RVA: 0x00201065 File Offset: 0x001FF265
	// (set) Token: 0x06005401 RID: 21505 RVA: 0x0020106D File Offset: 0x001FF26D
	public bool StopStabilityCalculation
	{
		get
		{
			return this.stopStabilityCalculation;
		}
		set
		{
			if (value)
			{
				World world = GameManager.Instance.World;
				if (world != null)
				{
					ChunkManager chunkManager = world.m_ChunkManager;
					if (chunkManager != null)
					{
						Action<Chunk> onChunkStabilityCalculationEnabled = chunkManager.OnChunkStabilityCalculationEnabled;
						if (onChunkStabilityCalculationEnabled != null)
						{
							onChunkStabilityCalculationEnabled(this);
						}
					}
				}
			}
			this.stopStabilityCalculation = value;
		}
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x002010A5 File Offset: 0x001FF2A5
	public void AssignWaterSimHandle(WaterSimulationNative.ChunkHandle handle)
	{
		this.waterSimHandle = handle;
	}

	// Token: 0x06005403 RID: 21507 RVA: 0x002010AE File Offset: 0x001FF2AE
	public void ResetWaterSimHandle()
	{
		this.waterSimHandle.Reset();
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x002010BB File Offset: 0x001FF2BB
	public void AssignWaterDebugRenderer(WaterDebugManager.RendererHandle handle)
	{
		this.waterDebugHandle = handle;
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x000027FC File Offset: 0x000009FC
	public void ResetWaterDebugHandle()
	{
	}

	// Token: 0x06005406 RID: 21510 RVA: 0x002010C4 File Offset: 0x001FF2C4
	public byte[] GetTopSoil()
	{
		return this.m_bTopSoilBroken;
	}

	// Token: 0x06005407 RID: 21511 RVA: 0x002010CC File Offset: 0x001FF2CC
	public void SetTopSoil(IList<byte> soil)
	{
		for (int i = 0; i < this.m_bTopSoilBroken.Length; i++)
		{
			this.m_bTopSoilBroken[i] = soil[i];
		}
	}

	// Token: 0x06005408 RID: 21512 RVA: 0x002010FC File Offset: 0x001FF2FC
	public Chunk()
	{
		this.m_X = 0;
		this.m_Y = 0;
		this.Z = 0;
		for (int i = 0; i < this.trisInMesh.GetLength(0); i++)
		{
			this.trisInMesh[i] = new int[MeshDescription.meshes.Length];
			this.sizeOfMesh[i] = new int[MeshDescription.meshes.Length];
		}
		for (int j = 0; j < 16; j++)
		{
			this.entityLists[j] = new List<Entity>();
		}
		this.NeedsLightCalculation = true;
		this.NeedsDecoration = true;
		this.hasEntities = false;
		this.isModified = false;
		this.m_BlockLayers = new ChunkBlockLayer[64];
		this.chnLight = new ChunkBlockChannel(0L, 1);
		this.chnDensity = new ChunkBlockChannel((long)((ulong)((byte)MarchingCubes.DensityAir)), 1);
		this.chnStability = new ChunkBlockChannel(0L, 1);
		this.chnDamage = new ChunkBlockChannel(0L, 2);
		this.chnTextures = new ChunkBlockChannel[1];
		for (int k = 0; k < 1; k++)
		{
			this.chnTextures[k] = new ChunkBlockChannel(0L, 6);
		}
		this.chnWater = new ChunkBlockChannel(0L, 2);
		this.m_HeightMap = new byte[256];
		this.m_TerrainHeight = new byte[256];
		this.m_bTopSoilBroken = new byte[32];
		this.m_Biomes = new byte[256];
		this.m_BiomeIntensities = new byte[1536];
		this.m_NormalX = new byte[256];
		this.m_NormalY = new byte[256];
		this.m_NormalZ = new byte[256];
		Chunk.InstanceCount++;
	}

	// Token: 0x06005409 RID: 21513 RVA: 0x002013BF File Offset: 0x001FF5BF
	public Chunk(int _x, int _z)
		: this()
	{
		this.m_X = _x;
		this.m_Y = 0;
		this.m_Z = _z;
		this.ResetStability();
		this.RefreshSunlight();
		this.NeedsLightCalculation = true;
		this.NeedsDecoration = false;
	}

	// Token: 0x0600540A RID: 21514 RVA: 0x002013FC File Offset: 0x001FF5FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~Chunk()
	{
		Chunk.InstanceCount--;
	}

	// Token: 0x0600540B RID: 21515 RVA: 0x00201430 File Offset: 0x001FF630
	public void ResetLights(byte _lightValue = 0)
	{
		this.chnLight.Clear((long)((ulong)_lightValue));
	}

	// Token: 0x0600540C RID: 21516 RVA: 0x00201440 File Offset: 0x001FF640
	public void Reset()
	{
		if (this.InProgressSaving)
		{
			Log.Warning("Unloading: chunk while saving " + ((this != null) ? this.ToString() : null));
		}
		this.cachedToString = null;
		this.m_X = 0;
		this.m_Y = 0;
		this.Z = 0;
		this.MeshLayerCount = 0;
		for (int i = 0; i < 16; i++)
		{
			this.entityLists[i].Clear();
		}
		this.entityStubs.Clear();
		this.pendingEntityCreateOps.Clear();
		this.blockEntityStubs.Clear();
		this.sleeperVolumes.Clear();
		this.triggerVolumes.Clear();
		this.tileEntities.Clear();
		this.IndexedBlocks.Clear();
		this.triggerData.Clear();
		this.insideDevices.Clear();
		this.insideDevicesHashSet.Clear();
		this.NeedsRegeneration = false;
		this.NeedsDecoration = true;
		this.NeedsLightDecoration = false;
		this.NeedsLightCalculation = true;
		this.hasEntities = false;
		this.isModified = false;
		this.InProgressRegeneration = false;
		this.InProgressSaving = false;
		this.InProgressCopying = false;
		this.InProgressDecorating = false;
		this.InProgressLighting = false;
		this.InProgressUnloading = false;
		this.NeedsOnlyCollisionMesh = false;
		this.IsCollisionMeshGenerated = false;
		this.SavedInWorldTicks = 0UL;
		MemoryPools.poolCBL.FreeSync(this.m_BlockLayers);
		this.chnDensity.FreeLayers();
		this.chnStability.FreeLayers();
		this.chnLight.FreeLayers();
		this.chnDamage.FreeLayers();
		for (int j = 0; j < 1; j++)
		{
			this.chnTextures[j].FreeLayers();
		}
		this.chnWater.FreeLayers();
		this.ResetLights(0);
		Array.Clear(this.m_HeightMap, 0, this.m_HeightMap.GetLength(0));
		Array.Clear(this.m_TerrainHeight, 0, this.m_TerrainHeight.GetLength(0));
		Array.Clear(this.m_bTopSoilBroken, 0, this.m_bTopSoilBroken.GetLength(0));
		Array.Clear(this.m_Biomes, 0, this.m_Biomes.GetLength(0));
		Array.Clear(this.m_NormalX, 0, this.m_NormalX.GetLength(0));
		Array.Clear(this.m_NormalY, 0, this.m_NormalY.GetLength(0));
		Array.Clear(this.m_NormalZ, 0, this.m_NormalZ.GetLength(0));
		this.ResetBiomeIntensity(BiomeIntensity.Default);
		this.DominantBiome = 0;
		this.AreaMasterDominantBiome = byte.MaxValue;
		this.biomeSpawnData = null;
		if (this.m_DecoBiomeArray != null)
		{
			Array.Clear(this.m_DecoBiomeArray, 0, this.m_DecoBiomeArray.GetLength(0));
		}
		this.ChunkCustomData.Clear();
		this.bMapDirty = true;
		DictionaryKeyList<Vector3i, int> dictionaryKeyList = this.tickedBlocks;
		lock (dictionaryKeyList)
		{
			this.tickedBlocks.Clear();
		}
		this.bEmptyDirty = true;
		this.StopStabilityCalculation = true;
		this.waterSimHandle.Reset();
	}

	// Token: 0x0600540D RID: 21517 RVA: 0x002010AE File Offset: 0x001FF2AE
	public void Cleanup()
	{
		this.waterSimHandle.Reset();
	}

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x0600540E RID: 21518 RVA: 0x00201758 File Offset: 0x001FF958
	// (set) Token: 0x0600540F RID: 21519 RVA: 0x00201760 File Offset: 0x001FF960
	public int X
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_X;
		}
		set
		{
			this.cachedToString = null;
			this.m_X = value;
			this.updateBounds();
		}
	}

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x06005410 RID: 21520 RVA: 0x00201776 File Offset: 0x001FF976
	public int Y
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_Y;
		}
	}

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x06005411 RID: 21521 RVA: 0x0020177E File Offset: 0x001FF97E
	// (set) Token: 0x06005412 RID: 21522 RVA: 0x00201786 File Offset: 0x001FF986
	public int Z
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_Z;
		}
		set
		{
			this.cachedToString = null;
			this.m_Z = value;
			this.updateBounds();
		}
	}

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x06005413 RID: 21523 RVA: 0x0020179C File Offset: 0x001FF99C
	// (set) Token: 0x06005414 RID: 21524 RVA: 0x002017B5 File Offset: 0x001FF9B5
	public Vector3i ChunkPos
	{
		get
		{
			return new Vector3i(this.m_X, this.m_Y, this.m_Z);
		}
		set
		{
			this.cachedToString = null;
			this.m_X = value.x;
			this.m_Z = value.z;
			this.updateBounds();
		}
	}

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x06005415 RID: 21525 RVA: 0x002017DC File Offset: 0x001FF9DC
	public long Key
	{
		get
		{
			return WorldChunkCache.MakeChunkKey(this.m_X, this.m_Z);
		}
	}

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x06005416 RID: 21526 RVA: 0x002017F0 File Offset: 0x001FF9F0
	public bool IsLocked
	{
		get
		{
			return this.InProgressCopying || this.InProgressDecorating || this.InProgressLighting || this.InProgressRegeneration || this.InProgressUnloading || this.InProgressSaving || this.InProgressNetworking || this.InProgressWaterSim;
		}
	}

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06005417 RID: 21527 RVA: 0x00201850 File Offset: 0x001FFA50
	public bool IsLockedExceptUnloading
	{
		get
		{
			return this.InProgressCopying || this.InProgressDecorating || this.InProgressLighting || this.InProgressRegeneration || this.InProgressSaving || this.InProgressNetworking || this.InProgressWaterSim;
		}
	}

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x06005418 RID: 21528 RVA: 0x002018A3 File Offset: 0x001FFAA3
	public bool IsInitialized
	{
		get
		{
			return !this.NeedsLightCalculation && !this.InProgressDecorating && !this.InProgressUnloading;
		}
	}

	// Token: 0x06005419 RID: 21529 RVA: 0x002018C6 File Offset: 0x001FFAC6
	public bool GetAvailable()
	{
		return this.IsCollisionMeshGenerated;
	}

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x0600541A RID: 21530 RVA: 0x002018D0 File Offset: 0x001FFAD0
	// (set) Token: 0x0600541B RID: 21531 RVA: 0x00201914 File Offset: 0x001FFB14
	public bool NeedsRegeneration
	{
		get
		{
			bool flag2;
			lock (this)
			{
				flag2 = this.m_NeedsRegenerationAtY != 0;
			}
			return flag2;
		}
		set
		{
			Queue<int> layerIndexQueue = this.m_layerIndexQueue;
			lock (layerIndexQueue)
			{
				this.MeshLayerCount = 0;
				this.m_layerIndexQueue.Clear();
				MemoryPools.poolVML.FreeSync(this.m_meshLayers);
			}
			lock (this)
			{
				if (value)
				{
					this.m_NeedsRegenerationAtY = 65535;
				}
				else
				{
					this.m_NeedsRegenerationAtY = 0;
				}
			}
			this.NeedsRegenerationDebug = this.m_NeedsRegenerationAtY;
		}
	}

	// Token: 0x0600541C RID: 21532 RVA: 0x002019C0 File Offset: 0x001FFBC0
	public void ClearNeedsRegenerationAt(int _idx)
	{
		lock (this)
		{
			this.m_NeedsRegenerationAtY &= ~(1 << _idx);
			this.NeedsRegenerationDebug = this.m_NeedsRegenerationAtY;
		}
	}

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x0600541D RID: 21533 RVA: 0x00201A1C File Offset: 0x001FFC1C
	public bool NeedsCopying
	{
		get
		{
			return this.HasMeshLayer();
		}
	}

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x0600541E RID: 21534 RVA: 0x00201A24 File Offset: 0x001FFC24
	// (set) Token: 0x0600541F RID: 21535 RVA: 0x00201A64 File Offset: 0x001FFC64
	public int NeedsRegenerationAt
	{
		get
		{
			int needsRegenerationAtY;
			lock (this)
			{
				needsRegenerationAtY = this.m_NeedsRegenerationAtY;
			}
			return needsRegenerationAtY;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			lock (this)
			{
				this.m_NeedsRegenerationAtY |= 1 << (value >> 4);
			}
		}
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x00201AB4 File Offset: 0x001FFCB4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void NeedsRegenerationOrBits(int _v)
	{
		lock (this)
		{
			this.m_NeedsRegenerationAtY |= _v;
		}
	}

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x06005421 RID: 21537 RVA: 0x00201AFC File Offset: 0x001FFCFC
	public bool NeedsSaving
	{
		get
		{
			return this.isModified || this.hasEntities || this.tileEntities.Count > 0 || this.triggerData.Count > 0;
		}
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x00201B2E File Offset: 0x001FFD2E
	public void load(PooledBinaryReader stream, uint _version)
	{
		this.read(stream, _version, false);
		this.isModified = false;
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x00201B40 File Offset: 0x001FFD40
	public void read(PooledBinaryReader stream, uint _version)
	{
		this.read(stream, _version, true);
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x00201B4C File Offset: 0x001FFD4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void read(PooledBinaryReader _br, uint _version, bool _bNetworkRead)
	{
		this.cachedToString = null;
		this.m_X = _br.ReadInt32();
		this.m_Y = _br.ReadInt32();
		this.Z = _br.ReadInt32();
		this.SavedInWorldTicks = _br.ReadUInt64();
		this.LastTimeRandomTicked = this.SavedInWorldTicks;
		MemoryPools.poolCBL.FreeSync(this.m_BlockLayers);
		Array.Clear(this.m_HeightMap, 0, 256);
		if (_version < 32U)
		{
			throw new Exception("Chunk version " + _version.ToString() + " not supported!");
		}
		for (int i = 0; i < 64; i++)
		{
			if (_br.ReadBoolean())
			{
				ChunkBlockLayer chunkBlockLayer = MemoryPools.poolCBL.AllocSync(false);
				chunkBlockLayer.Read(_br, _version, _bNetworkRead);
				this.m_BlockLayers[i] = chunkBlockLayer;
				this.bEmptyDirty = true;
			}
		}
		if (!_bNetworkRead)
		{
			this.chnStability.Read(_br, _version, _bNetworkRead);
		}
		_br.Flush();
		this.recalcIndexedBlocks();
		_br.Read(this.m_HeightMap, 0, 256);
		_br.Read(this.m_TerrainHeight, 0, this.m_TerrainHeight.Length);
		if (_version > 41U)
		{
			_br.Read(this.m_bTopSoilBroken, 0, 32);
		}
		_br.Read(this.m_Biomes, 0, 256);
		_br.Read(this.m_BiomeIntensities, 0, 1536);
		this.DominantBiome = _br.ReadByte();
		this.AreaMasterDominantBiome = _br.ReadByte();
		int num = (int)_br.ReadUInt16();
		this.ChunkCustomData.Clear();
		for (int j = 0; j < num; j++)
		{
			ChunkCustomData chunkCustomData = new ChunkCustomData();
			chunkCustomData.Read(_br);
			this.ChunkCustomData.Set(chunkCustomData.key, chunkCustomData);
		}
		_br.Read(this.m_NormalX, 0, 256);
		_br.Read(this.m_NormalY, 0, 256);
		_br.Read(this.m_NormalZ, 0, 256);
		this.chnDensity.Read(_br, _version, _bNetworkRead);
		this.chnLight.Read(_br, _version, _bNetworkRead);
		if (_version >= 33U && _version < 36U)
		{
			ChunkBlockChannel chunkBlockChannel = new ChunkBlockChannel(0L, 1);
			chunkBlockChannel.Read(_br, _version, _bNetworkRead);
			chunkBlockChannel.Read(_br, _version, _bNetworkRead);
		}
		if (_version >= 36U)
		{
			this.chnDamage.Read(_br, _version, _bNetworkRead);
		}
		if (_version >= 47U)
		{
			for (int k = 0; k < 1; k++)
			{
				this.chnTextures[k].Read(_br, _version, _bNetworkRead);
			}
		}
		else if (_version >= 35U)
		{
			this.chnTextures[0].Read(_br, _version, _bNetworkRead);
		}
		if (_version >= 46U)
		{
			this.chnWater.Read(_br, _version, _bNetworkRead);
		}
		else if (WaterSimulationNative.Instance.IsInitialized)
		{
			throw new Exception("Serialized data incompatible with new water simulation");
		}
		this.NeedsDecoration = false;
		this.NeedsLightCalculation = _br.ReadBoolean();
		int num2 = _br.ReadInt32();
		for (int l = 0; l < 16; l++)
		{
			this.entityLists[l].Clear();
		}
		this.entityStubs.Clear();
		for (int m = 0; m < num2; m++)
		{
			EntityCreationData entityCreationData = new EntityCreationData();
			entityCreationData.read(_br, _bNetworkRead);
			this.entityStubs.Add(entityCreationData);
		}
		this.hasEntities = this.entityStubs.Count > 0;
		num2 = _br.ReadInt32();
		this.tileEntities.Clear();
		for (int n = 0; n < num2; n++)
		{
			TileEntityType tileEntityType = (TileEntityType)_br.ReadInt32();
			TileEntity.StreamModeRead streamModeRead = (_bNetworkRead ? TileEntity.StreamModeRead.FromServer : TileEntity.StreamModeRead.Persistency);
			TileEntity tileEntity = TileEntity.InstantiateFromRead(_br, streamModeRead, tileEntityType, this, null, new Func<int, int, int, BlockValue>(this.GetBlock));
			if (tileEntity != null)
			{
				tileEntity.OnReadComplete();
				this.tileEntities.Set(tileEntity.localChunkPos, tileEntity);
			}
		}
		if (_version > 10U && _version < 43U && !_bNetworkRead)
		{
			_br.ReadUInt16();
			_br.ReadByte();
		}
		if (_version > 33U && _br.ReadBoolean())
		{
			for (int num3 = 0; num3 < 16; num3++)
			{
				_br.ReadUInt16();
			}
		}
		if (!_bNetworkRead && _version == 37U)
		{
			byte b = _br.ReadByte();
			for (int num4 = 0; num4 < (int)b; num4++)
			{
				SleeperVolume.Read(_br);
			}
		}
		if (!_bNetworkRead && _version > 37U)
		{
			this.sleeperVolumes.Clear();
			int num5 = (int)_br.ReadByte();
			for (int num6 = 0; num6 < num5; num6++)
			{
				int num7 = _br.ReadInt32();
				if (num7 < 0)
				{
					Log.Error("chunk sleeper volumeId invalid {0}", new object[] { num7 });
				}
				else
				{
					this.AddSleeperVolumeId(num7);
				}
			}
		}
		if (!_bNetworkRead && _version >= 44U)
		{
			this.triggerVolumes.Clear();
			int num8 = (int)_br.ReadByte();
			for (int num9 = 0; num9 < num8; num9++)
			{
				int num10 = _br.ReadInt32();
				if (num10 < 0)
				{
					Log.Error("chunk trigger volumeId invalid {0}", new object[] { num10 });
				}
				else
				{
					this.AddTriggerVolumeId(num10);
				}
			}
		}
		if (_version >= 45U)
		{
			this.wallVolumes.Clear();
			int num11 = (int)_br.ReadByte();
			for (int num12 = 0; num12 < num11; num12++)
			{
				int num13 = _br.ReadInt32();
				if (num13 < 0)
				{
					Log.Error("chunk wall volumeId invalid {0}", new object[] { num13 });
				}
				else
				{
					this.AddWallVolumeId(num13);
				}
			}
		}
		if (_bNetworkRead)
		{
			_br.ReadBoolean();
		}
		DictionaryKeyList<Vector3i, int> dictionaryKeyList = this.tickedBlocks;
		lock (dictionaryKeyList)
		{
			this.tickedBlocks.Clear();
			for (int num14 = 0; num14 < 64; num14++)
			{
				ChunkBlockLayer chunkBlockLayer2 = this.m_BlockLayers[num14];
				if (chunkBlockLayer2 != null)
				{
					for (int num15 = 0; num15 < 1024; num15++)
					{
						int idAt = chunkBlockLayer2.GetIdAt(num15);
						if (idAt != 0 && Block.BlocksLoaded && idAt < Block.list.Length && Block.list[idAt] != null && Block.list[idAt].IsRandomlyTick && !chunkBlockLayer2.GetAt(num15).ischild)
						{
							int num16 = num15 % 256 % 16;
							int num17 = num14 * 4 + num15 / 256;
							int num18 = num15 % 256 / 16;
							this.tickedBlocks.Add(this.ToWorldPos(num16, num17, num18), 0);
						}
					}
				}
			}
		}
		this.insideDevices.Clear();
		if (_version > 39U)
		{
			int num19 = (int)_br.ReadInt16();
			this.insideDevices.Capacity = num19;
			byte b2 = 0;
			byte b3 = 0;
			int num20 = 0;
			for (int num21 = 0; num21 < num19; num21++)
			{
				if (num20 == 0)
				{
					b2 = _br.ReadByte();
					b3 = _br.ReadByte();
					num20 = (int)_br.ReadByte();
				}
				Vector3b vector3b = new Vector3b(b2, _br.ReadByte(), b3);
				this.insideDevices.Add(vector3b);
				this.insideDevicesHashSet.Add(vector3b.GetHashCode());
				num20--;
			}
		}
		if (_version > 40U)
		{
			this.IsInternalBlocksCulled = _br.ReadBoolean();
		}
		if (_version > 42U && !_bNetworkRead)
		{
			this.triggerData.Clear();
			int num22 = (int)_br.ReadInt16();
			for (int num23 = 0; num23 < num22; num23++)
			{
				Vector3i vector3i = StreamUtils.ReadVector3i(_br);
				BlockTrigger blockTrigger = new BlockTrigger(this);
				blockTrigger.LocalChunkPos = vector3i;
				blockTrigger.Read(_br);
				this.triggerData.Add(vector3i, blockTrigger);
			}
		}
		if (_bNetworkRead)
		{
			this.ResetStabilityToBottomMost();
			this.NeedsLightCalculation = true;
		}
		this.bMapDirty = true;
		this.StopStabilityCalculation = false;
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x002022A4 File Offset: 0x002004A4
	public void save(PooledBinaryWriter stream)
	{
		this.saveBlockIds();
		this.write(stream, false);
		this.isModified = false;
		this.SavedInWorldTicks = GameTimer.Instance.ticks;
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x002022CC File Offset: 0x002004CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void saveBlockIds()
	{
		if (Block.nameIdMapping != null)
		{
			NameIdMapping nameIdMapping = Block.nameIdMapping;
			NameIdMapping nameIdMapping2 = nameIdMapping;
			lock (nameIdMapping2)
			{
				for (int i = 0; i < 256; i += 4)
				{
					int num = i >> 2;
					ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[num];
					if (chunkBlockLayer == null)
					{
						Block block = BlockValue.Air.Block;
						nameIdMapping.AddMapping(block.blockID, block.GetBlockName(), false);
					}
					else
					{
						chunkBlockLayer.SaveBlockMappings(nameIdMapping);
					}
				}
			}
		}
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x00202360 File Offset: 0x00200560
	public void write(PooledBinaryWriter stream)
	{
		this.write(stream, true);
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x0020236C File Offset: 0x0020056C
	[PublicizedFrom(EAccessModifier.Private)]
	public void write(PooledBinaryWriter _bw, bool _bNetworkWrite)
	{
		byte[] array = MemoryPools.poolByte.Alloc(256);
		_bw.Write(this.m_X);
		_bw.Write(this.m_Y);
		_bw.Write(this.m_Z);
		_bw.Write(this.SavedInWorldTicks);
		for (int i = 0; i < 64; i++)
		{
			bool flag = this.m_BlockLayers[i] != null;
			_bw.Write(flag);
			if (flag)
			{
				this.m_BlockLayers[i].Write(_bw, _bNetworkWrite);
			}
		}
		if (!_bNetworkWrite)
		{
			this.chnStability.Write(_bw, _bNetworkWrite, array);
		}
		_bw.Write(this.m_HeightMap);
		_bw.Write(this.m_TerrainHeight);
		_bw.Write(this.m_bTopSoilBroken);
		_bw.Write(this.m_Biomes);
		_bw.Write(this.m_BiomeIntensities);
		_bw.Write(this.DominantBiome);
		_bw.Write(this.AreaMasterDominantBiome);
		int num = 0;
		if (_bNetworkWrite)
		{
			for (int j = 0; j < this.ChunkCustomData.valueList.Count; j++)
			{
				if (this.ChunkCustomData.valueList[j].isSavedToNetwork)
				{
					num++;
				}
			}
		}
		else
		{
			num = this.ChunkCustomData.valueList.Count;
		}
		_bw.Write((ushort)num);
		for (int k = 0; k < this.ChunkCustomData.valueList.Count; k++)
		{
			if (!_bNetworkWrite || this.ChunkCustomData.valueList[k].isSavedToNetwork)
			{
				this.ChunkCustomData.valueList[k].Write(_bw);
			}
		}
		_bw.Write(this.m_NormalX);
		_bw.Write(this.m_NormalY);
		_bw.Write(this.m_NormalZ);
		this.chnDensity.Write(_bw, _bNetworkWrite, array);
		this.chnLight.Write(_bw, _bNetworkWrite, array);
		this.chnDamage.Write(_bw, _bNetworkWrite, array);
		for (int l = 0; l < 1; l++)
		{
			this.chnTextures[l].Write(_bw, _bNetworkWrite, array);
		}
		this.chnWater.Write(_bw, _bNetworkWrite, array);
		_bw.Write(this.NeedsLightCalculation);
		int num2 = 0;
		for (int m = 0; m < 16; m++)
		{
			List<Entity> list = this.entityLists[m];
			for (int n = 0; n < list.Count; n++)
			{
				Entity entity = list[n];
				if (!(entity is EntityVehicle) && !(entity is EntityDrone) && ((!_bNetworkWrite && entity.IsSavedToFile()) || (_bNetworkWrite && entity.IsSavedToNetwork())))
				{
					num2++;
				}
			}
		}
		_bw.Write(num2);
		for (int num3 = 0; num3 < 16; num3++)
		{
			List<Entity> list2 = this.entityLists[num3];
			for (int num4 = 0; num4 < list2.Count; num4++)
			{
				Entity entity2 = list2[num4];
				if (!(entity2 is EntityVehicle) && !(entity2 is EntityDrone) && ((!_bNetworkWrite && entity2.IsSavedToFile()) || (_bNetworkWrite && entity2.IsSavedToNetwork())))
				{
					new EntityCreationData(entity2, true).write(_bw, _bNetworkWrite);
				}
			}
		}
		_bw.Write(this.tileEntities.Count);
		for (int num5 = 0; num5 < this.tileEntities.list.Count; num5++)
		{
			_bw.Write((int)this.tileEntities.list[num5].GetTileEntityType());
			this.tileEntities.list[num5].write(_bw, _bNetworkWrite ? TileEntity.StreamModeWrite.ToClient : TileEntity.StreamModeWrite.Persistency);
		}
		_bw.Write(false);
		if (!_bNetworkWrite)
		{
			int count = this.sleeperVolumes.Count;
			_bw.Write((byte)count);
			for (int num6 = 0; num6 < count; num6++)
			{
				_bw.Write(this.sleeperVolumes[num6]);
			}
		}
		if (!_bNetworkWrite)
		{
			int count2 = this.triggerVolumes.Count;
			_bw.Write((byte)count2);
			for (int num7 = 0; num7 < count2; num7++)
			{
				_bw.Write(this.triggerVolumes[num7]);
			}
		}
		int count3 = this.wallVolumes.Count;
		_bw.Write((byte)count3);
		for (int num8 = 0; num8 < count3; num8++)
		{
			_bw.Write(this.wallVolumes[num8]);
		}
		if (_bNetworkWrite)
		{
			_bw.Write(false);
		}
		List<byte> list3 = new List<byte>();
		int num9 = int.MaxValue;
		int num10 = int.MaxValue;
		_bw.Write((short)this.insideDevices.Count);
		foreach (Vector3b vector3b in this.insideDevices)
		{
			if (list3.Count > 254 || num9 != (int)vector3b.x || num10 != (int)vector3b.z)
			{
				if (list3.Count > 0)
				{
					_bw.Write((byte)num9);
					_bw.Write((byte)num10);
					_bw.Write((byte)list3.Count);
					for (int num11 = 0; num11 < list3.Count; num11++)
					{
						_bw.Write(list3[num11]);
					}
					list3.Clear();
				}
				num9 = (int)vector3b.x;
				num10 = (int)vector3b.z;
			}
			list3.Add(vector3b.y);
		}
		if (list3.Count > 0)
		{
			_bw.Write((byte)num9);
			_bw.Write((byte)num10);
			_bw.Write((byte)list3.Count);
			for (int num12 = 0; num12 < list3.Count; num12++)
			{
				_bw.Write(list3[num12]);
			}
		}
		_bw.Write(this.IsInternalBlocksCulled);
		if (!_bNetworkWrite)
		{
			int count4 = this.triggerData.Count;
			_bw.Write((short)count4);
			for (int num13 = 0; num13 < count4; num13++)
			{
				StreamUtils.Write(_bw, this.triggerData.list[num13].LocalChunkPos);
				this.triggerData.list[num13].Write(_bw);
			}
		}
		MemoryPools.poolByte.Free(array);
	}

	// Token: 0x06005429 RID: 21545 RVA: 0x00202970 File Offset: 0x00200B70
	[PublicizedFrom(EAccessModifier.Private)]
	public void recalcIndexedBlocks()
	{
		this.IndexedBlocks.Clear();
		for (int i = 0; i < 64; i++)
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[i];
			if (chunkBlockLayer != null)
			{
				chunkBlockLayer.AddIndexedBlocks(i, this.IndexedBlocks);
			}
		}
	}

	// Token: 0x0600542A RID: 21546 RVA: 0x002029AE File Offset: 0x00200BAE
	public void AddEntityStub(EntityCreationData _ecd)
	{
		this.entityStubs.Add(_ecd);
	}

	// Token: 0x0600542B RID: 21547 RVA: 0x002029BC File Offset: 0x00200BBC
	public BlockEntityData GetBlockEntity(Vector3i _worldPos)
	{
		BlockEntityData blockEntityData;
		this.blockEntityStubs.dict.TryGetValue(GameUtils.Vector3iToUInt64(_worldPos), out blockEntityData);
		return blockEntityData;
	}

	// Token: 0x0600542C RID: 21548 RVA: 0x002029E4 File Offset: 0x00200BE4
	public BlockEntityData GetBlockEntity(Transform _transform)
	{
		for (int i = 0; i < this.blockEntityStubs.list.Count; i++)
		{
			if (this.blockEntityStubs.list[i].transform == _transform)
			{
				return this.blockEntityStubs.list[i];
			}
		}
		return null;
	}

	// Token: 0x0600542D RID: 21549 RVA: 0x00202A40 File Offset: 0x00200C40
	public void AddEntityBlockStub(BlockEntityData _ecd)
	{
		ulong num = GameUtils.Vector3iToUInt64(_ecd.pos);
		BlockEntityData blockEntityData;
		if (this.blockEntityStubs.dict.TryGetValue(num, out blockEntityData))
		{
			this.blockEntityStubsToRemove.Add(blockEntityData);
		}
		this.blockEntityStubs.Set(num, _ecd);
	}

	// Token: 0x0600542E RID: 21550 RVA: 0x00202A88 File Offset: 0x00200C88
	public void RemoveEntityBlockStub(Vector3i _pos)
	{
		ulong num = GameUtils.Vector3iToUInt64(_pos);
		BlockEntityData blockEntityData;
		if (this.blockEntityStubs.dict.TryGetValue(num, out blockEntityData))
		{
			this.blockEntityStubsToRemove.Add(blockEntityData);
			this.blockEntityStubs.Remove(num);
			return;
		}
		string text = "Entity block on pos ";
		Vector3i vector3i = _pos;
		Log.Warning(text + vector3i.ToString() + " not found!");
	}

	// Token: 0x0600542F RID: 21551 RVA: 0x00202AF0 File Offset: 0x00200CF0
	public int EnableEntityBlocks(bool _on, string _name)
	{
		_name = _name.ToLower();
		int num = 0;
		for (int i = 0; i < this.blockEntityStubs.list.Count; i++)
		{
			BlockEntityData blockEntityData = this.blockEntityStubs.list[i];
			if (blockEntityData.transform)
			{
				string text = blockEntityData.transform.name.ToLower();
				if (_name.Length == 0 || text.Contains(_name))
				{
					blockEntityData.transform.gameObject.SetActive(_on);
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06005430 RID: 21552 RVA: 0x00202B7C File Offset: 0x00200D7C
	public void AddInsideDevicePosition(int _blockX, int _blockY, int _blockZ, BlockValue _bv)
	{
		Vector3b vector3b = new Vector3b(_blockX, _blockY, _blockZ);
		this.insideDevices.Add(vector3b);
		this.insideDevicesHashSet.Add(vector3b.GetHashCode());
		this.IsInternalBlocksCulled = true;
	}

	// Token: 0x06005431 RID: 21553 RVA: 0x00202BC0 File Offset: 0x00200DC0
	public int EnableInsideBlockEntities(bool _bOn)
	{
		int num = 0;
		foreach (Vector3b vector3b in this.insideDevices)
		{
			ulong num2 = GameUtils.Vector3iToUInt64(this.ToWorldPos(vector3b.ToVector3i()));
			BlockEntityData blockEntityData;
			if (this.blockEntityStubs.dict.TryGetValue(num2, out blockEntityData) && blockEntityData.bHasTransform)
			{
				blockEntityData.transform.gameObject.SetActive(_bOn);
				num++;
			}
		}
		return num;
	}

	// Token: 0x06005432 RID: 21554 RVA: 0x00202C58 File Offset: 0x00200E58
	public void ResetStability()
	{
		this.chnStability.Clear(-1L);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 256; k++)
				{
					int blockId = this.GetBlockId(i, k, j);
					if (blockId == 0)
					{
						break;
					}
					if (!Block.list[blockId].StabilitySupport)
					{
						this.chnStability.Set(i, k, j, 1L);
						break;
					}
					this.chnStability.Set(i, k, j, 15L);
				}
			}
		}
	}

	// Token: 0x06005433 RID: 21555 RVA: 0x00202CDC File Offset: 0x00200EDC
	public void ResetStabilityToBottomMost()
	{
		this.chnStability.Clear(-1L);
		for (int i = 0; i < 16; i++)
		{
			int j = 0;
			IL_0096:
			while (j < 16)
			{
				for (int k = 0; k < 256; k++)
				{
					int blockId = this.GetBlockId(j, k, i);
					if (blockId != 0 && Block.list[blockId].StabilitySupport)
					{
						IL_008A:
						while (k < 256)
						{
							int blockId2 = this.GetBlockId(j, k, i);
							if (blockId2 == 0)
							{
								break;
							}
							if (!Block.list[blockId2].StabilitySupport)
							{
								this.chnStability.Set(j, k, i, 1L);
								break;
							}
							this.chnStability.Set(j, k, i, 15L);
							k++;
						}
						j++;
						goto IL_0096;
					}
				}
				goto IL_008A;
			}
		}
	}

	// Token: 0x06005434 RID: 21556 RVA: 0x00202D94 File Offset: 0x00200F94
	public void RefreshSunlight()
	{
		this.chnLight.SetHalf(false, 15);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				int num = 15;
				bool flag = true;
				int k = 255;
				while (k >= 0)
				{
					int blockId = this.GetBlockId(i, k, j);
					if (!flag)
					{
						goto IL_003F;
					}
					if (blockId != 0)
					{
						flag = false;
						goto IL_003F;
					}
					IL_008D:
					k--;
					continue;
					IL_003F:
					Block block = Block.list[blockId];
					bool flag2 = block.shape.IsTerrain();
					if (!flag2)
					{
						num -= block.lightOpacity;
						if (num <= 0)
						{
							break;
						}
					}
					this.chnLight.Set(i, k, j, (long)((ulong)((byte)num)));
					if (!flag2)
					{
						goto IL_008D;
					}
					num -= block.lightOpacity;
					if (num > 0)
					{
						goto IL_008D;
					}
					break;
				}
				for (k--; k >= 0; k--)
				{
					this.chnLight.Set(i, k, j, 0L);
				}
			}
		}
		this.isModified = true;
	}

	// Token: 0x06005435 RID: 21557 RVA: 0x00202E7C File Offset: 0x0020107C
	public void SetFullSunlight()
	{
		this.chnLight.SetHalf(false, 15);
	}

	// Token: 0x06005436 RID: 21558 RVA: 0x00202E8C File Offset: 0x0020108C
	public void CopyLightsFrom(Chunk _other)
	{
		this.chnLight.CopyFrom(_other.chnLight);
		this.isModified = true;
	}

	// Token: 0x06005437 RID: 21559 RVA: 0x00202EA8 File Offset: 0x002010A8
	public bool CanMobsSpawnAtPos(int _x, int _y, int _z, bool _ignoreCanMobsSpawnOn = false, bool _checkWater = true)
	{
		if (_y < 2 || _y > 251)
		{
			return false;
		}
		if (this.IsTraderArea(_x, _z))
		{
			return false;
		}
		if (_checkWater || !this.IsWater(_x, _y - 1, _z))
		{
			Block block = this.GetBlockNoDamage(_x, _y - 1, _z).Block;
			if (!_ignoreCanMobsSpawnOn && !block.CanMobsSpawnOn)
			{
				return false;
			}
			if (!block.IsCollideMovement)
			{
				return false;
			}
		}
		Block block2 = this.GetBlockNoDamage(_x, _y, _z).Block;
		if (!block2.IsCollideMovement || !block2.shape.IsSolidSpace)
		{
			Block block3 = this.GetBlockNoDamage(_x, _y + 1, _z).Block;
			if ((!block3.IsCollideMovement || !block3.shape.IsSolidSpace) && (!_checkWater || !this.IsWater(_x, _y, _z)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005438 RID: 21560 RVA: 0x00202F70 File Offset: 0x00201170
	public bool CanSleeperSpawnAtPos(int _x, int _y, int _z, bool _checkBelow)
	{
		if (_checkBelow && !this.GetBlockNoDamage(_x, _y - 1, _z).Block.IsCollideMovement)
		{
			return false;
		}
		Block block = this.GetBlockNoDamage(_x, _y, _z).Block;
		return !block.IsCollideMovement && !block.shape.IsSolidSpace;
	}

	// Token: 0x06005439 RID: 21561 RVA: 0x00202FC8 File Offset: 0x002011C8
	public bool CanPlayersSpawnAtPos(int _x, int _y, int _z, bool _allowOnAirPos = false)
	{
		if (_y < 2 || _y > 251)
		{
			return false;
		}
		Block block = this.GetBlockNoDamage(_x, _y - 1, _z).Block;
		if (!block.CanPlayersSpawnOn)
		{
			return false;
		}
		Block block2 = this.GetBlockNoDamage(_x, _y, _z).Block;
		Block block3 = this.GetBlockNoDamage(_x, _y + 1, _z).Block;
		return ((_allowOnAirPos && block.blockID == 0) || block.IsCollideMovement) && (!block2.IsCollideMovement || !block2.shape.IsSolidSpace) && !this.IsWater(_x, _y, _z) && (!block3.IsCollideMovement || !block3.shape.IsSolidSpace);
	}

	// Token: 0x0600543A RID: 21562 RVA: 0x00203074 File Offset: 0x00201274
	public bool IsPositionOnTerrain(int _x, int _y, int _z)
	{
		return _y >= 1 && this.GetBlockNoDamage(_x, _y - 1, _z).Block.shape.IsTerrain();
	}

	// Token: 0x0600543B RID: 21563 RVA: 0x002030A4 File Offset: 0x002012A4
	public bool FindRandomTopSoilPoint(World _world, out int x, out int y, out int z, int numTrys)
	{
		x = 0;
		y = 0;
		z = 0;
		while (numTrys-- > 0)
		{
			x = _world.GetGameRandom().RandomRange(15);
			z = _world.GetGameRandom().RandomRange(15);
			y = (int)this.GetHeight(x, z);
			if (y >= 2 && this.CanMobsSpawnAtPos(x, y, z, false, true))
			{
				x += this.m_X * 16;
				y++;
				z += this.m_Z * 16;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x00203130 File Offset: 0x00201330
	public bool FindRandomCavePoint(World _world, out int x, out int y, out int z, int numTrys, int relMinY)
	{
		x = 0;
		y = 0;
		z = 0;
		while (numTrys-- > 0)
		{
			x = _world.GetGameRandom().RandomRange(15);
			z = _world.GetGameRandom().RandomRange(15);
			int height = (int)this.GetHeight(x, z);
			y = height;
			while (y > height - relMinY && y > 2)
			{
				if (this.CanMobsSpawnAtPos(x, y, z, false, true))
				{
					x += this.m_X * 16;
					y++;
					z += this.m_Z * 16;
					return true;
				}
				y--;
			}
		}
		return false;
	}

	// Token: 0x0600543D RID: 21565 RVA: 0x002031D4 File Offset: 0x002013D4
	public bool FindSpawnPointAtXZ(int x, int z, out int y, int _maxLightV, int _darknessV, int startY, int endY, bool _bIgnoreCanMobsSpawnOn = false)
	{
		endY = Utils.FastClamp(endY, 1, 255);
		startY = Utils.FastClamp(startY - 1, 1, 255);
		y = endY;
		while (y > startY)
		{
			if (this.GetLightValue(x, y, z, _darknessV) <= _maxLightV)
			{
				if (this.CanMobsSpawnAtPos(x, y, z, _bIgnoreCanMobsSpawnOn, true))
				{
					y++;
					return true;
				}
				y--;
			}
		}
		return false;
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x0020323B File Offset: 0x0020143B
	public float GetLightBrightness(int x, int y, int z, int _ss)
	{
		return (float)this.GetLightValue(x, y, z, _ss) / 15f;
	}

	// Token: 0x0600543F RID: 21567 RVA: 0x00203250 File Offset: 0x00201450
	public int GetLightValue(int x, int y, int z, int _darknessValue)
	{
		int num = (int)this.GetLight(x, y, z, Chunk.LIGHT_TYPE.SUN);
		num -= _darknessValue;
		if (num == 15)
		{
			return num;
		}
		int light = (int)this.GetLight(x, y, z, Chunk.LIGHT_TYPE.BLOCK);
		if (num > light)
		{
			return num;
		}
		return light;
	}

	// Token: 0x06005440 RID: 21568 RVA: 0x00203288 File Offset: 0x00201488
	public byte GetLight(int x, int y, int z, Chunk.LIGHT_TYPE type)
	{
		x &= 15;
		z &= 15;
		int @byte = (int)this.chnLight.GetByte(x, y, z);
		if (type == Chunk.LIGHT_TYPE.SUN)
		{
			return (byte)(@byte & 15);
		}
		return (byte)(@byte >> 4);
	}

	// Token: 0x06005441 RID: 21569 RVA: 0x002032C0 File Offset: 0x002014C0
	public void SetLight(int x, int y, int z, byte intensity, Chunk.LIGHT_TYPE type)
	{
		x &= 15;
		z &= 15;
		int @byte = (int)this.chnLight.GetByte(x, y, z);
		int num = (int)intensity;
		if (type == Chunk.LIGHT_TYPE.SUN)
		{
			num |= @byte & 240;
		}
		else if (type == Chunk.LIGHT_TYPE.BLOCK)
		{
			num = (num << 4) | (@byte & 15);
		}
		if (num != @byte)
		{
			this.chnLight.Set(x, y, z, (long)((ulong)((byte)num)));
			this.NeedsRegenerationAt = y;
		}
		this.isModified = true;
	}

	// Token: 0x06005442 RID: 21570 RVA: 0x0020332C File Offset: 0x0020152C
	public void CheckSameLight()
	{
		this.chnLight.CheckSameValue();
	}

	// Token: 0x06005443 RID: 21571 RVA: 0x00203339 File Offset: 0x00201539
	public void CheckSameStability()
	{
		this.chnStability.CheckSameValue();
	}

	// Token: 0x06005444 RID: 21572 RVA: 0x00203348 File Offset: 0x00201548
	public static bool IsNeighbourChunksDecorated(Chunk[] _neighbours)
	{
		foreach (Chunk chunk in _neighbours)
		{
			if (chunk == null || chunk.NeedsDecoration)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005445 RID: 21573 RVA: 0x00203378 File Offset: 0x00201578
	public static bool IsNeighbourChunksLit(Chunk[] _neighbours)
	{
		foreach (Chunk chunk in _neighbours)
		{
			if (chunk == null || chunk.NeedsLightCalculation)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005446 RID: 21574 RVA: 0x002033A7 File Offset: 0x002015A7
	public Vector3i GetWorldPos()
	{
		return new Vector3i(this.m_X << 4, this.m_Y << 8, this.m_Z << 4);
	}

	// Token: 0x06005447 RID: 21575 RVA: 0x002033C6 File Offset: 0x002015C6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetBlockWorldPosX(int _x)
	{
		return (this.m_X << 4) + _x;
	}

	// Token: 0x06005448 RID: 21576 RVA: 0x002033D2 File Offset: 0x002015D2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetBlockWorldPosZ(int _z)
	{
		return (this.m_Z << 4) + _z;
	}

	// Token: 0x06005449 RID: 21577 RVA: 0x002033DE File Offset: 0x002015DE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte GetHeight(int _x, int _z)
	{
		return this.m_HeightMap[_x + _z * 16];
	}

	// Token: 0x0600544A RID: 21578 RVA: 0x002033ED File Offset: 0x002015ED
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetHeight(int _x, int _z, byte _h)
	{
		this.m_HeightMap[_x + _z * 16] = _h;
	}

	// Token: 0x0600544B RID: 21579 RVA: 0x00203400 File Offset: 0x00201600
	public byte GetMaxHeight()
	{
		byte b = 0;
		for (int i = this.m_HeightMap.Length - 1; i >= 0; i--)
		{
			byte b2 = this.m_HeightMap[i];
			if (b2 > b)
			{
				b = b2;
			}
		}
		return b;
	}

	// Token: 0x0600544C RID: 21580 RVA: 0x00203434 File Offset: 0x00201634
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte GetTerrainHeight(int _x, int _z)
	{
		return this.m_TerrainHeight[_x + _z * 16];
	}

	// Token: 0x0600544D RID: 21581 RVA: 0x00203443 File Offset: 0x00201643
	public void SetTerrainHeight(int _x, int _z, byte _h)
	{
		this.m_TerrainHeight[_x + _z * 16] = _h;
	}

	// Token: 0x0600544E RID: 21582 RVA: 0x00203454 File Offset: 0x00201654
	public byte GetTopMostTerrainHeight()
	{
		byte b = 0;
		for (int i = 0; i < this.m_TerrainHeight.Length; i++)
		{
			if (this.m_TerrainHeight[i] > b)
			{
				b = this.m_TerrainHeight[i];
			}
		}
		return b;
	}

	// Token: 0x0600544F RID: 21583 RVA: 0x0020348C File Offset: 0x0020168C
	public bool IsTopSoil(int _x, int _z)
	{
		int num = (_x + _z * 16) / 8;
		int num2 = (_x + _z * 16) % 8;
		return ((int)this.m_bTopSoilBroken[num] & (1 << num2)) == 0;
	}

	// Token: 0x06005450 RID: 21584 RVA: 0x002034C0 File Offset: 0x002016C0
	public void SetTopSoilBroken(int _x, int _z)
	{
		int num = (_x + _z * 16) / 8;
		int num2 = (_x + _z * 16) % 8;
		int num3 = (int)this.m_bTopSoilBroken[num];
		num3 |= 1 << num2;
		this.m_bTopSoilBroken[num] = (byte)num3;
	}

	// Token: 0x06005451 RID: 21585 RVA: 0x002034FC File Offset: 0x002016FC
	public BlockValue GetBlock(int _x, int _y, int _z)
	{
		if (this.IsInternalBlocksCulled && this.isInside(_x, _y, _z))
		{
			if (Chunk.bvPOIFiller.isair)
			{
				Chunk.bvPOIFiller = new BlockValue((uint)Block.GetBlockByName(Constants.cPOIFillerBlock, false).blockID);
			}
			return Chunk.bvPOIFiller;
		}
		BlockValue blockValue = BlockValue.Air;
		try
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[_y >> 2];
			if (chunkBlockLayer != null)
			{
				blockValue = chunkBlockLayer.GetAt(_x, _y, _z);
			}
		}
		catch (IndexOutOfRangeException)
		{
			Log.Error(string.Concat(new string[]
			{
				"GetBlock failed: _y = ",
				_y.ToString(),
				", len = ",
				this.m_BlockLayers.Length.ToString(),
				" (chunk ",
				this.m_X.ToString(),
				"/",
				this.m_Z.ToString(),
				")"
			}));
			throw;
		}
		blockValue.damage = this.GetDamage(_x, _y, _z);
		return blockValue;
	}

	// Token: 0x06005452 RID: 21586 RVA: 0x001B233C File Offset: 0x001B053C
	public BlockValue GetBlock(Vector3i pos)
	{
		return IBlockAccess.DefaultGetBlock(this, pos);
	}

	// Token: 0x06005453 RID: 21587 RVA: 0x001B2345 File Offset: 0x001B0545
	public BlockValue GetBlock(BlockValueRef bvRef)
	{
		return IBlockAccess.DefaultGetBlock(this, bvRef);
	}

	// Token: 0x06005454 RID: 21588 RVA: 0x00203600 File Offset: 0x00201800
	public BlockValue GetBlockNoDamage(int _x, int _y, int _z)
	{
		BlockValue blockValue = BlockValue.Air;
		try
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[_y >> 2];
			if (chunkBlockLayer != null)
			{
				blockValue = chunkBlockLayer.GetAt(_x, _y, _z);
			}
		}
		catch (IndexOutOfRangeException)
		{
			Log.Error(string.Concat(new string[]
			{
				"GetBlockNoDamage failed: _y = ",
				_y.ToString(),
				", len = ",
				this.m_BlockLayers.Length.ToString(),
				" (chunk ",
				this.m_X.ToString(),
				"/",
				this.m_Z.ToString(),
				")"
			}));
			throw;
		}
		return blockValue;
	}

	// Token: 0x06005455 RID: 21589 RVA: 0x002036B4 File Offset: 0x002018B4
	public void GetBlockColumn(int _x, int _y, int _z, BlockValue[] _blocks)
	{
		try
		{
			int num = _blocks.Length;
			for (int i = 0; i < num; i++)
			{
				BlockValue blockValue = BlockValue.Air;
				ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[_y >> 2];
				if (chunkBlockLayer != null)
				{
					blockValue = chunkBlockLayer.GetAt(_x, _y, _z);
					blockValue.damage = this.GetDamage(_x, _y, _z);
				}
				_blocks[i] = blockValue;
				_y++;
			}
		}
		catch (IndexOutOfRangeException)
		{
			Log.Error(string.Concat(new string[]
			{
				"GetBlockColumn failed: _y = ",
				_y.ToString(),
				", len = ",
				this.m_BlockLayers.Length.ToString(),
				" (chunk ",
				this.m_X.ToString(),
				"/",
				this.m_Z.ToString(),
				")"
			}));
			throw;
		}
	}

	// Token: 0x06005456 RID: 21590 RVA: 0x00203798 File Offset: 0x00201998
	public int GetBlockId(int _x, int _y, int _z)
	{
		ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[_y >> 2];
		if (chunkBlockLayer != null)
		{
			return chunkBlockLayer.GetIdAt(_x, _y, _z);
		}
		return 0;
	}

	// Token: 0x06005457 RID: 21591 RVA: 0x002037C0 File Offset: 0x002019C0
	public void CopyMeshDataFrom(Chunk _other)
	{
		for (int i = 0; i < this.m_BlockLayers.Length; i++)
		{
			if (_other.m_BlockLayers[i] == null)
			{
				if (this.m_BlockLayers[i] != null)
				{
					MemoryPools.poolCBL.FreeSync(this.m_BlockLayers[i]);
					this.m_BlockLayers[i] = null;
				}
			}
			else
			{
				if (this.m_BlockLayers[i] == null)
				{
					this.m_BlockLayers[i] = MemoryPools.poolCBL.AllocSync(true);
				}
				this.m_BlockLayers[i].CopyFrom(_other.m_BlockLayers[i]);
			}
		}
		this.chnDensity.CopyFrom(_other.chnDensity);
		this.chnDamage.CopyFrom(_other.chnDamage);
	}

	// Token: 0x06005458 RID: 21592 RVA: 0x00203865 File Offset: 0x00201A65
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte GetBiomeId(int _x, int _z)
	{
		return this.m_Biomes[_x + _z * 16];
	}

	// Token: 0x06005459 RID: 21593 RVA: 0x00203874 File Offset: 0x00201A74
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetBiomeId(int _x, int _z, byte _biomeId)
	{
		this.m_Biomes[_x + _z * 16] = _biomeId;
	}

	// Token: 0x0600545A RID: 21594 RVA: 0x00203884 File Offset: 0x00201A84
	public void FillBiomeId(byte _biomeId)
	{
		for (int i = 0; i < this.m_Biomes.Length; i++)
		{
			this.m_Biomes[i] = _biomeId;
		}
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x002038AD File Offset: 0x00201AAD
	public BiomeIntensity GetBiomeIntensity(int _x, int _z)
	{
		if (this.m_BiomeIntensities == null)
		{
			return BiomeIntensity.Default;
		}
		return new BiomeIntensity(this.m_BiomeIntensities, (_x + _z * 16) * 6);
	}

	// Token: 0x0600545C RID: 21596 RVA: 0x002038D0 File Offset: 0x00201AD0
	public void CalcBiomeIntensity(Chunk[] _neighbours)
	{
		int[] array = new int[50];
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				Array.Clear(array, 0, array.Length);
				for (int k = -16; k < 16; k++)
				{
					int num = i + k;
					int num2 = j + k;
					Chunk chunk = this;
					if (num < 0)
					{
						if (num2 < 0)
						{
							chunk = _neighbours[5];
						}
						else if (num2 >= 16)
						{
							chunk = _neighbours[6];
						}
						else
						{
							chunk = _neighbours[1];
						}
					}
					else if (num >= 16)
					{
						if (num2 < 0)
						{
							chunk = _neighbours[3];
						}
						else if (num2 >= 16)
						{
							chunk = _neighbours[4];
						}
						else
						{
							chunk = _neighbours[0];
						}
					}
					else if (num2 >= 16)
					{
						chunk = _neighbours[2];
					}
					else if (num2 < 0)
					{
						chunk = _neighbours[3];
					}
					int biomeId = (int)chunk.GetBiomeId(World.toBlockXZ(num), World.toBlockXZ(num2));
					if (biomeId >= 0 && biomeId < array.Length)
					{
						array[biomeId]++;
					}
				}
				BiomeIntensity.FromArray(array).ToArray(this.m_BiomeIntensities, (i + j * 16) * 6);
			}
		}
	}

	// Token: 0x0600545D RID: 21597 RVA: 0x002039EC File Offset: 0x00201BEC
	public void CalcDominantBiome()
	{
		int[] array = new int[50];
		for (int i = 0; i < this.m_Biomes.Length; i++)
		{
			array[(int)this.m_Biomes[i]]++;
		}
		int num = 0;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] > num)
			{
				this.DominantBiome = (byte)j;
				num = array[j];
			}
		}
	}

	// Token: 0x0600545E RID: 21598 RVA: 0x00203A4C File Offset: 0x00201C4C
	public void ResetBiomeIntensity(BiomeIntensity _v)
	{
		for (int i = 0; i < this.m_BiomeIntensities.Length; i += 6)
		{
			_v.ToArray(this.m_BiomeIntensities, i);
		}
	}

	// Token: 0x0600545F RID: 21599 RVA: 0x00203A7A File Offset: 0x00201C7A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte GetStability(int _x, int _y, int _z)
	{
		return (byte)this.chnStability.Get(_x, _y, _z);
	}

	// Token: 0x06005460 RID: 21600 RVA: 0x00203A8B File Offset: 0x00201C8B
	public void SetStability(int _x, int _y, int _z, byte _v)
	{
		this.chnStability.Set(_x, _y, _z, (long)((ulong)_v));
	}

	// Token: 0x06005461 RID: 21601 RVA: 0x00203A9E File Offset: 0x00201C9E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetDensity(int _x, int _y, int _z, sbyte _density)
	{
		this.chnDensity.Set(_x, _y, _z, (long)((ulong)((byte)_density)));
	}

	// Token: 0x06005462 RID: 21602 RVA: 0x00203AB2 File Offset: 0x00201CB2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public sbyte GetDensity(int _x, int _y, int _z)
	{
		return (sbyte)this.chnDensity.Get(_x, _y, _z);
	}

	// Token: 0x06005463 RID: 21603 RVA: 0x00203AC3 File Offset: 0x00201CC3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasSameDensityValue(int _y)
	{
		return this.chnDensity.HasSameValue(_y);
	}

	// Token: 0x06005464 RID: 21604 RVA: 0x00203AD1 File Offset: 0x00201CD1
	public sbyte GetSameDensityValue(int _y)
	{
		if (_y < 0)
		{
			return MarchingCubes.DensityTerrain;
		}
		if (_y >= 256)
		{
			return MarchingCubes.DensityAir;
		}
		return (sbyte)this.chnDensity.GetSameValue(_y);
	}

	// Token: 0x06005465 RID: 21605 RVA: 0x00203AF8 File Offset: 0x00201CF8
	public void CheckSameDensity()
	{
		this.chnDensity.CheckSameValue();
	}

	// Token: 0x06005466 RID: 21606 RVA: 0x00203B08 File Offset: 0x00201D08
	public bool IsOnlyTerrain(int _y)
	{
		int num = _y >> 2;
		return this.IsOnlyTerrainLayer(num);
	}

	// Token: 0x06005467 RID: 21607 RVA: 0x00203B20 File Offset: 0x00201D20
	public bool IsOnlyTerrainLayer(int _idx)
	{
		return _idx < 0 || _idx >= this.m_BlockLayers.Length || (this.m_BlockLayers[_idx] != null && this.m_BlockLayers[_idx].IsOnlyTerrain());
	}

	// Token: 0x06005468 RID: 21608 RVA: 0x00203B4C File Offset: 0x00201D4C
	public void CheckOnlyTerrain()
	{
		for (int i = 0; i < this.m_BlockLayers.Length; i++)
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[i];
			if (chunkBlockLayer != null)
			{
				chunkBlockLayer.CheckOnlyTerrain();
			}
		}
	}

	// Token: 0x06005469 RID: 21609 RVA: 0x00203B7E File Offset: 0x00201D7E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long GetTextureFull(int _x, int _y, int _z, int channel = 0)
	{
		if (!Chunk.IgnorePaintTextures)
		{
			return this.chnTextures[channel].Get(_x, _y, _z);
		}
		return 0L;
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x00203B9C File Offset: 0x00201D9C
	public TextureFullArray GetTextureFullArray(int _x, int _y, int _z, bool applyIgnore = true)
	{
		TextureFullArray textureFullArray;
		for (int i = 0; i < 1; i++)
		{
			textureFullArray[i] = ((applyIgnore && Chunk.IgnorePaintTextures) ? 0L : this.chnTextures[i].Get(_x, _y, _z));
		}
		return textureFullArray;
	}

	// Token: 0x0600546B RID: 21611 RVA: 0x00203BDD File Offset: 0x00201DDD
	public void SetTextureFull(int _x, int _y, int _z, long _texturefull, int channel = 0)
	{
		this.chnTextures[channel].Set(_x, _y, _z, _texturefull);
		this.isModified = true;
	}

	// Token: 0x0600546C RID: 21612 RVA: 0x00203BFC File Offset: 0x00201DFC
	public TextureFullArray GetSetTextureFullArray(int _x, int _y, int _z, TextureFullArray _texturefullArray)
	{
		TextureFullArray textureFullArray;
		for (int i = 0; i < 1; i++)
		{
			textureFullArray[i] = this.chnTextures[i].GetSet(_x, _y, _z, _texturefullArray[i]);
		}
		this.isModified = true;
		return textureFullArray;
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x00203C3D File Offset: 0x00201E3D
	public int GetBlockFaceTexture(int _x, int _y, int _z, BlockFace _face, int channel)
	{
		return (int)((this.chnTextures[channel].Get(_x, _y, _z) >> (int)(_face * (BlockFace)8)) & 255L);
	}

	// Token: 0x0600546E RID: 21614 RVA: 0x00203C60 File Offset: 0x00201E60
	public long SetBlockFaceTexture(int _x, int _y, int _z, BlockFace _face, int _texture, int channel = 0)
	{
		long num2;
		long num = (num2 = this.chnTextures[channel].Get(_x, _y, _z));
		int num3 = (int)(_face * (BlockFace)8);
		num2 &= ~(255L << num3);
		num2 |= (long)(_texture & 255) << num3;
		this.chnTextures[channel].Set(_x, _y, _z, num2);
		this.isModified = true;
		return num;
	}

	// Token: 0x0600546F RID: 21615 RVA: 0x00203CBE File Offset: 0x00201EBE
	public static int Value64FullToIndex(long _valueFull, BlockFace _blockFace)
	{
		return (int)((_valueFull >> (int)(_blockFace * (BlockFace)8)) & 255L);
	}

	// Token: 0x06005470 RID: 21616 RVA: 0x00203CD0 File Offset: 0x00201ED0
	public static long TextureIdxToTextureFullValue64(int _idx)
	{
		long num = (long)_idx;
		return ((num & 255L) << 40) | ((num & 255L) << 32) | ((num & 255L) << 24) | ((num & 255L) << 16) | ((num & 255L) << 8) | (num & 255L);
	}

	// Token: 0x06005471 RID: 21617 RVA: 0x00203D23 File Offset: 0x00201F23
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetDamage(int _x, int _y, int _z, int _damage)
	{
		this.chnDamage.Set(_x, _y, _z, (long)_damage);
	}

	// Token: 0x06005472 RID: 21618 RVA: 0x00203D36 File Offset: 0x00201F36
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetDamage(int _x, int _y, int _z)
	{
		return (int)this.chnDamage.Get(_x, _y, _z);
	}

	// Token: 0x06005473 RID: 21619 RVA: 0x00203D48 File Offset: 0x00201F48
	public bool IsAir(int _x, int _y, int _z)
	{
		return !this.IsWater(_x, _y, _z) && this.GetBlockNoDamage(_x, _y, _z).isair;
	}

	// Token: 0x06005474 RID: 21620 RVA: 0x00203D73 File Offset: 0x00201F73
	public void ClearWater()
	{
		this.chnWater.Clear(0L);
	}

	// Token: 0x06005475 RID: 21621 RVA: 0x00203D84 File Offset: 0x00201F84
	public bool IsWater(int _x, int _y, int _z)
	{
		return this.GetWater(_x, _y, _z).HasMass();
	}

	// Token: 0x06005476 RID: 21622 RVA: 0x00203DA2 File Offset: 0x00201FA2
	public WaterValue GetWater(int _x, int _y, int _z)
	{
		return WaterValue.FromRawData(this.chnWater.Get(_x, _y, _z));
	}

	// Token: 0x06005477 RID: 21623 RVA: 0x00203DB7 File Offset: 0x00201FB7
	public void SetWater(int _x, int _y, int _z, WaterValue _data)
	{
		this.SetWaterRaw(_x, _y, _z, _data);
		this.waterSimHandle.WakeNeighbours(_x, _y, _z);
	}

	// Token: 0x06005478 RID: 21624 RVA: 0x00203DD4 File Offset: 0x00201FD4
	public void SetWaterRaw(int _x, int _y, int _z, WaterValue _data)
	{
		if (!WaterUtils.CanWaterFlowThrough(this.GetBlockNoDamage(_x, _y, _z)))
		{
			_data.SetMass(0);
		}
		this.chnWater.Set(_x, _y, _z, _data.RawData);
		this.bEmptyDirty = true;
		this.bMapDirty = true;
		this.isModified = true;
		this.waterSimHandle.SetWaterMass(_x, _y, _z, _data.GetMass());
		if (_data.HasMass())
		{
			int num = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
			if ((int)this.m_HeightMap[num] < _y)
			{
				this.m_HeightMap[num] = (byte)_y;
			}
		}
	}

	// Token: 0x06005479 RID: 21625 RVA: 0x00203E60 File Offset: 0x00202060
	public void SetWaterSimUpdate(int _x, int _y, int _z, WaterValue _data, out WaterValue _lastData)
	{
		if (!WaterUtils.CanWaterFlowThrough(this.GetBlockNoDamage(_x, _y, _z)))
		{
			_lastData = WaterValue.FromRawData(this.chnWater.Get(_x, _y, _z));
			return;
		}
		long set = this.chnWater.GetSet(_x, _y, _z, _data.RawData);
		_lastData = WaterValue.FromRawData(set);
		if (_lastData.GetMass() == _data.GetMass())
		{
			return;
		}
		GameManager.Instance.World.HandleWaterLevelChanged(this.ToWorldPos(_x, _y, _z), _data.GetMassPercent());
		this.bEmptyDirty = true;
		this.bMapDirty = true;
		this.isModified = true;
		if (_data.HasMass())
		{
			int num = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
			if ((int)this.m_HeightMap[num] < _y)
			{
				this.m_HeightMap[num] = (byte)_y;
			}
		}
	}

	// Token: 0x0600547A RID: 21626 RVA: 0x00203F28 File Offset: 0x00202128
	public bool IsEmpty()
	{
		if (this.bEmptyDirty)
		{
			this.bEmpty = true;
			for (int i = 0; i < this.m_BlockLayers.Length; i++)
			{
				if (this.m_BlockLayers[i] != null)
				{
					this.bEmpty = false;
					break;
				}
			}
			if (this.bEmpty)
			{
				this.bEmpty = this.chnWater.IsDefault();
			}
			this.bEmptyDirty = false;
		}
		return this.bEmpty;
	}

	// Token: 0x0600547B RID: 21627 RVA: 0x00203F90 File Offset: 0x00202190
	public bool IsEmpty(int _y)
	{
		int num = _y >> 2;
		return this.IsEmptyLayer(num);
	}

	// Token: 0x0600547C RID: 21628 RVA: 0x00203FA8 File Offset: 0x002021A8
	public bool IsEmptyLayer(int _idx)
	{
		return (ulong)_idx >= (ulong)((long)this.m_BlockLayers.Length) || (this.m_BlockLayers[_idx] == null && this.chnWater.IsDefaultLayer(_idx));
	}

	// Token: 0x0600547D RID: 21629 RVA: 0x00203FD4 File Offset: 0x002021D4
	public int RecalcHeights()
	{
		int num = 0;
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				int num2 = ChunkBlockLayerLegacy.CalcOffset(j, i);
				this.m_HeightMap[num2] = 0;
				for (int k = 255; k >= 0; k--)
				{
					ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[k >> 2];
					if ((chunkBlockLayer != null && !chunkBlockLayer.GetAt(j, k, i).isair) || this.IsWater(j, k, i))
					{
						this.m_HeightMap[num2] = (byte)k;
						num = Utils.FastMax(num, k);
						break;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0600547E RID: 21630 RVA: 0x0020407C File Offset: 0x0020227C
	public byte RecalcHeightAt(int _x, int _yMaxStart, int _z)
	{
		int num = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
		for (int i = _yMaxStart; i >= 0; i--)
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[i >> 2];
			if ((chunkBlockLayer != null && !chunkBlockLayer.GetAt(_x, i, _z).isair) || this.IsWater(_x, i, _z))
			{
				this.m_HeightMap[num] = (byte)i;
				return (byte)i;
			}
		}
		return 0;
	}

	// Token: 0x0600547F RID: 21631 RVA: 0x002040E0 File Offset: 0x002022E0
	public BlockValue SetBlock(WorldBase _world, int x, int y, int z, BlockValue _blockValue, bool _notifyAddChange = true, bool _notifyRemove = true, bool _fromReset = false, bool _poiOwned = false, int _changedByEntityId = -1)
	{
		Vector3i vector3i = new Vector3i((this.m_X << 4) + x, y, (this.m_Z << 4) + z);
		BlockValue blockValue = this.SetBlockRaw(x, y, z, _blockValue);
		bool flag = !blockValue.isair && _blockValue.isair;
		if (flag)
		{
			this.waterSimHandle.WakeNeighbours(x, y, z);
			if (blockValue.Block.StabilitySupport)
			{
				MultiBlockManager.Instance.SetOversizedStabilityDirty(vector3i);
			}
		}
		if (!_blockValue.ischild)
		{
			MultiBlockManager.Instance.UpdateTrackedBlockData(vector3i, _blockValue, _poiOwned);
		}
		_blockValue = this.GetBlock(x, y, z);
		if (_notifyRemove && !blockValue.isair && blockValue.type != _blockValue.type)
		{
			Block block = blockValue.Block;
			if (block != null)
			{
				block.OnBlockRemoved(_world, this, vector3i, blockValue);
			}
		}
		if (_notifyAddChange)
		{
			Block block2 = _blockValue.Block;
			if (block2 != null)
			{
				if (blockValue.type != _blockValue.type)
				{
					if (!_blockValue.isair)
					{
						PlatformUserIdentifierAbs platformUserIdentifierAbs = null;
						if (_changedByEntityId != -1)
						{
							platformUserIdentifierAbs = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(_changedByEntityId).PrimaryId;
						}
						block2.OnBlockAdded(_world, this, vector3i, _blockValue, platformUserIdentifierAbs);
					}
				}
				else
				{
					block2.OnBlockValueChanged(_world, this, vector3i, blockValue, _blockValue);
					if (_fromReset)
					{
						block2.OnBlockReset(_world, this, vector3i, _blockValue);
					}
				}
			}
		}
		if (flag)
		{
			this.RemoveBlockTrigger(new Vector3i(x, y, z));
			GameEventManager.Current.BlockRemoved(vector3i);
		}
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !GameManager.Instance.IsEditMode() && BlockLimitTracker.instance != null && !blockValue.Equals(_blockValue))
		{
			BlockLimitTracker.instance.TryRemoveOrReplaceBlock(blockValue, _blockValue, vector3i);
			if (!flag)
			{
				BlockLimitTracker.instance.TryAddTrackedBlock(_blockValue, vector3i, _changedByEntityId);
			}
			BlockLimitTracker.instance.ServerUpdateClients();
		}
		return blockValue;
	}

	// Token: 0x06005480 RID: 21632 RVA: 0x002042A0 File Offset: 0x002024A0
	public BlockValue SetBlockRaw(int _x, int _y, int _z, BlockValue _blockValue)
	{
		if (_y >= 255)
		{
			return BlockValue.Air;
		}
		Block block = _blockValue.Block;
		if (block == null)
		{
			return BlockValue.Air;
		}
		if (_blockValue.isWater)
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[_y >> 2];
			BlockValue blockValue = ((chunkBlockLayer != null) ? chunkBlockLayer.GetAt(_x, _y, _z) : BlockValue.Air);
			if (!WaterUtils.CanWaterFlowThrough(blockValue))
			{
				this.SetBlockRaw(_x, _y, _z, BlockValue.Air);
			}
			this.SetWater(_x, _y, _z, WaterValue.Full);
			return blockValue;
		}
		if (!WaterUtils.CanWaterFlowThrough(_blockValue))
		{
			this.SetWater(_x, _y, _z, WaterValue.Empty);
		}
		this.waterSimHandle.SetVoxelSolid(_x, _y, _z, BlockFaceFlags.RotateFlags(block.WaterFlowMask, _blockValue.rotation));
		BlockValue blockValue2 = BlockValue.Air;
		int num = _y >> 2;
		ChunkBlockLayer chunkBlockLayer2 = this.m_BlockLayers[num];
		if (chunkBlockLayer2 != null)
		{
			int num2 = ChunkBlockLayer.CalcOffset(_x, _y, _z);
			blockValue2 = chunkBlockLayer2.GetAt(num2);
			chunkBlockLayer2.SetAt(num2, _blockValue.rawData);
			if (!blockValue2.ischild)
			{
				blockValue2.damage = this.GetDamage(_x, _y, _z);
			}
		}
		else if (!_blockValue.isair)
		{
			chunkBlockLayer2 = MemoryPools.poolCBL.AllocSync(true);
			this.m_BlockLayers[num] = chunkBlockLayer2;
			chunkBlockLayer2.SetAt(_x, _y, _z, _blockValue.rawData);
		}
		if (!_blockValue.ischild)
		{
			this.SetDamage(_x, _y, _z, _blockValue.damage);
		}
		Block block2 = blockValue2.Block;
		if (blockValue2.type != _blockValue.type)
		{
			if (!blockValue2.ischild && block2.IndexName != null && this.IndexedBlocks.ContainsKey(block2.IndexName))
			{
				this.IndexedBlocks[block2.IndexName].Remove(new Vector3i(_x, _y, _z));
				if (this.IndexedBlocks[block2.IndexName].Count == 0)
				{
					this.IndexedBlocks.Remove(block2.IndexName);
				}
			}
			if (!_blockValue.ischild && block.IndexName != null && block.FilterIndexType(_blockValue))
			{
				if (!this.IndexedBlocks.ContainsKey(block.IndexName))
				{
					this.IndexedBlocks[block.IndexName] = new List<Vector3i>();
				}
				this.IndexedBlocks[block.IndexName].Add(new Vector3i(_x, _y, _z));
			}
		}
		int num3 = ChunkBlockLayerLegacy.CalcOffset(_x, _z);
		if (_blockValue.isair)
		{
			if ((int)this.m_HeightMap[num3] == _y)
			{
				this.RecalcHeightAt(_x, _y - 1, _z);
			}
		}
		else if ((int)this.m_HeightMap[num3] < _y)
		{
			this.m_HeightMap[num3] = (byte)_y;
		}
		if (blockValue2.isair && !_blockValue.isair && !_blockValue.ischild)
		{
			if (!block.IsRandomlyTick)
			{
				goto IL_03CF;
			}
			DictionaryKeyList<Vector3i, int> dictionaryKeyList = this.tickedBlocks;
			lock (dictionaryKeyList)
			{
				this.tickedBlocks.Replace(this.ToWorldPos(_x, _y, _z), 0);
				goto IL_03CF;
			}
		}
		if (!blockValue2.isair && _blockValue.isair && !blockValue2.ischild)
		{
			if (!block2.IsRandomlyTick)
			{
				goto IL_03CF;
			}
			DictionaryKeyList<Vector3i, int> dictionaryKeyList = this.tickedBlocks;
			lock (dictionaryKeyList)
			{
				this.tickedBlocks.Remove(this.ToWorldPos(_x, _y, _z));
				goto IL_03CF;
			}
		}
		if (block2.IsRandomlyTick && !block.IsRandomlyTick && !blockValue2.ischild)
		{
			DictionaryKeyList<Vector3i, int> dictionaryKeyList = this.tickedBlocks;
			lock (dictionaryKeyList)
			{
				this.tickedBlocks.Remove(this.ToWorldPos(_x, _y, _z));
				goto IL_03CF;
			}
		}
		if (!block2.IsRandomlyTick && block.IsRandomlyTick && !_blockValue.ischild)
		{
			DictionaryKeyList<Vector3i, int> dictionaryKeyList = this.tickedBlocks;
			lock (dictionaryKeyList)
			{
				this.tickedBlocks.Replace(this.ToWorldPos(_x, _y, _z), 0);
			}
		}
		IL_03CF:
		this.bMapDirty = true;
		this.isModified = true;
		this.bEmptyDirty = true;
		return blockValue2;
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x002046C8 File Offset: 0x002028C8
	public bool FillBlockRaw(int _heightIncl, BlockValue _blockValue)
	{
		if (_heightIncl >= 255)
		{
			return false;
		}
		if (_blockValue.isair || _blockValue.ischild)
		{
			return false;
		}
		Block block = _blockValue.Block;
		if (block == null)
		{
			return false;
		}
		if (_blockValue.isWater)
		{
			return false;
		}
		if (!this.IsEmpty())
		{
			return false;
		}
		uint rawData = _blockValue.rawData;
		sbyte b = (block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir);
		int damage = _blockValue.damage;
		int i;
		for (i = 0; i <= _heightIncl - 4; i += 4)
		{
			int num = i >> 2;
			if (this.m_BlockLayers[num] == null)
			{
				this.m_BlockLayers[num] = MemoryPools.poolCBL.AllocSync(true);
			}
			this.m_BlockLayers[num].Fill(rawData);
		}
		while (i <= _heightIncl)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					int num2 = i >> 2;
					if (this.m_BlockLayers[num2] == null)
					{
						this.m_BlockLayers[num2] = MemoryPools.poolCBL.AllocSync(true);
					}
					this.m_BlockLayers[num2].SetAt(j, i, k, rawData);
				}
			}
			i++;
		}
		List<Vector3i> list = null;
		if (block.IndexName != null)
		{
			if (!this.IndexedBlocks.ContainsKey(block.IndexName))
			{
				this.IndexedBlocks[block.IndexName] = new List<Vector3i>();
			}
			list = this.IndexedBlocks[block.IndexName];
			list.Clear();
		}
		DictionaryKeyList<Vector3i, int> dictionaryKeyList = this.tickedBlocks;
		lock (dictionaryKeyList)
		{
			this.tickedBlocks.Clear();
			for (i = 0; i <= _heightIncl; i++)
			{
				for (int l = 0; l < 16; l++)
				{
					for (int m = 0; m < 16; m++)
					{
						this.SetDensity(l, i, m, b);
						this.SetDamage(l, i, m, damage);
						if (list != null)
						{
							list.Add(new Vector3i(l, i, m));
						}
						if (block.IsRandomlyTick)
						{
							this.tickedBlocks.Replace(this.ToWorldPos(l, i, m), 0);
						}
					}
				}
			}
		}
		for (int n = 0; n < 16; n++)
		{
			for (int num3 = 0; num3 < 16; num3++)
			{
				int num4 = ChunkBlockLayerLegacy.CalcOffset(n, num3);
				this.m_HeightMap[num4] = (byte)_heightIncl;
			}
		}
		this.bMapDirty = true;
		this.isModified = true;
		this.bEmptyDirty = true;
		return true;
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x00204948 File Offset: 0x00202B48
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsInChunk(Vector3 position)
	{
		float x = position.x;
		if (x >= 0f && x < 16f)
		{
			float y = position.y;
			if (y >= 0f && y < 256f)
			{
				float z = position.z;
				if (z >= 0f)
				{
					return z < 16f;
				}
			}
		}
		return false;
	}

	// Token: 0x06005483 RID: 21635 RVA: 0x0020499C File Offset: 0x00202B9C
	[PublicizedFrom(EAccessModifier.Private)]
	public PropValue AddProp(Vector3 position, Quaternion rotation, Vector3 scale, BlockValue blockValue)
	{
		if (blockValue.isair)
		{
			Log.Warning(string.Format("Can not create a prop out of an air block. BlockValue({0})", blockValue));
			return PropValue.AIR;
		}
		if (!this.IsInChunk(position))
		{
			Log.Warning(string.Format("Prop being created is not contained within this chunk. Position: {0}", position));
		}
		Quaternion normalized = rotation.normalized;
		PropValue propValue = new PropValue
		{
			transform = new PropTransform
			{
				position = position,
				rotation = normalized,
				scale = scale
			},
			blockValue = blockValue
		};
		int num = this.propIdNext;
		this.propIdNext = num + 1;
		int num2 = num;
		this.props[num2] = propValue;
		return propValue;
	}

	// Token: 0x06005484 RID: 21636 RVA: 0x00204A54 File Offset: 0x00202C54
	public PropValue GetProp(int chunkX, int chunkZ, int propId)
	{
		if (chunkX != this.X || chunkZ != this.Z)
		{
			return PropValue.AIR;
		}
		PropValue propValue;
		if (!this.props.TryGetValue(propId, out propValue))
		{
			return PropValue.AIR;
		}
		return propValue;
	}

	// Token: 0x06005485 RID: 21637 RVA: 0x00204A90 File Offset: 0x00202C90
	public PropValue GetProp(long chunkKey, int propId)
	{
		return this.GetProp(WorldChunkCache.extractX(chunkKey), WorldChunkCache.extractZ(chunkKey), propId);
	}

	// Token: 0x06005486 RID: 21638 RVA: 0x001B24D1 File Offset: 0x001B06D1
	public PropValue GetProp(Vector2i chunkPos, int propId)
	{
		return IBlockAccess.DefaultGetProp(this, chunkPos, propId);
	}

	// Token: 0x06005487 RID: 21639 RVA: 0x001B24DB File Offset: 0x001B06DB
	public PropValue GetProp(PropRef propRef)
	{
		return IBlockAccess.DefaultGetProp(this, propRef);
	}

	// Token: 0x06005488 RID: 21640 RVA: 0x00204AA8 File Offset: 0x00202CA8
	public PropValue SetProp(int? propId = null, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null, BlockValue? blockValue = null)
	{
		if (propId == null)
		{
			if (position == null || rotation == null || scale == null || blockValue == null)
			{
				Log.Error(string.Format("Creating a new prop requires valid position, rotation, scale and block values. Position{0}, Rotation{1}, Scale{2}, BlockValue({3})", new object[] { position, rotation, scale, blockValue }));
				return PropValue.AIR;
			}
			return this.AddProp(position.Value, rotation.Value, scale.Value, blockValue.Value);
		}
		else
		{
			PropValue propValue;
			if (!this.props.TryGetValue(propId.Value, out propValue))
			{
				Log.Error(string.Format("Could not find prop with id {0}.", propId.Value));
				return PropValue.AIR;
			}
			bool flag = false;
			if (blockValue != null)
			{
				if (blockValue.Value.isair)
				{
					this.props.Remove(propId.Value);
					return PropValue.AIR;
				}
				if (propValue.blockValue.rawData != blockValue.Value.rawData)
				{
					propValue.blockValue.rawData = blockValue.Value.rawData;
					flag = true;
				}
				if (propValue.blockValue.damage != blockValue.Value.damage)
				{
					propValue.blockValue.damage = blockValue.Value.damage;
					flag = true;
				}
			}
			if (position != null)
			{
				if (!this.IsInChunk(position.Value))
				{
					Log.Warning(string.Format("Prop being updated is not contained within this chunk. Position: {0}", position.Value));
				}
				if (propValue.position != position.Value)
				{
					propValue.position = position.Value;
					flag = true;
				}
			}
			if (rotation != null)
			{
				Quaternion normalized = rotation.Value.normalized;
				if (propValue.rotation != normalized)
				{
					propValue.rotation = normalized;
					flag = true;
				}
			}
			if (scale != null)
			{
				Vector3 value = scale.Value;
				if (propValue.scale != value)
				{
					propValue.scale = value;
					flag = true;
				}
			}
			if (!flag)
			{
				return propValue;
			}
			this.props[propId.Value] = propValue;
			return propValue;
		}
	}

	// Token: 0x06005489 RID: 21641 RVA: 0x00204CEF File Offset: 0x00202EEF
	public DictionaryKeyList<Vector3i, int> GetTickedBlocks()
	{
		return this.tickedBlocks;
	}

	// Token: 0x0600548A RID: 21642 RVA: 0x00204CF8 File Offset: 0x00202EF8
	public void RemoveTileEntityAt<T>(World world, Vector3i _posInChunk)
	{
		TileEntity tileEntity;
		if (this.tileEntities.dict.TryGetValue(_posInChunk, out tileEntity) && tileEntity is T)
		{
			tileEntity.IsRemoving = true;
			tileEntity.OnRemove(world);
			this.tileEntities.Remove(_posInChunk);
			tileEntity.IsRemoving = false;
		}
		this.isModified = true;
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x00204D4B File Offset: 0x00202F4B
	public void RemoveAllTileEntities()
	{
		this.isModified = this.tileEntities.Count > 0;
		this.tileEntities.Clear();
	}

	// Token: 0x0600548C RID: 21644 RVA: 0x00204D6C File Offset: 0x00202F6C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public byte GetHeight(int _blockOffset)
	{
		return this.m_HeightMap[_blockOffset];
	}

	// Token: 0x0600548D RID: 21645 RVA: 0x00204D76 File Offset: 0x00202F76
	public void AddTileEntity(TileEntity _te)
	{
		this.tileEntities.Set(_te.localChunkPos, _te);
	}

	// Token: 0x0600548E RID: 21646 RVA: 0x00204D8C File Offset: 0x00202F8C
	public void RemoveTileEntity(World world, TileEntity _te)
	{
		TileEntity tileEntity;
		if (this.tileEntities.dict.TryGetValue(_te.localChunkPos, out tileEntity) && tileEntity != null)
		{
			tileEntity.IsRemoving = true;
			tileEntity.OnRemove(world);
			this.tileEntities.Remove(_te.localChunkPos);
			tileEntity.IsRemoving = false;
			this.isModified = true;
		}
	}

	// Token: 0x0600548F RID: 21647 RVA: 0x00204DE4 File Offset: 0x00202FE4
	public TileEntity GetTileEntity(Vector3i _blockPosInChunk)
	{
		TileEntity tileEntity;
		if (!this.tileEntities.dict.TryGetValue(_blockPosInChunk, out tileEntity))
		{
			return null;
		}
		return tileEntity;
	}

	// Token: 0x06005490 RID: 21648 RVA: 0x00204E09 File Offset: 0x00203009
	public DictionaryList<Vector3i, TileEntity> GetTileEntities()
	{
		return this.tileEntities;
	}

	// Token: 0x06005491 RID: 21649 RVA: 0x00204E11 File Offset: 0x00203011
	public void AddSleeperVolumeId(int id)
	{
		if (!this.sleeperVolumes.Contains(id))
		{
			if (this.sleeperVolumes.Count < 255)
			{
				this.sleeperVolumes.Add(id);
				return;
			}
			Log.Error("Chunk AddSleeperVolumeId at max");
		}
	}

	// Token: 0x06005492 RID: 21650 RVA: 0x00204E4A File Offset: 0x0020304A
	public List<int> GetSleeperVolumes()
	{
		return this.sleeperVolumes;
	}

	// Token: 0x06005493 RID: 21651 RVA: 0x00204E52 File Offset: 0x00203052
	public void AddTriggerVolumeId(int id)
	{
		if (!this.triggerVolumes.Contains(id))
		{
			if (this.triggerVolumes.Count < 255)
			{
				this.triggerVolumes.Add(id);
				return;
			}
			Log.Error("Chunk AddTriggerVolumeId at max");
		}
	}

	// Token: 0x06005494 RID: 21652 RVA: 0x00204E8B File Offset: 0x0020308B
	public List<int> GetTriggerVolumes()
	{
		return this.triggerVolumes;
	}

	// Token: 0x06005495 RID: 21653 RVA: 0x00204E93 File Offset: 0x00203093
	public void AddWallVolumeId(int id)
	{
		if (!this.wallVolumes.Contains(id))
		{
			if (this.wallVolumes.Count < 255)
			{
				this.wallVolumes.Add(id);
				return;
			}
			Log.Error("Chunk AddWallVolume at max");
		}
	}

	// Token: 0x06005496 RID: 21654 RVA: 0x00204ECC File Offset: 0x002030CC
	public List<int> GetWallVolumes()
	{
		return this.wallVolumes;
	}

	// Token: 0x06005497 RID: 21655 RVA: 0x00204ED4 File Offset: 0x002030D4
	public int GetTickRefCount(int _layerIdx)
	{
		if (this.m_BlockLayers[_layerIdx] == null)
		{
			return 0;
		}
		return this.m_BlockLayers[_layerIdx].GetTickRefCount();
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x00204EEF File Offset: 0x002030EF
	public DictionaryList<Vector3i, BlockTrigger> GetBlockTriggers()
	{
		return this.triggerData;
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x00204EF7 File Offset: 0x002030F7
	public void AddBlockTrigger(BlockTrigger _td)
	{
		this.triggerData.Set(_td.LocalChunkPos, _td);
		this.isModified = true;
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x00204F14 File Offset: 0x00203114
	public void RemoveBlockTrigger(BlockTrigger _td)
	{
		BlockTrigger blockTrigger;
		if (this.triggerData.dict.TryGetValue(_td.LocalChunkPos, out blockTrigger) && blockTrigger != null)
		{
			this.triggerData.Remove(_td.LocalChunkPos);
			this.isModified = true;
		}
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x00204F57 File Offset: 0x00203157
	public void RemoveBlockTrigger(Vector3i _blockPos)
	{
		if (this.triggerData.dict.ContainsKey(_blockPos))
		{
			this.triggerData.Remove(_blockPos);
			this.isModified = true;
		}
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x00204F80 File Offset: 0x00203180
	public BlockTrigger GetBlockTrigger(Vector3i _blockPosInChunk)
	{
		BlockTrigger blockTrigger;
		this.triggerData.dict.TryGetValue(_blockPosInChunk, out blockTrigger);
		return blockTrigger;
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x00204FA4 File Offset: 0x002031A4
	public void UpdateTick(World _world, bool _bSpawnEnemies)
	{
		this.ProfilerBegin("TeTick");
		for (int i = 0; i < this.tileEntities.list.Count; i++)
		{
			this.tileEntities.list[i].UpdateTick(_world);
		}
		this.ProfilerEnd();
	}

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x0600549E RID: 21662 RVA: 0x00204FF4 File Offset: 0x002031F4
	public bool NeedsTicking
	{
		get
		{
			return this.tileEntities.Count > 0 || this.sleeperVolumes.Count > 0;
		}
	}

	// Token: 0x0600549F RID: 21663 RVA: 0x00205014 File Offset: 0x00203214
	public bool IsOpenSkyAbove(int _x, int _y, int _z)
	{
		return _y >= (int)this.GetHeight(_x, _z);
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x00205024 File Offset: 0x00203224
	public void GetLivingEntitiesInBounds(EntityAlive _excludeEntity, Bounds _aabb, List<EntityAlive> _entityOutputList)
	{
		int num = Utils.Fastfloor((double)(_aabb.min.y - 5f) / 16.0);
		int num2 = Utils.Fastfloor((double)(_aabb.max.y + 5f) / 16.0);
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= 16)
		{
			num2 = 15;
		}
		for (int i = num; i <= num2; i++)
		{
			List<Entity> list = this.entityLists[i];
			for (int j = 0; j < list.Count; j++)
			{
				EntityAlive entityAlive = list[j] as EntityAlive;
				if (!(entityAlive == null) && !(entityAlive == _excludeEntity) && !entityAlive.IsDead() && entityAlive.boundingBox.Intersects(_aabb) && (!(_excludeEntity != null) || _excludeEntity.CanCollideWith(entityAlive)))
				{
					_entityOutputList.Add(entityAlive);
				}
			}
		}
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x00205108 File Offset: 0x00203308
	public void GetEntitiesInBounds(Entity _excludeEntity, Bounds _aabb, List<Entity> _entityOutputList, bool isAlive)
	{
		int num = Utils.Fastfloor((double)(_aabb.min.y - 5f) / 16.0);
		int num2 = Utils.Fastfloor((double)(_aabb.max.y + 5f) / 16.0);
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= 16)
		{
			num2 = 15;
		}
		for (int i = num; i <= num2; i++)
		{
			List<Entity> list = this.entityLists[i];
			for (int j = 0; j < list.Count; j++)
			{
				Entity entity = list[j];
				if (!(entity == _excludeEntity) && isAlive == entity.IsAlive() && entity.boundingBox.Intersects(_aabb) && (!(_excludeEntity != null) || _excludeEntity.CanCollideWith(entity)))
				{
					_entityOutputList.Add(entity);
				}
			}
		}
	}

	// Token: 0x060054A2 RID: 21666 RVA: 0x002051DC File Offset: 0x002033DC
	public void GetEntitiesInBounds(FastTags<TagGroup.Global> _tags, Bounds _bb, List<Entity> _list)
	{
		int num = Utils.Fastfloor((double)(_bb.min.y - 5f) / 16.0);
		int num2 = Utils.Fastfloor((double)(_bb.max.y + 5f) / 16.0);
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= 16)
		{
			num = 15;
		}
		if (num2 >= 16)
		{
			num2 = 15;
		}
		else if (num2 < 0)
		{
			num2 = 0;
		}
		for (int i = num; i <= num2; i++)
		{
			List<Entity> list = this.entityLists[i];
			for (int j = 0; j < list.Count; j++)
			{
				Entity entity = list[j];
				if (entity.HasAnyTags(_tags) && entity.boundingBox.Intersects(_bb))
				{
					_list.Add(entity);
				}
			}
		}
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x002052A4 File Offset: 0x002034A4
	public void GetEntitiesInBounds(Type _class, Bounds _bb, List<Entity> _list)
	{
		int num = Utils.Fastfloor((double)(_bb.min.y - 5f) / 16.0);
		int num2 = Utils.Fastfloor((double)(_bb.max.y + 5f) / 16.0);
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= 16)
		{
			num = 15;
		}
		if (num2 >= 16)
		{
			num2 = 15;
		}
		else if (num2 < 0)
		{
			num2 = 0;
		}
		for (int i = num; i <= num2; i++)
		{
			List<Entity> list = this.entityLists[i];
			for (int j = 0; j < list.Count; j++)
			{
				Entity entity = list[j];
				if (_class.IsAssignableFrom(entity.GetType()) && entity.boundingBox.Intersects(_bb))
				{
					_list.Add(entity);
				}
			}
		}
	}

	// Token: 0x060054A4 RID: 21668 RVA: 0x00205374 File Offset: 0x00203574
	public void GetEntitiesAround(EntityFlags _mask, Vector3 _pos, float _radius, List<Entity> _list)
	{
		int num = Utils.Fastfloor((double)(_pos.y - _radius) / 16.0);
		int num2 = Utils.Fastfloor((double)(_pos.y + _radius) / 16.0);
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= 16)
		{
			num = 15;
		}
		if (num2 >= 16)
		{
			num2 = 15;
		}
		else if (num2 < 0)
		{
			num2 = 0;
		}
		float num3 = _radius * _radius;
		for (int i = num; i <= num2; i++)
		{
			List<Entity> list = this.entityLists[i];
			for (int j = 0; j < list.Count; j++)
			{
				Entity entity = list[j];
				if ((entity.entityFlags & _mask) != EntityFlags.None && (entity.position - _pos).sqrMagnitude <= num3)
				{
					_list.Add(entity);
				}
			}
		}
	}

	// Token: 0x060054A5 RID: 21669 RVA: 0x0020543C File Offset: 0x0020363C
	public void GetEntitiesAround(EntityFlags _flags, EntityFlags _mask, Vector3 _pos, float _radius, List<Entity> _list)
	{
		int num = Utils.Fastfloor((double)(_pos.y - _radius) / 16.0);
		int num2 = Utils.Fastfloor((double)(_pos.y + _radius) / 16.0);
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= 16)
		{
			num = 15;
		}
		if (num2 >= 16)
		{
			num2 = 15;
		}
		else if (num2 < 0)
		{
			num2 = 0;
		}
		float num3 = _radius * _radius;
		for (int i = num; i <= num2; i++)
		{
			List<Entity> list = this.entityLists[i];
			for (int j = 0; j < list.Count; j++)
			{
				Entity entity = list[j];
				if ((entity.entityFlags & _mask) == _flags && (entity.position - _pos).sqrMagnitude <= num3)
				{
					_list.Add(entity);
				}
			}
		}
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x00205508 File Offset: 0x00203708
	public void RemoveEntityFromChunk(Entity _entity)
	{
		int y = _entity.chunkPosAddedEntityTo.y;
		this.entityLists[y].Remove(_entity);
		this.isModified = true;
		bool flag = false;
		for (int i = 0; i < 16; i++)
		{
			if (this.entityLists[i].Count > 0)
			{
				flag = true;
				break;
			}
		}
		this.hasEntities = flag;
	}

	// Token: 0x060054A7 RID: 21671 RVA: 0x00205564 File Offset: 0x00203764
	public void AddEntityToChunk(Entity _entity)
	{
		this.hasEntities = true;
		int num = World.toChunkXZ(Utils.Fastfloor(_entity.position.x));
		int num2 = World.toChunkXZ(Utils.Fastfloor(_entity.position.z));
		if (num != this.m_X || num2 != this.m_Z)
		{
			Log.Error(string.Concat(new string[]
			{
				"Wrong entity chunk position! ",
				(_entity != null) ? _entity.ToString() : null,
				" x=",
				num.ToString(),
				" z=",
				num2.ToString(),
				"/",
				(this != null) ? this.ToString() : null
			}));
		}
		int num3 = Utils.Fastfloor((double)_entity.position.y / 16.0);
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num3 >= 16)
		{
			num3 = 15;
		}
		_entity.addedToChunk = true;
		_entity.chunkPosAddedEntityTo.x = this.m_X;
		_entity.chunkPosAddedEntityTo.y = num3;
		_entity.chunkPosAddedEntityTo.z = this.m_Z;
		this.entityLists[num3].Add(_entity);
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x0020568C File Offset: 0x0020388C
	public void AdJustEntityTracking(Entity _entity)
	{
		if (!_entity.addedToChunk)
		{
			return;
		}
		int num = Utils.Fastfloor((double)_entity.position.y / 16.0);
		if (num < 0)
		{
			num = 0;
		}
		if (num >= 16)
		{
			num = 15;
		}
		if (_entity.chunkPosAddedEntityTo.y != num)
		{
			this.entityLists[_entity.chunkPosAddedEntityTo.y].Remove(_entity);
			_entity.chunkPosAddedEntityTo.y = num;
			this.entityLists[num].Add(_entity);
			this.isModified = true;
		}
	}

	// Token: 0x060054A9 RID: 21673 RVA: 0x00205714 File Offset: 0x00203914
	public Bounds GetAABB()
	{
		return this.boundingBox;
	}

	// Token: 0x060054AA RID: 21674 RVA: 0x0020571C File Offset: 0x0020391C
	public static Bounds CalculateAABB(int _chunkX, int _chunkY, int _chunkZ)
	{
		return BoundsUtils.BoundsForMinMax((float)(_chunkX * 16), (float)(_chunkY * 256), (float)(_chunkZ * 16), (float)(_chunkX * 16 + 16), (float)(_chunkY * 256 + 256), (float)(_chunkZ * 16 + 16));
	}

	// Token: 0x060054AB RID: 21675 RVA: 0x00205754 File Offset: 0x00203954
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBounds()
	{
		this.boundingBox = Chunk.CalculateAABB(this.m_X, this.m_Y, this.m_Z);
		this.worldPosIMin.x = this.m_X << 4;
		this.worldPosIMin.y = this.m_Y << 8;
		this.worldPosIMin.z = this.m_Z << 4;
		this.worldPosIMax.x = this.worldPosIMin.x + 15;
		this.worldPosIMax.y = this.worldPosIMin.y + 255;
		this.worldPosIMax.z = this.worldPosIMin.z + 15;
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x00205805 File Offset: 0x00203A05
	public int GetTris()
	{
		return this.totalTris;
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x00205810 File Offset: 0x00203A10
	public int GetTrisInMesh(int _idx)
	{
		int num = 0;
		for (int i = 0; i < this.trisInMesh.GetLength(0); i++)
		{
			num += this.trisInMesh[i][_idx];
		}
		return num;
	}

	// Token: 0x060054AE RID: 21678 RVA: 0x00205844 File Offset: 0x00203A44
	public int GetSizeOfMesh(int _idx)
	{
		int num = 0;
		for (int i = 0; i < this.trisInMesh.GetLength(0); i++)
		{
			num += this.sizeOfMesh[i][_idx];
		}
		return num;
	}

	// Token: 0x060054AF RID: 21679 RVA: 0x00205878 File Offset: 0x00203A78
	public int GetUsedMem()
	{
		this.TotalMemory = 0;
		for (int i = 0; i < this.m_BlockLayers.Length; i++)
		{
			this.TotalMemory += ((this.m_BlockLayers[i] != null) ? this.m_BlockLayers[i].GetUsedMem() : 0);
		}
		this.TotalMemory += 12;
		this.TotalMemory += this.m_TerrainHeight.Length;
		this.TotalMemory += this.m_HeightMap.Length;
		this.TotalMemory += this.m_Biomes.Length;
		this.TotalMemory += this.m_BiomeIntensities.Length;
		this.TotalMemory += this.m_NormalX.Length;
		this.TotalMemory += this.m_NormalY.Length;
		this.TotalMemory += this.m_NormalZ.Length;
		this.TotalMemory += this.chnStability.GetUsedMem();
		this.TotalMemory += this.chnLight.GetUsedMem();
		this.TotalMemory += this.chnDensity.GetUsedMem();
		this.TotalMemory += this.chnDamage.GetUsedMem();
		for (int j = 0; j < 1; j++)
		{
			this.TotalMemory += this.chnTextures[j].GetUsedMem();
		}
		this.TotalMemory += this.chnWater.GetUsedMem();
		return this.TotalMemory;
	}

	// Token: 0x060054B0 RID: 21680 RVA: 0x00205A0C File Offset: 0x00203C0C
	public void GetTextureChannelMemory(out int[] texMem)
	{
		texMem = new int[1];
		for (int i = 0; i < 1; i++)
		{
			texMem[i] = this.chnTextures[i].GetUsedMem();
		}
	}

	// Token: 0x060054B1 RID: 21681 RVA: 0x00205A40 File Offset: 0x00203C40
	public void OnLoadedFromCache()
	{
		this.NeedsRegeneration = true;
		this.isModified = true;
		this.InProgressRegeneration = false;
		this.InProgressSaving = false;
		this.InProgressCopying = false;
		this.InProgressDecorating = false;
		this.InProgressLighting = false;
		this.InProgressUnloading = false;
		this.NeedsOnlyCollisionMesh = false;
		this.IsCollisionMeshGenerated = false;
		this.entityStubs.Clear();
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < this.entityLists[i].Count; j++)
			{
				if (this.entityLists[i][j].IsSavedToFile())
				{
					this.entityStubs.Add(new EntityCreationData(this.entityLists[i][j], true));
				}
			}
			this.entityLists[i].Clear();
		}
	}

	// Token: 0x060054B2 RID: 21682 RVA: 0x00205B14 File Offset: 0x00203D14
	public void OnLoad(World _world)
	{
		if (!_world.IsRemote())
		{
			for (int i = 0; i < this.entityStubs.Count; i++)
			{
				EntityCreationData entityCreationData = this.entityStubs[i];
				if (!(_world.GetEntity(entityCreationData.id) != null))
				{
					this.SpawnEntityAsync(_world, entityCreationData, null);
				}
			}
			this.removeExpiredCustomChunkDataEntries(_world.GetWorldTime());
		}
		if (!_world.IsEditor())
		{
			GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
		}
		for (int j = 0; j < this.m_BlockLayers.Length; j++)
		{
			if (this.m_BlockLayers[j] != null)
			{
				this.m_BlockLayers[j].OnLoad(_world, this.X * 16, j * 4, this.Z * 16);
			}
		}
		for (int k = 0; k < this.tileEntities.list.Count; k++)
		{
			this.tileEntities.list[k].OnLoad();
		}
	}

	// Token: 0x060054B3 RID: 21683 RVA: 0x00205BF8 File Offset: 0x00203DF8
	public void OnUnload(WorldBase _world)
	{
		this.ProfilerBegin("Chunk OnUnload");
		this.InProgressUnloading = true;
		if (this.biomeParticles != null)
		{
			this.ProfilerBegin("biome particles");
			for (int i = 0; i < this.biomeParticles.Count; i++)
			{
				global::UnityEngine.Object.Destroy(this.biomeParticles[i]);
			}
			this.biomeParticles = null;
			this.ProfilerEnd();
		}
		this.spawnedBiomeParticles = false;
		if (!_world.IsRemote())
		{
			this.ProfilerBegin("enities");
			if (this.pendingEntityCreateOps.Count > 0)
			{
				EntityAsyncManager.EntityCreateHandle[] array = new EntityAsyncManager.EntityCreateHandle[this.pendingEntityCreateOps.Count];
				this.pendingEntityCreateOps.CopyTo(array);
				EntityAsyncManager.EntityCreateHandle[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].WaitForComplete();
				}
				this.pendingEntityCreateOps.Clear();
			}
			for (int k = 0; k < 16; k++)
			{
				if (this.entityLists[k].Count != 0)
				{
					_world.UnloadEntities(this.entityLists[k], false);
				}
			}
			this.ProfilerEnd();
			this.removeExpiredCustomChunkDataEntries(_world.GetWorldTime());
		}
		this.ProfilerBegin("tile entities");
		for (int l = 0; l < this.tileEntities.list.Count; l++)
		{
			this.tileEntities.list[l].OnUnload(GameManager.Instance.World);
		}
		this.ProfilerEnd();
		this.RemoveBlockEntityTransforms();
		this.ProfilerBegin("block layers");
		for (int m = 0; m < this.m_BlockLayers.Length; m++)
		{
			if (this.m_BlockLayers[m] != null)
			{
				this.m_BlockLayers[m].OnUnload(_world, this.X * 16, m * 4, this.Z * 16);
			}
		}
		this.ProfilerEnd();
		this.ProfilerBegin("water");
		this.waterSimHandle.Reset();
		this.ProfilerEnd();
		this.ProfilerEnd();
	}

	// Token: 0x060054B4 RID: 21684 RVA: 0x00205DE0 File Offset: 0x00203FE0
	public void SpawnEntityAsync(World _world, EntityCreationData _ecd, Action<Entity> _onEntityCreated = null)
	{
		if (this.InProgressUnloading)
		{
			Log.Error(string.Format("Spawning entity onto chunk ({0},{1}) which is unloading", this.X, this.Z));
			return;
		}
		EntityAsyncManager.EntityCreateHandle entityCreateHandle = _world.entityAsyncManager.StartCreateEntity(_ecd, delegate(EntityAsyncManager.EntityCreateHandle handle)
		{
			this.pendingEntityCreateOps.Remove(handle);
			Action<Entity> onEntityCreated = _onEntityCreated;
			if (onEntityCreated != null)
			{
				onEntityCreated(handle.Entity);
			}
			_world.SpawnEntityInWorld(handle.Entity);
		});
		this.pendingEntityCreateOps.Add(entityCreateHandle);
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x00205E63 File Offset: 0x00204063
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnBiomeParticles(Transform _parentForEntityBlocks)
	{
		if (!this.spawnedBiomeParticles)
		{
			this.biomeParticles = BiomeParticleManager.SpawnParticles(this, _parentForEntityBlocks);
			this.spawnedBiomeParticles = true;
		}
	}

	// Token: 0x060054B6 RID: 21686 RVA: 0x00205E84 File Offset: 0x00204084
	public void OnDisplay(World _world, Transform _entityBlocksParentT, ChunkCluster _chunkCluster)
	{
		this.ProfilerBegin("Chunk OnDisplay");
		this.SpawnBiomeParticles(_entityBlocksParentT);
		this.displayState = Chunk.DisplayState.BlockEntities;
		this.blockEntitiesIndex = 0;
		this.blockEntityStubs.list.Sort((BlockEntityData a, BlockEntityData b) => a.pos.y.CompareTo(b.pos.y));
		this.ProfilerEnd();
	}

	// Token: 0x060054B7 RID: 21687 RVA: 0x00205EE8 File Offset: 0x002040E8
	public void OnDisplayBlockEntities(World _world, Transform _entityBlocksParentT, ChunkCluster _chunkCluster)
	{
		this.ProfilerBegin("Chunk OnDisplayBlockEntities");
		Vector3 vector = new Vector3((float)(this.X * 16), 0f, (float)(this.Z * 16));
		int num = ChunkCluster.LayerMappingTable["nocollision"];
		int num2 = ChunkCluster.LayerMappingTable["terraincollision"];
		int num3 = 0;
		int num4 = Utils.FastMax(50, this.blockEntityStubs.list.Count / 3 + 8);
		while (this.blockEntitiesIndex < this.blockEntityStubs.list.Count)
		{
			BlockEntityData blockEntityData = this.blockEntityStubs.list[this.blockEntitiesIndex];
			if (blockEntityData.bHasTransform)
			{
				if (!this.NeedsOnlyCollisionMesh && !blockEntityData.bRenderingOn)
				{
					this.SetBlockEntityRendering(blockEntityData, true);
				}
			}
			else
			{
				if (++num3 > num4)
				{
					this.ProfilerEnd();
					return;
				}
				BlockValue block = _chunkCluster.GetBlock(blockEntityData.pos);
				if (!this.IsInternalBlocksCulled || block.type == blockEntityData.blockValue.type)
				{
					Block block2 = blockEntityData.blockValue.Block;
					BlockShapeModelEntity blockShapeModelEntity = block2.shape as BlockShapeModelEntity;
					if (blockShapeModelEntity == null)
					{
						this.RemoveEntityBlockStub(blockEntityData.pos);
					}
					else
					{
						float num5 = 0f;
						if (block2.IsTerrainDecoration && _world.GetBlock(blockEntityData.pos - Vector3i.up).Block.shape.IsTerrain())
						{
							num5 = _world.GetDecorationOffsetY(blockEntityData.pos);
						}
						Quaternion rotation = blockShapeModelEntity.GetRotation(block);
						Vector3 rotatedOffset = blockShapeModelEntity.GetRotatedOffset(block2, rotation);
						rotatedOffset.x += 0.5f;
						rotatedOffset.z += 0.5f;
						rotatedOffset.y += num5;
						Vector3 vector2 = blockEntityData.pos.ToVector3() + rotatedOffset;
						GameObject objectForType = GameObjectPool.Instance.GetObjectForType(blockShapeModelEntity.modelName, out block2.defaultTintColor);
						if (objectForType)
						{
							this.ProfilerBegin("BE setup");
							Transform transform = objectForType.transform;
							blockEntityData.transform = transform;
							blockEntityData.bHasTransform = true;
							transform.SetParent(_entityBlocksParentT, false);
							transform.localScale = Vector3.one;
							transform.SetLocalPositionAndRotation(vector2 - vector, rotation);
							bool isCollideMovement = block2.IsCollideMovement;
							int num6 = num;
							if (isCollideMovement)
							{
								int layer = objectForType.layer;
								if (layer == 30)
								{
									num6 = ChunkCluster.LayerMappingTable["Glass"];
								}
								else if (layer != 4)
								{
									num6 = num2;
								}
							}
							Utils.SetColliderLayerRecursively(objectForType, num6);
							Vector3i vector3i = Chunk.ToLocalPosition(blockEntityData.pos);
							this.ProfilerBegin("BE TBA");
							block2.OnBlockEntityTransformBeforeActivated(_world, blockEntityData.pos, this.GetBlock(vector3i.x, vector3i.y, vector3i.z), blockEntityData);
							this.ProfilerEnd();
							objectForType.SetActive(true);
							this.ProfilerBegin("BE TAA");
							block2.OnBlockEntityTransformAfterActivated(_world, blockEntityData.pos, this.GetBlock(vector3i.x, vector3i.y, vector3i.z), blockEntityData);
							this.ProfilerEnd();
							if (this.NeedsOnlyCollisionMesh)
							{
								this.SetBlockEntityRendering(blockEntityData, false);
							}
							else
							{
								Chunk.occlusionTs.Add(blockEntityData.transform);
							}
							this.ProfilerEnd();
						}
					}
				}
			}
			this.blockEntitiesIndex++;
		}
		HashSet<int> hashSet = new HashSet<int>(this.propEntities.Keys);
		List<ValueTuple<int, PropValue>> list = new List<ValueTuple<int, PropValue>>();
		foreach (KeyValuePair<int, PropValue> keyValuePair in this.props)
		{
			int num7;
			PropValue propValue;
			keyValuePair.Deconstruct(out num7, out propValue);
			int num8 = num7;
			PropValue propValue2 = propValue;
			BlockValue blockValue = propValue2.blockValue;
			if (blockValue.Block.shape is BlockShapeModelEntity)
			{
				hashSet.Remove(num8);
				list.Add(new ValueTuple<int, PropValue>(num8, propValue2));
			}
		}
		foreach (int num9 in hashSet)
		{
			PropEntityData propEntityData;
			if (this.propEntities.Remove(num9, out propEntityData))
			{
				this.propEntitiesToRemove.Add(propEntityData);
			}
		}
		foreach (ValueTuple<int, PropValue> valueTuple in list)
		{
			int item = valueTuple.Item1;
			PropValue item2 = valueTuple.Item2;
			BlockValue blockValue2 = item2.blockValue;
			Block block3 = blockValue2.Block;
			BlockShapeModelEntity blockShapeModelEntity2 = block3.shape as BlockShapeModelEntity;
			if (blockShapeModelEntity2 != null)
			{
				PropEntityData propEntityData2;
				if (!this.propEntities.TryGetValue(item, out propEntityData2))
				{
					propEntityData2 = (this.propEntities[item] = new PropEntityData());
				}
				PropValue propValue3 = propEntityData2.propValue;
				if (!(item2 == propValue3))
				{
					propEntityData2.propValue = item2;
					if (propEntityData2.bHasTransform)
					{
						Chunk.occlusionTs.Add(propEntityData2.transform);
						if (OcclusionManager.Instance.cullChunkEntities)
						{
							OcclusionManager.Instance.RemoveChunkTransforms(this, Chunk.occlusionTs);
						}
						Chunk.occlusionTs.Clear();
						GameObjectPool.Instance.PoolObject(propEntityData2.transform.gameObject);
						propEntityData2.bHasTransform = false;
						propEntityData2.transform = null;
					}
					float num10 = 0f;
					Vector3 position = item2.position;
					Vector3i vector3i2 = Vector3i.Floor(position);
					if (block3.IsTerrainDecoration && _world.GetBlock(vector3i2 - Vector3i.up).Block.shape.IsTerrain())
					{
						num10 = _world.GetDecorationOffsetY(vector3i2);
					}
					Quaternion rotation2 = blockShapeModelEntity2.GetRotation(blockValue2);
					Vector3 rotatedOffset2 = blockShapeModelEntity2.GetRotatedOffset(block3, rotation2);
					rotatedOffset2.x += 0.5f;
					rotatedOffset2.z += 0.5f;
					rotatedOffset2.y += num10;
					Vector3 vector3 = position + rotatedOffset2;
					GameObject objectForType2 = GameObjectPool.Instance.GetObjectForType(blockShapeModelEntity2.modelName, out block3.defaultTintColor);
					if (objectForType2)
					{
						this.ProfilerBegin("BE setup");
						Transform transform2 = objectForType2.transform;
						propEntityData2.transform = transform2;
						propEntityData2.bHasTransform = true;
						transform2.SetParent(_entityBlocksParentT, false);
						transform2.localScale = Vector3.one;
						transform2.SetLocalPositionAndRotation(vector3, rotation2);
						PropReference propReference;
						if (!objectForType2.TryGetComponent<PropReference>(out propReference))
						{
							propReference = objectForType2.AddComponent<PropReference>();
						}
						propReference.ChunkPos = new Vector2i(this.X, this.Z);
						propReference.PropId = item;
						bool isCollideMovement2 = block3.IsCollideMovement;
						int num11 = num;
						if (isCollideMovement2)
						{
							int layer2 = objectForType2.layer;
							if (layer2 == 30)
							{
								num11 = ChunkCluster.LayerMappingTable["Glass"];
							}
							else if (layer2 != 4)
							{
								num11 = num2;
							}
						}
						Utils.SetColliderLayerRecursively(objectForType2, num11);
						objectForType2.SetActive(true);
						Chunk.occlusionTs.Add(propEntityData2.transform);
						this.ProfilerEnd();
					}
				}
			}
		}
		if (Chunk.occlusionTs.Count > 0)
		{
			if (OcclusionManager.Instance.cullChunkEntities)
			{
				this.ProfilerBegin("BE occlusion");
				OcclusionManager.Instance.AddChunkTransforms(this, Chunk.occlusionTs);
				this.ProfilerEnd();
			}
			Chunk.occlusionTs.Clear();
		}
		this.removeBlockEntitesMarkedForRemoval();
		AstarManager.AddBoundsToUpdate(this.boundingBox);
		this.displayState = Chunk.DisplayState.Done;
		DynamicMeshThread.AddChunkGameObject(this);
		this.ProfilerEnd();
	}

	// Token: 0x060054B8 RID: 21688 RVA: 0x00206698 File Offset: 0x00204898
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3i ToLocalPosition(Vector3i _pos)
	{
		_pos.x &= 15;
		_pos.y &= 255;
		_pos.z &= 15;
		return _pos;
	}

	// Token: 0x060054B9 RID: 21689 RVA: 0x002066C8 File Offset: 0x002048C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeBlockEntitesMarkedForRemoval()
	{
		if (OcclusionManager.Instance.cullChunkEntities)
		{
			for (int i = 0; i < this.blockEntityStubsToRemove.Count; i++)
			{
				BlockEntityData blockEntityData = this.blockEntityStubsToRemove[i];
				if (blockEntityData.bHasTransform)
				{
					Chunk.occlusionTs.Add(blockEntityData.transform);
				}
			}
			foreach (PropEntityData propEntityData in this.propEntitiesToRemove)
			{
				if (propEntityData.bHasTransform)
				{
					Chunk.occlusionTs.Add(propEntityData.transform);
				}
			}
			if (Chunk.occlusionTs.Count > 0)
			{
				OcclusionManager.Instance.RemoveChunkTransforms(this, Chunk.occlusionTs);
				Chunk.occlusionTs.Clear();
			}
		}
		for (int j = 0; j < this.blockEntityStubsToRemove.Count; j++)
		{
			BlockEntityData blockEntityData2 = this.blockEntityStubsToRemove[j];
			blockEntityData2.Cleanup();
			if (blockEntityData2.bHasTransform)
			{
				this.poolBlockEntityTransform(blockEntityData2);
			}
		}
		this.blockEntityStubsToRemove.Clear();
		foreach (PropEntityData propEntityData2 in this.propEntitiesToRemove)
		{
			if (propEntityData2.bHasTransform)
			{
				if (propEntityData2.transform)
				{
					PropReference propReference;
					if (propEntityData2.transform.TryGetComponent<PropReference>(out propReference))
					{
						global::UnityEngine.Object.Destroy(propReference);
					}
					GameObjectPool.Instance.PoolObject(propEntityData2.transform.gameObject);
				}
				propEntityData2.bHasTransform = false;
				propEntityData2.transform = null;
			}
		}
		this.propEntitiesToRemove.Clear();
	}

	// Token: 0x060054BA RID: 21690 RVA: 0x00206884 File Offset: 0x00204A84
	public void OnHide()
	{
		this.RemoveBlockEntityTransforms();
		AstarManager.AddBoundsToUpdate(this.boundingBox);
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x00206898 File Offset: 0x00204A98
	public void RemoveBlockEntityTransforms()
	{
		this.ProfilerBegin("RemoveBlockEntityTransforms");
		if (OcclusionManager.Instance.cullChunkEntities)
		{
			this.ProfilerBegin("OcclusionManager RemoveChunk");
			OcclusionManager.Instance.RemoveChunk(this);
			this.ProfilerEnd();
		}
		for (int i = 0; i < this.blockEntityStubs.list.Count; i++)
		{
			BlockEntityData blockEntityData = this.blockEntityStubs.list[i];
			if (blockEntityData.bHasTransform)
			{
				this.poolBlockEntityTransform(blockEntityData);
			}
		}
		this.ProfilerEnd();
	}

	// Token: 0x060054BC RID: 21692 RVA: 0x0020691C File Offset: 0x00204B1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void poolBlockEntityTransform(BlockEntityData _bed)
	{
		if (!_bed.bRenderingOn)
		{
			this.SetBlockEntityRendering(_bed, true);
		}
		if (_bed.transform)
		{
			GameObjectPool.Instance.PoolObject(_bed.transform.gameObject);
		}
		else
		{
			Log.Error("BlockEntity {0} at pos {1} null transform!", new object[]
			{
				_bed.ToString(),
				_bed.pos
			});
		}
		_bed.bHasTransform = false;
		_bed.transform = null;
	}

	// Token: 0x060054BD RID: 21693 RVA: 0x00206994 File Offset: 0x00204B94
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetBlockEntityRendering(BlockEntityData _bed, bool _bOn)
	{
		_bed.bRenderingOn = _bOn;
		if (!_bed.transform)
		{
			Log.Error(string.Format("2: {0} on pos {1} with empty transform/gameobject!", _bed.ToString(), _bed.pos));
			return;
		}
		this.ProfilerBegin("SetBlockEntityRendering set enable");
		_bed.transform.GetComponentsInChildren<MeshRenderer>(Chunk.tempMeshRenderers);
		for (int i = 0; i < Chunk.tempMeshRenderers.Count; i++)
		{
			Chunk.tempMeshRenderers[i].enabled = _bOn;
		}
		Chunk.tempMeshRenderers.Clear();
		this.ProfilerEnd();
		this.ProfilerBegin("SetBlockEntityRendering BroadcastMessage");
		if (_bOn)
		{
			_bed.transform.BroadcastMessage("SetRenderingOn", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			_bed.transform.BroadcastMessage("SetRenderingOff", SendMessageOptions.DontRequireReceiver);
		}
		this.ProfilerEnd();
	}

	// Token: 0x060054BE RID: 21694 RVA: 0x00206A60 File Offset: 0x00204C60
	public static void ToTerrain(Chunk _chunk, Chunk _terrainChunk)
	{
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				byte height = _chunk.GetHeight(i, j);
				for (int k = 0; k <= (int)height; k++)
				{
					if (!_chunk.GetBlock(i, k, j).isair)
					{
						_terrainChunk.SetBlockRaw(i, k, j, Constants.cTerrainBlockValue);
					}
				}
				for (int l = 0; l < 256; l++)
				{
					_terrainChunk.SetDensity(i, l, j, _chunk.GetDensity(i, l, j));
				}
				_terrainChunk.SetHeight(i, j, height);
				_terrainChunk.SetTerrainHeight(i, j, height);
			}
		}
		_terrainChunk.CopyLightsFrom(_chunk);
		_terrainChunk.isModified = true;
		_terrainChunk.NeedsLightCalculation = false;
	}

	// Token: 0x060054BF RID: 21695 RVA: 0x00206B18 File Offset: 0x00204D18
	public void AddMeshLayer(VoxelMeshLayer _vml)
	{
		for (int i = 0; i < MeshDescription.meshes.Length; i++)
		{
			this.trisInMesh[_vml.idx][i] = _vml.GetTrisInMesh(i);
			this.sizeOfMesh[_vml.idx][i] = _vml.GetSizeOfMesh(i);
		}
		this.totalTris = 0;
		for (int j = 0; j < this.trisInMesh.GetLength(0); j++)
		{
			for (int k = 0; k < MeshDescription.meshes.Length; k++)
			{
				this.totalTris += this.trisInMesh[j][k];
			}
		}
		Queue<int> layerIndexQueue = this.m_layerIndexQueue;
		lock (layerIndexQueue)
		{
			VoxelMeshLayer voxelMeshLayer = this.m_meshLayers[_vml.idx];
			if (voxelMeshLayer == null)
			{
				this.MeshLayerCount++;
				this.m_layerIndexQueue.Enqueue(_vml.idx);
			}
			else
			{
				MemoryPools.poolVML.FreeSync(voxelMeshLayer);
			}
			this.m_meshLayers[_vml.idx] = _vml;
		}
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x00206C28 File Offset: 0x00204E28
	public bool HasMeshLayer()
	{
		Queue<int> layerIndexQueue = this.m_layerIndexQueue;
		bool flag2;
		lock (layerIndexQueue)
		{
			flag2 = this.m_layerIndexQueue.Count > 0;
		}
		return flag2;
	}

	// Token: 0x060054C1 RID: 21697 RVA: 0x00206C74 File Offset: 0x00204E74
	public VoxelMeshLayer GetMeshLayer()
	{
		Queue<int> layerIndexQueue = this.m_layerIndexQueue;
		VoxelMeshLayer voxelMeshLayer2;
		lock (layerIndexQueue)
		{
			if (this.m_layerIndexQueue.Count > 0)
			{
				this.MeshLayerCount--;
				int num = this.m_layerIndexQueue.Dequeue();
				VoxelMeshLayer voxelMeshLayer = this.m_meshLayers[num];
				this.m_meshLayers[num] = null;
				voxelMeshLayer2 = voxelMeshLayer;
			}
			else
			{
				voxelMeshLayer2 = null;
			}
		}
		return voxelMeshLayer2;
	}

	// Token: 0x060054C2 RID: 21698 RVA: 0x00206CF0 File Offset: 0x00204EF0
	public EnumDecoAllowed GetDecoAllowedAt(int x, int z)
	{
		EnumDecoAllowed enumDecoAllowed = EnumDecoAllowed.Everything;
		if (this.m_DecoBiomeArray != null)
		{
			enumDecoAllowed = this.m_DecoBiomeArray[x + z * 16];
		}
		if (enumDecoAllowed.AllowBigDeco())
		{
			EnumDecoOccupied decoOccupiedAt = DecoManager.Instance.GetDecoOccupiedAt(x + this.m_X * 16, z + this.m_Z * 16);
			if (decoOccupiedAt > EnumDecoOccupied.Perimeter && decoOccupiedAt != EnumDecoOccupied.POI)
			{
				enumDecoAllowed = enumDecoAllowed.WithSize(EnumDecoAllowedSize.None);
			}
		}
		return enumDecoAllowed;
	}

	// Token: 0x060054C3 RID: 21699 RVA: 0x00206D4F File Offset: 0x00204F4F
	public EnumDecoAllowedSlope GetDecoAllowedSlopeAt(int x, int z)
	{
		return this.GetDecoAllowedAt(x, z).GetSlope();
	}

	// Token: 0x060054C4 RID: 21700 RVA: 0x00206D5E File Offset: 0x00204F5E
	public EnumDecoAllowedSize GetDecoAllowedSizeAt(int x, int z)
	{
		return this.GetDecoAllowedAt(x, z).GetSize();
	}

	// Token: 0x060054C5 RID: 21701 RVA: 0x00206D6D File Offset: 0x00204F6D
	public bool GetDecoAllowedStreetOnlyAt(int x, int z)
	{
		return this.GetDecoAllowedAt(x, z).GetStreetOnly();
	}

	// Token: 0x060054C6 RID: 21702 RVA: 0x00206D7C File Offset: 0x00204F7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void EnsureDecoBiomeArray()
	{
		if (this.m_DecoBiomeArray == null)
		{
			this.m_DecoBiomeArray = new EnumDecoAllowed[256];
		}
	}

	// Token: 0x060054C7 RID: 21703 RVA: 0x00206D98 File Offset: 0x00204F98
	public void SetDecoAllowedAt(int x, int z, EnumDecoAllowed _newVal)
	{
		this.EnsureDecoBiomeArray();
		int num = x + z * 16;
		EnumDecoAllowed enumDecoAllowed = this.m_DecoBiomeArray[num];
		EnumDecoAllowedSlope slope = enumDecoAllowed.GetSlope();
		if (slope > _newVal.GetSlope())
		{
			_newVal = _newVal.WithSlope(slope);
		}
		EnumDecoAllowedSize size = enumDecoAllowed.GetSize();
		if (size > _newVal.GetSize())
		{
			_newVal = _newVal.WithSize(size);
		}
		if (enumDecoAllowed.GetStreetOnly() && !_newVal.GetStreetOnly())
		{
			_newVal = _newVal.WithStreetOnly(true);
		}
		this.m_DecoBiomeArray[num] = _newVal;
	}

	// Token: 0x060054C8 RID: 21704 RVA: 0x00206E10 File Offset: 0x00205010
	public void SetDecoAllowedSlopeAt(int x, int z, EnumDecoAllowedSlope _newVal)
	{
		this.EnsureDecoBiomeArray();
		int num = x + z * 16;
		this.SetDecoAllowedAt(x, z, this.m_DecoBiomeArray[num].WithSlope(_newVal));
	}

	// Token: 0x060054C9 RID: 21705 RVA: 0x00206E40 File Offset: 0x00205040
	public void SetDecoAllowedSizeAt(int x, int z, EnumDecoAllowedSize _newVal)
	{
		this.EnsureDecoBiomeArray();
		int num = x + z * 16;
		this.SetDecoAllowedAt(x, z, this.m_DecoBiomeArray[num].WithSize(_newVal));
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x00206E70 File Offset: 0x00205070
	public void SetDecoAllowedStreetOnlyAt(int x, int z, bool _newVal)
	{
		this.EnsureDecoBiomeArray();
		int num = x + z * 16;
		this.SetDecoAllowedAt(x, z, this.m_DecoBiomeArray[num].WithStreetOnly(_newVal));
	}

	// Token: 0x060054CB RID: 21707 RVA: 0x00206EA0 File Offset: 0x002050A0
	public Vector3 GetTerrainNormal(int _x, int _z)
	{
		int num = _x + _z * 16;
		Vector3 vector;
		vector.x = (float)((sbyte)this.m_NormalX[num]) / 127f;
		vector.y = (float)((sbyte)this.m_NormalY[num]) / 127f;
		vector.z = (float)((sbyte)this.m_NormalZ[num]) / 127f;
		return vector;
	}

	// Token: 0x060054CC RID: 21708 RVA: 0x00206EFC File Offset: 0x002050FC
	public float GetTerrainNormalY(int _x, int _z)
	{
		int num = _x + _z * 16;
		return (float)((sbyte)this.m_NormalY[num]) / 127f;
	}

	// Token: 0x060054CD RID: 21709 RVA: 0x00206F20 File Offset: 0x00205120
	public void SetTerrainNormal(int x, int z, Vector3 _v)
	{
		int num = x + z * 16;
		this.m_NormalX[num] = (byte)Utils.FastClamp(_v.x * 127f, -128f, 127f);
		this.m_NormalY[num] = (byte)Utils.FastClamp(_v.y * 127f, -128f, 127f);
		this.m_NormalZ[num] = (byte)Utils.FastClamp(_v.z * 127f, -128f, 127f);
	}

	// Token: 0x060054CE RID: 21710 RVA: 0x00206FA0 File Offset: 0x002051A0
	public Vector3i ToWorldPos()
	{
		return new Vector3i(this.m_X * 16, this.m_Y * 256, this.m_Z * 16);
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x00206FC5 File Offset: 0x002051C5
	public Vector3i ToWorldPos(int _x, int _y, int _z)
	{
		return new Vector3i(this.m_X * 16 + _x, this.m_Y * 256 + _y, this.m_Z * 16 + _z);
	}

	// Token: 0x060054D0 RID: 21712 RVA: 0x00206FF0 File Offset: 0x002051F0
	public Vector3i ToWorldPos(Vector3i _pos)
	{
		return new Vector3i(this.m_X * 16, this.m_Y * 256, this.m_Z * 16) + _pos;
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x0020701C File Offset: 0x0020521C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateFullMap()
	{
		if (this.mapColors == null)
		{
			this.mapColors = new ushort[256];
		}
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				int num = i + j * 16;
				int num2 = (int)this.m_HeightMap[num];
				int num3 = num2 >> 2;
				BlockValue blockValue = ((this.m_BlockLayers[num3] != null) ? this.m_BlockLayers[num3].GetAt(i, num2, j) : BlockValue.Air);
				WaterValue waterValue = this.GetWater(i, num2, j);
				while (num2 > 0 && (blockValue.isair || blockValue.Block.IsTerrainDecoration) && !waterValue.HasMass())
				{
					num2--;
					blockValue = ((this.m_BlockLayers[num3] != null) ? this.m_BlockLayers[num3].GetAt(i, num2, j) : BlockValue.Air);
					waterValue = this.GetWater(i, num2, j);
				}
				Color color = BlockLiquidv2.Color;
				if (!waterValue.HasMass())
				{
					float num4 = (float)((sbyte)this.m_NormalX[num]) / 127f;
					float num5 = (float)((sbyte)this.m_NormalY[num]) / 127f;
					float num6 = (float)((sbyte)this.m_NormalZ[num]) / 127f;
					color = blockValue.Block.GetMapColor(blockValue, new Vector3(num4, num5, num6), num2);
				}
				this.mapColors[num] = Utils.ToColor5(color);
			}
		}
		this.bMapDirty = false;
		ModEvents.SCalcChunkColorsDoneData scalcChunkColorsDoneData = new ModEvents.SCalcChunkColorsDoneData(this);
		ModEvents.CalcChunkColorsDone.Invoke(ref scalcChunkColorsDoneData);
	}

	// Token: 0x060054D2 RID: 21714 RVA: 0x0020719C File Offset: 0x0020539C
	public ushort[] GetMapColors()
	{
		if (this.mapColors == null || this.bMapDirty)
		{
			this.updateFullMap();
		}
		return this.mapColors;
	}

	// Token: 0x060054D3 RID: 21715 RVA: 0x002071BA File Offset: 0x002053BA
	public void OnDecorated()
	{
		this.CheckSameDensity();
		this.CheckOnlyTerrain();
	}

	// Token: 0x060054D4 RID: 21716 RVA: 0x002071C8 File Offset: 0x002053C8
	public bool IsAreaMaster()
	{
		return this.m_X % 5 == 0 && this.m_Z % 5 == 0;
	}

	// Token: 0x060054D5 RID: 21717 RVA: 0x002071E4 File Offset: 0x002053E4
	public bool IsAreaMasterCornerChunksLoaded(ChunkCluster _cc)
	{
		return _cc.GetChunkSync(this.m_X - 2, this.m_Z) != null && _cc.GetChunkSync(this.m_X, this.m_Z + 2) != null && _cc.GetChunkSync(this.m_X + 2, this.m_Z + 2) != null && _cc.GetChunkSync(this.m_X - 2, this.m_Z - 2) != null;
	}

	// Token: 0x060054D6 RID: 21718 RVA: 0x00207250 File Offset: 0x00205450
	public static Vector3i ToAreaMasterChunkPos(Vector3i _worldBlockPos)
	{
		return new Vector3i(World.toChunkXZ(_worldBlockPos.x) / 5 * 5, World.toChunkY(_worldBlockPos.y), World.toChunkXZ(_worldBlockPos.z) / 5 * 5);
	}

	// Token: 0x060054D7 RID: 21719 RVA: 0x00207280 File Offset: 0x00205480
	public bool IsAreaMasterDominantBiomeInitialized(ChunkCluster _cc)
	{
		if (this.AreaMasterDominantBiome != 255)
		{
			return true;
		}
		if (_cc == null)
		{
			return false;
		}
		for (int i = 0; i < 50; i++)
		{
			Chunk.biomeCnt[i] = 0;
		}
		for (int j = this.m_X - 2; j < this.m_X + 2; j++)
		{
			for (int k = this.m_Z - 2; k < this.m_Z + 2; k++)
			{
				Chunk chunkSync = _cc.GetChunkSync(j, k);
				if (chunkSync == null)
				{
					return false;
				}
				if (chunkSync.DominantBiome > 0)
				{
					Chunk.biomeCnt[(int)chunkSync.DominantBiome]++;
				}
			}
		}
		int num = 0;
		for (int l = 1; l < Chunk.biomeCnt.Length; l++)
		{
			if (Chunk.biomeCnt[l] > num)
			{
				this.AreaMasterDominantBiome = (byte)l;
				num = Chunk.biomeCnt[l];
			}
		}
		return true;
	}

	// Token: 0x060054D8 RID: 21720 RVA: 0x00207350 File Offset: 0x00205550
	public ChunkAreaBiomeSpawnData GetChunkBiomeSpawnData()
	{
		if (this.AreaMasterDominantBiome == 255)
		{
			return null;
		}
		if (this.biomeSpawnData == null)
		{
			ChunkCustomData chunkCustomData;
			if (!this.ChunkCustomData.dict.TryGetValue("bspd.main", out chunkCustomData) || chunkCustomData == null)
			{
				chunkCustomData = new ChunkCustomData("bspd.main", ulong.MaxValue, false);
				this.ChunkCustomData.Set(chunkCustomData.key, chunkCustomData);
			}
			this.biomeSpawnData = new ChunkAreaBiomeSpawnData(this, this.AreaMasterDominantBiome, chunkCustomData);
		}
		return this.biomeSpawnData;
	}

	// Token: 0x060054D9 RID: 21721 RVA: 0x002073CC File Offset: 0x002055CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeExpiredCustomChunkDataEntries(ulong _worldTime)
	{
		List<string> list = null;
		for (int i = 0; i < this.ChunkCustomData.valueList.Count; i++)
		{
			if (this.ChunkCustomData.valueList[i].expiresInWorldTime <= _worldTime)
			{
				if (list == null)
				{
					list = new List<string>();
				}
				list.Add(this.ChunkCustomData.keyList[i]);
				this.ChunkCustomData.valueList[i].OnRemove(this);
			}
		}
		if (list != null)
		{
			for (int j = 0; j < list.Count; j++)
			{
				this.ChunkCustomData.Remove(list[j]);
			}
		}
	}

	// Token: 0x060054DA RID: 21722 RVA: 0x0020746C File Offset: 0x0020566C
	public bool IsTraderArea(int _x, int _z)
	{
		Vector3i vector3i = this.worldPosIMin;
		vector3i.x += _x;
		vector3i.z += _z;
		return GameManager.Instance.World.IsWithinTraderArea(vector3i);
	}

	// Token: 0x060054DB RID: 21723 RVA: 0x002074A8 File Offset: 0x002056A8
	public override int GetHashCode()
	{
		return 31 * this.m_X + this.m_Z;
	}

	// Token: 0x060054DC RID: 21724 RVA: 0x002074BA File Offset: 0x002056BA
	public void EnterReadLock()
	{
		this.sync.EnterReadLock();
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x002074C7 File Offset: 0x002056C7
	public void EnterWriteLock()
	{
		this.sync.EnterWriteLock();
	}

	// Token: 0x060054DE RID: 21726 RVA: 0x002074D4 File Offset: 0x002056D4
	public void ExitReadLock()
	{
		this.sync.ExitReadLock();
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x002074E1 File Offset: 0x002056E1
	public void ExitWriteLock()
	{
		this.sync.ExitWriteLock();
	}

	// Token: 0x060054E0 RID: 21728 RVA: 0x002074EE File Offset: 0x002056EE
	public override bool Equals(object obj)
	{
		return base.Equals(obj) && obj.GetHashCode() == this.GetHashCode();
	}

	// Token: 0x060054E1 RID: 21729 RVA: 0x00207509 File Offset: 0x00205709
	public override string ToString()
	{
		if (this.cachedToString == null)
		{
			this.cachedToString = string.Format("Chunk_{0},{1}", this.m_X, this.m_Z);
		}
		return this.cachedToString;
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x00207540 File Offset: 0x00205740
	public List<Chunk.DensityMismatchInformation> CheckDensities(bool _logAllMismatches = false)
	{
		Vector3i vector3i = new Vector3i(0, 0, 0);
		Vector3i vector3i2 = new Vector3i(16, 256, 16);
		int num = this.m_X << 4;
		int num2 = this.m_Y << 8;
		int num3 = this.m_Z << 4;
		bool flag = true;
		List<Chunk.DensityMismatchInformation> list = new List<Chunk.DensityMismatchInformation>();
		for (int i = vector3i.x; i < vector3i2.x; i++)
		{
			for (int j = vector3i.z; j < vector3i2.z; j++)
			{
				for (int k = vector3i.y; k < vector3i2.y; k++)
				{
					sbyte density = this.GetDensity(i, k, j);
					BlockValue block = this.GetBlock(i, k, j);
					bool flag2 = block.Block.shape.IsTerrain();
					bool flag3;
					if (flag2)
					{
						flag3 = density < 0;
					}
					else
					{
						flag3 = density >= 0;
					}
					if (!flag3)
					{
						Chunk.DensityMismatchInformation densityMismatchInformation = new Chunk.DensityMismatchInformation(num + i, num2 + k, num3 + j, density, block.type, flag2);
						list.Add(densityMismatchInformation);
						if (flag || _logAllMismatches)
						{
							Log.Warning(densityMismatchInformation.ToString());
							flag = false;
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x060054E3 RID: 21731 RVA: 0x00207680 File Offset: 0x00205880
	public bool RepairDensities()
	{
		Vector3i vector3i = new Vector3i(0, 0, 0);
		Vector3i vector3i2 = new Vector3i(16, 256, 16);
		bool flag = false;
		for (int i = vector3i.x; i < vector3i2.x; i++)
		{
			for (int j = vector3i.z; j < vector3i2.z; j++)
			{
				for (int k = vector3i.y; k < vector3i2.y; k++)
				{
					Block block = this.GetBlock(i, k, j).Block;
					sbyte density = this.GetDensity(i, k, j);
					if (block.shape.IsTerrain())
					{
						if (density >= 0)
						{
							this.SetDensity(i, k, j, -1);
							flag = true;
						}
					}
					else if (density < 0)
					{
						this.SetDensity(i, k, j, 1);
						flag = true;
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x00207750 File Offset: 0x00205950
	public void LoopOverAllBlocks(ChunkBlockLayer.LoopBlocksDelegate _delegate, bool _bIncludeChilds = false, bool _bIncludeAirBlocks = false)
	{
		for (int i = 0; i < this.m_BlockLayers.Length; i++)
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[i];
			if (chunkBlockLayer != null)
			{
				chunkBlockLayer.LoopOverAllBlocks(this, i << 2, _delegate, _bIncludeChilds, _bIncludeAirBlocks);
			}
		}
	}

	// Token: 0x060054E5 RID: 21733 RVA: 0x00207789 File Offset: 0x00205989
	public IEnumerator LoopOverAllBlocksCoroutine(ChunkBlockLayer.LoopBlocksDelegate _delegate, bool _bIncludeChilds = false, bool _bIncludeAirBlocks = false)
	{
		int num;
		for (int i = 0; i < this.m_BlockLayers.Length; i = num + 1)
		{
			ChunkBlockLayer chunkBlockLayer = this.m_BlockLayers[i];
			if (chunkBlockLayer != null)
			{
				chunkBlockLayer.LoopOverAllBlocks(this, i << 2, _delegate, _bIncludeChilds, _bIncludeAirBlocks);
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060054E6 RID: 21734 RVA: 0x002077B0 File Offset: 0x002059B0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInside(int _x, int _y, int _z)
	{
		Vector3b vector3b = new Vector3b(_x, _y, _z);
		return this.insideDevicesHashSet.Contains(vector3b.GetHashCode());
	}

	// Token: 0x060054E7 RID: 21735 RVA: 0x002077E0 File Offset: 0x002059E0
	public BlockFaceFlag RestoreCulledBlocks(World _world)
	{
		BlockFaceFlag blockFaceFlag = BlockFaceFlag.None;
		for (int i = this.insideDevices.Count - 1; i >= 0; i--)
		{
			Vector3b vector3b = this.insideDevices[i];
			if (vector3b.x == 0)
			{
				blockFaceFlag |= BlockFaceFlag.West;
			}
			else if (vector3b.x == 15)
			{
				blockFaceFlag |= BlockFaceFlag.East;
			}
			if (vector3b.z == 0)
			{
				blockFaceFlag |= BlockFaceFlag.North;
			}
			else if (vector3b.z == 15)
			{
				blockFaceFlag |= BlockFaceFlag.South;
			}
		}
		this.IsInternalBlocksCulled = false;
		return blockFaceFlag;
	}

	// Token: 0x060054E8 RID: 21736 RVA: 0x00207858 File Offset: 0x00205A58
	public bool HasFallingBlocks()
	{
		foreach (List<Entity> list in this.entityLists)
		{
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j] is EntityFallingBlock || list[j] is EntityFallingBlocks)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_CHUNK_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerBegin(string _name)
	{
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_CHUNK_PROFILE")]
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ProfilerEnd()
	{
	}

	// Token: 0x060054EB RID: 21739 RVA: 0x002078AE File Offset: 0x00205AAE
	[Conditional("DEBUG_CHUNK_RWCHECK")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void RWCheck(PooledBinaryReader stream)
	{
		if (stream.ReadInt32() != 1431655765)
		{
			Log.Error("Chunk !RWCheck");
		}
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x002078C7 File Offset: 0x00205AC7
	[Conditional("DEBUG_CHUNK_RWCHECK")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void RWCheck(PooledBinaryWriter stream)
	{
		stream.Write(1431655765);
	}

	// Token: 0x060054ED RID: 21741 RVA: 0x002078D4 File Offset: 0x00205AD4
	[Conditional("DEBUG_CHUNK_TRIGGERLOG")]
	public void LogTrigger(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Chunk {1} trigger {2}", GameManager.frameCount, this.ChunkPos, _format);
		Log.Warning(_format, _args);
	}

	// Token: 0x060054EE RID: 21742 RVA: 0x00207900 File Offset: 0x00205B00
	[Conditional("DEBUG_CHUNK_CHUNK")]
	public static void LogChunk(long _key, string _format = "", params object[] _args)
	{
		int num = WorldChunkCache.extractX(_key);
		int num2 = WorldChunkCache.extractZ(_key);
		if (num == 136 && num2 == 25)
		{
			_format = string.Format("{0} Chunk pos {1} {2}, {3}", new object[]
			{
				GameManager.frameCount,
				num,
				num2,
				_format
			});
			Log.Warning(_format, _args);
		}
	}

	// Token: 0x060054EF RID: 21743 RVA: 0x00207964 File Offset: 0x00205B64
	[Conditional("DEBUG_CHUNK_ENTITY")]
	public void LogEntity(string _format = "", params object[] _args)
	{
		if (this.m_X == 136 && this.m_Z == 25)
		{
			_format = string.Format("{0} Chunk {1} entity {2}", GameManager.frameCount, this.ChunkPos, _format);
			Log.Warning(_format, _args);
		}
	}

	// Token: 0x060054F0 RID: 21744 RVA: 0x002079B4 File Offset: 0x00205BB4
	public void LogChunkState()
	{
		Log.Out(string.Concat(new string[]
		{
			string.Format("[FELLTHROUGHWORLD] Chunk {0} State\n", this.Key),
			string.Format("  Displayed: {0}\n", this.IsDisplayed),
			string.Format("  IsCollisionMeshGenerated: {0}\n", this.IsCollisionMeshGenerated),
			string.Format("  NeedsDecoration: {0}\n", this.NeedsDecoration),
			string.Format("  NeedsLightDecoration: {0}\n", this.NeedsLightDecoration),
			string.Format("  NeedsLightCalculation: {0}\n", this.NeedsLightCalculation),
			string.Format("  NeedsRegeneration: {0} {1}\n", this.NeedsRegeneration, this.FormatRegenerationLayers(this.m_NeedsRegenerationAtY)),
			string.Format("  NeedsCopying: {0} (layers: {1})", this.NeedsCopying, this.m_layerIndexQueue.Count)
		}));
	}

	// Token: 0x060054F1 RID: 21745 RVA: 0x00207ABC File Offset: 0x00205CBC
	[PublicizedFrom(EAccessModifier.Private)]
	public string FormatRegenerationLayers(int mask)
	{
		if (mask == 0)
		{
			return "(none)";
		}
		if (mask == 65535)
		{
			return "(all layers)";
		}
		List<int> list = new List<int>();
		for (int i = 0; i < 16; i++)
		{
			if ((mask & (1 << i)) != 0)
			{
				list.Add(i);
			}
		}
		return "(Y layers: " + string.Join<int>(", ", list) + ")";
	}

	// Token: 0x04004193 RID: 16787
	public const uint CurrentSaveVersion = 47U;

	// Token: 0x04004194 RID: 16788
	public const uint SupportedSaveVersion = 32U;

	// Token: 0x04004195 RID: 16789
	public const int cAreaMasterSizeChunks = 5;

	// Token: 0x04004196 RID: 16790
	public const int cAreaMasterSizeBlocks = 80;

	// Token: 0x04004197 RID: 16791
	public const int cTextureChannelCount = 1;

	// Token: 0x04004198 RID: 16792
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkBlockLayer[] m_BlockLayers;

	// Token: 0x04004199 RID: 16793
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkBlockChannel chnStability;

	// Token: 0x0400419A RID: 16794
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkBlockChannel chnDensity;

	// Token: 0x0400419B RID: 16795
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkBlockChannel chnLight;

	// Token: 0x0400419C RID: 16796
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkBlockChannel chnDamage;

	// Token: 0x0400419D RID: 16797
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkBlockChannel[] chnTextures;

	// Token: 0x0400419E RID: 16798
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkBlockChannel chnWater;

	// Token: 0x0400419F RID: 16799
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_X;

	// Token: 0x040041A0 RID: 16800
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_Y;

	// Token: 0x040041A1 RID: 16801
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_Z;

	// Token: 0x040041A2 RID: 16802
	public Vector3i worldPosIMin;

	// Token: 0x040041A3 RID: 16803
	public Vector3i worldPosIMax;

	// Token: 0x040041A4 RID: 16804
	[PublicizedFrom(EAccessModifier.Private)]
	public const double cEntityListHeight = 16.0;

	// Token: 0x040041A5 RID: 16805
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cEntityListCount = 16;

	// Token: 0x040041A6 RID: 16806
	public List<Entity>[] entityLists = new List<Entity>[16];

	// Token: 0x040041A7 RID: 16807
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryList<Vector3i, TileEntity> tileEntities = new DictionaryList<Vector3i, TileEntity>();

	// Token: 0x040041A8 RID: 16808
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> sleeperVolumes = new List<int>();

	// Token: 0x040041A9 RID: 16809
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> triggerVolumes = new List<int>();

	// Token: 0x040041AA RID: 16810
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> wallVolumes = new List<int>();

	// Token: 0x040041AB RID: 16811
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_HeightMap;

	// Token: 0x040041AC RID: 16812
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_bTopSoilBroken;

	// Token: 0x040041AD RID: 16813
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_Biomes;

	// Token: 0x040041AE RID: 16814
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_BiomeIntensities;

	// Token: 0x040041AF RID: 16815
	public byte DominantBiome;

	// Token: 0x040041B0 RID: 16816
	public byte AreaMasterDominantBiome = byte.MaxValue;

	// Token: 0x040041B1 RID: 16817
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_NormalX;

	// Token: 0x040041B2 RID: 16818
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_NormalY;

	// Token: 0x040041B3 RID: 16819
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_NormalZ;

	// Token: 0x040041B4 RID: 16820
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] m_TerrainHeight;

	// Token: 0x040041B5 RID: 16821
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityCreationData> entityStubs = new List<EntityCreationData>();

	// Token: 0x040041B6 RID: 16822
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<EntityAsyncManager.EntityCreateHandle> pendingEntityCreateOps = new HashSet<EntityAsyncManager.EntityCreateHandle>();

	// Token: 0x040041B7 RID: 16823
	public DictionaryKeyValueList<string, ChunkCustomData> ChunkCustomData = new DictionaryKeyValueList<string, ChunkCustomData>();

	// Token: 0x040041B8 RID: 16824
	public ulong SavedInWorldTicks;

	// Token: 0x040041B9 RID: 16825
	public ulong LastTimeRandomTicked;

	// Token: 0x040041BA RID: 16826
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3b> insideDevices = new List<Vector3b>();

	// Token: 0x040041BB RID: 16827
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> insideDevicesHashSet = new HashSet<int>();

	// Token: 0x040041BC RID: 16828
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryList<Vector3i, BlockTrigger> triggerData = new DictionaryList<Vector3i, BlockTrigger>();

	// Token: 0x040041BD RID: 16829
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedDictionary<int, PropValue> props = new LinkedDictionary<int, PropValue>();

	// Token: 0x040041BE RID: 16830
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, PropEntityData> propEntities = new Dictionary<int, PropEntityData>();

	// Token: 0x040041BF RID: 16831
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PropEntityData> propEntitiesToRemove = new List<PropEntityData>();

	// Token: 0x040041C0 RID: 16832
	[PublicizedFrom(EAccessModifier.Private)]
	public int propIdNext;

	// Token: 0x040041C1 RID: 16833
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryList<ulong, BlockEntityData> blockEntityStubs = new DictionaryList<ulong, BlockEntityData>();

	// Token: 0x040041C2 RID: 16834
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockEntityData> blockEntityStubsToRemove = new List<BlockEntityData>();

	// Token: 0x040041C3 RID: 16835
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkAreaBiomeSpawnData biomeSpawnData;

	// Token: 0x040041C4 RID: 16836
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<int> m_layerIndexQueue = new Queue<int>();

	// Token: 0x040041C5 RID: 16837
	[PublicizedFrom(EAccessModifier.Private)]
	public VoxelMeshLayer[] m_meshLayers = new VoxelMeshLayer[16];

	// Token: 0x040041C6 RID: 16838
	public volatile bool hasEntities;

	// Token: 0x040041C7 RID: 16839
	public bool isModified;

	// Token: 0x040041C8 RID: 16840
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds boundingBox;

	// Token: 0x040041C9 RID: 16841
	public DictionarySave<string, List<Vector3i>> IndexedBlocks = new DictionarySave<string, List<Vector3i>>();

	// Token: 0x040041CA RID: 16842
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int m_NeedsRegenerationAtY;

	// Token: 0x040041CB RID: 16843
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDecoAllowed[] m_DecoBiomeArray;

	// Token: 0x040041CC RID: 16844
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort[] mapColors;

	// Token: 0x040041CD RID: 16845
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bMapDirty;

	// Token: 0x040041CE RID: 16846
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEmpty;

	// Token: 0x040041CF RID: 16847
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bEmptyDirty = true;

	// Token: 0x040041D0 RID: 16848
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryKeyList<Vector3i, int> tickedBlocks = new DictionaryKeyList<Vector3i, int>();

	// Token: 0x040041D1 RID: 16849
	public bool IsInternalBlocksCulled;

	// Token: 0x040041D2 RID: 16850
	[PublicizedFrom(EAccessModifier.Private)]
	public bool stopStabilityCalculation;

	// Token: 0x040041D3 RID: 16851
	public OcclusionManager.OccludeeZone occludeeZone;

	// Token: 0x040041D4 RID: 16852
	public readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();

	// Token: 0x040041D5 RID: 16853
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterSimulationNative.ChunkHandle waterSimHandle;

	// Token: 0x040041D6 RID: 16854
	public static int InstanceCount;

	// Token: 0x040041D7 RID: 16855
	public int TotalMemory;

	// Token: 0x040041D8 RID: 16856
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalTris;

	// Token: 0x040041D9 RID: 16857
	[PublicizedFrom(EAccessModifier.Private)]
	public int[][] trisInMesh = new int[16][];

	// Token: 0x040041DA RID: 16858
	[PublicizedFrom(EAccessModifier.Private)]
	public int[][] sizeOfMesh = new int[16][];

	// Token: 0x040041DB RID: 16859
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterDebugManager.RendererHandle waterDebugHandle;

	// Token: 0x040041DC RID: 16860
	public volatile bool InProgressCopying;

	// Token: 0x040041DD RID: 16861
	public volatile bool InProgressDecorating;

	// Token: 0x040041DE RID: 16862
	public volatile bool InProgressLighting;

	// Token: 0x040041DF RID: 16863
	public volatile bool InProgressRegeneration;

	// Token: 0x040041E0 RID: 16864
	public volatile bool InProgressUnloading;

	// Token: 0x040041E1 RID: 16865
	public volatile bool InProgressSaving;

	// Token: 0x040041E2 RID: 16866
	public volatile bool InProgressNetworking;

	// Token: 0x040041E3 RID: 16867
	public volatile bool InProgressWaterSim;

	// Token: 0x040041E4 RID: 16868
	public volatile bool IsDisplayed;

	// Token: 0x040041E5 RID: 16869
	public volatile bool IsCollisionMeshGenerated;

	// Token: 0x040041E6 RID: 16870
	public volatile bool NeedsOnlyCollisionMesh;

	// Token: 0x040041E7 RID: 16871
	public int NeedsRegenerationDebug;

	// Token: 0x040041E8 RID: 16872
	public volatile bool NeedsDecoration;

	// Token: 0x040041E9 RID: 16873
	public volatile bool NeedsLightDecoration;

	// Token: 0x040041EA RID: 16874
	public volatile bool NeedsLightCalculation;

	// Token: 0x040041EB RID: 16875
	[PublicizedFrom(EAccessModifier.Private)]
	public static BlockValue bvPOIFiller;

	// Token: 0x040041EC RID: 16876
	public static bool IgnorePaintTextures = false;

	// Token: 0x040041ED RID: 16877
	[PublicizedFrom(EAccessModifier.Private)]
	public bool spawnedBiomeParticles;

	// Token: 0x040041EE RID: 16878
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameObject> biomeParticles;

	// Token: 0x040041EF RID: 16879
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Transform> occlusionTs = new List<Transform>(200);

	// Token: 0x040041F0 RID: 16880
	public Chunk.DisplayState displayState;

	// Token: 0x040041F1 RID: 16881
	[PublicizedFrom(EAccessModifier.Private)]
	public int blockEntitiesIndex;

	// Token: 0x040041F2 RID: 16882
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<MeshRenderer> tempMeshRenderers = new List<MeshRenderer>();

	// Token: 0x040041F3 RID: 16883
	public int MeshLayerCount;

	// Token: 0x040041F4 RID: 16884
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] biomeCnt = new int[50];

	// Token: 0x040041F5 RID: 16885
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedToString;

	// Token: 0x040041F6 RID: 16886
	[PublicizedFrom(EAccessModifier.Private)]
	public const int dbChunkX = 136;

	// Token: 0x040041F7 RID: 16887
	[PublicizedFrom(EAccessModifier.Private)]
	public const int dbChunkZ = 25;

	// Token: 0x02000B02 RID: 2818
	public enum LIGHT_TYPE
	{
		// Token: 0x040041F9 RID: 16889
		BLOCK,
		// Token: 0x040041FA RID: 16890
		SUN
	}

	// Token: 0x02000B03 RID: 2819
	public enum DisplayState
	{
		// Token: 0x040041FC RID: 16892
		Start,
		// Token: 0x040041FD RID: 16893
		BlockEntities,
		// Token: 0x040041FE RID: 16894
		Done
	}

	// Token: 0x02000B04 RID: 2820
	public struct DensityMismatchInformation
	{
		// Token: 0x060054F3 RID: 21747 RVA: 0x00207B4B File Offset: 0x00205D4B
		public DensityMismatchInformation(int _x, int _y, int _z, sbyte _density, int _bvType, bool _isTerrain)
		{
			this.x = _x;
			this.y = _y;
			this.z = _z;
			this.density = _density;
			this.bvType = _bvType;
			this.isTerrain = _isTerrain;
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x00207B7C File Offset: 0x00205D7C
		public string ToJsonString()
		{
			return string.Format("{{\"x\":{0}, \"y\":{1}, \"z\":{2}, \"density\":{3}, \"bvtype\":{4}, \"terrain\":{5}}}", new object[]
			{
				this.x,
				this.y,
				this.z,
				this.density,
				this.bvType,
				this.isTerrain.ToString().ToLower()
			});
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x00207BF4 File Offset: 0x00205DF4
		public override string ToString()
		{
			return string.Format("DENSITYMISMATCH;{0};{1};{2};{3};{4};{5}", new object[] { this.x, this.y, this.z, this.density, this.isTerrain, this.bvType });
		}

		// Token: 0x040041FF RID: 16895
		public int x;

		// Token: 0x04004200 RID: 16896
		public int y;

		// Token: 0x04004201 RID: 16897
		public int z;

		// Token: 0x04004202 RID: 16898
		public sbyte density;

		// Token: 0x04004203 RID: 16899
		public int bvType;

		// Token: 0x04004204 RID: 16900
		public bool isTerrain;
	}
}
