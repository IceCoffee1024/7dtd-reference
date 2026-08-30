using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000864 RID: 2148
[Preserve]
public class NetPackageVehicleSpawn : NetPackage
{
	// Token: 0x06003EBE RID: 16062 RVA: 0x0018B805 File Offset: 0x00189A05
	public NetPackageVehicleSpawn Setup(int _entityType, Vector3 _pos, Vector3 _rot, ItemValue _itemValue, int _entityThatPlaced = -1)
	{
		this.entityType = _entityType;
		this.pos = _pos;
		this.rot = _rot;
		this.itemValue = _itemValue;
		this.entityThatPlaced = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x0018B830 File Offset: 0x00189A30
	public override void read(PooledBinaryReader _reader)
	{
		this.entityType = _reader.ReadInt32();
		this.pos = StreamUtils.ReadVector3(_reader);
		this.rot = StreamUtils.ReadVector3(_reader);
		this.itemValue = new ItemValue();
		this.itemValue.Read(_reader);
		this.entityThatPlaced = _reader.ReadInt32();
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x0018B884 File Offset: 0x00189A84
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityType);
		StreamUtils.Write(_writer, this.pos);
		StreamUtils.Write(_writer, this.rot);
		this.itemValue.Write(_writer);
		_writer.Write(this.entityThatPlaced);
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x0018B8D4 File Offset: 0x00189AD4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityThatPlaced, false))
		{
			return;
		}
		if (VehicleManager.CanAddMoreVehicles())
		{
			EntityVehicle entityVehicle = (EntityVehicle)EntityFactory.CreateEntity(this.entityType, this.pos, this.rot);
			entityVehicle.SetSpawnerSource(EnumSpawnerSource.StaticSpawner);
			entityVehicle.GetVehicle().SetItemValue(this.itemValue.Clone());
			if (GameManager.Instance.World.GetEntity(this.entityThatPlaced) as EntityPlayer != null)
			{
				entityVehicle.Spawned = true;
				ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(this.entityThatPlaced);
				entityVehicle.SetOwner(clientInfo.InternalId);
			}
			_world.SpawnEntityInWorld(entityVehicle);
			entityVehicle.bPlayerStatsChanged = true;
		}
		else
		{
			GameManager.Instance.ItemDropServer(new ItemStack(this.itemValue, 1), this.pos, Vector3.zero, this.entityThatPlaced, 60f, false);
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageVehicleCount>().Setup(), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040032B8 RID: 12984
	public int entityType;

	// Token: 0x040032B9 RID: 12985
	public Vector3 pos;

	// Token: 0x040032BA RID: 12986
	public Vector3 rot;

	// Token: 0x040032BB RID: 12987
	public ItemValue itemValue;

	// Token: 0x040032BC RID: 12988
	public int entityThatPlaced;
}
