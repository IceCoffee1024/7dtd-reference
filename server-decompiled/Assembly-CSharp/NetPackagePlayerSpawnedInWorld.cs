using System;
using UnityEngine.Scripting;

// Token: 0x02000830 RID: 2096
[Preserve]
public class NetPackagePlayerSpawnedInWorld : NetPackage
{
	// Token: 0x06003D45 RID: 15685 RVA: 0x0018614E File Offset: 0x0018434E
	public NetPackagePlayerSpawnedInWorld Setup(RespawnType _respawnReason, Vector3i _position, int _entityId)
	{
		this.respawnReason = _respawnReason;
		this.position = _position;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06003D46 RID: 15686 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003D47 RID: 15687 RVA: 0x00186166 File Offset: 0x00184366
	public override void read(PooledBinaryReader _reader)
	{
		this.respawnReason = (RespawnType)_reader.ReadInt32();
		this.position = StreamUtils.ReadVector3i(_reader);
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003D48 RID: 15688 RVA: 0x0018618D File Offset: 0x0018438D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.respawnReason);
		StreamUtils.Write(_writer, this.position);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003D49 RID: 15689 RVA: 0x001861BC File Offset: 0x001843BC
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

	// Token: 0x040031C1 RID: 12737
	[PublicizedFrom(EAccessModifier.Private)]
	public RespawnType respawnReason;

	// Token: 0x040031C2 RID: 12738
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i position;

	// Token: 0x040031C3 RID: 12739
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
