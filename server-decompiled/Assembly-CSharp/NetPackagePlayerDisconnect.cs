using System;
using UnityEngine.Scripting;

// Token: 0x0200082D RID: 2093
[Preserve]
public class NetPackagePlayerDisconnect : NetPackagePlayerData
{
	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x0018653B File Offset: 0x0018473B
	public new NetPackagePlayerDisconnect Setup(EntityPlayer _player)
	{
		base.Setup(_player);
		return this;
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x00186546 File Offset: 0x00184746
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		base.ProcessPackage(_world, _callbacks);
		_callbacks.PlayerDisconnected(base.Sender);
	}
}
