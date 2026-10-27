using System;
using UnityEngine.Scripting;

// Token: 0x0200083A RID: 2106
[Preserve]
public class NetPackagePlayerSpawnedInWorld : NetPackage
{
	// Token: 0x06003D68 RID: 15720 RVA: 0x00187252 File Offset: 0x00185452
	public NetPackagePlayerSpawnedInWorld Setup(RespawnType _respawnReason, Vector3i _position, int _entityId)
	{
		this.respawnReason = _respawnReason;
		this.position = _position;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06003D69 RID: 15721 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x0018726A File Offset: 0x0018546A
	public override void read(PooledBinaryReader _reader)
	{
		this.respawnReason = (RespawnType)_reader.ReadInt32();
		this.position = StreamUtils.ReadVector3i(_reader);
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003D6B RID: 15723 RVA: 0x00187291 File Offset: 0x00185491
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.respawnReason);
		StreamUtils.Write(_writer, this.position);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003D6C RID: 15724 RVA: 0x001872C0 File Offset: 0x001854C0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, false))
		{
			return;
		}
		GameManager.Instance.PlayerSpawnedInWorld(base.Sender, this.respawnReason, this.position, this.entityId);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerSpawnedInWorld>().Setup(this.respawnReason, new Vector3i(this.position), this.entityId), false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x040031E1 RID: 12769
	[PublicizedFrom(EAccessModifier.Private)]
	public RespawnType respawnReason;

	// Token: 0x040031E2 RID: 12770
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i position;

	// Token: 0x040031E3 RID: 12771
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
