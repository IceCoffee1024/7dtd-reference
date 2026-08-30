using System;
using UnityEngine.Scripting;

// Token: 0x020007AE RID: 1966
[Preserve]
public class NetPackageBag : NetPackage
{
	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x06003A77 RID: 14967 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003A78 RID: 14968 RVA: 0x0017BD2C File Offset: 0x00179F2C
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

	// Token: 0x06003A79 RID: 14969 RVA: 0x0017BD7C File Offset: 0x00179F7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageBag()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x0017BDB4 File Offset: 0x00179FB4
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x0017BDE8 File Offset: 0x00179FE8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write((ushort)this.ms.Length);
		this.ms.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x0017BE20 File Offset: 0x0017A020
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

	// Token: 0x06003A7D RID: 14973 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04002FD2 RID: 12242
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04002FD3 RID: 12243
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
