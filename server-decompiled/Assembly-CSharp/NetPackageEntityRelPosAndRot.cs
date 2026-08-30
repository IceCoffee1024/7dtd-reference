using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D9 RID: 2009
[Preserve]
public class NetPackageEntityRelPosAndRot : NetPackageEntityRotation
{
	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06003B5A RID: 15194 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ReliableDelivery
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x0017DA38 File Offset: 0x0017BC38
	public NetPackageEntityRelPosAndRot Setup(int _entityId, Vector3i _deltaPos, Vector3i _absRot, Quaternion _qrot, bool _onGround, bool _bUseQRot, int _updateSteps)
	{
		base.Setup(_entityId, _absRot, _qrot, _bUseQRot);
		this.dPos = _deltaPos;
		this.onGround = _onGround;
		this.updateSteps = (short)_updateSteps;
		return this;
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x0017DA60 File Offset: 0x0017BC60
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.dPos = new Vector3i((int)_reader.ReadInt16(), (int)_reader.ReadInt16(), (int)_reader.ReadInt16());
		this.onGround = _reader.ReadBoolean();
		this.updateSteps = _reader.ReadInt16();
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x0017DAA0 File Offset: 0x0017BCA0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.dPos.x);
		_writer.Write((short)this.dPos.y);
		_writer.Write((short)this.dPos.z);
		_writer.Write(this.onGround);
		_writer.Write(this.updateSteps);
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x0017DB04 File Offset: 0x0017BD04
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

	// Token: 0x06003B5F RID: 15199 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400302B RID: 12331
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i dPos;

	// Token: 0x0400302C RID: 12332
	[PublicizedFrom(EAccessModifier.Private)]
	public bool onGround;

	// Token: 0x0400302D RID: 12333
	[PublicizedFrom(EAccessModifier.Private)]
	public short updateSteps;
}
