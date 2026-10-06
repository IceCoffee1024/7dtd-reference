using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E4 RID: 2020
[Preserve]
public class NetPackageEntityPosAndRot : NetPackageEntityTargeted
{
	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x0017FDD0 File Offset: 0x0017DFD0
	public NetPackageEntityPosAndRot Setup(Entity _entity)
	{
		base.Setup(_entity.entityId);
		this.pos = _entity.position;
		this.rot = _entity.rotation;
		this.onGround = _entity.onGround;
		this.qrot = _entity.qrotation;
		this.bUseQRotation = _entity.IsQRotationUsed();
		return this;
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0017FE28 File Offset: 0x0017E028
	public override void read(PooledBinaryReader _br)
	{
		base.read(_br);
		this.pos = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		this.bUseQRotation = _br.ReadBoolean();
		if (!this.bUseQRotation)
		{
			this.rot = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		}
		else
		{
			this.qrot = new Quaternion(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		}
		this.onGround = _br.ReadBoolean();
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x0017FEBC File Offset: 0x0017E0BC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.pos.x);
		_bw.Write(this.pos.y);
		_bw.Write(this.pos.z);
		_bw.Write(this.bUseQRotation);
		if (!this.bUseQRotation)
		{
			_bw.Write(this.rot.x);
			_bw.Write(this.rot.y);
			_bw.Write(this.rot.z);
		}
		else
		{
			_bw.Write(this.qrot.x);
			_bw.Write(this.qrot.y);
			_bw.Write(this.qrot.z);
			_bw.Write(this.qrot.w);
		}
		_bw.Write(this.onGround);
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x0017FFA8 File Offset: 0x0017E1A8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.entityId, true))
		{
			return;
		}
		Entity entity = _world.GetEntity(this.entityId);
		if (entity == null)
		{
			return;
		}
		Entity attachedMainEntity = entity.AttachedMainEntity;
		if (attachedMainEntity != null && _world.GetPrimaryPlayerId() == attachedMainEntity.entityId)
		{
			return;
		}
		entity.serverPos = NetEntityDistributionEntry.EncodePos(this.pos);
		if (this.bUseQRotation)
		{
			entity.SetPosAndQRotFromNetwork(this.pos, this.qrot, 3);
		}
		else
		{
			entity.SetPosAndRotFromNetwork(this.pos, this.rot, 3);
		}
		entity.onGround = this.onGround;
	}

	// Token: 0x04003091 RID: 12433
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x04003092 RID: 12434
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 rot;

	// Token: 0x04003093 RID: 12435
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onGround;

	// Token: 0x04003094 RID: 12436
	[PublicizedFrom(EAccessModifier.Protected)]
	public Quaternion qrot;

	// Token: 0x04003095 RID: 12437
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseQRotation;
}
