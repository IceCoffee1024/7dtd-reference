using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000482 RID: 1154
[Preserve]
public class EntityAnimalStag : EntityAnimal
{
	// Token: 0x060023D5 RID: 9173 RVA: 0x000D90B0 File Offset: 0x000D72B0
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
