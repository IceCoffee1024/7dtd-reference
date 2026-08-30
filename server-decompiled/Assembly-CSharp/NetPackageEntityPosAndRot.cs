using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E1 RID: 2017
[Preserve]
public class NetPackageEntityPosAndRot : NetPackageEntityTargeted
{
	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0017F890 File Offset: 0x0017DA90
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

	// Token: 0x06003BA6 RID: 15270 RVA: 0x0017F8E8 File Offset: 0x0017DAE8
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

	// Token: 0x06003BA7 RID: 15271 RVA: 0x0017F97C File Offset: 0x0017DB7C
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

	// Token: 0x06003BA8 RID: 15272 RVA: 0x0017FA68 File Offset: 0x0017DC68
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

	// Token: 0x06003BA9 RID: 15273 RVA: 0x0004AC4A File Offset: 0x00048E4A
	public override int GetLength()
	{
		return 25;
	}

	// Token: 0x04003098 RID: 12440
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x04003099 RID: 12441
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 rot;

	// Token: 0x0400309A RID: 12442
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onGround;

	// Token: 0x0400309B RID: 12443
	[PublicizedFrom(EAccessModifier.Protected)]
	public Quaternion qrot;

	// Token: 0x0400309C RID: 12444
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseQRotation;
}
