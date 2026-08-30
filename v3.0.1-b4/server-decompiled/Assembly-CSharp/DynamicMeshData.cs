using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x02000369 RID: 873
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshData
{
	// Token: 0x060019B6 RID: 6582 RVA: 0x00093CD8 File Offset: 0x00091ED8
	public string GetByteLengthString()
	{
		if (this.Bytes == null)
		{
			return "null";
		}
		return this.Bytes.Length.ToString();
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x00093D03 File Offset: 0x00091F03
	public string Path(bool isRegionQueue)
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0},{1}.{2}", this.X, this.Z, isRegionQueue ? "region" : "mesh");
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x00093D3E File Offset: 0x00091F3E
	public bool Exists(bool isRegionQueue)
	{
		return SdFile.Exists(DynamicMeshFile.MeshLocation + string.Format("{0},{1}.{2}", this.X, this.Z, isRegionQueue ? "region" : "mesh"));
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x00093D80 File Offset: 0x00091F80
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

	// Token: 0x060019BA RID: 6586 RVA: 0x00093E74 File Offset: 0x00092074
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x00093E84 File Offset: 0x00092084
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

	// Token: 0x060019BC RID: 6588 RVA: 0x00093F3A File Offset: 0x0009213A
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x00093F47 File Offset: 0x00092147
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

	// Token: 0x060019BE RID: 6590 RVA: 0x00093F60 File Offset: 0x00092160
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

	// Token: 0x060019BF RID: 6591 RVA: 0x00093FD0 File Offset: 0x000921D0
	public bool IsAvailableToLoad()
	{
		return this.Bytes != null && !this.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete) && !this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) && !this.StateInfo.HasFlag(DynamicMeshStates.LoadRequired);
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x00094031 File Offset: 0x00092231
	public string ToDebugLocation()
	{
		return string.Format("{0}:{1},{2}", this.IsRegion ? "R" : "C", this.X, this.Z);
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x00094067 File Offset: 0x00092267
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x00094096 File Offset: 0x00092296
	public static DynamicMeshData Create(int x, int z, bool isRegion)
	{
		return new DynamicMeshData
		{
			X = x,
			Z = z,
			IsRegion = isRegion
		};
	}

	// Token: 0x0400103A RID: 4154
	public byte[] Bytes;

	// Token: 0x0400103B RID: 4155
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x0400103C RID: 4156
	public string lastLock;

	// Token: 0x0400103D RID: 4157
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsRegion;

	// Token: 0x0400103E RID: 4158
	public DynamicMeshStates StateInfo;

	// Token: 0x0400103F RID: 4159
	public int X;

	// Token: 0x04001040 RID: 4160
	public int Z;

	// Token: 0x04001041 RID: 4161
	public int StreamLength;
}
