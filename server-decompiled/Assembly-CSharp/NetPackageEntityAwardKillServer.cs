using System;
using UnityEngine.Scripting;

// Token: 0x020007DB RID: 2011
[Preserve]
public class NetPackageEntityAwardKillServer : NetPackage
{
	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06003B81 RID: 15233 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x0017F17C File Offset: 0x0017D37C
	public NetPackageEntityAwardKillServer Setup(int _killerEntityId, int _killedEntityId)
	{
		this.EntityId = _killerEntityId;
		this.KilledEntityId = _killedEntityId;
		return this;
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x0017F18D File Offset: 0x0017D38D
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.KilledEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x0017F1A7 File Offset: 0x0017D3A7
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write(this.KilledEntityId);
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0017F1C8 File Offset: 0x0017D3C8
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

	// Token: 0x06003B86 RID: 15238 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x04003083 RID: 12419
	public int EntityId;

	// Token: 0x04003084 RID: 12420
	public int KilledEntityId;
}
