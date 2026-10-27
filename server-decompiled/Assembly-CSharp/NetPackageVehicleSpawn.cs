using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000871 RID: 2161
[Preserve]
public class NetPackageVehicleSpawn : NetPackage
{
	// Token: 0x06003E7D RID: 15997 RVA: 0x0018C739 File Offset: 0x0018A939
	public NetPackageVehicleSpawn Setup(int _entityType, Vector3 _pos, Vector3 _rot, ItemValue _itemValue, int _entityThatPlaced = -1)
	{
		this.entityType = _entityType;
		this.pos = _pos;
		this.rot = _rot;
		this.itemValue = _itemValue;
		this.entityThatPlaced = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0018C764 File Offset: 0x0018A964
	public override void read(PooledBinaryReader _reader)
	{
		this.entityType = _reader.ReadInt32();
		this.pos = StreamUtils.ReadVector3(_reader);
		this.rot = StreamUtils.ReadVector3(_reader);
		this.itemValue = new ItemValue();
		this.itemValue.Read(_reader);
		this.entityThatPlaced = _reader.ReadInt32();
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0018C7B8 File Offset: 0x0018A9B8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityType);
		StreamUtils.Write(_writer, this.pos);
		StreamUtils.Write(_writer, this.rot);
		this.itemValue.Write(_writer);
		_writer.Write(this.entityThatPlaced);
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0018C808 File Offset: 0x0018AA08
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

	// Token: 0x040032C3 RID: 12995
	public int entityType;

	// Token: 0x040032C4 RID: 12996
	public Vector3 pos;

	// Token: 0x040032C5 RID: 12997
	public Vector3 rot;

	// Token: 0x040032C6 RID: 12998
	public ItemValue itemValue;

	// Token: 0x040032C7 RID: 12999
	public int entityThatPlaced;
}
