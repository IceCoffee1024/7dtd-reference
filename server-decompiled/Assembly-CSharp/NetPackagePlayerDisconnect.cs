using System;
using UnityEngine.Scripting;

// Token: 0x02000825 RID: 2085
[Preserve]
public class NetPackagePlayerDisconnect : NetPackagePlayerData
{
	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06003D42 RID: 15682 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D43 RID: 15683 RVA: 0x0018586F File Offset: 0x00183A6F
	public new NetPackagePlayerDisconnect Setup(EntityPlayer _player)
	{
		base.Setup(_player);
		return this;
	}

	// Token: 0x06003D44 RID: 15684 RVA: 0x0018587A File Offset: 0x00183A7A
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		base.ProcessPackage(_world, _callbacks);
		_callbacks.PlayerDisconnected(base.Sender);
	}
}
