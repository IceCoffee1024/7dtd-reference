using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000835 RID: 2101
[Preserve]
public class NetPackagePlayerLaserSight : NetPackage
{
	// Token: 0x06003D48 RID: 15688 RVA: 0x00186B37 File Offset: 0x00184D37
	public NetPackagePlayerLaserSight Setup(int _entityId, bool _laserSightActive, Vector3 _laserSightPosition)
	{
		this.entityId = _entityId;
		this.laserSightActive = _laserSightActive;
		this.laserSightPosition = _laserSightPosition;
		return this;
	}

	// Token: 0x06003D49 RID: 15689 RVA: 0x00186B4F File Offset: 0x00184D4F
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.laserSightActive = _reader.ReadBoolean();
		if (this.laserSightActive)
		{
			this.laserSightPosition = StreamUtils.ReadVector3(_reader);
		}
	}

	// Token: 0x06003D4A RID: 15690 RVA: 0x00186B7D File Offset: 0x00184D7D
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

	// Token: 0x06003D4B RID: 15691 RVA: 0x00186BB4 File Offset: 0x00184DB4
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

	// Token: 0x040031CE RID: 12750
	public int entityId;

	// Token: 0x040031CF RID: 12751
	[PublicizedFrom(EAccessModifier.Private)]
	public bool laserSightActive;

	// Token: 0x040031D0 RID: 12752
	public Vector3 laserSightPosition;
}
