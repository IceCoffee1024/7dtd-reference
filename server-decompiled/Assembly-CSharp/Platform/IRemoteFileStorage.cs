using System;

namespace Platform
{
	// Token: 0x02001BC5 RID: 7109
	public interface IRemoteFileStorage
	{
		// Token: 0x0600D474 RID: 54388
		void Init(IPlatform _owner);

		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x0600D475 RID: 54389
		bool IsReady { get; }

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600D476 RID: 54390
		bool Unavailable { get; }

		// Token: 0x0600D477 RID: 54391
		void GetFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x0600D478 RID: 54392
		void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x0600D479 RID: 54393
		bool CancelGetFile(string _filename, string reason);

		// Token: 0x02001BC6 RID: 7110
		public enum EFileDownloadResult
		{
			// Token: 0x0400A1A6 RID: 41382
			Ok,
			// Token: 0x0400A1A7 RID: 41383
			EmptyFilename,
			// Token: 0x0400A1A8 RID: 41384
			FileNotFound,
			// Token: 0x0400A1A9 RID: 41385
			Other
		}

		// Token: 0x02001BC7 RID: 7111
		// (Invoke) Token: 0x0600D47B RID: 54395
		public delegate void FileDownloadCompleteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data);
	}
}
