using System;
using UnityEngine.Scripting;

// Token: 0x020007D5 RID: 2005
[Preserve]
public class NetPackageDroneRecoveryResponse : NetPackage
{
	// Token: 0x06003B60 RID: 15200 RVA: 0x0017F784 File Offset: 0x0017D984
	public NetPackageDroneRecoveryResponse Setup(int _playerId, int _droneId)
	{
		this.playerId = _playerId;
		this.droneId = _droneId;
		return this;
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x0017F795 File Offset: 0x0017D995
	public override void read(PooledBinaryReader _reader)
	{
		this.playerId = _reader.ReadInt32();
		this.droneId = _reader.ReadInt32();
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x0017F7AF File Offset: 0x0017D9AF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.playerId);
		_writer.Write(this.droneId);
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x0017F7D0 File Offset: 0x0017D9D0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (this.droneId != -1)
		{
			EntityPlayer entityPlayer = _world.GetEntity(this.playerId) as EntityPlayer;
			if (entityPlayer)
			{
				EntityDrone entityDrone = _world.GetEntity(this.droneId) as EntityDrone;
				if (entityDrone)
				{
					entityPlayer.AddOwnedEntity(entityDrone);
				}
			}
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06003B64 RID: 15204 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400305B RID: 12379
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x0400305C RID: 12380
	[PublicizedFrom(EAccessModifier.Private)]
	public int droneId;
}
