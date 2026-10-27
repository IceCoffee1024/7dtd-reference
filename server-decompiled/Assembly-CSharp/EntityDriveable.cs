using System;
using UnityEngine.Scripting;

// Token: 0x020004A0 RID: 1184
[Preserve]
public class EntityDriveable : EntityVehicle
{
	// Token: 0x0600245B RID: 9307 RVA: 0x000DEFE3 File Offset: 0x000DD1E3
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		if (this.nativeCollider != null)
		{
			this.nativeCollider.enabled = true;
		}
	}
}
