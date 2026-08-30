using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

// Token: 0x02000A15 RID: 2581
public abstract class SaveDataManagerBase : ISaveDataManager
{
	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06004DBB RID: 19899 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004DBC RID: 19900
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract Stream GetStream(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004DBD RID: 19901
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ReturnStream(Stream stream);

	// Token: 0x06004DBE RID: 19902 RVA: 0x001DC1A4 File Offset: 0x001DA3A4
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

	// Token: 0x06004DBF RID: 19903 RVA: 0x001DC1F4 File Offset: 0x001DA3F4
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

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001DC244 File Offset: 0x001DA444
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

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001DC2A8 File Offset: 0x001DA4A8
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

	// Token: 0x06004DC2 RID: 19906 RVA: 0x001DC344 File Offset: 0x001DA544
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string text)
	{
		Log.Out("[SaveDataManagerBase] " + text);
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Init()
	{
	}

	// Token: 0x06004DC4 RID: 19908 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004DC5 RID: 19909 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004DC6 RID: 19910 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004DC9 RID: 19913 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004DCA RID: 19914 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004DCB RID: 19915 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CommitSync()
	{
	}

	// Token: 0x06004DCC RID: 19916 RVA: 0x001DC356 File Offset: 0x001DA556
	public virtual IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x06004DCD RID: 19917 RVA: 0x001DC360 File Offset: 0x001DA560
	// (remove) Token: 0x06004DCE RID: 19918 RVA: 0x001DC398 File Offset: 0x001DA598
	public event Action CommitStarted;

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06004DCF RID: 19919 RVA: 0x001DC3D0 File Offset: 0x001DA5D0
	// (remove) Token: 0x06004DD0 RID: 19920 RVA: 0x001DC408 File Offset: 0x001DA608
	public event Action CommitFinished;

	// Token: 0x06004DD1 RID: 19921 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004DD2 RID: 19922 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004DD3 RID: 19923 RVA: 0x001DC440 File Offset: 0x001DA640
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004DD4 RID: 19924 RVA: 0x001DC456 File Offset: 0x001DA656
	public Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return new SaveDataManagerBase.WrappedStream(this, path, mode, access, share);
	}

	// Token: 0x06004DD5 RID: 19925 RVA: 0x001DC463 File Offset: 0x001DA663
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		SaveDataManager_Placeholder.Instance.ManagedFileDelete(path);
	}

	// Token: 0x06004DD6 RID: 19926 RVA: 0x001DC470 File Offset: 0x001DA670
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileExists(path);
	}

	// Token: 0x06004DD7 RID: 19927 RVA: 0x001DC47D File Offset: 0x001DA67D
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004DD8 RID: 19928 RVA: 0x001DC48A File Offset: 0x001DA68A
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004DD9 RID: 19929 RVA: 0x001DC497 File Offset: 0x001DA697
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004DDA RID: 19930 RVA: 0x001DC4A4 File Offset: 0x001DA6A4
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryExists(path);
	}

	// Token: 0x06004DDB RID: 19931 RVA: 0x001DC4B1 File Offset: 0x001DA6B1
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004DDC RID: 19932 RVA: 0x001DC4C0 File Offset: 0x001DA6C0
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004DDD RID: 19933 RVA: 0x001DC4CF File Offset: 0x001DA6CF
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004DDE RID: 19934 RVA: 0x001DC4DE File Offset: 0x001DA6DE
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataManager_Placeholder.Instance.ManagedDirectoryDelete(path, recursive);
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x001DC4EC File Offset: 0x001DA6EC
	public virtual long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileInfoLength(path);
	}

	// Token: 0x06004DE0 RID: 19936 RVA: 0x001DC4F9 File Offset: 0x001DA6F9
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001DC508 File Offset: 0x001DA708
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004DE2 RID: 19938 RVA: 0x001DC517 File Offset: 0x001DA717
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption);
	}

	// Token: 0x06004DE3 RID: 19939 RVA: 0x001DC526 File Offset: 0x001DA726
	[PublicizedFrom(EAccessModifier.Protected)]
	public SaveDataManagerBase()
	{
	}

	// Token: 0x06004DE4 RID: 19940 RVA: 0x001DC539 File Offset: 0x001DA739
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

	// Token: 0x06004DE5 RID: 19941 RVA: 0x001DC54B File Offset: 0x001DA74B
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

	// Token: 0x04003C15 RID: 15381
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_commitProgressLock = new object();

	// Token: 0x04003C16 RID: 15382
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x04003C17 RID: 15383
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressChanged;

	// Token: 0x04003C18 RID: 15384
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressLast;

	// Token: 0x04003C19 RID: 15385
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressHasTask;

	// Token: 0x02000A16 RID: 2582
	[PublicizedFrom(EAccessModifier.Private)]
	public class WrappedStream : Stream, IDisposable
	{
		// Token: 0x06004DE6 RID: 19942 RVA: 0x001DC55D File Offset: 0x001DA75D
		public WrappedStream(SaveDataManagerBase sdm, SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
		{
			this.sdm = sdm;
			this.stream = sdm.GetStream(path, mode, access, share);
		}

		// Token: 0x06004DE7 RID: 19943 RVA: 0x001DC57E File Offset: 0x001DA77E
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

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x001DC5AA File Offset: 0x001DA7AA
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06004DE9 RID: 19945 RVA: 0x001DC5B7 File Offset: 0x001DA7B7
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06004DEA RID: 19946 RVA: 0x001DC5C4 File Offset: 0x001DA7C4
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06004DEB RID: 19947 RVA: 0x001DC5D1 File Offset: 0x001DA7D1
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001DC5DE File Offset: 0x001DA7DE
		// (set) Token: 0x06004DED RID: 19949 RVA: 0x001DC5EB File Offset: 0x001DA7EB
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

		// Token: 0x06004DEE RID: 19950 RVA: 0x001DC5F9 File Offset: 0x001DA7F9
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x001DC606 File Offset: 0x001DA806
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x001DC616 File Offset: 0x001DA816
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x001DC625 File Offset: 0x001DA825
		public override void SetLength(long value)
		{
			this.stream.SetLength(value);
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x001DC633 File Offset: 0x001DA833
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x06004DF3 RID: 19955 RVA: 0x001DC643 File Offset: 0x001DA843
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06004DF4 RID: 19956 RVA: 0x001DC657 File Offset: 0x001DA857
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x001DC665 File Offset: 0x001DA865
		public override int ReadByte()
		{
			return this.stream.ReadByte();
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x001DC672 File Offset: 0x001DA872
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x001DC686 File Offset: 0x001DA886
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x001DC694 File Offset: 0x001DA894
		public override void WriteByte(byte value)
		{
			this.stream.WriteByte(value);
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x001DC6A2 File Offset: 0x001DA8A2
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x001DC6B4 File Offset: 0x001DA8B4
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x001DC6C6 File Offset: 0x001DA8C6
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.stream.FlushAsync(cancellationToken);
		}

		// Token: 0x04003C1C RID: 15388
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposedValue;

		// Token: 0x04003C1D RID: 15389
		[PublicizedFrom(EAccessModifier.Private)]
		public Stream stream;

		// Token: 0x04003C1E RID: 15390
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManagerBase sdm;
	}
}
