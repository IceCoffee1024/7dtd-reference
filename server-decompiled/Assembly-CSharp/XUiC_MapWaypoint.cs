using System;
using UnityEngine.Scripting;

// Token: 0x02000F90 RID: 3984
[Preserve]
public class XUiC_MapWaypoint : XUiController
{
	// Token: 0x06007DCB RID: 32203 RVA: 0x002FA868 File Offset: 0x002F8A68
	public override void Init()
	{
		base.Init();
		this.waypointList = (XUiC_MapWaypointList)base.Parent.GetChildById("waypointList");
	}

	// Token: 0x06007DCC RID: 32204 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleWaypointSetPressed(XUiController _sender, EventArgs _e)
	{
	}

	// Token: 0x04005D36 RID: 23862
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}
