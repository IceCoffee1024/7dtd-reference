using System;
using UnityEngine.Scripting;

// Token: 0x020007D6 RID: 2006
[Preserve]
public class NetPackageEntityAddScoreClient : NetPackage
{
	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06003B60 RID: 15200 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x0017EBD1 File Offset: 0x0017CDD1
	public NetPackageEntityAddScoreClient Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		this.entityId = _entityId;
		this.zombieKills = _zombieKills;
		this.playerKills = _playerKills;
		this.otherTeamNumber = _otherTeamNumber;
		this.conditions = _conditions;
		return this;
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x0017EBF9 File Offset: 0x0017CDF9
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.zombieKills = (int)_reader.ReadInt16();
		this.playerKills = (int)_reader.ReadInt16();
		this.otherTeamNumber = (int)_reader.ReadInt16();
		this.conditions = _reader.ReadInt32();
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x0017EC38 File Offset: 0x0017CE38
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((short)this.zombieKills);
		_writer.Write((short)this.playerKills);
		_writer.Write((short)this.otherTeamNumber);
		_writer.Write(this.conditions);
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x0017EC8C File Offset: 0x0017CE8C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = (EntityAlive)_world.GetEntity(this.entityId);
		if (entityAlive != null)
		{
			entityAlive.AddScore(0, this.zombieKills, this.playerKills, this.otherTeamNumber, this.conditions);
		}
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400306F RID: 12399
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04003070 RID: 12400
	[PublicizedFrom(EAccessModifier.Protected)]
	public int zombieKills;

	// Token: 0x04003071 RID: 12401
	[PublicizedFrom(EAccessModifier.Protected)]
	public int playerKills;

	// Token: 0x04003072 RID: 12402
	[PublicizedFrom(EAccessModifier.Protected)]
	public int otherTeamNumber;

	// Token: 0x04003073 RID: 12403
	[PublicizedFrom(EAccessModifier.Protected)]
	public int conditions;
}
