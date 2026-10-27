using System;
using UnityEngine.Scripting;

// Token: 0x0200084F RID: 2127
[Preserve]
public class NetPackageSetAttackTarget : NetPackageEntityTargeted
{
	// Token: 0x06003DD5 RID: 15829 RVA: 0x00189C82 File Offset: 0x00187E82
	public NetPackageSetAttackTarget Setup(int entityId, int targetId)
	{
		base.Setup(entityId);
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x00189C93 File Offset: 0x00187E93
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x00189CA8 File Offset: 0x00187EA8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x00189CC0 File Offset: 0x00187EC0
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

	// Token: 0x04003260 RID: 12896
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
