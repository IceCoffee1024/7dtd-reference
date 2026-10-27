using System;
using UnityEngine.Scripting;

// Token: 0x020001D7 RID: 471
[Preserve]
public class NetPackageModifyCVar : NetPackage
{
	// Token: 0x06000E7C RID: 3708 RVA: 0x0005FB8A File Offset: 0x0005DD8A
	public NetPackageModifyCVar Setup(EntityAlive entity, string _cvarName, float _value, CVarOperation _operation)
	{
		this.m_entityId = entity.entityId;
		this.cvarName = _cvarName;
		this.value = _value;
		this.operation = _operation;
		return this;
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0005FBAF File Offset: 0x0005DDAF
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.cvarName = _reader.ReadString();
		this.value = _reader.ReadSingle();
		this.operation = (CVarOperation)_reader.ReadInt16();
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0005FBE1 File Offset: 0x0005DDE1
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		_writer.Write(this.cvarName);
		_writer.Write(this.value);
		_writer.Write((short)this.operation);
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x0005FC1C File Offset: 0x0005DE1C
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

	// Token: 0x04000C59 RID: 3161
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x04000C5A RID: 3162
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;

	// Token: 0x04000C5B RID: 3163
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04000C5C RID: 3164
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;
}
