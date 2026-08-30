using System;
using UnityEngine.Scripting;

// Token: 0x020007C8 RID: 1992
[Preserve]
public class NetPackageEntitySetSkillLevelClient : NetPackage
{
	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06003AF2 RID: 15090 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x0017C66F File Offset: 0x0017A86F
	public NetPackageEntitySetSkillLevelClient Setup(int _entityId, string _skill, int _level)
	{
		this.entityId = _entityId;
		this.skill = _skill;
		this.level = _level;
		return this;
	}

	// Token: 0x06003AF4 RID: 15092 RVA: 0x0017C687 File Offset: 0x0017A887
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.skill = _reader.ReadString();
		this.level = _reader.ReadInt32();
	}

	// Token: 0x06003AF5 RID: 15093 RVA: 0x0017C6AD File Offset: 0x0017A8AD
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write(this.skill);
		_writer.Write(this.level);
	}

	// Token: 0x06003AF6 RID: 15094 RVA: 0x0017C6DC File Offset: 0x0017A8DC
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

	// Token: 0x06003AF7 RID: 15095 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04002FED RID: 12269
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04002FEE RID: 12270
	[PublicizedFrom(EAccessModifier.Protected)]
	public string skill;

	// Token: 0x04002FEF RID: 12271
	[PublicizedFrom(EAccessModifier.Protected)]
	public int level;
}
