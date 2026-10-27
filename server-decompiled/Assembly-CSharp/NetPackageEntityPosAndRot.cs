using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007EE RID: 2030
[Preserve]
public class NetPackageEntityPosAndRot : NetPackageEntityTargeted
{
	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06003BD8 RID: 15320 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x00180ED4 File Offset: 0x0017F0D4
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

	// Token: 0x06003BDA RID: 15322 RVA: 0x00180F2C File Offset: 0x0017F12C
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

	// Token: 0x06003BDB RID: 15323 RVA: 0x00180FC0 File Offset: 0x0017F1C0
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

	// Token: 0x06003BDC RID: 15324 RVA: 0x001810AC File Offset: 0x0017F2AC
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

	// Token: 0x040030B1 RID: 12465
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 pos;

	// Token: 0x040030B2 RID: 12466
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 rot;

	// Token: 0x040030B3 RID: 12467
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool onGround;

	// Token: 0x040030B4 RID: 12468
	[PublicizedFrom(EAccessModifier.Protected)]
	public Quaternion qrot;

	// Token: 0x040030B5 RID: 12469
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseQRotation;
}
