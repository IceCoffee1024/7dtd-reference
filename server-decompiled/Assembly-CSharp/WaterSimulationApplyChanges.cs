using System;
using System.Collections.Generic;
using Unity.Mathematics;

// Token: 0x02000CE4 RID: 3300
public class WaterSimulationApplyChanges
{
	// Token: 0x06006519 RID: 25881 RVA: 0x00278188 File Offset: 0x00276388
	public WaterSimulationApplyChanges(ChunkCluster _cc)
	{
		this.chunks = _cc;
		this.isServer = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
		this.applyThread = ThreadManager.StartThread("WaterSimulationApplyChanges", null, new ThreadManager.ThreadFunctionLoopDelegate(this.ThreadLoop), null, null, null, true, false);
	}

	// Token: 0x0600651A RID: 25882 RVA: 0x00278228 File Offset: 0x00276428
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

	// Token: 0x0600651B RID: 25883 RVA: 0x002782A4 File Offset: 0x002764A4
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

	// Token: 0x0600651C RID: 25884 RVA: 0x00278360 File Offset: 0x00276560
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

	// Token: 0x0600651D RID: 25885 RVA: 0x00278400 File Offset: 0x00276600
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

	// Token: 0x0600651E RID: 25886 RVA: 0x002784FC File Offset: 0x002766FC
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

	// Token: 0x0600651F RID: 25887 RVA: 0x002787A4 File Offset: 0x002769A4
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

	// Token: 0x06006520 RID: 25888 RVA: 0x00278814 File Offset: 0x00276A14
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

	// Token: 0x06006521 RID: 25889 RVA: 0x002788C4 File Offset: 0x00276AC4
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

	// Token: 0x06006522 RID: 25890 RVA: 0x00278930 File Offset: 0x00276B30
	public void Cleanup()
	{
		this.applyThread.WaitForEnd(30);
	}

	// Token: 0x04004EA1 RID: 20129
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryPooledObject<WaterSimulationApplyChanges.ChangesForChunk> changesPool = new MemoryPooledObject<WaterSimulationApplyChanges.ChangesForChunk>(300);

	// Token: 0x04004EA2 RID: 20130
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkCluster chunks;

	// Token: 0x04004EA3 RID: 20131
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo applyThread;

	// Token: 0x04004EA4 RID: 20132
	[PublicizedFrom(EAccessModifier.Private)]
	public const int noWorkPauseDurationMs = 15;

	// Token: 0x04004EA5 RID: 20133
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk> changeCache = new Dictionary<long, WaterSimulationApplyChanges.ChangesForChunk>();

	// Token: 0x04004EA6 RID: 20134
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<long> changedChunkList = new LinkedList<long>();

	// Token: 0x04004EA7 RID: 20135
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageMeasure networkMeasure = new NetPackageMeasure(1.0);

	// Token: 0x04004EA8 RID: 20136
	public long networkMaxBytesPerSecond = 524288L;

	// Token: 0x04004EA9 RID: 20137
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ClientInfo> clientsNearChunkBuffer = new List<ClientInfo>();

	// Token: 0x04004EAA RID: 20138
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isServer;

	// Token: 0x02000CE5 RID: 3301
	public class ChangesForChunk : IMemoryPoolableObject
	{
		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06006523 RID: 25891 RVA: 0x00278940 File Offset: 0x00276B40
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

		// Token: 0x06006524 RID: 25892 RVA: 0x00278980 File Offset: 0x00276B80
		public void Cleanup()
		{
			this.Reset();
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x00278988 File Offset: 0x00276B88
		public void Reset()
		{
			this.isRecordingChanges = false;
			this.changedVoxels.Clear();
		}

		// Token: 0x04004EAB RID: 20139
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isRecordingChanges;

		// Token: 0x04004EAC RID: 20140
		public Dictionary<int, WaterValue> changedVoxels = new Dictionary<int, WaterValue>();

		// Token: 0x02000CE6 RID: 3302
		public struct Writer : IDisposable
		{
			// Token: 0x06006527 RID: 25895 RVA: 0x002789B0 File Offset: 0x00276BB0
			public Writer(WaterSimulationApplyChanges.ChangesForChunk _changes)
			{
				lock (_changes)
				{
					_changes.isRecordingChanges = true;
				}
				this.changes = _changes;
			}

			// Token: 0x06006528 RID: 25896 RVA: 0x002789F4 File Offset: 0x00276BF4
			public void RecordChange(int _voxelIndex, WaterValue _waterValue)
			{
				this.changes.changedVoxels[_voxelIndex] = _waterValue;
			}

			// Token: 0x06006529 RID: 25897 RVA: 0x00278A08 File Offset: 0x00276C08
			public void Dispose()
			{
				WaterSimulationApplyChanges.ChangesForChunk changesForChunk = this.changes;
				lock (changesForChunk)
				{
					this.changes.isRecordingChanges = false;
				}
			}

			// Token: 0x04004EAD RID: 20141
			[PublicizedFrom(EAccessModifier.Private)]
			public WaterSimulationApplyChanges.ChangesForChunk changes;
		}
	}
}
