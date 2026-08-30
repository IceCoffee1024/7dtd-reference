using System;
using UnityEngine.Scripting;

// Token: 0x02000824 RID: 2084
[Preserve]
public class NetPackagePlayerSpawnedInWorld : NetPackage
{
	// Token: 0x06003D29 RID: 15657 RVA: 0x0018423E File Offset: 0x0018243E
	public NetPackagePlayerSpawnedInWorld Setup(RespawnType _respawnReason, Vector3i _position, int _entityId)
	{
		this.respawnReason = _respawnReason;
		this.position = _position;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06003D2A RID: 15658 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x00184256 File Offset: 0x00182456
	public override void read(PooledBinaryReader _reader)
	{
		this.respawnReason = (RespawnType)_reader.ReadInt32();
		this.position = StreamUtils.ReadVector3i(_reader);
		this.entityId = _reader.ReadInt32();
	}

	// Token: 0x06003D2C RID: 15660 RVA: 0x0018427D File Offset: 0x0018247D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((int)this.respawnReason);
		StreamUtils.Write(_writer, this.position);
		_writer.Write(this.entityId);
	}

	// Token: 0x06003D2D RID: 15661 RVA: 0x001842AC File Offset: 0x001824AC
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

	// Token: 0x06003D2E RID: 15662 RVA: 0x0017BE6E File Offset: 0x0017A06E
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x0400315B RID: 12635
	[PublicizedFrom(EAccessModifier.Private)]
	public RespawnType respawnReason;

	// Token: 0x0400315C RID: 12636
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i position;

	// Token: 0x0400315D RID: 12637
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
