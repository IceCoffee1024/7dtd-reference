using System;
using UnityEngine.Scripting;

// Token: 0x0200084A RID: 2122
[Preserve]
public class NetPackageSleeperWakeup : NetPackage
{
	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06003E02 RID: 15874 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x001883F2 File Offset: 0x001865F2
	public NetPackageSleeperWakeup Setup(int targetId)
	{
		this.m_targetId = targetId;
		return this;
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x001883FC File Offset: 0x001865FC
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x0018840A File Offset: 0x0018660A
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x00188420 File Offset: 0x00186620
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

	// Token: 0x06003E07 RID: 15879 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x0400320C RID: 12812
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;
}
