using System;
using UnityEngine.Scripting;

// Token: 0x020007D0 RID: 2000
[Preserve]
public class NetPackageDecoResetWorldChunk : NetPackage
{
	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06003B3F RID: 15167 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x0017F06C File Offset: 0x0017D26C
	public NetPackageDecoResetWorldChunk Setup(long _chunkKey)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			pooledBinaryWriter.Write(_chunkKey);
		}
		return this;
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x0017F0B8 File Offset: 0x0017D2B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldChunk()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x0017F0F0 File Offset: 0x0017D2F0
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B43 RID: 15171 RVA: 0x0017F118 File Offset: 0x0017D318
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B44 RID: 15172 RVA: 0x0017F144 File Offset: 0x0017D344
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

	// Token: 0x04003051 RID: 12369
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
