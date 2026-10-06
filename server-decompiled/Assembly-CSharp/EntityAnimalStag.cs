using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000488 RID: 1160
[Preserve]
public class EntityAnimalStag : EntityAnimal
{
	// Token: 0x060023E9 RID: 9193 RVA: 0x000D9CA4 File Offset: 0x000D7EA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component)
		{
			component.center = new Vector3(0f, 0.85f, 0f);
			component.size = new Vector3(0.8f, 1.6f, 0.8f);
		}
		base.Awake();
	}
}
