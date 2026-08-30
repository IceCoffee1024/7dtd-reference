using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D8 RID: 2008
[Preserve]
public class NetPackageEntityRagdoll : NetPackage
{
	// Token: 0x06003B53 RID: 15187 RVA: 0x0017D7B5 File Offset: 0x0017B9B5
	public NetPackageEntityRagdoll Setup(Entity _entity, byte _state)
	{
		this.entityId = _entity.entityId;
		this.flags = 4;
		this.state = _state;
		return this;
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x0017D7D4 File Offset: 0x0017B9D4
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

	// Token: 0x06003B55 RID: 15189 RVA: 0x0017D840 File Offset: 0x0017BA40
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

	// Token: 0x06003B56 RID: 15190 RVA: 0x0017D8E8 File Offset: 0x0017BAE8
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

	// Token: 0x06003B57 RID: 15191 RVA: 0x0017D98C File Offset: 0x0017BB8C
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

	// Token: 0x06003B58 RID: 15192 RVA: 0x0017DA34 File Offset: 0x0017BC34
	public override int GetLength()
	{
		return 48;
	}

	// Token: 0x0400301F RID: 12319
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003020 RID: 12320
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsForce = 1;

	// Token: 0x04003021 RID: 12321
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsMode = 2;

	// Token: 0x04003022 RID: 12322
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cFlagsState = 4;

	// Token: 0x04003023 RID: 12323
	[PublicizedFrom(EAccessModifier.Private)]
	public byte flags;

	// Token: 0x04003024 RID: 12324
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x04003025 RID: 12325
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBodyPartHit bodyPart;

	// Token: 0x04003026 RID: 12326
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceVec;

	// Token: 0x04003027 RID: 12327
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 forceWorldPos;

	// Token: 0x04003028 RID: 12328
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 hipPos;

	// Token: 0x04003029 RID: 12329
	[PublicizedFrom(EAccessModifier.Private)]
	public byte mode;

	// Token: 0x0400302A RID: 12330
	[PublicizedFrom(EAccessModifier.Private)]
	public byte state;
}
