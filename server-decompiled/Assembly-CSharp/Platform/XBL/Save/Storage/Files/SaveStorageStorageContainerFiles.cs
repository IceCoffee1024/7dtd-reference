using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage.Files
{
	// Token: 0x02001C50 RID: 7248
	public sealed class SaveStorageStorageContainerFiles : ISaveStorageContainer, IDisposable
	{
		// Token: 0x0600D6C8 RID: 54984 RVA: 0x004D4D99 File Offset: 0x004D2F99
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D6C9 RID: 54985 RVA: 0x004D4DAB File Offset: 0x004D2FAB
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D6CA RID: 54986 RVA: 0x004D4D99 File Offset: 0x004D2F99
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D6CB RID: 54987 RVA: 0x004D4DC0 File Offset: 0x004D2FC0
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

		// Token: 0x0600D6CC RID: 54988 RVA: 0x004D4EA0 File Offset: 0x004D30A0
		public void Dispose()
		{
			this.m_disposed = true;
		}

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x0600D6CD RID: 54989 RVA: 0x004D4EA9 File Offset: 0x004D30A9
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x0600D6CE RID: 54990 RVA: 0x004D4EB1 File Offset: 0x004D30B1
		public string Name
		{
			get
			{
				return this.m_containerName;
			}
		}

		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x0600D6CF RID: 54991 RVA: 0x004D4EB9 File Offset: 0x004D30B9
		public DateTime LastAccessed
		{
			get
			{
				return this.m_lastAccessHelper.Time;
			}
		}

		// Token: 0x0600D6D0 RID: 54992 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600D6D1 RID: 54993 RVA: 0x004D4EC8 File Offset: 0x004D30C8
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

		// Token: 0x0600D6D2 RID: 54994 RVA: 0x004D4F68 File Offset: 0x004D3168
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

		// Token: 0x0600D6D3 RID: 54995 RVA: 0x004D4FE4 File Offset: 0x004D31E4
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

		// Token: 0x0600D6D4 RID: 54996 RVA: 0x004D5124 File Offset: 0x004D3324
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

		// Token: 0x0600D6D5 RID: 54997 RVA: 0x004D5194 File Offset: 0x004D3394
		public void DeleteBlob(string blobName)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				File.Delete(this.GetBlobPath(blobName));
			}
		}

		// Token: 0x0600D6D6 RID: 54998 RVA: 0x004D51DC File Offset: 0x004D33DC
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetBlobPath(string blobName)
		{
			return Path.Join(this.m_containerPath, blobName);
		}

		// Token: 0x0400A382 RID: 41858
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LastAccessHelper m_lastAccessHelper = new LastAccessHelper();

		// Token: 0x0400A383 RID: 41859
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerName;

		// Token: 0x0400A384 RID: 41860
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerPath;

		// Token: 0x0400A385 RID: 41861
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}
}
