using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

// Token: 0x02000A3A RID: 2618
public abstract class SaveDataManagerBase : ISaveDataManager
{
	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06004E34 RID: 20020 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004E35 RID: 20021
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract Stream GetStream(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004E36 RID: 20022
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ReturnStream(Stream stream);

	// Token: 0x06004E37 RID: 20023 RVA: 0x001E0FD4 File Offset: 0x001DF1D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnCommitStarted()
	{
		object commitProgressLock = this.m_commitProgressLock;
		lock (commitProgressLock)
		{
			this.m_commitInProgress = true;
			this.m_commitInProgressChanged = true;
			this.QueueOnCommitTask();
		}
	}

	// Token: 0x06004E38 RID: 20024 RVA: 0x001E1024 File Offset: 0x001DF224
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnCommitFinished()
	{
		object commitProgressLock = this.m_commitProgressLock;
		lock (commitProgressLock)
		{
			this.m_commitInProgress = false;
			this.m_commitInProgressChanged = true;
			this.QueueOnCommitTask();
		}
	}

	// Token: 0x06004E39 RID: 20025 RVA: 0x001E1074 File Offset: 0x001DF274
	[PublicizedFrom(EAccessModifier.Private)]
	public void QueueOnCommitTask()
	{
		object commitProgressLock = this.m_commitProgressLock;
		lock (commitProgressLock)
		{
			if (!this.m_commitInProgressHasTask)
			{
				this.m_commitInProgressHasTask = true;
				ThreadManager.AddSingleTaskMainThread("SaveDataManager.OnCommitTask", new ThreadManager.MainThreadTaskFunctionDelegate(this.OnCommitTask), null);
			}
		}
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001E10D8 File Offset: 0x001DF2D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCommitTask(object _)
	{
		object commitProgressLock = this.m_commitProgressLock;
		bool commitInProgress;
		bool commitInProgressChanged;
		bool commitInProgressLast;
		lock (commitProgressLock)
		{
			commitInProgress = this.m_commitInProgress;
			commitInProgressChanged = this.m_commitInProgressChanged;
			commitInProgressLast = this.m_commitInProgressLast;
			this.m_commitInProgressChanged = false;
			this.m_commitInProgressLast = commitInProgress;
			this.m_commitInProgressHasTask = false;
		}
		if (commitInProgressLast != commitInProgress)
		{
			if (commitInProgress)
			{
				this.<OnCommitTask>g__FireCommitStarted|12_0();
				return;
			}
			this.<OnCommitTask>g__FireCommitFinished|12_1();
			return;
		}
		else
		{
			if (!commitInProgressChanged)
			{
				return;
			}
			if (!commitInProgress)
			{
				this.<OnCommitTask>g__FireCommitStarted|12_0();
				this.<OnCommitTask>g__FireCommitFinished|12_1();
				return;
			}
			this.<OnCommitTask>g__FireCommitFinished|12_1();
			this.<OnCommitTask>g__FireCommitStarted|12_0();
			return;
		}
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x001E1174 File Offset: 0x001DF374
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string text)
	{
		Log.Out("[SaveDataManagerBase] " + text);
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Init()
	{
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004E3E RID: 20030 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004E3F RID: 20031 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x06004E40 RID: 20032 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E41 RID: 20033 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004E44 RID: 20036 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CommitSync()
	{
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x001E1186 File Offset: 0x001DF386
	public virtual IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06004E46 RID: 20038 RVA: 0x001E1190 File Offset: 0x001DF390
	// (remove) Token: 0x06004E47 RID: 20039 RVA: 0x001E11C8 File Offset: 0x001DF3C8
	public event Action CommitStarted;

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06004E48 RID: 20040 RVA: 0x001E1200 File Offset: 0x001DF400
	// (remove) Token: 0x06004E49 RID: 20041 RVA: 0x001E1238 File Offset: 0x001DF438
	public event Action CommitFinished;

	// Token: 0x06004E4A RID: 20042 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004E4B RID: 20043 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004E4C RID: 20044 RVA: 0x001E1270 File Offset: 0x001DF470
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004E4D RID: 20045 RVA: 0x001E1286 File Offset: 0x001DF486
	public Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return new SaveDataManagerBase.WrappedStream(this, path, mode, access, share);
	}

	// Token: 0x06004E4E RID: 20046 RVA: 0x001E1293 File Offset: 0x001DF493
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		SaveDataManager_Placeholder.Instance.ManagedFileDelete(path);
	}

	// Token: 0x06004E4F RID: 20047 RVA: 0x001E12A0 File Offset: 0x001DF4A0
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileExists(path);
	}

	// Token: 0x06004E50 RID: 20048 RVA: 0x001E12AD File Offset: 0x001DF4AD
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E51 RID: 20049 RVA: 0x001E12BA File Offset: 0x001DF4BA
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004E52 RID: 20050 RVA: 0x001E12C7 File Offset: 0x001DF4C7
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E53 RID: 20051 RVA: 0x001E12D4 File Offset: 0x001DF4D4
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryExists(path);
	}

	// Token: 0x06004E54 RID: 20052 RVA: 0x001E12E1 File Offset: 0x001DF4E1
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004E55 RID: 20053 RVA: 0x001E12F0 File Offset: 0x001DF4F0
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004E56 RID: 20054 RVA: 0x001E12FF File Offset: 0x001DF4FF
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004E57 RID: 20055 RVA: 0x001E130E File Offset: 0x001DF50E
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataManager_Placeholder.Instance.ManagedDirectoryDelete(path, recursive);
	}

