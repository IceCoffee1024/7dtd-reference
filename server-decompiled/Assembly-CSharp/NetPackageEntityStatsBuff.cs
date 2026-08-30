using System;
using UnityEngine.Scripting;

// Token: 0x020001D2 RID: 466
[Preserve]
public class NetPackageEntityStatsBuff : NetPackage
{
	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x0005F538 File Offset: 0x0005D738
	public NetPackageEntityStatsBuff Setup(EntityAlive entity, byte[] _data = null)
	{
		this.m_entityId = entity.entityId;
		if (_data == null)
		{
			using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(true))
				{
					pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
					entity.Buffs.Write(pooledBinaryWriter, true);
					this.data = pooledExpandableMemoryStream.ToArray();
					return this;
				}
			}
		}
		this.data = _data;
		return this;
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0005F5C8 File Offset: 0x0005D7C8
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.data = _reader.ReadBytes(_reader.ReadInt32());
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0005F5E8 File Offset: 0x0005D7E8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		_writer.Write(this.data.Length);
		_writer.Write(this.data);
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0005F618 File Offset: 0x0005D818
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.m_entityId) as EntityAlive;
		if (entityAlive != null)
		{
			if (entityAlive.isEntityRemote)
			{
				using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
				{
					pooledExpandableMemoryStream.Write(this.data, 0, this.data.Length);
					pooledExpandableMemoryStream.Position = 0L;
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
					{
						pooledBinaryReader.SetBaseStream(pooledExpandableMemoryStream);
						entityAlive.Buffs.Read(pooledBinaryReader);
					}
				}
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityStatsBuff>().Setup(entityAlive, this.data), false, -1, entityAlive.entityId, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0005F708 File Offset: 0x0005D908
	public override int GetLength()
	{
		return this.data.Length + 4;
	}

	// Token: 0x04000C56 RID: 3158
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x04000C57 RID: 3159
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;
}
