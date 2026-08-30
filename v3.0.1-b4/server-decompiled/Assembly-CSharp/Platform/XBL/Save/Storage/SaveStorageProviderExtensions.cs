using System;
using Platform.XBL.Save.Storage.Blobs;
using Platform.XBL.Save.Storage.Files;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001C28 RID: 7208
	public static class SaveStorageProviderExtensions
	{
		// Token: 0x0600D537 RID: 54583 RVA: 0x004CE928 File Offset: 0x004CCB28
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
