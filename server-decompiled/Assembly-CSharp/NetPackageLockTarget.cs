using System;
using UnityEngine.Scripting;

// Token: 0x02000816 RID: 2070
[Preserve]
public class NetPackageLockTarget : NetPackage, IMemoryPoolableObject
{
	// Token: 0x06003CB0 RID: 15536 RVA: 0x00184388 File Offset: 0x00182588
	public NetPackageLockTarget Setup(PooledExpandableMemoryStream _payload)
	{
		this.payload = MemoryPools.poolMemoryStream.AllocSync(true);
		_payload.WriteTo(this.payload);
		return this;
	}

	// Token: 0x06003CB1 RID: 15537 RVA: 0x001843A8 File Offset: 0x001825A8
	public override void read(PooledBinaryReader _br)
	{
		this.payload = MemoryPools.poolMemoryStream.AllocSync(true);
		int num = _br.ReadInt32();
		StreamUtils.StreamCopy(_br.BaseStream, this.payload, num, null, true);
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x001843E1 File Offset: 0x001825E1
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((int)this.payload.Length);
		this.payload.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x00184410 File Offset: 0x00182610
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

	// Token: 0x06003CB4 RID: 15540 RVA: 0x001844B4 File Offset: 0x001826B4
	public void Reset()
	{
		if (this.payload != null)
		{
			MemoryPools.poolMemoryStream.FreeSync(this.payload);
			this.payload = null;
		}
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x001844D5 File Offset: 0x001826D5
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x04003154 RID: 12628
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream payload;
}
