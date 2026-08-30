using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

// Token: 0x02000A26 RID: 2598
public abstract class SaveDataManagerBase : ISaveDataManager
{
	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06004E41 RID: 20033 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AppliesSaveSizeLimit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004E42 RID: 20034
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract Stream GetStream(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share);

	// Token: 0x06004E43 RID: 20035
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void ReturnStream(Stream stream);

	// Token: 0x06004E44 RID: 20036 RVA: 0x001DEC3C File Offset: 0x001DCE3C
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

	// Token: 0x06004E45 RID: 20037 RVA: 0x001DEC8C File Offset: 0x001DCE8C
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

	// Token: 0x06004E46 RID: 20038 RVA: 0x001DECDC File Offset: 0x001DCEDC
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

	// Token: 0x06004E47 RID: 20039 RVA: 0x001DED40 File Offset: 0x001DCF40
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

	// Token: 0x06004E48 RID: 20040 RVA: 0x001DEDDC File Offset: 0x001DCFDC
	[Conditional("DEBUG_SAVE_DATA_MANAGER")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void DebugLog(string text)
	{
		Log.Out("[SaveDataManagerBase] " + text);
	}

	// Token: 0x06004E49 RID: 20041 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Init()
	{
	}

	// Token: 0x06004E4A RID: 20042 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual SaveDataWriteMode GetWriteMode()
	{
		return SaveDataWriteMode.None;
	}

	// Token: 0x06004E4B RID: 20043 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetWriteMode(SaveDataWriteMode writeMode)
	{
	}

	// Token: 0x06004E4C RID: 20044 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x06004E4D RID: 20045 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void RegisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E4E RID: 20046 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void DeregisterRegionFileManager(RegionFileManager regionFileManager)
	{
	}

	// Token: 0x06004E4F RID: 20047 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int CommitAsync()
	{
		return 0;
	}

	// Token: 0x06004E50 RID: 20048 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsCommitPending(int token)
	{
		return false;
	}

	// Token: 0x06004E51 RID: 20049 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CommitSync()
	{
	}

	// Token: 0x06004E52 RID: 20050 RVA: 0x001DEDEE File Offset: 0x001DCFEE
	public virtual IEnumerator CommitCoroutine(Action<double> progressFeedback = null)
	{
		yield break;
	}

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x06004E53 RID: 20051 RVA: 0x001DEDF8 File Offset: 0x001DCFF8
	// (remove) Token: 0x06004E54 RID: 20052 RVA: 0x001DEE30 File Offset: 0x001DD030
	public event Action CommitStarted;

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06004E55 RID: 20053 RVA: 0x001DEE68 File Offset: 0x001DD068
	// (remove) Token: 0x06004E56 RID: 20054 RVA: 0x001DEEA0 File Offset: 0x001DD0A0
	public event Action CommitFinished;

	// Token: 0x06004E57 RID: 20055 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ShouldLimitSize()
	{
		return false;
	}

	// Token: 0x06004E58 RID: 20056 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateSizes()
	{
	}

	// Token: 0x06004E59 RID: 20057 RVA: 0x001DEED8 File Offset: 0x001DD0D8
	public virtual SaveDataSizes GetSizes()
	{
		return default(SaveDataSizes);
	}

	// Token: 0x06004E5A RID: 20058 RVA: 0x001DEEEE File Offset: 0x001DD0EE
	public Stream ManagedFileOpen(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		return new SaveDataManagerBase.WrappedStream(this, path, mode, access, share);
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001DEEFB File Offset: 0x001DD0FB
	public virtual void ManagedFileDelete(SaveDataManagedPath path)
	{
		SaveDataManager_Placeholder.Instance.ManagedFileDelete(path);
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001DEF08 File Offset: 0x001DD108
	public virtual bool ManagedFileExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileExists(path);
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x001DEF15 File Offset: 0x001DD115
	public virtual DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001DEF22 File Offset: 0x001DD122
	public virtual SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryCreateDirectory(path);
	}

	// Token: 0x06004E5F RID: 20063 RVA: 0x001DEF2F File Offset: 0x001DD12F
	public virtual DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryGetLastWriteTimeUtc(path);
	}

	// Token: 0x06004E60 RID: 20064 RVA: 0x001DEF3C File Offset: 0x001DD13C
	public virtual bool ManagedDirectoryExists(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryExists(path);
	}

	// Token: 0x06004E61 RID: 20065 RVA: 0x001DEF49 File Offset: 0x001DD149
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x001DEF58 File Offset: 0x001DD158
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x001DEF67 File Offset: 0x001DD167
	public virtual IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryEnumerateFileSystemEntries(path, searchPattern, searchOption);
	}

