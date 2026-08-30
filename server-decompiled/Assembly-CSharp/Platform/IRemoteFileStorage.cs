using System;

namespace Platform
{
	// Token: 0x02001B93 RID: 7059
	public interface IRemoteFileStorage
	{
		// Token: 0x0600D2FC RID: 54012
		void Init(IPlatform _owner);

		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x0600D2FD RID: 54013
		bool IsReady { get; }

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x0600D2FE RID: 54014
		bool Unavailable { get; }

		// Token: 0x0600D2FF RID: 54015
		void GetFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x0600D300 RID: 54016
		void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x0600D301 RID: 54017
		bool CancelGetFile(string _filename, string reason);

		// Token: 0x02001B94 RID: 7060
		public enum EFileDownloadResult
		{
			// Token: 0x0400A0D1 RID: 41169
			Ok,
			// Token: 0x0400A0D2 RID: 41170
			EmptyFilename,
			// Token: 0x0400A0D3 RID: 41171
			FileNotFound,
			// Token: 0x0400A0D4 RID: 41172
			Other
		}

		// Token: 0x02001B95 RID: 7061
		// (Invoke) Token: 0x0600D303 RID: 54019
		public delegate void FileDownloadCompleteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data);
	}
}
