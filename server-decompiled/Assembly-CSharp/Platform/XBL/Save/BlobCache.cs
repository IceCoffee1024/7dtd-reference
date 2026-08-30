using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using Platform.XBL.Save.MasterFileTable.Latest;

namespace Platform.XBL.Save
{
	// Token: 0x02001C12 RID: 7186
	public class BlobCache
	{
		// Token: 0x0600D4BC RID: 54460 RVA: 0x004CB5AD File Offset: 0x004C97AD
		[Conditional("DEBUG_SAVE_DATA_MANAGER")]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogTrace(string text)
		{
			Log.Out("[XBL: BlobCache] " + text);
		}

		// Token: 0x0600D4BD RID: 54461 RVA: 0x004CB5AD File Offset: 0x004C97AD
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogInfo(string text)
		{
			Log.Out("[XBL: BlobCache] " + text);
		}

		// Token: 0x0600D4BE RID: 54462 RVA: 0x004CB5BF File Offset: 0x004C97BF
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogWarning(string text)
		{
			Log.Warning("[XBL: BlobCache] " + text);
		}

		// Token: 0x0600D4BF RID: 54463 RVA: 0x004CB5D1 File Offset: 0x004C97D1
		[PublicizedFrom(EAccessModifier.Private)]
		public static void LogError(string text)
		{
			Log.Error("[XBL: BlobCache] " + text);
		}

		// Token: 0x0600D4C0 RID: 54464 RVA: 0x004CB5E4 File Offset: 0x004C97E4
		public BlobCache(string containerName, IEnumerable<BlobRef> expectedBlobRefs)
		{
			if (!LaunchPrefs.GameCoreBlobCache.Value)
			{
				return;
			}
			MicroStopwatch microStopwatch = new MicroStopwatch(true);
			this.m_root = GameIO.GetNormalizedPath(Path.Combine(GameIO.GetNormalizedPath(GameIO.GetDeviceLocalUserGameDataDir()) + "BlobCache", containerName));
			Directory.CreateDirectory(this.m_root);
			this.m_blobRefs = expectedBlobRefs.ToDictionary<BlobRef, ulong, BlobRef>((BlobRef blobRef) => blobRef.Id, (BlobRef blobRef) => blobRef);
			this.m_inUse = new ConcurrentDictionary<ulong, byte>();
			HashSet<ulong> cachedIds = new HashSet<ulong>();
			foreach (string text in Directory.EnumerateDirectories(this.m_root))
			{
				Directory.Delete(text, true);
			}
			HashAlgorithm hashAlgorithm = BlobRef.GetHashAlgorithm();
			foreach (FileInfo fileInfo in new DirectoryInfo(this.m_root).EnumerateFiles())
			{
				string name = fileInfo.Name;
				ulong num;
				BlobRef blobRef2;
				if (!SaveContainer.TryConvertToId(name, out num))
				{
					BlobCache.LogWarning("Invalid cached blob name '" + name + "'.");
				}
				else if (!this.m_blobRefs.TryGetValue(num, out blobRef2))
				{
					BlobCache.LogWarning("Extraneous cached blob '" + name + "'.");
					fileInfo.Delete();
				}
				else if (fileInfo.Length != (long)((ulong)blobRef2.Length))
				{
					BlobCache.LogWarning(string.Format("Length mismatch for blob '{0}'. {1} != {2}", name, fileInfo.Length, blobRef2.Length));
					this.m_blobRefs.Remove(blobRef2.Id);
					fileInfo.Delete();
				}
				else
				{
					byte[] array = null;
					using (FileStream fileStream = fileInfo.OpenRead())
					{
						if (fileStream.Length == (long)((ulong)blobRef2.Length))
						{
							array = hashAlgorithm.ComputeHash(fileStream);
						}
					}
					if (array == null)
					{
						BlobCache.LogWarning(string.Format("Length mismatch for blob '{0}'. {1} != {2}", name, fileInfo.Length, blobRef2.Length));
						this.m_blobRefs.Remove(blobRef2.Id);
						fileInfo.Delete();
					}
					else if (!blobRef2.Hash.Span.SequenceEqual<byte>(array))
					{
						BlobCache.LogWarning(string.Concat(new string[]
						{
							"Hash mismatch for '",
							name,
							"'. ",
							array.ToHexString(),
							" != ",
							blobRef2.Hash.ToHexString()
						}));
						this.m_blobRefs.Remove(blobRef2.Id);
						fileInfo.Delete();
					}
					else
					{
						cachedIds.Add(num);
					}
				}
			}
			this.m_blobRefs.RemoveAll<ulong, BlobRef>((ulong blobId) => !cachedIds.Contains(blobId));
			BlobCache.LogInfo(string.Format("Cached validated in {0:F3} ms.", microStopwatch.Elapsed.TotalMilliseconds));
		}

