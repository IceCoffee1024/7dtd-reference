using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x020009F2 RID: 2546
public sealed class SdDirectoryInfo : SdFileSystemInfo
{
	// Token: 0x06004C37 RID: 19511 RVA: 0x001D6B94 File Offset: 0x001D4D94
	public SdDirectoryInfo(string path)
		: this(new DirectoryInfo(path))
	{
	}

	// Token: 0x06004C38 RID: 19512 RVA: 0x001D6BA2 File Offset: 0x001D4DA2
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(SaveDataManagedPath saveDataManagedPath)
		: this(new DirectoryInfo(saveDataManagedPath.GetOriginalPath()), saveDataManagedPath)
	{
	}

	// Token: 0x06004C39 RID: 19513 RVA: 0x001D6BB6 File Offset: 0x001D4DB6
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo)
		: base(directoryInfo)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x06004C3A RID: 19514 RVA: 0x001D6BC6 File Offset: 0x001D4DC6
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo, SaveDataManagedPath saveDataManagedPath)
		: base(directoryInfo, saveDataManagedPath)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x06004C3B RID: 19515 RVA: 0x001D6BD7 File Offset: 0x001D4DD7
	public override string Name
	{
		get
		{
			return this.m_directoryInfo.Name;
		}
	}

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x06004C3C RID: 19516 RVA: 0x001D6BE4 File Offset: 0x001D4DE4
	public override string FullName
	{
		get
		{
			return this.m_directoryInfo.FullName;
		}
	}

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x06004C3D RID: 19517 RVA: 0x001D6BF1 File Offset: 0x001D4DF1
	public SdDirectoryInfo Parent
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Parent);
		}
	}

	// Token: 0x06004C3E RID: 19518 RVA: 0x001D6C03 File Offset: 0x001D4E03
	public SdDirectoryInfo CreateSubdirectory(string path)
	{
		if (base.IsManaged)
		{
			return this.ManagedCreateSubdirectory(path);
		}
		return new SdDirectoryInfo(this.m_directoryInfo.CreateSubdirectory(path));
	}

	// Token: 0x06004C3F RID: 19519 RVA: 0x001D6C26 File Offset: 0x001D4E26
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo ManagedCreateSubdirectory(string path)
	{
		return SdDirectory.CreateDirectory(Path.Combine(this.FullName, path));
	}

	// Token: 0x06004C40 RID: 19520 RVA: 0x001D6C39 File Offset: 0x001D4E39
	public void Create()
	{
		if (base.IsManaged)
		{
			this.ManagedCreateDirectory();
			return;
		}
		this.m_directoryInfo.Create();
	}

	// Token: 0x06004C41 RID: 19521 RVA: 0x001D6C55 File Offset: 0x001D4E55
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedCreateDirectory()
	{
		SdDirectory.ManagedCreateDirectory(base.ManagedPath);
	}

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x06004C42 RID: 19522 RVA: 0x001D6C63 File Offset: 0x001D4E63
	public override bool Exists
	{
		get
		{
			if (!base.IsManaged)
			{
				return this.m_directoryInfo.Exists;
			}
			return SdDirectory.ManagedExists(base.ManagedPath);
		}
	}

	// Token: 0x06004C43 RID: 19523 RVA: 0x001D6C84 File Offset: 0x001D4E84
	public SdFileInfo[] GetFiles(string searchPattern)
	{
		return this.EnumerateFiles(searchPattern).ToArray<SdFileInfo>();
	}

	// Token: 0x06004C44 RID: 19524 RVA: 0x001D6C92 File Offset: 0x001D4E92
	public SdFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFiles(searchPattern, searchOption).ToArray<SdFileInfo>();
	}

	// Token: 0x06004C45 RID: 19525 RVA: 0x001D6CA1 File Offset: 0x001D4EA1
	public SdFileInfo[] GetFiles()
	{
		return this.EnumerateFiles().ToArray<SdFileInfo>();
	}

	// Token: 0x06004C46 RID: 19526 RVA: 0x001D6CAE File Offset: 0x001D4EAE
	public SdDirectoryInfo[] GetDirectories()
	{
		return this.EnumerateDirectories().ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004C47 RID: 19527 RVA: 0x001D6CBB File Offset: 0x001D4EBB
	public SdDirectoryInfo[] GetDirectories(string searchPattern)
	{
		return this.EnumerateDirectories(searchPattern).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004C48 RID: 19528 RVA: 0x001D6CC9 File Offset: 0x001D4EC9
	public SdDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateDirectories(searchPattern, searchOption).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004C49 RID: 19529 RVA: 0x001D6CD8 File Offset: 0x001D4ED8
	public SdFileSystemInfo[] GetFileSystemInfos()
	{
		return this.EnumerateFileSystemInfos().ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004C4A RID: 19530 RVA: 0x001D6CE5 File Offset: 0x001D4EE5
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern)
	{
		return this.EnumerateFileSystemInfos(searchPattern).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004C4B RID: 19531 RVA: 0x001D6CF3 File Offset: 0x001D4EF3
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFileSystemInfos(searchPattern, searchOption).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004C4C RID: 19532 RVA: 0x001D6D04 File Offset: 0x001D4F04
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories()
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004C4D RID: 19533 RVA: 0x001D6D58 File Offset: 0x001D4F58
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004C4E RID: 19534 RVA: 0x001D6DA8 File Offset: 0x001D4FA8
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004C4F RID: 19535 RVA: 0x001D6DF7 File Offset: 0x001D4FF7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdDirectoryInfo> ManagedEnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateDirectories(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004C50 RID: 19536 RVA: 0x001D6E0C File Offset: 0x001D500C
	public IEnumerable<SdFileInfo> EnumerateFiles()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles()
			select new SdFileInfo(x);
	}

	// Token: 0x06004C51 RID: 19537 RVA: 0x001D6E60 File Offset: 0x001D5060
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern)
			select new SdFileInfo(x);
	}

	// Token: 0x06004C52 RID: 19538 RVA: 0x001D6EB0 File Offset: 0x001D50B0
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004C53 RID: 19539 RVA: 0x001D6EFF File Offset: 0x001D50FF
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileInfo> ManagedEnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFiles(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004C54 RID: 19540 RVA: 0x001D6F13 File Offset: 0x001D5113
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos().Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004C55 RID: 19541 RVA: 0x001D6F46 File Offset: 0x001D5146
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004C56 RID: 19542 RVA: 0x001D6F76 File Offset: 0x001D5176
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, searchOption);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004C57 RID: 19543 RVA: 0x001D6FA7 File Offset: 0x001D51A7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileSystemInfo> ManagedEnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFileSystemInfos(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004C58 RID: 19544 RVA: 0x001D6FBC File Offset: 0x001D51BC
	[PublicizedFrom(EAccessModifier.Internal)]
	public static SdFileSystemInfo WrapFileSystemInfo(FileSystemInfo fileSystemInfo)
	{
		FileInfo fileInfo = fileSystemInfo as FileInfo;
		SdFileSystemInfo sdFileSystemInfo;
		if (fileInfo == null)
		{
			DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
			if (directoryInfo == null)
			{
				throw new NotImplementedException("Unsupported implementation of FileSystemInfo: " + fileSystemInfo.GetType().FullName + ".");
			}
			sdFileSystemInfo = new SdDirectoryInfo(directoryInfo);
		}
		else
		{
			sdFileSystemInfo = new SdFileInfo(fileInfo);
		}
		return sdFileSystemInfo;
	}

	// Token: 0x06004C59 RID: 19545 RVA: 0x001D7014 File Offset: 0x001D5214
	public bool IsDirEmpty()
	{
		bool flag;
		using (IEnumerator<SdFileSystemInfo> enumerator = this.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
		{
			flag = !enumerator.MoveNext();
		}
		return flag;
	}

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x06004C5A RID: 19546 RVA: 0x001D705C File Offset: 0x001D525C
	public SdDirectoryInfo Root
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Root);
		}
	}

	// Token: 0x06004C5B RID: 19547 RVA: 0x001D706E File Offset: 0x001D526E
	public override void Delete()
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(false);
			return;
		}
		this.m_directoryInfo.Delete();
	}

	// Token: 0x06004C5C RID: 19548 RVA: 0x001D708B File Offset: 0x001D528B
	public void Delete(bool recursive)
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(recursive);
			return;
		}
		this.m_directoryInfo.Delete(recursive);
	}

	// Token: 0x06004C5D RID: 19549 RVA: 0x001D70A9 File Offset: 0x001D52A9
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedDelete(bool recursive)
	{
		SdDirectory.ManagedDelete(base.ManagedPath, recursive);
	}

	// Token: 0x06004C5E RID: 19550 RVA: 0x001D70B7 File Offset: 0x001D52B7
	public override string ToString()
	{
		return this.m_directoryInfo.ToString();
	}

	// Token: 0x04003B66 RID: 15206
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DirectoryInfo m_directoryInfo;
}
