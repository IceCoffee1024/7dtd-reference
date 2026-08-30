using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E4 RID: 2020
[Preserve]
public class NetPackageEntityRelPosAndRot : NetPackageEntityRotation
{
	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06003BB9 RID: 15289 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x0017FE10 File Offset: 0x0017E010
	public NetPackageEntityRelPosAndRot Setup(int _entityId, Vector3i _deltaPos, Vector3i _absRot, Quaternion _qrot, bool _onGround, bool _bUseQRot, int _updateSteps)
	{
		base.Setup(_entityId, _absRot, _qrot, _bUseQRot);
		this.dPos = _deltaPos;
		this.onGround = _onGround;
		this.updateSteps = (short)_updateSteps;
		return this;
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x0017FE38 File Offset: 0x0017E038
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.dPos = new Vector3i((int)_reader.ReadInt16(), (int)_reader.ReadInt16(), (int)_reader.ReadInt16());
		this.onGround = _reader.ReadBoolean();
		this.updateSteps = _reader.ReadInt16();
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x0017FE78 File Offset: 0x0017E078
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.dPos.x);
		_writer.Write((short)this.dPos.y);
		_writer.Write((short)this.dPos.z);
		_writer.Write(this.onGround);
		_writer.Write(this.updateSteps);
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x0017FEDC File Offset: 0x0017E0DC
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
		entity.serverPos += this.dPos;
		Vector3 vector = entity.serverPos.ToVector3() / 32f;
		Vector3 vector2 = new Vector3((float)(this.rot.x * 360) / 256f, (float)(this.rot.y * 360) / 256f, (float)(this.rot.z * 360) / 256f);
		if (this.bUseQRotation)
		{
			entity.SetPosAndQRotFromNetwork(vector, this.qrot, (int)this.updateSteps);
		}
		else
		{
			entity.SetPosAndRotFromNetwork(vector, vector2, (int)this.updateSteps);
		}
		entity.onGround = this.onGround;
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040030AA RID: 12458
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i dPos;

	// Token: 0x040030AB RID: 12459
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onGround;

	// Token: 0x040030AC RID: 12460
	[PublicizedFrom(EAccessModifier.Private)]
	public short updateSteps;
}
