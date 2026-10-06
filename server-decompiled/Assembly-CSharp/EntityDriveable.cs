using System;
using UnityEngine.Scripting;

// Token: 0x0200049E RID: 1182
[Preserve]
public class EntityDriveable : EntityVehicle
{
	// Token: 0x06002455 RID: 9301 RVA: 0x000DEC5F File Offset: 0x000DCE5F
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		if (this.nativeCollider != null)
		{
			this.nativeCollider.enabled = true;
		}
	}
}
