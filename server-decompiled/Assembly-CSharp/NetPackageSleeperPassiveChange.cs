using System;
using UnityEngine.Scripting;

// Token: 0x02000854 RID: 2132
[Preserve]
public class NetPackageSleeperPassiveChange : NetPackageEntityTargeted
{
	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06003E5C RID: 15964 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E5D RID: 15965 RVA: 0x0018A352 File Offset: 0x00188552
	public new NetPackageSleeperPassiveChange Setup(int targetId)
	{
		base.Setup(targetId);
		return this;
	}

	// Token: 0x06003E5E RID: 15966 RVA: 0x0018A35C File Offset: 0x0018855C
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

	// Token: 0x06003E5F RID: 15967 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int GetLength()
	{
		return 8;
	}
}
