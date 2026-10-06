using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x0200039F RID: 927
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshRegionDataWrapper
{
	// Token: 0x06001B8C RID: 7052 RVA: 0x000A3CA4 File Offset: 0x000A1EA4
	public void Reset()
	{
		this.StateInfo = DynamicMeshStates.None;
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x000A3CB0 File Offset: 0x000A1EB0
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

	// Token: 0x06001B8E RID: 7054 RVA: 0x000A3D66 File Offset: 0x000A1F66
	public string Path()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.group", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x000A3D9C File Offset: 0x000A1F9C
	public string RawPath()
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0}.raw", WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.X), World.toChunkXZ(this.Z)));
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x000A3DD2 File Offset: 0x000A1FD2
	public bool Exists()
	{
		return SdFile.Exists(this.Path());
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x000A3DE0 File Offset: 0x000A1FE0
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

	// Token: 0x06001B92 RID: 7058 RVA: 0x000A3E9D File Offset: 0x000A209D
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x000A3EAC File Offset: 0x000A20AC
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

	// Token: 0x06001B94 RID: 7060 RVA: 0x000A3F62 File Offset: 0x000A2162
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x000A3F70 File Offset: 0x000A2170
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

	// Token: 0x06001B96 RID: 7062 RVA: 0x000A3FD8 File Offset: 0x000A21D8
	public string ToDebugLocation()
	{
		return string.Format("{0},{1}", this.X, this.Z);
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x000A3FFA File Offset: 0x000A21FA
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000A4029 File Offset: 0x000A2229
	public static DynamicMeshRegionDataWrapper Create(long key)
	{
		return new DynamicMeshRegionDataWrapper
		{
			X = DynamicMeshUnity.GetWorldXFromKey(key),
			Z = DynamicMeshUnity.GetWorldZFromKey(key),
			Key = key
		};
	}

	// Token: 0x040011C0 RID: 4544
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x040011C1 RID: 4545
	public string lastLock;

	// Token: 0x040011C2 RID: 4546
	public DynamicMeshStates StateInfo;

	// Token: 0x040011C3 RID: 4547
	public int X;

	// Token: 0x040011C4 RID: 4548
	public int Z;

	// Token: 0x040011C5 RID: 4549
	public long Key;
}
