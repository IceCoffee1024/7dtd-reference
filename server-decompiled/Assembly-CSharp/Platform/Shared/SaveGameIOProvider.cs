using System;
using System.Collections.Generic;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CB0 RID: 7344
	public abstract class SaveGameIOProvider : IPlatformSaveGameIOProvider
	{
		// Token: 0x0600D9DE RID: 55774
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract string GetPath(SaveDataManagedPath path);

		// Token: 0x0600D9DF RID: 55775 RVA: 0x004E1DB8 File Offset: 0x004DFFB8
		public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
		{
			using (FileStream fileStream = File.OpenRead(this.GetPath(path)))
			{
				StreamUtils.StreamCopy(fileStream, dest, null, true);
			}
		}

		// Token: 0x0600D9E0 RID: 55776 RVA: 0x004E1DF8 File Offset: 0x004DFFF8
		public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
		{
			using (FileStream fileStream = File.Open(this.GetPath(path), FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				StreamUtils.StreamCopy(src, fileStream, null, true);
			}
		}

		// Token: 0x0600D9E1 RID: 55777 RVA: 0x004E1E3C File Offset: 0x004E003C
		public void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false)
		{
			File.Copy(this.GetPath(sourceFileName), this.GetPath(destFileName), overwrite);
		}

		// Token: 0x0600D9E2 RID: 55778 RVA: 0x004E1E52 File Offset: 0x004E0052
		public void ManagedFileDelete(SaveDataManagedPath path)
		{
			File.Delete(this.GetPath(path));
		}

		// Token: 0x0600D9E3 RID: 55779 RVA: 0x004E1E60 File Offset: 0x004E0060
		public bool ManagedFileExists(SaveDataManagedPath path)
		{
			return File.Exists(this.GetPath(path));
		}

		// Token: 0x0600D9E4 RID: 55780 RVA: 0x004E1E6E File Offset: 0x004E006E
		public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			return File.GetLastWriteTimeUtc(this.GetPath(path));
		}

		// Token: 0x0600D9E5 RID: 55781 RVA: 0x004E1E7C File Offset: 0x004E007C
		public void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName)
		{
			File.Move(this.GetPath(sourceFileName), this.GetPath(destFileName));
		}

		// Token: 0x0600D9E6 RID: 55782 RVA: 0x004E1E91 File Offset: 0x004E0091
		public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
		{
			return new SdDirectoryInfo(Directory.CreateDirectory(this.GetPath(path)));
		}

		// Token: 0x0600D9E7 RID: 55783 RVA: 0x004E1EA4 File Offset: 0x004E00A4
		public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			return Directory.GetLastWriteTimeUtc(this.GetPath(path));
		}

		// Token: 0x0600D9E8 RID: 55784 RVA: 0x004E1EB2 File Offset: 0x004E00B2
		public bool ManagedDirectoryExists(SaveDataManagedPath path)
		{
			return Directory.Exists(this.GetPath(path));
		}

		// Token: 0x0600D9E9 RID: 55785 RVA: 0x004E1EC0 File Offset: 0x004E00C0
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateDirectories(path2, searchPattern, searchOption));
		}

		// Token: 0x0600D9EA RID: 55786 RVA: 0x004E1EE4 File Offset: 0x004E00E4
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateFiles(path2, searchPattern, searchOption));
		}

		// Token: 0x0600D9EB RID: 55787 RVA: 0x004E1F08 File Offset: 0x004E0108
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateFileSystemEntries(path2, searchPattern, searchOption));
		}

		// Token: 0x0600D9EC RID: 55788 RVA: 0x004E1F2C File Offset: 0x004E012C
		public void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
		{
			Directory.Delete(this.GetPath(path), recursive);
		}

		// Token: 0x0600D9ED RID: 55789 RVA: 0x004E1F3B File Offset: 0x004E013B
		public long ManagedFileInfoLength(SaveDataManagedPath path)
		{
			return new FileInfo(this.GetPath(path)).Length;
		}

		// Token: 0x0600D9EE RID: 55790 RVA: 0x004E1F4E File Offset: 0x004E014E
		public IEnumerable<SdDirectoryInfo> ManagedDirectoryInfoEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string basePath = this.GetPath(path);
			DirectoryInfo directoryInfo = new DirectoryInfo(basePath);
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.EnumerateDirectories(searchPattern, searchOption))
			{
				string fullName = directoryInfo2.FullName;
				SaveDataManagedPath managedPathFromBaseAndSubPath = SaveGameProviderHelper.GetManagedPathFromBaseAndSubPath(path, basePath, fullName);
				yield return new SdDirectoryInfo(managedPathFromBaseAndSubPath);
			}
			IEnumerator<DirectoryInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600D9EF RID: 55791 RVA: 0x004E1F73 File Offset: 0x004E0173
		public IEnumerable<SdFileInfo> ManagedDirectoryInfoEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string basePath = this.GetPath(path);
			DirectoryInfo directoryInfo = new DirectoryInfo(basePath);
			foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles(searchPattern, searchOption))
			{
				string fullName = fileInfo.FullName;
				SaveDataManagedPath managedPathFromBaseAndSubPath = SaveGameProviderHelper.GetManagedPathFromBaseAndSubPath(path, basePath, fullName);
				yield return new SdFileInfo(managedPathFromBaseAndSubPath);
			}
			IEnumerator<FileInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600D9F0 RID: 55792 RVA: 0x004E1F98 File Offset: 0x004E0198
		public IEnumerable<SdFileSystemInfo> ManagedDirectoryInfoEnumerateFileSystemInfos(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string basePath = this.GetPath(path);
			DirectoryInfo directoryInfo = new DirectoryInfo(basePath);
			foreach (FileSystemInfo fileSystemInfo in directoryInfo.EnumerateFileSystemInfos(searchPattern, searchOption))
			{
				string fullName = fileSystemInfo.FullName;
				SaveDataManagedPath managedPathFromBaseAndSubPath = SaveGameProviderHelper.GetManagedPathFromBaseAndSubPath(path, basePath, fullName);
				SdFileSystemInfo sdFileSystemInfo;
				if (!(fileSystemInfo is FileInfo))
				{
					if (!(fileSystemInfo is DirectoryInfo))
					{
						throw new NotImplementedException("Unsupported implementation of FileSystemInfo: " + fileSystemInfo.GetType().FullName + ".");
					}
					sdFileSystemInfo = new SdDirectoryInfo(managedPathFromBaseAndSubPath);
				}
				else
				{
					sdFileSystemInfo = new SdFileInfo(managedPathFromBaseAndSubPath);
				}
				yield return sdFileSystemInfo;
			}
			IEnumerator<FileSystemInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600D9F1 RID: 55793 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SaveGameIOProvider()
		{
		}
	}
}