		// Token: 0x0600D4C1 RID: 54465 RVA: 0x004CB978 File Offset: 0x004C9B78
		public bool Contains(BlobRef requestedRef)
		{
			if (!LaunchPrefs.GameCoreBlobCache.Value)
			{
				return false;
			}
			object blobRefsLock = this.m_blobRefsLock;
			BlobRef blobRef;
			lock (blobRefsLock)
			{
				if (!this.m_blobRefs.TryGetValue(requestedRef.Id, out blobRef))
				{
					return false;
				}
			}
			return !(requestedRef != blobRef);
		}

		// Token: 0x0600D4C2 RID: 54466 RVA: 0x004CB9E8 File Offset: 0x004C9BE8
		public bool TryGet(BlobRef requestedRef, out RefCountedBuffer buffer)
		{
			if (!LaunchPrefs.GameCoreBlobCache.Value)
			{
				buffer = null;
				return false;
			}
			object blobRefsLock = this.m_blobRefsLock;
			BlobRef blobRef;
			lock (blobRefsLock)
			{
				if (!this.m_blobRefs.TryGetValue(requestedRef.Id, out blobRef))
				{
					buffer = null;
					return false;
				}
			}
			if (requestedRef != blobRef)
			{
				buffer = null;
				return false;
			}
			ulong id = blobRef.Id;
			string text;
			bool flag2;
			using (this.GetCachedPath(id, out text))
			{
				if (!File.Exists(text))
				{
					BlobCache.LogWarning(string.Format("Cache MISS Expected {0} to exist in the cache?", blobRef));
					this.Invalidate(id);
					buffer = null;
					flag2 = false;
				}
				else
				{
					buffer = null;
					try
					{
						using (FileStream fileStream = File.OpenRead(text))
						{
							if (fileStream.Length != (long)((ulong)blobRef.Length))
							{
								BlobCache.LogWarning(string.Format("Cache MISS Unexpected length of {0} for {1}.", fileStream.Length, blobRef));
								this.Invalidate(id);
								buffer = null;
								flag2 = false;
							}
							else
							{
								buffer = RefCountedBuffer.CreatePooled((int)fileStream.Length);
								int num = 0;
								while ((long)num < fileStream.Length)
								{
									Stream stream = fileStream;
									Span<byte> span = buffer.Span;
									int num2 = num;
									int num3 = stream.Read(span.Slice(num2, span.Length - num2));
									if (num3 <= 0)
									{
										throw new IOException(string.Format("Expected {0} bytes but only read {1}.", fileStream.Length, num));
									}
									num += num3;
								}
								flag2 = true;
							}
						}
					}
					catch (Exception ex)
					{
						BlobCache.LogError(string.Format("Cache MISS Failed to read {0}: {1}", blobRef, ex));
						this.Invalidate(id);
						RefCountedBuffer refCountedBuffer = buffer;
						if (refCountedBuffer != null)
						{
							refCountedBuffer.Dispose();
						}
						buffer = null;
						flag2 = false;
					}
				}
			}
			return flag2;
		}

