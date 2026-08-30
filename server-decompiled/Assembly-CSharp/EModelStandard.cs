using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000519 RID: 1305
[Preserve]
public class EModelStandard : EModelBase
{
	// Token: 0x06002AF3 RID: 10995 RVA: 0x0010F94C File Offset: 0x0010DB4C
	public override void PostInit()
	{
		base.PostInit();
		Transform modelTransform = base.GetModelTransform();
		if (modelTransform)
		{
			base.SetColliderLayers(modelTransform, 0);
		}
	}
}
