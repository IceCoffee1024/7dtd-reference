using System;
using System.Collections.Generic;
using Unity.Mathematics;

// Token: 0x02000CFD RID: 3325
public class WaterSimulationApplyChanges
{
	// Token: 0x06006530 RID: 25904 RVA: 0x0027CA54 File Offset: 0x0027AC54
	public WaterSimulationApplyChanges(ChunkCluster _cc)
	{
		this.chunks = _cc;
		this.isServer = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
		this.applyThread = ThreadManager.StartThread("WaterSimulationApplyChanges", null, new ThreadManager.ThreadFunctionLoopDelegate(this.ThreadLoop), null, null, null, true, false);
	}

	// Token: 0x06006531 RID: 25905 RVA: 0x0027CAF4 File Offset: 0x0027ACF4
	public WaterSimulationApplyChanges.ChangesForChunk.Writer GetChangeWriter(long _chunkKey)
	{
		Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk> dictionary = this.changeCache;
		WaterSimulationApplyChanges.ChangesForChunk.Writer writer;
		lock (dictionary)
		{
			WaterSimulationApplyChanges.ChangesForChunk changesForChunk;
			if (!this.changeCache.TryGetValue(_chunkKey, out changesForChunk))
			{
				changesForChunk = this.changesPool.AllocSync(true);
				this.changeCache.Add(_chunkKey, changesForChunk);
				this.changedChunkList.AddLast(_chunkKey);
			}
			writer = new WaterSimulationApplyChanges.ChangesForChunk.Writer(changesForChunk);
		}
		return writer;
	}

	// Token: 0x06006532 RID: 25906 RVA: 0x0027CB70 File Offset: 0x0027AD70
	public void DiscardChangesForChunks(List<long> _chunkKeys)
	{
		Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk> dictionary = this.changeCache;
		lock (dictionary)
		{
			foreach (long num in _chunkKeys)
			{
				WaterSimulationApplyChanges.ChangesForChunk changesForChunk;
				if (this.changeCache.TryGetValue(num, out changesForChunk))
				{
					this.changesPool.FreeSync(changesForChunk);
					this.changeCache.Remove(num);
					this.changedChunkList.Remove(num);
					Log.Out(string.Format("[DiscardChangesForChunks] Discarding pending water changes for chunk: {0}", num));
				}
			}
		}
	}

	// Token: 0x06006533 RID: 25907 RVA: 0x0027CC2C File Offset: 0x0027AE2C
	[PublicizedFrom(EAccessModifier.Private)]
	public int ThreadLoop(ThreadManager.ThreadInfo _threadInfo)
	{
		if (_threadInfo.TerminationRequested())
		{
			return -1;
		}
		if (this.isServer && this.networkMaxBytesPerSecond > 0L)
		{
			NetPackageMeasure netPackageMeasure = this.networkMeasure;
			lock (netPackageMeasure)
			{
				this.networkMeasure.RecalculateTotals();
			}
		}
		Chunk chunk;
		WaterSimulationApplyChanges.ChangesForChunk changesForChunk;
		if (!this.TryFindChangeToApply(out chunk, out changesForChunk))
		{
			return 15;
		}
		this.ApplyChanges(chunk, changesForChunk.changedVoxels);
		chunk.EnterWriteLock();
		chunk.InProgressWaterSim = false;
		chunk.ExitWriteLock();
		this.changesPool.FreeSync(changesForChunk);
		return 0;
	}

	// Token: 0x06006534 RID: 25908 RVA: 0x0027CCCC File Offset: 0x0027AECC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryFindChangeToApply(out Chunk _chunk, out WaterSimulationApplyChanges.ChangesForChunk _changes)
	{
		Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk> dictionary = this.changeCache;
		bool flag2;
		lock (dictionary)
		{
			if (this.changedChunkList.Count == 0)
			{
				_chunk = null;
				_changes = null;
				flag2 = false;
			}
			else
			{
				LinkedListNode<long> linkedListNode = this.changedChunkList.First;
				while (linkedListNode != null)
				{
					long value = linkedListNode.Value;
					if (!this.changeCache.TryGetValue(value, out _changes))
					{
						LinkedListNode<long> linkedListNode2 = linkedListNode;
						linkedListNode = linkedListNode.Next;
						this.changedChunkList.Remove(linkedListNode2);
					}
					else if (_changes.IsRecordingChanges)
					{
						linkedListNode = linkedListNode.Next;
					}
					else
					{
						if (WaterUtils.TryOpenChunkForUpdate(this.chunks, value, out _chunk))
						{
							LinkedListNode<long> linkedListNode3 = linkedListNode;
							linkedListNode = linkedListNode.Next;
							this.changedChunkList.Remove(linkedListNode3);
							this.changeCache.Remove(value);
							return true;
						}
						linkedListNode = linkedListNode.Next;
					}
				}
				_chunk = null;
				_changes = null;
				flag2 = false;
			}
		}
		return flag2;
	}

