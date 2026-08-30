using System;

namespace Platform
{
	// Token: 0x02001B6C RID: 7020
	public interface IRemoteFileStorage
	{
		// Token: 0x0600D171 RID: 53617
		void Init(IPlatform _owner);

		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x0600D172 RID: 53618
		bool IsReady { get; }

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x0600D173 RID: 53619
		bool Unavailable { get; }

		// Token: 0x0600D174 RID: 53620
		void GetFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x0600D175 RID: 53621
		void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback);

		// Token: 0x02001B6D RID: 7021
		public enum EFileDownloadResult
		{
			// Token: 0x04009FA0 RID: 40864
			Ok,
			// Token: 0x04009FA1 RID: 40865
			EmptyFilename,
			// Token: 0x04009FA2 RID: 40866
			FileNotFound,
			// Token: 0x04009FA3 RID: 40867
			Other
		}

		// Token: 0x02001B6E RID: 7022
		// (Invoke) Token: 0x0600D177 RID: 53623
		public delegate void FileDownloadCompleteCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorName, byte[] _data);
	}
}
