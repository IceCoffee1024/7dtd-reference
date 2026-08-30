using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E3 RID: 2019
[Preserve]
public class NetPackageEntityRagdoll : NetPackage
{
	// Token: 0x06003BB2 RID: 15282 RVA: 0x0017FB8D File Offset: 0x0017DD8D
	public NetPackageEntityRagdoll Setup(Entity _entity, byte _state)
	{
		this.entityId = _entity.entityId;
		this.flags = 4;
		this.state = _state;
		return this;
	}

	// Token: 0x06003BB3 RID: 15283 RVA: 0x0017FBAC File Offset: 0x0017DDAC
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

	// Token: 0x06003BB4 RID: 15284 RVA: 0x0017FC18 File Offset: 0x0017DE18
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

	// Token: 0x06003BB5 RID: 15285 RVA: 0x0017FCC0 File Offset: 0x0017DEC0
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

	// Token: 0x06003BB6 RID: 15286 RVA: 0x0017FD64 File Offset: 0x0017DF64
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

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0017FE0C File Offset: 0x0017E00C
	public override int GetLength()
	{
		return 48;
	}

	// Token: 0x0400309E RID: 12446
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400309F RID: 12447
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsForce = 1;

	// Token: 0x040030A0 RID: 12448
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsMode = 2;

	// Token: 0x040030A1 RID: 12449
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsState = 4;

	// Token: 0x040030A2 RID: 12450
	[PublicizedFrom(EAccessModifier.Private)]
	public byte flags;

	// Token: 0x040030A3 RID: 12451
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x040030A4 RID: 12452
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBodyPartHit bodyPart;

	// Token: 0x040030A5 RID: 12453
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceVec;

	// Token: 0x040030A6 RID: 12454
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceWorldPos;

	// Token: 0x040030A7 RID: 12455
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 hipPos;

	// Token: 0x040030A8 RID: 12456
	[PublicizedFrom(EAccessModifier.Private)]
	public byte mode;

	// Token: 0x040030A9 RID: 12457
	[PublicizedFrom(EAccessModifier.Private)]
	public byte state;
}
