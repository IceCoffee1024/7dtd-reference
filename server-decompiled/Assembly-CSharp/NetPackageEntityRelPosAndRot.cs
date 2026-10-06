using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E7 RID: 2023
[Preserve]
public class NetPackageEntityRelPosAndRot : NetPackageEntityRotation
{
	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003BC8 RID: 15304 RVA: 0x0018034C File Offset: 0x0017E54C
	public NetPackageEntityRelPosAndRot Setup(int _entityId, Vector3i _deltaPos, Vector3i _absRot, Quaternion _qrot, bool _onGround, bool _bUseQRot, int _updateSteps)
	{
		base.Setup(_entityId, _absRot, _qrot, _bUseQRot);
		this.dPos = _deltaPos;
		this.onGround = _onGround;
		this.updateSteps = (short)_updateSteps;
		return this;
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x00180374 File Offset: 0x0017E574
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.dPos = new Vector3i((int)_reader.ReadInt16(), (int)_reader.ReadInt16(), (int)_reader.ReadInt16());
		this.onGround = _reader.ReadBoolean();
		this.updateSteps = _reader.ReadInt16();
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x001803B4 File Offset: 0x0017E5B4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.dPos.x);
		_writer.Write((short)this.dPos.y);
		_writer.Write((short)this.dPos.z);
		_writer.Write(this.onGround);
		_writer.Write(this.updateSteps);
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x00180418 File Offset: 0x0017E618
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

	// Token: 0x040030A3 RID: 12451
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i dPos;

	// Token: 0x040030A4 RID: 12452
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onGround;

	// Token: 0x040030A5 RID: 12453
	[PublicizedFrom(EAccessModifier.Private)]
	public short updateSteps;
}
