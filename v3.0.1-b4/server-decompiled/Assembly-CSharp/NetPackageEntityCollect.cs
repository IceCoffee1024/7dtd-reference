using System;
using UnityEngine.Scripting;

// Token: 0x020007D1 RID: 2001
[Preserve]
public class NetPackageEntityCollect : NetPackage
{
	// Token: 0x06003B29 RID: 15145 RVA: 0x0017CE37 File Offset: 0x0017B037
	public NetPackageEntityCollect Setup(int _entityId, int _playerId)
	{
		this.entityId = _entityId;
		this.playerId = _playerId;
		return this;
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x0017CE48 File Offset: 0x0017B048
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.playerId = _br.ReadInt32();
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x0017CE62 File Offset: 0x0017B062
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write(this.playerId);
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x0017CE84 File Offset: 0x0017B084
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

	// Token: 0x06003B2D RID: 15149 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003006 RID: 12294
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003007 RID: 12295
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;
}
