using System;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007AE RID: 1966
[Preserve]
public class NetPackageChunk : NetPackage
{
	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x06003A50 RID: 14928 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06003A51 RID: 14929 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06003A52 RID: 14930 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x0017A370 File Offset: 0x00178570
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageChunk()
	{
		if (this.chunk != null)
		{
			this.chunk.InProgressNetworking = false;
			this.chunk = null;
		}
		if (this.serializedData != null)
		{
			MemoryPools.poolMS.FreeSync(this.serializedData);
			this.serializedData = null;
		}
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x0017A3D4 File Offset: 0x001785D4
	public NetPackageChunk Setup(Chunk _chunk, bool _bOverwriteExisting = false)
	{
		this.chunk = _chunk;
		this.bOverwriteExisting = _bOverwriteExisting;
		this.serializedData = MemoryPools.poolMS.AllocSync(true);
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.serializedData);
			this.chunk.write(pooledBinaryWriter);
		}
		return this;
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x0017A444 File Offset: 0x00178644
	public override void read(PooledBinaryReader _reader)
	{
		this.bOverwriteExisting = _reader.ReadBoolean();
		if (this.bOverwriteExisting)
		{
			this.chunkX = (int)_reader.ReadInt16();
			this.chunkY = (int)_reader.ReadInt16();
			this.chunkZ = (int)_reader.ReadInt16();
		}
		this.dataLen = _reader.ReadInt32();
		this.data = _reader.ReadBytes(this.dataLen);
		if (!this.bOverwriteExisting)
		{
			if (this.chunk == null)
			{
				this.chunk = MemoryPools.PoolChunks.AllocSync(true);
			}
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(new MemoryStream(this.data));
				this.chunk.read(pooledBinaryReader, uint.MaxValue);
			}
		}
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x0017A510 File Offset: 0x00178710
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.bOverwriteExisting);
		if (this.bOverwriteExisting)
		{
			_writer.Write((short)this.chunk.X);
			_writer.Write((short)this.chunk.Y);
			_writer.Write((short)this.chunk.Z);
		}
		_writer.Write((int)this.serializedData.Length);
		this.serializedData.Position = 0L;
		StreamUtils.StreamCopy(this.serializedData, _writer.BaseStream, null, true);
		MemoryPools.poolMS.FreeSync(this.serializedData);
		this.serializedData = null;
		this.chunk = null;
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x0017A5C0 File Offset: 0x001787C0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			Log.Warning("Received chunk while world is not set up");
			if (this.chunk != null)
			{
				MemoryPools.PoolChunks.FreeSync(this.chunk);
				this.chunk = null;
			}
			return;
		}
		long num = ((!this.bOverwriteExisting) ? this.chunk.Key : WorldChunkCache.MakeChunkKey(this.chunkX, this.chunkZ));
		Chunk chunkSync;
		if ((chunkSync = _world.ChunkCache.GetChunkSync(num)) != null && !this.bOverwriteExisting)
		{
			string name = base.GetType().Name;
			string text = ": chunk already loaded ";
			Chunk chunk = this.chunk;
			Log.Error(name + text + ((chunk != null) ? chunk.ToString() : null));
			return;
		}
		if (this.bOverwriteExisting)
		{
			Bounds bounds = Chunk.CalculateAABB(this.chunkX, this.chunkY, this.chunkZ);
			MultiBlockManager.Instance.DeregisterTrackedBlockDatas(bounds);
		}
		if (!this.bOverwriteExisting)
		{
			_world.ChunkCache.AddChunkSync(this.chunk, false);
			this.chunk.NeedsRegeneration = true;
			this.chunk = null;
			return;
		}
		if (chunkSync != null)
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(new MemoryStream(this.data));
				chunkSync.OnUnload(_world);
				_world.ChunkCache.RemoveChunkSync(chunkSync.Key);
				chunkSync.Reset();
				chunkSync.read(pooledBinaryReader, uint.MaxValue);
				_world.ChunkCache.AddChunkSync(chunkSync, false);
				this.data = null;
			}
		}
	}

	// Token: 0x06003A58 RID: 14936 RVA: 0x0017A738 File Offset: 0x00178938
	public override int GetLength()
	{
		return 14 + (int)this.serializedData.Length;
	}

	// Token: 0x04002F7D RID: 12157
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04002F7E RID: 12158
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream serializedData;

	// Token: 0x04002F7F RID: 12159
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOverwriteExisting;

	// Token: 0x04002F80 RID: 12160
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLen;

	// Token: 0x04002F81 RID: 12161
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04002F82 RID: 12162
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x04002F83 RID: 12163
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkY;

	// Token: 0x04002F84 RID: 12164
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;
}
