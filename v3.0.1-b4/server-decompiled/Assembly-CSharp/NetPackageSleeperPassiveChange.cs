using System;
using UnityEngine.Scripting;

// Token: 0x02000848 RID: 2120
[Preserve]
public class NetPackageSleeperPassiveChange : NetPackageEntityTargeted
{
	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x0018831E File Offset: 0x0018651E
	public new NetPackageSleeperPassiveChange Setup(int targetId)
	{
		base.Setup(targetId);
		return this;
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x00188328 File Offset: 0x00186528
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

	// Token: 0x06003DFA RID: 15866 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int GetLength()
	{
		return 8;
	}
}
