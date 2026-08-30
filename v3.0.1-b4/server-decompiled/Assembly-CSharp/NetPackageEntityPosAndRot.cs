using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D6 RID: 2006
[Preserve]
public class NetPackageEntityPosAndRot : NetPackageEntityTargeted
{
	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06003B45 RID: 15173 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003B46 RID: 15174 RVA: 0x0017D4B8 File Offset: 0x0017B6B8
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

	// Token: 0x06003B47 RID: 15175 RVA: 0x0017D510 File Offset: 0x0017B710
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

	// Token: 0x06003B48 RID: 15176 RVA: 0x0017D5A4 File Offset: 0x0017B7A4
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

	// Token: 0x06003B49 RID: 15177 RVA: 0x0017D690 File Offset: 0x0017B890
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

	// Token: 0x06003B4A RID: 15178 RVA: 0x0004A832 File Offset: 0x00048A32
	public override int GetLength()
	{
		return 25;
	}

	// Token: 0x04003019 RID: 12313
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x0400301A RID: 12314
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 rot;

	// Token: 0x0400301B RID: 12315
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onGround;

	// Token: 0x0400301C RID: 12316
	[PublicizedFrom(EAccessModifier.Protected)]
	public Quaternion qrot;

	// Token: 0x0400301D RID: 12317
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseQRotation;
}
