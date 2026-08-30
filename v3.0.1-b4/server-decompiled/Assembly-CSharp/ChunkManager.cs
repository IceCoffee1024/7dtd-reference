using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

// Token: 0x02000B12 RID: 2834
public class ChunkManager : IChunkProviderIndicator
{
	// Token: 0x060055AE RID: 21934 RVA: 0x0020C86C File Offset: 0x0020AA6C
	public void Init(World _world)
	{
		this.m_World = _world;
		this.rectanglesAroundPlayers = new List<Vector2i>[15];
		for (int i = 0; i < 15; i++)
		{
			this.rectanglesAroundPlayers[i] = new List<Vector2i>();
			for (int j = -i; j <= i; j++)
			{
				for (int k = -i; k <= i; k++)
				{
					if (j == -i || j == i || k == -i || k == i)
					{
						this.rectanglesAroundPlayers[i].Add(new Vector2i(j, k));
					}
				}
			}
		}
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
		ChunkManager.MaxQueuedMeshLayers = GamePrefs.GetInt(EnumGamePrefs.MaxQueuedMeshLayers);
		this.threadInfoRegenerating = ThreadManager.StartThread("ChunkRegeneration", new ThreadManager.ThreadFunctionDelegate(this.thread_RegeneratingInit), new ThreadManager.ThreadFunctionLoopDelegate(this.thread_Regenerating), null, null, null, true, false);
		this.threadInfoCalc = ThreadManager.StartThread("ChunkCalc", null, new ThreadManager.ThreadFunctionLoopDelegate(this.thread_Calc), null, null, null, true, false);
		this.BakeInit();
	}

	// Token: 0x060055AF RID: 21935 RVA: 0x0020C95C File Offset: 0x0020AB5C
	public void Cleanup()
	{
		this.threadInfoRegenerating.WaitForEnd(30);
		this.threadInfoRegenerating = null;
		this.calcThreadWaitHandle.Set();
		this.threadInfoCalc.WaitForEnd(30);
		this.threadInfoCalc = null;
		this.GroundAlignCleanup();
		this.BakeCleanup();
		for (int i = this.m_ObservedEntities.Count - 1; i >= 0; i--)
		{
			this.RemoveChunkObserver(this.m_ObservedEntities[i]);
		}
		this.FreePools();
		this.m_UsedChunkGameObjects.Clear();
		ChunkManager.chunkGenerationTimestamps.Clear();
	}

	// Token: 0x060055B0 RID: 21936 RVA: 0x0020C9F0 File Offset: 0x0020ABF0
	public void FreePools()
	{
		for (int i = 0; i < this.m_FreeChunkGameObjects.Count; i++)
		{
			ChunkGameObject chunkGameObject = this.m_FreeChunkGameObjects[i];
			chunkGameObject.Cleanup();
			global::UnityEngine.Object.Destroy(chunkGameObject.gameObject);
		}
		this.m_FreeChunkGameObjects.Clear();
	}

