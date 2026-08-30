using System;
using UnityEngine.Scripting;

// Token: 0x02000849 RID: 2121
[Preserve]
public class NetPackageSleeperPose : NetPackage
{
	// Token: 0x06003DFC RID: 15868 RVA: 0x00188364 File Offset: 0x00186564
	public NetPackageSleeperPose Setup(int targetId, byte pose)
	{
		this.m_targetId = targetId;
		this.m_pose = pose;
		return this;
	}

	// Token: 0x06003DFD RID: 15869 RVA: 0x00188375 File Offset: 0x00186575
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
		this.m_pose = _reader.ReadByte();
	}

	// Token: 0x06003DFE RID: 15870 RVA: 0x0018838F File Offset: 0x0018658F
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
		_writer.Write(this.m_pose);
	}

	// Token: 0x06003DFF RID: 15871 RVA: 0x001883B0 File Offset: 0x001865B0
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
		entityAlive.TriggerSleeperPose((int)this.m_pose, false);
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x0400320A RID: 12810
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;

	// Token: 0x0400320B RID: 12811
	[PublicizedFrom(EAccessModifier.Private)]
	public byte m_pose;
}
