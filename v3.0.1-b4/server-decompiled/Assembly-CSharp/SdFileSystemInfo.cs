using System;
using System.IO;

// Token: 0x020009F9 RID: 2553
public abstract class SdFileSystemInfo
{
	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x001D80A3 File Offset: 0x001D62A3
	// (set) Token: 0x06004CC4 RID: 19652 RVA: 0x001D80AB File Offset: 0x001D62AB
	public bool IsManaged
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x001D80B4 File Offset: 0x001D62B4
	// (set) Token: 0x06004CC6 RID: 19654 RVA: 0x001D80BC File Offset: 0x001D62BC
	public SaveDataManagedPath ManagedPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004CC7 RID: 19655 RVA: 0x001D80C5 File Offset: 0x001D62C5
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo)
	{
		this.Reinitialize(fileSystemInfo);
	}

	// Token: 0x06004CC8 RID: 19656 RVA: 0x001D80D4 File Offset: 0x001D62D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public SdFileSystemInfo(FileSystemInfo fileSystemInfo, SaveDataManagedPath managedPath)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		this.IsManaged = true;
		this.ManagedPath = managedPath;
	}

	// Token: 0x06004CC9 RID: 19657 RVA: 0x001D80F4 File Offset: 0x001D62F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Reinitialize(FileSystemInfo fileSystemInfo)
	{
		this.m_fileSystemInfo = fileSystemInfo;
		SaveDataManagedPath saveDataManagedPath;
		this.IsManaged = SaveDataUtils.TryGetManagedPath(fileSystemInfo.FullName, out saveDataManagedPath);
		this.ManagedPath = saveDataManagedPath;
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x06004CCA RID: 19658 RVA: 0x001D8122 File Offset: 0x001D6322
	public virtual string FullName
	{
		get
		{
			return this.m_fileSystemInfo.FullName;
		}
	}

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x06004CCB RID: 19659 RVA: 0x001D812F File Offset: 0x001D632F
	public string Extension
	{
		get
		{
			return this.m_fileSystemInfo.Extension;
		}
	}

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x06004CCC RID: 19660
	public abstract string Name { get; }

	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x06004CCD RID: 19661
	public abstract bool Exists { get; }

	// Token: 0x06004CCE RID: 19662
	public abstract void Delete();

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x06004CCF RID: 19663 RVA: 0x001D813C File Offset: 0x001D633C
	public DateTime LastWriteTime
	{
		get
		{
			return this.LastWriteTimeUtc.ToLocalTime();
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x001D8157 File Offset: 0x001D6357
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

	// Token: 0x06004CD1 RID: 19665 RVA: 0x001D8191 File Offset: 0x001D6391
	public void Refresh()
	{
		this.m_fileSystemInfo.Refresh();
		this.Reinitialize(this.m_fileSystemInfo);
	}

	// Token: 0x04003B7A RID: 15226
	[PublicizedFrom(EAccessModifier.Private)]
	public FileSystemInfo m_fileSystemInfo;
}
