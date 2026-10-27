using System;

namespace Platform
{
	// Token: 0x02001BF6 RID: 7158
	public interface IRemoteFileStorage
	{
		// Token: 0x0600D588 RID: 54664
		void Init(IPlatform _owner);

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x0600D589 RID: 54665
		bool IsReady { get; }

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x0600D58A RID: 54666
		bool Unavailable { get; }

		// Token: 0x0600D58B RID: 54667
		void GetFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x0600D58C RID: 54668
		void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x0600D58D RID: 54669
		bool CancelGetFile(string _filename, string reason);

		// Token: 0x02001BF7 RID: 7159
		public enum EFileDownloadResult
		{
			// Token: 0x0400A28F RID: 41615
			Ok,
			// Token: 0x0400A290 RID: 41616
			EmptyFilename,
			// Token: 0x0400A291 RID: 41617
			FileNotFound,
			// Token: 0x0400A292 RID: 41618
			Other
		}

		// Token: 0x02001BF8 RID: 7160
		// (Invoke) Token: 0x0600D58F RID: 54671
		public delegate void FileDownloadCompleteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data);
	}
}
