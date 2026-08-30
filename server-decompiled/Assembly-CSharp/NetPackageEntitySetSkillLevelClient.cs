using System;
using UnityEngine.Scripting;

// Token: 0x020007D3 RID: 2003
[Preserve]
public class NetPackageEntitySetSkillLevelClient : NetPackage
{
	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06003B51 RID: 15185 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x0017EA47 File Offset: 0x0017CC47
	public NetPackageEntitySetSkillLevelClient Setup(int _entityId, string _skill, int _level)
	{
		this.entityId = _entityId;
		this.skill = _skill;
		this.level = _level;
		return this;
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x0017EA5F File Offset: 0x0017CC5F
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.skill = _reader.ReadString();
		this.level = _reader.ReadInt32();
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x0017EA85 File Offset: 0x0017CC85
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.skill);
		_writer.Write(this.level);
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x0017EAB4 File Offset: 0x0017CCB4
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

	// Token: 0x06003B56 RID: 15190 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x0400306C RID: 12396
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x0400306D RID: 12397
	[PublicizedFrom(EAccessModifier.Protected)]
	public string skill;

	// Token: 0x0400306E RID: 12398
	[PublicizedFrom(EAccessModifier.Protected)]
	public int level;
}