	// Token: 0x06004E64 RID: 20068 RVA: 0x001DEF76 File Offset: 0x001DD176
	public virtual void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
	{
		SaveDataManager_Placeholder.Instance.ManagedDirectoryDelete(path, recursive);
	}

	// Token: 0x06004E65 RID: 20069 RVA: 0x001DEF84 File Offset: 0x001DD184
	public virtual long ManagedFileInfoLength(SaveDataManagedPath path)
	{
		return SaveDataManager_Placeholder.Instance.ManagedFileInfoLength(path);
	}

	// Token: 0x06004E66 RID: 20070 RVA: 0x001DEF91 File Offset: 0x001DD191
	public virtual IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateDirectories(path, searchPattern, searchOption);
	}

	// Token: 0x06004E67 RID: 20071 RVA: 0x001DEFA0 File Offset: 0x001DD1A0
	public virtual IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFiles(path, searchPattern, searchOption);
	}

	// Token: 0x06004E68 RID: 20072 RVA: 0x001DEFAF File Offset: 0x001DD1AF
	public virtual IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
	{
		return SaveDataManager_Placeholder.Instance.ManagedDirectoryInfoEnumerateFileSystemInfos(path, searchPattern, searchOption);
	}

	// Token: 0x06004E69 RID: 20073 RVA: 0x001DEFBE File Offset: 0x001DD1BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public SaveDataManagerBase()
	{
	}

	// Token: 0x06004E6A RID: 20074 RVA: 0x001DEFD1 File Offset: 0x001DD1D1
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

	// Token: 0x06004E6B RID: 20075 RVA: 0x001DEFE3 File Offset: 0x001DD1E3
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

	// Token: 0x04003CA6 RID: 15526
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_commitProgressLock = new object();

	// Token: 0x04003CA7 RID: 15527
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgress;

	// Token: 0x04003CA8 RID: 15528
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressChanged;

	// Token: 0x04003CA9 RID: 15529
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressLast;

	// Token: 0x04003CAA RID: 15530
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_commitInProgressHasTask;

	// Token: 0x02000A27 RID: 2599
	[PublicizedFrom(EAccessModifier.Private)]
	public class WrappedStream : Stream, IDisposable
	{
		// Token: 0x06004E6C RID: 20076 RVA: 0x001DEFF5 File Offset: 0x001DD1F5
		public WrappedStream(SaveDataManagerBase sdm, SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
		{
			this.sdm = sdm;
			this.stream = sdm.GetStream(path, mode, access, share);
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x001DF016 File Offset: 0x001DD216
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

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06004E6E RID: 20078 RVA: 0x001DF042 File Offset: 0x001DD242
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x001DF04F File Offset: 0x001DD24F
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x001DF05C File Offset: 0x001DD25C
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06004E71 RID: 20081 RVA: 0x001DF069 File Offset: 0x001DD269
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06004E72 RID: 20082 RVA: 0x001DF076 File Offset: 0x001DD276
		// (set) Token: 0x06004E73 RID: 20083 RVA: 0x001DF083 File Offset: 0x001DD283
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

		// Token: 0x06004E74 RID: 20084 RVA: 0x001DF091 File Offset: 0x001DD291
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x001DF09E File Offset: 0x001DD29E
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x001DF0AE File Offset: 0x001DD2AE
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x001DF0BD File Offset: 0x001DD2BD
		public override void SetLength(long value)
		{
			this.stream.SetLength(value);
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x001DF0CB File Offset: 0x001DD2CB
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x001DF0DB File Offset: 0x001DD2DB
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x001DF0EF File Offset: 0x001DD2EF
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x001DF0FD File Offset: 0x001DD2FD
		public override int ReadByte()
		{
			return this.stream.ReadByte();
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x001DF10A File Offset: 0x001DD30A
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x001DF11E File Offset: 0x001DD31E
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x001DF12C File Offset: 0x001DD32C
		public override void WriteByte(byte value)
		{
			this.stream.WriteByte(value);
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x001DF13A File Offset: 0x001DD33A
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001DF14C File Offset: 0x001DD34C
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this.stream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001DF15E File Offset: 0x001DD35E
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.stream.FlushAsync(cancellationToken);
		}

		// Token: 0x04003CAD RID: 15533
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposedValue;

		// Token: 0x04003CAE RID: 15534
		[PublicizedFrom(EAccessModifier.Private)]
		public Stream stream;

		// Token: 0x04003CAF RID: 15535
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataManagerBase sdm;
	}
}
