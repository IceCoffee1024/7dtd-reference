using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Platform.Shared;
using Platform.XBL.Save.MasterFileTable;
using Platform.XBL.Save.MasterFileTable.Latest;
using Platform.XBL.Save.Storage;
using Unity.XGamingRuntime;

namespace Platform.XBL.Save
{
	// Token: 0x02001C19 RID: 7193
	public sealed class SaveContainer : IDisposable
	{
		// Token: 0x0600D4D9 RID: 54489 RVA: 0x004CC055 File Offset: 0x004CA255
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: SaveContainer] " + text);
		}

		// Token: 0x0600D4DA RID: 54490 RVA: 0x004CC055 File Offset: 0x004CA255
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: SaveContainer] " + text);
		}

		// Token: 0x0600D4DB RID: 54491 RVA: 0x004CC067 File Offset: 0x004CA267
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string text)
		{
			Log.Warning("[XBL: SaveContainer] " + text);
		}

		// Token: 0x0600D4DC RID: 54492 RVA: 0x004CC079 File Offset: 0x004CA279
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: SaveContainer] " + text);
		}

		// Token: 0x0600D4DD RID: 54493 RVA: 0x004CC08C File Offset: 0x004CA28C
		public SaveContainer(ISaveStorageContainer saveStorageContainer, SizeTracker sizeTracker)
		{
			bool flag = false;
			try
			{
				this.m_mftMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
				this.m_mftBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(true);
				this.m_mftBinaryWriter.SetBaseStream(this.m_mftMemoryStream);
				this.m_saveStorageContainer = saveStorageContainer;
				this.m_sizeTracker = sizeTracker;
				this.m_containerData = new ContainerData();
				XGameSaveBlobInfo[] array;
				if (this.m_saveStorageContainer.TryEnumerateBlobInfos(out array))
				{
					HashSet<string> hashSet = new HashSet<string>();
					foreach (XGameSaveBlobInfo xgameSaveBlobInfo in array)
					{
						hashSet.Add(xgameSaveBlobInfo.Name);
						ulong num;
						if (!SaveContainer.TryConvertToId(xgameSaveBlobInfo.Name, out num))
						{
							SaveContainer.LogWarning(string.Concat(new string[] { "Container '", saveStorageContainer.Name, "' has a blob with non-id name '", xgameSaveBlobInfo.Name, "'. Will be deleted." }));
						}
						else
						{
							this.m_blobIdToInfo[num] = xgameSaveBlobInfo;
						}
					}
					XGameSaveBlobInfo xgameSaveBlobInfo2;
					if (this.m_blobIdToInfo.TryGetValue(18364758544493064720UL, out xgameSaveBlobInfo2))
					{
						using (RefCountedBuffer blob = this.m_saveStorageContainer.GetBlob(xgameSaveBlobInfo2.Name, "$MFT"))
						{
							this.ReadMFT(blob);
							goto IL_0167;
						}
					}
					this.WriteMFT();
					IL_0167:
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add(SaveContainer.IdToString(18364758544493064720UL));
					foreach (Node node4 in this.m_containerData.RootNode.Enumerate(false, true))
					{
						foreach (BlobRef blobRef4 in node4.BlobRefs)
						{
							hashSet2.Add(SaveContainer.IdToString(blobRef4.Id));
						}
					}
					HashSet<string> hashSet3 = new HashSet<string>(hashSet);
					hashSet3.ExceptWith(hashSet2);
					if (hashSet3.Count > 0)
					{
						foreach (string text in hashSet3)
						{
							SaveContainer.LogWarning(string.Concat(new string[] { "Deleting unreachable blob named '", text, "' from container named '", saveStorageContainer.Name, "'." }));
							this.DeleteBlob(text);
						}
					}
					foreach (Node node2 in this.m_containerData.RootNode.Enumerate(false, true))
					{
						IReadOnlyList<BlobRef> blobRefs = node2.BlobRefs;
						List<BlobRef> list = null;
						for (int j = blobRefs.Count - 1; j >= 0; j--)
						{
							BlobRef blobRef2 = blobRefs[j];
							if (!this.m_blobIdToInfo.ContainsKey(blobRef2.Id))
							{
								SaveContainer.LogWarning(string.Format("Remove non-existent {0} in the metadata for node '{1}' from container named '{2}'.", blobRef2, node2.Name, saveStorageContainer.Name));
								if (list == null)
								{
									list = new List<BlobRef>(blobRefs);
								}
								list.RemoveAt(j);
							}
						}
						if (list != null)
						{
							node2.SetBlobRefs(list.ToArray());
						}
					}
					foreach (Node node3 in this.m_containerData.RootNode.Enumerate(false, true))
					{
						IReadOnlyList<BlobRef> blobRefs2 = node3.BlobRefs;
						BlobRef[] array3 = null;
						for (int k = 0; k < blobRefs2.Count; k++)
						{
							BlobRef blobRef3 = blobRefs2[k];
							XGameSaveBlobInfo xgameSaveBlobInfo3;
							if (!this.m_blobIdToInfo.TryGetValue(blobRef3.Id, out xgameSaveBlobInfo3))
							{
								SaveContainer.LogWarning(string.Format("Expected blob info to exist for {0}.", blobRef3));
							}
							else if (blobRef3.Length != xgameSaveBlobInfo3.Size)
							{
								SaveContainer.LogWarning(string.Format("Length of {0} (in node '{1}') is out of sync. Updating to {2}.", blobRef3, node3.Name, xgameSaveBlobInfo3.Size.FormatSize(false)));
								if (array3 == null)
								{
									array3 = blobRefs2.ToArray<BlobRef>();
								}
								array3[k] = new BlobRef
								{
									Id = blobRef3.Id,
									Length = xgameSaveBlobInfo3.Size,
									Hash = blobRef3.Hash
								};
							}
						}
						if (array3 != null)
						{
							node3.SetBlobRefs(array3);
						}
					}
					this.m_blobCache = new BlobCache(saveStorageContainer.Name, this.m_containerData.RootNode.Enumerate(false, true).SelectMany<Node, BlobRef>((Node node) => node.BlobRefs));
					if (LaunchPrefs.GameCoreBlobCache.Value)
					{
						if (LaunchPrefs.GameCoreBlobCacheProactive.Value)
						{
							this.m_blobCacheQueue = (from blobRef in this.m_containerData.RootNode.Enumerate(false, true).SelectMany<Node, BlobRef>((Node node) => node.BlobRefs)
								where !this.m_blobCache.Contains(blobRef)
								orderby blobRef.Length
								select blobRef).Aggregate(new LinkedDictionary<ulong, uint>(), delegate(LinkedDictionary<ulong, uint> queue, BlobRef blobRef)
							{
								queue.Add(blobRef.Id, blobRef.Length);
								return queue;
							});
							if (this.m_blobCacheQueue.Count > 0)
							{
								this.m_blobCacheQueueCancellationTokenSource = new CancellationTokenSource();
								this.m_blobCacheQueueTask = Task.Run(() => this.BlobCacheQueueTask(this.m_blobCacheQueueCancellationTokenSource.Token), this.m_blobCacheQueueCancellationTokenSource.Token);
							}
							else
							{
								SaveContainer.LogInfo("BlobCacheQueueTask does not need to run as there are no uncached blobs.");
							}
						}
						else
						{
							SaveContainer.LogInfo("BlobCacheQueueTask does not need to be run because proactive caching is disabled.");
							this.m_blobCacheQueue = new LinkedDictionary<ulong, uint>();
						}
					}
					else
					{
						SaveContainer.LogInfo("BlobCacheQueueTask does not need to run as the BlobCache is disabled.");
						this.m_blobCacheQueue = new LinkedDictionary<ulong, uint>();
					}
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

		// Token: 0x0600D4DE RID: 54494 RVA: 0x004CC734 File Offset: 0x004CA934
		public void Dispose()
		{
			CancellationTokenSource blobCacheQueueCancellationTokenSource = this.m_blobCacheQueueCancellationTokenSource;
			if (blobCacheQueueCancellationTokenSource != null)
			{
				blobCacheQueueCancellationTokenSource.Cancel();
			}
			Task blobCacheQueueTask = this.m_blobCacheQueueTask;
			if (blobCacheQueueTask != null)
			{
				blobCacheQueueTask.Wait();
			}
			this.m_blobCacheQueueTask = null;
			CancellationTokenSource blobCacheQueueCancellationTokenSource2 = this.m_blobCacheQueueCancellationTokenSource;
			if (blobCacheQueueCancellationTokenSource2 != null)
			{
				blobCacheQueueCancellationTokenSource2.Dispose();
			}
			this.m_blobCacheQueueCancellationTokenSource = null;
			object blobCacheLock = this.m_blobCacheLock;
			lock (blobCacheLock)
			{
				LinkedDictionary<ulong, uint> blobCacheQueue = this.m_blobCacheQueue;
				if (blobCacheQueue != null)
				{
					blobCacheQueue.Clear();
				}
				this.m_blobCacheQueue = null;
				this.m_blobCache = null;
			}
			Dictionary<ulong, XGameSaveBlobInfo> blobIdToInfo = this.m_blobIdToInfo;
			lock (blobIdToInfo)
			{
				Dictionary<ulong, XGameSaveBlobInfo> blobIdToInfo2 = this.m_blobIdToInfo;
				if (blobIdToInfo2 != null)
				{
					blobIdToInfo2.Clear();
				}
				this.m_blobIdToInfo = null;
			}
			ContainerData containerData = this.m_containerData;
			if (containerData != null)
			{
				containerData.Dispose();
			}
			this.m_containerData = null;
			if (this.m_mftBinaryWriter != null)
			{
				MemoryPools.poolBinaryWriter.FreeSync(this.m_mftBinaryWriter);
				this.m_mftBinaryWriter = null;
			}
			if (this.m_mftMemoryStream != null)
			{
				MemoryPools.poolMemoryStream.FreeSync(this.m_mftMemoryStream);
				this.m_mftMemoryStream = null;
			}
			this.m_sizeTracker = null;
			this.m_saveStorageContainer = null;
		}

		// Token: 0x0600D4DF RID: 54495 RVA: 0x004CC874 File Offset: 0x004CAA74
		public bool FileExists(StringSpan relativePath)
		{
			Node node = this.GetNode(relativePath);
			return node != null && node.IsFile();
		}

		// Token: 0x0600D4E0 RID: 54496 RVA: 0x004CC894 File Offset: 0x004CAA94
		public void FileRead(StringSpan relativePath, Stream outputStream)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' does not exist."));
			}
			if (!node.IsFile())
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' is not a file."));
			}
			if (node.Children.Count > 0)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' has children so can not be read from."));
			}
			object blobLock = node.m_blobLock;
			lock (blobLock)
			{
				IReadOnlyList<BlobRef> blobRefs = node.BlobRefs;
				BlobRef[] array = null;
				int i = 0;
				while (i < blobRefs.Count)
				{
					BlobRef blobRef = blobRefs[i];
					object obj = this.m_blobCacheLock;
					RefCountedBuffer refCountedBuffer;
					bool flag3;
					lock (obj)
					{
						flag3 = this.m_blobCache.TryGet(blobRef, out refCountedBuffer);
					}
					if (flag3)
					{
						using (refCountedBuffer)
						{
							outputStream.Write(refCountedBuffer.Span);
							goto IL_01FC;
						}
						goto IL_010C;
					}
					goto IL_010C;
					IL_01FC:
					i++;
					continue;
					IL_010C:
					XGameSaveBlobInfo blobInfoCached = this.GetBlobInfoCached(blobRef.Id);
					using (RefCountedBuffer blob = this.m_saveStorageContainer.GetBlob(blobInfoCached.Name, relativePath))
					{
						if ((long)blob.Length != (long)((ulong)blobInfoCached.Size))
						{
							throw new IOException(string.Format("Expected blob data for blob with name '{0}' and size '{1}' bytes, but was {2} bytes.", blobInfoCached.Name, blobInfoCached.Size, blob.Length));
						}
						outputStream.Write(blob.Span);
						obj = this.m_blobCacheLock;
						BlobRef blobRef2;
						lock (obj)
						{
							blobRef2 = this.m_blobCache.Set(blobRef.Id, blob);
							this.m_blobCacheQueue.Remove(blobRef.Id);
						}
						if (blobRef2 != blobRef)
						{
							if (array == null)
							{
								array = blobRefs.ToArray<BlobRef>();
							}
							array[i] = blobRef2;
						}
					}
					goto IL_01FC;
				}
				if (array != null)
				{
					node.SetBlobRefs(array);
				}
			}
		}

		// Token: 0x0600D4E1 RID: 54497 RVA: 0x004CCB44 File Offset: 0x004CAD44
		public void FileWrite(StringSpan relativePath, Stream inputStream)
		{
			bool flag;
			Node orCreateNode = this.GetOrCreateNode(relativePath, false, false, out flag);
			if (!orCreateNode.IsFile())
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' is not a file."));
			}
			if (orCreateNode.Children.Count > 0)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' has children so can not be written to."));
			}
			long num = inputStream.Length - inputStream.Position;
			ulong[] array = null;
			int num2 = 0;
			bool flag2 = false;
			ulong[] array2;
			try
			{
				object blobLock = orCreateNode.m_blobLock;
				lock (blobLock)
				{
					array2 = orCreateNode.BlobRefs.Select<BlobRef, ulong>((BlobRef blobRef) => blobRef.Id).ToArray<ulong>();
					long num3 = 0L;
					for (int i = 0; i < array2.Length; i++)
					{
						num3 += 28L;
					}
					long num4 = num;
					int num5 = (int)(num4 / 16777216L + ((num4 % 16777216L > 0L) ? 1L : 0L));
					num4 += (long)(28 * num5);
					long num6 = num4 - num3;
					long remaining = this.m_sizeTracker.Sizes.Remaining;
					if (remaining < 0L || num6 > remaining)
					{
						throw new IOException(string.Format("Can not write {0} + {1} bytes to '{2}' because there is only {3} bytes of free space.", new object[]
						{
							num,
							8 * num5,
							relativePath.ToString(),
							remaining
						}));
					}
					array = new ulong[num5];
					BlobRef[] array3 = new BlobRef[array.Length];
					for (int j = 0; j < num5; j++)
					{
						array[j] = this.GenerateNewId();
					}
					long num7 = 0L;
					for (int k = 0; k < num5; k++)
					{
						int num8;
						if (k == num5 - 1)
						{
							num8 = (int)(num - num7);
						}
						else
						{
							num8 = 16777216;
						}
						using (RefCountedBuffer refCountedBuffer = RefCountedBuffer.CreatePooled(num8))
						{
							Span<byte> span = refCountedBuffer.Span;
							int l = 0;
							while (l < span.Length)
							{
								int num9 = inputStream.Read(span.Slice(l, span.Length - l));
								if (num9 <= 0)
								{
									throw new IOException(string.Format("Reached end of stream after {0} bytes saved, expected a total of {1} bytes.", num7, num));
								}
								l += num9;
								num7 += (long)num9;
							}
							ulong num10 = array[k];
							object blobCacheLock = this.m_blobCacheLock;
							lock (blobCacheLock)
							{
								array3[k] = this.m_blobCache.Set(num10, refCountedBuffer);
								this.m_blobCacheQueue.Remove(num10);
							}
							this.SetBlob(num10, refCountedBuffer);
							num2++;
						}
					}
					orCreateNode.LastWriteTimeUtc = DateTime.UtcNow;
					orCreateNode.SetBlobRefs(array3);
					array = null;
					flag2 = true;
				}
			}
			finally
			{
				if (!flag2 && array != null && array.Length != 0)
				{
					for (int m = 0; m < num2; m++)
					{
						try
						{
							this.DeleteBlob(SaveContainer.IdToString(array[m]));
						}
						catch (IOException)
						{
						}
					}
				}
			}
			this.WriteMFT();
			foreach (ulong num11 in array2)
			{
				this.DeleteBlob(SaveContainer.IdToString(num11));
			}
		}

		// Token: 0x0600D4E2 RID: 54498 RVA: 0x004CCF00 File Offset: 0x004CB100
		public void FileDelete(StringSpan relativePath)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				return;
			}
			if (!node.IsFile())
			{
				throw new IOException(SpanUtils.Concat("Node at ", relativePath, "' is not a file."));
			}
			if (node.Children.Count > 0)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' has children so is not a file that can be deleted."));
			}
			if (node.Parent == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' has no parent so might be a root node which can not be deleted."));
			}
			node.Parent.DeleteChildNode(node.Name);
			this.WriteMFT();
			foreach (BlobRef blobRef in node.BlobRefs)
			{
				this.DeleteBlob(SaveContainer.IdToString(blobRef.Id));
			}
			node.Dispose();
		}

		// Token: 0x0600D4E3 RID: 54499 RVA: 0x004CD004 File Offset: 0x004CB204
		public long FileLength(StringSpan relativePath)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' does not exist."));
			}
			long num = 0L;
			object blobLock = node.m_blobLock;
			lock (blobLock)
			{
				foreach (BlobRef blobRef in node.BlobRefs)
				{
					XGameSaveBlobInfo blobInfoCached = this.GetBlobInfoCached(blobRef.Id);
					num += (long)((ulong)blobInfoCached.Size);
				}
			}
			return num;
		}

		// Token: 0x0600D4E4 RID: 54500 RVA: 0x004CD0C0 File Offset: 0x004CB2C0
		public void FileMove(StringSpan sourceRelativePath, StringSpan destRelativePath)
		{
			Node node = this.GetNode(sourceRelativePath);
			if (node == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", sourceRelativePath, "' does not exist."));
			}
			if (!node.IsFile())
			{
				throw new IOException(SpanUtils.Concat("Node at '", sourceRelativePath, "' is not a file."));
			}
			if (node.Children.Count > 0)
			{
				throw new IOException(SpanUtils.Concat("Node at '", sourceRelativePath, "' has children so is not a file that can be moved."));
			}
			if (node.Parent == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", sourceRelativePath, "' has no parent so might be a root node which can not be moved."));
			}
			if (this.GetNode(destRelativePath) != null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", destRelativePath, "' exists, so can't be moved to."));
			}
			Node nodeParent = this.GetNodeParent(destRelativePath);
			if (nodeParent == null)
			{
				throw new IOException(SpanUtils.Concat("Parent of '", destRelativePath, "' does not exist, so can't be moved to."));
			}
			StringSpan stringSpan = destRelativePath.Substring(destRelativePath.LastIndexOf('/') + 1);
			object mftLock = this.m_mftLock;
			lock (mftLock)
			{
				nodeParent.MoveChild(node, stringSpan);
				this.WriteMFT();
			}
		}

		// Token: 0x0600D4E5 RID: 54501 RVA: 0x004CD21C File Offset: 0x004CB41C
		public bool DirectoryExists(StringSpan relativePath)
		{
			Node node = this.GetNode(relativePath);
			return node != null && node.IsDirectory();
		}

		// Token: 0x0600D4E6 RID: 54502 RVA: 0x004CD23C File Offset: 0x004CB43C
		public void DirectoryCreate(StringSpan relativePath)
		{
			bool flag;
			if (!this.GetOrCreateNode(relativePath, true, true, out flag).IsDirectory())
			{
				throw new IOException(SpanUtils.Concat("A non-directory node already exists at '", relativePath, "'."));
			}
			if (!flag)
			{
				return;
			}
			this.WriteMFT();
		}

		// Token: 0x0600D4E7 RID: 54503 RVA: 0x004CD288 File Offset: 0x004CB488
		public void DirectoryDelete(StringSpan relativePath, bool recursive)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' does not exist."));
			}
			if (!node.IsDirectory())
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' is not a directory."));
			}
			if (node.BlobRefs.Count > 0)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' contains blobIds so is considered a file."));
			}
			if (node.Children.Count > 0 && !recursive)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' has children so is not a directory that can be deleted without recursive = true."));
			}
			if (node.Parent == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' has no parent so might be a root node which can not be deleted."));
			}
			node.Parent.DeleteChildNode(node.Name);
			this.WriteMFT();
			if (recursive)
			{
				int num = 0;
				foreach (Node node2 in node.Enumerate(false, true))
				{
					foreach (BlobRef blobRef in node2.BlobRefs)
					{
						this.DeleteBlob(SaveContainer.IdToString(blobRef.Id));
						num++;
					}
				}
			}
			node.Dispose();
		}

		// Token: 0x0600D4E8 RID: 54504 RVA: 0x004CD41C File Offset: 0x004CB61C
		public IEnumerable<string> DirectoryEnumerate(string relativePath, string searchPattern, bool recursive, bool includeDirectories, bool includeFiles)
		{
			return from x in this.DirectoryEnumerate(relativePath, searchPattern, recursive)
				where (x.IsDirectory & includeDirectories) || (x.IsFile & includeFiles)
				select x.RelativePath;
		}

		// Token: 0x0600D4E9 RID: 54505 RVA: 0x004CD480 File Offset: 0x004CB680
		public IEnumerable<PathEnumerationInfo> DirectoryEnumerate(string relativePath, string searchPattern, bool recursive)
		{
			Node baseNode;
			if (string.IsNullOrEmpty(relativePath))
			{
				baseNode = this.m_containerData.RootNode;
			}
			else
			{
				baseNode = this.GetNode(relativePath);
			}
			if (baseNode == null)
			{
				throw new IOException("Node at '" + relativePath + "' does not exist.");
			}
			Func<Node, bool> func;
			if (searchPattern != "*")
			{
				string text = "^" + Regex.Escape(searchPattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
				Regex regex = new Regex(text, RegexOptions.Compiled);
				func = (Node node) => regex.IsMatch(node.Name);
			}
			else
			{
				func = (Node _) => true;
			}
			string baseRelativePath = SaveContainer.<DirectoryEnumerate>g__GetRelativePathFromNode|33_1(baseNode, this.m_containerData.RootNode);
			if (baseRelativePath.Length > 0)
			{
				baseRelativePath += "/";
			}
			return from n in baseNode.Enumerate(false, recursive).Where<Node>(func)
				select new PathEnumerationInfo(baseRelativePath + SaveContainer.<DirectoryEnumerate>g__GetRelativePathFromNode|33_1(n, baseNode), n.IsDirectory(), n.IsFile());
		}

		// Token: 0x0600D4EA RID: 54506 RVA: 0x004CD5C3 File Offset: 0x004CB7C3
		public void SetCreationTimeUtc(StringSpan relativePath, DateTime lastWriteTimeUtc)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' does not exist."));
			}
			node.CreationTimeUtc = lastWriteTimeUtc;
			this.WriteMFT();
		}

		// Token: 0x0600D4EB RID: 54507 RVA: 0x004CD5FC File Offset: 0x004CB7FC
		public DateTime GetCreationTimeUtc(StringSpan relativePath)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				return DateTime.FromFileTimeUtc(0L);
			}
			return node.CreationTimeUtc;
		}

		// Token: 0x0600D4EC RID: 54508 RVA: 0x004CD622 File Offset: 0x004CB822
		public void SetLastWriteTimeUtc(StringSpan relativePath, DateTime lastWriteTimeUtc)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				throw new IOException(SpanUtils.Concat("Node at '", relativePath, "' does not exist."));
			}
			node.LastWriteTimeUtc = lastWriteTimeUtc;
			this.WriteMFT();
		}

		// Token: 0x0600D4ED RID: 54509 RVA: 0x004CD65C File Offset: 0x004CB85C
		public DateTime GetLastWriteTimeUtc(StringSpan relativePath)
		{
			Node node = this.GetNode(relativePath);
			if (node == null)
			{
				return DateTime.FromFileTimeUtc(0L);
			}
			return node.LastWriteTimeUtc;
		}

		// Token: 0x0600D4EE RID: 54510 RVA: 0x004CD684 File Offset: 0x004CB884
		[PublicizedFrom(EAccessModifier.Private)]
		public Node GetNode(StringSpan relativePath)
		{
			Node node = this.m_containerData.RootNode;
			foreach (StringSpan stringSpan in relativePath.GetSplitEnumerator('/', StringSplitOptions.None))
			{
				if (stringSpan.Length != 0)
				{
					node = node.GetChildNode(stringSpan);
					if (node == null)
					{
						return null;
					}
				}
			}
			return node;
		}

		// Token: 0x0600D4EF RID: 54511 RVA: 0x004CD6DC File Offset: 0x004CB8DC
		[PublicizedFrom(EAccessModifier.Private)]
		public Node GetNodeParent(StringSpan relativePath)
		{
			Node node;
			if (relativePath.Length <= 0)
			{
				node = null;
			}
			else
			{
				int num = relativePath.LastIndexOf('/');
				if (num < 0)
				{
					node = this.m_containerData.RootNode;
				}
				else
				{
					StringSpan stringSpan = relativePath;
					node = this.GetNode(stringSpan.Slice(0, num));
				}
			}
			return node;
		}

		// Token: 0x0600D4F0 RID: 54512 RVA: 0x004CD728 File Offset: 0x004CB928
		[PublicizedFrom(EAccessModifier.Private)]
		public Node GetOrCreateNode(StringSpan relativePath, bool createDirectory, bool createParents, out bool wasCreated)
		{
			Node node;
			if (relativePath.Length <= 0)
			{
				node = this.m_containerData.RootNode;
				wasCreated = false;
			}
			else
			{
				int num = relativePath.LastIndexOf('/');
				Node node2;
				StringSpan stringSpan;
				if (num < 0)
				{
					node2 = this.m_containerData.RootNode;
					stringSpan = relativePath;
				}
				else
				{
					StringSpan stringSpan2 = relativePath;
					StringSpan stringSpan3 = stringSpan2.Slice(0, num);
					stringSpan2 = relativePath;
					int num2 = num + 1;
					stringSpan = stringSpan2.Slice(num2, stringSpan2.Length - num2);
					if (!createParents)
					{
						node2 = this.GetNode(stringSpan3);
					}
					else
					{
						Node node3 = this.m_containerData.RootNode;
						foreach (StringSpan stringSpan4 in stringSpan3.GetSplitEnumerator('/', StringSplitOptions.None))
						{
							if (stringSpan4.Length != 0)
							{
								bool flag;
								node3 = node3.GetOrCreateChildNode(stringSpan4, true, out flag);
								if (!node3.IsDirectory())
								{
									throw new IOException(SpanUtils.Concat("Parent '", node3.Name, "' of '", relativePath, "' is not a directory."));
								}
							}
						}
						node2 = node3;
					}
				}
				if (node2 == null)
				{
					throw new IOException(SpanUtils.Concat("Parent of '", relativePath, "' does not exist."));
				}
				if (!node2.IsDirectory())
				{
					throw new IOException(SpanUtils.Concat("Parent of '", relativePath, "' is not a directory."));
				}
				node = node2.GetOrCreateChildNode(stringSpan, createDirectory, out wasCreated);
			}
			return node;
		}

		// Token: 0x0600D4F1 RID: 54513 RVA: 0x004CD898 File Offset: 0x004CBA98
		[PublicizedFrom(EAccessModifier.Private)]
		public XGameSaveBlobInfo GetBlobInfoCached(ulong blobId)
		{
			Dictionary<ulong, XGameSaveBlobInfo> dictionary = this.m_blobIdToInfo;
			lock (dictionary)
			{
				XGameSaveBlobInfo xgameSaveBlobInfo;
				if (this.m_blobIdToInfo.TryGetValue(blobId, out xgameSaveBlobInfo) && xgameSaveBlobInfo != null)
				{
					return xgameSaveBlobInfo;
				}
			}
			string text = SaveContainer.IdToString(blobId);
			XGameSaveBlobInfo blobInfo = this.m_saveStorageContainer.GetBlobInfo(text);
			dictionary = this.m_blobIdToInfo;
			lock (dictionary)
			{
				XGameSaveBlobInfo xgameSaveBlobInfo2;
				if (this.m_blobIdToInfo.TryGetValue(blobId, out xgameSaveBlobInfo2) && xgameSaveBlobInfo2 != null)
				{
					return xgameSaveBlobInfo2;
				}
				if (blobInfo != null)
				{
					this.m_blobIdToInfo[blobId] = blobInfo;
					return blobInfo;
				}
			}
			throw new IOException("Expected to find a blob with the name '" + text + "'");
		}

		// Token: 0x0600D4F2 RID: 54514 RVA: 0x004CD974 File Offset: 0x004CBB74
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetBlob(ulong blobId, RefCountedBuffer blobData)
		{
			string text = SaveContainer.IdToString(blobId);
			this.m_saveStorageContainer.SetBlob(text, blobData);
			XGameSaveBlobInfo xgameSaveBlobInfo = new XGameSaveBlobInfo
			{
				Name = text,
				Size = (uint)blobData.Length
			};
			Dictionary<ulong, XGameSaveBlobInfo> blobIdToInfo = this.m_blobIdToInfo;
			lock (blobIdToInfo)
			{
				XGameSaveBlobInfo xgameSaveBlobInfo2;
				if (this.m_blobIdToInfo.TryGetValue(blobId, out xgameSaveBlobInfo2))
				{
					this.m_sizeTracker.UpdateUsedEstimate((long)blobData.Length - (long)((ulong)xgameSaveBlobInfo2.Size));
				}
				else
				{
					this.m_sizeTracker.UpdateUsedEstimate((long)blobData.Length);
				}
				this.m_blobIdToInfo[blobId] = xgameSaveBlobInfo;
			}
		}

		// Token: 0x0600D4F3 RID: 54515 RVA: 0x004CDA28 File Offset: 0x004CBC28
		[PublicizedFrom(EAccessModifier.Private)]
		public void DeleteBlob(string blobName)
		{
			this.m_saveStorageContainer.DeleteBlob(blobName);
			ulong num;
			if (!SaveContainer.TryConvertToId(blobName, out num))
			{
				return;
			}
			Dictionary<ulong, XGameSaveBlobInfo> blobIdToInfo = this.m_blobIdToInfo;
			lock (blobIdToInfo)
			{
				XGameSaveBlobInfo xgameSaveBlobInfo;
				if (this.m_blobIdToInfo.TryGetValue(num, out xgameSaveBlobInfo))
				{
					this.m_sizeTracker.UpdateUsedEstimate((long)(0UL - (ulong)xgameSaveBlobInfo.Size));
				}
				this.m_blobIdToInfo.Remove(num);
				object blobCacheLock = this.m_blobCacheLock;
				lock (blobCacheLock)
				{
					BlobCache blobCache = this.m_blobCache;
					if (blobCache != null)
					{
						blobCache.Invalidate(num);
					}
					LinkedDictionary<ulong, uint> blobCacheQueue = this.m_blobCacheQueue;
					if (blobCacheQueue != null)
					{
						blobCacheQueue.Remove(num);
					}
				}
			}
		}

		// Token: 0x0600D4F4 RID: 54516 RVA: 0x004CDAFC File Offset: 0x004CBCFC
		[PublicizedFrom(EAccessModifier.Private)]
		public void WriteMFT()
		{
			object mftLock = this.m_mftLock;
			lock (mftLock)
			{
				this.m_mftMemoryStream.Reset();
				this.m_containerData.Write(this.m_mftBinaryWriter);
				if (this.m_mftMemoryStream.Length > 16777216L)
				{
					throw new IOException("The MFT has grown too large to be persisted.");
				}
				int num = (int)this.m_mftMemoryStream.Length;
				using (RefCountedBuffer refCountedBuffer = RefCountedBuffer.CreatePooled(num))
				{
					this.m_mftMemoryStream.GetBuffer().AsSpan<byte>(0, num).CopyTo(refCountedBuffer.Span);
					this.SetBlob(18364758544493064720UL, refCountedBuffer);
				}
			}
		}

		// Token: 0x0600D4F5 RID: 54517 RVA: 0x004CDBCC File Offset: 0x004CBDCC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ReadMFT(RefCountedBuffer buffer)
		{
			using (MemoryStream memoryStream = new MemoryStream(buffer.BufferRaw, buffer.Offset, buffer.Length, false))
			{
				Migrator.ReadMigrate(memoryStream, this.m_containerData, "ContainerData", Migrator.s_containerDataMigrators);
			}
		}

		// Token: 0x0600D4F6 RID: 54518 RVA: 0x004CDC24 File Offset: 0x004CBE24
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong GenerateNewId()
		{
			ulong num = 0UL;
			Span<byte> span = MemoryMarshal.Cast<ulong, byte>(MemoryMarshal.CreateSpan<ulong>(ref num, 1));
			Random value = Platform.Shared.Utils.RandLocal.Value;
			Dictionary<ulong, XGameSaveBlobInfo> blobIdToInfo = this.m_blobIdToInfo;
			ulong num2;
			lock (blobIdToInfo)
			{
				do
				{
					value.NextBytes(span);
				}
				while (this.m_blobIdToInfo.ContainsKey(num));
				num2 = num;
			}
			return num2;
		}

		// Token: 0x0600D4F7 RID: 54519 RVA: 0x004CDC98 File Offset: 0x004CBE98
		public static string IdToString(ulong id)
		{
			return FormattableString.Invariant(FormattableStringFactory.Create("{0:x16}", new object[] { id }));
		}

		// Token: 0x0600D4F8 RID: 54520 RVA: 0x004CDCB8 File Offset: 0x004CBEB8
		public static bool TryConvertToId(string input, out ulong id)
		{
			id = 0UL;
			return input.Length == 16 && input.Equals(input.ToLowerInvariant(), StringComparison.InvariantCulture) && ulong.TryParse(input, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out id);
		}

		// Token: 0x0600D4F9 RID: 54521 RVA: 0x004CDCEC File Offset: 0x004CBEEC
		[PublicizedFrom(EAccessModifier.Private)]
		public async Task BlobCacheQueueTask(CancellationToken cancellationToken)
		{
			SaveContainer.<>c__DisplayClass49_0 CS$<>8__locals1 = new SaveContainer.<>c__DisplayClass49_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cancellationToken = cancellationToken;
			SaveContainer.LogInfo(string.Format("{0} Started with {1} blobs in queue.", "BlobCacheQueueTask", this.m_blobCacheQueue.Count));
			CS$<>8__locals1.blobIdsToCache = new List<ulong>();
			CS$<>8__locals1.totalActiveTime = TimeSpan.Zero;
			CS$<>8__locals1.totalBytesCached = 0UL;
			try
			{
				while (!CS$<>8__locals1.cancellationToken.IsCancellationRequested)
				{
					try
					{
						ValueTaskAwaiter<bool> valueTaskAwaiter = CS$<>8__locals1.<BlobCacheQueueTask>g__InnerLoop|0().GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							await valueTaskAwaiter;
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						}
						if (!valueTaskAwaiter.GetResult())
						{
							break;
						}
					}
					catch (Exception ex)
					{
						if (!(ex is TaskCanceledException) && !(ex is OperationCanceledException))
						{
							SaveContainer.LogError(string.Format("{0} Exception: {1}", "BlobCacheQueueTask", ex));
						}
					}
				}
			}
			finally
			{
				SaveContainer.LogInfo(string.Format("{0} Finished {1} with total active time {2:F3} s and {3} cached.", new object[]
				{
					"BlobCacheQueueTask",
					(this.m_blobCacheQueue.Count > 0) ? string.Format("with {0} remaining blobs in queue", this.m_blobCacheQueue.Count) : "",
					CS$<>8__locals1.totalActiveTime.TotalSeconds,
					CS$<>8__locals1.totalBytesCached.FormatSize(false)
				}));
			}
		}

		// Token: 0x0600D4FD RID: 54525 RVA: 0x004CDD70 File Offset: 0x004CBF70
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static string <DirectoryEnumerate>g__GetRelativePathFromNode|33_1(Node currentNode, Node baseNode)
		{
			List<Node> list = new List<Node>();
			while (currentNode.Parent != null && currentNode != baseNode)
			{
				list.Add(currentNode);
				currentNode = currentNode.Parent;
			}
			return string.Join<string>('/', list.Select<Node, string>((Node n) => n.Name).Reverse<string>());
		}

		// Token: 0x0400A216 RID: 41494
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxBlobSize = 16777216;

		// Token: 0x0400A217 RID: 41495
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxNodeBlobSize = 16777216;

		// Token: 0x0400A218 RID: 41496
		public const ulong RootNodeId = 18364758544493064720UL;

		// Token: 0x0400A219 RID: 41497
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxBlobCacheReadSize = 1048576;

		// Token: 0x0400A21A RID: 41498
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly TimeSpan BlobCacheInactivityThreshold = TimeSpan.FromSeconds(1.0);

		// Token: 0x0400A21B RID: 41499
		[PublicizedFrom(EAccessModifier.Private)]
		public SizeTracker m_sizeTracker;

		// Token: 0x0400A21C RID: 41500
		[PublicizedFrom(EAccessModifier.Private)]
		public ISaveStorageContainer m_saveStorageContainer;

		// Token: 0x0400A21D RID: 41501
		[PublicizedFrom(EAccessModifier.Private)]
		public ContainerData m_containerData;

		// Token: 0x0400A21E RID: 41502
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_mftLock = new object();

		// Token: 0x0400A21F RID: 41503
		[PublicizedFrom(EAccessModifier.Private)]
		public PooledExpandableMemoryStream m_mftMemoryStream;

		// Token: 0x0400A220 RID: 41504
		[PublicizedFrom(EAccessModifier.Private)]
		public PooledBinaryWriter m_mftBinaryWriter;

		// Token: 0x0400A221 RID: 41505
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<ulong, XGameSaveBlobInfo> m_blobIdToInfo = new Dictionary<ulong, XGameSaveBlobInfo>();

		// Token: 0x0400A222 RID: 41506
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_blobCacheLock = new object();

		// Token: 0x0400A223 RID: 41507
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobCache m_blobCache;

		// Token: 0x0400A224 RID: 41508
		[PublicizedFrom(EAccessModifier.Private)]
		public LinkedDictionary<ulong, uint> m_blobCacheQueue;

		// Token: 0x0400A225 RID: 41509
		[PublicizedFrom(EAccessModifier.Private)]
		public CancellationTokenSource m_blobCacheQueueCancellationTokenSource;

		// Token: 0x0400A226 RID: 41510
		[PublicizedFrom(EAccessModifier.Private)]
		public Task m_blobCacheQueueTask;
	}
}
