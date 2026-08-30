using System;
using UnityEngine.Scripting;

// Token: 0x020007C5 RID: 1989
[Preserve]
public class NetPackageDecoResetWorldChunk : NetPackage
{
	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06003B02 RID: 15106 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B03 RID: 15107 RVA: 0x0017DAD0 File Offset: 0x0017BCD0
	public NetPackageDecoResetWorldChunk Setup(long _chunkKey)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			pooledBinaryWriter.Write(_chunkKey);
		}
		return this;
	}

	// Token: 0x06003B04 RID: 15108 RVA: 0x0017DB1C File Offset: 0x0017BD1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldChunk()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B05 RID: 15109 RVA: 0x0017DB54 File Offset: 0x0017BD54
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B06 RID: 15110 RVA: 0x0017DB7C File Offset: 0x0017BD7C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B07 RID: 15111 RVA: 0x0017DBA8 File Offset: 0x0017BDA8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.ms;
			lock (pooledExpandableMemoryStream)
			{
				pooledBinaryReader.SetBaseStream(this.ms);
				this.ms.Position = 0L;
				long num = pooledBinaryReader.ReadInt64();
				DecoManager.Instance.ResetDecosForWorldChunk(num);
			}
		}
	}

	// Token: 0x06003B08 RID: 15112 RVA: 0x0017DC30 File Offset: 0x0017BE30
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x0400303B RID: 12347
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
