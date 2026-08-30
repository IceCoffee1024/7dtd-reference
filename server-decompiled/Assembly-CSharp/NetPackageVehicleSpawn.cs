using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000858 RID: 2136
[Preserve]
public class NetPackageVehicleSpawn : NetPackage
{
	// Token: 0x06003E59 RID: 15961 RVA: 0x00189735 File Offset: 0x00187935
	public NetPackageVehicleSpawn Setup(int _entityType, Vector3 _pos, Vector3 _rot, ItemValue _itemValue, int _entityThatPlaced = -1)
	{
		this.entityType = _entityType;
		this.pos = _pos;
		this.rot = _rot;
		this.itemValue = _itemValue;
		this.entityThatPlaced = _entityThatPlaced;
		return this;
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x00189760 File Offset: 0x00187960
	public override void read(PooledBinaryReader _reader)
	{
		this.entityType = _reader.ReadInt32();
		this.pos = StreamUtils.ReadVector3(_reader);
		this.rot = StreamUtils.ReadVector3(_reader);
		this.itemValue = new ItemValue();
		this.itemValue.Read(_reader);
		this.entityThatPlaced = _reader.ReadInt32();
	}

	// Token: 0x06003E5B RID: 15963 RVA: 0x001897B4 File Offset: 0x001879B4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityType);
		StreamUtils.Write(_writer, this.pos);
		StreamUtils.Write(_writer, this.rot);
		this.itemValue.Write(_writer);
		_writer.Write(this.entityThatPlaced);
	}

	// Token: 0x06003E5C RID: 15964 RVA: 0x00189804 File Offset: 0x00187A04
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

	// Token: 0x06003E5D RID: 15965 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04003237 RID: 12855
	public int entityType;

	// Token: 0x04003238 RID: 12856
	public Vector3 pos;

	// Token: 0x04003239 RID: 12857
	public Vector3 rot;

	// Token: 0x0400323A RID: 12858
	public ItemValue itemValue;

	// Token: 0x0400323B RID: 12859
	public int entityThatPlaced;
}
