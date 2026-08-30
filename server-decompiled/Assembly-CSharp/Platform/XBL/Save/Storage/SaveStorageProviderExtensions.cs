using System;
using Platform.XBL.Save.Storage.Blobs;
using Platform.XBL.Save.Storage.Files;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C4F RID: 7247
	public static class SaveStorageProviderExtensions
	{
		// Token: 0x0600D6C7 RID: 54983 RVA: 0x004D4D4C File Offset: 0x004D2F4C
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
