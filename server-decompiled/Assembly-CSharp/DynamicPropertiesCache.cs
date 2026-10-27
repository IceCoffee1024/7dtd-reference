using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using MemoryPack;
using Platform;

// Token: 0x02001422 RID: 5154
public class DynamicPropertiesCache
{
	// Token: 0x0600A153 RID: 41299 RVA: 0x003CAEAC File Offset: 0x003C90AC
	public DynamicPropertiesCache()
	{
		Log.Out(string.Format("[BLOCKPROPERTIES] Creating DynamicProperties Cache, max cache size {0}", 1000));
		this.m_filePath = PlatformManager.NativePlatform.Utils.GetTempFileName("dpc", ".dpc");
		this.m_fileStream = new FileStream(this.m_filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 4096, FileOptions.DeleteOnClose);
		this.m_buffer = new ArrayBufferWriter<byte>(65536);
		this.m_cache = new Dictionary<int, DynamicProperties>(1000);
		this.m_queue = new LinkedList<int>();
		this.offsetsAndLengths = new ValueTuple<long, int>[Block.MAX_BLOCKS];
	}

	// Token: 0x0600A154 RID: 41300 RVA: 0x003CAF5C File Offset: 0x003C915C
	public void Cleanup()
	{
		this.m_cache.Clear();
		this.m_cache = null;
		this.m_queue.Clear();
		this.m_queue = null;
		this.m_buffer.Clear();
		this.m_buffer = null;
		this.m_fileStream.Close();
	}

	// Token: 0x0600A155 RID: 41301 RVA: 0x003CAFAC File Offset: 0x003C91AC
	public bool Store(int blockID, DynamicProperties props)
	{
		long position = this.m_fileStream.Position;
		this.m_buffer.Clear();
		IBufferWriter<byte> buffer = this.m_buffer;
		MemoryPackSerializer.Serialize<DynamicProperties>(in buffer, in props, null);
		int writtenCount = this.m_buffer.WrittenCount;
		this.m_fileStream.Write(this.m_buffer.WrittenSpan);
		this.offsetsAndLengths[blockID] = new ValueTuple<long, int>(position, writtenCount);
		return true;
	}

	// Token: 0x0600A156 RID: 41302 RVA: 0x003CB018 File Offset: 0x003C9218
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicProperties Retrieve(long offset, int length)
	{
		this.m_buffer.Clear();
		Span<byte> span = this.m_buffer.GetSpan(length).Slice(0, length);
		this.m_fileStream.Seek(offset, SeekOrigin.Begin);
		int num;
		for (int i = 0; i < length; i += num)
		{
			num = this.m_fileStream.Read(span.Slice(i, length - i));
			if (num <= 0)
			{
				throw new IOException(string.Format("Expected to read {0} bytes total but only read {1} bytes.", length, i));
			}
		}
		return MemoryPackSerializer.Deserialize<DynamicProperties>(span, null);
	}

	// Token: 0x0600A157 RID: 41303 RVA: 0x003CB0A4 File Offset: 0x003C92A4
	public DynamicProperties Cache(int blockID)
	{
		LinkedListNode<int> linkedListNode = null;
		object cacheLock = this._cacheLock;
		DynamicProperties dynamicProperties;
		lock (cacheLock)
		{
			if (!this.m_cache.TryGetValue(blockID, out dynamicProperties))
			{
				this.m_cacheMisses++;
				dynamicProperties = this.Retrieve(this.offsetsAndLengths[blockID].Item1, this.offsetsAndLengths[blockID].Item2);
				this.m_cache.Add(blockID, dynamicProperties);
				while (this.m_queue.Count >= 1000)
				{
					linkedListNode = this.m_queue.Last;
					this.m_queue.Remove(linkedListNode);
					this.m_cache.Remove(linkedListNode.Value);
				}
				if (linkedListNode == null)
				{
					linkedListNode = new LinkedListNode<int>(blockID);
				}
				else
				{
					linkedListNode.Value = blockID;
				}
				this.m_queue.AddFirst(linkedListNode);
			}
			else
			{
				this.m_cacheHits++;
				linkedListNode = this.m_queue.Find(blockID);
				this.m_queue.Remove(linkedListNode);
				this.m_queue.AddFirst(linkedListNode);
			}
		}
		return dynamicProperties;
	}

	// Token: 0x0600A158 RID: 41304 RVA: 0x003CB1CC File Offset: 0x003C93CC
	public void Stats()
	{
		Log.Out("[BLOCKPROPERTIES] Block DynamicProperties Cache Stats:");
		Log.Out(string.Format("[BLOCKPROPERTIES] Cache Size: {0}", this.m_cache.Count));
		Log.Out(string.Format("[BLOCKPROPERTIES] Hits: {0}, Misses: {1}, Rate: {2}%", this.m_cacheHits, this.m_cacheMisses, (float)this.m_cacheHits / (float)(this.m_cacheHits + this.m_cacheMisses) * 100f));
	}

	// Token: 0x040079C1 RID: 31169
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_filePath;

	// Token: 0x040079C2 RID: 31170
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream m_fileStream;

	// Token: 0x040079C3 RID: 31171
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayBufferWriter<byte> m_buffer;

	// Token: 0x040079C4 RID: 31172
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_STREAM_BUFFER_SIZE = 4096;

	// Token: 0x040079C5 RID: 31173
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<long, int>[] offsetsAndLengths;

	// Token: 0x040079C6 RID: 31174
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<int> m_queue;

	// Token: 0x040079C7 RID: 31175
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, DynamicProperties> m_cache;

	// Token: 0x040079C8 RID: 31176
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_cacheHits;

	// Token: 0x040079C9 RID: 31177
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_cacheMisses;

	// Token: 0x040079CA RID: 31178
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cacheSize = 1000;

	// Token: 0x040079CB RID: 31179
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _cacheLock = new object();
}
