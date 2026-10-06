using System;
using System.Collections.Generic;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CE2 RID: 7394
	public abstract class SaveGameIOProvider : IPlatformSaveGameIOProvider
	{
		// Token: 0x0600DB58 RID: 56152
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract string GetPath(SaveDataManagedPath path);

		// Token: 0x0600DB59 RID: 56153 RVA: 0x004E6900 File Offset: 0x004E4B00
		public void ManagedFileRead(SaveDataManagedPath path, Stream dest)
		{
			using (FileStream fileStream = File.OpenRead(this.GetPath(path)))
			{
				StreamUtils.StreamCopy(fileStream, dest, null, true);
			}
		}

		// Token: 0x0600DB5A RID: 56154 RVA: 0x004E6940 File Offset: 0x004E4B40
		public void ManagedFileWrite(SaveDataManagedPath path, Stream src)
		{
			using (FileStream fileStream = File.Open(this.GetPath(path), FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				StreamUtils.StreamCopy(src, fileStream, null, true);
			}
		}

		// Token: 0x0600DB5B RID: 56155 RVA: 0x004E6984 File Offset: 0x004E4B84
		public void ManagedFileCopy(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName, bool overwrite = false)
		{
			File.Copy(this.GetPath(sourceFileName), this.GetPath(destFileName), overwrite);
		}

		// Token: 0x0600DB5C RID: 56156 RVA: 0x004E699A File Offset: 0x004E4B9A
		public void ManagedFileDelete(SaveDataManagedPath path)
		{
			File.Delete(this.GetPath(path));
		}

		// Token: 0x0600DB5D RID: 56157 RVA: 0x004E69A8 File Offset: 0x004E4BA8
		public bool ManagedFileExists(SaveDataManagedPath path)
		{
			return File.Exists(this.GetPath(path));
		}

		// Token: 0x0600DB5E RID: 56158 RVA: 0x004E69B6 File Offset: 0x004E4BB6
		public DateTime ManagedFileGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			return File.GetLastWriteTimeUtc(this.GetPath(path));
		}

		// Token: 0x0600DB5F RID: 56159 RVA: 0x004E69C4 File Offset: 0x004E4BC4
		public void ManagedFileMove(SaveDataManagedPath sourceFileName, SaveDataManagedPath destFileName)
		{
			File.Move(this.GetPath(sourceFileName), this.GetPath(destFileName));
		}

		// Token: 0x0600DB60 RID: 56160 RVA: 0x004E69D9 File Offset: 0x004E4BD9
		public SdDirectoryInfo ManagedDirectoryCreateDirectory(SaveDataManagedPath path)
		{
			return new SdDirectoryInfo(Directory.CreateDirectory(this.GetPath(path)));
		}

		// Token: 0x0600DB61 RID: 56161 RVA: 0x004E69EC File Offset: 0x004E4BEC
		public DateTime ManagedDirectoryGetLastWriteTimeUtc(SaveDataManagedPath path)
		{
			return Directory.GetLastWriteTimeUtc(this.GetPath(path));
		}

		// Token: 0x0600DB62 RID: 56162 RVA: 0x004E69FA File Offset: 0x004E4BFA
		public bool ManagedDirectoryExists(SaveDataManagedPath path)
		{
			return Directory.Exists(this.GetPath(path));
		}

		// Token: 0x0600DB63 RID: 56163 RVA: 0x004E6A08 File Offset: 0x004E4C08
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateDirectories(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateDirectories(path2, searchPattern, searchOption));
		}

		// Token: 0x0600DB64 RID: 56164 RVA: 0x004E6A2C File Offset: 0x004E4C2C
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFiles(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateFiles(path2, searchPattern, searchOption));
		}

		// Token: 0x0600DB65 RID: 56165 RVA: 0x004E6A50 File Offset: 0x004E4C50
		public IEnumerable<SaveDataManagedPath> ManagedDirectoryEnumerateFileSystemEntries(SaveDataManagedPath path, string searchPattern, SearchOption searchOption)
		{
			string path2 = this.GetPath(path);
			return SaveGameProviderHelper.GetManagedPathsFromBaseAndSubPaths(path, path2, Directory.EnumerateFileSystemEntries(path2, searchPattern, searchOption));
		}

		// Token: 0x0600DB66 RID: 56166 RVA: 0x004E6A74 File Offset: 0x004E4C74
		public void ManagedDirectoryDelete(SaveDataManagedPath path, bool recursive)
		{
			Directory.Delete(this.GetPath(path), recursive);
		}

		// Token: 0x0600DB67 RID: 56167 RVA: 0x004E6A83 File Offset: 0x004E4C83
		public long ManagedFileInfoLength(SaveDataManagedPath path)
		{
			return new FileInfo(this.GetPath(path)).Length;
		}

		// Token: 0x0600DB68 RID: 56168 RVA: 0x004E6A96 File Offset: 0x004E4C96
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

		// Token: 0x0600DB69 RID: 56169 RVA: 0x004E6ABB File Offset: 0x004E4CBB
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

		// Token: 0x0600DB6A RID: 56170 RVA: 0x004E6AE0 File Offset: 0x004E4CE0
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

		// Token: 0x0600DB6B RID: 56171 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SaveGameIOProvider()
		{
		}
	}
}
