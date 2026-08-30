using System;
using UnityEngine.Scripting;

// Token: 0x02000844 RID: 2116
[Preserve]
public class NetPackageSetAttackTarget : NetPackageEntityTargeted
{
	// Token: 0x06003E05 RID: 15877 RVA: 0x00189046 File Offset: 0x00187246
	public NetPackageSetAttackTarget Setup(int entityId, int targetId)
	{
		base.Setup(entityId);
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x00189057 File Offset: 0x00187257
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003E07 RID: 15879 RVA: 0x0018906C File Offset: 0x0018726C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003E08 RID: 15880 RVA: 0x00189084 File Offset: 0x00187284
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive == null)
		{
			return;
		}
		EntityAlive entityAlive2 = _world.GetEntity(this.m_targetId) as EntityAlive;
		entityAlive.SetAttackTargetClient(entityAlive2);
	}

	// Token: 0x06003E09 RID: 15881 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003259 RID: 12889
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
