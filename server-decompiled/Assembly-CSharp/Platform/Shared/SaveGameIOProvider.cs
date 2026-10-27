using System;
using System.Collections.Generic;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001D13 RID: 7443
	public abstract class SaveGameIOProvider : IPlatformSaveGameIOProvider
	{
		// Token: 0x0600DC6C RID: 56428
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract string GetPath(SaveDataManagedPath path);

		// Token: 0x0600DC6D RID: 56429 RVA: 0x004EA4A0 File Offset: 0x004E86A0
		public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
		{
			using (FileStream fileStream = File.OpenRead(this.GetPath(path)))
			{
				StreamUtils.StreamCopy(fileStream, dest, null, true);
			}
		}

		// Token: 0x0600DC6E RID: 56430 RVA: 0x004EA4E0 File Offset: 0x004E86E0
		public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
		{
			using (FileStream fileStream = File.Open(this.GetPath(path), FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				StreamUtils.StreamCopy(src, fileStream, null, true);
			}
		}

		// Token: 0x0600DC6F RID: 56431 RVA: 0x004EA524 File Offset: 0x004E8724
		public void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false)
		{
			File.Copy(this.GetPath(sourceFileName), this.GetPath(destFileName), overwrite);
		}

		// Token: 0x0600DC70 RID: 56432 RVA: 0x004EA53A File Offset: 0x004E873A
		public void ManagedFileDelete(SaveDataManagedPath path)
		{
			File.Delete(this.GetPath(path));
		}

		// Token: 0x0600DC71 RID: 56433 RVA: 0x004EA548 File Offset: 0x004E8748
		public bool ManagedFileExists(SaveDataManagedPath path)
		{
			return File.Exists(this.GetPath(path));
		}

		// Token: 0x0600DC72 RID: 56434 RVA: 0x004EA556 File Offset: 0x004E8756
		public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			return File.GetLastWriteTimeUtc(this.GetPath(path));
		}

		// Token: 0x0600DC73 RID: 56435 RVA: 0x004EA564 File Offset: 0x004E8764
		public void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName)
		{
			File.Move(this.GetPath(sourceFileName), this.GetPath(destFileName));
		}

		// Token: 0x0600DC74 RID: 56436 RVA: 0x004EA579 File Offset: 0x004E8779
		public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
		{
			return new SdDirectoryInfo(Directory.CreateDirectory(this.GetPath(path)));
		}

		// Token: 0x0600DC75 RID: 56437 RVA: 0x004EA58C File Offset: 0x004E878C
		public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			return Directory.GetLastWriteTimeUtc(this.GetPath(path));
		}

		// Token: 0x0600DC76 RID: 56438 RVA: 0x004EA59A File Offset: 0x004E879A
		public bool ManagedDirectoryExists(SaveDataManagedPath path)
		{
			return Directory.Exists(this.GetPath(path));
		}

		// Token: 0x0600DC77 RID: 56439 RVA: 0x004EA5A8 File Offset: 0x004E87A8
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateDirectories(path2, searchPattern, searchOption));
		}

		// Token: 0x0600DC78 RID: 56440 RVA: 0x004EA5CC File Offset: 0x004E87CC
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateFiles(path2, searchPattern, searchOption));
		}

		// Token: 0x0600DC79 RID: 56441 RVA: 0x004EA5F0 File Offset: 0x004E87F0
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateFileSystemEntries(path2, searchPattern, searchOption));
		}

		// Token: 0x0600DC7A RID: 56442 RVA: 0x004EA614 File Offset: 0x004E8814
		public void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
		{
			Directory.Delete(this.GetPath(path), recursive);
		}

		// Token: 0x0600DC7B RID: 56443 RVA: 0x004EA623 File Offset: 0x004E8823
		public long ManagedFileInfoLength(SaveDataManagedPath path)
		{
			return new FileInfo(this.GetPath(path)).Length;
		}

		// Token: 0x0600DC7C RID: 56444 RVA: 0x004EA636 File Offset: 0x004E8836
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

		// Token: 0x0600DC7D RID: 56445 RVA: 0x004EA65B File Offset: 0x004E885B
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

		// Token: 0x0600DC7E RID: 56446 RVA: 0x004EA680 File Offset: 0x004E8880
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

		// Token: 0x0600DC7F RID: 56447 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SaveGameIOProvider()
		{
		}
	}
}
