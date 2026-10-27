using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x02000372 RID: 882
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshData
{
	// Token: 0x060019D2 RID: 6610 RVA: 0x000948CC File Offset: 0x00092ACC
	public string GetByteLengthString()
	{
		if (this.Bytes == null)
		{
			return "null";
		}
		return this.Bytes.Length.ToString();
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x000948F7 File Offset: 0x00092AF7
	public string Path(bool isRegionQueue)
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0},{1}.{2}", this.X, this.Z, isRegionQueue ? "region" : "mesh");
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x00094932 File Offset: 0x00092B32
	public bool Exists(bool isRegionQueue)
	{
		return SdFile.Exists(DynamicMeshFile.MeshLocation + string.Format("{0},{1}.{2}", this.X, this.Z, isRegionQueue ? "region" : "mesh"));
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x00094974 File Offset: 0x00092B74
	public bool GetLock(string debug)
	{
		DateTime now = DateTime.Now;
		bool flag = false;
		while (!this.TryTakeLock(debug))
		{
			if (DynamicMeshThread.RequestThreadStop)
			{
				Log.Out(this.ToDebugLocation() + " World is unloading so lock attempt failed " + debug);
				return false;
			}
			double totalSeconds = (DateTime.Now - now).TotalSeconds;
			if (!flag && totalSeconds > 5.0)
			{
				flag = true;
				if (DynamicMeshManager.DoLog)
				{
					Log.Out(this.ToDebugLocation() + " Waiting for lock to release: " + this.lastLock);
				}
			}
			if (totalSeconds > 60.0 && Monitor.IsEntered(this._lock))
			{
				Log.Warning(string.Concat(new string[]
				{
					"Forcing lock release to ",
					debug,
					" from ",
					this.lastLock,
					" after ",
					totalSeconds.ToString(),
					" seconds"
				}));
				this.ReleaseLock();
			}
		}
		return true;
	}

	// Token: 0x060019D6 RID: 6614 RVA: 0x00094A68 File Offset: 0x00092C68
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x00094A78 File Offset: 0x00092C78
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

	// Token: 0x060019D8 RID: 6616 RVA: 0x00094B2E File Offset: 0x00092D2E
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x00094B3B File Offset: 0x00092D3B
	public bool TryGetBytes(out byte[] bytes, string debug)
	{
		if (this.TryTakeLock(debug))
		{
			bytes = this.Bytes;
			return true;
		}
		bytes = null;
		return false;
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x00094B54 File Offset: 0x00092D54
	public bool TryExit(string debug)
	{
		if (!Monitor.IsEntered(this._lock))
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Warning(this.ToDebugLocation() + " Tried to release lock when not owner " + debug);
			}
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

	// Token: 0x060019DB RID: 6619 RVA: 0x00094BC4 File Offset: 0x00092DC4
	public bool IsAvailableToLoad()
	{
		return this.Bytes != null && !this.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete) && !this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) && !this.StateInfo.HasFlag(DynamicMeshStates.LoadRequired);
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x00094C25 File Offset: 0x00092E25
	public string ToDebugLocation()
	{
		return string.Format("{0}:{1},{2}", this.IsRegion ? "R" : "C", this.X, this.Z);
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x00094C5B File Offset: 0x00092E5B
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x060019DE RID: 6622 RVA: 0x00094C8A File Offset: 0x00092E8A
	public static DynamicMeshData Create(int x, int z, bool isRegion)
	{
		return new DynamicMeshData
		{
			X = x,
			Z = z,
			IsRegion = isRegion
		};
	}

	// Token: 0x04001058 RID: 4184
	public byte[] Bytes;

	// Token: 0x04001059 RID: 4185
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x0400105A RID: 4186
	public string lastLock;

	// Token: 0x0400105B RID: 4187
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsRegion;

	// Token: 0x0400105C RID: 4188
	public DynamicMeshStates StateInfo;

	// Token: 0x0400105D RID: 4189
	public int X;

	// Token: 0x0400105E RID: 4190
	public int Z;

	// Token: 0x0400105F RID: 4191
	public int StreamLength;
}
