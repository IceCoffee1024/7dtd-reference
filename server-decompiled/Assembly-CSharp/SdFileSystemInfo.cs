using System;
using System.IO;

// Token: 0x02000A1E RID: 2590
public abstract class SdFileSystemInfo
{
	// Token: 0x1700085B RID: 2139
	// (get) Token: 0x06004D3C RID: 19772 RVA: 0x001DCC23 File Offset: 0x001DAE23
	// (set) Token: 0x06004D3D RID: 19773 RVA: 0x001DCC2B File Offset: 0x001DAE2B
	public bool IsManaged
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06004D3E RID: 19774 RVA: 0x001DCC34 File Offset: 0x001DAE34
	// (set) Token: 0x06004D3F RID: 19775 RVA: 0x001DCC3C File Offset: 0x001DAE3C
	public SaveDataManagedPath ManagedPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004D40 RID: 19776 RVA: 0x001DCC45 File Offset: 0x001DAE45
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo)
	{
		this.Reinitialize(fileSystemInfo);
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x001DCC54 File Offset: 0x001DAE54
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo, SaveDataManagedPath managedPath)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		this.IsManaged = true;
		this.ManagedPath = managedPath;
	}

	// Token: 0x06004D42 RID: 19778 RVA: 0x001DCC74 File Offset: 0x001DAE74
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Reinitialize(FileSystemInfo fileSystemInfo)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		SaveDataManagedPath saveDataManagedPath;
		this.IsManaged = SaveDataUtils.TryGetManagedPath(fileSystemInfo.FullName, out saveDataManagedPath);
		this.ManagedPath = saveDataManagedPath;
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x06004D43 RID: 19779 RVA: 0x001DCCA2 File Offset: 0x001DAEA2
	public virtual string FullName
	{
		get
		{
			return this.m_fileSystemInfo.FullName;
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x06004D44 RID: 19780 RVA: 0x001DCCAF File Offset: 0x001DAEAF
	public string Extension
	{
		get
		{
			return this.m_fileSystemInfo.Extension;
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x06004D45 RID: 19781
	public abstract string Name { get; }

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06004D46 RID: 19782
	public abstract bool Exists { get; }

	// Token: 0x06004D47 RID: 19783
	public abstract void Delete();

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06004D48 RID: 19784 RVA: 0x001DCCBC File Offset: 0x001DAEBC
	public DateTime LastWriteTime
	{
		get
		{
			return this.LastWriteTimeUtc.ToLocalTime();
		}
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06004D49 RID: 19785 RVA: 0x001DCCD7 File Offset: 0x001DAED7
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

	// Token: 0x06004D4A RID: 19786 RVA: 0x001DCD11 File Offset: 0x001DAF11
	public void Refresh()
	{
		this.m_fileSystemInfo.Refresh();
		this.Reinitialize(this.m_fileSystemInfo);
	}

	// Token: 0x04003C39 RID: 15417
	[PublicizedFrom(EAccessModifier.Private)]
	public FileSystemInfo m_fileSystemInfo;
}
