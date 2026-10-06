using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E6 RID: 2022
[Preserve]
public class NetPackageEntityRagdoll : NetPackage
{
	// Token: 0x06003BC1 RID: 15297 RVA: 0x001800CD File Offset: 0x0017E2CD
	public NetPackageEntityRagdoll Setup(Entity _entity, byte _state)
	{
		this.entityId = _entity.entityId;
		this.flags = 4;
		this.state = _state;
		return this;
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x001800EC File Offset: 0x0017E2EC
	public NetPackageEntityRagdoll Setup(Entity _entity, byte _mode, float _duration, EnumBodyPartHit _bodyPart, Vector3 _forceVec, Vector3 _forceWorldPos)
	{
		this.entityId = _entity.entityId;
		this.flags = 1;
		if (_mode != 0)
		{
			this.flags |= 2;
			this.mode = _mode;
		}
		this.duration = _duration;
		this.bodyPart = _bodyPart;
		this.forceVec = _forceVec;
		this.forceWorldPos = _forceWorldPos;
		this.hipPos = _entity.emodel.GetHipPosition();
		return this;
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x00180158 File Offset: 0x0017E358
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
		this.flags = _br.ReadByte();
		if ((this.flags & 1) > 0)
		{
			this.duration = _br.ReadSingle();
			this.bodyPart = (EnumBodyPartHit)_br.ReadInt16();
			this.forceVec = StreamUtils.ReadVector3(_br);
			this.forceWorldPos = StreamUtils.ReadVector3(_br);
			this.hipPos = StreamUtils.ReadVector3(_br);
		}
		this.mode = 0;
		if ((this.flags & 2) > 0)
		{
			this.mode = _br.ReadByte();
		}
		this.state = 0;
		if ((this.flags & 4) > 0)
		{
			this.state = _br.ReadByte();
		}
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x00180200 File Offset: 0x0017E400
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
		_bw.Write(this.flags);
		if ((this.flags & 1) > 0)
		{
			_bw.Write(this.duration);
			_bw.Write((short)this.bodyPart);
			StreamUtils.Write(_bw, this.forceVec);
			StreamUtils.Write(_bw, this.forceWorldPos);
			StreamUtils.Write(_bw, this.hipPos);
		}
		if ((this.flags & 2) > 0)
		{
			_bw.Write(this.mode);
		}
		if ((this.flags & 4) > 0)
		{
			_bw.Write(this.state);
		}
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x001802A4 File Offset: 0x0017E4A4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive == null)
		{
			Log.Out("Discarding " + base.GetType().Name + " for entity Id=" + this.entityId.ToString());
			return;
		}
		if ((this.flags & 1) > 0)
		{
			entityAlive.emodel.DoRagdoll((EModelBase.RagdollMode)this.mode, this.duration, this.bodyPart, this.forceVec, this.forceWorldPos, true);
		}
		if ((this.flags & 4) > 0)
		{
			entityAlive.emodel.SetRagdollState((int)this.state);
		}
	}

	// Token: 0x04003097 RID: 12439
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003098 RID: 12440
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsForce = 1;

	// Token: 0x04003099 RID: 12441
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsMode = 2;

	// Token: 0x0400309A RID: 12442
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsState = 4;

	// Token: 0x0400309B RID: 12443
	[PublicizedFrom(EAccessModifier.Private)]
	public byte flags;

	// Token: 0x0400309C RID: 12444
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x0400309D RID: 12445
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBodyPartHit bodyPart;

	// Token: 0x0400309E RID: 12446
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceVec;

	// Token: 0x0400309F RID: 12447
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceWorldPos;

	// Token: 0x040030A0 RID: 12448
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 hipPos;

	// Token: 0x040030A1 RID: 12449
	[PublicizedFrom(EAccessModifier.Private)]
	public byte mode;

	// Token: 0x040030A2 RID: 12450
	[PublicizedFrom(EAccessModifier.Private)]
	public byte state;
}
