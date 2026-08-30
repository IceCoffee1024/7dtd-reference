using System;
using UnityEngine.Scripting;

// Token: 0x02000856 RID: 2134
[Preserve]
public class NetPackageSleeperWakeup : NetPackage
{
	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06003E67 RID: 15975 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E68 RID: 15976 RVA: 0x0018A426 File Offset: 0x00188626
	public NetPackageSleeperWakeup Setup(int targetId)
	{
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003E69 RID: 15977 RVA: 0x0018A430 File Offset: 0x00188630
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003E6A RID: 15978 RVA: 0x0018A43E File Offset: 0x0018863E
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003E6B RID: 15979 RVA: 0x0018A454 File Offset: 0x00188654
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

	// Token: 0x06003E6C RID: 15980 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x0400328B RID: 12939
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
