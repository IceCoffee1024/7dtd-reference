using System;
using UnityEngine.Scripting;

// Token: 0x020007B9 RID: 1977
[Preserve]
public class NetPackageBag : NetPackage
{
	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003AC8 RID: 15048 RVA: 0x0017D360 File Offset: 0x0017B560
	public NetPackageBag Setup(int _entityId, Bag _bag)
	{
		this.entityId = _entityId;
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_bag.Write(pooledBinaryWriter, StreamModeWrite.ToServer);
		}
		return this;
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x0017D3B4 File Offset: 0x0017B5B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageBag()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x0017D3EC File Offset: 0x0017B5EC
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x0017D420 File Offset: 0x0017B620
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write((ushort)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x0017D458 File Offset: 0x0017B658
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Entity entity = _world.GetEntity(this.entityId);
		Bag bag = ((entity != null) ? entity.bag : null);
		if (bag == null)
		{
			Log.Warning("[NetPackageBag] Entity " + this.entityId.ToString() + " with bag not found");
			return;
		}
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.ms;
			lock (pooledExpandableMemoryStream)
			{
				pooledBinaryReader.SetBaseStream(this.ms);
				this.ms.Position = 0L;
				bag.ReadInto(pooledBinaryReader, StreamModeRead.FromClient);
			}
		}
	}

	// Token: 0x04002FE8 RID: 12264
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04002FE9 RID: 12265
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
