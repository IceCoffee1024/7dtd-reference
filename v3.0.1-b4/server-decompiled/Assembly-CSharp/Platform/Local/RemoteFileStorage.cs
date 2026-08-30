using System;
using System.IO;

namespace Platform.Local
{
	// Token: 0x02001CA2 RID: 7330
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600D92F RID: 55599 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x0600D930 RID: 55600 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600D931 RID: 55601 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool Unavailable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600D932 RID: 55602 RVA: 0x004DD688 File Offset: 0x004DB888
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

		// Token: 0x0600D933 RID: 55603 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
		{
			throw new NotImplementedException();
		}
	}
}