	// Token: 0x06004E58 RID: 20056 RVA: 0x001E131C File Offset: 0x001DF51C
	public virtual long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileInfoLength(path);
	}

	// Token: 0x06004E59 RID: 20057 RVA: 0x001E1329 File Offset: 0x001DF529
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004E5A RID: 20058 RVA: 0x001E1338 File Offset: 0x001DF538
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001E1347 File Offset: 0x001DF547
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption);
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001E1356 File Offset: 0x001DF556
	[PublicizedFrom(EAccessModifier.Protected)]
	public SaveDataManagerBase()
	{
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x001E1369 File Offset: 0x001DF569
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <OnCommitTask>g__FireCommitStarted|12_0()
	{
		Action commitStarted = this.CommitStarted;
		if (commitStarted == null)
		{
			return;
		}
		commitStarted();
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001E137B File Offset: 0x001DF57B
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <OnCommitTask>g__FireCommitFinished|12_1()
	{
		Action commitFinished = this.CommitFinished;
		if (commitFinished == null)
		{
			return;
		}
		commitFinished();
	}

	// Token: 0x04003CD4 RID: 15572
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_commitProgressLock = new object();

	// Token: 0x04003CD5 RID: 15573
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x04003CD6 RID: 15574
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressChanged;

	// Token: 0x04003CD7 RID: 15575
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressLast;

	// Token: 0x04003CD8 RID: 15576
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressHasTask;

	// Token: 0x02000A3B RID: 2619
	[PublicizedFrom(EAccessModifier.Private)]
	public class WrappedStream : Stream, IDisposable
	{
		// Token: 0x06004E5F RID: 20063 RVA: 0x001E138D File Offset: 0x001DF58D
		public WrappedStream(SaveDataManagerBase sdm, SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
		{
			this.sdm = sdm;
			this.stream = sdm.GetStream(path, mode, access, share);
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x001E13AE File Offset: 0x001DF5AE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					this.sdm.ReturnStream(this.stream);
				}
				this.disposedValue = true;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06004E61 RID: 20065 RVA: 0x001E13DA File Offset: 0x001DF5DA
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06004E62 RID: 20066 RVA: 0x001E13E7 File Offset: 0x001DF5E7
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x001E13F4 File Offset: 0x001DF5F4
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x001E1401 File Offset: 0x001DF601
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06004E65 RID: 20069 RVA: 0x001E140E File Offset: 0x001DF60E
		// (set) Token: 0x06004E66 RID: 20070 RVA: 0x001E141B File Offset: 0x001DF61B
		public override long Position
		{
			get
			{
				return this.stream.Position;
			}
			set
			{
				this.stream.Position = value;
			}
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x001E1429 File Offset: 0x001DF629
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x001E1436 File Offset: 0x001DF636
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x001E1446 File Offset: 0x001DF646
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06004E6A RID: 20074 RVA: 0x001E1455 File Offset: 0x001DF655
		public override void SetLength(long value)
		{
			this.stream.SetLength(value);
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x001E1463 File Offset: 0x001DF663
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x001E1473 File Offset: 0x001DF673
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x001E1487 File Offset: 0x001DF687
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x001E1495 File Offset: 0x001DF695
		public override int ReadByte()
		{
			return this.stream.ReadByte();
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x001E14A2 File Offset: 0x001DF6A2
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x001E14B6 File Offset: 0x001DF6B6
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x001E14C4 File Offset: 0x001DF6C4
		public override void WriteByte(byte value)
		{
			this.stream.WriteByte(value);
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x001E14D2 File Offset: 0x001DF6D2
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x001E14E4 File Offset: 0x001DF6E4
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x001E14F6 File Offset: 0x001DF6F6
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.stream.FlushAsync(cancellationToken);
		}

		// Token: 0x04003CDB RID: 15579
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposedValue;

		// Token: 0x04003CDC RID: 15580
		[PublicizedFrom(EAccessModifier.Private)]
		public Stream stream;

		// Token: 0x04003CDD RID: 15581
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManagerBase sdm;
	}
}
