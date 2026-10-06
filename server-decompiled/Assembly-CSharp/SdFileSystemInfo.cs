using System;
using System.IO;

// Token: 0x02000A14 RID: 2580
public abstract class SdFileSystemInfo
{
	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x06004D18 RID: 19736 RVA: 0x001DBB3B File Offset: 0x001D9D3B
	// (set) Token: 0x06004D19 RID: 19737 RVA: 0x001DBB43 File Offset: 0x001D9D43
	public bool IsManaged
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06004D1A RID: 19738 RVA: 0x001DBB4C File Offset: 0x001D9D4C
	// (set) Token: 0x06004D1B RID: 19739 RVA: 0x001DBB54 File Offset: 0x001D9D54
	public SaveDataManagedPath ManagedPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004D1C RID: 19740 RVA: 0x001DBB5D File Offset: 0x001D9D5D
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo)
	{
		this.Reinitialize(fileSystemInfo);
	}

	// Token: 0x06004D1D RID: 19741 RVA: 0x001DBB6C File Offset: 0x001D9D6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo, SaveDataManagedPath managedPath)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		this.IsManaged = true;
		this.ManagedPath = managedPath;
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x001DBB8C File Offset: 0x001D9D8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Reinitialize(FileSystemInfo fileSystemInfo)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		SaveDataManagedPath saveDataManagedPath;
		this.IsManaged = SaveDataUtils.TryGetManagedPath(fileSystemInfo.FullName, out saveDataManagedPath);
		this.ManagedPath = saveDataManagedPath;
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06004D1F RID: 19743 RVA: 0x001DBBBA File Offset: 0x001D9DBA
	public virtual string FullName
	{
		get
		{
			return this.m_fileSystemInfo.FullName;
		}
	}

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x06004D20 RID: 19744 RVA: 0x001DBBC7 File Offset: 0x001D9DC7
	public string Extension
	{
		get
		{
			return this.m_fileSystemInfo.Extension;
		}
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x06004D21 RID: 19745
	public abstract string Name { get; }

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x06004D22 RID: 19746
	public abstract bool Exists { get; }

	// Token: 0x06004D23 RID: 19747
	public abstract void Delete();

	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06004D24 RID: 19748 RVA: 0x001DBBD4 File Offset: 0x001D9DD4
	public DateTime LastWriteTime
	{
		get
		{
			return this.LastWriteTimeUtc.ToLocalTime();
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06004D25 RID: 19749 RVA: 0x001DBBEF File Offset: 0x001D9DEF
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

	// Token: 0x06004D26 RID: 19750 RVA: 0x001DBC29 File Offset: 0x001D9E29
	public void Refresh()
	{
		this.m_fileSystemInfo.Refresh();
		this.Reinitialize(this.m_fileSystemInfo);
	}

	// Token: 0x04003C18 RID: 15384
	[PublicizedFrom(EAccessModifier.Private)]
	public FileSystemInfo m_fileSystemInfo;
}
