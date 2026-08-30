using System;
using UnityEngine.Scripting;

// Token: 0x02000F45 RID: 3909
[Preserve]
public class XUiC_InGameHUD : XUiController
{
	// Token: 0x06007ABF RID: 31423 RVA: 0x002E4C5C File Offset: 0x002E2E5C
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_HUDStatBar>(null);
		this.statBarList = childrenByType;
		this.IsDirty = true;
	}

	// Token: 0x06007AC0 RID: 31424 RVA: 0x002E4C85 File Offset: 0x002E2E85
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			this.IsDirty = true;
		}
	}

	// Token: 0x04005AEB RID: 23275
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] statBarList;
}
