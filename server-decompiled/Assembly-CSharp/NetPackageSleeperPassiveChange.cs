using System;
using UnityEngine.Scripting;

// Token: 0x02000857 RID: 2135
[Preserve]
public class NetPackageSleeperPassiveChange : NetPackageEntityTargeted
{
	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06003E07 RID: 15879 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E08 RID: 15880 RVA: 0x0018A1FB File Offset: 0x001883FB
	public new NetPackageSleeperPassiveChange Setup(int targetId)
	{
		base.Setup(targetId);
		return this;
	}

	// Token: 0x06003E09 RID: 15881 RVA: 0x0018A208 File Offset: 0x00188408
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null || !_world.IsRemote())
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive == null)
		{
			return;
		}
		entityAlive.IsSleeperPassive = false;
	}
}
