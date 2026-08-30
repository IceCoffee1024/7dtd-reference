using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x0200035F RID: 863
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshChunkDataWrapper
{
	// Token: 0x0600194D RID: 6477 RVA: 0x0008EB40 File Offset: 0x0008CD40
	public void Reset()
	{
		this.StateInfo = DynamicMeshStates.None;
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x0008EB4C File Offset: 0x0008CD4C
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

	// Token: 0x0600194F RID: 6479 RVA: 0x0008EC02 File Offset: 0x0008CE02
	public string Path()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.update", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x0008EC38 File Offset: 0x0008CE38
	public string RawPath()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.raw", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x0008EC6E File Offset: 0x0008CE6E
	public bool Exists()
	{
		return SdFile.Exists(this.Path());
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x0008EC7C File Offset: 0x0008CE7C
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

	// Token: 0x06001953 RID: 6483 RVA: 0x0008ED3C File Offset: 0x0008CF3C
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x0008ED4C File Offset: 0x0008CF4C
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

	// Token: 0x06001955 RID: 6485 RVA: 0x0008EE02 File Offset: 0x0008D002
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x0008EE0F File Offset: 0x0008D00F
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

	// Token: 0x06001957 RID: 6487 RVA: 0x0008EE28 File Offset: 0x0008D028
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

	// Token: 0x06001958 RID: 6488 RVA: 0x0008EE90 File Offset: 0x0008D090
	public bool IsAvailableToLoad()
	{
		return this.Data != null && !this.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete) && !this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) && !this.StateInfo.HasFlag(DynamicMeshStates.LoadRequired);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x0008EEF1 File Offset: 0x0008D0F1
	public string ToDebugLocation()
	{
		return string.Format("{0},{1}", this.X, this.Z);
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x0008EF13 File Offset: 0x0008D113
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x0008EF42 File Offset: 0x0008D142
	public static DynamicMeshChunkDataWrapper Create(long key)
	{
		return new DynamicMeshChunkDataWrapper
		{
			X = DynamicMeshUnity.GetWorldXFromKey(key),
			Z = DynamicMeshUnity.GetWorldZFromKey(key),
			Key = key
		};
	}

	// Token: 0x04001005 RID: 4101
	public DynamicMeshChunkData Data;

	// Token: 0x04001006 RID: 4102
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x04001007 RID: 4103
	public string lastLock;

	// Token: 0x04001008 RID: 4104
	public DynamicMeshStates StateInfo;

	// Token: 0x04001009 RID: 4105
	public int X;

	// Token: 0x0400100A RID: 4106
	public int Z;

	// Token: 0x0400100B RID: 4107
	public long Key;
}
