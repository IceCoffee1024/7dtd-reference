using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E70 RID: 3696
[Preserve]
public class XUiC_FullScreenCollider : XUiController
{
	// Token: 0x060072D9 RID: 29401 RVA: 0x002BA480 File Offset: 0x002B8680
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_FullScreenCollider.visibleInstances.Remove(this);
	}

	// Token: 0x060072DA RID: 29402 RVA: 0x002BA494 File Offset: 0x002B8694
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

	// Token: 0x060072DB RID: 29403 RVA: 0x002BA4B2 File Offset: 0x002B86B2
	public static bool IsBlocked(XUiView _view)
	{
		return XUiC_FullScreenCollider.visibleInstances.Count != 0 && XUiC_FullScreenCollider.visibleInstances.Max.ViewComponent.Depth > _view.Depth + 5;
	}

	// Token: 0x0400559A RID: 21914
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SortedSet<XUiC_FullScreenCollider> visibleInstances = new SortedSet<XUiC_FullScreenCollider>(XUiControllerComparerDepth.Instance);
}
