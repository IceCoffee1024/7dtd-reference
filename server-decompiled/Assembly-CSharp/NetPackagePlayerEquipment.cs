using System;
using UnityEngine.Scripting;

// Token: 0x0200081B RID: 2075
[Preserve]
public class NetPackagePlayerEquipment : NetPackageEntityTargeted
{
	// Token: 0x06003CE7 RID: 15591 RVA: 0x001834C0 File Offset: 0x001816C0
	public NetPackagePlayerEquipment Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.equipment = _entity.equipment;
		return this;
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x001834DB File Offset: 0x001816DB
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.equipment = Equipment.Read(_reader);
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x001834F0 File Offset: 0x001816F0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.equipment.Write(_writer);
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x00183508 File Offset: 0x00181708
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

	// Token: 0x06003CEB RID: 15595 RVA: 0x00175819 File Offset: 0x00173A19
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x0400313D RID: 12605
	public Equipment equipment;
}
