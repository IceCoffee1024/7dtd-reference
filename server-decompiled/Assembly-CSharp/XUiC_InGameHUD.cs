using System;
using UnityEngine.Scripting;

// Token: 0x02000F54 RID: 3924
[Preserve]
public class XUiC_InGameHUD : XUiController
{
	// Token: 0x06007ABC RID: 31420 RVA: 0x002E7FDC File Offset: 0x002E61DC
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_HUDStatBar>(null);
		this.statBarList = childrenByType;
		this.IsDirty = true;
	}

	// Token: 0x06007ABD RID: 31421 RVA: 0x002E8005 File Offset: 0x002E6205
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
			this.IsDirty = true;
		}
	}

	// Token: 0x04005B23 RID: 23331
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] statBarList;
}
