using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200048A RID: 1162
[Preserve]
public class EntityAnimalStag : EntityAnimal
{
	// Token: 0x060023EF RID: 9199 RVA: 0x000DA028 File Offset: 0x000D8228
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
