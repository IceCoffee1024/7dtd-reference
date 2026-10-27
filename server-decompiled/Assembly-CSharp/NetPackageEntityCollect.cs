using System;
using UnityEngine.Scripting;

// Token: 0x020007E9 RID: 2025
[Preserve]
public class NetPackageEntityCollect : NetPackage
{
	// Token: 0x06003BC0 RID: 15296 RVA: 0x00180857 File Offset: 0x0017EA57
	public NetPackageEntityCollect Setup(int _entityId, int _playerId)
	{
		this.entityId = _entityId;
		this.playerId = _playerId;
		return this;
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x00180868 File Offset: 0x0017EA68
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.playerId = _br.ReadInt32();
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x00180882 File Offset: 0x0017EA82
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write(this.playerId);
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x001808A4 File Offset: 0x0017EAA4
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

	// Token: 0x0400309E RID: 12446
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400309F RID: 12447
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;
}
