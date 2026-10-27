using System;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007C3 RID: 1987
[Preserve]
public class NetPackageChunk : NetPackage
{
	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x00020119 File Offset: 0x0001E319
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AF4 RID: 15092 RVA: 0x0017DBDC File Offset: 0x0017BDDC
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

	// Token: 0x06003AF5 RID: 15093 RVA: 0x0017DC40 File Offset: 0x0017BE40
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

	// Token: 0x06003AF6 RID: 15094 RVA: 0x0017DCB0 File Offset: 0x0017BEB0
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

	// Token: 0x06003AF7 RID: 15095 RVA: 0x0017DD7C File Offset: 0x0017BF7C
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

	// Token: 0x06003AF8 RID: 15096 RVA: 0x0017DE2C File Offset: 0x0017C02C
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

	// Token: 0x0400300C RID: 12300
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x0400300D RID: 12301
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledMemoryStream serializedData;

	// Token: 0x0400300E RID: 12302
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bOverwriteExisting;

	// Token: 0x0400300F RID: 12303
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLen;

	// Token: 0x04003010 RID: 12304
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04003011 RID: 12305
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkX;

	// Token: 0x04003012 RID: 12306
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkY;

	// Token: 0x04003013 RID: 12307
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkZ;
}
