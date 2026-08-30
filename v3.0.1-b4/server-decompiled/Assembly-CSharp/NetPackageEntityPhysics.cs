using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D5 RID: 2005
[Preserve]
public class NetPackageEntityPhysics : NetPackage
{
	// Token: 0x06003B3E RID: 15166 RVA: 0x0017D0E8 File Offset: 0x0017B2E8
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

	// Token: 0x06003B3F RID: 15167 RVA: 0x0017D164 File Offset: 0x0017B364
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

	// Token: 0x06003B40 RID: 15168 RVA: 0x0017D1BC File Offset: 0x0017B3BC
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

	// Token: 0x06003B41 RID: 15169 RVA: 0x0017D2C0 File Offset: 0x0017B4C0
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

	// Token: 0x06003B42 RID: 15170 RVA: 0x0017D3CC File Offset: 0x0017B5CC
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

	// Token: 0x06003B43 RID: 15171 RVA: 0x0017D4B4 File Offset: 0x0017B6B4
	public override int GetLength()
	{
		return 58;
	}

	// Token: 0x04003010 RID: 12304
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagIsMaster = 1;

	// Token: 0x04003011 RID: 12305
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagIsCollided = 2;

	// Token: 0x04003012 RID: 12306
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagOnGround = 4;

	// Token: 0x04003013 RID: 12307
	[PublicizedFrom(EAccessModifier.Private)]
	public int EntityId;

	// Token: 0x04003014 RID: 12308
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 Pos;

	// Token: 0x04003015 RID: 12309
	[PublicizedFrom(EAccessModifier.Private)]
	public Quaternion QRot;

	// Token: 0x04003016 RID: 12310
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 Velocity;

	// Token: 0x04003017 RID: 12311
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 AngularVelocity;

	// Token: 0x04003018 RID: 12312
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort Flags;
}
