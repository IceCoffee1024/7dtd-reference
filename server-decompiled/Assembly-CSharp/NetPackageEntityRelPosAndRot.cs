using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F1 RID: 2033
[Preserve]
public class NetPackageEntityRelPosAndRot : NetPackageEntityRotation
{
	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06003BEA RID: 15338 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x00181450 File Offset: 0x0017F650
	public NetPackageEntityRelPosAndRot Setup(int _entityId, Vector3i _deltaPos, Vector3i _absRot, Quaternion _qrot, bool _onGround, bool _bUseQRot, int _updateSteps)
	{
		base.Setup(_entityId, _absRot, _qrot, _bUseQRot);
		this.dPos = _deltaPos;
		this.onGround = _onGround;
		this.updateSteps = (short)_updateSteps;
		return this;
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x00181478 File Offset: 0x0017F678
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.dPos = new Vector3i((int)_reader.ReadInt16(), (int)_reader.ReadInt16(), (int)_reader.ReadInt16());
		this.onGround = _reader.ReadBoolean();
		this.updateSteps = _reader.ReadInt16();
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x001814B8 File Offset: 0x0017F6B8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.dPos.x);
		_writer.Write((short)this.dPos.y);
		_writer.Write((short)this.dPos.z);
		_writer.Write(this.onGround);
		_writer.Write(this.updateSteps);
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x0018151C File Offset: 0x0017F71C
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

	// Token: 0x040030C3 RID: 12483
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i dPos;

	// Token: 0x040030C4 RID: 12484
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onGround;

	// Token: 0x040030C5 RID: 12485
	[PublicizedFrom(EAccessModifier.Private)]
	public short updateSteps;
}