	// Token: 0x060055B1 RID: 21937 RVA: 0x0020CA3A File Offset: 0x0020AC3A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.MaxQueuedMeshLayers)
		{
			ChunkManager.MaxQueuedMeshLayers = GamePrefs.GetInt(_pref);
		}
	}

	// Token: 0x060055B2 RID: 21938 RVA: 0x0020CA50 File Offset: 0x0020AC50
	public void OriginChanged(Vector3 _offset)
	{
		for (int i = 0; i < this.m_UsedChunkGameObjects.Count; i++)
		{
			GameObject gameObject = this.m_UsedChunkGameObjects[i].gameObject;
			if (gameObject)
			{
				gameObject.transform.position += _offset;
			}
		}
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x0020CAA4 File Offset: 0x0020ACA4
	public ChunkManager.ChunkObserver AddChunkObserver(Vector3 _initialPosition, bool _bBuildVisualMeshAround, int _viewDim, int _entityIdToSendChunksTo)
	{
		ChunkManager.ChunkObserver chunkObserver = new ChunkManager.ChunkObserver(_initialPosition, _bBuildVisualMeshAround, _viewDim, _entityIdToSendChunksTo);
		this.m_ObservedEntities.Add(chunkObserver);
		this.isInternalForceUpdate = true;
		return chunkObserver;
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x0020CAD0 File Offset: 0x0020ACD0
	public void RemoveChunkObserver(ChunkManager.ChunkObserver _chunkObserver)
	{
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			if (this.m_ObservedEntities[i].id == _chunkObserver.id)
			{
				this.m_ObservedEntities.RemoveAt(i);
				this.isInternalForceUpdate = true;
				return;
			}
		}
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x0020CB20 File Offset: 0x0020AD20
	public void SendChunksToClients()
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		this.sendToClientPackages.Clear();
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			ChunkManager.ChunkObserver chunkObserver = this.m_ObservedEntities[i];
			if (chunkObserver.entityIdToSendChunksTo != -1)
			{
				foreach (long num in chunkObserver.chunksToRemove)
				{
					this.sendToClientPackages.Add(NetPackageManager.GetPackage<NetPackageChunkRemove>().Setup(num));
					chunkObserver.chunksLoaded.Remove(num);
					chunkObserver.chunksToReload.Remove(num);
				}
				chunkObserver.chunksToRemove.Clear();
				if (this.sendToClientPackages.Count > 0)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(this.sendToClientPackages, false, chunkObserver.entityIdToSendChunksTo, -1, -1, null, 192, false);
					this.sendToClientPackages.Clear();
				}
				int j = 0;
				while (j < chunkObserver.chunksToLoad.list.Count)
				{
					long num2 = chunkObserver.chunksToLoad.list[j];
					Chunk chunkSync;
					if (chunkCache != null && (chunkSync = chunkCache.GetChunkSync(num2)) != null && !chunkSync.NeedsLightCalculation)
					{
						this.sendToClientPackages.Add(NetPackageManager.GetPackage<NetPackageChunk>().Setup(chunkSync, false));
						chunkObserver.chunksLoaded.Add(num2);
						chunkObserver.chunksToLoad.Remove(num2);
					}
					else
					{
						j++;
					}
					if (this.sendToClientPackages.Count >= 3)
					{
						break;
					}
				}
				for (int k = chunkObserver.chunksToReload.Count - 1; k >= 0; k--)
				{
					long num3 = chunkObserver.chunksToReload[k];
					if (chunkCache != null)
					{
						Chunk chunkSync2 = chunkCache.GetChunkSync(num3);
						if (chunkSync2 != null && !chunkSync2.NeedsLightCalculation)
						{
							this.sendToClientPackages.Add(NetPackageManager.GetPackage<NetPackageChunk>().Setup(chunkSync2, true));
							chunkObserver.chunksToReload.RemoveAt(k);
						}
					}
				}
				if (chunkObserver.mapDatabase != null)
				{
					NetPackage mapChunkPackagesToSend = chunkObserver.mapDatabase.GetMapChunkPackagesToSend();
					if (mapChunkPackagesToSend != null)
					{
						this.sendToClientPackages.Add(mapChunkPackagesToSend);
					}
				}
				if (this.sendToClientPackages.Count > 0)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(this.sendToClientPackages, false, chunkObserver.entityIdToSendChunksTo, -1, -1, null, 192, false);
					this.sendToClientPackages.Clear();
				}
			}
		}
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x0020CDA4 File Offset: 0x0020AFA4
	public void ResendChunksToClients(HashSetLong _chunks)
	{
		List<EntityPlayerLocal> localPlayers = this.m_World.GetLocalPlayers();
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			ChunkManager.ChunkObserver chunkObserver = this.m_ObservedEntities[i];
			if (!chunkObserver.bBuildVisualMeshAround)
			{
				bool flag = false;
				int num = 0;
				while (!flag && num < localPlayers.Count)
				{
					if (chunkObserver.entityIdToSendChunksTo == localPlayers[num].entityId)
					{
						flag = true;
					}
					num++;
				}
				if (!flag)
				{
					chunkObserver.chunksToReload.AddRange(_chunks);
				}
			}
		}
	}

	// Token: 0x060055B7 RID: 21943 RVA: 0x0020CE28 File Offset: 0x0020B028
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk FindNextChunkToCopy()
	{
		if (this.ChunksToCopyInOneFrame.Count > 0)
		{
			if (this.chunksToCopyInOneFramePass == 0)
			{
				this.chunksToCopyInOneFramePass = 1;
				for (int i = 0; i < this.ChunksToCopyInOneFrame.Count; i++)
				{
					if (!this.ChunksToCopyInOneFrame[i].NeedsCopying)
					{
						this.chunksToCopyInOneFramePass = 0;
						break;
					}
				}
			}
			if (this.chunksToCopyInOneFramePass > 0)
			{
				if (this.chunksToCopyInOneFrameIndex < this.ChunksToCopyInOneFrame.Count)
				{
					List<Chunk> chunksToCopyInOneFrame = this.ChunksToCopyInOneFrame;
					int num = this.chunksToCopyInOneFrameIndex;
					this.chunksToCopyInOneFrameIndex = num + 1;
					Chunk chunk = chunksToCopyInOneFrame[num];
					if (chunk != null)
					{
						chunk.InProgressCopying = true;
					}
					return chunk;
				}
				this.chunksToCopyInOneFramePass = 0;
				this.ChunksToCopyInOneFrame.Clear();
			}
		}
		bool flag = false;
		long num2;
		do
		{
			long[] array = this.chunksToCopyArr;
			lock (array)
			{
				if (this.chunksToCopyIdx >= this.m_ChunksToCopy.Count)
				{
					return null;
				}
				long[] array2 = this.m_ChunksToCopy.Array;
				int num = this.chunksToCopyIdx;
				this.chunksToCopyIdx = num + 1;
				num2 = array2[num];
			}
			for (int j = 0; j < this.ChunksToCopyInOneFrame.Count; j++)
			{
				if (num2 == this.ChunksToCopyInOneFrame[j].Key)
				{
					flag = true;
					break;
				}
			}
		}
		while (flag);
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		Chunk chunkSync = chunkCache.GetChunkSync(num2);
		if (chunkSync == null)
		{
			return null;
		}
		chunkSync.EnterWriteLock();
		if (chunkSync.IsLocked)
		{
			chunkSync.ExitWriteLock();
			return null;
		}
		chunkSync.InProgressCopying = true;
		chunkSync.ExitWriteLock();
		return chunkSync;
	}

	// Token: 0x060055B8 RID: 21944 RVA: 0x0020CFDC File Offset: 0x0020B1DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void freeChunkGameObjects()
	{
		long[] array = this.chunksToFreeArr;
		lock (array)
		{
			ChunkCluster chunkCache = this.m_World.ChunkCache;
			if (chunkCache == null)
			{
				Log.Warning("freeChunkGameObjects: cluster not found");
				this.m_ChunksToFree = default(ArraySegment<long>);
			}
			else
			{
				for (int i = 0; i < this.m_ChunksToFree.Count; i++)
				{
					long num = this.m_ChunksToFree.Array[i];
					Chunk chunkSync = chunkCache.GetChunkSync(num);
					if (chunkSync != null && !chunkSync.hasEntities && chunkSync.IsDisplayed)
					{
						this.FreeChunkGameObject(chunkCache, chunkSync);
						chunkSync.IsCollisionMeshGenerated = false;
						chunkSync.NeedsRegeneration = true;
					}
				}
				this.m_ChunksToFree = default(ArraySegment<long>);
			}
		}
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x0020D0B0 File Offset: 0x0020B2B0
	public void ResetChunksToCopyInOneFrame()
	{
		this.ChunksToCopyInOneFrame.Clear();
		this.chunksToCopyInOneFrameIndex = 0;
		this.chunksToCopyInOneFramePass = 0;
	}

	// Token: 0x060055BA RID: 21946 RVA: 0x0020D0CC File Offset: 0x0020B2CC
	public bool CopyChunksToUnity()
	{
		bool flag;
		do
		{
			flag = this.doCopyChunksToUnity();
		}
		while (this.ChunksToCopyInOneFrame.Count > 0 && this.currentCopiedChunk != null);
		return flag;
	}

	// Token: 0x060055BB RID: 21947 RVA: 0x0020D0F8 File Offset: 0x0020B2F8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool doCopyChunksToUnity()
	{
		if (this.m_ChunksToFree.Count > 0)
		{
			this.freeChunkGameObjects();
		}
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (!this.isContinueCopying && this.currentCopiedChunk != null)
		{
			this.currentCopiedChunkGameObject.EndCopyMeshLayer();
			if (chunkCache == null || chunkCache.GetChunkSync(this.currentCopiedChunk.Key) != this.currentCopiedChunk)
			{
				if (chunkCache.DisplayedChunkGameObjects.ContainsKey(this.currentCopiedChunkGameObject.chunk.Key))
				{
					this.FreeChunkGameObject(chunkCache, this.currentCopiedChunk);
				}
				else
				{
					this.currentCopiedChunk.InProgressCopying = false;
					this.currentCopiedChunk = null;
				}
				return false;
			}
			if (!this.currentCopiedChunk.NeedsCopying)
			{
				if (this.currentCopiedChunk.displayState == Chunk.DisplayState.Start)
				{
					this.currentCopiedChunk.InProgressCopying = false;
					this.currentCopiedChunk.IsCollisionMeshGenerated = true;
					if (chunkCache != null)
					{
						chunkCache.OnChunkDisplayed(this.currentCopiedChunk.Key, true);
						this.currentCopiedChunk.OnDisplay(this.m_World, this.currentCopiedChunkGameObject.blockEntitiesParentT, chunkCache);
					}
				}
				if (this.currentCopiedChunk.displayState == Chunk.DisplayState.BlockEntities && chunkCache != null)
				{
					this.currentCopiedChunk.OnDisplayBlockEntities(this.m_World, this.currentCopiedChunkGameObject.blockEntitiesParentT, chunkCache);
				}
				if (this.currentCopiedChunk.displayState != Chunk.DisplayState.Done)
				{
					return true;
				}
				Action<Chunk> onChunkCopiedToUnity = this.OnChunkCopiedToUnity;
				if (onChunkCopiedToUnity != null)
				{
					onChunkCopiedToUnity(this.currentCopiedChunk);
				}
				this.currentCopiedChunk = null;
				if (this.ChunksToCopyInOneFrame.Count == 0)
				{
					return true;
				}
			}
		}
		if (this.currentCopiedChunk == null)
		{
			this.isContinueCopying = false;
			this.currentCopiedChunk = this.FindNextChunkToCopy();
			if (this.currentCopiedChunk == null)
			{
				return this.chunksToCopyIdx < this.m_ChunksToCopy.Count - 1;
			}
			if (chunkCache == null)
			{
				return false;
			}
			this.currentCopiedChunk.displayState = Chunk.DisplayState.Start;
			long key = this.currentCopiedChunk.Key;
			if (!chunkCache.DisplayedChunkGameObjects.TryGetValue(key, out this.currentCopiedChunkGameObject))
			{
				this.currentCopiedChunkGameObject = this.GetNextFreeChunkGameObject();
				this.currentCopiedChunkGameObject.SetChunk(this.currentCopiedChunk, chunkCache);
				this.currentCopiedChunkGameObject.gameObject.SetActive(true);
				chunkCache.SetDisplayedChunkGameObject(key, this.currentCopiedChunkGameObject);
			}
			else if (this.currentCopiedChunkGameObject.chunk != this.currentCopiedChunk)
			{
				Log.Warning("currentCopiedChunk {0} wrong chunk on obj!", new object[] { this.currentCopiedChunk });
				this.currentCopiedChunkGameObject.SetChunk(this.currentCopiedChunk, chunkCache);
			}
		}
		if (!this.isContinueCopying)
		{
			this.currentCopiedChunkLayer = this.currentCopiedChunkGameObject.StartCopyMeshLayer();
			if (this.currentCopiedChunkLayer < 0)
			{
				return true;
			}
		}
		if (this.chunksToCopyInOneFramePass > 0)
		{
			int num;
			int num2;
			this.currentCopiedChunkGameObject.CreateMeshAll(out num, out num2);
			this.isContinueCopying = false;
		}
		else
		{
			int num;
			int num2;
			int num3;
			int num4;
			this.isContinueCopying = this.currentCopiedChunkGameObject.CreateFromChunkNext(out num3, out num4, out num, out num2);
		}
		return this.isContinueCopying;
	}

	// Token: 0x060055BC RID: 21948 RVA: 0x0020D3C8 File Offset: 0x0020B5C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void task_Lighting(ThreadManager.TaskInfo _taskInfo)
	{
		Chunk chunk = null;
		int i = 0;
		while (i < this.m_World.Players.list.Count * 2)
		{
			this.chunksToLightIdx = 0;
			ChunkCluster chunkCache;
			for (;;)
			{
				long[] array = this.chunksToLightArr;
				long num2;
				lock (array)
				{
					if (this.chunksToLightIdx >= this.m_ChunksToLight.Count)
					{
						chunk = null;
						break;
					}
					long[] array2 = this.m_ChunksToLight.Array;
					int num = this.chunksToLightIdx;
					this.chunksToLightIdx = num + 1;
					num2 = array2[num];
					if (num2 == 9223372036854775807L)
					{
						continue;
					}
					this.chunksToLightArr[this.chunksToLightIdx - 1] = long.MaxValue;
				}
				chunkCache = this.m_World.ChunkCache;
				if (chunkCache != null)
				{
					int num3 = WorldChunkCache.extractX(num2);
					int num4 = WorldChunkCache.extractZ(num2);
					chunk = chunkCache.GetChunkSync(num3, num4);
					if (chunk != null && !chunk.IsLocked)
					{
						if (chunk.NeedsLightDecoration && !chunk.IsLocked)
						{
							chunk.EnterWriteLock();
							if (chunk.IsLocked)
							{
								chunk.ExitWriteLock();
								continue;
							}
							chunk.InProgressDecorating = true;
							chunk.ExitWriteLock();
							ChunkProviderGenerateWorld chunkProviderGenerateWorld = chunkCache.ChunkProvider as ChunkProviderGenerateWorld;
							if (chunkProviderGenerateWorld != null)
							{
								chunkProviderGenerateWorld.UpdateDecorations(chunk);
								if (chunk.InProgressDecorating)
								{
									chunk.InProgressDecorating = false;
									continue;
								}
							}
							if (chunkCache.ChunkProvider is ChunkProviderGenerateFlat)
							{
								WaterSimulationNative.Instance.InitializeChunk(chunk);
							}
							chunk.NeedsLightDecoration = false;
							chunk.NeedsDecoration = false;
							chunk.InProgressDecorating = false;
						}
						if (chunk.NeedsLightCalculation && !chunk.NeedsDecoration && !chunk.IsLocked)
						{
							chunk.EnterWriteLock();
							if (chunk.IsLocked)
							{
								chunk.ExitWriteLock();
							}
							else
							{
								chunk.InProgressLighting = true;
								chunk.ExitWriteLock();
								if (!chunkCache.GetNeighborChunks(chunk, this.neighborsLightingThread))
								{
									chunk.InProgressLighting = false;
								}
								else if (!Chunk.IsNeighbourChunksDecorated(this.neighborsLightingThread))
								{
									chunk.InProgressLighting = false;
								}
								else
								{
									if (this.lockLightingInProgress(this.neighborsLightingThread))
									{
										goto IL_0216;
									}
									chunk.InProgressLighting = false;
								}
							}
						}
					}
				}
			}
			IL_0276:
			if (chunk != null)
			{
				i++;
				continue;
			}
			break;
			IL_0216:
			chunkCache.LightChunk(chunk, this.neighborsLightingThread);
			chunk.InProgressLighting = false;
			chunk.NeedsLightCalculation = false;
			chunk.NeedsRegeneration = true;
			for (int j = 0; j < this.neighborsLightingThread.Length; j++)
			{
				this.neighborsLightingThread[j].InProgressLighting = false;
			}
			Action<Chunk> onChunkInitialized = this.OnChunkInitialized;
			if (onChunkInitialized == null)
			{
				goto IL_0276;
			}
			onChunkInitialized(chunk);
			goto IL_0276;
		}
		Array.Clear(this.neighborsLightingThread, 0, this.neighborsLightingThread.Length);
		this.bLightingDone = true;
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x0020D69C File Offset: 0x0020B89C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lockLightingInProgress(Chunk[] chunks)
	{
		for (int i = 0; i < chunks.Length; i++)
		{
			Chunk chunk = chunks[i];
			chunk.EnterWriteLock();
			if (chunk.IsLocked)
			{
				for (int j = 0; j < i; j++)
				{
					chunks[j].InProgressLighting = false;
				}
				chunk.ExitWriteLock();
				return false;
			}
			chunk.InProgressLighting = true;
			chunk.ExitWriteLock();
		}
		return true;
	}

	// Token: 0x060055BE RID: 21950 RVA: 0x0020D6F8 File Offset: 0x0020B8F8
	[PublicizedFrom(EAccessModifier.Private)]
	public int thread_Calc(ThreadManager.ThreadInfo _threadInfo)
	{
		if (!_threadInfo.TerminationRequested())
		{
			this.calcThreadWaitHandle.WaitOne();
			this.task_CalcChunkPositions(_threadInfo);
			this.task_Lighting(null);
			return 5;
		}
		return -1;
	}

	// Token: 0x060055BF RID: 21951 RVA: 0x0020D720 File Offset: 0x0020B920
	[PublicizedFrom(EAccessModifier.Private)]
	public void task_CalcChunkPositions(ThreadManager.ThreadInfo _threadInfo)
	{
		if (this.isViewingOrCollisionPositionsChanged_threadCalc)
		{
			this.viewingChunkPositionsCopy.Clear();
			this.collisionChunkPositionsCopy.Clear();
			this.lightingChunkPositionsCopy.Clear();
			object obj = this.lockObject;
			lock (obj)
			{
				this.viewingChunkPositionsCopy.AddRange(this.m_ViewingChunkPositions.list);
				this.collisionChunkPositionsCopy.AddRange(this.m_CollisionChunkPositions.list);
				this.lightingChunkPositionsCopy.AddRange(this.m_AllChunkPositions.list);
			}
			this.isViewingOrCollisionPositionsChanged_threadCalc = false;
		}
		this.chunksToCopyTemp.Clear();
		this.chunksToFreeTemp.Clear();
		this.chunksToLightTemp.Clear();
		int num = Utils.FastMax(Utils.FastMax(this.viewingChunkPositionsCopy.Count, this.collisionChunkPositionsCopy.Count), this.lightingChunkPositionsCopy.Count);
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < num; i++)
		{
			if (this.chunksToCopyTemp.Count < 100 && num2 < this.viewingChunkPositionsCopy.Count)
			{
				long num6 = this.viewingChunkPositionsCopy[num2];
				if (this.checkChunkNeedsCopying(num6) != null)
				{
					this.chunksToCopyTemp.Add(num6);
				}
				num2++;
			}
			if (this.chunksToCopyTemp.Count < 100 && num3 < this.collisionChunkPositionsCopy.Count)
			{
				long num7 = this.collisionChunkPositionsCopy[num3];
				if (this.checkChunkNeedsCopying(num7) != null)
				{
					this.chunksToCopyTemp.Add(num7);
				}
				num3++;
			}
			if (this.chunksToLightTemp.Count < 100 && num5 < this.lightingChunkPositionsCopy.Count)
			{
				long num8 = this.lightingChunkPositionsCopy[num5];
				ChunkCluster chunkCache = this.m_World.ChunkCache;
				if (chunkCache != null)
				{
					Chunk chunkSync = chunkCache.GetChunkSync(num8);
					if (chunkSync != null && chunkSync.NeedsLightCalculation && (!chunkSync.NeedsDecoration || chunkSync.NeedsLightDecoration) && !chunkSync.IsLocked && !chunkCache.IsOnBorder(chunkSync))
					{
						this.chunksToLightTemp.Add(num8);
					}
				}
				num5++;
			}
			if (ChunkManager.GenerateCollidersOnlyAroundEntites && num4 < this.collisionChunkPositionsCopy.Count)
			{
				ChunkCluster chunkCache2 = this.m_World.ChunkCache;
				if (chunkCache2 != null)
				{
					long num9 = chunkCache2.ToLocalKey(this.collisionChunkPositionsCopy[num4]);
					Chunk chunkSync2 = chunkCache2.GetChunkSync(num9);
					if (chunkSync2 != null && chunkSync2.IsCollisionMeshGenerated && chunkSync2.NeedsOnlyCollisionMesh)
					{
						if (!chunkSync2.hasEntities && chunkCache2.GetNeighborChunks(chunkSync2, this.neighborsGenerationThread2) && !this.hasAnyChunkEntities(this.neighborsGenerationThread2))
						{
							this.chunksToFreeTemp.Add(chunkSync2.Key);
						}
						Array.Clear(this.neighborsGenerationThread2, 0, this.neighborsGenerationThread2.Length);
					}
				}
				num4++;
			}
		}
		long[] array = this.chunksToCopyArr;
		lock (array)
		{
			this.chunksToCopyIdx = 0;
			this.chunksToCopyTemp.CopyTo(this.chunksToCopyArr);
			this.m_ChunksToCopy = new ArraySegment<long>(this.chunksToCopyArr, 0, this.chunksToCopyTemp.Count);
		}
		array = this.chunksToFreeArr;
		lock (array)
		{
			this.chunksToFreeTemp.CopyTo(this.chunksToFreeArr);
			this.m_ChunksToFree = new ArraySegment<long>(this.chunksToFreeArr, 0, this.chunksToFreeTemp.Count);
		}
		array = this.chunksToLightArr;
		lock (array)
		{
			this.chunksToLightIdx = 0;
			this.chunksToLightTemp.CopyTo(this.chunksToLightArr);
			this.m_ChunksToLight = new ArraySegment<long>(this.chunksToLightArr, 0, this.chunksToLightTemp.Count);
		}
		this.bCalcPositionsDone = true;
	}

	// Token: 0x060055C0 RID: 21952 RVA: 0x0020DB50 File Offset: 0x0020BD50
	[PublicizedFrom(EAccessModifier.Protected)]
	public void thread_RegeneratingInit(ThreadManager.ThreadInfo _threadInfo)
	{
		_threadInfo.threadData = new ChunkManager.ThreadRegeneratingData();
	}

	// Token: 0x060055C1 RID: 21953 RVA: 0x0020DB60 File Offset: 0x0020BD60
	[PublicizedFrom(EAccessModifier.Protected)]
	public int thread_Regenerating(ThreadManager.ThreadInfo _threadInfo)
	{
		if (_threadInfo.TerminationRequested())
		{
			return -1;
		}
		int poolSize = MemoryPools.poolVML.GetPoolSize();
		int instanceCount = VoxelMeshLayer.InstanceCount;
		int num = instanceCount - poolSize;
		if (num > ChunkManager.MaxQueuedMeshLayers)
		{
			int num2 = num - ChunkManager.MaxQueuedMeshLayers;
			DateTime utcNow = DateTime.UtcNow;
			if (ChunkManager.vmlExhaustionStartTime == DateTime.MinValue)
			{
				ChunkManager.vmlExhaustionStartTime = utcNow;
			}
			TimeSpan timeSpan = utcNow - ChunkManager.vmlExhaustionStartTime;
			if (timeSpan.TotalSeconds >= 1.0 && utcNow - ChunkManager.lastVmlExhaustionLog >= ChunkManager.vmlExhaustionLogInterval)
			{
				Log.Warning(string.Format("ChunkManager mesh regeneration thread blocked for {0:F1}s - VML max queued exceeded by {1}, (Instance: {2}, Pooled: {3}, MaxQueued: {4})", new object[]
				{
					timeSpan.TotalSeconds,
					num2,
					instanceCount,
					poolSize,
					ChunkManager.MaxQueuedMeshLayers
				}));
				ChunkManager.lastVmlExhaustionLog = utcNow;
				ChunkManager.vmlExhaustionLogInterval = TimeSpan.FromMilliseconds(Math.Min(ChunkManager.vmlExhaustionLogInterval.TotalMilliseconds * 2.0, ChunkManager.MaxVmlLogInterval.TotalMilliseconds));
			}
			return 20;
		}
		if (ChunkManager.vmlExhaustionStartTime != DateTime.MinValue)
		{
			TimeSpan timeSpan2 = DateTime.UtcNow - ChunkManager.vmlExhaustionStartTime;
			if (timeSpan2.TotalSeconds >= 1.0)
			{
				Log.Out(string.Format("ChunkManager mesh regeneration thread resumed after {0:F1}s blocked", timeSpan2.TotalSeconds));
			}
			ChunkManager.vmlExhaustionStartTime = DateTime.MinValue;
			ChunkManager.vmlExhaustionLogInterval = TimeSpan.FromSeconds(30.0);
		}
		ChunkManager.ThreadRegeneratingData threadRegeneratingData = _threadInfo.threadData as ChunkManager.ThreadRegeneratingData;
		if (this.isViewingOrCollisionPositionsChanged_threadReg)
		{
			this.isViewingOrCollisionPositionsChanged_threadReg = false;
			threadRegeneratingData.viewingChunkPositionsCopy.Clear();
			threadRegeneratingData.collisionChunkPositionsCopy.Clear();
			object obj = this.lockObject;
			lock (obj)
			{
				threadRegeneratingData.viewingChunkPositionsCopy.AddRange(this.m_ViewingChunkPositions.list);
				threadRegeneratingData.collisionChunkPositionsCopy.AddRange(this.m_CollisionChunkPositions.list);
			}
		}
		this.RegenerateNextChunk(threadRegeneratingData.viewingChunkPositionsCopy, false);
		this.RegenerateNextChunk(threadRegeneratingData.collisionChunkPositionsCopy, true);
		return 5;
	}

	// Token: 0x060055C2 RID: 21954 RVA: 0x0020DDA8 File Offset: 0x0020BFA8
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk RegenerateNextChunk(List<long> _chunkPositions, bool _isOnlyColliders)
	{
		for (int i = 0; i < _chunkPositions.Count; i++)
		{
			long num = _chunkPositions[i];
			ChunkCluster chunkCache = this.m_World.ChunkCache;
			if (chunkCache != null)
			{
				Chunk chunkSync = chunkCache.GetChunkSync(num);
				if (chunkSync != null && !chunkSync.IsLocked && (chunkSync.NeedsRegeneration || (chunkSync.NeedsOnlyCollisionMesh && !_isOnlyColliders)) && !chunkSync.NeedsLightCalculation && !chunkSync.NeedsDecoration && !chunkSync.IsLocked)
				{
					chunkSync.EnterWriteLock();
					if (chunkSync.IsLocked)
					{
						chunkSync.ExitWriteLock();
					}
					else
					{
						chunkSync.InProgressRegeneration = true;
						chunkSync.ExitWriteLock();
						if (!chunkCache.GetNeighborChunks(chunkSync, this.regenerateNextChunkNeighbors))
						{
							chunkSync.InProgressRegeneration = false;
						}
						else if (!Chunk.IsNeighbourChunksLit(this.regenerateNextChunkNeighbors))
						{
							chunkSync.InProgressRegeneration = false;
						}
						else if (ChunkManager.GenerateCollidersOnlyAroundEntites && _isOnlyColliders && !chunkSync.hasEntities && !this.hasAnyChunkEntities(this.regenerateNextChunkNeighbors))
						{
							chunkSync.InProgressRegeneration = false;
						}
						else
						{
							if (this.lockGenerateInProgress(this.regenerateNextChunkNeighbors))
							{
								if (chunkSync.NeedsOnlyCollisionMesh && !_isOnlyColliders)
								{
									chunkSync.NeedsRegeneration = true;
								}
								chunkCache.RegenerateChunk(chunkSync, this.regenerateNextChunkNeighbors);
								chunkSync.InProgressRegeneration = false;
								chunkSync.NeedsOnlyCollisionMesh = _isOnlyColliders;
								for (int j = 0; j < this.regenerateNextChunkNeighbors.Length; j++)
								{
									this.regenerateNextChunkNeighbors[j].InProgressRegeneration = false;
								}
								Action<Chunk> onChunkRegenerated = this.OnChunkRegenerated;
								if (onChunkRegenerated != null)
								{
									onChunkRegenerated(chunkSync);
								}
								for (int k = 0; k < this.regenerateNextChunkNeighbors.Length; k++)
								{
									Chunk chunk = this.regenerateNextChunkNeighbors[k];
									if (!chunk.NeedsRegeneration)
									{
										Action<Chunk> onChunkRegenerated2 = this.OnChunkRegenerated;
										if (onChunkRegenerated2 != null)
										{
											onChunkRegenerated2(chunk);
										}
									}
								}
								Array.Clear(this.regenerateNextChunkNeighbors, 0, this.regenerateNextChunkNeighbors.Length);
								return chunkSync;
							}
							chunkSync.InProgressRegeneration = false;
						}
					}
				}
			}
		}
		Array.Clear(this.regenerateNextChunkNeighbors, 0, this.regenerateNextChunkNeighbors.Length);
		return null;
	}

	// Token: 0x060055C3 RID: 21955 RVA: 0x0020DFC0 File Offset: 0x0020C1C0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAnyChunkEntities(Chunk[] chunks)
	{
		return chunks[0].hasEntities || chunks[1].hasEntities || chunks[2].hasEntities || chunks[3].hasEntities || chunks[4].hasEntities || chunks[5].hasEntities || chunks[6].hasEntities || chunks[7].hasEntities;
	}

	// Token: 0x060055C4 RID: 21956 RVA: 0x0020E040 File Offset: 0x0020C240
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lockGenerateInProgress(Chunk[] chunks)
	{
		for (int i = 0; i < chunks.Length; i++)
		{
			Chunk chunk = chunks[i];
			chunk.EnterWriteLock();
			if (chunk.IsLocked)
			{
				for (int j = 0; j < i; j++)
				{
					chunks[j].InProgressRegeneration = false;
				}
				chunk.ExitWriteLock();
				return false;
			}
			chunk.InProgressRegeneration = true;
			chunk.ExitWriteLock();
		}
		return true;
	}

	// Token: 0x060055C5 RID: 21957 RVA: 0x0020E09C File Offset: 0x0020C29C
	public void ReloadAllChunks()
	{
		this.m_ViewingChunkPositions.Clear();
		this.m_AllChunkPositions.Clear();
		this.m_CollisionChunkPositions.Clear();
		this.recalcFreeChunkGameObjects(int.MaxValue, true);
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache != null)
		{
			chunkCache.Clear();
		}
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			this.m_ObservedEntities[i].curChunkPos.y = -1;
		}
	}

	// Token: 0x060055C6 RID: 21958 RVA: 0x0020E119 File Offset: 0x0020C319
	public ArraySegment<long> GetActiveChunkSet()
	{
		return new ArraySegment<long>(this.activeChunkSetArr, 0, this.m_AllChunkPositions.list.Count);
	}

	// Token: 0x060055C7 RID: 21959 RVA: 0x0020E137 File Offset: 0x0020C337
	public bool IsForceUpdate()
	{
		return this.isInternalForceUpdate || this.isChunkClusterChanged;
	}

	// Token: 0x060055C8 RID: 21960 RVA: 0x0020E14C File Offset: 0x0020C34C
	public void DetermineChunksToLoad()
	{
		int num = this.removeChunksToUnload(8);
		bool flag = this.isInternalForceUpdate || this.isChunkClusterChanged;
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			ChunkManager.ChunkObserver chunkObserver = this.m_ObservedEntities[i];
			Vector3i vector3i = new Vector3i(World.toChunkXZ(Utils.Fastfloor(chunkObserver.position.x)), 0, World.toChunkXZ(Utils.Fastfloor(chunkObserver.position.z)));
			bool flag2 = vector3i != chunkObserver.curChunkPos;
			chunkObserver.curChunkPos = vector3i;
			if (this.isChunkClusterChanged || flag2)
			{
				flag = true;
				int x = vector3i.x;
				int z = vector3i.z;
				chunkObserver.chunksAround.Clear();
				for (int j = 0; j < chunkObserver.viewDim + 2; j++)
				{
					List<Vector2i> list = this.rectanglesAroundPlayers[j];
					for (int k = 0; k < list.Count; k++)
					{
						chunkObserver.chunksAround.Add(j, WorldChunkCache.MakeChunkKey(list[k].x + x, list[k].y + z));
					}
				}
				chunkObserver.chunksAround.RecalcHashSetList();
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					chunkObserver.chunksToLoad.Clear();
					for (int l = 0; l < chunkObserver.chunksToLoad.buckets.Count; l++)
					{
						chunkObserver.chunksToLoad.Add(l, chunkObserver.chunksAround.buckets.array[l]);
						chunkObserver.chunksToLoad.buckets.array[l].ExceptWithHashSetLong(chunkObserver.chunksLoaded);
					}
					chunkObserver.chunksToLoad.RecalcHashSetList();
					chunkObserver.chunksToRemove.Clear();
					chunkObserver.chunksToRemove.UnionWithHashSetLong(chunkObserver.chunksLoaded);
					chunkObserver.chunksAround.ExceptTarget(chunkObserver.chunksToRemove);
					foreach (long num2 in chunkObserver.chunksToLoad.list)
					{
						ChunkManager.chunkGenerationTimestamps[num2] = DateTime.UtcNow;
					}
				}
			}
		}
		this.isInternalForceUpdate = false;
		this.isChunkClusterChanged = false;
		if (flag)
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.isViewingOrCollisionPositionsChanged_threadCalc = true;
				this.isViewingOrCollisionPositionsChanged_threadReg = true;
				this.m_ViewingChunkPositions.Clear();
				this.m_AllChunkPositions.Clear();
				for (int m = 0; m <= this.m_AllChunkPositions.buckets.Count; m++)
				{
					for (int n = 0; n < this.m_ObservedEntities.Count; n++)
					{
						ChunkManager.ChunkObserver chunkObserver2 = this.m_ObservedEntities[n];
						if (m < chunkObserver2.viewDim + 2)
						{
							this.m_AllChunkPositions.Add(m, chunkObserver2.chunksAround.buckets.array[m]);
							if (chunkObserver2.bBuildVisualMeshAround)
							{
								this.m_ViewingChunkPositions.buckets.array[m].UnionWithHashSetLong(chunkObserver2.chunksAround.buckets.array[m]);
							}
						}
					}
				}
				this.m_ViewingChunkPositions.RecalcHashSetList();
				this.m_AllChunkPositions.RecalcHashSetList();
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					this.m_AllChunkPositions.list.CopyTo(this.activeChunkSetArr);
					this.m_CollisionChunkPositions.Clear();
					for (int num3 = 0; num3 < this.m_CollisionChunkPositions.buckets.Count; num3++)
					{
						this.m_CollisionChunkPositions.buckets.array[num3].UnionWithHashSetLong(this.m_AllChunkPositions.buckets.array[num3]);
						this.m_CollisionChunkPositions.buckets.array[num3].ExceptWithHashSetLong(this.m_ViewingChunkPositions.buckets.array[num3]);
					}
					this.m_CollisionChunkPositions.RecalcHashSetList();
					ChunkCluster chunkCache = this.m_World.ChunkCache;
					if (chunkCache != null && !chunkCache.IsFixedSize)
					{
						HashSetList<Chunk> hashSetList = this.chunksToUnload;
						lock (hashSetList)
						{
							foreach (long num4 in chunkCache.GetChunkKeysCopySync())
							{
								if (!this.m_AllChunkPositions.Contains(num4) && !DynamicMeshThread.ChunksToProcess.Contains(num4) && !DynamicMeshThread.ChunksToLoad.Contains(num4))
								{
									Chunk chunkSync = chunkCache.GetChunkSync(num4);
									chunkSync.InProgressUnloading = true;
									this.chunksToUnload.Add(chunkSync);
								}
							}
							for (int num5 = 0; num5 < this.chunksToUnload.list.Count; num5++)
							{
								chunkCache.RemoveChunk(this.chunksToUnload.list[num5]);
							}
						}
					}
				}
			}
			if (8 - num > 0)
			{
				this.recalcFreeChunkGameObjects(8 - num, false);
			}
		}
		this.calcThreadWaitHandle.Set();
	}

	// Token: 0x060055C9 RID: 21961 RVA: 0x0020E6D4 File Offset: 0x0020C8D4
	[PublicizedFrom(EAccessModifier.Private)]
	public int removeChunksToUnload(int _maxCGOsToUnload = 2147483647)
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache == null)
		{
			return 0;
		}
		HashSetList<Chunk> hashSetList = this.chunksToUnload;
		int num2;
		lock (hashSetList)
		{
			int num = 0;
			for (int i = this.chunksToUnload.list.Count - 1; i >= 0; i--)
			{
				Chunk chunk = this.chunksToUnload.list[i];
				bool flag2 = false;
				chunk.EnterWriteLock();
				chunk.InProgressUnloading = true;
				if (!chunk.IsLockedExceptUnloading)
				{
					flag2 = true;
				}
				chunk.ExitWriteLock();
				if (flag2)
				{
					this.chunksToUnload.Remove(chunk);
					if (chunk.IsDisplayed)
					{
						this.FreeChunkGameObject(chunkCache, chunk);
						num++;
					}
					chunkCache.UnloadChunk(chunk);
					if (num >= _maxCGOsToUnload)
					{
						break;
					}
				}
			}
			num2 = num;
		}
		return num2;
	}

	// Token: 0x060055CA RID: 21962 RVA: 0x0020E7B8 File Offset: 0x0020C9B8
	public void ProcessChunksPendingUnload(Action<Chunk> action)
	{
		HashSetList<Chunk> hashSetList = this.chunksToUnload;
		lock (hashSetList)
		{
			foreach (Chunk chunk in this.chunksToUnload.list)
			{
				action(chunk);
			}
		}
	}

	// Token: 0x060055CB RID: 21963 RVA: 0x0020E838 File Offset: 0x0020CA38
	public long GetNextChunkToProvide()
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache != null)
		{
			int num = 0;
			object obj = this.lockObject;
			lock (obj)
			{
				this.m_AllChunkPositions.list.CopyTo(this.allChunkPositionsCopy);
				num = this.m_AllChunkPositions.list.Count;
			}
			for (int i = 0; i < num; i++)
			{
				long num2 = this.allChunkPositionsCopy[i];
				if (!chunkCache.ContainsChunkSync(num2))
				{
					return num2;
				}
			}
			IChunkProvider chunkProvider = chunkCache.ChunkProvider;
			if (chunkProvider != null)
			{
				HashSetList<long> requestedChunks = chunkProvider.GetRequestedChunks();
				if (requestedChunks != null)
				{
					List<long> list = requestedChunks.list;
					lock (list)
					{
						int count = requestedChunks.list.Count;
						if (count > 0)
						{
							long num3 = requestedChunks.list[count - 1];
							requestedChunks.Remove(num3);
							return num3;
						}
					}
				}
			}
		}
		return long.MaxValue;
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x0020E95C File Offset: 0x0020CB5C
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk checkChunkNeedsCopying(long key)
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache == null)
		{
			return null;
		}
		Chunk chunkSync = chunkCache.GetChunkSync(key);
		if (chunkSync == null || chunkSync.IsLocked)
		{
			return null;
		}
		bool needsCopying = chunkSync.NeedsCopying;
		DictionarySave<long, ChunkGameObject> displayedChunkGameObjects = chunkCache.DisplayedChunkGameObjects;
		bool flag2;
		lock (displayedChunkGameObjects)
		{
			flag2 = chunkCache.DisplayedChunkGameObjects.ContainsKey(chunkSync.Key);
		}
		bool flag3 = !chunkSync.NeedsDecoration && !chunkSync.NeedsLightCalculation && !chunkSync.NeedsRegeneration;
		chunkSync.EnterWriteLock();
		if ((flag2 && !needsCopying) || (!flag2 && !flag3) || chunkSync.IsLocked)
		{
			chunkSync.ExitWriteLock();
			return null;
		}
		if (chunkSync.HasMeshLayer() || chunkSync.IsEmpty())
		{
			chunkSync.ExitWriteLock();
			return chunkSync;
		}
		chunkSync.NeedsRegeneration = true;
		chunkSync.ExitWriteLock();
		return null;
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x0020EA48 File Offset: 0x0020CC48
	public ICollection GetCurrDisplayedChunkGameObjects()
	{
		return this.tempDisplayedCGOs;
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x0020EA50 File Offset: 0x0020CC50
	public IList<ChunkGameObject> GetDisplayedChunkGameObjects()
	{
		this.tempDisplayedCGOs.Clear();
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache != null)
		{
			this.tempDisplayedCGOs.AddRange(chunkCache.DisplayedChunkGameObjects.Dict.Values);
		}
		return this.tempDisplayedCGOs;
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x0020EA98 File Offset: 0x0020CC98
	public int GetDisplayedChunkGameObjectsCount()
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		int? num;
		if (chunkCache == null)
		{
			num = null;
		}
		else
		{
			DictionarySave<long, ChunkGameObject> displayedChunkGameObjects = chunkCache.DisplayedChunkGameObjects;
			num = ((displayedChunkGameObjects != null) ? new int?(displayedChunkGameObjects.Count) : null);
		}
		int? num2 = num;
		return num2.GetValueOrDefault();
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x0020EAE5 File Offset: 0x0020CCE5
	public List<ChunkGameObject> GetFreeChunkGameObjects()
	{
		return this.m_FreeChunkGameObjects;
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x0020EAED File Offset: 0x0020CCED
	public List<ChunkGameObject> GetUsedChunkGameObjects()
	{
		return this.m_UsedChunkGameObjects;
	}

	// Token: 0x060055D2 RID: 21970 RVA: 0x0020EAF8 File Offset: 0x0020CCF8
	public void RemoveChunk(long _chunkKey)
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache == null)
		{
			Log.Warning("RemoveChunk: cluster not found");
			return;
		}
		Chunk chunkSync = chunkCache.GetChunkSync(_chunkKey);
		if (chunkSync == null)
		{
			Log.Warning("RemoveChunk: chunk not found " + WorldChunkCache.extractX(_chunkKey).ToString() + "/" + WorldChunkCache.extractZ(_chunkKey).ToString());
			return;
		}
		chunkSync.InProgressUnloading = true;
		chunkCache.RemoveChunk(chunkSync);
		HashSetList<Chunk> hashSetList = this.chunksToUnload;
		lock (hashSetList)
		{
			this.chunksToUnload.Add(chunkSync);
		}
	}

	// Token: 0x060055D3 RID: 21971 RVA: 0x0020EBA8 File Offset: 0x0020CDA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void recalcFreeChunkGameObjects(int _maxToUnload = 2147483647, bool _bIgnoreFixedSizeFlag = false)
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache == null || (chunkCache.IsFixedSize && !_bIgnoreFixedSizeFlag))
		{
			return;
		}
		this.cgoToRemove.Clear();
		foreach (KeyValuePair<long, ChunkGameObject> keyValuePair in chunkCache.DisplayedChunkGameObjects.Dict)
		{
			if (!this.m_ViewingChunkPositions.Contains(keyValuePair.Key) && !this.m_CollisionChunkPositions.Contains(keyValuePair.Key))
			{
				this.cgoToRemove.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < this.cgoToRemove.Count; i++)
		{
			this.FreeChunkGameObject(chunkCache, this.cgoToRemove[i]);
			if (_maxToUnload-- <= 0)
			{
				break;
			}
		}
	}

	// Token: 0x060055D4 RID: 21972 RVA: 0x0020EC8C File Offset: 0x0020CE8C
	public void FreeChunkGameObject(ChunkCluster _cc, Chunk _chunk)
	{
		long key = _chunk.Key;
		ChunkGameObject chunkGameObject = _cc.DisplayedChunkGameObjects[key];
		if (chunkGameObject == null)
		{
			return;
		}
		if (chunkGameObject.GetChunk() != _chunk)
		{
			return;
		}
		this.FreeChunkGameObject(_cc, key);
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x0020ECCC File Offset: 0x0020CECC
	public void FreeChunkGameObject(ChunkCluster _cc, long _key)
	{
		DynamicMeshThread.RemoveChunkGameObject(_key);
		ChunkGameObject chunkGameObject = _cc.RemoveDisplayedChunkGameObject(_key);
		chunkGameObject.gameObject.SetActive(false);
		_cc.OnChunkDisplayed(_key, false);
		if (this.currentCopiedChunkGameObject == chunkGameObject && this.currentCopiedChunk != null)
		{
			this.currentCopiedChunk.InProgressCopying = false;
			this.currentCopiedChunk = null;
		}
		chunkGameObject.SetChunk(null, null);
		this.m_UsedChunkGameObjects.Remove(chunkGameObject);
		this.m_FreeChunkGameObjects.Add(chunkGameObject);
	}

	// Token: 0x060055D6 RID: 21974 RVA: 0x0020ED48 File Offset: 0x0020CF48
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkGameObject GetNextFreeChunkGameObject()
	{
		ChunkGameObject chunkGameObject;
		if (this.m_FreeChunkGameObjects.Count == 0)
		{
			chunkGameObject = new GameObject("Chunk (new)").AddComponent<ChunkGameObject>();
		}
		else
		{
			chunkGameObject = this.m_FreeChunkGameObjects[this.m_FreeChunkGameObjects.Count - 1];
			this.m_FreeChunkGameObjects.RemoveAt(this.m_FreeChunkGameObjects.Count - 1);
		}
		this.m_UsedChunkGameObjects.Add(chunkGameObject);
		return chunkGameObject;
	}

	// Token: 0x060055D7 RID: 21975 RVA: 0x0020EDB4 File Offset: 0x0020CFB4
	public void ClearChunksForAllObservers(ChunkCluster _cc)
	{
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			ChunkManager.ChunkObserver chunkObserver = this.m_ObservedEntities[i];
			this.ClearChunksForObserver(chunkObserver, _cc);
		}
	}

	// Token: 0x060055D8 RID: 21976 RVA: 0x0020EDEC File Offset: 0x0020CFEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearChunksForObserver(ChunkManager.ChunkObserver _chunkObserver, ChunkCluster cc)
	{
		for (int i = 0; i < _chunkObserver.chunksAround.buckets.Count; i++)
		{
			_chunkObserver.chunksAround.buckets.array[i].Clear();
			_chunkObserver.chunksToLoad.buckets.array[i].Clear();
		}
		_chunkObserver.chunksAround.RecalcHashSetList();
		_chunkObserver.chunksToLoad.RecalcHashSetList();
		_chunkObserver.chunksToReload.Clear();
		HashSetLong chunkKeysCopySync = cc.GetChunkKeysCopySync();
		_chunkObserver.chunksLoaded.ExceptWithHashSetLong(chunkKeysCopySync);
		_chunkObserver.chunksToRemove.ExceptWithHashSetLong(chunkKeysCopySync);
	}

	// Token: 0x060055D9 RID: 21977 RVA: 0x0020EE84 File Offset: 0x0020D084
	public void DebugOnGUI(float middleX, float middleY, int size)
	{
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			Color color = Color.white;
			if (!this.m_ObservedEntities[i].bBuildVisualMeshAround && this.m_ObservedEntities[i].entityIdToSendChunksTo != -1)
			{
				color = Color.cyan;
			}
			Vector3i vector3i = World.worldToBlockPos(this.m_ObservedEntities[i].position);
			GUIUtils.DrawRect(new Rect(middleX + (float)(World.toChunkXZ(vector3i.x) * size) - (float)(size / 2), middleY - (float)(World.toChunkXZ(vector3i.z) * size) - (float)(size / 2), (float)size, (float)size), color);
		}
	}

	// Token: 0x060055DA RID: 21978 RVA: 0x0020EF30 File Offset: 0x0020D130
	public void RemoveAllChunksOnAllClients()
	{
		HashSetLong chunkKeysCopySync = this.m_World.ChunkCache.GetChunkKeysCopySync();
		List<EntityPlayerLocal> localPlayers = this.m_World.GetLocalPlayers();
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			if (!this.m_ObservedEntities[i].bBuildVisualMeshAround)
			{
				bool flag = false;
				int num = 0;
				while (!flag && num < localPlayers.Count)
				{
					if (this.m_ObservedEntities[i].entityIdToSendChunksTo == localPlayers[num].entityId)
					{
						flag = true;
					}
					num++;
				}
				if (!flag)
				{
					foreach (long num2 in chunkKeysCopySync)
					{
						this.m_ObservedEntities[i].chunksLoaded.Remove(num2);
					}
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageChunkRemoveAll>(), false, this.m_ObservedEntities[i].entityIdToSendChunksTo, -1, -1, null, 192, false);
				}
			}
		}
		this.isChunkClusterChanged = true;
	}

	// Token: 0x060055DB RID: 21979 RVA: 0x0020F05C File Offset: 0x0020D25C
	public void RemoveAllChunks()
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		foreach (long num in chunkCache.GetChunkKeysCopySync())
		{
			Chunk chunkSync = chunkCache.GetChunkSync(num);
			if (chunkSync != null)
			{
				this.RemoveChunk(chunkSync.Key);
			}
		}
		this.removeChunksToUnload(int.MaxValue);
		chunkCache.Clear();
	}

	// Token: 0x060055DC RID: 21980 RVA: 0x0020F0E0 File Offset: 0x0020D2E0
	public void ForceUpdate()
	{
		this.isInternalForceUpdate = true;
	}

	// Token: 0x060055DD RID: 21981 RVA: 0x0020F0E9 File Offset: 0x0020D2E9
	public void AddGroundAlignBlock(BlockEntityData _data)
	{
		this.groundAlignBlockLists[this.groundAlignIndex].Add(_data);
	}

	// Token: 0x060055DE RID: 21982 RVA: 0x0020F100 File Offset: 0x0020D300
	public void GroundAlignFrameUpdate()
	{
		this.groundAlignIndex = (this.groundAlignIndex + 1) & 1;
		List<BlockEntityData> list = this.groundAlignBlockLists[this.groundAlignIndex];
		int count = list.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				BlockEntityData blockEntityData = list[i];
				blockEntityData.blockValue.Block.GroundAlign(blockEntityData);
			}
			list.Clear();
		}
	}

	// Token: 0x060055DF RID: 21983 RVA: 0x0020F161 File Offset: 0x0020D361
	[PublicizedFrom(EAccessModifier.Private)]
	public void GroundAlignCleanup()
	{
		this.groundAlignBlockLists[0].Clear();
		this.groundAlignBlockLists[1].Clear();
	}

	// Token: 0x060055E0 RID: 21984 RVA: 0x0020F180 File Offset: 0x0020D380
	public void BakeInit()
	{
		this.bakeThreadInfo = ThreadManager.StartThread("ChunkMeshBake", null, new ThreadManager.ThreadFunctionLoopDelegate(this.BakeThread), null, null, null, true, false);
		this.bakeCoroutine = this.BakeEndOfFrame();
		GameManager.Instance.StartCoroutine(this.bakeCoroutine);
	}

	// Token: 0x060055E1 RID: 21985 RVA: 0x0020F1CC File Offset: 0x0020D3CC
	public void BakeCleanup()
	{
		this.bakeThreadInfo.RequestTermination();
		this.bakeEvent.Set();
		this.bakeThreadInfo.WaitForEnd(30);
		this.bakeThreadInfo = null;
		this.bakes.Clear();
		GameManager.Instance.StopCoroutine(this.bakeCoroutine);
	}

	// Token: 0x060055E2 RID: 21986 RVA: 0x0020F220 File Offset: 0x0020D420
	public void BakeDestroyCancel(MeshCollider _meshCollider)
	{
		List<ChunkManager.BakeCollider> list = this.bakes;
		lock (list)
		{
			Mesh sharedMesh = _meshCollider.sharedMesh;
			if (sharedMesh)
			{
				_meshCollider.sharedMesh = null;
				global::UnityEngine.Object.Destroy(sharedMesh);
			}
			for (int i = 0; i < this.bakes.Count; i++)
			{
				ChunkManager.BakeCollider bakeCollider = this.bakes[i];
				if (bakeCollider.meshCollider == _meshCollider)
				{
					if (bakeCollider.mesh)
					{
						global::UnityEngine.Object.Destroy(bakeCollider.mesh);
						bakeCollider.mesh = null;
					}
					bakeCollider.isBaked = true;
				}
			}
		}
	}

	// Token: 0x060055E3 RID: 21987 RVA: 0x0020F2D4 File Offset: 0x0020D4D4
	public Mesh BakeCancelAndGetMesh(MeshCollider _meshCollider)
	{
		List<ChunkManager.BakeCollider> list = this.bakes;
		Mesh mesh2;
		lock (list)
		{
			Mesh mesh = _meshCollider.sharedMesh;
			ChunkManager.BakeCollider bakeCollider = null;
			for (int i = this.bakes.Count - 1; i >= 0; i--)
			{
				ChunkManager.BakeCollider bakeCollider2 = this.bakes[i];
				if (bakeCollider2.meshCollider == _meshCollider && bakeCollider2.mesh)
				{
					mesh = bakeCollider2.mesh;
					bakeCollider = bakeCollider2;
					break;
				}
			}
			ChunkManager.BakeCollider bakeCollider3 = this.bakeCurrent;
			if (bakeCollider3 != null && bakeCollider3.mesh && bakeCollider3.mesh == mesh)
			{
				bakeCollider3.isCancelledDestroy = true;
				mesh2 = null;
			}
			else
			{
				if (bakeCollider != null)
				{
					bakeCollider.mesh = null;
					bakeCollider.isBaked = true;
				}
				mesh2 = mesh;
			}
		}
		return mesh2;
	}

	// Token: 0x060055E4 RID: 21988 RVA: 0x0020F3BC File Offset: 0x0020D5BC
	public void BakeAdd(Mesh _mesh, MeshCollider _meshCollider)
	{
		ChunkManager.BakeCollider bakeCollider = new ChunkManager.BakeCollider();
		bakeCollider.mesh = _mesh;
		bakeCollider.id = _mesh.GetInstanceID();
		bakeCollider.meshCollider = _meshCollider;
		List<ChunkManager.BakeCollider> list = this.bakes;
		lock (list)
		{
			this.bakes.Add(bakeCollider);
		}
		this.bakeEvent.Set();
	}

	// Token: 0x060055E5 RID: 21989 RVA: 0x0020F430 File Offset: 0x0020D630
	[PublicizedFrom(EAccessModifier.Private)]
	public int BakeThread(ThreadManager.ThreadInfo _threadInfo)
	{
		this.bakeEvent.WaitOne();
		if (_threadInfo.TerminationRequested())
		{
			return -1;
		}
		for (;;)
		{
			List<ChunkManager.BakeCollider> list = this.bakes;
			ChunkManager.BakeCollider bakeCollider;
			lock (list)
			{
				if (this.bakeIndex >= this.bakes.Count)
				{
					break;
				}
				List<ChunkManager.BakeCollider> list2 = this.bakes;
				int num = this.bakeIndex;
				this.bakeIndex = num + 1;
				bakeCollider = list2[num];
				if (bakeCollider.isBaked)
				{
					continue;
				}
				this.bakeCurrent = bakeCollider;
			}
			Physics.BakeMesh(bakeCollider.id, false);
			bakeCollider.isBaked = true;
			this.bakeCurrent = null;
		}
		return 0;
	}

	// Token: 0x060055E6 RID: 21990 RVA: 0x0020F4E4 File Offset: 0x0020D6E4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator BakeEndOfFrame()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		for (;;)
		{
			yield return wait;
			this.BakeMeshAssign();
		}
		yield break;
	}

	// Token: 0x060055E7 RID: 21991 RVA: 0x0020F4F4 File Offset: 0x0020D6F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BakeMeshAssign()
	{
		List<ChunkManager.BakeCollider> list = this.bakes;
		lock (list)
		{
			this.bakeIndex = 0;
			for (int i = 0; i < this.bakes.Count; i++)
			{
				ChunkManager.BakeCollider bakeCollider = this.bakes[i];
				if (bakeCollider.mesh)
				{
					if (bakeCollider.isCancelledDestroy)
					{
						global::UnityEngine.Object.Destroy(bakeCollider.mesh);
					}
					else
					{
						Mesh sharedMesh = bakeCollider.meshCollider.sharedMesh;
						bakeCollider.meshCollider.sharedMesh = bakeCollider.mesh;
						if (sharedMesh && sharedMesh != bakeCollider.mesh)
						{
							global::UnityEngine.Object.Destroy(sharedMesh);
						}
					}
				}
			}
			this.bakes.Clear();
		}
	}

	// Token: 0x060055E8 RID: 21992 RVA: 0x0020F5C4 File Offset: 0x0020D7C4
	public int SetBlockEntitiesVisible(bool _on, string _name)
	{
		ChunkCluster chunkCache = this.m_World.ChunkCache;
		if (chunkCache == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this.m_AllChunkPositions.list.Count; i++)
		{
			long num2 = this.m_AllChunkPositions.list[i];
			Chunk chunkSync = chunkCache.GetChunkSync(num2);
			if (chunkSync != null)
			{
				num += chunkSync.EnableEntityBlocks(_on, _name);
			}
		}
		return num;
	}

	// Token: 0x060055E9 RID: 21993 RVA: 0x0020F62B File Offset: 0x0020D82B
	public static void LogChunk(string format, params object[] args)
	{
		format = string.Format("{0} {1}", GameManager.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x060055EA RID: 21994 RVA: 0x0020F64C File Offset: 0x0020D84C
	[Conditional("DEBUG_CHUNK")]
	public static void TimerStart(string _name)
	{
		Dictionary<string, MicroStopwatch> dictionary = ChunkManager.debugTimers;
		MicroStopwatch microStopwatch;
		lock (dictionary)
		{
			if (!ChunkManager.debugTimers.TryGetValue(_name, out microStopwatch))
			{
				microStopwatch = new MicroStopwatch(true);
				ChunkManager.debugTimers.Add(_name, microStopwatch);
			}
		}
		microStopwatch.Restart();
	}

	// Token: 0x060055EB RID: 21995 RVA: 0x0020F6B0 File Offset: 0x0020D8B0
	[Conditional("DEBUG_CHUNK")]
	public static void TimerLog(string _name, string _format = "", params object[] _args)
	{
		Dictionary<string, MicroStopwatch> dictionary = ChunkManager.debugTimers;
		MicroStopwatch microStopwatch;
		lock (dictionary)
		{
			ChunkManager.debugTimers.TryGetValue(_name, out microStopwatch);
		}
		if (microStopwatch != null)
		{
			float num = (float)microStopwatch.ElapsedMicroseconds * 0.001f;
			int num2 = GameManager.frameCount;
			if (ThreadManager.IsMainThread())
			{
				num2 = Time.frameCount;
			}
			_format = string.Format("{0} {1}ms {2} {3}", new object[] { num2, num, _name, _format });
			Log.Warning(_format, _args);
			microStopwatch.Restart();
		}
	}

	// Token: 0x060055EC RID: 21996 RVA: 0x0020F758 File Offset: 0x0020D958
	public static double SecondsSinceChunkSelectedForGeneration(long chunkKey)
	{
		DateTime dateTime;
		if (ChunkManager.chunkGenerationTimestamps.TryGetValue(chunkKey, out dateTime))
		{
			return (DateTime.UtcNow - dateTime).TotalSeconds;
		}
		return -1.0;
	}

	// Token: 0x060055ED RID: 21997 RVA: 0x0020F794 File Offset: 0x0020D994
	public void LogCurrentGenerationState()
	{
		int poolSize = MemoryPools.poolVML.GetPoolSize();
		int instanceCount = VoxelMeshLayer.InstanceCount;
		int num = instanceCount - poolSize;
		if (num > ChunkManager.MaxQueuedMeshLayers)
		{
			int num2 = num - ChunkManager.MaxQueuedMeshLayers;
			Log.Warning(string.Format("[FELLTHROUGHWORLD] ChunkManager mesh regeneration thread blocked for {0:F1}s - VML max queued exceeded by {1}, (Instance: {2}, Pooled: {3}, MaxQueued: {4})", new object[]
			{
				(DateTime.UtcNow - ChunkManager.vmlExhaustionStartTime).TotalSeconds,
				num2,
				instanceCount,
				poolSize,
				ChunkManager.MaxQueuedMeshLayers
			}));
		}
		else
		{
			Log.Out(string.Format("[FELLTHROUGHWORLD] ChunkManager mesh regeneration thread running - VML pool (Instance: {0}, Pooled: {1}, Queued: {2})", instanceCount, poolSize, num));
		}
		if (this.threadInfoRegenerating.HasTerminated())
		{
			Log.Error("[FELLTHROUGHWORLD] ChunkManager mesh regeneration thread is terminated.");
		}
		else
		{
			Log.Out("[FELLTHROUGHWORLD] ChunkManager mesh regeneration thread is running.");
		}
		if (this.threadInfoCalc.HasTerminated())
		{
			Log.Error("[FELLTHROUGHWORLD] ChunkManager mesh calculation thread is terminated.");
			return;
		}
		Log.Out("[FELLTHROUGHWORLD] ChunkManager mesh calculation thread is running.");
	}

	// Token: 0x0400420B RID: 16907
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cReloadPosY = -1;

	// Token: 0x0400420C RID: 16908
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxChunksSupported = 100000;

	// Token: 0x0400420D RID: 16909
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxChunksAroundPlayers = 15;

	// Token: 0x0400420E RID: 16910
	public static bool GenerateCollidersOnlyAroundEntites = true;

	// Token: 0x0400420F RID: 16911
	[PublicizedFrom(EAccessModifier.Private)]
	public World m_World;

	// Token: 0x04004210 RID: 16912
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObject = new object();

	// Token: 0x04004211 RID: 16913
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToCopyIdx;

	// Token: 0x04004212 RID: 16914
	[PublicizedFrom(EAccessModifier.Private)]
	public ArraySegment<long> m_ChunksToCopy;

	// Token: 0x04004213 RID: 16915
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] chunksToCopyArr = new long[100000];

	// Token: 0x04004214 RID: 16916
	[PublicizedFrom(EAccessModifier.Private)]
	public ArraySegment<long> m_ChunksToFree;

	// Token: 0x04004215 RID: 16917
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] chunksToFreeArr = new long[100000];

	// Token: 0x04004216 RID: 16918
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToLightIdx;

	// Token: 0x04004217 RID: 16919
	[PublicizedFrom(EAccessModifier.Private)]
	public ArraySegment<long> m_ChunksToLight;

	// Token: 0x04004218 RID: 16920
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] chunksToLightArr = new long[100000];

	// Token: 0x04004219 RID: 16921
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] allChunkPositionsCopy = new long[100000];

	// Token: 0x0400421A RID: 16922
	[PublicizedFrom(EAccessModifier.Private)]
	public BucketHashSetList m_ViewingChunkPositions = new BucketHashSetList(15);

	// Token: 0x0400421B RID: 16923
	[PublicizedFrom(EAccessModifier.Private)]
	public BucketHashSetList m_AllChunkPositions = new BucketHashSetList(15);

	// Token: 0x0400421C RID: 16924
	[PublicizedFrom(EAccessModifier.Private)]
	public BucketHashSetList m_CollisionChunkPositions = new BucketHashSetList(15);

	// Token: 0x0400421D RID: 16925
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] activeChunkSetArr = new long[100000];

	// Token: 0x0400421E RID: 16926
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkGameObject> m_FreeChunkGameObjects = new List<ChunkGameObject>();

	// Token: 0x0400421F RID: 16927
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkGameObject> m_UsedChunkGameObjects = new List<ChunkGameObject>();

	// Token: 0x04004220 RID: 16928
	public List<ChunkManager.ChunkObserver> m_ObservedEntities = new List<ChunkManager.ChunkObserver>();

	// Token: 0x04004221 RID: 16929
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo threadInfoRegenerating;

	// Token: 0x04004222 RID: 16930
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo threadInfoCalc;

	// Token: 0x04004223 RID: 16931
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<Chunk> chunksToUnload = new HashSetList<Chunk>();

	// Token: 0x04004224 RID: 16932
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector2i>[] rectanglesAroundPlayers;

	// Token: 0x04004225 RID: 16933
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NetPackage> sendToClientPackages = new List<NetPackage>();

	// Token: 0x04004226 RID: 16934
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConcurrentDictionary<long, DateTime> chunkGenerationTimestamps = new ConcurrentDictionary<long, DateTime>();

	// Token: 0x04004227 RID: 16935
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime lastVmlExhaustionLog = DateTime.MinValue;

	// Token: 0x04004228 RID: 16936
	[PublicizedFrom(EAccessModifier.Private)]
	public static TimeSpan vmlExhaustionLogInterval = TimeSpan.FromSeconds(30.0);

	// Token: 0x04004229 RID: 16937
	[PublicizedFrom(EAccessModifier.Private)]
	public static TimeSpan MaxVmlLogInterval = TimeSpan.FromMinutes(10.0);

	// Token: 0x0400422A RID: 16938
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime vmlExhaustionStartTime = DateTime.MinValue;

	// Token: 0x0400422B RID: 16939
	[PublicizedFrom(EAccessModifier.Private)]
	public const double MinLogThresholdSeconds = 1.0;

	// Token: 0x0400422C RID: 16940
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk currentCopiedChunk;

	// Token: 0x0400422D RID: 16941
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkGameObject currentCopiedChunkGameObject;

	// Token: 0x0400422E RID: 16942
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isContinueCopying;

	// Token: 0x0400422F RID: 16943
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCopiedChunkLayer;

	// Token: 0x04004230 RID: 16944
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInternalForceUpdate;

	// Token: 0x04004231 RID: 16945
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isChunkClusterChanged;

	// Token: 0x04004232 RID: 16946
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent calcThreadWaitHandle = new AutoResetEvent(false);

	// Token: 0x04004233 RID: 16947
	public static int MaxQueuedMeshLayers = 1000;

	// Token: 0x04004234 RID: 16948
	public Action<Chunk> OnChunkInitialized;

	// Token: 0x04004235 RID: 16949
	public Action<Chunk> OnChunkRegenerated;

	// Token: 0x04004236 RID: 16950
	public Action<Chunk> OnChunkCopiedToUnity;

	// Token: 0x04004237 RID: 16951
	public Action<Chunk> OnChunkStabilityCalculationEnabled;

	// Token: 0x04004238 RID: 16952
	public List<Chunk> ChunksToCopyInOneFrame = new List<Chunk>();

	// Token: 0x04004239 RID: 16953
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToCopyInOneFrameIndex;

	// Token: 0x0400423A RID: 16954
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToCopyInOneFramePass;

	// Token: 0x0400423B RID: 16955
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[] neighborsLightingThread = new Chunk[8];

	// Token: 0x0400423C RID: 16956
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool bLightingDone = true;

	// Token: 0x0400423D RID: 16957
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool bCalcPositionsDone = true;

	// Token: 0x0400423E RID: 16958
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isViewingOrCollisionPositionsChanged_threadCalc;

	// Token: 0x0400423F RID: 16959
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> viewingChunkPositionsCopy = new List<long>();

	// Token: 0x04004240 RID: 16960
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> collisionChunkPositionsCopy = new List<long>();

	// Token: 0x04004241 RID: 16961
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> lightingChunkPositionsCopy = new List<long>();

	// Token: 0x04004242 RID: 16962
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> chunksToCopyTemp = new List<long>();

	// Token: 0x04004243 RID: 16963
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> chunksToFreeTemp = new List<long>();

	// Token: 0x04004244 RID: 16964
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> chunksToLightTemp = new List<long>();

	// Token: 0x04004245 RID: 16965
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[] neighborsGenerationThread2 = new Chunk[8];

	// Token: 0x04004246 RID: 16966
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isViewingOrCollisionPositionsChanged_threadReg = true;

	// Token: 0x04004247 RID: 16967
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[] regenerateNextChunkNeighbors = new Chunk[8];

	// Token: 0x04004248 RID: 16968
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxCGOsToUnloadPerFrame = 8;

	// Token: 0x04004249 RID: 16969
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkGameObject> tempDisplayedCGOs = new List<ChunkGameObject>();

	// Token: 0x0400424A RID: 16970
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> cgoToRemove = new List<long>();

	// Token: 0x0400424B RID: 16971
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockEntityData>[] groundAlignBlockLists = new List<BlockEntityData>[]
	{
		new List<BlockEntityData>(),
		new List<BlockEntityData>()
	};

	// Token: 0x0400424C RID: 16972
	[PublicizedFrom(EAccessModifier.Private)]
	public int groundAlignIndex;

	// Token: 0x0400424D RID: 16973
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo bakeThreadInfo;

	// Token: 0x0400424E RID: 16974
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent bakeEvent = new AutoResetEvent(false);

	// Token: 0x0400424F RID: 16975
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator bakeCoroutine;

	// Token: 0x04004250 RID: 16976
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkManager.BakeCollider> bakes = new List<ChunkManager.BakeCollider>();

	// Token: 0x04004251 RID: 16977
	[PublicizedFrom(EAccessModifier.Private)]
	public int bakeIndex;

	// Token: 0x04004252 RID: 16978
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile ChunkManager.BakeCollider bakeCurrent;

	// Token: 0x04004253 RID: 16979
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, MicroStopwatch> debugTimers = new Dictionary<string, MicroStopwatch>();

	// Token: 0x02000B13 RID: 2835
	public class ChunkObserver
	{
		// Token: 0x060055F0 RID: 22000 RVA: 0x0020FAA8 File Offset: 0x0020DCA8
		public ChunkObserver(Vector3 _initialPosition, bool _bBuildVisualMeshAround, int _viewDim, int _entityIdToSendChunksTo)
		{
			this.id = ++ChunkManager.ChunkObserver.idCnt;
			this.entityIdToSendChunksTo = _entityIdToSendChunksTo;
			this.position = _initialPosition;
			this.curChunkPos = new Vector3i(int.MaxValue, 0, int.MaxValue);
			this.bBuildVisualMeshAround = _bBuildVisualMeshAround;
			this.viewDim = _viewDim;
			this.chunksLoaded = new HashSetLong();
			this.chunksToLoad = new BucketHashSetList(_viewDim + 2);
			this.chunksToReload = new List<long>();
			this.chunksToRemove = new HashSetLong();
			this.chunksAround = new BucketHashSetList(_viewDim + 2);
		}

		// Token: 0x060055F1 RID: 22001 RVA: 0x0020FB3E File Offset: 0x0020DD3E
		public void SetPosition(Vector3 _position)
		{
			this.position = _position;
		}

		// Token: 0x04004254 RID: 16980
		[PublicizedFrom(EAccessModifier.Private)]
		public static int idCnt;

		// Token: 0x04004255 RID: 16981
		public int id;

		// Token: 0x04004256 RID: 16982
		public Vector3 position;

		// Token: 0x04004257 RID: 16983
		public Vector3i curChunkPos;

		// Token: 0x04004258 RID: 16984
		public bool bBuildVisualMeshAround;

		// Token: 0x04004259 RID: 16985
		public int viewDim;

		// Token: 0x0400425A RID: 16986
		public HashSetLong chunksLoaded;

		// Token: 0x0400425B RID: 16987
		public BucketHashSetList chunksToLoad;

		// Token: 0x0400425C RID: 16988
		public List<long> chunksToReload;

		// Token: 0x0400425D RID: 16989
		public HashSetLong chunksToRemove;

		// Token: 0x0400425E RID: 16990
		public BucketHashSetList chunksAround;

		// Token: 0x0400425F RID: 16991
		public int entityIdToSendChunksTo;

		// Token: 0x04004260 RID: 16992
		public IMapChunkDatabase mapDatabase;
	}

	// Token: 0x02000B14 RID: 2836
	[PublicizedFrom(EAccessModifier.Private)]
	public class ThreadRegeneratingData
	{
		// Token: 0x04004261 RID: 16993
		public List<long> viewingChunkPositionsCopy = new List<long>();

		// Token: 0x04004262 RID: 16994
		public List<long> collisionChunkPositionsCopy = new List<long>();
	}

	// Token: 0x02000B15 RID: 2837
	public class BakeCollider
	{
		// Token: 0x04004263 RID: 16995
		public Mesh mesh;

		// Token: 0x04004264 RID: 16996
		public int id;

		// Token: 0x04004265 RID: 16997
		public MeshCollider meshCollider;

		// Token: 0x04004266 RID: 16998
		public bool isBaked;

		// Token: 0x04004267 RID: 16999
		public bool isCancelledDestroy;
	}
}
