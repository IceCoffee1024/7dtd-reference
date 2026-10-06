using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200082B RID: 2091
[Preserve]
public class NetPackagePlayerLaserSight : NetPackage
{
	// Token: 0x06003D25 RID: 15653 RVA: 0x00185A33 File Offset: 0x00183C33
	public NetPackagePlayerLaserSight Setup(int _entityId, bool _laserSightActive, Vector3 _laserSightPosition)
	{
		this.entityId = _entityId;
		this.laserSightActive = _laserSightActive;
		this.laserSightPosition = _laserSightPosition;
		return this;
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x00185A4B File Offset: 0x00183C4B
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.laserSightActive = _reader.ReadBoolean();
		if (this.laserSightActive)
		{
			this.laserSightPosition = StreamUtils.ReadVector3(_reader);
		}
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x00185A79 File Offset: 0x00183C79
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

	// Token: 0x06003D28 RID: 15656 RVA: 0x00185AB0 File Offset: 0x00183CB0
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

	// Token: 0x040031AE RID: 12718
	public int entityId;

	// Token: 0x040031AF RID: 12719
	[PublicizedFrom(EAccessModifier.Private)]
	public bool laserSightActive;

	// Token: 0x040031B0 RID: 12720
	public Vector3 laserSightPosition;
}
