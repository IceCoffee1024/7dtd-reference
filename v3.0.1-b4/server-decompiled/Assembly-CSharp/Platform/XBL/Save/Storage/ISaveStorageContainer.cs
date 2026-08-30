using System;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C24 RID: 7204
	public interface ISaveStorageContainer : IDisposable
	{
		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x0600D525 RID: 54565
		bool IsDisposed { get; }

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x0600D526 RID: 54566
		string Name { get; }

		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x0600D527 RID: 54567
		DateTime LastAccessed { get; }

		// Token: 0x0600D528 RID: 54568
		void Flush(bool waitForFlush);

		// Token: 0x0600D529 RID: 54569
		bool TryEnumerateBlobInfos(out XGameSaveBlobInfo[] blobInfos);

		// Token: 0x0600D52A RID: 54570
		XGameSaveBlobInfo GetBlobInfo(string blobName);

		// Token: 0x0600D52B RID: 54571 RVA: 0x004CE910 File Offset: 0x004CCB10
		RefCountedBuffer GetBlob(string blobName, StringSpan debugIdentifier)
		{
			return this.GetBlobs(new string[] { blobName }, debugIdentifier)[0];
		}

		// Token: 0x0600D52C RID: 54572
		RefCountedBuffer[] GetBlobs(string[] blobNames, StringSpan debugIdentifier);

		// Token: 0x0600D52D RID: 54573
		void SetBlob(string blobName, RefCountedBuffer blobData);

		// Token: 0x0600D52E RID: 54574
		void DeleteBlob(string blobName);

		// Token: 0x0400A24D RID: 41549
		public const string RootSaveContainerName = "root";
	}
}
