using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000A03 RID: 2563
public sealed class SdDirectoryInfo : SdFileSystemInfo
{
	// Token: 0x06004CBD RID: 19645 RVA: 0x001D937C File Offset: 0x001D757C
	public SdDirectoryInfo(string path)
		: this(new DirectoryInfo(path))
	{
	}

	// Token: 0x06004CBE RID: 19646 RVA: 0x001D938A File Offset: 0x001D758A
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(SaveDataManagedPath saveDataManagedPath)
		: this(new DirectoryInfo(saveDataManagedPath.GetOriginalPath()), saveDataManagedPath)
	{
	}

	// Token: 0x06004CBF RID: 19647 RVA: 0x001D939E File Offset: 0x001D759E
	[PublicizedFrom(EAccessModifier.Internal)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo)
		: base(directoryInfo)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x001D93AE File Offset: 0x001D75AE
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo(DirectoryInfo directoryInfo, SaveDataManagedPath saveDataManagedPath)
		: base(directoryInfo, saveDataManagedPath)
	{
		this.m_directoryInfo = directoryInfo;
	}

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x001D93BF File Offset: 0x001D75BF
	public override string Name
	{
		get
		{
			return this.m_directoryInfo.Name;
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x001D93CC File Offset: 0x001D75CC
	public override string FullName
	{
		get
		{
			return this.m_directoryInfo.FullName;
		}
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x001D93D9 File Offset: 0x001D75D9
	public SdDirectoryInfo Parent
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Parent);
		}
	}

	// Token: 0x06004CC4 RID: 19652 RVA: 0x001D93EB File Offset: 0x001D75EB
	public SdDirectoryInfo CreateSubdirectory(string path)
	{
		if (base.IsManaged)
		{
			return this.ManagedCreateSubdirectory(path);
		}
		return new SdDirectoryInfo(this.m_directoryInfo.CreateSubdirectory(path));
	}

	// Token: 0x06004CC5 RID: 19653 RVA: 0x001D940E File Offset: 0x001D760E
	[PublicizedFrom(EAccessModifier.Private)]
	public SdDirectoryInfo ManagedCreateSubdirectory(string path)
	{
		return SdDirectory.CreateDirectory(Path.Combine(this.FullName, path));
	}

	// Token: 0x06004CC6 RID: 19654 RVA: 0x001D9421 File Offset: 0x001D7621
	public void Create()
	{
		if (base.IsManaged)
		{
			this.ManagedCreateDirectory();
			return;
		}
		this.m_directoryInfo.Create();
	}

