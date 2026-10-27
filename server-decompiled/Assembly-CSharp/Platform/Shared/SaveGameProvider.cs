using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001D11 RID: 7441
	public class SaveGameProvider : SaveGameIOProviderFixedRoot, IPlatformSaveGameProvider, IPlatformSaveGameIOProvider
	{
		// Token: 0x0600DC5B RID: 56411 RVA: 0x004EA388 File Offset: 0x004E8588
		public SaveGameProvider()
			: base(null)
		{
		}

		// Token: 0x0600DC5C RID: 56412 RVA: 0x004EA391 File Offset: 0x004E8591
		public SaveGameProvider(string rootPath, long maxStorageSizeBytes = 0L)
			: base(rootPath)
		{
			this.m_maxStorageSizeBytes = maxStorageSizeBytes;
			this.UpdateSizes();
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x0600DC5D RID: 56413 RVA: 0x00020119 File Offset: 0x0001E319
		public ESaveGameProviderStatus Status
		{
			get
			{
				return ESaveGameProviderStatus.Ok;
			}
		}

		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x0600DC5E RID: 56414 RVA: 0x000200DA File Offset: 0x0001E2DA
		public IPlatformSaveGameIOProvider Cache
		{
			get
			{
				return null;
			}
		}

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x0600DC5F RID: 56415 RVA: 0x004EA3A7 File Offset: 0x004E85A7
		// (remove) Token: 0x0600DC60 RID: 56416 RVA: 0x000027FC File Offset: 0x000009FC
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

		// Token: 0x0600DC61 RID: 56417 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DC62 RID: 56418 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DC63 RID: 56419 RVA: 0x00020119 File Offset: 0x0001E319
		public bool ShouldBackup()
		{
			return true;
		}

		// Token: 0x0600DC64 RID: 56420 RVA: 0x00020119 File Offset: 0x0001E319
		public bool ShouldCommit()
		{
			return true;
		}

		// Token: 0x0600DC65 RID: 56421 RVA: 0x004D4AE9 File Offset: 0x004D2CE9
		public double GetCommitProgress()
		{
			return 1.0;
		}

		// Token: 0x0600DC66 RID: 56422 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600DC67 RID: 56423 RVA: 0x004EA3AF File Offset: 0x004E85AF
		public bool ShouldLimitSize()
		{
			return !string.IsNullOrEmpty(this.m_rootPath) && this.m_maxStorageSizeBytes > 0L;
		}

		// Token: 0x0600DC68 RID: 56424 RVA: 0x004EA3CC File Offset: 0x004E85CC
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

		// Token: 0x0600DC69 RID: 56425 RVA: 0x004EA450 File Offset: 0x004E8650
		public SaveDataSizes GetSizes()
		{
			return this.m_currentSize;
		}

		// Token: 0x0400A6DE RID: 42718
		[PublicizedFrom(EAccessModifier.Private)]
		public long m_maxStorageSizeBytes;

		// Token: 0x0400A6DF RID: 42719
		[PublicizedFrom(EAccessModifier.Private)]
		public SaveDataSizes m_currentSize;
	}
}
