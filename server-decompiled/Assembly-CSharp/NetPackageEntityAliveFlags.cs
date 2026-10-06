using System;
using UnityEngine.Scripting;

// Token: 0x020007DC RID: 2012
[Preserve]
public class NetPackageEntityAliveFlags : NetPackageEntityTargeted
{
	// Token: 0x06003B8A RID: 15242 RVA: 0x0017F308 File Offset: 0x0017D508
	public NetPackageEntityAliveFlags Setup(EntityAlive _entity)
	{
		base.Setup(_entity.entityId);
		this.flags = 0;
		if (_entity.AimingGun)
		{
			this.flags |= 4;
		}
		if (_entity.Spawned)
		{
			this.flags |= 8;
		}
		if (_entity.Jumping)
		{
			this.flags |= 16;
		}
		if (_entity.IsBreakingBlocks)
		{
			this.flags |= 32;
		}
		if (_entity.IsAlert)
		{
			this.flags |= 64;
		}
		if (_entity.IsGodMode.Value)
		{
			this.flags |= 128;
		}
		if (_entity.IsCrouching)
		{
			this.flags |= 256;
		}
		return this;
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x0017F3DA File Offset: 0x0017D5DA
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.flags = _reader.ReadUInt16();
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x0017F3EF File Offset: 0x0017D5EF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.flags);
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x0017F404 File Offset: 0x0017D604
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
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive)
		{
			entityAlive.AimingGun = (this.flags & 4) > 0;
			entityAlive.Spawned = (this.flags & 8) > 0;
			entityAlive.Jumping = (this.flags & 16) > 0;
			entityAlive.IsBreakingBlocks = (this.flags & 32) > 0;
			entityAlive.IsGodMode.Value = (this.flags & 128) > 0;
			entityAlive.Crouching = (this.flags & 256) > 0;
			if (entityAlive.isEntityRemote)
			{
				entityAlive.bReplicatedAlertFlag = (this.flags & 64) > 0;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityAliveFlags>().Setup(entityAlive), false, -1, base.Sender.entityId, -1, null, 192, false);
			}
		}
	}

	// Token: 0x04003070 RID: 12400
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingEnemy = 1;

	// Token: 0x04003071 RID: 12401
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingPlayer = 2;

	// Token: 0x04003072 RID: 12402
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFAimingGun = 4;

	// Token: 0x04003073 RID: 12403
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFSpawned = 8;

	// Token: 0x04003074 RID: 12404
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFJumping = 16;

	// Token: 0x04003075 RID: 12405
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsBreakingBlocks = 32;

	// Token: 0x04003076 RID: 12406
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 64;

	// Token: 0x04003077 RID: 12407
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsGodMode = 128;

	// Token: 0x04003078 RID: 12408
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFCrouching = 256;

	// Token: 0x04003079 RID: 12409
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort flags;
}
