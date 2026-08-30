using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000514 RID: 1300
[Preserve]
public class EModelStandard : EModelBase
{
	// Token: 0x06002AC9 RID: 10953 RVA: 0x0010E910 File Offset: 0x0010CB10
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
