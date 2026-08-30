using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CAE RID: 7342
	public class SaveGameProvider : SaveGameIOProviderFixedRoot, IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D9CD RID: 55757 RVA: 0x004E1CA0 File Offset: 0x004DFEA0
		public SaveGameProvider()
			: base(null)
		{
		}

		// Token: 0x0600D9CE RID: 55758 RVA: 0x004E1CA9 File Offset: 0x004DFEA9
		public SaveGameProvider(string rootPath, long maxStorageSizeBytes = 0L)
			: base(rootPath)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
			this.UpdateSizes();
		}

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x0600D9CF RID: 55759 RVA: 0x00020021 File Offset: 0x0001E221
		public ESaveGameProviderStatus Status
		{
			get
			{
				return ESaveGameProviderStatus.Ok;
			}
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x0600D9D0 RID: 55760 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public IPlatformSaveGameIOProvider Cache
		{
			get
			{
				return null;
			}
		}

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x0600D9D1 RID: 55761 RVA: 0x004E1CBF File Offset: 0x004DFEBF
		// (remove) Token: 0x0600D9D2 RID: 55762 RVA: 0x000027FC File Offset: 0x000009FC
		public event Action Initialized
		{
			add
			{
				value();
			}
			remove
			{
			}
		}

		// Token: 0x0600D9D3 RID: 55763 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D9D4 RID: 55764 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D9D5 RID: 55765 RVA: 0x00020021 File Offset: 0x0001E221
		public bool ShouldBackup()
		{
			return true;
		}

		// Token: 0x0600D9D6 RID: 55766 RVA: 0x00020021 File Offset: 0x0001E221
		public bool ShouldCommit()
		{
			return true;
		}

		// Token: 0x0600D9D7 RID: 55767 RVA: 0x004CC4D9 File Offset: 0x004CA6D9
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600D9D8 RID: 55768 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600D9D9 RID: 55769 RVA: 0x004E1CC7 File Offset: 0x004DFEC7
		public bool ShouldLimitSize()
		{
			return !string.IsNullOrEmpty(this.m_rootPath) && this.m_maxStorageSizeBytes > 0L;
		}

		// Token: 0x0600D9DA RID: 55770 RVA: 0x004E1CE4 File Offset: 0x004DFEE4
		public void UpdateSizes()
		{
			if (this.ShouldLimitSize())
			{
				long num = 0L;
				foreach (string text in Directory.EnumerateFiles(this.m_rootPath, "*", SearchOption.AllDirectories))
				{
					FileInfo fileInfo = new FileInfo(text);
					num += fileInfo.Length;
				}
				this.m_currentSize = new SaveDataSizes(this.m_maxStorageSizeBytes, this.m_maxStorageSizeBytes - num);
			}
		}

		// Token: 0x0600D9DB RID: 55771 RVA: 0x004E1D68 File Offset: 0x004DFF68
		public SaveDataSizes GetSizes()
		{
			return this.m_currentSize;
		}

		// Token: 0x0400A520 RID: 42272
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A521 RID: 42273
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_currentSize;
	}
}
