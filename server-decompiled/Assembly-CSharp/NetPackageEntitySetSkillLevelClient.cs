using System;
using UnityEngine.Scripting;

// Token: 0x020007E0 RID: 2016
[Preserve]
public class NetPackageEntitySetSkillLevelClient : NetPackage
{
	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06003B8F RID: 15247 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x001800CB File Offset: 0x0017E2CB
	public NetPackageEntitySetSkillLevelClient Setup(int _entityId, string _skill, int _level)
	{
		this.entityId = _entityId;
		this.skill = _skill;
		this.level = _level;
		return this;
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x001800E3 File Offset: 0x0017E2E3
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.skill = _reader.ReadString();
		this.level = _reader.ReadInt32();
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x00180109 File Offset: 0x0017E309
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.skill);
		_writer.Write(this.level);
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x00180138 File Offset: 0x0017E338
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = (EntityPlayer)_world.GetEntity(this.entityId);
		if (entityPlayer != null)
		{
			entityPlayer.Progression.GetProgressionValue(this.skill).Level = this.level;
		}
	}

	// Token: 0x04003086 RID: 12422
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04003087 RID: 12423
	[PublicizedFrom(EAccessModifier.Protected)]
	public string skill;

	// Token: 0x04003088 RID: 12424
	[PublicizedFrom(EAccessModifier.Protected)]
	public int level;
}
