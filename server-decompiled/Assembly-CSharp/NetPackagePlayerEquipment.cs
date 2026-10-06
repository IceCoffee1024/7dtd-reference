using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000824 RID: 2084
[Preserve]
public class NetPackagePlayerEquipment : NetPackageEntityTargeted
{
	// Token: 0x06003D04 RID: 15620 RVA: 0x00185460 File Offset: 0x00183660
	public NetPackagePlayerEquipment Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.equipmentData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_entity.equipment.Write(pbw, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? StreamModeWrite.ToClient : StreamModeWrite.ToServer);
		});
		return this;
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x001854A3 File Offset: 0x001836A3
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.equipmentData = StreamUtils.ReadBlob(_reader);
	}

	// Token: 0x06003D06 RID: 15622 RVA: 0x001854B8 File Offset: 0x001836B8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		StreamUtils.WriteBlob(_writer, this.equipmentData);
	}

	// Token: 0x06003D07 RID: 15623 RVA: 0x001854D0 File Offset: 0x001836D0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive ea = _world.GetEntity(this.entityId) as EntityAlive;
		if (!ea)
		{
			Log.Out("Discarding " + base.GetType().Name + " for entity Id=" + this.entityId.ToString());
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, true))
		{
			return;
		}
		StreamUtils.FromBlob(this.equipmentData, delegate(PooledBinaryReader pbr)
		{
			ea.equipment.ReadInto(pbr, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? StreamModeRead.FromClient : StreamModeRead.FromServer);
		});
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerEquipment>().Setup(ea), false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x040031A0 RID: 12704
	public MemoryStream equipmentData;
}
