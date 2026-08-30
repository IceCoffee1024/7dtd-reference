using System;
using UnityEngine.Scripting;

// Token: 0x02000838 RID: 2104
[Preserve]
public class NetPackageSetAttackTarget : NetPackageEntityTargeted
{
	// Token: 0x06003DA0 RID: 15776 RVA: 0x00187026 File Offset: 0x00185226
	public NetPackageSetAttackTarget Setup(int entityId, int targetId)
	{
		base.Setup(entityId);
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003DA1 RID: 15777 RVA: 0x00187037 File Offset: 0x00185237
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x0018704C File Offset: 0x0018524C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x00187064 File Offset: 0x00185264
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

	// Token: 0x06003DA4 RID: 15780 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x040031DA RID: 12762
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
