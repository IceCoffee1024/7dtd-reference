using System;
using UnityEngine.Scripting;

// Token: 0x02000F63 RID: 3939
[Preserve]
public class XUiC_InGameHUD : XUiController
{
	// Token: 0x06007AFF RID: 31487 RVA: 0x002E98D8 File Offset: 0x002E7AD8
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_HUDStatBar>(null);
		this.statBarList = childrenByType;
		this.IsDirty = true;
	}

	// Token: 0x06007B00 RID: 31488 RVA: 0x002E9901 File Offset: 0x002E7B01
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			this.IsDirty = true;
		}
	}

	// Token: 0x04005B56 RID: 23382
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] statBarList;
}
