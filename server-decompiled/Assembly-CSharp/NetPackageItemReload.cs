using System;
using UnityEngine.Scripting;

// Token: 0x0200080F RID: 2063
[Preserve]
public class NetPackageItemReload : NetPackage
{
	// Token: 0x06003C85 RID: 15493 RVA: 0x00183BD4 File Offset: 0x00181DD4
	public NetPackageItemReload Setup(int _entityId)
	{
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x00183BDE File Offset: 0x00181DDE
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x00183BEC File Offset: 0x00181DEC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x00183C01 File Offset: 0x00181E01
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

	// Token: 0x0400313C RID: 12604
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
