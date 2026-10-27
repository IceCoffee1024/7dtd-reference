using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F0 RID: 2032
[Preserve]
public class NetPackageEntityRagdoll : NetPackage
{
	// Token: 0x06003BE4 RID: 15332 RVA: 0x001811D1 File Offset: 0x0017F3D1
	public NetPackageEntityRagdoll Setup(Entity _entity, byte _state)
	{
		this.entityId = _entity.entityId;
		this.flags = 4;
		this.state = _state;
		return this;
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x001811F0 File Offset: 0x0017F3F0
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

	// Token: 0x06003BE6 RID: 15334 RVA: 0x0018125C File Offset: 0x0017F45C
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

	// Token: 0x06003BE7 RID: 15335 RVA: 0x00181304 File Offset: 0x0017F504
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

	// Token: 0x06003BE8 RID: 15336 RVA: 0x001813A8 File Offset: 0x0017F5A8
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

	// Token: 0x040030B7 RID: 12471
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030B8 RID: 12472
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsForce = 1;

	// Token: 0x040030B9 RID: 12473
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsMode = 2;

	// Token: 0x040030BA RID: 12474
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsState = 4;

	// Token: 0x040030BB RID: 12475
	[PublicizedFrom(EAccessModifier.Private)]
	public byte flags;

	// Token: 0x040030BC RID: 12476
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x040030BD RID: 12477
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBodyPartHit bodyPart;

	// Token: 0x040030BE RID: 12478
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceVec;

	// Token: 0x040030BF RID: 12479
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceWorldPos;

	// Token: 0x040030C0 RID: 12480
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 hipPos;

	// Token: 0x040030C1 RID: 12481
	[PublicizedFrom(EAccessModifier.Private)]
	public byte mode;

	// Token: 0x040030C2 RID: 12482
	[PublicizedFrom(EAccessModifier.Private)]
	public byte state;
}
