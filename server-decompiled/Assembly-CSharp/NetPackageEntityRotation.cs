using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E6 RID: 2022
[Preserve]
public class NetPackageEntityRotation : NetPackageEntityTargeted
{
	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BC8 RID: 15304 RVA: 0x0018007E File Offset: 0x0017E27E
	public NetPackageEntityRotation Setup(int _entityId, Vector3i _rot, Quaternion _qrot, bool _bUseQRot)
	{
		base.Setup(_entityId);
		this.rot = _rot;
		this.qrot = _qrot;
		this.bUseQRotation = _bUseQRot;
		return this;
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x001800A0 File Offset: 0x0017E2A0
	public override void read(PooledBinaryReader _br)
	{
		base.read(_br);
		this.bUseQRotation = _br.ReadBoolean();
		if (!this.bUseQRotation)
		{
			this.rot = new Vector3i((int)_br.ReadInt16(), (int)_br.ReadInt16(), (int)_br.ReadInt16());
			return;
		}
		this.qrot = new Quaternion(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x0018010C File Offset: 0x0017E30C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.bUseQRotation);
		if (!this.bUseQRotation)
		{
			_bw.Write((short)this.rot.x);
			_bw.Write((short)this.rot.y);
			_bw.Write((short)this.rot.z);
			return;
		}
		_bw.Write(this.qrot.x);
		_bw.Write(this.qrot.y);
		_bw.Write(this.qrot.z);
		_bw.Write(this.qrot.w);
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x001801B4 File Offset: 0x0017E3B4
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
		if (this.bUseQRotation)
		{
			entity.SetQRotFromNetwork(this.qrot, 3);
			return;
		}
		Vector3 vector = new Vector3((float)(this.rot.x * 360) / 256f, (float)(this.rot.y * 360) / 256f, (float)(this.rot.z * 360) / 256f);
		entity.SetRotFromNetwork(vector, 3);
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x00180278 File Offset: 0x0017E478
	public override int GetLength()
	{
		return 12;
	}

	// Token: 0x040030AE RID: 12462
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i rot;

	// Token: 0x040030AF RID: 12463
	[PublicizedFrom(EAccessModifier.Protected)]
	public Quaternion qrot;

	// Token: 0x040030B0 RID: 12464
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseQRotation;
}
