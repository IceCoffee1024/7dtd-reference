using System;
using UnityEngine.Scripting;

// Token: 0x020007CB RID: 1995
[Preserve]
public class NetPackageEntityAddScoreClient : NetPackage
{
	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06003B01 RID: 15105 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B02 RID: 15106 RVA: 0x0017C7F9 File Offset: 0x0017A9F9
	public NetPackageEntityAddScoreClient Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		this.entityId = _entityId;
		this.zombieKills = _zombieKills;
		this.playerKills = _playerKills;
		this.otherTeamNumber = _otherTeamNumber;
		this.conditions = _conditions;
		return this;
	}

	// Token: 0x06003B03 RID: 15107 RVA: 0x0017C821 File Offset: 0x0017AA21
	public override void read(PooledBinaryReader _reader)
	{
		this.entityId = _reader.ReadInt32();
		this.zombieKills = (int)_reader.ReadInt16();
		this.playerKills = (int)_reader.ReadInt16();
		this.otherTeamNumber = (int)_reader.ReadInt16();
		this.conditions = _reader.ReadInt32();
	}

	// Token: 0x06003B04 RID: 15108 RVA: 0x0017C860 File Offset: 0x0017AA60
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityId);
		_writer.Write((short)this.zombieKills);
		_writer.Write((short)this.playerKills);
		_writer.Write((short)this.otherTeamNumber);
		_writer.Write(this.conditions);
	}

	// Token: 0x06003B05 RID: 15109 RVA: 0x0017C8B4 File Offset: 0x0017AAB4
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

	// Token: 0x06003B06 RID: 15110 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04002FF0 RID: 12272
	[PublicizedFrom(EAccessModifier.Protected)]
	public int entityId;

	// Token: 0x04002FF1 RID: 12273
	[PublicizedFrom(EAccessModifier.Protected)]
	public int zombieKills;

	// Token: 0x04002FF2 RID: 12274
	[PublicizedFrom(EAccessModifier.Protected)]
	public int playerKills;

	// Token: 0x04002FF3 RID: 12275
	[PublicizedFrom(EAccessModifier.Protected)]
	public int otherTeamNumber;

	// Token: 0x04002FF4 RID: 12276
	[PublicizedFrom(EAccessModifier.Protected)]
	public int conditions;
}
