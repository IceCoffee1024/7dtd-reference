using System;
using System.IO;

// Token: 0x02000A13 RID: 2579
public class SdFileInfo : SdFileSystemInfo
{
	// Token: 0x06004D01 RID: 19713 RVA: 0x001DB833 File Offset: 0x001D9A33
	public SdFileInfo(string fileName)
		: this(new FileInfo(fileName))
	{
	}

	// Token: 0x06004D02 RID: 19714 RVA: 0x001DB841 File Offset: 0x001D9A41
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(SaveDataManagedPath managedPath)
		: this(new FileInfo(managedPath.GetOriginalPath()), managedPath)
	{
	}

	// Token: 0x06004D03 RID: 19715 RVA: 0x001DB855 File Offset: 0x001D9A55
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(FileInfo fileInfo)
		: base(fileInfo)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004D04 RID: 19716 RVA: 0x001DB865 File Offset: 0x001D9A65
	[PublicizedFrom(EAccessModifier.Private)]
	public SdFileInfo(FileInfo fileInfo, SaveDataManagedPath managedPath)
		: base(fileInfo, managedPath)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004D05 RID: 19717 RVA: 0x001DB876 File Offset: 0x001D9A76
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reinitialize(FileInfo fileInfo)
	{
		base.Reinitialize(fileInfo);
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x06004D06 RID: 19718 RVA: 0x001DB886 File Offset: 0x001D9A86
	public override string Name
	{
		get
		{
			return this.m_fileInfo.Name;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06004D07 RID: 19719 RVA: 0x001DB893 File Offset: 0x001D9A93
	public long Length
	{
		get
		{
			if (base.IsManaged)
			{
				return SaveDataUtils.SaveDataManager.ManagedFileInfoLength(base.ManagedPath);
			}
			return this.m_fileInfo.Length;
		}
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06004D08 RID: 19720 RVA: 0x001DB8B9 File Offset: 0x001D9AB9
	public string DirectoryName
	{
		get
		{
			return this.m_fileInfo.DirectoryName;
		}
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06004D09 RID: 19721 RVA: 0x001DB8C6 File Offset: 0x001D9AC6
	public SdDirectoryInfo Directory
	{
		get
		{
			return new SdDirectoryInfo(this.m_fileInfo.Directory);
		}
	}

	// Token: 0x06004D0A RID: 19722 RVA: 0x001DB8D8 File Offset: 0x001D9AD8
	public StreamReader OpenText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.OpenText();
		}
		return SdFile.ManagedOpenText(base.ManagedPath);
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x001DB8F9 File Offset: 0x001D9AF9
	public StreamWriter CreateText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.CreateText();
		}
		return SdFile.ManagedCreateText(base.ManagedPath);
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x001DB91A File Offset: 0x001D9B1A
	public StreamWriter AppendText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.AppendText();
		}
		return SdFile.ManagedAppendText(base.ManagedPath);
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x001DB93C File Offset: 0x001D9B3C
	public SdFileInfo CopyTo(string destFileName)
	{
		bool isManaged = base.IsManaged;
		SaveDataManagedPath saveDataManagedPath;
		bool flag = SaveDataUtils.TryGetManagedPath(destFileName, out saveDataManagedPath);
		if (isManaged && flag)
		{
			SdFile.ManagedToManagedCopy(base.ManagedPath, saveDataManagedPath, false);
		}
		else if (isManaged)
		{
			SdFile.ManagedToUnmanagedCopy(base.ManagedPath, destFileName, false);
		}
		else if (flag)
		{
			SdFile.UnmanagedToManagedCopy(this.FullName, saveDataManagedPath, false);
		}
		else
		{
			this.m_fileInfo.CopyTo(destFileName, false);
		}
		return new SdFileInfo(destFileName);
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x001DB9A8 File Offset: 0x001D9BA8
	public SdFileInfo CopyTo(string destFileName, bool overwrite)
	{
		bool isManaged = base.IsManaged;
		SaveDataManagedPath saveDataManagedPath;
		bool flag = SaveDataUtils.TryGetManagedPath(destFileName, out saveDataManagedPath);
		if (isManaged && flag)
		{
			SdFile.ManagedToManagedCopy(base.ManagedPath, saveDataManagedPath, overwrite);
		}
		else if (isManaged)
		{
			SdFile.ManagedToUnmanagedCopy(base.ManagedPath, destFileName, overwrite);
		}
		else if (flag)
		{
			SdFile.UnmanagedToManagedCopy(this.FullName, saveDataManagedPath, overwrite);
		}
		else
		{
			this.m_fileInfo.CopyTo(destFileName, overwrite);
		}
		return new SdFileInfo(destFileName);
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x001DBA11 File Offset: 0x001D9C11
	public Stream Create()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.Create();
		}
		return SdFile.ManagedCreate(base.ManagedPath);
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x001DBA32 File Offset: 0x001D9C32
	public override void Delete()
	{
		if (base.IsManaged)
		{
			SdFile.ManagedDelete(base.ManagedPath);
			return;
		}
		this.m_fileInfo.Delete();
	}

	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x06004D11 RID: 19729 RVA: 0x001DBA53 File Offset: 0x001D9C53
	public override bool Exists
	{
		get
		{
			if (!base.IsManaged)
			{
				return this.m_fileInfo.Exists;
			}
			return SdFile.ManagedExists(base.ManagedPath);
		}
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x001DBA74 File Offset: 0x001D9C74
	public Stream Open(FileMode mode)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, FileAccess.ReadWrite, FileShare.None);
		}
		return this.m_fileInfo.Open(mode);
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x001DBA99 File Offset: 0x001D9C99
	public Stream Open(FileMode mode, FileAccess access)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, FileShare.None);
		}
		return this.m_fileInfo.Open(mode, access);
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x001DBABF File Offset: 0x001D9CBF
	public Stream Open(FileMode mode, FileAccess access, FileShare share)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, share);
		}
		return this.m_fileInfo.Open(mode, access, share);
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x001DBAE6 File Offset: 0x001D9CE6
	public Stream OpenRead()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return this.m_fileInfo.OpenRead();
	}

	// Token: 0x06004D16 RID: 19734 RVA: 0x001DBB0A File Offset: 0x001D9D0A
	public Stream OpenWrite()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}
		return this.m_fileInfo.OpenWrite();
	}

	// Token: 0x06004D17 RID: 19735 RVA: 0x001DBB2E File Offset: 0x001D9D2E
	public override string ToString()
	{
		return this.m_fileInfo.ToString();
	}

	// Token: 0x04003C17 RID: 15383
	[PublicizedFrom(EAccessModifier.Private)]
	public FileInfo m_fileInfo;
}
