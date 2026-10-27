using System;
using UnityEngine.Scripting;

// Token: 0x020007E6 RID: 2022
[Preserve]
public class NetPackageEntityAliveFlags : NetPackageEntityTargeted
{
	// Token: 0x06003BAD RID: 15277 RVA: 0x0018040C File Offset: 0x0017E60C
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

	// Token: 0x06003BAE RID: 15278 RVA: 0x001804DE File Offset: 0x0017E6DE
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.flags = _reader.ReadUInt16();
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x001804F3 File Offset: 0x0017E6F3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.flags);
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x00180508 File Offset: 0x0017E708
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

	// Token: 0x04003090 RID: 12432
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingEnemy = 1;

	// Token: 0x04003091 RID: 12433
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingPlayer = 2;

	// Token: 0x04003092 RID: 12434
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFAimingGun = 4;

	// Token: 0x04003093 RID: 12435
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFSpawned = 8;

	// Token: 0x04003094 RID: 12436
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFJumping = 16;

	// Token: 0x04003095 RID: 12437
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsBreakingBlocks = 32;

	// Token: 0x04003096 RID: 12438
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 64;

	// Token: 0x04003097 RID: 12439
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsGodMode = 128;

	// Token: 0x04003098 RID: 12440
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFCrouching = 256;

	// Token: 0x04003099 RID: 12441
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort flags;
}
