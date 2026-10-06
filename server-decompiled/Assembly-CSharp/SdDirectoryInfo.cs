using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A0D RID: 2573
public sealed class SdDirectoryInfo : SdFileSystemInfo
{
	// Token: 0x06004C8C RID: 19596 RVA: 0x001DA62C File Offset: 0x001D882C
	public SdDirectoryInfo(string path)
		: this(new DirectoryInfo(path))
	{
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x001DA63A File Offset: 0x001D883A
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(SaveDataManagedPath saveDataManagedPath)
		: this(new DirectoryInfo(saveDataManagedPath.GetOriginalPath()), saveDataManagedPath)
	{
	}

	// Token: 0x06004C8E RID: 19598 RVA: 0x001DA64E File Offset: 0x001D884E
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo)
		: base(directoryInfo)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x001DA65E File Offset: 0x001D885E
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo, SaveDataManagedPath saveDataManagedPath)
		: base(directoryInfo, saveDataManagedPath)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x17000849 RID: 2121
	// (get) Token: 0x06004C90 RID: 19600 RVA: 0x001DA66F File Offset: 0x001D886F
	public override string Name
	{
		get
		{
			return this.m_directoryInfo.Name;
		}
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06004C91 RID: 19601 RVA: 0x001DA67C File Offset: 0x001D887C
	public override string FullName
	{
		get
		{
			return this.m_directoryInfo.FullName;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x06004C92 RID: 19602 RVA: 0x001DA689 File Offset: 0x001D8889
	public SdDirectoryInfo Parent
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Parent);
		}
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x001DA69B File Offset: 0x001D889B
	public SdDirectoryInfo CreateSubdirectory(string path)
	{
		if (base.IsManaged)
		{
			return this.ManagedCreateSubdirectory(path);
		}
		return new SdDirectoryInfo(this.m_directoryInfo.CreateSubdirectory(path));
	}

	// Token: 0x06004C94 RID: 19604 RVA: 0x001DA6BE File Offset: 0x001D88BE
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo ManagedCreateSubdirectory(string path)
	{
		return SdDirectory.CreateDirectory(Path.Combine(this.FullName, path));
	}

	// Token: 0x06004C95 RID: 19605 RVA: 0x001DA6D1 File Offset: 0x001D88D1
	public void Create()
	{
		if (base.IsManaged)
		{
			this.ManagedCreateDirectory();
			return;
		}
		this.m_directoryInfo.Create();
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x001DA6ED File Offset: 0x001D88ED
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedCreateDirectory()
	{
		SdDirectory.ManagedCreateDirectory(base.ManagedPath);
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x06004C97 RID: 19607 RVA: 0x001DA6FB File Offset: 0x001D88FB
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

	// Token: 0x06004C98 RID: 19608 RVA: 0x001DA71C File Offset: 0x001D891C
	public SdFileInfo[] GetFiles(string searchPattern)
	{
		return this.EnumerateFiles(searchPattern).ToArray<SdFileInfo>();
	}

	// Token: 0x06004C99 RID: 19609 RVA: 0x001DA72A File Offset: 0x001D892A
	public SdFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFiles(searchPattern, searchOption).ToArray<SdFileInfo>();
	}

	// Token: 0x06004C9A RID: 19610 RVA: 0x001DA739 File Offset: 0x001D8939
	public SdFileInfo[] GetFiles()
	{
		return this.EnumerateFiles().ToArray<SdFileInfo>();
	}

	// Token: 0x06004C9B RID: 19611 RVA: 0x001DA746 File Offset: 0x001D8946
	public SdDirectoryInfo[] GetDirectories()
	{
		return this.EnumerateDirectories().ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x001DA753 File Offset: 0x001D8953
	public SdDirectoryInfo[] GetDirectories(string searchPattern)
	{
		return this.EnumerateDirectories(searchPattern).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x001DA761 File Offset: 0x001D8961
	public SdDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateDirectories(searchPattern, searchOption).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x001DA770 File Offset: 0x001D8970
	public SdFileSystemInfo[] GetFileSystemInfos()
	{
		return this.EnumerateFileSystemInfos().ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x001DA77D File Offset: 0x001D897D
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern)
	{
		return this.EnumerateFileSystemInfos(searchPattern).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x001DA78B File Offset: 0x001D898B
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFileSystemInfos(searchPattern, searchOption).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x001DA79C File Offset: 0x001D899C
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories()
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CA2 RID: 19618 RVA: 0x001DA7F0 File Offset: 0x001D89F0
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CA3 RID: 19619 RVA: 0x001DA840 File Offset: 0x001D8A40
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CA4 RID: 19620 RVA: 0x001DA88F File Offset: 0x001D8A8F
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdDirectoryInfo> ManagedEnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateDirectories(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CA5 RID: 19621 RVA: 0x001DA8A4 File Offset: 0x001D8AA4
	public IEnumerable<SdFileInfo> EnumerateFiles()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles()
			select new SdFileInfo(x);
	}

	// Token: 0x06004CA6 RID: 19622 RVA: 0x001DA8F8 File Offset: 0x001D8AF8
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern)
			select new SdFileInfo(x);
	}

	// Token: 0x06004CA7 RID: 19623 RVA: 0x001DA948 File Offset: 0x001D8B48
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004CA8 RID: 19624 RVA: 0x001DA997 File Offset: 0x001D8B97
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileInfo> ManagedEnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFiles(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CA9 RID: 19625 RVA: 0x001DA9AB File Offset: 0x001D8BAB
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos().Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CAA RID: 19626 RVA: 0x001DA9DE File Offset: 0x001D8BDE
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CAB RID: 19627 RVA: 0x001DAA0E File Offset: 0x001D8C0E
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, searchOption);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CAC RID: 19628 RVA: 0x001DAA3F File Offset: 0x001D8C3F
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileSystemInfo> ManagedEnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFileSystemInfos(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x001DAA54 File Offset: 0x001D8C54
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

	// Token: 0x06004CAE RID: 19630 RVA: 0x001DAAAC File Offset: 0x001D8CAC
	public bool IsDirEmpty()
	{
		bool flag;
		using (IEnumerator<SdFileSystemInfo> enumerator = this.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
		{
			flag = !enumerator.MoveNext();
		}
		return flag;
	}

	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x06004CAF RID: 19631 RVA: 0x001DAAF4 File Offset: 0x001D8CF4
	public SdDirectoryInfo Root
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Root);
		}
	}

	// Token: 0x06004CB0 RID: 19632 RVA: 0x001DAB06 File Offset: 0x001D8D06
	public override void Delete()
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(false);
			return;
		}
		this.m_directoryInfo.Delete();
	}

	// Token: 0x06004CB1 RID: 19633 RVA: 0x001DAB23 File Offset: 0x001D8D23
	public void Delete(bool recursive)
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(recursive);
			return;
		}
		this.m_directoryInfo.Delete(recursive);
	}

	// Token: 0x06004CB2 RID: 19634 RVA: 0x001DAB41 File Offset: 0x001D8D41
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedDelete(bool recursive)
	{
		SdDirectory.ManagedDelete(base.ManagedPath, recursive);
	}

	// Token: 0x06004CB3 RID: 19635 RVA: 0x001DAB4F File Offset: 0x001D8D4F
	public override string ToString()
	{
		return this.m_directoryInfo.ToString();
	}

	// Token: 0x04003C04 RID: 15364
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DirectoryInfo m_directoryInfo;
}
