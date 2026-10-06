using System;
using Platform.XBL.Save.Storage.Blobs;
using Platform.XBL.Save.Storage.Files;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C81 RID: 7297
	public static class SaveStorageProviderExtensions
	{
		// Token: 0x0600D840 RID: 55360 RVA: 0x004D9888 File Offset: 0x004D7A88
		public static ISaveStorageProvider Create(this SaveStorageProvider storageProvider)
		{
			ISaveStorageProvider saveStorageProvider;
			if (storageProvider != SaveStorageProvider.Blobs)
			{
				if (storageProvider != SaveStorageProvider.Files)
				{
					throw new ArgumentOutOfRangeException("storageProvider", storageProvider, string.Format("Unknown {0}: {1}", "SaveStorageProvider", storageProvider));
				}
				saveStorageProvider = new SaveStorageStorageProviderFiles();
			}
			else
			{
				saveStorageProvider = new SaveStorageStorageProviderBlobs();
			}
			return saveStorageProvider;
		}
	}
}
