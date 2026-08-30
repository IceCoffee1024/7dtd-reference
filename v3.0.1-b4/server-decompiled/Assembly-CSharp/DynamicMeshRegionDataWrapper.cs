using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x02000398 RID: 920
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshRegionDataWrapper
{
	// Token: 0x06001B76 RID: 7030 RVA: 0x000A3324 File Offset: 0x000A1524
	public void Reset()
	{
		this.StateInfo = DynamicMeshStates.None;
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x000A3330 File Offset: 0x000A1530
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

	// Token: 0x06001B78 RID: 7032 RVA: 0x000A33E6 File Offset: 0x000A15E6
	public string Path()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.group", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x000A341C File Offset: 0x000A161C
	public string RawPath()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.raw", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000A3452 File Offset: 0x000A1652
	public bool Exists()
	{
		return SdFile.Exists(this.Path());
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x000A3460 File Offset: 0x000A1660
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
			if (++num % 10 == 0)
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

	// Token: 0x06001B7C RID: 7036 RVA: 0x000A351D File Offset: 0x000A171D
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000A352C File Offset: 0x000A172C
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

	// Token: 0x06001B7E RID: 7038 RVA: 0x000A35E2 File Offset: 0x000A17E2
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x000A35F0 File Offset: 0x000A17F0
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

	// Token: 0x06001B80 RID: 7040 RVA: 0x000A3658 File Offset: 0x000A1858
	public string ToDebugLocation()
	{
		return string.Format("{0},{1}", this.X, this.Z);
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x000A367A File Offset: 0x000A187A
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x000A36A9 File Offset: 0x000A18A9
	public static DynamicMeshRegionDataWrapper Create(long key)
	{
		return new DynamicMeshRegionDataWrapper
		{
			X = DynamicMeshUnity.GetWorldXFromKey(key),
			Z = DynamicMeshUnity.GetWorldZFromKey(key),
			Key = key
		};
	}

	// Token: 0x040011B2 RID: 4530
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x040011B3 RID: 4531
	public string lastLock;

	// Token: 0x040011B4 RID: 4532
	public DynamicMeshStates StateInfo;

	// Token: 0x040011B5 RID: 4533
	public int X;

	// Token: 0x040011B6 RID: 4534
	public int Z;

	// Token: 0x040011B7 RID: 4535
	public long Key;
}
