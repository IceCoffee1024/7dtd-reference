using System;
using UnityEngine.Scripting;

// Token: 0x020007DC RID: 2012
[Preserve]
public class NetPackageEntityCollect : NetPackage
{
	// Token: 0x06003B88 RID: 15240 RVA: 0x0017F20F File Offset: 0x0017D40F
	public NetPackageEntityCollect Setup(int _entityId, int _playerId)
	{
		this.entityId = _entityId;
		this.playerId = _playerId;
		return this;
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x0017F220 File Offset: 0x0017D420
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.playerId = _br.ReadInt32();
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x0017F23A File Offset: 0x0017D43A
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write(this.playerId);
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x0017F25C File Offset: 0x0017D45C
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

	// Token: 0x06003B8C RID: 15244 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003085 RID: 12421
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003086 RID: 12422
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;
}
