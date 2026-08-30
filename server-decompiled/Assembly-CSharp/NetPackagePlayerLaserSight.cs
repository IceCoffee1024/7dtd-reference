using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200081F RID: 2079
[Preserve]
public class NetPackagePlayerLaserSight : NetPackage
{
	// Token: 0x06003D04 RID: 15620 RVA: 0x00183B17 File Offset: 0x00181D17
	public NetPackagePlayerLaserSight Setup(int _entityId, bool _laserSightActive, Vector3 _laserSightPosition)
	{
		this.entityId = _entityId;
		this.laserSightActive = _laserSightActive;
		this.laserSightPosition = _laserSightPosition;
		return this;
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x00183B2F File Offset: 0x00181D2F
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.laserSightActive = _reader.ReadBoolean();
		if (this.laserSightActive)
		{
			this.laserSightPosition = StreamUtils.ReadVector3(_reader);
		}
	}

	// Token: 0x06003D06 RID: 15622 RVA: 0x00183B5D File Offset: 0x00181D5D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.laserSightActive);
		if (this.laserSightActive)
		{
			StreamUtils.Write(_writer, this.laserSightPosition);
		}
	}

	// Token: 0x06003D07 RID: 15623 RVA: 0x00183B94 File Offset: 0x00181D94
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerLaserSight>().Setup(this.entityId, this.laserSightActive, this.laserSightPosition), false, -1, base.Sender.entityId, -1, null, 192, false);
		}
		EntityAlive entityAlive = (EntityAlive)_world.GetEntity(this.entityId);
		if (entityAlive != null && entityAlive.inventory.holdingItem != null)
		{
			ItemActionRanged.ItemActionDataRanged itemActionDataRanged = entityAlive.inventory.holdingItemData.actionData[0] as ItemActionRanged.ItemActionDataRanged;
			if (itemActionDataRanged != null && itemActionDataRanged.Laser != null)
			{
				itemActionDataRanged.Laser.gameObject.SetActive(this.laserSightActive);
				itemActionDataRanged.Laser.position = this.laserSightPosition - Origin.position;
			}
		}
	}

	// Token: 0x06003D08 RID: 15624 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003148 RID: 12616
	public int entityId;

	// Token: 0x04003149 RID: 12617
	[PublicizedFrom(EAccessModifier.Private)]
	public bool laserSightActive;

	// Token: 0x0400314A RID: 12618
	public Vector3 laserSightPosition;
}
