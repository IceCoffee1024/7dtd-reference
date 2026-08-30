using System;
using System.IO;

// Token: 0x02000A0A RID: 2570
public abstract class SdFileSystemInfo
{
	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x06004D49 RID: 19785 RVA: 0x001DA88B File Offset: 0x001D8A8B
	// (set) Token: 0x06004D4A RID: 19786 RVA: 0x001DA893 File Offset: 0x001D8A93
	public bool IsManaged
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x06004D4B RID: 19787 RVA: 0x001DA89C File Offset: 0x001D8A9C
	// (set) Token: 0x06004D4C RID: 19788 RVA: 0x001DA8A4 File Offset: 0x001D8AA4
	public SaveDataManagedPath ManagedPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004D4D RID: 19789 RVA: 0x001DA8AD File Offset: 0x001D8AAD
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo)
	{
		this.Reinitialize(fileSystemInfo);
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001DA8BC File Offset: 0x001D8ABC
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo, SaveDataManagedPath managedPath)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		this.IsManaged = true;
		this.ManagedPath = managedPath;
	}

	// Token: 0x06004D4F RID: 19791 RVA: 0x001DA8DC File Offset: 0x001D8ADC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Reinitialize(FileSystemInfo fileSystemInfo)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		SaveDataManagedPath saveDataManagedPath;
		this.IsManaged = SaveDataUtils.TryGetManagedPath(fileSystemInfo.FullName, out saveDataManagedPath);
		this.ManagedPath = saveDataManagedPath;
	}

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x06004D50 RID: 19792 RVA: 0x001DA90A File Offset: 0x001D8B0A
	public virtual string FullName
	{
		get
		{
			return this.m_fileSystemInfo.FullName;
		}
	}

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x06004D51 RID: 19793 RVA: 0x001DA917 File Offset: 0x001D8B17
	public string Extension
	{
		get
		{
			return this.m_fileSystemInfo.Extension;
		}
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x06004D52 RID: 19794
	public abstract string Name { get; }

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06004D53 RID: 19795
	public abstract bool Exists { get; }

	// Token: 0x06004D54 RID: 19796
	public abstract void Delete();

	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06004D55 RID: 19797 RVA: 0x001DA924 File Offset: 0x001D8B24
	public DateTime LastWriteTime
	{
		get
		{
			return this.LastWriteTimeUtc.ToLocalTime();
		}
	}

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x06004D56 RID: 19798 RVA: 0x001DA93F File Offset: 0x001D8B3F
	public DateTime LastWriteTimeUtc
	{
		get
		{
			if (!this.IsManaged)
			{
				return this.m_fileSystemInfo.LastWriteTimeUtc;
			}
			if (this.m_fileSystemInfo is DirectoryInfo)
			{
				return SdDirectory.ManagedGetLastWriteTimeUtc(this.ManagedPath);
			}
			return SdFile.ManagedGetLastWriteTimeUtc(this.ManagedPath);
		}
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x001DA979 File Offset: 0x001D8B79
	public void Refresh()
	{
		this.m_fileSystemInfo.Refresh();
		this.Reinitialize(this.m_fileSystemInfo);
	}

	// Token: 0x04003C0B RID: 15371
	[PublicizedFrom(EAccessModifier.Private)]
	public FileSystemInfo m_fileSystemInfo;
}
