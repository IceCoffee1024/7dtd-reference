using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001C87 RID: 7303
	public class SaveGameProvider : SaveGameIOProviderFixedRoot, IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D837 RID: 55351 RVA: 0x004DB6C4 File Offset: 0x004D98C4
		public SaveGameProvider()
			: base(null)
		{
		}

		// Token: 0x0600D838 RID: 55352 RVA: 0x004DB6CD File Offset: 0x004D98CD
		public SaveGameProvider(string rootPath, long maxStorageSizeBytes = 0L)
			: base(rootPath)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
			this.UpdateSizes();
		}

		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x0600D839 RID: 55353 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public ESaveGameProviderStatus Status
		{
			get
			{
				return ESaveGameProviderStatus.Ok;
			}
		}

		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x0600D83A RID: 55354 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public IPlatformSaveGameIOProvider Cache
		{
			get
			{
				return null;
			}
		}

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x0600D83B RID: 55355 RVA: 0x004DB6E3 File Offset: 0x004D98E3
		// (remove) Token: 0x0600D83C RID: 55356 RVA: 0x000027FC File Offset: 0x000009FC
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

		// Token: 0x0600D83D RID: 55357 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D83E RID: 55358 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D83F RID: 55359 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool ShouldBackup()
		{
			return true;
		}

		// Token: 0x0600D840 RID: 55360 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public bool ShouldCommit()
		{
			return true;
		}

		// Token: 0x0600D841 RID: 55361 RVA: 0x004C60E5 File Offset: 0x004C42E5
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600D842 RID: 55362 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600D843 RID: 55363 RVA: 0x004DB6EB File Offset: 0x004D98EB
		public bool ShouldLimitSize()
		{
			return !string.IsNullOrEmpty(this.m_rootPath) && this.m_maxStorageSizeBytes > 0L;
		}

		// Token: 0x0600D844 RID: 55364 RVA: 0x004DB708 File Offset: 0x004D9908
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

		// Token: 0x0600D845 RID: 55365 RVA: 0x004DB78C File Offset: 0x004D998C
		public SaveDataSizes GetSizes()
		{
			return this.m_currentSize;
		}

		// Token: 0x0400A3ED RID: 41965
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A3EE RID: 41966
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_currentSize;
	}
}
