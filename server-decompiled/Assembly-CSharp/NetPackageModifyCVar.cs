using System;
using UnityEngine.Scripting;

// Token: 0x020001D4 RID: 468
[Preserve]
public class NetPackageModifyCVar : NetPackage
{
	// Token: 0x06000E84 RID: 3716 RVA: 0x0005F8D6 File Offset: 0x0005DAD6
	public NetPackageModifyCVar Setup(EntityAlive entity, string _cvarName, float _value, CVarOperation _operation)
	{
		this.m_entityId = entity.entityId;
		this.cvarName = _cvarName;
		this.value = _value;
		this.operation = _operation;
		return this;
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x0005F8FB File Offset: 0x0005DAFB
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.cvarName = _reader.ReadString();
		this.value = _reader.ReadSingle();
		this.operation = (CVarOperation)_reader.ReadInt16();
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x0005F92D File Offset: 0x0005DB2D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		_writer.Write(this.cvarName);
		_writer.Write(this.value);
		_writer.Write((short)this.operation);
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0005F968 File Offset: 0x0005DB68
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

	// Token: 0x06000E88 RID: 3720 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04000C5E RID: 3166
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x04000C5F RID: 3167
	[PublicizedFrom(EAccessModifier.Private)]
	public string cvarName;

	// Token: 0x04000C60 RID: 3168
	[PublicizedFrom(EAccessModifier.Private)]
	public float value;

	// Token: 0x04000C61 RID: 3169
	[PublicizedFrom(EAccessModifier.Private)]
	public CVarOperation operation;
}
