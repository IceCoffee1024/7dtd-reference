using System;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007BB RID: 1979
[Preserve]
public class NetPackageChunk : NetPackage
{
	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06003ADA RID: 15066 RVA: 0x00020119 File Offset: 0x0001E319
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06003ADB RID: 15067 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06003ADC RID: 15068 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x0017CC44 File Offset: 0x0017AE44
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

	// Token: 0x06003ADE RID: 15070 RVA: 0x0017CCA8 File Offset: 0x0017AEA8
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

	// Token: 0x06003ADF RID: 15071 RVA: 0x0017CD18 File Offset: 0x0017AF18
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

	// Token: 0x06003AE0 RID: 15072 RVA: 0x0017CDE4 File Offset: 0x0017AFE4
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

	// Token: 0x06003AE1 RID: 15073 RVA: 0x0017CE94 File Offset: 0x0017B094
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

	// Token: 0x04002FF0 RID: 12272
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04002FF1 RID: 12273
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream serializedData;

	// Token: 0x04002FF2 RID: 12274
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOverwriteExisting;

	// Token: 0x04002FF3 RID: 12275
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLen;

	// Token: 0x04002FF4 RID: 12276
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04002FF5 RID: 12277
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x04002FF6 RID: 12278
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkY;

	// Token: 0x04002FF7 RID: 12279
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;
}
