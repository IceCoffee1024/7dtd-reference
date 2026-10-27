using System;
using UnityEngine.Scripting;

// Token: 0x02000863 RID: 2147
[Preserve]
public class NetPackageSleeperWakeup : NetPackage
{
	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06003E33 RID: 15923 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x0018B37A File Offset: 0x0018957A
	public NetPackageSleeperWakeup Setup(int targetId)
	{
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x0018B384 File Offset: 0x00189584
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x0018B392 File Offset: 0x00189592
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x0018B3A8 File Offset: 0x001895A8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null || !_world.IsRemote())
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.m_targetId) as EntityAlive;
		if (entityAlive == null)
		{
			return;
		}
		entityAlive.ConditionalTriggerSleeperWakeUp();
	}

	// Token: 0x04003296 RID: 12950
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
