using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage.Files
{
	// Token: 0x02001C82 RID: 7298
	public sealed class SaveStorageStorageContainerFiles : ISaveStorageContainer, IDisposable
	{
		// Token: 0x0600D841 RID: 55361 RVA: 0x004D98D5 File Offset: 0x004D7AD5
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D842 RID: 55362 RVA: 0x004D98E7 File Offset: 0x004D7AE7
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D843 RID: 55363 RVA: 0x004D98D5 File Offset: 0x004D7AD5
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D844 RID: 55364 RVA: 0x004D98FC File Offset: 0x004D7AFC
		public SaveStorageStorageContainerFiles(string gameSaveFolder, string containerName)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				bool flag = false;
				try
				{
					this.m_containerName = containerName;
					this.m_containerPath = Path.GetFullPath(Path.Join(gameSaveFolder, this.m_containerName));
					Directory.CreateDirectory(this.m_containerPath);
					if (!Directory.Exists(this.m_containerPath))
					{
						SaveStorageStorageContainerFiles.LogError("Container '" + containerName + "' failed to create directory at: " + this.m_containerPath);
					}
					else
					{
						SaveStorageStorageContainerFiles.LogInfo("Container '" + containerName + "' exists at: " + this.m_containerPath);
						flag = true;
					}
				}
				finally
				{
					if (!flag)
					{
						this.Dispose();
					}
				}
			}
		}

		// Token: 0x0600D845 RID: 55365 RVA: 0x004D99DC File Offset: 0x004D7BDC
		public void Dispose()
		{
			this.m_disposed = true;
		}

		// Token: 0x17001B41 RID: 6977
		// (get) Token: 0x0600D846 RID: 55366 RVA: 0x004D99E5 File Offset: 0x004D7BE5
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x17001B42 RID: 6978
		// (get) Token: 0x0600D847 RID: 55367 RVA: 0x004D99ED File Offset: 0x004D7BED
		public string Name
		{
			get
			{
				return this.m_containerName;
			}
		}

		// Token: 0x17001B43 RID: 6979
		// (get) Token: 0x0600D848 RID: 55368 RVA: 0x004D99F5 File Offset: 0x004D7BF5
		public DateTime LastAccessed
		{
			get
			{
				return this.m_lastAccessHelper.Time;
			}
		}

		// Token: 0x0600D849 RID: 55369 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600D84A RID: 55370 RVA: 0x004D9A04 File Offset: 0x004D7C04
		public bool TryEnumerateBlobInfos(out XGameSaveBlobInfo[] blobInfos)
		{
			bool flag;
			using (this.m_lastAccessHelper.CreateScope())
			{
				try
				{
					blobInfos = (from fi in new DirectoryInfo(this.m_containerPath).EnumerateFiles()
						select new XGameSaveBlobInfo
						{
							Name = fi.Name,
							Size = (uint)fi.Length
						}).ToArray<XGameSaveBlobInfo>();
					flag = true;
				}
				catch (IOException ex)
				{
					SaveStorageStorageContainerFiles.LogError(string.Format("{0} failed: {1}", "TryEnumerateBlobInfos", ex));
					blobInfos = null;
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600D84B RID: 55371 RVA: 0x004D9AA4 File Offset: 0x004D7CA4
		public XGameSaveBlobInfo GetBlobInfo(string blobName)
		{
			XGameSaveBlobInfo xgameSaveBlobInfo;
			using (this.m_lastAccessHelper.CreateScope())
			{
				FileInfo fileInfo = new FileInfo(Path.Join(this.m_containerPath, blobName));
				if (!fileInfo.Exists)
				{
					xgameSaveBlobInfo = null;
				}
				else
				{
					xgameSaveBlobInfo = new XGameSaveBlobInfo
					{
						Name = blobName,
						Size = (uint)fileInfo.Length
					};
				}
			}
			return xgameSaveBlobInfo;
		}

		// Token: 0x0600D84C RID: 55372 RVA: 0x004D9B20 File Offset: 0x004D7D20
		public RefCountedBuffer[] GetBlobs(string[] blobNames, StringSpan debugIdentifier)
		{
			RefCountedBuffer[] array2;
			using (this.m_lastAccessHelper.CreateScope())
			{
				bool flag = false;
				RefCountedBuffer[] array = new RefCountedBuffer[blobNames.Length];
				try
				{
					for (int i = 0; i < blobNames.Length; i++)
					{
						string text = blobNames[i];
						using (FileStream fileStream = File.Open(this.GetBlobPath(text), FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							int num = (int)fileStream.Length;
							using (RefCountedBuffer refCountedBuffer = RefCountedBuffer.CreatePooled(num))
							{
								int num2;
								for (int j = 0; j < num; j += num2)
								{
									num2 = fileStream.Read(refCountedBuffer.Span.Slice(j, num - j));
									if (num2 <= 0)
									{
										throw new IOException("Unexpected end of file stream.");
									}
								}
								array[i] = refCountedBuffer.CreateRef();
							}
						}
					}
					flag = true;
					array2 = array;
				}
				finally
				{
					if (!flag)
					{
						foreach (RefCountedBuffer refCountedBuffer2 in array)
						{
							if (refCountedBuffer2 != null)
							{
								refCountedBuffer2.Dispose();
							}
						}
					}
				}
			}
			return array2;
		}

		// Token: 0x0600D84D RID: 55373 RVA: 0x004D9C60 File Offset: 0x004D7E60
		public void SetBlob(string blobName, RefCountedBuffer blobData)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				using (FileStream fileStream = File.Open(this.GetBlobPath(blobName), FileMode.Create, FileAccess.Write))
				{
					fileStream.Write(blobData.Span);
				}
			}
		}

		// Token: 0x0600D84E RID: 55374 RVA: 0x004D9CD0 File Offset: 0x004D7ED0
		public void DeleteBlob(string blobName)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				File.Delete(this.GetBlobPath(blobName));
			}
		}

		// Token: 0x0600D84F RID: 55375 RVA: 0x004D9D18 File Offset: 0x004D7F18
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetBlobPath(string blobName)
		{
			return Path.Join(this.m_containerPath, blobName);
		}

		// Token: 0x0400A457 RID: 42071
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LastAccessHelper m_lastAccessHelper = new LastAccessHelper();

		// Token: 0x0400A458 RID: 42072
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerName;

		// Token: 0x0400A459 RID: 42073
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerPath;

		// Token: 0x0400A45A RID: 42074
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}
}
