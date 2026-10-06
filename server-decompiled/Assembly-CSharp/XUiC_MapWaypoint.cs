using System;
using UnityEngine.Scripting;

// Token: 0x02000F9F RID: 3999
[Preserve]
public class XUiC_MapWaypoint : XUiController
{
	// Token: 0x06007DDB RID: 32219 RVA: 0x002FD90C File Offset: 0x002FBB0C
	public override void Init()
	{
		base.Init();
		this.waypointList = (XUiC_MapWaypointList)base.Parent.GetChildById("waypointList");
	}

	// Token: 0x06007DDC RID: 32220 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleWaypointSetPressed(XUiController _sender, EventArgs _e)
	{
	}

	// Token: 0x04005D5F RID: 23903
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}
