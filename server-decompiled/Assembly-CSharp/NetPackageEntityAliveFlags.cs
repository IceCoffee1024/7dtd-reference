using System;
using UnityEngine.Scripting;

// Token: 0x020007CE RID: 1998
[Preserve]
public class NetPackageEntityAliveFlags : NetPackageEntityTargeted
{
	// Token: 0x06003B13 RID: 15123 RVA: 0x0017C9B0 File Offset: 0x0017ABB0
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

	// Token: 0x06003B14 RID: 15124 RVA: 0x0017CAA2 File Offset: 0x0017ACA2
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.flags = _reader.ReadUInt16();
	}

	// Token: 0x06003B15 RID: 15125 RVA: 0x0017CAB7 File Offset: 0x0017ACB7
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.flags);
	}

	// Token: 0x06003B16 RID: 15126 RVA: 0x0017CACC File Offset: 0x0017ACCC
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

	// Token: 0x06003B17 RID: 15127 RVA: 0x00175819 File Offset: 0x00173A19
	public override int GetLength()
	{
		return 60;
	}

	// Token: 0x04002FF7 RID: 12279
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingEnemy = 1;

	// Token: 0x04002FF8 RID: 12280
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFApproachingPlayer = 2;

	// Token: 0x04002FF9 RID: 12281
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFAimingGun = 4;

	// Token: 0x04002FFA RID: 12282
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFSpawned = 8;

	// Token: 0x04002FFB RID: 12283
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFJumping = 16;

	// Token: 0x04002FFC RID: 12284
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsBreakingBlocks = 32;

	// Token: 0x04002FFD RID: 12285
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsAlert = 64;

	// Token: 0x04002FFE RID: 12286
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsFlashlightOn = 128;

	// Token: 0x04002FFF RID: 12287
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFIsGodMode = 256;

	// Token: 0x04003000 RID: 12288
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFCrouching = 512;

	// Token: 0x04003001 RID: 12289
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort flags;
}
