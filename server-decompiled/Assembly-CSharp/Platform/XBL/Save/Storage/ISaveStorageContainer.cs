using System;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C7D RID: 7293
	public interface ISaveStorageContainer : IDisposable
	{
		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x0600D82E RID: 55342
		bool IsDisposed { get; }

		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x0600D82F RID: 55343
		string Name { get; }

		// Token: 0x17001B3E RID: 6974
		// (get) Token: 0x0600D830 RID: 55344
		DateTime LastAccessed { get; }

		// Token: 0x0600D831 RID: 55345
		void Flush(bool waitForFlush);

		// Token: 0x0600D832 RID: 55346
		bool TryEnumerateBlobInfos(out XGameSaveBlobInfo[] blobInfos);

		// Token: 0x0600D833 RID: 55347
		XGameSaveBlobInfo GetBlobInfo(string blobName);

		// Token: 0x0600D834 RID: 55348 RVA: 0x004D9870 File Offset: 0x004D7A70
		RefCountedBuffer GetBlob(string blobName, StringSpan debugIdentifier)
		{
			return this.GetBlobs(new string[] { blobName }, debugIdentifier)[0];
		}

		// Token: 0x0600D835 RID: 55349
		RefCountedBuffer[] GetBlobs(string[] blobNames, StringSpan debugIdentifier);

		// Token: 0x0600D836 RID: 55350
		void SetBlob(string blobName, RefCountedBuffer blobData);

		// Token: 0x0600D837 RID: 55351
		void DeleteBlob(string blobName);

		// Token: 0x0400A453 RID: 42067
		public const string RootSaveContainerName = "root";
	}
}
