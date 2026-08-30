using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E0 RID: 2016
[Preserve]
public class NetPackageEntityPhysics : NetPackage
{
	// Token: 0x06003B9D RID: 15261 RVA: 0x0017F4C0 File Offset: 0x0017D6C0
	public NetPackageEntityPhysics Setup(Entity _entity)
	{
		this.EntityId = _entity.entityId;
		this.Pos = _entity.position;
		this.QRot = _entity.qrotation;
		ushort num = 0;
		if (_entity.isPhysicsMaster)
		{
			num |= 1;
		}
		if (_entity.isCollided)
		{
			num |= 2;
		}
		if (_entity.onGround)
		{
			num |= 4;
		}
		this.Velocity = _entity.GetVelocityPerSecond();
		this.AngularVelocity = _entity.GetAngularVelocityPerSecond();
		this.Flags = num;
		return this;
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x0017F53C File Offset: 0x0017D73C
	public NetPackageEntityPhysics Setup(NetPackageEntityPhysics _p)
	{
		this.EntityId = _p.EntityId;
		this.Pos = _p.Pos;
		this.QRot = _p.QRot;
		this.Velocity = _p.Velocity;
		this.AngularVelocity = _p.AngularVelocity;
		this.Flags = _p.Flags;
		return this;
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0017F594 File Offset: 0x0017D794
	public override void read(PooledBinaryReader _br)
	{
		this.Flags = _br.ReadUInt16();
		this.EntityId = _br.ReadInt32();
		this.Pos.x = _br.ReadSingle();
		this.Pos.y = _br.ReadSingle();
		this.Pos.z = _br.ReadSingle();
		this.QRot.x = _br.ReadSingle();
		this.QRot.y = _br.ReadSingle();
		this.QRot.z = _br.ReadSingle();
		this.QRot.w = _br.ReadSingle();
		this.Velocity.x = _br.ReadSingle();
		this.Velocity.y = _br.ReadSingle();
		this.Velocity.z = _br.ReadSingle();
		this.AngularVelocity.x = _br.ReadSingle();
		this.AngularVelocity.y = _br.ReadSingle();
		this.AngularVelocity.z = _br.ReadSingle();
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x0017F698 File Offset: 0x0017D898
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.Flags);
		_bw.Write(this.EntityId);
		_bw.Write(this.Pos.x);
		_bw.Write(this.Pos.y);
		_bw.Write(this.Pos.z);
		_bw.Write(this.QRot.x);
		_bw.Write(this.QRot.y);
		_bw.Write(this.QRot.z);
		_bw.Write(this.QRot.w);
		_bw.Write(this.Velocity.x);
		_bw.Write(this.Velocity.y);
		_bw.Write(this.Velocity.z);
		_bw.Write(this.AngularVelocity.x);
		_bw.Write(this.AngularVelocity.y);
		_bw.Write(this.AngularVelocity.z);
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0017F7A4 File Offset: 0x0017D9A4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Entity entity = _world.GetEntity(this.EntityId);
		if (entity == null)
		{
			return;
		}
		if (entity.isPhysicsMaster)
		{
			return;
		}
		Entity attachedMainEntity = entity.AttachedMainEntity;
		if (attachedMainEntity != null && _world.GetPrimaryPlayerId() == attachedMainEntity.entityId)
		{
			return;
		}
		entity.serverPos = NetEntityDistributionEntry.EncodePos(this.Pos);
		entity.isCollided = (this.Flags & 2) > 0;
		entity.onGround = (this.Flags & 4) > 0;
		entity.PhysicsMasterSetTargetOrientation(this.Pos, this.QRot);
		entity.SetVelocityPerSecond(this.Velocity, this.AngularVelocity);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if ((this.Flags & 1) == 0)
			{
				entity.PhysicsMasterBecome();
			}
			NetPackageEntityPhysics netPackageEntityPhysics = NetPackageManager.GetPackage<NetPackageEntityPhysics>().Setup(this);
			_world.entityDistributer.SendPacketToTrackedPlayers(entity.entityId, entity.belongsPlayerId, netPackageEntityPhysics, false);
		}
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x0017F88C File Offset: 0x0017DA8C
	public override int GetLength()
	{
		return 58;
	}

	// Token: 0x0400308F RID: 12431
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagIsMaster = 1;

	// Token: 0x04003090 RID: 12432
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagIsCollided = 2;

	// Token: 0x04003091 RID: 12433
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagOnGround = 4;

	// Token: 0x04003092 RID: 12434
	[PublicizedFrom(EAccessModifier.Private)]
	public int EntityId;

	// Token: 0x04003093 RID: 12435
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 Pos;

	// Token: 0x04003094 RID: 12436
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion QRot;

	// Token: 0x04003095 RID: 12437
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 Velocity;

	// Token: 0x04003096 RID: 12438
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 AngularVelocity;

	// Token: 0x04003097 RID: 12439
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort Flags;
}