		// Token: 0x0600D4C3 RID: 54467 RVA: 0x004CBC08 File Offset: 0x004C9E08
		public BlobRef Set(ulong blobId, RefCountedBuffer buffer)
		{
			BlobRef blobRef = new BlobRef
			{
				Id = blobId,
				Length = (uint)buffer.Length,
				Hash = BlobRef.CalculateHash(buffer)
			};
			if (!LaunchPrefs.GameCoreBlobCache.Value)
			{
				return blobRef;
			}
			object obj = this.m_blobRefsLock;
			lock (obj)
			{
				BlobRef blobRef2;
				if (this.m_blobRefs.TryGetValue(blobId, out blobRef2) && blobRef == blobRef2)
				{
					return blobRef2;
				}
			}
			string text;
			BlobRef blobRef3;
			using (this.GetCachedPath(blobId, out text))
			{
				try
				{
					using (FileStream fileStream = File.Create(text))
					{
						fileStream.Write(buffer.Span);
					}
				}
				catch (Exception ex)
				{
					BlobCache.LogError(string.Format("Cache FAIL {0}: {1}", blobId, ex));
					this.Invalidate(blobId);
					return blobRef;
				}
				obj = this.m_blobRefsLock;
				lock (obj)
				{
					this.m_blobRefs[blobId] = blobRef;
				}
				blobRef3 = blobRef;
			}
			return blobRef3;
		}

		// Token: 0x0600D4C4 RID: 54468 RVA: 0x004CBD64 File Offset: 0x004C9F64
		public void Invalidate(ulong blobId)
		{
			if (!LaunchPrefs.GameCoreBlobCache.Value)
			{
				return;
			}
			object blobRefsLock = this.m_blobRefsLock;
			lock (blobRefsLock)
			{
				if (!this.m_blobRefs.Remove(blobId))
				{
					return;
				}
			}
			try
			{
				string text;
				using (this.GetCachedPath(blobId, out text))
				{
					File.Delete(text);
				}
			}
			catch (Exception ex)
			{
				BlobCache.LogError(string.Format("Failed to invalidate '{0}': {1}", blobId, ex));
			}
		}

		// Token: 0x0600D4C5 RID: 54469 RVA: 0x004CBE14 File Offset: 0x004CA014
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobCache.FileScope GetCachedPath(ulong blobId, out string filePath)
		{
			BlobCache.FileScope fileScope = new BlobCache.FileScope(this, blobId);
			filePath = null;
			BlobCache.FileScope fileScope2;
			try
			{
				filePath = GameIO.GetNormalizedPath(Path.Join(this.m_root, SaveContainer.IdToString(blobId)));
				fileScope2 = fileScope;
			}
			finally
			{
				if (filePath == null)
				{
					fileScope.Dispose();
				}
			}
			return fileScope2;
		}

		// Token: 0x0400A204 RID: 41476
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string m_root;

		// Token: 0x0400A205 RID: 41477
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly object m_blobRefsLock = new object();

		// Token: 0x0400A206 RID: 41478
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ulong, BlobRef> m_blobRefs;

		// Token: 0x0400A207 RID: 41479
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ConcurrentDictionary<ulong, byte> m_inUse;

		// Token: 0x02001C13 RID: 7187
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly struct FileScope : IDisposable
		{
			// Token: 0x0600D4C6 RID: 54470 RVA: 0x004CBE70 File Offset: 0x004CA070
			public FileScope(BlobCache parent, ulong blobId)
			{
				this.m_parent = parent;
				this.m_blobId = blobId;
				while (!this.m_parent.m_inUse.TryAdd(this.m_blobId, 0))
				{
					Thread.Sleep(0);
				}
			}

			// Token: 0x0600D4C7 RID: 54471 RVA: 0x004CBEA4 File Offset: 0x004CA0A4
			public void Dispose()
			{
				byte b;
				if (!this.m_parent.m_inUse.TryRemove(this.m_blobId, out b))
				{
					BlobCache.LogError(string.Format("Expected to release '{0}'.", this.m_blobId));
				}
			}

			// Token: 0x0400A208 RID: 41480
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly BlobCache m_parent;

			// Token: 0x0400A209 RID: 41481
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly ulong m_blobId;
		}
	}
}
