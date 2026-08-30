using System;
using UnityEngine.Scripting;

// Token: 0x02000497 RID: 1175
[Preserve]
public class EntityDriveable : EntityVehicle
{
	// Token: 0x06002450 RID: 9296 RVA: 0x000DE8AD File Offset: 0x000DCAAD
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		if (this.nativeCollider != null)
		{
			this.nativeCollider.enabled = true;
		}
	}
}
