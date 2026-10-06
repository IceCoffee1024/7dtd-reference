using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x02000370 RID: 880
[DebuggerDisplay("{X},{Z} {StateInfo}")]
public class DynamicMeshData
{
	// Token: 0x060019CE RID: 6606 RVA: 0x00094698 File Offset: 0x00092898
	public string GetByteLengthString()
	{
		if (this.Bytes == null)
		{
			return "null";
		}
		return this.Bytes.Length.ToString();
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000946C3 File Offset: 0x000928C3
	public string Path(bool isRegionQueue)
	{
		return DynamicMeshFile.MeshLocation + string.Format("{0},{1}.{2}", this.X, this.Z, isRegionQueue ? "region" : "mesh");
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000946FE File Offset: 0x000928FE
	public bool Exists(bool isRegionQueue)
	{
		return SdFile.Exists(DynamicMeshFile.MeshLocation + string.Format("{0},{1}.{2}", this.X, this.Z, isRegionQueue ? "region" : "mesh"));
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x00094740 File Offset: 0x00092940
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

	// Token: 0x060019D2 RID: 6610 RVA: 0x00094834 File Offset: 0x00092A34
	public bool ReleaseLock()
	{
		return this.TryExit("releaseLock");
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x00094844 File Offset: 0x00092A44
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

	// Token: 0x060019D4 RID: 6612 RVA: 0x000948FA File Offset: 0x00092AFA
	public bool ThreadHasLock()
	{
		return Monitor.IsEntered(this._lock);
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x00094907 File Offset: 0x00092B07
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

	// Token: 0x060019D6 RID: 6614 RVA: 0x00094920 File Offset: 0x00092B20
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

	// Token: 0x060019D7 RID: 6615 RVA: 0x00094990 File Offset: 0x00092B90
	public bool IsAvailableToLoad()
	{
		return this.Bytes != null && !this.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete) && !this.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating) && !this.StateInfo.HasFlag(DynamicMeshStates.LoadRequired);
	}

	// Token: 0x060019D8 RID: 6616 RVA: 0x000949F1 File Offset: 0x00092BF1
	public string ToDebugLocation()
	{
		return string.Format("{0}:{1},{2}", this.IsRegion ? "R" : "C", this.X, this.Z);
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x00094A27 File Offset: 0x00092C27
	public void ClearUnloadMarks()
	{
		this.StateInfo &= ~DynamicMeshStates.UnloadMark1;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark2;
		this.StateInfo &= ~DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x00094A56 File Offset: 0x00092C56
	public static DynamicMeshData Create(int x, int z, bool isRegion)
	{
		return new DynamicMeshData
		{
			X = x,
			Z = z,
			IsRegion = isRegion
		};
	}

	// Token: 0x04001048 RID: 4168
	public byte[] Bytes;

	// Token: 0x04001049 RID: 4169
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x0400104A RID: 4170
	public string lastLock;

	// Token: 0x0400104B RID: 4171
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsRegion;

	// Token: 0x0400104C RID: 4172
	public DynamicMeshStates StateInfo;

	// Token: 0x0400104D RID: 4173
	public int X;

	// Token: 0x0400104E RID: 4174
	public int Z;

	// Token: 0x0400104F RID: 4175
	public int StreamLength;
}
