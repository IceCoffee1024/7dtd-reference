using System;
using UnityEngine.Scripting;

// Token: 0x02000FBF RID: 4031
[Preserve]
public class XUiC_PrefabEditorHelp : XUiController
{
	// Token: 0x06007F7D RID: 32637 RVA: 0x00305EDA File Offset: 0x003040DA
	public override void Init()
	{
		base.Init();
		XUiC_PrefabEditorHelp.ID = base.WindowGroup.Id;
		base.GetChildById("outclick").OnPress += this.Close_OnPress;
	}

	// Token: 0x06007F7E RID: 32638 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Private)]
	public void Close_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x04005E9D RID: 24221
	public static string ID = "";
}
