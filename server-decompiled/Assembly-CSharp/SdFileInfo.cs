using System;
using System.IO;

// Token: 0x02000A1D RID: 2589
public class SdFileInfo : SdFileSystemInfo
{
	// Token: 0x06004D25 RID: 19749 RVA: 0x001DC91B File Offset: 0x001DAB1B
	public SdFileInfo(string fileName)
		: this(new FileInfo(fileName))
	{
	}

	// Token: 0x06004D26 RID: 19750 RVA: 0x001DC929 File Offset: 0x001DAB29
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(SaveDataManagedPath managedPath)
		: this(new FileInfo(managedPath.GetOriginalPath()), managedPath)
	{
	}

	// Token: 0x06004D27 RID: 19751 RVA: 0x001DC93D File Offset: 0x001DAB3D
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(FileInfo fileInfo)
		: base(fileInfo)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004D28 RID: 19752 RVA: 0x001DC94D File Offset: 0x001DAB4D
	[PublicizedFrom(EAccessModifier.Private)]
	public SdFileInfo(FileInfo fileInfo, SaveDataManagedPath managedPath)
		: base(fileInfo, managedPath)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004D29 RID: 19753 RVA: 0x001DC95E File Offset: 0x001DAB5E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reinitialize(FileInfo fileInfo)
	{
		base.Reinitialize(fileInfo);
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06004D2A RID: 19754 RVA: 0x001DC96E File Offset: 0x001DAB6E
	public override string Name
	{
		get
		{
			return this.m_fileInfo.Name;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06004D2B RID: 19755 RVA: 0x001DC97B File Offset: 0x001DAB7B
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

	// Token: 0x17000858 RID: 2136
	// (get) Token: 0x06004D2C RID: 19756 RVA: 0x001DC9A1 File Offset: 0x001DABA1
	public string DirectoryName
	{
		get
		{
			return this.m_fileInfo.DirectoryName;
		}
	}

	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x06004D2D RID: 19757 RVA: 0x001DC9AE File Offset: 0x001DABAE
	public SdDirectoryInfo Directory
	{
		get
		{
			return new SdDirectoryInfo(this.m_fileInfo.Directory);
		}
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x001DC9C0 File Offset: 0x001DABC0
	public StreamReader OpenText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.OpenText();
		}
		return SdFile.ManagedOpenText(base.ManagedPath);
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x001DC9E1 File Offset: 0x001DABE1
	public StreamWriter CreateText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.CreateText();
		}
		return SdFile.ManagedCreateText(base.ManagedPath);
	}

	// Token: 0x06004D30 RID: 19760 RVA: 0x001DCA02 File Offset: 0x001DAC02
	public StreamWriter AppendText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.AppendText();
		}
		return SdFile.ManagedAppendText(base.ManagedPath);
	}

	// Token: 0x06004D31 RID: 19761 RVA: 0x001DCA24 File Offset: 0x001DAC24
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

	// Token: 0x06004D32 RID: 19762 RVA: 0x001DCA90 File Offset: 0x001DAC90
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

	// Token: 0x06004D33 RID: 19763 RVA: 0x001DCAF9 File Offset: 0x001DACF9
	public Stream Create()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.Create();
		}
		return SdFile.ManagedCreate(base.ManagedPath);
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x001DCB1A File Offset: 0x001DAD1A
	public override void Delete()
	{
		if (base.IsManaged)
		{
			SdFile.ManagedDelete(base.ManagedPath);
			return;
		}
		this.m_fileInfo.Delete();
	}

	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x06004D35 RID: 19765 RVA: 0x001DCB3B File Offset: 0x001DAD3B
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

	// Token: 0x06004D36 RID: 19766 RVA: 0x001DCB5C File Offset: 0x001DAD5C
	public Stream Open(FileMode mode)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, FileAccess.ReadWrite, FileShare.None);
		}
		return this.m_fileInfo.Open(mode);
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x001DCB81 File Offset: 0x001DAD81
	public Stream Open(FileMode mode, FileAccess access)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, FileShare.None);
		}
		return this.m_fileInfo.Open(mode, access);
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x001DCBA7 File Offset: 0x001DADA7
	public Stream Open(FileMode mode, FileAccess access, FileShare share)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, share);
		}
		return this.m_fileInfo.Open(mode, access, share);
	}

	// Token: 0x06004D39 RID: 19769 RVA: 0x001DCBCE File Offset: 0x001DADCE
	public Stream OpenRead()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return this.m_fileInfo.OpenRead();
	}

	// Token: 0x06004D3A RID: 19770 RVA: 0x001DCBF2 File Offset: 0x001DADF2
	public Stream OpenWrite()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}
		return this.m_fileInfo.OpenWrite();
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x001DCC16 File Offset: 0x001DAE16
	public override string ToString()
	{
		return this.m_fileInfo.ToString();
	}

	// Token: 0x04003C38 RID: 15416
	[PublicizedFrom(EAccessModifier.Private)]
	public FileInfo m_fileInfo;
}
