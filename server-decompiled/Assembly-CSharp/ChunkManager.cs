using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
public class ChunkManager : IChunkProviderIndicator
{
	// Token: 0x06005674 RID: 22132 RVA: 0x00213D74 File Offset: 0x00211F74
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

	// Token: 0x06005675 RID: 22133 RVA: 0x00213E64 File Offset: 0x00212064
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

	// Token: 0x06005676 RID: 22134 RVA: 0x00213EF8 File Offset: 0x002120F8
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

	// Token: 0x06005677 RID: 22135 RVA: 0x00213F42 File Offset: 0x00212142
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.MaxQueuedMeshLayers)
		{
			ChunkManager.MaxQueuedMeshLayers = GamePrefs.GetInt(_pref);
		}
	}

	// Token: 0x06005678 RID: 22136 RVA: 0x00213F58 File Offset: 0x00212158
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

	// Token: 0x06005679 RID: 22137 RVA: 0x00213FAC File Offset: 0x002121AC
	public ChunkManager.ChunkObserver AddChunkObserver(Vector3 _initialPosition, bool _bBuildVisualMeshAround, int _viewDim, int _entityIdToSendChunksTo)
	{
		ChunkManager.ChunkObserver chunkObserver = new ChunkManager.ChunkObserver(_initialPosition, _bBuildVisualMeshAround, _viewDim, _entityIdToSendChunksTo);
		this.m_ObservedEntities.Add(chunkObserver);
		this.isInternalForceUpdate = true;
		return chunkObserver;
	}

	// Token: 0x0600567A RID: 22138 RVA: 0x00213FD8 File Offset: 0x002121D8
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

	// Token: 0x0600567B RID: 22139 RVA: 0x00214028 File Offset: 0x00212228
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

	// Token: 0x0600567C RID: 22140 RVA: 0x002142AC File Offset: 0x002124AC
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

	// Token: 0x0600567D RID: 22141 RVA: 0x00214330 File Offset: 0x00212530
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

	// Token: 0x0600567E RID: 22142 RVA: 0x002144E4 File Offset: 0x002126E4
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

	// Token: 0x0600567F RID: 22143 RVA: 0x002145B8 File Offset: 0x002127B8
	public void ResetChunksToCopyInOneFrame()
	{
		this.ChunksToCopyInOneFrame.Clear();
		this.chunksToCopyInOneFrameIndex = 0;
		this.chunksToCopyInOneFramePass = 0;
	}

	// Token: 0x06005680 RID: 22144 RVA: 0x002145D4 File Offset: 0x002127D4
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

	// Token: 0x06005681 RID: 22145 RVA: 0x00214600 File Offset: 0x00212800
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

	// Token: 0x06005682 RID: 22146 RVA: 0x002148D0 File Offset: 0x00212AD0
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

	// Token: 0x06005683 RID: 22147 RVA: 0x00214BA4 File Offset: 0x00212DA4
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

	// Token: 0x06005684 RID: 22148 RVA: 0x00214C00 File Offset: 0x00212E00
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

	// Token: 0x06005685 RID: 22149 RVA: 0x00214C28 File Offset: 0x00212E28
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

	// Token: 0x06005686 RID: 22150 RVA: 0x00215058 File Offset: 0x00213258
	[PublicizedFrom(EAccessModifier.Protected)]
	public void thread_RegeneratingInit(ThreadManager.ThreadInfo _threadInfo)
	{
		_threadInfo.threadData = new ChunkManager.ThreadRegeneratingData();
	}

	// Token: 0x06005687 RID: 22151 RVA: 0x00215068 File Offset: 0x00213268
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

	// Token: 0x06005688 RID: 22152 RVA: 0x002152B0 File Offset: 0x002134B0
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

	// Token: 0x06005689 RID: 22153 RVA: 0x002154C8 File Offset: 0x002136C8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAnyChunkEntities(Chunk[] chunks)
	{
		return chunks[0].hasEntities || chunks[1].hasEntities || chunks[2].hasEntities || chunks[3].hasEntities || chunks[4].hasEntities || chunks[5].hasEntities || chunks[6].hasEntities || chunks[7].hasEntities;
	}

	// Token: 0x0600568A RID: 22154 RVA: 0x00215548 File Offset: 0x00213748
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

	// Token: 0x0600568B RID: 22155 RVA: 0x002155A4 File Offset: 0x002137A4
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

	// Token: 0x0600568C RID: 22156 RVA: 0x00215621 File Offset: 0x00213821
	public ArraySegment<long> GetActiveChunkSet()
	{
		return new ArraySegment<long>(this.activeChunkSetArr, 0, this.m_AllChunkPositions.list.Count);
	}

	// Token: 0x0600568D RID: 22157 RVA: 0x0021563F File Offset: 0x0021383F
	public bool IsForceUpdate()
	{
		return this.isInternalForceUpdate || this.isChunkClusterChanged;
	}

	// Token: 0x0600568E RID: 22158 RVA: 0x00215654 File Offset: 0x00213854
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
							HashSetLong chunkKeysCopySync = chunkCache.GetChunkKeysCopySync();
							bool flag5 = false;
							foreach (long num4 in chunkKeysCopySync)
							{
								if (!this.m_AllChunkPositions.Contains(num4))
								{
									Chunk chunkSync = chunkCache.GetChunkSync(num4);
									chunkSync.InProgressUnloading = true;
									this.chunksToUnload.Add(chunkSync);
									flag5 = true;
								}
							}
							if (flag5)
							{
								this.UpdateChunkKeysToUnloadSnapshot();
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

	// Token: 0x0600568F RID: 22159 RVA: 0x00215BD0 File Offset: 0x00213DD0
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
			bool flag2 = false;
			for (int i = this.chunksToUnload.list.Count - 1; i >= 0; i--)
			{
				Chunk chunk = this.chunksToUnload.list[i];
				bool flag3 = false;
				chunk.EnterWriteLock();
				chunk.InProgressUnloading = true;
				if (!chunk.IsLockedExceptUnloading)
				{
					flag3 = true;
				}
				chunk.ExitWriteLock();
				if (flag3)
				{
					this.chunksToUnload.Remove(chunk);
					flag2 = true;
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
			if (flag2)
			{
				this.UpdateChunkKeysToUnloadSnapshot();
			}
			num2 = num;
		}
		return num2;
	}

	// Token: 0x06005690 RID: 22160 RVA: 0x00215CC4 File Offset: 0x00213EC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateChunkKeysToUnloadSnapshot()
	{
		int count = this.chunksToUnload.list.Count;
		long[] array = ((count == 0) ? Array.Empty<long>() : new long[count]);
		for (int i = 0; i < count; i++)
		{
			array[i] = this.chunksToUnload.list[i].Key;
		}
		this.chunkKeysToUnloadSnapshot = array;
	}

	// Token: 0x06005691 RID: 22161 RVA: 0x00215D21 File Offset: 0x00213F21
	public ReadOnlySpan<long> GetChunkKeysToUnloadSnapshot()
	{
		return this.chunkKeysToUnloadSnapshot;
	}

	// Token: 0x06005692 RID: 22162 RVA: 0x00215D30 File Offset: 0x00213F30
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

	// Token: 0x06005693 RID: 22163 RVA: 0x00215E54 File Offset: 0x00214054
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

	// Token: 0x06005694 RID: 22164 RVA: 0x00215F40 File Offset: 0x00214140
	public ICollection GetCurrDisplayedChunkGameObjects()
	{
		return this.tempDisplayedCGOs;
	}

	// Token: 0x06005695 RID: 22165 RVA: 0x00215F48 File Offset: 0x00214148
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

	// Token: 0x06005696 RID: 22166 RVA: 0x00215F90 File Offset: 0x00214190
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

	// Token: 0x06005697 RID: 22167 RVA: 0x00215FDD File Offset: 0x002141DD
	public List<ChunkGameObject> GetFreeChunkGameObjects()
	{
		return this.m_FreeChunkGameObjects;
	}

	// Token: 0x06005698 RID: 22168 RVA: 0x00215FE5 File Offset: 0x002141E5
	public List<ChunkGameObject> GetUsedChunkGameObjects()
	{
		return this.m_UsedChunkGameObjects;
	}

	// Token: 0x06005699 RID: 22169 RVA: 0x00215FF0 File Offset: 0x002141F0
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
			this.UpdateChunkKeysToUnloadSnapshot();
		}
	}

	// Token: 0x0600569A RID: 22170 RVA: 0x002160A4 File Offset: 0x002142A4
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

	// Token: 0x0600569B RID: 22171 RVA: 0x00216188 File Offset: 0x00214388
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

	// Token: 0x0600569C RID: 22172 RVA: 0x002161C8 File Offset: 0x002143C8
	public void FreeChunkGameObject(ChunkCluster _cc, long _key)
	{
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

	// Token: 0x0600569D RID: 22173 RVA: 0x00216240 File Offset: 0x00214440
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

	// Token: 0x0600569E RID: 22174 RVA: 0x002162AC File Offset: 0x002144AC
	public void ClearChunksForAllObservers(ChunkCluster _cc)
	{
		for (int i = 0; i < this.m_ObservedEntities.Count; i++)
		{
			ChunkManager.ChunkObserver chunkObserver = this.m_ObservedEntities[i];
			this.ClearChunksForObserver(chunkObserver, _cc);
		}
	}

	// Token: 0x0600569F RID: 22175 RVA: 0x002162E4 File Offset: 0x002144E4
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

	// Token: 0x060056A0 RID: 22176 RVA: 0x0021637C File Offset: 0x0021457C
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

	// Token: 0x060056A1 RID: 22177 RVA: 0x00216428 File Offset: 0x00214628
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

	// Token: 0x060056A2 RID: 22178 RVA: 0x00216554 File Offset: 0x00214754
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

	// Token: 0x060056A3 RID: 22179 RVA: 0x002165D8 File Offset: 0x002147D8
	public void ForceUpdate()
	{
		this.isInternalForceUpdate = true;
	}

	// Token: 0x060056A4 RID: 22180 RVA: 0x002165E1 File Offset: 0x002147E1
	public void AddGroundAlignBlock(BlockEntityData _data)
	{
		this.groundAlignBlockLists[this.groundAlignIndex].Add(_data);
	}

	// Token: 0x060056A5 RID: 22181 RVA: 0x002165F8 File Offset: 0x002147F8
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

	// Token: 0x060056A6 RID: 22182 RVA: 0x00216659 File Offset: 0x00214859
	[PublicizedFrom(EAccessModifier.Private)]
	public void GroundAlignCleanup()
	{
		this.groundAlignBlockLists[0].Clear();
		this.groundAlignBlockLists[1].Clear();
	}

	// Token: 0x060056A7 RID: 22183 RVA: 0x00216678 File Offset: 0x00214878
	public void BakeInit()
	{
		this.bakeThreadInfo = ThreadManager.StartThread("ChunkMeshBake", null, new ThreadManager.ThreadFunctionLoopDelegate(this.BakeThread), null, null, null, true, false);
		this.bakeCoroutine = this.BakeEndOfFrame();
		GameManager.Instance.StartCoroutine(this.bakeCoroutine);
	}

	// Token: 0x060056A8 RID: 22184 RVA: 0x002166C4 File Offset: 0x002148C4
	public void BakeCleanup()
	{
		this.bakeThreadInfo.RequestTermination();
		this.bakeEvent.Set();
		this.bakeThreadInfo.WaitForEnd(30);
		this.bakeThreadInfo = null;
		this.bakes.Clear();
		GameManager.Instance.StopCoroutine(this.bakeCoroutine);
	}

	// Token: 0x060056A9 RID: 22185 RVA: 0x00216718 File Offset: 0x00214918
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

	// Token: 0x060056AA RID: 22186 RVA: 0x002167CC File Offset: 0x002149CC
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

	// Token: 0x060056AB RID: 22187 RVA: 0x002168B4 File Offset: 0x00214AB4
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

	// Token: 0x060056AC RID: 22188 RVA: 0x00216928 File Offset: 0x00214B28
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

	// Token: 0x060056AD RID: 22189 RVA: 0x002169DC File Offset: 0x00214BDC
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

	// Token: 0x060056AE RID: 22190 RVA: 0x002169EC File Offset: 0x00214BEC
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

	// Token: 0x060056AF RID: 22191 RVA: 0x00216ABC File Offset: 0x00214CBC
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

	// Token: 0x060056B0 RID: 22192 RVA: 0x00216B23 File Offset: 0x00214D23
	public static void LogChunk(string format, params object[] args)
	{
		format = string.Format("{0} {1}", GameManager.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x060056B1 RID: 22193 RVA: 0x00216B44 File Offset: 0x00214D44
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

	// Token: 0x060056B2 RID: 22194 RVA: 0x00216BA8 File Offset: 0x00214DA8
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

	// Token: 0x060056B3 RID: 22195 RVA: 0x00216C50 File Offset: 0x00214E50
	public static double SecondsSinceChunkSelectedForGeneration(long chunkKey)
	{
		DateTime dateTime;
		if (ChunkManager.chunkGenerationTimestamps.TryGetValue(chunkKey, out dateTime))
		{
			return (DateTime.UtcNow - dateTime).TotalSeconds;
		}
		return -1.0;
	}

	// Token: 0x060056B4 RID: 22196 RVA: 0x00216C8C File Offset: 0x00214E8C
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

	// Token: 0x04004308 RID: 17160
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cReloadPosY = -1;

	// Token: 0x04004309 RID: 17161
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxChunksSupported = 100000;

	// Token: 0x0400430A RID: 17162
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxChunksAroundPlayers = 15;

	// Token: 0x0400430B RID: 17163
	public static bool GenerateCollidersOnlyAroundEntites = true;

	// Token: 0x0400430C RID: 17164
	[PublicizedFrom(EAccessModifier.Private)]
	public World m_World;

	// Token: 0x0400430D RID: 17165
	[PublicizedFrom(EAccessModifier.Private)]
	public object lockObject = new object();

	// Token: 0x0400430E RID: 17166
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToCopyIdx;

	// Token: 0x0400430F RID: 17167
	[PublicizedFrom(EAccessModifier.Private)]
	public ArraySegment<long> m_ChunksToCopy;

	// Token: 0x04004310 RID: 17168
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] chunksToCopyArr = new long[100000];

	// Token: 0x04004311 RID: 17169
	[PublicizedFrom(EAccessModifier.Private)]
	public ArraySegment<long> m_ChunksToFree;

	// Token: 0x04004312 RID: 17170
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] chunksToFreeArr = new long[100000];

	// Token: 0x04004313 RID: 17171
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToLightIdx;

	// Token: 0x04004314 RID: 17172
	[PublicizedFrom(EAccessModifier.Private)]
	public ArraySegment<long> m_ChunksToLight;

	// Token: 0x04004315 RID: 17173
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] chunksToLightArr = new long[100000];

	// Token: 0x04004316 RID: 17174
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] allChunkPositionsCopy = new long[100000];

	// Token: 0x04004317 RID: 17175
	[PublicizedFrom(EAccessModifier.Private)]
	public BucketHashSetList m_ViewingChunkPositions = new BucketHashSetList(15);

	// Token: 0x04004318 RID: 17176
	[PublicizedFrom(EAccessModifier.Private)]
	public BucketHashSetList m_AllChunkPositions = new BucketHashSetList(15);

	// Token: 0x04004319 RID: 17177
	[PublicizedFrom(EAccessModifier.Private)]
	public BucketHashSetList m_CollisionChunkPositions = new BucketHashSetList(15);

	// Token: 0x0400431A RID: 17178
	[PublicizedFrom(EAccessModifier.Private)]
	public long[] activeChunkSetArr = new long[100000];

	// Token: 0x0400431B RID: 17179
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkGameObject> m_FreeChunkGameObjects = new List<ChunkGameObject>();

	// Token: 0x0400431C RID: 17180
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkGameObject> m_UsedChunkGameObjects = new List<ChunkGameObject>();

	// Token: 0x0400431D RID: 17181
	public List<ChunkManager.ChunkObserver> m_ObservedEntities = new List<ChunkManager.ChunkObserver>();

	// Token: 0x0400431E RID: 17182
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo threadInfoRegenerating;

	// Token: 0x0400431F RID: 17183
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo threadInfoCalc;

	// Token: 0x04004320 RID: 17184
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<Chunk> chunksToUnload = new HashSetList<Chunk>();

	// Token: 0x04004321 RID: 17185
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile long[] chunkKeysToUnloadSnapshot = Array.Empty<long>();

	// Token: 0x04004322 RID: 17186
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector2i>[] rectanglesAroundPlayers;

	// Token: 0x04004323 RID: 17187
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NetPackage> sendToClientPackages = new List<NetPackage>();

	// Token: 0x04004324 RID: 17188
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConcurrentDictionary<long, DateTime> chunkGenerationTimestamps = new ConcurrentDictionary<long, DateTime>();

	// Token: 0x04004325 RID: 17189
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime lastVmlExhaustionLog = DateTime.MinValue;

	// Token: 0x04004326 RID: 17190
	[PublicizedFrom(EAccessModifier.Private)]
	public static TimeSpan vmlExhaustionLogInterval = TimeSpan.FromSeconds(30.0);

	// Token: 0x04004327 RID: 17191
	[PublicizedFrom(EAccessModifier.Private)]
	public static TimeSpan MaxVmlLogInterval = TimeSpan.FromMinutes(10.0);

	// Token: 0x04004328 RID: 17192
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime vmlExhaustionStartTime = DateTime.MinValue;

	// Token: 0x04004329 RID: 17193
	[PublicizedFrom(EAccessModifier.Private)]
	public const double MinLogThresholdSeconds = 1.0;

	// Token: 0x0400432A RID: 17194
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk currentCopiedChunk;

	// Token: 0x0400432B RID: 17195
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkGameObject currentCopiedChunkGameObject;

	// Token: 0x0400432C RID: 17196
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isContinueCopying;

	// Token: 0x0400432D RID: 17197
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCopiedChunkLayer;

	// Token: 0x0400432E RID: 17198
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInternalForceUpdate;

	// Token: 0x0400432F RID: 17199
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isChunkClusterChanged;

	// Token: 0x04004330 RID: 17200
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent calcThreadWaitHandle = new AutoResetEvent(false);

	// Token: 0x04004331 RID: 17201
	public static int MaxQueuedMeshLayers = 1000;

	// Token: 0x04004332 RID: 17202
	public Action<Chunk> OnChunkInitialized;

	// Token: 0x04004333 RID: 17203
	public Action<Chunk> OnChunkRegenerated;

	// Token: 0x04004334 RID: 17204
	public Action<Chunk> OnChunkCopiedToUnity;

	// Token: 0x04004335 RID: 17205
	public Action<Chunk> OnChunkStabilityCalculationEnabled;

	// Token: 0x04004336 RID: 17206
	public List<Chunk> ChunksToCopyInOneFrame = new List<Chunk>();

	// Token: 0x04004337 RID: 17207
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToCopyInOneFrameIndex;

	// Token: 0x04004338 RID: 17208
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunksToCopyInOneFramePass;

	// Token: 0x04004339 RID: 17209
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[] neighborsLightingThread = new Chunk[8];

	// Token: 0x0400433A RID: 17210
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool bLightingDone = true;

	// Token: 0x0400433B RID: 17211
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool bCalcPositionsDone = true;

	// Token: 0x0400433C RID: 17212
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isViewingOrCollisionPositionsChanged_threadCalc;

	// Token: 0x0400433D RID: 17213
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> viewingChunkPositionsCopy = new List<long>();

	// Token: 0x0400433E RID: 17214
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> collisionChunkPositionsCopy = new List<long>();

	// Token: 0x0400433F RID: 17215
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> lightingChunkPositionsCopy = new List<long>();

	// Token: 0x04004340 RID: 17216
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> chunksToCopyTemp = new List<long>();

	// Token: 0x04004341 RID: 17217
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> chunksToFreeTemp = new List<long>();

	// Token: 0x04004342 RID: 17218
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> chunksToLightTemp = new List<long>();

	// Token: 0x04004343 RID: 17219
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[] neighborsGenerationThread2 = new Chunk[8];

	// Token: 0x04004344 RID: 17220
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile bool isViewingOrCollisionPositionsChanged_threadReg = true;

	// Token: 0x04004345 RID: 17221
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk[] regenerateNextChunkNeighbors = new Chunk[8];

	// Token: 0x04004346 RID: 17222
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxCGOsToUnloadPerFrame = 8;

	// Token: 0x04004347 RID: 17223
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkGameObject> tempDisplayedCGOs = new List<ChunkGameObject>();

	// Token: 0x04004348 RID: 17224
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> cgoToRemove = new List<long>();

	// Token: 0x04004349 RID: 17225
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockEntityData>[] groundAlignBlockLists = new List<BlockEntityData>[]
	{
		new List<BlockEntityData>(),
		new List<BlockEntityData>()
	};

	// Token: 0x0400434A RID: 17226
	[PublicizedFrom(EAccessModifier.Private)]
	public int groundAlignIndex;

	// Token: 0x0400434B RID: 17227
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo bakeThreadInfo;

	// Token: 0x0400434C RID: 17228
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent bakeEvent = new AutoResetEvent(false);

	// Token: 0x0400434D RID: 17229
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator bakeCoroutine;

	// Token: 0x0400434E RID: 17230
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChunkManager.BakeCollider> bakes = new List<ChunkManager.BakeCollider>();

	// Token: 0x0400434F RID: 17231
	[PublicizedFrom(EAccessModifier.Private)]
	public int bakeIndex;

	// Token: 0x04004350 RID: 17232
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile ChunkManager.BakeCollider bakeCurrent;

	// Token: 0x04004351 RID: 17233
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, MicroStopwatch> debugTimers = new Dictionary<string, MicroStopwatch>();

	// Token: 0x02000B42 RID: 2882
	public class ChunkObserver
	{
		// Token: 0x060056B7 RID: 22199 RVA: 0x00216FAC File Offset: 0x002151AC
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

		// Token: 0x060056B8 RID: 22200 RVA: 0x00217042 File Offset: 0x00215242
		public void SetPosition(Vector3 _position)
		{
			this.position = _position;
		}

		// Token: 0x04004352 RID: 17234
		[PublicizedFrom(EAccessModifier.Private)]
		public static int idCnt;

		// Token: 0x04004353 RID: 17235
		public int id;

		// Token: 0x04004354 RID: 17236
		public Vector3 position;

		// Token: 0x04004355 RID: 17237
		public Vector3i curChunkPos;

		// Token: 0x04004356 RID: 17238
		public bool bBuildVisualMeshAround;

		// Token: 0x04004357 RID: 17239
		public int viewDim;

		// Token: 0x04004358 RID: 17240
		public HashSetLong chunksLoaded;

		// Token: 0x04004359 RID: 17241
		public BucketHashSetList chunksToLoad;

		// Token: 0x0400435A RID: 17242
		public List<long> chunksToReload;

		// Token: 0x0400435B RID: 17243
		public HashSetLong chunksToRemove;

		// Token: 0x0400435C RID: 17244
		public BucketHashSetList chunksAround;

		// Token: 0x0400435D RID: 17245
		public int entityIdToSendChunksTo;

		// Token: 0x0400435E RID: 17246
		public IMapChunkDatabase mapDatabase;
	}

	// Token: 0x02000B43 RID: 2883
	[PublicizedFrom(EAccessModifier.Private)]
	public class ThreadRegeneratingData
	{
		// Token: 0x0400435F RID: 17247
		public List<long> viewingChunkPositionsCopy = new List<long>();

		// Token: 0x04004360 RID: 17248
		public List<long> collisionChunkPositionsCopy = new List<long>();
	}

	// Token: 0x02000B44 RID: 2884
	public class BakeCollider
	{
		// Token: 0x04004361 RID: 17249
		public Mesh mesh;

		// Token: 0x04004362 RID: 17250
		public int id;

		// Token: 0x04004363 RID: 17251
		public MeshCollider meshCollider;

		// Token: 0x04004364 RID: 17252
		public bool isBaked;

		// Token: 0x04004365 RID: 17253
		public bool isCancelledDestroy;
	}
}
