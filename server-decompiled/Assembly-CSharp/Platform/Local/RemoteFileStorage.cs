using System;
using System.IO;

namespace Platform.Local
{
	// Token: 0x02001CFB RID: 7419
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600DC44 RID: 56388 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x0600DC45 RID: 56389 RVA: 0x00020119 File Offset: 0x0001E319
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x0600DC46 RID: 56390 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool Unavailable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DC47 RID: 56391 RVA: 0x004E87E4 File Offset: 0x004E69E4
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

		// Token: 0x0600DC48 RID: 56392 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DC49 RID: 56393 RVA: 0x0008806C File Offset: 0x0008626C
		public bool CancelGetFile(string _filename, string _cancelReason)
		{
			throw new NotImplementedException();
		}
	}
}
