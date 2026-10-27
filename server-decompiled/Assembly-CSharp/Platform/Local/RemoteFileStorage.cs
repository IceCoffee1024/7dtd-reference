using System;
using System.IO;

namespace Platform.Local
{
	// Token: 0x02001D2C RID: 7468
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600DD58 RID: 56664 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x0600DD59 RID: 56665 RVA: 0x00020119 File Offset: 0x0001E319
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C42 RID: 7234
		// (get) Token: 0x0600DD5A RID: 56666 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool Unavailable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DD5B RID: 56667 RVA: 0x004EC384 File Offset: 0x004EA584
		public void GetFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
		{
			if (_callback == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(_filename))
			{
				_callback(IRemoteFileStorage.EFileDownloadResult.EmptyFilename, null, null);
				return;
			}
			string text = GameIO.GetApplicationPath() + "/fakeRemoteFileStorage/" + _filename;
			if (!File.Exists(text))
			{
				_callback(IRemoteFileStorage.EFileDownloadResult.FileNotFound, "", null);
				return;
			}
			try
			{
				byte[] array = File.ReadAllBytes(text);
				_callback(IRemoteFileStorage.EFileDownloadResult.Ok, null, array);
			}
			catch (Exception ex)
			{
				Log.Error("[Local] ReadFile (" + _filename + ") failed:");
				Log.Exception(ex);
				_callback(IRemoteFileStorage.EFileDownloadResult.Other, ex.Message, null);
			}
		}

		// Token: 0x0600DD5C RID: 56668 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD5D RID: 56669 RVA: 0x000882A0 File Offset: 0x000864A0
		public bool CancelGetFile(string _filename, string _cancelReason)
		{
			throw new NotImplementedException();
		}
	}
}
