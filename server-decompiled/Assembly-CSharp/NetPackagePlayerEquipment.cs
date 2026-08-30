using System;
using UnityEngine.Scripting;

// Token: 0x02000826 RID: 2086
[Preserve]
public class NetPackagePlayerEquipment : NetPackageEntityTargeted
{
	// Token: 0x06003D46 RID: 15686 RVA: 0x00185898 File Offset: 0x00183A98
	public NetPackagePlayerEquipment Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.equipment = _entity.equipment;
		return this;
	}

	// Token: 0x06003D47 RID: 15687 RVA: 0x001858B3 File Offset: 0x00183AB3
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.equipment = Equipment.Read(_reader);
	}

	// Token: 0x06003D48 RID: 15688 RVA: 0x001858C8 File Offset: 0x00183AC8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.equipment.Write(_writer);
	}

	// Token: 0x06003D49 RID: 15689 RVA: 0x001858E0 File Offset: 0x00183AE0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (!entityAlive)
		{
			Log.Out("Discarding " + base.GetType().Name + " for entity Id=" + this.entityId.ToString());
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, true))
		{
			return;
		}
		entityAlive.equipment.Apply(this.equipment, false);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerEquipment>().Setup(entityAlive), false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x06003D4A RID: 15690 RVA: 0x00177A99 File Offset: 0x00175C99
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x040031BC RID: 12732
	public Equipment equipment;
}
