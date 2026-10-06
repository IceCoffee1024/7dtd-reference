using System;
using UnityEngine.Scripting;

// Token: 0x020007DF RID: 2015
[Preserve]
public class NetPackageEntityCollect : NetPackage
{
	// Token: 0x06003B9D RID: 15261 RVA: 0x0017F753 File Offset: 0x0017D953
	public NetPackageEntityCollect Setup(int _entityId, int _playerId)
	{
		this.entityId = _entityId;
		this.playerId = _playerId;
		return this;
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x0017F764 File Offset: 0x0017D964
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.playerId = _br.ReadInt32();
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0017F77E File Offset: 0x0017D97E
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write(this.playerId);
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x0017F7A0 File Offset: 0x0017D9A0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.playerId, false))
		{
			return;
		}
		Entity entity = _world.GetEntity(this.entityId);
		if (entity == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityCollect>().Setup(this.entityId, this.playerId), false, this.playerId, -1, -1, null, 192, false);
			entity.OnCollectServer(this.playerId);
			return;
		}
		entity.OnCollectLocal(this.playerId);
	}

	// Token: 0x0400307E RID: 12414
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400307F RID: 12415
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;
}