	// Token: 0x06004CC7 RID: 19655 RVA: 0x001D943D File Offset: 0x001D763D
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedCreateDirectory()
	{
		SdDirectory.ManagedCreateDirectory(base.ManagedPath);
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x06004CC8 RID: 19656 RVA: 0x001D944B File Offset: 0x001D764B
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

	// Token: 0x06004CC9 RID: 19657 RVA: 0x001D946C File Offset: 0x001D766C
	public SdFileInfo[] GetFiles(string searchPattern)
	{
		return this.EnumerateFiles(searchPattern).ToArray<SdFileInfo>();
	}

	// Token: 0x06004CCA RID: 19658 RVA: 0x001D947A File Offset: 0x001D767A
	public SdFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFiles(searchPattern, searchOption).ToArray<SdFileInfo>();
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x001D9489 File Offset: 0x001D7689
	public SdFileInfo[] GetFiles()
	{
		return this.EnumerateFiles().ToArray<SdFileInfo>();
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x001D9496 File Offset: 0x001D7696
	public SdDirectoryInfo[] GetDirectories()
	{
		return this.EnumerateDirectories().ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004CCD RID: 19661 RVA: 0x001D94A3 File Offset: 0x001D76A3
	public SdDirectoryInfo[] GetDirectories(string searchPattern)
	{
		return this.EnumerateDirectories(searchPattern).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x001D94B1 File Offset: 0x001D76B1
	public SdDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateDirectories(searchPattern, searchOption).ToArray<SdDirectoryInfo>();
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x001D94C0 File Offset: 0x001D76C0
	public SdFileSystemInfo[] GetFileSystemInfos()
	{
		return this.EnumerateFileSystemInfos().ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x001D94CD File Offset: 0x001D76CD
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern)
	{
		return this.EnumerateFileSystemInfos(searchPattern).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CD1 RID: 19665 RVA: 0x001D94DB File Offset: 0x001D76DB
	public SdFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return this.EnumerateFileSystemInfos(searchPattern, searchOption).ToArray<SdFileSystemInfo>();
	}

	// Token: 0x06004CD2 RID: 19666 RVA: 0x001D94EC File Offset: 0x001D76EC
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories()
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CD3 RID: 19667 RVA: 0x001D9540 File Offset: 0x001D7740
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x001D9590 File Offset: 0x001D7790
	public IEnumerable<SdDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateDirectories(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateDirectories(searchPattern, searchOption)
			select new SdDirectoryInfo(x);
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x001D95DF File Offset: 0x001D77DF
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdDirectoryInfo> ManagedEnumerateDirectories(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateDirectories(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CD6 RID: 19670 RVA: 0x001D95F4 File Offset: 0x001D77F4
	public IEnumerable<SdFileInfo> EnumerateFiles()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles("*", SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles()
			select new SdFileInfo(x);
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x001D9648 File Offset: 0x001D7848
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern)
			select new SdFileInfo(x);
	}

	// Token: 0x06004CD8 RID: 19672 RVA: 0x001D9698 File Offset: 0x001D7898
	public IEnumerable<SdFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFiles(searchPattern, searchOption);
		}
		return from x in this.m_directoryInfo.EnumerateFiles(searchPattern, searchOption)
			select new SdFileInfo(x);
	}

	// Token: 0x06004CD9 RID: 19673 RVA: 0x001D96E7 File Offset: 0x001D78E7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileInfo> ManagedEnumerateFiles(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFiles(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x001D96FB File Offset: 0x001D78FB
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos()
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos().Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CDB RID: 19675 RVA: 0x001D972E File Offset: 0x001D792E
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CDC RID: 19676 RVA: 0x001D975E File Offset: 0x001D795E
	public IEnumerable<SdFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		if (base.IsManaged)
		{
			return this.ManagedEnumerateFileSystemInfos(searchPattern, searchOption);
		}
		return this.m_directoryInfo.EnumerateFileSystemInfos(searchPattern, searchOption).Select<FileSystemInfo, SdFileSystemInfo>(new Func<FileSystemInfo, SdFileSystemInfo>(SdDirectoryInfo.WrapFileSystemInfo));
	}

	// Token: 0x06004CDD RID: 19677 RVA: 0x001D978F File Offset: 0x001D798F
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<SdFileSystemInfo> ManagedEnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
	{
		return SaveDataUtils.SaveDataManager.ManagedDirectoryInfoEnumerateFileSystemInfos(base.ManagedPath, searchPattern, searchOption);
	}

	// Token: 0x06004CDE RID: 19678 RVA: 0x001D97A4 File Offset: 0x001D79A4
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

	// Token: 0x06004CDF RID: 19679 RVA: 0x001D97FC File Offset: 0x001D79FC
	public bool IsDirEmpty()
	{
		bool flag;
		using (IEnumerator<SdFileSystemInfo> enumerator = this.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
		{
			flag = !enumerator.MoveNext();
		}
		return flag;
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x06004CE0 RID: 19680 RVA: 0x001D9844 File Offset: 0x001D7A44
	public SdDirectoryInfo Root
	{
		get
		{
			return new SdDirectoryInfo(this.m_directoryInfo.Root);
		}
	}

	// Token: 0x06004CE1 RID: 19681 RVA: 0x001D9856 File Offset: 0x001D7A56
	public override void Delete()
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(false);
			return;
		}
		this.m_directoryInfo.Delete();
	}

	// Token: 0x06004CE2 RID: 19682 RVA: 0x001D9873 File Offset: 0x001D7A73
	public void Delete(bool recursive)
	{
		if (base.IsManaged)
		{
			this.ManagedDelete(recursive);
			return;
		}
		this.m_directoryInfo.Delete(recursive);
	}

	// Token: 0x06004CE3 RID: 19683 RVA: 0x001D9891 File Offset: 0x001D7A91
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagedDelete(bool recursive)
	{
		SdDirectory.ManagedDelete(base.ManagedPath, recursive);
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x001D989F File Offset: 0x001D7A9F
	public override string ToString()
	{
		return this.m_directoryInfo.ToString();
	}

	// Token: 0x04003BF7 RID: 15351
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DirectoryInfo m_directoryInfo;
}
