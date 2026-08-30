using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using MemoryPack;
using Platform;

// Token: 0x020013D5 RID: 5077
public class DynamicPropertiesCache
{
	// Token: 0x06009F53 RID: 40787 RVA: 0x003BEBC8 File Offset: 0x003BCDC8
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

	// Token: 0x06009F54 RID: 40788 RVA: 0x003BEC78 File Offset: 0x003BCE78
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

	// Token: 0x06009F55 RID: 40789 RVA: 0x003BECC8 File Offset: 0x003BCEC8
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

	// Token: 0x06009F56 RID: 40790 RVA: 0x003BED34 File Offset: 0x003BCF34
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

	// Token: 0x06009F57 RID: 40791 RVA: 0x003BEDC0 File Offset: 0x003BCFC0
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

	// Token: 0x06009F58 RID: 40792 RVA: 0x003BEEE8 File Offset: 0x003BD0E8
	public void Stats()
	{
		Log.Out("[BLOCKPROPERTIES] Block DynamicProperties Cache Stats:");
		Log.Out(string.Format("[BLOCKPROPERTIES] Cache Size: {0}", this.m_cache.Count));
		Log.Out(string.Format("[BLOCKPROPERTIES] Hits: {0}, Misses: {1}, Rate: {2}%", this.m_cacheHits, this.m_cacheMisses, (float)this.m_cacheHits / (float)(this.m_cacheHits + this.m_cacheMisses) * 100f));
	}

	// Token: 0x0400782C RID: 30764
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_filePath;

	// Token: 0x0400782D RID: 30765
	[PublicizedFrom(EAccessModifier.Private)]
	public FileStream m_fileStream;

	// Token: 0x0400782E RID: 30766
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayBufferWriter<byte> m_buffer;

	// Token: 0x0400782F RID: 30767
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_STREAM_BUFFER_SIZE = 4096;

	// Token: 0x04007830 RID: 30768
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<long, int>[] offsetsAndLengths;

	// Token: 0x04007831 RID: 30769
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<int> m_queue;

	// Token: 0x04007832 RID: 30770
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, DynamicProperties> m_cache;

	// Token: 0x04007833 RID: 30771
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_cacheHits;

	// Token: 0x04007834 RID: 30772
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_cacheMisses;

	// Token: 0x04007835 RID: 30773
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cacheSize = 1000;

	// Token: 0x04007836 RID: 30774
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object _cacheLock = new object();
}
