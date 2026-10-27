using System;
using UnityEngine.Scripting;

// Token: 0x020007D4 RID: 2004
[Preserve]
public class NetPackageDroneRecoveryRequest : NetPackage
{
	// Token: 0x06003B5A RID: 15194 RVA: 0x0017F6B8 File Offset: 0x0017D8B8
	public NetPackageDroneRecoveryRequest Setup(int _playerId, string _actionId)
	{
		this.playerId = _playerId;
		this.actionId = _actionId;
		return this;
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x0017F6C9 File Offset: 0x0017D8C9
	public override void read(PooledBinaryReader _reader)
	{
		this.playerId = _reader.ReadInt32();
		this.actionId = _reader.ReadString();
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x0017F6E3 File Offset: 0x0017D8E3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.playerId);
		_writer.Write(this.actionId);
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x0017F704 File Offset: 0x0017D904
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(this.playerId) as EntityPlayer;
		EntityDrone entityDrone = null;
		if (entityPlayer)
		{
			entityDrone = DroneManager.Instance.RecoverDroneForPlayer(entityPlayer);
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageDroneRecoveryResponse>().Setup(this.playerId, entityDrone ? entityDrone.entityId : (-1)), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x06003B5E RID: 15198 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x04003059 RID: 12377
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x0400305A RID: 12378
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionId;
}
