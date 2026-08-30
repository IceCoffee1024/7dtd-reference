using System;
using UnityEngine.Scripting;

// Token: 0x020007BA RID: 1978
[Preserve]
public class NetPackageDecoResetWorldChunk : NetPackage
{
	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x06003AA3 RID: 15011 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x0017B6F8 File Offset: 0x001798F8
	public NetPackageDecoResetWorldChunk Setup(long _chunkKey)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			pooledBinaryWriter.Write(_chunkKey);
		}
		return this;
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x0017B744 File Offset: 0x00179944
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldChunk()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003AA6 RID: 15014 RVA: 0x0017B77C File Offset: 0x0017997C
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x0017B7A4 File Offset: 0x001799A4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x0017B7D0 File Offset: 0x001799D0
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

	// Token: 0x06003AA9 RID: 15017 RVA: 0x0017B858 File Offset: 0x00179A58
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x04002FBC RID: 12220
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
