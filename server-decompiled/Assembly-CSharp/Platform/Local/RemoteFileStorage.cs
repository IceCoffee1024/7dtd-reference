using System;
using System.IO;

namespace Platform.Local
{
	// Token: 0x02001CC9 RID: 7369
	public class RemoteFileStorage : IRemoteFileStorage
	{
		// Token: 0x0600DAC9 RID: 56009 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x0600DACA RID: 56010 RVA: 0x00020021 File Offset: 0x0001E221
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x0600DACB RID: 56011 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool Unavailable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DACC RID: 56012 RVA: 0x004E3C9C File Offset: 0x004E1E9C
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

		// Token: 0x0600DACD RID: 56013 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetCachedFile(string _filename, IRemoteFileStorage.FileDownloadCompleteCallback _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DACE RID: 56014 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public bool CancelGetFile(string _filename, string _cancelReason)
		{
			throw new NotImplementedException();
		}
	}
}
