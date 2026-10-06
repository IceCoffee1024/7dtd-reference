using System;
using UnityEngine.Scripting;

// Token: 0x020007D6 RID: 2006
[Preserve]
public class NetPackageEntitySetSkillLevelClient : NetPackage
{
	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x06003B6C RID: 15212 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x0017EFC7 File Offset: 0x0017D1C7
	public NetPackageEntitySetSkillLevelClient Setup(int _entityId, string _skill, int _level)
	{
		this.entityId = _entityId;
		this.skill = _skill;
		this.level = _level;
		return this;
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x0017EFDF File Offset: 0x0017D1DF
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.skill = _reader.ReadString();
		this.level = _reader.ReadInt32();
	}

	// Token: 0x06003B6F RID: 15215 RVA: 0x0017F005 File Offset: 0x0017D205
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.skill);
		_writer.Write(this.level);
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x0017F034 File Offset: 0x0017D234
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

	// Token: 0x04003066 RID: 12390
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04003067 RID: 12391
	[PublicizedFrom(EAccessModifier.Protected)]
	public string skill;

	// Token: 0x04003068 RID: 12392
	[PublicizedFrom(EAccessModifier.Protected)]
	public int level;
}
