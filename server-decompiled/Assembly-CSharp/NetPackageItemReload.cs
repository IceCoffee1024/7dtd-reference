using System;
using UnityEngine.Scripting;

// Token: 0x02000806 RID: 2054
[Preserve]
public class NetPackageItemReload : NetPackage
{
	// Token: 0x06003C88 RID: 15496 RVA: 0x00182A78 File Offset: 0x00180C78
	public NetPackageItemReload Setup(int _entityId)
	{
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x00182A82 File Offset: 0x00180C82
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x00182A90 File Offset: 0x00180C90
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00182AA5 File Offset: 0x00180CA5
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			_world.GetGameManager().ItemReloadServer(this.entityId);
			return;
		}
		_world.GetGameManager().ItemReloadClient(this.entityId);
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x0400312E RID: 12590
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
