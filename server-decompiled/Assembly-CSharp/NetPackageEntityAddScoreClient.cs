using System;
using UnityEngine.Scripting;

// Token: 0x020007E3 RID: 2019
[Preserve]
public class NetPackageEntityAddScoreClient : NetPackage
{
	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x06003B9D RID: 15261 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x00180255 File Offset: 0x0017E455
	public NetPackageEntityAddScoreClient Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		this.entityId = _entityId;
		this.zombieKills = _zombieKills;
		this.playerKills = _playerKills;
		this.otherTeamNumber = _otherTeamNumber;
		this.conditions = _conditions;
		return this;
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0018027D File Offset: 0x0017E47D
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.zombieKills = (int)_reader.ReadInt16();
		this.playerKills = (int)_reader.ReadInt16();
		this.otherTeamNumber = (int)_reader.ReadInt16();
		this.conditions = _reader.ReadInt32();
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x001802BC File Offset: 0x0017E4BC
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((short)this.zombieKills);
		_writer.Write((short)this.playerKills);
		_writer.Write((short)this.otherTeamNumber);
		_writer.Write(this.conditions);
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x00180310 File Offset: 0x0017E510
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

	// Token: 0x04003089 RID: 12425
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x0400308A RID: 12426
	[PublicizedFrom(EAccessModifier.Protected)]
	public int zombieKills;

	// Token: 0x0400308B RID: 12427
	[PublicizedFrom(EAccessModifier.Protected)]
	public int playerKills;

	// Token: 0x0400308C RID: 12428
	[PublicizedFrom(EAccessModifier.Protected)]
	public int otherTeamNumber;

	// Token: 0x0400308D RID: 12429
	[PublicizedFrom(EAccessModifier.Protected)]
	public int conditions;
}
