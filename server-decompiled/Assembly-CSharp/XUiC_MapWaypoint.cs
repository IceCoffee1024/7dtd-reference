using System;
using UnityEngine.Scripting;

// Token: 0x02000F77 RID: 3959
[Preserve]
public class XUiC_MapWaypoint : XUiController
{
	// Token: 0x06007CD9 RID: 31961 RVA: 0x002F6094 File Offset: 0x002F4294
	public override void Init()
	{
		base.Init();
		this.waypointList = (XUiC_MapWaypointList)base.Parent.GetChildById("waypointList");
	}

	// Token: 0x06007CDA RID: 31962 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleWaypointSetPressed(XUiController _sender, EventArgs _e)
	{
	}

	// Token: 0x04005C6F RID: 23663
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_MapWaypointList waypointList;
}
