using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E94 RID: 3732
[Preserve]
public class XUiC_FullScreenCollider : XUiController
{
	// Token: 0x06007371 RID: 29553 RVA: 0x002C0D54 File Offset: 0x002BEF54
	public override void Cleanup()
	{
		base.Cleanup();
		XUiC_FullScreenCollider.visibleInstances.Remove(this);
	}

	// Token: 0x06007372 RID: 29554 RVA: 0x002C0D68 File Offset: 0x002BEF68
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

	// Token: 0x06007373 RID: 29555 RVA: 0x002C0D86 File Offset: 0x002BEF86
	public static bool IsBlocked(XUiView _view)
	{
		return XUiC_FullScreenCollider.visibleInstances.Count != 0 && XUiC_FullScreenCollider.visibleInstances.Max.ViewComponent.Depth > _view.Depth + 5;
	}

	// Token: 0x04005675 RID: 22133
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly SortedSet<XUiC_FullScreenCollider> visibleInstances = new SortedSet<XUiC_FullScreenCollider>(XUiControllerComparerDepth.Instance);
}
