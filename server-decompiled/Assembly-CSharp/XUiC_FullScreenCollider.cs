using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E85 RID: 3717
[Preserve]
public class XUiC_FullScreenCollider : XUiController
{
	// Token: 0x06007379 RID: 29561 RVA: 0x002BD9A4 File Offset: 0x002BBBA4
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_FullScreenCollider.visibleInstances.Remove(this);
	}

	// Token: 0x0600737A RID: 29562 RVA: 0x002BD9B8 File Offset: 0x002BBBB8
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

	// Token: 0x0600737B RID: 29563 RVA: 0x002BD9D6 File Offset: 0x002BBBD6
	public static bool IsBlocked(XUiView _view)
	{
		return XUiC_FullScreenCollider.visibleInstances.Count != 0 && XUiC_FullScreenCollider.visibleInstances.Max.ViewComponent.Depth > _view.Depth + 5;
	}

	// Token: 0x0400563E RID: 22078
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SortedSet<XUiC_FullScreenCollider> visibleInstances = new SortedSet<XUiC_FullScreenCollider>(XUiControllerComparerDepth.Instance);
}
