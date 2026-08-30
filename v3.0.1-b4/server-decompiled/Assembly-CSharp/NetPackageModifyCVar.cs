using System;
using UnityEngine.Scripting;

// Token: 0x020001D2 RID: 466
[Preserve]
public class NetPackageModifyCVar : NetPackage
{
	// Token: 0x06000E79 RID: 3705 RVA: 0x0005F38E File Offset: 0x0005D58E
	public NetPackageModifyCVar Setup(EntityAlive entity, string _cvarName, float _value, CVarOperation _operation)
	{
		this.m_entityId = entity.entityId;
		this.cvarName = _cvarName;
		this.value = _value;
		this.operation = _operation;
		return this;
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0005F3B3 File Offset: 0x0005D5B3
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.cvarName = _reader.ReadString();
		this.value = _reader.ReadSingle();
		this.operation = (CVarOperation)_reader.ReadInt16();
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0005F3E5 File Offset: 0x0005D5E5
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		_writer.Write(this.cvarName);
		_writer.Write(this.value);
		_writer.Write((short)this.operation);
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x0005F420 File Offset: 0x0005D620
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.m_entityId) as EntityAlive;
		if (entityAlive != null)
		{
			entityAlive.Buffs.SetCustomVar(this.cvarName, this.value, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer, this.operation);
		}
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04000C51 RID: 3153
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x04000C52 RID: 3154
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;

	// Token: 0x04000C53 RID: 3155
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04000C54 RID: 3156
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;
}
