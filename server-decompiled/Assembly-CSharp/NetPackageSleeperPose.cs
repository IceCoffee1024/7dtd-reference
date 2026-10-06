using System;
using UnityEngine.Scripting;

// Token: 0x02000858 RID: 2136
[Preserve]
public class NetPackageSleeperPose : NetPackage
{
	// Token: 0x06003E0B RID: 15883 RVA: 0x0018A244 File Offset: 0x00188444
	public NetPackageSleeperPose Setup(int targetId, byte pose)
	{
		this.m_targetId = targetId;
		this.m_pose = pose;
		return this;
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x0018A255 File Offset: 0x00188455
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
		this.m_pose = _reader.ReadByte();
	}

	// Token: 0x06003E0D RID: 15885 RVA: 0x0018A26F File Offset: 0x0018846F
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
		_writer.Write(this.m_pose);
	}

	// Token: 0x06003E0E RID: 15886 RVA: 0x0018A290 File Offset: 0x00188490
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

	// Token: 0x04003274 RID: 12916
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;

	// Token: 0x04003275 RID: 12917
	[PublicizedFrom(EAccessModifier.Private)]
	public byte m_pose;
}
