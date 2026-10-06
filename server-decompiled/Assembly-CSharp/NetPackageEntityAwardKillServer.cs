using System;
using UnityEngine.Scripting;

// Token: 0x020007DE RID: 2014
[Preserve]
public class NetPackageEntityAwardKillServer : NetPackage
{
	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06003B97 RID: 15255 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x0017F6C0 File Offset: 0x0017D8C0
	public NetPackageEntityAwardKillServer Setup(int _killerEntityId, int _killedEntityId)
	{
		this.EntityId = _killerEntityId;
		this.KilledEntityId = _killedEntityId;
		return this;
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0017F6D1 File Offset: 0x0017D8D1
	public override void read(PooledBinaryReader _reader)
	{
		this.EntityId = _reader.ReadInt32();
		this.KilledEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x0017F6EB File Offset: 0x0017D8EB
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.EntityId);
		_writer.Write(this.KilledEntityId);
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x0017F70C File Offset: 0x0017D90C
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

	// Token: 0x0400307C RID: 12412
	public int EntityId;

	// Token: 0x0400307D RID: 12413
	public int KilledEntityId;
}
