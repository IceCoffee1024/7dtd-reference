using System;
using UnityEngine.Scripting;

// Token: 0x0200082F RID: 2095
[Preserve]
public class NetPackagePlayerSpawnedInWorld : NetPackage
{
	// Token: 0x06003D88 RID: 15752 RVA: 0x00186616 File Offset: 0x00184816
	public NetPackagePlayerSpawnedInWorld Setup(RespawnType _respawnReason, Vector3i _position, int _entityId)
	{
		this.respawnReason = _respawnReason;
		this.position = _position;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06003D89 RID: 15753 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003D8A RID: 15754 RVA: 0x0018662E File Offset: 0x0018482E
	public override void read(PooledBinaryReader _reader)
	{
		this.respawnReason = (RespawnType)_reader.ReadInt32();
		this.position = StreamUtils.ReadVector3i(_reader);
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003D8B RID: 15755 RVA: 0x00186655 File Offset: 0x00184855
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.respawnReason);
		StreamUtils.Write(_writer, this.position);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003D8C RID: 15756 RVA: 0x00186684 File Offset: 0x00184884
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

	// Token: 0x06003D8D RID: 15757 RVA: 0x0017E246 File Offset: 0x0017C446
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x040031DA RID: 12762
	[PublicizedFrom(EAccessModifier.Private)]
	public RespawnType respawnReason;

	// Token: 0x040031DB RID: 12763
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i position;

	// Token: 0x040031DC RID: 12764
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
