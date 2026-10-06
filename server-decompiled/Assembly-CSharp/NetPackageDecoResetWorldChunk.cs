using System;
using UnityEngine.Scripting;

// Token: 0x020007C8 RID: 1992
[Preserve]
public class NetPackageDecoResetWorldChunk : NetPackage
{
	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06003B28 RID: 15144 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x0017E0D4 File Offset: 0x0017C2D4
	public NetPackageDecoResetWorldChunk Setup(long _chunkKey)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			pooledBinaryWriter.Write(_chunkKey);
		}
		return this;
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x0017E120 File Offset: 0x0017C320
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageDecoResetWorldChunk()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x0017E158 File Offset: 0x0017C358
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x0017E180 File Offset: 0x0017C380
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003B2D RID: 15149 RVA: 0x0017E1AC File Offset: 0x0017C3AC
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

	// Token: 0x04003035 RID: 12341
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
