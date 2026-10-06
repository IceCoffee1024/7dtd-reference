using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x02000366 RID: 870
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshChunkDataWrapper
{
	// Token: 0x06001965 RID: 6501 RVA: 0x0008F500 File Offset: 0x0008D700
	public void Reset()
	{
		this.StateInfo = DynamicMeshStates.None;
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x0008F50C File Offset: 0x0008D70C
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

	// Token: 0x06001967 RID: 6503 RVA: 0x0008F5C2 File Offset: 0x0008D7C2
	public string Path()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.update", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x0008F5F8 File Offset: 0x0008D7F8
	public string RawPath()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.raw", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x0008F62E File Offset: 0x0008D82E
	public bool Exists()
	{
		return SdFile.Exists(this.Path());
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x0008F63C File Offset: 0x0008D83C
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

	// Token: 0x0600196B RID: 6507 RVA: 0x0008F6FC File Offset: 0x0008D8FC
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x0008F70C File Offset: 0x0008D90C
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

	// Token: 0x0600196D RID: 6509 RVA: 0x0008F7C2 File Offset: 0x0008D9C2
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0008F7CF File Offset: 0x0008D9CF
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

	// Token: 0x0600196F RID: 6511 RVA: 0x0008F7E8 File Offset: 0x0008D9E8
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

	// Token: 0x06001970 RID: 6512 RVA: 0x0008F850 File Offset: 0x0008DA50
	public bool IsAvailableToLoad()
	{
		return this.Data != null && !this.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete) && !this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) && !this.StateInfo.HasFlag(DynamicMeshStates.LoadRequired);
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x0008F8B1 File Offset: 0x0008DAB1
	public string ToDebugLocation()
	{
		return string.Format("{0},{1}", this.X, this.Z);
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x0008F8D3 File Offset: 0x0008DAD3
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x0008F902 File Offset: 0x0008DB02
	public static DynamicMeshChunkDataWrapper Create(long key)
	{
		return new DynamicMeshChunkDataWrapper
		{
			X = DynamicMeshUnity.GetWorldXFromKey(key),
			Z = DynamicMeshUnity.GetWorldZFromKey(key),
			Key = key
		};
	}

	// Token: 0x04001013 RID: 4115
	public DynamicMeshChunkData Data;

	// Token: 0x04001014 RID: 4116
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x04001015 RID: 4117
	public string lastLock;

	// Token: 0x04001016 RID: 4118
	public DynamicMeshStates StateInfo;

	// Token: 0x04001017 RID: 4119
	public int X;

	// Token: 0x04001018 RID: 4120
	public int Z;

	// Token: 0x04001019 RID: 4121
	public long Key;
}
