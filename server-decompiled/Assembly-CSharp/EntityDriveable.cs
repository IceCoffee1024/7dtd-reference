using System;
using UnityEngine.Scripting;

// Token: 0x02000493 RID: 1171
[Preserve]
public class EntityDriveable : EntityVehicle
{
	// Token: 0x0600242E RID: 9262 RVA: 0x000DDC25 File Offset: 0x000DBE25
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		if (this.nativeCollider != null)
		{
			this.nativeCollider.enabled = true;
		}
	}
}
