using System;
using UnityEngine.Scripting;

// Token: 0x02000861 RID: 2145
[Preserve]
public class NetPackageSleeperPassiveChange : NetPackageEntityTargeted
{
	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06003E2A RID: 15914 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x0018B2A3 File Offset: 0x001894A3
	public new NetPackageSleeperPassiveChange Setup(int targetId)
	{
		base.Setup(targetId);
		return this;
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x0018B2B0 File Offset: 0x001894B0
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
