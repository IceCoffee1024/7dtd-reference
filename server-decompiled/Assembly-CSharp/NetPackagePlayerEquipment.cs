using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x0200082E RID: 2094
[Preserve]
public class NetPackagePlayerEquipment : NetPackageEntityTargeted
{
	// Token: 0x06003D27 RID: 15655 RVA: 0x00186564 File Offset: 0x00184764
	public NetPackagePlayerEquipment Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.equipmentData = StreamUtils.ToBlob(delegate(PooledBinaryWriter pbw)
		{
			_entity.equipment.Write(pbw, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? StreamModeWrite.ToClient : StreamModeWrite.ToServer);
		});
		return this;
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x001865A7 File Offset: 0x001847A7
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.equipmentData = StreamUtils.ReadBlob(_reader);
	}

	// Token: 0x06003D29 RID: 15657 RVA: 0x001865BC File Offset: 0x001847BC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		StreamUtils.WriteBlob(_writer, this.equipmentData);
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x001865D4 File Offset: 0x001847D4
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

	// Token: 0x040031C0 RID: 12736
	public MemoryStream equipmentData;
}
