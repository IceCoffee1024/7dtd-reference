using System;
using UnityEngine.Scripting;

// Token: 0x020007B1 RID: 1969
[Preserve]
public class NetPackageBag : NetPackage
{
	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06003AB0 RID: 15024 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x0017C3C8 File Offset: 0x0017A5C8
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

	// Token: 0x06003AB2 RID: 15026 RVA: 0x0017C41C File Offset: 0x0017A61C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageBag()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x0017C454 File Offset: 0x0017A654
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x0017C488 File Offset: 0x0017A688
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write((ushort)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x0017C4C0 File Offset: 0x0017A6C0
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

	// Token: 0x04002FCC RID: 12236
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04002FCD RID: 12237
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
