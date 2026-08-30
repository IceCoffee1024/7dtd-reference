using System;
using UnityEngine.Scripting;

// Token: 0x020007A4 RID: 1956
[Preserve]
public class NetPackageBag : NetPackage
{
	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x06003A1F RID: 14879 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x00179AAC File Offset: 0x00177CAC
	public NetPackageBag Setup(int _entityId, Bag _bag)
	{
		this.entityId = _entityId;
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_bag.Write(pooledBinaryWriter);
		}
		return this;
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x00179AFC File Offset: 0x00177CFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageBag()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x00179B34 File Offset: 0x00177D34
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x00179B68 File Offset: 0x00177D68
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write((ushort)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003A24 RID: 14884 RVA: 0x00179BA0 File Offset: 0x00177DA0
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
				bag.ReadInto(pooledBinaryReader);
			}
		}
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04002F59 RID: 12121
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04002F5A RID: 12122
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
