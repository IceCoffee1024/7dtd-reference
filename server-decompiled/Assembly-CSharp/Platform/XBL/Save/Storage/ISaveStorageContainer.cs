using System;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C4B RID: 7243
	public interface ISaveStorageContainer : IDisposable
	{
		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x0600D6B5 RID: 54965
		bool IsDisposed { get; }

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x0600D6B6 RID: 54966
		string Name { get; }

		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x0600D6B7 RID: 54967
		DateTime LastAccessed { get; }

		// Token: 0x0600D6B8 RID: 54968
		void Flush(bool waitForFlush);

		// Token: 0x0600D6B9 RID: 54969
		bool TryEnumerateBlobInfos(out XGameSaveBlobInfo[] blobInfos);

		// Token: 0x0600D6BA RID: 54970
		XGameSaveBlobInfo GetBlobInfo(string blobName);

		// Token: 0x0600D6BB RID: 54971 RVA: 0x004D4D34 File Offset: 0x004D2F34
		RefCountedBuffer GetBlob(string blobName, StringSpan debugIdentifier)
		{
			return this.GetBlobs(new string[] { blobName }, debugIdentifier)[0];
		}

		// Token: 0x0600D6BC RID: 54972
		RefCountedBuffer[] GetBlobs(string[] blobNames, StringSpan debugIdentifier);

		// Token: 0x0600D6BD RID: 54973
		void SetBlob(string blobName, RefCountedBuffer blobData);

		// Token: 0x0600D6BE RID: 54974
		void DeleteBlob(string blobName);

		// Token: 0x0400A37E RID: 41854
		public const string RootSaveContainerName = "root";
	}
}
