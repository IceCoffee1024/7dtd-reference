using System;
using UnityEngine.Scripting;

// Token: 0x02000823 RID: 2083
[Preserve]
public class NetPackagePlayerDisconnect : NetPackagePlayerData
{
	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06003D00 RID: 15616 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x00185437 File Offset: 0x00183637
	public new NetPackagePlayerDisconnect Setup(EntityPlayer _player)
	{
		base.Setup(_player);
		return this;
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x00185442 File Offset: 0x00183642
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		base.ProcessPackage(_world, _callbacks);
		_callbacks.PlayerDisconnected(base.Sender);
	}
}
