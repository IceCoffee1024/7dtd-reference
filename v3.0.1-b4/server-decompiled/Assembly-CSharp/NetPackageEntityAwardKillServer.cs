using System;
using UnityEngine.Scripting;

// Token: 0x020007D0 RID: 2000
[Preserve]
public class NetPackageEntityAwardKillServer : NetPackage
{
	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06003B22 RID: 15138 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x0017CDA4 File Offset: 0x0017AFA4
	public NetPackageEntityAwardKillServer Setup(int _killerEntityId, int _killedEntityId)
	{
		this.EntityId = _killerEntityId;
		this.KilledEntityId = _killedEntityId;
		return this;
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x0017CDB5 File Offset: 0x0017AFB5
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.KilledEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003B25 RID: 15141 RVA: 0x0017CDCF File Offset: 0x0017AFCF
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write(this.KilledEntityId);
	}

	// Token: 0x06003B26 RID: 15142 RVA: 0x0017CDF0 File Offset: 0x0017AFF0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = _world.GetEntity(this.EntityId) as EntityPlayerLocal;
		if (entityPlayerLocal != null)
		{
			EntityAlive entityAlive = _world.GetEntity(this.KilledEntityId) as EntityAlive;
			if (entityAlive != null)
			{
				QuestEventManager.Current.EntityKilled(entityPlayerLocal, entityAlive);
			}
		}
	}

	// Token: 0x06003B27 RID: 15143 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003004 RID: 12292
	public int EntityId;

	// Token: 0x04003005 RID: 12293
	public int KilledEntityId;
}
