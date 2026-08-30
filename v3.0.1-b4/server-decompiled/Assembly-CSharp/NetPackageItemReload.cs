using System;
using UnityEngine.Scripting;

// Token: 0x020007FB RID: 2043
[Preserve]
public class NetPackageItemReload : NetPackage
{
	// Token: 0x06003C29 RID: 15401 RVA: 0x001806A0 File Offset: 0x0017E8A0
	public NetPackageItemReload Setup(int _entityId)
	{
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x001806AA File Offset: 0x0017E8AA
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x001806B8 File Offset: 0x0017E8B8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x001806CD File Offset: 0x0017E8CD
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

	// Token: 0x06003C2D RID: 15405 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}

	// Token: 0x040030AF RID: 12463
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
