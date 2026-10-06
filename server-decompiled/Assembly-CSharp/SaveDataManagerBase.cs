using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

// Token: 0x02000A30 RID: 2608
public abstract class SaveDataManagerBase : ISaveDataManager
{
	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x06004E10 RID: 19984 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004E11 RID: 19985
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract Stream GetStream(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004E12 RID: 19986
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ReturnStream(Stream stream);

	// Token: 0x06004E13 RID: 19987 RVA: 0x001DFEEC File Offset: 0x001DE0EC
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

	// Token: 0x06004E14 RID: 19988 RVA: 0x001DFF3C File Offset: 0x001DE13C
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

	// Token: 0x06004E15 RID: 19989 RVA: 0x001DFF8C File Offset: 0x001DE18C
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

	// Token: 0x06004E16 RID: 19990 RVA: 0x001DFFF0 File Offset: 0x001DE1F0
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

	// Token: 0x06004E17 RID: 19991 RVA: 0x001E008C File Offset: 0x001DE28C
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string text)
	{
		Log.Out("[SaveDataManagerBase] " + text);
	}

	// Token: 0x06004E18 RID: 19992 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Init()
	{
	}

	// Token: 0x06004E19 RID: 19993 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x06004E1C RID: 19996 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E1D RID: 19997 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CommitSync()
	{
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x001E009E File Offset: 0x001DE29E
	public virtual IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06004E22 RID: 20002 RVA: 0x001E00A8 File Offset: 0x001DE2A8
	// (remove) Token: 0x06004E23 RID: 20003 RVA: 0x001E00E0 File Offset: 0x001DE2E0
	public event Action CommitStarted;

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06004E24 RID: 20004 RVA: 0x001E0118 File Offset: 0x001DE318
	// (remove) Token: 0x06004E25 RID: 20005 RVA: 0x001E0150 File Offset: 0x001DE350
	public event Action CommitFinished;

	// Token: 0x06004E26 RID: 20006 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004E27 RID: 20007 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001E0188 File Offset: 0x001DE388
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001E019E File Offset: 0x001DE39E
	public Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return new SaveDataManagerBase.WrappedStream(this, path, mode, access, share);
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001E01AB File Offset: 0x001DE3AB
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		SaveDataManager_Placeholder.Instance.ManagedFileDelete(path);
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x001E01B8 File Offset: 0x001DE3B8
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileExists(path);
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x001E01C5 File Offset: 0x001DE3C5
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E2D RID: 20013 RVA: 0x001E01D2 File Offset: 0x001DE3D2
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004E2E RID: 20014 RVA: 0x001E01DF File Offset: 0x001DE3DF
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E2F RID: 20015 RVA: 0x001E01EC File Offset: 0x001DE3EC
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryExists(path);
	}

	// Token: 0x06004E30 RID: 20016 RVA: 0x001E01F9 File Offset: 0x001DE3F9
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004E31 RID: 20017 RVA: 0x001E0208 File Offset: 0x001DE408
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004E32 RID: 20018 RVA: 0x001E0217 File Offset: 0x001DE417
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004E33 RID: 20019 RVA: 0x001E0226 File Offset: 0x001DE426
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataManager_Placeholder.Instance.ManagedDirectoryDelete(path, recursive);
	}

	// Token: 0x06004E34 RID: 20020 RVA: 0x001E0234 File Offset: 0x001DE434
	public virtual long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileInfoLength(path);
	}

	// Token: 0x06004E35 RID: 20021 RVA: 0x001E0241 File Offset: 0x001DE441
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004E36 RID: 20022 RVA: 0x001E0250 File Offset: 0x001DE450
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004E37 RID: 20023 RVA: 0x001E025F File Offset: 0x001DE45F
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption);
	}

	// Token: 0x06004E38 RID: 20024 RVA: 0x001E026E File Offset: 0x001DE46E
	[PublicizedFrom(EAccessModifier.Protected)]
	public SaveDataManagerBase()
	{
	}

	// Token: 0x06004E39 RID: 20025 RVA: 0x001E0281 File Offset: 0x001DE481
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

	// Token: 0x06004E3A RID: 20026 RVA: 0x001E0293 File Offset: 0x001DE493
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

	// Token: 0x04003CB3 RID: 15539
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_commitProgressLock = new object();

	// Token: 0x04003CB4 RID: 15540
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x04003CB5 RID: 15541
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressChanged;

	// Token: 0x04003CB6 RID: 15542
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressLast;

	// Token: 0x04003CB7 RID: 15543
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressHasTask;

	// Token: 0x02000A31 RID: 2609
	[PublicizedFrom(EAccessModifier.Private)]
	public class WrappedStream : Stream, IDisposable
	{
		// Token: 0x06004E3B RID: 20027 RVA: 0x001E02A5 File Offset: 0x001DE4A5
		public WrappedStream(SaveDataManagerBase sdm, SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
		{
			this.sdm = sdm;
			this.stream = sdm.GetStream(path, mode, access, share);
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x001E02C6 File Offset: 0x001DE4C6
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

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06004E3D RID: 20029 RVA: 0x001E02F2 File Offset: 0x001DE4F2
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x001E02FF File Offset: 0x001DE4FF
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06004E3F RID: 20031 RVA: 0x001E030C File Offset: 0x001DE50C
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06004E40 RID: 20032 RVA: 0x001E0319 File Offset: 0x001DE519
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06004E41 RID: 20033 RVA: 0x001E0326 File Offset: 0x001DE526
		// (set) Token: 0x06004E42 RID: 20034 RVA: 0x001E0333 File Offset: 0x001DE533
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

		// Token: 0x06004E43 RID: 20035 RVA: 0x001E0341 File Offset: 0x001DE541
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x001E034E File Offset: 0x001DE54E
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x001E035E File Offset: 0x001DE55E
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x001E036D File Offset: 0x001DE56D
		public override void SetLength(long value)
		{
			this.stream.SetLength(value);
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x001E037B File Offset: 0x001DE57B
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x001E038B File Offset: 0x001DE58B
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x001E039F File Offset: 0x001DE59F
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x001E03AD File Offset: 0x001DE5AD
		public override int ReadByte()
		{
			return this.stream.ReadByte();
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x001E03BA File Offset: 0x001DE5BA
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x001E03CE File Offset: 0x001DE5CE
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x001E03DC File Offset: 0x001DE5DC
		public override void WriteByte(byte value)
		{
			this.stream.WriteByte(value);
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x001E03EA File Offset: 0x001DE5EA
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x001E03FC File Offset: 0x001DE5FC
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x001E040E File Offset: 0x001DE60E
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.stream.FlushAsync(cancellationToken);
		}

		// Token: 0x04003CBA RID: 15546
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposedValue;

		// Token: 0x04003CBB RID: 15547
		[PublicizedFrom(EAccessModifier.Private)]
		public Stream stream;

		// Token: 0x04003CBC RID: 15548
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManagerBase sdm;
	}
}
