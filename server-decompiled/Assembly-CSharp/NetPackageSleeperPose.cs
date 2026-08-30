using System;
using UnityEngine.Scripting;

// Token: 0x02000855 RID: 2133
[Preserve]
public class NetPackageSleeperPose : NetPackage
{
	// Token: 0x06003E61 RID: 15969 RVA: 0x0018A398 File Offset: 0x00188598
	public NetPackageSleeperPose Setup(int targetId, byte pose)
	{
		this.m_targetId = targetId;
		this.m_pose = pose;
		return this;
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0018A3A9 File Offset: 0x001885A9
	public override void read(PooledBinaryReader _reader)
	{
		this.m_targetId = _reader.ReadInt32();
		this.m_pose = _reader.ReadByte();
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x0018A3C3 File Offset: 0x001885C3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_targetId);
		_writer.Write(this.m_pose);
	}

	// Token: 0x06003E64 RID: 15972 RVA: 0x0018A3E4 File Offset: 0x001885E4
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

	// Token: 0x06003E65 RID: 15973 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003289 RID: 12937
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_targetId;

	// Token: 0x0400328A RID: 12938
	[PublicizedFrom(EAccessModifier.Private)]
	public byte m_pose;
}
