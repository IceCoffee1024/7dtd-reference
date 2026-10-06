using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200051F RID: 1311
[Preserve]
public class EModelStandard : EModelBase
{
	// Token: 0x06002AE8 RID: 10984 RVA: 0x00110210 File Offset: 0x0010E410
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
