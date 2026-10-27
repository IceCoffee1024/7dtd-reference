using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x02000368 RID: 872
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshChunkDataWrapper
{
	// Token: 0x06001969 RID: 6505 RVA: 0x0008F734 File Offset: 0x0008D934
	public void Reset()
	{
		this.StateInfo = DynamicMeshStates.None;
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x0008F740 File Offset: 0x0008D940
	public bool IsReadyForRelease()
	{
		if (this.StateInfo.HasFlag(DynamicMeshStates.SaveRequired))
		{
			if (DynamicMeshManager.DebugReleases)
			{
				Log.Out(string.Format("{0},{1} save required", this.X, this.Z));
			}
			return false;
		}
		if (this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) || this.StateInfo.HasFlag(DynamicMeshStates.Generating))
		{
			if (DynamicMeshManager.DebugReleases)
			{
				Log.Out(string.Format("{0},{1} thread updating", this.X, this.Z));
			}
			return false;
		}
		return true;
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x0008F7F6 File Offset: 0x0008D9F6
	public string Path()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.update", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x0008F82C File Offset: 0x0008DA2C
	public string RawPath()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.raw", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x0008F862 File Offset: 0x0008DA62
	public bool Exists()
	{
		return SdFile.Exists(this.Path());
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0008F870 File Offset: 0x0008DA70
	public bool GetLock(string debug)
	{
		int num = 0;
		while (!this.TryTakeLock(debug))
		{
			if (DynamicMeshThread.RequestThreadStop)
			{
				Log.Out(this.ToDebugLocation() + " World is unloading so lock attempt failed " + debug);
				return false;
			}
			if (num == 0 || ++num % 10 == 0)
			{
				Log.Out(this.ToDebugLocation() + " Waiting for lock to release: " + this.lastLock);
			}
			if (num > 600 && Monitor.IsEntered(this._lock))
			{
				Log.Warning(string.Concat(new string[] { "Forcing lock release to ", debug, " from ", this.lastLock, " after 60 seconds" }));
				this.ReleaseLock();
			}
			Thread.Sleep(100);
		}
		return true;
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x0008F930 File Offset: 0x0008DB30
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x0008F940 File Offset: 0x0008DB40
	public bool TryTakeLock(string debug)
	{
		bool flag = false;
		if (Monitor.IsEntered(this._lock))
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Warning(this.ToDebugLocation() + " Lock kept by " + debug);
			}
			this.lastLock = debug;
			return true;
		}
		Monitor.TryEnter(this._lock, ref flag);
		if (flag)
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Warning(this.ToDebugLocation() + " Lock taken by " + debug);
			}
			this.lastLock = debug;
		}
		else if (DynamicMeshManager.DoLog)
		{
			Log.Warning(string.Concat(new string[]
			{
				this.ToDebugLocation(),
				" Lock failed on ",
				debug,
				" : ",
				this.lastLock
			}));
		}
		return flag;
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x0008F9F6 File Offset: 0x0008DBF6
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x0008FA03 File Offset: 0x0008DC03
	public bool TryGetData(out DynamicMeshChunkData data, string debug)
	{
		if (this.TryTakeLock(debug))
		{
			data = this.Data;
			return true;
		}
		data = null;
		return false;
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x0008FA1C File Offset: 0x0008DC1C
	public bool TryExit(string debug)
	{
		if (!Monitor.IsEntered(this._lock))
		{
			Log.Warning(this.ToDebugLocation() + " Tried to release lock when not owner " + debug);
			return false;
		}
		while (Monitor.IsEntered(this._lock))
		{
			Monitor.Exit(this._lock);
		}
		if (DynamicMeshManager.DoLog)
		{
			Log.Out(this.ToDebugLocation() + " Lock released " + debug);
		}
		return true;
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x0008FA84 File Offset: 0x0008DC84
	public bool IsAvailableToLoad()
	{
		return this.Data != null && !this.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete) && !this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) && !this.StateInfo.HasFlag(DynamicMeshStates.LoadRequired);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x0008FAE5 File Offset: 0x0008DCE5
	public string ToDebugLocation()
	{
		return string.Format("{0},{1}", this.X, this.Z);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x0008FB07 File Offset: 0x0008DD07
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x0008FB36 File Offset: 0x0008DD36
	public static DynamicMeshChunkDataWrapper Create(long key)
	{
		return new DynamicMeshChunkDataWrapper
		{
			X = DynamicMeshUnity.GetWorldXFromKey(key),
			Z = DynamicMeshUnity.GetWorldZFromKey(key),
			Key = key
		};
	}

	// Token: 0x04001023 RID: 4131
	public DynamicMeshChunkData Data;

	// Token: 0x04001024 RID: 4132
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x04001025 RID: 4133
	public string lastLock;

	// Token: 0x04001026 RID: 4134
	public DynamicMeshStates StateInfo;

	// Token: 0x04001027 RID: 4135
	public int X;

	// Token: 0x04001028 RID: 4136
	public int Z;

	// Token: 0x04001029 RID: 4137
	public long Key;
}
