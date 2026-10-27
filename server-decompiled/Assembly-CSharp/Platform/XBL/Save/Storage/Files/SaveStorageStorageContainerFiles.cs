using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save.Storage.Files
{
	// Token: 0x02001CB3 RID: 7347
	public sealed class SaveStorageStorageContainerFiles : ISaveStorageContainer, IDisposable
	{
		// Token: 0x0600D955 RID: 55637 RVA: 0x004DD425 File Offset: 0x004DB625
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D956 RID: 55638 RVA: 0x004DD437 File Offset: 0x004DB637
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D957 RID: 55639 RVA: 0x004DD425 File Offset: 0x004DB625
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveStorageStorageContainerFiles] " + text);
		}

		// Token: 0x0600D958 RID: 55640 RVA: 0x004DD44C File Offset: 0x004DB64C
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

		// Token: 0x0600D959 RID: 55641 RVA: 0x004DD52C File Offset: 0x004DB72C
		public void Dispose()
		{
			this.m_disposed = true;
		}

		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x0600D95A RID: 55642 RVA: 0x004DD535 File Offset: 0x004DB735
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x0600D95B RID: 55643 RVA: 0x004DD53D File Offset: 0x004DB73D
		public string Name
		{
			get
			{
				return this.m_containerName;
			}
		}

		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x0600D95C RID: 55644 RVA: 0x004DD545 File Offset: 0x004DB745
		public DateTime LastAccessed
		{
			get
			{
				return this.m_lastAccessHelper.Time;
			}
		}

		// Token: 0x0600D95D RID: 55645 RVA: 0x000027FC File Offset: 0x000009FC
		public void Flush(bool waitForFlush)
		{
		}

		// Token: 0x0600D95E RID: 55646 RVA: 0x004DD554 File Offset: 0x004DB754
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

		// Token: 0x0600D95F RID: 55647 RVA: 0x004DD5F4 File Offset: 0x004DB7F4
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

		// Token: 0x0600D960 RID: 55648 RVA: 0x004DD670 File Offset: 0x004DB870
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

		// Token: 0x0600D961 RID: 55649 RVA: 0x004DD7B0 File Offset: 0x004DB9B0
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

		// Token: 0x0600D962 RID: 55650 RVA: 0x004DD820 File Offset: 0x004DBA20
		public void DeleteBlob(string blobName)
		{
			using (this.m_lastAccessHelper.CreateScope())
			{
				File.Delete(this.GetBlobPath(blobName));
			}
		}

		// Token: 0x0600D963 RID: 55651 RVA: 0x004DD868 File Offset: 0x004DBA68
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetBlobPath(string blobName)
		{
			return Path.Join(this.m_containerPath, blobName);
		}

		// Token: 0x0400A540 RID: 42304
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly LastAccessHelper m_lastAccessHelper = new LastAccessHelper();

		// Token: 0x0400A541 RID: 42305
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerName;

		// Token: 0x0400A542 RID: 42306
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_containerPath;

		// Token: 0x0400A543 RID: 42307
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;
	}
}
