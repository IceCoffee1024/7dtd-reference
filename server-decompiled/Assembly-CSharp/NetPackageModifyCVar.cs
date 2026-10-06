using System;
using UnityEngine.Scripting;

// Token: 0x020001D5 RID: 469
[Preserve]
public class NetPackageModifyCVar : NetPackage
{
	// Token: 0x06000E78 RID: 3704 RVA: 0x0005F8C6 File Offset: 0x0005DAC6
	public NetPackageModifyCVar Setup(EntityAlive entity, string _cvarName, float _value, CVarOperation _operation)
	{
		this.m_entityId = entity.entityId;
		this.cvarName = _cvarName;
		this.value = _value;
		this.operation = _operation;
		return this;
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x0005F8EB File Offset: 0x0005DAEB
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.cvarName = _reader.ReadString();
		this.value = _reader.ReadSingle();
		this.operation = (CVarOperation)_reader.ReadInt16();
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x0005F91D File Offset: 0x0005DB1D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		_writer.Write(this.cvarName);
		_writer.Write(this.value);
		_writer.Write((short)this.operation);
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x0005F958 File Offset: 0x0005DB58
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.m_entityId) as EntityAlive;
		if (entityAlive != null)
		{
			entityAlive.Buffs.SetCustomVar(this.cvarName, this.value, SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer, this.operation, false);
		}
	}

	// Token: 0x04000C57 RID: 3159
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x04000C58 RID: 3160
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;

	// Token: 0x04000C59 RID: 3161
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04000C5A RID: 3162
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;
}
