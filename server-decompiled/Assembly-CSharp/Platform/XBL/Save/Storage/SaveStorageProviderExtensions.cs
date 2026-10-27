using System;
using Platform.XBL.Save.Storage.Blobs;
using Platform.XBL.Save.Storage.Files;

namespace Platform.XBL.Save.Storage
{
	// Token: 0x02001CB2 RID: 7346
	public static class SaveStorageProviderExtensions
	{
		// Token: 0x0600D954 RID: 55636 RVA: 0x004DD3D8 File Offset: 0x004DB5D8
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
