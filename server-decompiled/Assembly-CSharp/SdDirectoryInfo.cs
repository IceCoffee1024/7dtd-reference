using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A17 RID: 2583
public sealed class SdDirectoryInfo : SdFileSystemInfo
{
	// Token: 0x06004CB0 RID: 19632 RVA: 0x001DB714 File Offset: 0x001D9914
	public SdDirectoryInfo(string path)
		: this(new DirectoryInfo(path))
	{
	}

	// Token: 0x06004CB1 RID: 19633 RVA: 0x001DB722 File Offset: 0x001D9922
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(SaveDataManagedPath saveDataManagedPath)
		: this(new DirectoryInfo(saveDataManagedPath.GetOriginalPath()), saveDataManagedPath)
	{
	}

	// Token: 0x06004CB2 RID: 19634 RVA: 0x001DB736 File Offset: 0x001D9936
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo)
		: base(directoryInfo)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x06004CB3 RID: 19635 RVA: 0x001DB746 File Offset: 0x001D9946
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo, SaveDataManagedPath saveDataManagedPath)
		: base(directoryInfo, saveDataManagedPath)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x001DB757 File Offset: 0x001D9957
	public override string Name
	{
		get
		{
			return this.m_directoryInfo.Name;
		}
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x06004CB5 RID: 19637 RVA: 0x001DB764 File Offset: 0x001D9964
	public override string FullName
	{
		get
		{
			return this.m_directoryInfo.FullName;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x001DB771 File Offset: 0x001D9971
	public SdDirectoryInfo Parent
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Parent);
		}
	}

	// Token: 0x06004CB7 RID: 19639 RVA: 0x001DB783 File Offset: 0x001D9983
	public SdDirectoryInfo CreateSubdirectory(string path)
	{
		if (base.IsManaged)
		{
			return this.ManagedCreateSubdirectory(path);
		}
		return new SdDirectoryInfo(this.m_directoryInfo.CreateSubdirectory(path));
	}

	// Token: 0x06004CB8 RID: 19640 RVA: 0x001DB7A6 File Offset: 0x001D99A6
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo ManagedCreateSubdirectory(string path)
	{
		return SdDirectory.CreateDirectory(Path.Combine(this.FullName, path));
	}

	// Token: 0x06004CB9 RID: 19641 RVA: 0x001DB7B9 File Offset: 0x001D99B9
	public void Create()
	{
		if (base.IsManaged)
		{
			this.ManagedCreateDirectory();
			return;
		}
		this.m_directoryInfo.Create();
	}

	// Token: 0x06004CBA RID: 19642 RVA: 0x001DB7D5 File Offset: 0x001D99D5
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedCreateDirectory()
	{
		SdDirectory.ManagedCreateDirectory(base.ManagedPath);
	}

	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x06004CBB RID: 19643 RVA: 0x001DB7E3 File Offset: 0x001D99E3
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

	// Token: 0x06004CBC RID: 19644 RVA: 0x001DB804 File Offset: 0x001D9A04
	public SdFileInfo[] GetFiles(string searchPattern)
	{
		return this.EnumerateFiles(searchPattern).ToArray<SdFileInfo>();
	}

	// Token: 0x06004CBD RID: 19645 RVA: 0x001DB812 File Offset: 0x001D9A12
	public SdFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFiles(searchPattern, searchOption).ToArray<SdFileInfo>();
	}

	// Token: 0x06004CBE RID: 19646 RVA: 0x001DB821 File Offset: 0x001D9A21
	public SdFileInfo[] GetFiles()
	{
		return this.EnumerateFiles().ToArray<SdFileInfo>();
	}

	// Token: 0x06004CBF RID: 19647 RVA: 0x001DB82E File Offset: 0x001D9A2E
	public SdDirectoryInfo[] GetDirectories()
	{
		return this.EnumerateDirectories().ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x001DB83B File Offset: 0x001D9A3B
	public SdDirectoryInfo[] GetDirectories(string searchPattern)
	{
		return this.EnumerateDirectories(searchPattern).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x001DB849 File Offset: 0x001D9A49
	public SdDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateDirectories(searchPattern, searchOption).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x001DB858 File Offset: 0x001D9A58
	public SdFileSystemInfo[] GetFileSystemInfos()
	{
		return this.EnumerateFileSystemInfos().ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CC3 RID: 19651 RVA: 0x001DB865 File Offset: 0x001D9A65
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern)
	{
		return this.EnumerateFileSystemInfos(searchPattern).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CC4 RID: 19652 RVA: 0x001DB873 File Offset: 0x001D9A73
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFileSystemInfos(searchPattern, searchOption).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CC5 RID: 19653 RVA: 0x001DB884 File Offset: 0x001D9A84
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories()
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CC6 RID: 19654 RVA: 0x001DB8D8 File Offset: 0x001D9AD8
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CC7 RID: 19655 RVA: 0x001DB928 File Offset: 0x001D9B28
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CC8 RID: 19656 RVA: 0x001DB977 File Offset: 0x001D9B77
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdDirectoryInfo> ManagedEnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateDirectories(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CC9 RID: 19657 RVA: 0x001DB98C File Offset: 0x001D9B8C
	public IEnumerable<SdFileInfo> EnumerateFiles()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles()
			select new SdFileInfo(x);
	}

	// Token: 0x06004CCA RID: 19658 RVA: 0x001DB9E0 File Offset: 0x001D9BE0
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern)
			select new SdFileInfo(x);
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x001DBA30 File Offset: 0x001D9C30
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x001DBA7F File Offset: 0x001D9C7F
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileInfo> ManagedEnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFiles(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CCD RID: 19661 RVA: 0x001DBA93 File Offset: 0x001D9C93
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos().Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x001DBAC6 File Offset: 0x001D9CC6
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x001DBAF6 File Offset: 0x001D9CF6
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, searchOption);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x001DBB27 File Offset: 0x001D9D27
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileSystemInfo> ManagedEnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFileSystemInfos(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CD1 RID: 19665 RVA: 0x001DBB3C File Offset: 0x001D9D3C
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

	// Token: 0x06004CD2 RID: 19666 RVA: 0x001DBB94 File Offset: 0x001D9D94
	public bool IsDirEmpty()
	{
		bool flag;
		using (IEnumerator<SdFileSystemInfo> enumerator = this.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
		{
			flag = !enumerator.MoveNext();
		}
		return flag;
	}

	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x06004CD3 RID: 19667 RVA: 0x001DBBDC File Offset: 0x001D9DDC
	public SdDirectoryInfo Root
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Root);
		}
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x001DBBEE File Offset: 0x001D9DEE
	public override void Delete()
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(false);
			return;
		}
		this.m_directoryInfo.Delete();
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x001DBC0B File Offset: 0x001D9E0B
	public void Delete(bool recursive)
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(recursive);
			return;
		}
		this.m_directoryInfo.Delete(recursive);
	}

	// Token: 0x06004CD6 RID: 19670 RVA: 0x001DBC29 File Offset: 0x001D9E29
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedDelete(bool recursive)
	{
		SdDirectory.ManagedDelete(base.ManagedPath, recursive);
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x001DBC37 File Offset: 0x001D9E37
	public override string ToString()
	{
		return this.m_directoryInfo.ToString();
	}

	// Token: 0x04003C25 RID: 15397
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DirectoryInfo m_directoryInfo;
}
