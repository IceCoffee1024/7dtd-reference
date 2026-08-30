using System;
using UnityEngine.Scripting;

// Token: 0x02000F2E RID: 3886
[Preserve]
public class XUiC_InGameHUD : XUiController
{
	// Token: 0x06007A06 RID: 31238 RVA: 0x002E0CA8 File Offset: 0x002DEEA8
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_HUDStatBar>(null);
		this.statBarList = childrenByType;
		this.IsDirty = true;
	}

	// Token: 0x06007A07 RID: 31239 RVA: 0x002E0CD1 File Offset: 0x002DEED1
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			this.IsDirty = true;
		}
	}

	// Token: 0x04005A38 RID: 23096
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] statBarList;
}
