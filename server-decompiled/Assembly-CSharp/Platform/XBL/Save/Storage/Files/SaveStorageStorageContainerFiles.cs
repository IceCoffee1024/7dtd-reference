using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage.Files
{
	// Token: 0x02001C29 RID: 7209
	public sealed class SaveStorageStorageContainerFiles : ISaveStorageContainer, IDisposable
	{
		// Token: 0x0600D538 RID: 54584 RVA: 0x004CE975 File Offset: 0x004CCB75
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D539 RID: 54585 RVA: 0x004CE987 File Offset: 0x004CCB87
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D53A RID: 54586 RVA: 0x004CE975 File Offset: 0x004CCB75
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D53B RID: 54587 RVA: 0x004CE99C File Offset: 0x004CCB9C
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

		// Token: 0x0600D53C RID: 54588 RVA: 0x004CEA7C File Offset: 0x004CCC7C
		public void Dispose()
		{
			this.m_disposed = true;
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x0600D53D RID: 54589 RVA: 0x004CEA85 File Offset: 0x004CCC85
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x0600D53E RID: 54590 RVA: 0x004CEA8D File Offset: 0x004CCC8D
		public string Name
		{
			get
			{
				return this.m_containerName;
			}
		}

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x0600D53F RID: 54591 RVA: 0x004CEA95 File Offset: 0x004CCC95
		public DateTime LastAccessed
		{
			get
			{
				return this.m_lastAccessHelper.Time;
			}
		}

		// Token: 0x0600D540 RID: 54592 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600D541 RID: 54593 RVA: 0x004CEAA4 File Offset: 0x004CCCA4
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

		// Token: 0x0600D542 RID: 54594 RVA: 0x004CEB44 File Offset: 0x004CCD44
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

		// Token: 0x0600D543 RID: 54595 RVA: 0x004CEBC0 File Offset: 0x004CCDC0
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

		// Token: 0x0600D544 RID: 54596 RVA: 0x004CED00 File Offset: 0x004CCF00
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

		// Token: 0x0600D545 RID: 54597 RVA: 0x004CED70 File Offset: 0x004CCF70
		public void DeleteBlob(string blobName)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				File.Delete(this.GetBlobPath(blobName));
			}
		}

		// Token: 0x0600D546 RID: 54598 RVA: 0x004CEDB8 File Offset: 0x004CCFB8
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetBlobPath(string blobName)
		{
			return Path.Join(this.m_containerPath, blobName);
		}

		// Token: 0x0400A251 RID: 41553
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LastAccessHelper m_lastAccessHelper = new LastAccessHelper();

		// Token: 0x0400A252 RID: 41554
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerName;

		// Token: 0x0400A253 RID: 41555
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerPath;

		// Token: 0x0400A254 RID: 41556
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}
}
