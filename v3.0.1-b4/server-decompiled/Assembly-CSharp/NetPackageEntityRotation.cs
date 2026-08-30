using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007DB RID: 2011
[Preserve]
public class NetPackageEntityRotation : NetPackageEntityTargeted
{
	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06003B68 RID: 15208 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x0017DCA6 File Offset: 0x0017BEA6
	public NetPackageEntityRotation Setup(int _entityId, Vector3i _rot, Quaternion _qrot, bool _bUseQRot)
	{
		base.Setup(_entityId);
		this.rot = _rot;
		this.qrot = _qrot;
		this.bUseQRotation = _bUseQRot;
		return this;
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x0017DCC8 File Offset: 0x0017BEC8
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

	// Token: 0x06003B6B RID: 15211 RVA: 0x0017DD34 File Offset: 0x0017BF34
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

	// Token: 0x06003B6C RID: 15212 RVA: 0x0017DDDC File Offset: 0x0017BFDC
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

	// Token: 0x06003B6D RID: 15213 RVA: 0x0017DEA0 File Offset: 0x0017C0A0
	public override int GetLength()
	{
		return 12;
	}

	// Token: 0x0400302F RID: 12335
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i rot;

	// Token: 0x04003030 RID: 12336
	[PublicizedFrom(EAccessModifier.Protected)]
	public Quaternion qrot;

	// Token: 0x04003031 RID: 12337
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseQRotation;
}
