using System;
using System.IO;

// Token: 0x02000A09 RID: 2569
public class SdFileInfo : SdFileSystemInfo
{
	// Token: 0x06004D32 RID: 19762 RVA: 0x001DA583 File Offset: 0x001D8783
	public SdFileInfo(string fileName)
		: this(new FileInfo(fileName))
	{
	}

	// Token: 0x06004D33 RID: 19763 RVA: 0x001DA591 File Offset: 0x001D8791
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(SaveDataManagedPath managedPath)
		: this(new FileInfo(managedPath.GetOriginalPath()), managedPath)
	{
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x001DA5A5 File Offset: 0x001D87A5
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(FileInfo fileInfo)
		: base(fileInfo)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x001DA5B5 File Offset: 0x001D87B5
	[PublicizedFrom(EAccessModifier.Private)]
	public SdFileInfo(FileInfo fileInfo, SaveDataManagedPath managedPath)
		: base(fileInfo, managedPath)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x001DA5C6 File Offset: 0x001D87C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reinitialize(FileInfo fileInfo)
	{
		base.Reinitialize(fileInfo);
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06004D37 RID: 19767 RVA: 0x001DA5D6 File Offset: 0x001D87D6
	public override string Name
	{
		get
		{
			return this.m_fileInfo.Name;
		}
	}

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06004D38 RID: 19768 RVA: 0x001DA5E3 File Offset: 0x001D87E3
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

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x06004D39 RID: 19769 RVA: 0x001DA609 File Offset: 0x001D8809
	public string DirectoryName
	{
		get
		{
			return this.m_fileInfo.DirectoryName;
		}
	}

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06004D3A RID: 19770 RVA: 0x001DA616 File Offset: 0x001D8816
	public SdDirectoryInfo Directory
	{
		get
		{
			return new SdDirectoryInfo(this.m_fileInfo.Directory);
		}
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x001DA628 File Offset: 0x001D8828
	public StreamReader OpenText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.OpenText();
		}
		return SdFile.ManagedOpenText(base.ManagedPath);
	}

	// Token: 0x06004D3C RID: 19772 RVA: 0x001DA649 File Offset: 0x001D8849
	public StreamWriter CreateText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.CreateText();
		}
		return SdFile.ManagedCreateText(base.ManagedPath);
	}

	// Token: 0x06004D3D RID: 19773 RVA: 0x001DA66A File Offset: 0x001D886A
	public StreamWriter AppendText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.AppendText();
		}
		return SdFile.ManagedAppendText(base.ManagedPath);
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x001DA68C File Offset: 0x001D888C
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

	// Token: 0x06004D3F RID: 19775 RVA: 0x001DA6F8 File Offset: 0x001D88F8
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

	// Token: 0x06004D40 RID: 19776 RVA: 0x001DA761 File Offset: 0x001D8961
	public Stream Create()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.Create();
		}
		return SdFile.ManagedCreate(base.ManagedPath);
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x001DA782 File Offset: 0x001D8982
	public override void Delete()
	{
		if (base.IsManaged)
		{
			SdFile.ManagedDelete(base.ManagedPath);
			return;
		}
		this.m_fileInfo.Delete();
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x06004D42 RID: 19778 RVA: 0x001DA7A3 File Offset: 0x001D89A3
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

	// Token: 0x06004D43 RID: 19779 RVA: 0x001DA7C4 File Offset: 0x001D89C4
	public Stream Open(FileMode mode)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, FileAccess.ReadWrite, FileShare.None);
		}
		return this.m_fileInfo.Open(mode);
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x001DA7E9 File Offset: 0x001D89E9
	public Stream Open(FileMode mode, FileAccess access)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, FileShare.None);
		}
		return this.m_fileInfo.Open(mode, access);
	}

	// Token: 0x06004D45 RID: 19781 RVA: 0x001DA80F File Offset: 0x001D8A0F
	public Stream Open(FileMode mode, FileAccess access, FileShare share)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, share);
		}
		return this.m_fileInfo.Open(mode, access, share);
	}

	// Token: 0x06004D46 RID: 19782 RVA: 0x001DA836 File Offset: 0x001D8A36
	public Stream OpenRead()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return this.m_fileInfo.OpenRead();
	}

	// Token: 0x06004D47 RID: 19783 RVA: 0x001DA85A File Offset: 0x001D8A5A
	public Stream OpenWrite()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}
		return this.m_fileInfo.OpenWrite();
	}

	// Token: 0x06004D48 RID: 19784 RVA: 0x001DA87E File Offset: 0x001D8A7E
	public override string ToString()
	{
		return this.m_fileInfo.ToString();
	}

	// Token: 0x04003C0A RID: 15370
	[PublicizedFrom(EAccessModifier.Private)]
	public FileInfo m_fileInfo;
}
