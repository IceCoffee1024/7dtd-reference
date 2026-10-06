using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CE0 RID: 7392
	public class SaveGameProvider : SaveGameIOProviderFixedRoot, IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600DB47 RID: 56135 RVA: 0x004E67E8 File Offset: 0x004E49E8
		public SaveGameProvider()
			: base(null)
		{
		}

		// Token: 0x0600DB48 RID: 56136 RVA: 0x004E67F1 File Offset: 0x004E49F1
		public SaveGameProvider(string rootPath, long maxStorageSizeBytes = 0L)
			: base(rootPath)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
			this.UpdateSizes();
		}

		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x0600DB49 RID: 56137 RVA: 0x00020119 File Offset: 0x0001E319
		public ESaveGameProviderStatus Status
		{
			get
			{
				return ESaveGameProviderStatus.Ok;
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x0600DB4A RID: 56138 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IPlatformSaveGameIOProvider Cache
		{
			get
			{
				return null;
			}
		}

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x0600DB4B RID: 56139 RVA: 0x004E6807 File Offset: 0x004E4A07
		// (remove) Token: 0x0600DB4C RID: 56140 RVA: 0x000027FC File Offset: 0x000009FC
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

		// Token: 0x0600DB4D RID: 56141 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DB4E RID: 56142 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DB4F RID: 56143 RVA: 0x00020119 File Offset: 0x0001E319
		public bool ShouldBackup()
		{
			return true;
		}

		// Token: 0x0600DB50 RID: 56144 RVA: 0x00020119 File Offset: 0x0001E319
		public bool ShouldCommit()
		{
			return true;
		}

		// Token: 0x0600DB51 RID: 56145 RVA: 0x004D0FC1 File Offset: 0x004CF1C1
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600DB52 RID: 56146 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600DB53 RID: 56147 RVA: 0x004E680F File Offset: 0x004E4A0F
		public bool ShouldLimitSize()
		{
			return !string.IsNullOrEmpty(this.m_rootPath) && this.m_maxStorageSizeBytes > 0L;
		}

		// Token: 0x0600DB54 RID: 56148 RVA: 0x004E682C File Offset: 0x004E4A2C
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

		// Token: 0x0600DB55 RID: 56149 RVA: 0x004E68B0 File Offset: 0x004E4AB0
		public SaveDataSizes GetSizes()
		{
			return this.m_currentSize;
		}

		// Token: 0x0400A5F5 RID: 42485
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A5F6 RID: 42486
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_currentSize;
	}
}
