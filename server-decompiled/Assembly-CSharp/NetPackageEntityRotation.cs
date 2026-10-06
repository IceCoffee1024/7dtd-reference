using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E9 RID: 2025
[Preserve]
public class NetPackageEntityRotation : NetPackageEntityTargeted
{
	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x001805BA File Offset: 0x0017E7BA
	public NetPackageEntityRotation Setup(int _entityId, Vector3i _rot, Quaternion _qrot, bool _bUseQRot)
	{
		base.Setup(_entityId);
		this.rot = _rot;
		this.qrot = _qrot;
		this.bUseQRotation = _bUseQRot;
		return this;
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x001805DC File Offset: 0x0017E7DC
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

	// Token: 0x06003BD6 RID: 15318 RVA: 0x00180648 File Offset: 0x0017E848
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

	// Token: 0x06003BD7 RID: 15319 RVA: 0x001806F0 File Offset: 0x0017E8F0
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

	// Token: 0x040030A7 RID: 12455
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i rot;

	// Token: 0x040030A8 RID: 12456
	[PublicizedFrom(EAccessModifier.Protected)]
	public Quaternion qrot;

	// Token: 0x040030A9 RID: 12457
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseQRotation;
}