	// Token: 0x06006535 RID: 25909 RVA: 0x0027CDC8 File Offset: 0x0027AFC8
	public void ApplyChanges(Chunk _chunk, Dictionary<int, WaterValue> changedVoxels)
	{
		NetPackageWaterSimChunkUpdate netPackageWaterSimChunkUpdate = null;
		if (this.isServer && SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() > 0)
		{
			netPackageWaterSimChunkUpdate = this.SetupForSend(_chunk);
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		foreach (KeyValuePair<int, WaterValue> keyValuePair in changedVoxels)
		{
			int key = keyValuePair.Key;
			int3 voxelCoords = WaterDataHandle.GetVoxelCoords(key);
			WaterValue value = keyValuePair.Value;
			WaterValue waterValue;
			_chunk.SetWaterSimUpdate(voxelCoords.x, voxelCoords.y, voxelCoords.z, value, out waterValue);
			if (value.GetMass() != waterValue.GetMass())
			{
				if (WaterUtils.GetWaterLevel(waterValue) != WaterUtils.GetWaterLevel(value))
				{
					int num7 = voxelCoords.y >> 4;
					int num8 = 1 << num7;
					if ((voxelCoords.y & 3) == 3 && voxelCoords.y < 255)
					{
						num8 |= 2 << num7;
					}
					num2 |= num8;
					if (voxelCoords.x == 0)
					{
						num3 |= num8;
					}
					else if (voxelCoords.x == 15)
					{
						num4 |= num8;
					}
					if (voxelCoords.z == 0)
					{
						num5 |= num8;
					}
					else if (voxelCoords.z == 15)
					{
						num6 |= num8;
					}
				}
				if (netPackageWaterSimChunkUpdate != null)
				{
					netPackageWaterSimChunkUpdate.AddChange((ushort)key, value);
				}
			}
		}
		if (netPackageWaterSimChunkUpdate != null)
		{
			netPackageWaterSimChunkUpdate.FinalizeSend();
			num += this.SendUpdateToClients(netPackageWaterSimChunkUpdate);
		}
		if (num2 != 0)
		{
			_chunk.NeedsRegenerationOrBits(num2);
		}
		if (num3 != 0)
		{
			Chunk chunkSync = this.chunks.GetChunkSync(_chunk.X - 1, _chunk.Z);
			if (chunkSync != null)
			{
				chunkSync.NeedsRegenerationOrBits(num3);
			}
		}
		if (num4 != 0)
		{
			Chunk chunkSync2 = this.chunks.GetChunkSync(_chunk.X + 1, _chunk.Z);
			if (chunkSync2 != null)
			{
				chunkSync2.NeedsRegenerationOrBits(num4);
			}
		}
		if (num5 != 0)
		{
			Chunk chunkSync3 = this.chunks.GetChunkSync(_chunk.X, _chunk.Z - 1);
			if (chunkSync3 != null)
			{
				chunkSync3.NeedsRegenerationOrBits(num5);
			}
		}
		if (num6 != 0)
		{
			Chunk chunkSync4 = this.chunks.GetChunkSync(_chunk.X, _chunk.Z + 1);
			if (chunkSync4 != null)
			{
				chunkSync4.NeedsRegenerationOrBits(num6);
			}
		}
		if (num > 0)
		{
			if (this.networkMaxBytesPerSecond > 0L)
			{
				NetPackageMeasure netPackageMeasure = this.networkMeasure;
				lock (netPackageMeasure)
				{
					this.networkMeasure.AddSample((long)num);
				}
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.FlushClientSendQueues();
		}
	}

	// Token: 0x06006536 RID: 25910 RVA: 0x0027D070 File Offset: 0x0027B270
	public bool HasNetWorkLimitBeenReached()
	{
		if (this.isServer && this.networkMaxBytesPerSecond > 0L && SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() > 0)
		{
			NetPackageMeasure netPackageMeasure = this.networkMeasure;
			lock (netPackageMeasure)
			{
				return this.networkMeasure.totalSent > this.networkMaxBytesPerSecond;
			}
			return false;
		}
		return false;
	}

	// Token: 0x06006537 RID: 25911 RVA: 0x0027D0E0 File Offset: 0x0027B2E0
	public NetPackageWaterSimChunkUpdate SetupForSend(Chunk _chunk)
	{
		this.clientsNearChunkBuffer.Clear();
		long key = _chunk.Key;
		foreach (ClientInfo clientInfo in SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List)
		{
			EntityPlayer entityPlayer;
			if (GameManager.Instance.World.Players.dict.TryGetValue(clientInfo.entityId, out entityPlayer) && entityPlayer.ChunkObserver.chunksAround.Contains(key))
			{
				this.clientsNearChunkBuffer.Add(clientInfo);
			}
		}
		NetPackageWaterSimChunkUpdate package = NetPackageManager.GetPackage<NetPackageWaterSimChunkUpdate>();
		package.SetupForSend(_chunk);
		return package;
	}

	// Token: 0x06006538 RID: 25912 RVA: 0x0027D190 File Offset: 0x0027B390
	public int SendUpdateToClients(NetPackageWaterSimChunkUpdate _package)
	{
		int num = 0;
		_package.RegisterSendQueue();
		foreach (ClientInfo clientInfo in this.clientsNearChunkBuffer)
		{
			clientInfo.SendPackage(_package);
			num += _package.GetPackageLength();
		}
		_package.SendQueueHandled();
		return num;
	}

	// Token: 0x06006539 RID: 25913 RVA: 0x0027D1FC File Offset: 0x0027B3FC
	public void Cleanup()
	{
		this.applyThread.WaitForEnd(30);
	}

	// Token: 0x04004EFC RID: 20220
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<WaterSimulationApplyChanges.ChangesForChunk> changesPool = new MemoryPooledObject<WaterSimulationApplyChanges.ChangesForChunk>(300);

	// Token: 0x04004EFD RID: 20221
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCluster chunks;

	// Token: 0x04004EFE RID: 20222
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo applyThread;

	// Token: 0x04004EFF RID: 20223
	[PublicizedFrom(EAccessModifier.Private)]
	public const int noWorkPauseDurationMs = 15;

	// Token: 0x04004F00 RID: 20224
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk> changeCache = new Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk>();

	// Token: 0x04004F01 RID: 20225
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<long> changedChunkList = new LinkedList<long>();

	// Token: 0x04004F02 RID: 20226
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageMeasure networkMeasure = new NetPackageMeasure(1.0);

	// Token: 0x04004F03 RID: 20227
	public long networkMaxBytesPerSecond = 524288L;

	// Token: 0x04004F04 RID: 20228
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ClientInfo> clientsNearChunkBuffer = new List<ClientInfo>();

	// Token: 0x04004F05 RID: 20229
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isServer;

	// Token: 0x02000CFE RID: 3326
	public class ChangesForChunk : IMemoryPoolableObject
	{
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x0600653A RID: 25914 RVA: 0x0027D20C File Offset: 0x0027B40C
		public bool IsRecordingChanges
		{
			get
			{
				bool flag2;
				lock (this)
				{
					flag2 = this.isRecordingChanges;
				}
				return flag2;
			}
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x0027D24C File Offset: 0x0027B44C
		public void Cleanup()
		{
			this.Reset();
		}

		// Token: 0x0600653C RID: 25916 RVA: 0x0027D254 File Offset: 0x0027B454
		public void Reset()
		{
			this.isRecordingChanges = false;
			this.changedVoxels.Clear();
		}

		// Token: 0x04004F06 RID: 20230
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRecordingChanges;

		// Token: 0x04004F07 RID: 20231
		public Dictionary<int, WaterValue> changedVoxels = new Dictionary<int, WaterValue>();

		// Token: 0x02000CFF RID: 3327
		public struct Writer : IDisposable
		{
			// Token: 0x0600653E RID: 25918 RVA: 0x0027D27C File Offset: 0x0027B47C
			public Writer(WaterSimulationApplyChanges.ChangesForChunk _changes)
			{
				lock (_changes)
				{
					_changes.isRecordingChanges = true;
				}
				this.changes = _changes;
			}

			// Token: 0x0600653F RID: 25919 RVA: 0x0027D2C0 File Offset: 0x0027B4C0
			public void RecordChange(int _voxelIndex, WaterValue _waterValue)
			{
				this.changes.changedVoxels[_voxelIndex] = _waterValue;
			}

			// Token: 0x06006540 RID: 25920 RVA: 0x0027D2D4 File Offset: 0x0027B4D4
			public void Dispose()
			{
				WaterSimulationApplyChanges.ChangesForChunk changesForChunk = this.changes;
				lock (changesForChunk)
				{
					this.changes.isRecordingChanges = false;
				}
			}

			// Token: 0x04004F08 RID: 20232
			[PublicizedFrom(EAccessModifier.Private)]
			public WaterSimulationApplyChanges.ChangesForChunk changes;
		}
	}
}
