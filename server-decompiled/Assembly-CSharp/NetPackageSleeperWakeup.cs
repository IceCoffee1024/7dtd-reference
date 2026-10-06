using System;
using UnityEngine.Scripting;

// Token: 0x02000859 RID: 2137
[Preserve]
public class NetPackageSleeperWakeup : NetPackage
{
	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06003E10 RID: 15888 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E11 RID: 15889 RVA: 0x0018A2D2 File Offset: 0x001884D2
	public NetPackageSleeperWakeup Setup(int targetId)
	{
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003E12 RID: 15890 RVA: 0x0018A2DC File Offset: 0x001884DC
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003E13 RID: 15891 RVA: 0x0018A2EA File Offset: 0x001884EA
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003E14 RID: 15892 RVA: 0x0018A300 File Offset: 0x00188500
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

	// Token: 0x04003276 RID: 12918
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
