using System;
using UnityEngine.Scripting;

// Token: 0x0200080C RID: 2060
[Preserve]
public class NetPackageLockTarget : NetPackage, IMemoryPoolableObject
{
	// Token: 0x06003C8D RID: 15501 RVA: 0x00183284 File Offset: 0x00181484
	public NetPackageLockTarget Setup(PooledExpandableMemoryStream _payload)
	{
		this.payload = MemoryPools.poolMemoryStream.AllocSync(true);
		_payload.WriteTo(this.payload);
		return this;
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x001832A4 File Offset: 0x001814A4
	public override void read(PooledBinaryReader _br)
	{
		this.payload = MemoryPools.poolMemoryStream.AllocSync(true);
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.payload, num, null, true);
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x001832DD File Offset: 0x001814DD
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.payload.Length);
		this.payload.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x0018330C File Offset: 0x0018150C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.payload;
			lock (pooledExpandableMemoryStream)
			{
				pooledBinaryReader.SetBaseStream(this.payload);
				this.payload.Position = 0L;
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					LockManager.Instance.ProcessRequest(pooledBinaryReader, base.Sender.entityId);
				}
				else
				{
					LockManager.Instance.ProcessResponse(pooledBinaryReader);
				}
			}
		}
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x001833B0 File Offset: 0x001815B0
	public void Reset()
	{
		if (this.payload != null)
		{
			MemoryPools.poolMemoryStream.FreeSync(this.payload);
			this.payload = null;
		}
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x001833D1 File Offset: 0x001815D1
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x04003134 RID: 12596
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream payload;
}
