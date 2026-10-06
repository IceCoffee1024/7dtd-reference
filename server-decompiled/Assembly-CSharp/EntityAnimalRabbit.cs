using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000486 RID: 1158
[Preserve]
public class EntityAnimalRabbit : EntityAnimal
{
	// Token: 0x060023E4 RID: 9188 RVA: 0x000D9BE8 File Offset: 0x000D7DE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component)
		{
			component.center = new Vector3(0f, 0.15f, 0f);
			component.size = new Vector3(0.4f, 0.4f, 0.4f);
		}
		base.Awake();
		Transform transform = base.transform.Find("Graphics/BlobShadowProjector");
		if (transform)
		{
			transform.gameObject.SetActive(false);
		}
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsAttackValid()
	{
		return false;
	}
}
