using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200082A RID: 2090
[Preserve]
public class NetPackagePlayerLaserSight : NetPackage
{
	// Token: 0x06003D63 RID: 15715 RVA: 0x00185EEF File Offset: 0x001840EF
	public NetPackagePlayerLaserSight Setup(int _entityId, bool _laserSightActive, Vector3 _laserSightPosition)
	{
		this.entityId = _entityId;
		this.laserSightActive = _laserSightActive;
		this.laserSightPosition = _laserSightPosition;
		return this;
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x00185F07 File Offset: 0x00184107
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.laserSightActive = _reader.ReadBoolean();
		if (this.laserSightActive)
		{
			this.laserSightPosition = StreamUtils.ReadVector3(_reader);
		}
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x00185F35 File Offset: 0x00184135
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

	// Token: 0x06003D66 RID: 15718 RVA: 0x00185F6C File Offset: 0x0018416C
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

	// Token: 0x06003D67 RID: 15719 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x040031C7 RID: 12743
	public int entityId;

	// Token: 0x040031C8 RID: 12744
	[PublicizedFrom(EAccessModifier.Private)]
	public bool laserSightActive;

	// Token: 0x040031C9 RID: 12745
	public Vector3 laserSightPosition;
}
