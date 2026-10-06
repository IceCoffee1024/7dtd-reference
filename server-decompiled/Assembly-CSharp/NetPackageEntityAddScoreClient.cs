using System;
using UnityEngine.Scripting;

// Token: 0x020007D9 RID: 2009
[Preserve]
public class NetPackageEntityAddScoreClient : NetPackage
{
	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06003B7A RID: 15226 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x0017F151 File Offset: 0x0017D351
	public NetPackageEntityAddScoreClient Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		this.entityId = _entityId;
		this.zombieKills = _zombieKills;
		this.playerKills = _playerKills;
		this.otherTeamNumber = _otherTeamNumber;
		this.conditions = _conditions;
		return this;
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x0017F179 File Offset: 0x0017D379
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.zombieKills = (int)_reader.ReadInt16();
		this.playerKills = (int)_reader.ReadInt16();
		this.otherTeamNumber = (int)_reader.ReadInt16();
		this.conditions = _reader.ReadInt32();
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x0017F1B8 File Offset: 0x0017D3B8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((short)this.zombieKills);
		_writer.Write((short)this.playerKills);
		_writer.Write((short)this.otherTeamNumber);
		_writer.Write(this.conditions);
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x0017F20C File Offset: 0x0017D40C
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

	// Token: 0x04003069 RID: 12393
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x0400306A RID: 12394
	[PublicizedFrom(EAccessModifier.Protected)]
	public int zombieKills;

	// Token: 0x0400306B RID: 12395
	[PublicizedFrom(EAccessModifier.Protected)]
	public int playerKills;

	// Token: 0x0400306C RID: 12396
	[PublicizedFrom(EAccessModifier.Protected)]
	public int otherTeamNumber;

	// Token: 0x0400306D RID: 12397
	[PublicizedFrom(EAccessModifier.Protected)]
	public int conditions;
}
