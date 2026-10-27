using System;
using UnityEngine.Scripting;

// Token: 0x02000FAF RID: 4015
[Preserve]
public class XUiC_MapWaypoint : XUiController
{
	// Token: 0x06007E2A RID: 32298 RVA: 0x002FF428 File Offset: 0x002FD628
	public override void Init()
	{
		base.Init();
		this.waypointList = (XUiC_MapWaypointList)base.Parent.GetChildById("waypointList");
	}

	// Token: 0x06007E2B RID: 32299 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleWaypointSetPressed(XUiController _sender, EventArgs _e)
	{
	}

	// Token: 0x04005D9A RID: 23962
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}
