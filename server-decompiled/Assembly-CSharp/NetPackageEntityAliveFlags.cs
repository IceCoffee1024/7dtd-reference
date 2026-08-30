using System;
using UnityEngine.Scripting;

// Token: 0x020007D9 RID: 2009
[Preserve]
public class NetPackageEntityAliveFlags : NetPackageEntityTargeted
{
	// Token: 0x06003B72 RID: 15218 RVA: 0x0017ED88 File Offset: 0x0017CF88
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
		if (_entity.inventory.IsFlashlightOn)
		{
			this.flags |= 128;
		}
		if (_entity.IsGodMode.Value)
		{
			this.flags |= 256;
		}
		if (_entity.IsCrouching)
		{
			this.flags |= 512;
		}
		return this;
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x0017EE7A File Offset: 0x0017D07A
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.flags = _reader.ReadUInt16();
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x0017EE8F File Offset: 0x0017D08F
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.flags);
	}

	// Token: 0x06003B75 RID: 15221 RVA: 0x0017EEA4 File Offset: 0x0017D0A4
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
			entityAlive.IsGodMode.Value = (this.flags & 256) > 0;
			entityAlive.Crouching = (this.flags & 512) > 0;
			if (entityAlive.isEntityRemote)
			{
				entityAlive.bReplicatedAlertFlag = (this.flags & 64) > 0;
			}
			entityAlive.inventory.SetFlashlight((this.flags & 128) > 0);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityAliveFlags>().Setup(entityAlive), false, -1, base.Sender.entityId, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x00177A99 File Offset: 0x00175C99
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x04003076 RID: 12406
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingEnemy = 1;

	// Token: 0x04003077 RID: 12407
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingPlayer = 2;

	// Token: 0x04003078 RID: 12408
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFAimingGun = 4;

	// Token: 0x04003079 RID: 12409
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFSpawned = 8;

	// Token: 0x0400307A RID: 12410
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFJumping = 16;

	// Token: 0x0400307B RID: 12411
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsBreakingBlocks = 32;

	// Token: 0x0400307C RID: 12412
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 64;

	// Token: 0x0400307D RID: 12413
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsFlashlightOn = 128;

	// Token: 0x0400307E RID: 12414
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsGodMode = 256;

	// Token: 0x0400307F RID: 12415
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFCrouching = 512;

	// Token: 0x04003080 RID: 12416
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort flags;
}
