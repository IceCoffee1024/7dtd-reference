using System;
using System.IO;

// Token: 0x020009F8 RID: 2552
public class SdFileInfo : SdFileSystemInfo
{
	// Token: 0x06004CAC RID: 19628 RVA: 0x001D7D9B File Offset: 0x001D5F9B
	public SdFileInfo(string fileName)
		: this(new FileInfo(fileName))
	{
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x001D7DA9 File Offset: 0x001D5FA9
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(SaveDataManagedPath managedPath)
		: this(new FileInfo(managedPath.GetOriginalPath()), managedPath)
	{
	}

	// Token: 0x06004CAE RID: 19630 RVA: 0x001D7DBD File Offset: 0x001D5FBD
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdFileInfo(FileInfo fileInfo)
		: base(fileInfo)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004CAF RID: 19631 RVA: 0x001D7DCD File Offset: 0x001D5FCD
	[PublicizedFrom(EAccessModifier.Private)]
	public SdFileInfo(FileInfo fileInfo, SaveDataManagedPath managedPath)
		: base(fileInfo, managedPath)
	{
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x06004CB0 RID: 19632 RVA: 0x001D7DDE File Offset: 0x001D5FDE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reinitialize(FileInfo fileInfo)
	{
		base.Reinitialize(fileInfo);
		this.m_fileInfo = fileInfo;
	}

	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x06004CB1 RID: 19633 RVA: 0x001D7DEE File Offset: 0x001D5FEE
	public override string Name
	{
		get
		{
			return this.m_fileInfo.Name;
		}
	}

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x001D7DFB File Offset: 0x001D5FFB
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

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x001D7E21 File Offset: 0x001D6021
	public string DirectoryName
	{
		get
		{
			return this.m_fileInfo.DirectoryName;
		}
	}

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x001D7E2E File Offset: 0x001D602E
	public SdDirectoryInfo Directory
	{
		get
		{
			return new SdDirectoryInfo(this.m_fileInfo.Directory);
		}
	}

	// Token: 0x06004CB5 RID: 19637 RVA: 0x001D7E40 File Offset: 0x001D6040
	public StreamReader OpenText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.OpenText();
		}
		return SdFile.ManagedOpenText(base.ManagedPath);
	}

	// Token: 0x06004CB6 RID: 19638 RVA: 0x001D7E61 File Offset: 0x001D6061
	public StreamWriter CreateText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.CreateText();
		}
		return SdFile.ManagedCreateText(base.ManagedPath);
	}

	// Token: 0x06004CB7 RID: 19639 RVA: 0x001D7E82 File Offset: 0x001D6082
	public StreamWriter AppendText()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.AppendText();
		}
		return SdFile.ManagedAppendText(base.ManagedPath);
	}

	// Token: 0x06004CB8 RID: 19640 RVA: 0x001D7EA4 File Offset: 0x001D60A4
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

	// Token: 0x06004CB9 RID: 19641 RVA: 0x001D7F10 File Offset: 0x001D6110
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

	// Token: 0x06004CBA RID: 19642 RVA: 0x001D7F79 File Offset: 0x001D6179
	public Stream Create()
	{
		if (!base.IsManaged)
		{
			return this.m_fileInfo.Create();
		}
		return SdFile.ManagedCreate(base.ManagedPath);
	}

	// Token: 0x06004CBB RID: 19643 RVA: 0x001D7F9A File Offset: 0x001D619A
	public override void Delete()
	{
		if (base.IsManaged)
		{
			SdFile.ManagedDelete(base.ManagedPath);
			return;
		}
		this.m_fileInfo.Delete();
	}

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x06004CBC RID: 19644 RVA: 0x001D7FBB File Offset: 0x001D61BB
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

	// Token: 0x06004CBD RID: 19645 RVA: 0x001D7FDC File Offset: 0x001D61DC
	public Stream Open(FileMode mode)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, FileAccess.ReadWrite, FileShare.None);
		}
		return this.m_fileInfo.Open(mode);
	}

	// Token: 0x06004CBE RID: 19646 RVA: 0x001D8001 File Offset: 0x001D6201
	public Stream Open(FileMode mode, FileAccess access)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, FileShare.None);
		}
		return this.m_fileInfo.Open(mode, access);
	}

	// Token: 0x06004CBF RID: 19647 RVA: 0x001D8027 File Offset: 0x001D6227
	public Stream Open(FileMode mode, FileAccess access, FileShare share)
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, mode, access, share);
		}
		return this.m_fileInfo.Open(mode, access, share);
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x001D804E File Offset: 0x001D624E
	public Stream OpenRead()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		return this.m_fileInfo.OpenRead();
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x001D8072 File Offset: 0x001D6272
	public Stream OpenWrite()
	{
		if (base.IsManaged)
		{
			return SdFile.ManagedOpen(base.ManagedPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}
		return this.m_fileInfo.OpenWrite();
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x001D8096 File Offset: 0x001D6296
	public override string ToString()
	{
		return this.m_fileInfo.ToString();
	}

	// Token: 0x04003B79 RID: 15225
	[PublicizedFrom(EAccessModifier.Private)]
	public FileInfo m_fileInfo;
}
