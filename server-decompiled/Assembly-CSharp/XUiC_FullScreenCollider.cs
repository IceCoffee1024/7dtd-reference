using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E9F RID: 3743
[Preserve]
public class XUiC_FullScreenCollider : XUiController
{
	// Token: 0x06007396 RID: 29590 RVA: 0x002C22C0 File Offset: 0x002C04C0
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_FullScreenCollider.visibleInstances.Remove(this);
	}

	// Token: 0x06007397 RID: 29591 RVA: 0x002C22D4 File Offset: 0x002C04D4
	[XuiBindEvent("OnVisiblity", null)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void visibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			XUiC_FullScreenCollider.visibleInstances.Add(this);
			return;
		}
		XUiC_FullScreenCollider.visibleInstances.Remove(this);
	}

	// Token: 0x06007398 RID: 29592 RVA: 0x002C22F2 File Offset: 0x002C04F2
	public static bool IsBlocked(XUiView _view)
	{
		return XUiC_FullScreenCollider.visibleInstances.Count != 0 && XUiC_FullScreenCollider.visibleInstances.Max.ViewComponent.Depth > _view.Depth + 5;
	}

	// Token: 0x0400569A RID: 22170
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SortedSet<XUiC_FullScreenCollider> visibleInstances = new SortedSet<XUiC_FullScreenCollider>(XUiControllerComparerDepth.Instance);
}
