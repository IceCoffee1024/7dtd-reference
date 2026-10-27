using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000521 RID: 1313
[Preserve]
public class EModelStandard : EModelBase
{
	// Token: 0x06002AEF RID: 10991 RVA: 0x00110784 File Offset: 0x0010E984
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
