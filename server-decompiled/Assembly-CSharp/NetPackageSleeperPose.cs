using System;
using UnityEngine.Scripting;

// Token: 0x02000862 RID: 2146
[Preserve]
public class NetPackageSleeperPose : NetPackage
{
	// Token: 0x06003E2E RID: 15918 RVA: 0x0018B2EC File Offset: 0x001894EC
	public NetPackageSleeperPose Setup(int targetId, byte pose)
	{
		this.m_targetId = targetId;
		this.m_pose = pose;
		return this;
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x0018B2FD File Offset: 0x001894FD
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
		this.m_pose = _reader.ReadByte();
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x0018B317 File Offset: 0x00189517
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
		_writer.Write(this.m_pose);
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x0018B338 File Offset: 0x00189538
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

	// Token: 0x04003294 RID: 12948
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;

	// Token: 0x04003295 RID: 12949
	[PublicizedFrom(EAccessModifier.Private)]
	public byte m_pose;
}
