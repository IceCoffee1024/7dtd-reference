using System;
using UnityEngine.Scripting;

// Token: 0x0200081A RID: 2074
[Preserve]
public class NetPackagePlayerDisconnect : NetPackagePlayerData
{
	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x06003CE3 RID: 15587 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x00183497 File Offset: 0x00181697
	public new NetPackagePlayerDisconnect Setup(EntityPlayer _player)
	{
		base.Setup(_player);
		return this;
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x001834A2 File Offset: 0x001816A2
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		base.ProcessPackage(_world, _callbacks);
		_callbacks.PlayerDisconnected(base.Sender);
	}
}
