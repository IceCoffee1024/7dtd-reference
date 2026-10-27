using System;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001CAE RID: 7342
	public interface ISaveStorageContainer : IDisposable
	{
		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x0600D942 RID: 55618
		bool IsDisposed { get; }

		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x0600D943 RID: 55619
		string Name { get; }

		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x0600D944 RID: 55620
		DateTime LastAccessed { get; }

		// Token: 0x0600D945 RID: 55621
		void Flush(bool waitForFlush);

		// Token: 0x0600D946 RID: 55622
		bool TryEnumerateBlobInfos(out XGameSaveBlobInfo[] blobInfos);

		// Token: 0x0600D947 RID: 55623
		XGameSaveBlobInfo GetBlobInfo(string blobName);

		// Token: 0x0600D948 RID: 55624 RVA: 0x004DD3C0 File Offset: 0x004DB5C0
		RefCountedBuffer GetBlob(string blobName, StringSpan debugIdentifier)
		{
			return this.GetBlobs(new string[] { blobName }, debugIdentifier)[0];
		}

		// Token: 0x0600D949 RID: 55625
		RefCountedBuffer[] GetBlobs(string[] blobNames, StringSpan debugIdentifier);

		// Token: 0x0600D94A RID: 55626
		void SetBlob(string blobName, RefCountedBuffer blobData);

		// Token: 0x0600D94B RID: 55627
		void DeleteBlob(string blobName);

		// Token: 0x0400A53C RID: 42300
		public const string RootSaveContainerName = "root";
	}
}
