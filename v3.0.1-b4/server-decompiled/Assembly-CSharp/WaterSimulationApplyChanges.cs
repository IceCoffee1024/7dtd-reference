using System;
using System.Collections.Generic;
using Unity.Mathematics;

// Token: 0x02000CD2 RID: 3282
public class WaterSimulationApplyChanges
{
	// Token: 0x0600648D RID: 25741 RVA: 0x0027528C File Offset: 0x0027348C
	public WaterSimulationApplyChanges(ChunkCluster _cc)
	{
		this.chunks = _cc;
		this.isServer = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
		this.applyThread = ThreadManager.StartThread("WaterSimulationApplyChanges", null, new ThreadManager.ThreadFunctionLoopDelegate(this.ThreadLoop), null, null, null, true, false);
	}

	// Token: 0x0600648E RID: 25742 RVA: 0x0027532C File Offset: 0x0027352C
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

	// Token: 0x0600648F RID: 25743 RVA: 0x002753A8 File Offset: 0x002735A8
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

	// Token: 0x06006490 RID: 25744 RVA: 0x00275464 File Offset: 0x00273664
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

	// Token: 0x06006491 RID: 25745 RVA: 0x00275504 File Offset: 0x00273704
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

	// Token: 0x06006492 RID: 25746 RVA: 0x00275600 File Offset: 0x00273800
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

	// Token: 0x06006493 RID: 25747 RVA: 0x002758A8 File Offset: 0x00273AA8
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

	// Token: 0x06006494 RID: 25748 RVA: 0x00275918 File Offset: 0x00273B18
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

	// Token: 0x06006495 RID: 25749 RVA: 0x002759C8 File Offset: 0x00273BC8
	public int SendUpdateToClients(NetPackageWaterSimChunkUpdate _package)
	{
		int num = 0;
		_package.RegisterSendQueue();
		foreach (ClientInfo clientInfo in this.clientsNearChunkBuffer)
		{
			clientInfo.SendPackage(_package);
			num += _package.GetLength();
		}
		_package.SendQueueHandled();
		return num;
	}

	// Token: 0x06006496 RID: 25750 RVA: 0x00275A34 File Offset: 0x00273C34
	public void Cleanup()
	{
		this.applyThread.WaitForEnd(30);
	}

	// Token: 0x04004E03 RID: 19971
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<WaterSimulationApplyChanges.ChangesForChunk> changesPool = new MemoryPooledObject<WaterSimulationApplyChanges.ChangesForChunk>(300);

	// Token: 0x04004E04 RID: 19972
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCluster chunks;

	// Token: 0x04004E05 RID: 19973
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo applyThread;

	// Token: 0x04004E06 RID: 19974
	[PublicizedFrom(EAccessModifier.Private)]
	public const int noWorkPauseDurationMs = 15;

	// Token: 0x04004E07 RID: 19975
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk> changeCache = new Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk>();

	// Token: 0x04004E08 RID: 19976
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<long> changedChunkList = new LinkedList<long>();

	// Token: 0x04004E09 RID: 19977
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageMeasure networkMeasure = new NetPackageMeasure(1.0);

	// Token: 0x04004E0A RID: 19978
	public long networkMaxBytesPerSecond = 524288L;

	// Token: 0x04004E0B RID: 19979
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ClientInfo> clientsNearChunkBuffer = new List<ClientInfo>();

	// Token: 0x04004E0C RID: 19980
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isServer;

	// Token: 0x02000CD3 RID: 3283
	public class ChangesForChunk : IMemoryPoolableObject
	{
		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06006497 RID: 25751 RVA: 0x00275A44 File Offset: 0x00273C44
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

		// Token: 0x06006498 RID: 25752 RVA: 0x00275A84 File Offset: 0x00273C84
		public void Cleanup()
		{
			this.Reset();
		}

		// Token: 0x06006499 RID: 25753 RVA: 0x00275A8C File Offset: 0x00273C8C
		public void Reset()
		{
			this.isRecordingChanges = false;
			this.changedVoxels.Clear();
		}

		// Token: 0x04004E0D RID: 19981
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRecordingChanges;

		// Token: 0x04004E0E RID: 19982
		public Dictionary<int, WaterValue> changedVoxels = new Dictionary<int, WaterValue>();

		// Token: 0x02000CD4 RID: 3284
		public struct Writer : IDisposable
		{
			// Token: 0x0600649B RID: 25755 RVA: 0x00275AB4 File Offset: 0x00273CB4
			public Writer(WaterSimulationApplyChanges.ChangesForChunk _changes)
			{
				lock (_changes)
				{
					_changes.isRecordingChanges = true;
				}
				this.changes = _changes;
			}

			// Token: 0x0600649C RID: 25756 RVA: 0x00275AF8 File Offset: 0x00273CF8
			public void RecordChange(int _voxelIndex, WaterValue _waterValue)
			{
				this.changes.changedVoxels[_voxelIndex] = _waterValue;
			}

			// Token: 0x0600649D RID: 25757 RVA: 0x00275B0C File Offset: 0x00273D0C
			public void Dispose()
			{
				WaterSimulationApplyChanges.ChangesForChunk changesForChunk = this.changes;
				lock (changesForChunk)
				{
					this.changes.isRecordingChanges = false;
				}
			}

			// Token: 0x04004E0F RID: 19983
			[PublicizedFrom(EAccessModifier.Private)]
			public WaterSimulationApplyChanges.ChangesForChunk changes;
		}
	}
}
