using System;
using UnityEngine.Scripting;

// Token: 0x02000805 RID: 2053
[Preserve]
public class NetPackageItemReload : NetPackage
{
	// Token: 0x06003C62 RID: 15458 RVA: 0x00182AD0 File Offset: 0x00180CD0
	public NetPackageItemReload Setup(int _entityId)
	{
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00182ADA File Offset: 0x00180CDA
	public override void read(PooledBinaryReader _br)
	{
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00182AE8 File Offset: 0x00180CE8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003C65 RID: 15461 RVA: 0x00182AFD File Offset: 0x00180CFD
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

	// Token: 0x0400311C RID: 12572
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
