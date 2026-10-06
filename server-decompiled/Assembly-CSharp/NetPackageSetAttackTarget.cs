using System;
using UnityEngine.Scripting;

// Token: 0x02000845 RID: 2117
[Preserve]
public class NetPackageSetAttackTarget : NetPackageEntityTargeted
{
	// Token: 0x06003DB2 RID: 15794 RVA: 0x00188B7E File Offset: 0x00186D7E
	public NetPackageSetAttackTarget Setup(int entityId, int targetId)
	{
		base.Setup(entityId);
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00188B8F File Offset: 0x00186D8F
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00188BA4 File Offset: 0x00186DA4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x00188BBC File Offset: 0x00186DBC
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

	// Token: 0x04003240 RID: 12864
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
