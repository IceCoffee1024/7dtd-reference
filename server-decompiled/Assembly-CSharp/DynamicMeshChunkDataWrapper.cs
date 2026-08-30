using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x02000363 RID: 867
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshChunkDataWrapper
{
	// Token: 0x0600196A RID: 6506 RVA: 0x0008F280 File Offset: 0x0008D480
	public void Reset()
	{
		this.StateInfo = DynamicMeshStates.None;
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x0008F28C File Offset: 0x0008D48C
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

	// Token: 0x0600196C RID: 6508 RVA: 0x0008F342 File Offset: 0x0008D542
	public string Path()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.update", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x0008F378 File Offset: 0x0008D578
	public string RawPath()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.raw", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0008F3AE File Offset: 0x0008D5AE
	public bool Exists()
	{
		return SdFile.Exists(this.Path());
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x0008F3BC File Offset: 0x0008D5BC
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

	// Token: 0x06001970 RID: 6512 RVA: 0x0008F47C File Offset: 0x0008D67C
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x0008F48C File Offset: 0x0008D68C
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

	// Token: 0x06001972 RID: 6514 RVA: 0x0008F542 File Offset: 0x0008D742
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x0008F54F File Offset: 0x0008D74F
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

	// Token: 0x06001974 RID: 6516 RVA: 0x0008F568 File Offset: 0x0008D768
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

	// Token: 0x06001975 RID: 6517 RVA: 0x0008F5D0 File Offset: 0x0008D7D0
	public bool IsAvailableToLoad()
	{
		return this.Data != null && !this.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete) && !this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) && !this.StateInfo.HasFlag(DynamicMeshStates.LoadRequired);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x0008F631 File Offset: 0x0008D831
	public string ToDebugLocation()
	{
		return string.Format("{0},{1}", this.X, this.Z);
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x0008F653 File Offset: 0x0008D853
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x0008F682 File Offset: 0x0008D882
	public static DynamicMeshChunkDataWrapper Create(long key)
	{
		return new DynamicMeshChunkDataWrapper
		{
			X = DynamicMeshUnity.GetWorldXFromKey(key),
			Z = DynamicMeshUnity.GetWorldZFromKey(key),
			Key = key
		};
	}

	// Token: 0x0400101A RID: 4122
	public DynamicMeshChunkData Data;

	// Token: 0x0400101B RID: 4123
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x0400101C RID: 4124
	public string lastLock;

	// Token: 0x0400101D RID: 4125
	public DynamicMeshStates StateInfo;

	// Token: 0x0400101E RID: 4126
	public int X;

	// Token: 0x0400101F RID: 4127
	public int Z;

	// Token: 0x04001020 RID: 4128
	public long Key;
}
