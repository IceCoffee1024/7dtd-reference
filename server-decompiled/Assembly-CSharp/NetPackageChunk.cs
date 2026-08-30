using System;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007B8 RID: 1976
[Preserve]
public class NetPackageChunk : NetPackage
{
	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x00020021 File Offset: 0x0001E221
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x06003AAA RID: 15018 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x0017C5F0 File Offset: 0x0017A7F0
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

	// Token: 0x06003AAC RID: 15020 RVA: 0x0017C654 File Offset: 0x0017A854
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

	// Token: 0x06003AAD RID: 15021 RVA: 0x0017C6C4 File Offset: 0x0017A8C4
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

	// Token: 0x06003AAE RID: 15022 RVA: 0x0017C790 File Offset: 0x0017A990
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

	// Token: 0x06003AAF RID: 15023 RVA: 0x0017C840 File Offset: 0x0017AA40
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

	// Token: 0x06003AB0 RID: 15024 RVA: 0x0017C9B8 File Offset: 0x0017ABB8
	public override int GetLength()
	{
		return 14 + (int)this.serializedData.Length;
	}

	// Token: 0x04002FF6 RID: 12278
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04002FF7 RID: 12279
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream serializedData;

	// Token: 0x04002FF8 RID: 12280
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOverwriteExisting;

	// Token: 0x04002FF9 RID: 12281
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLen;

	// Token: 0x04002FFA RID: 12282
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04002FFB RID: 12283
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x04002FFC RID: 12284
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkY;

	// Token: 0x04002FFD RID: 12285
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;
}
