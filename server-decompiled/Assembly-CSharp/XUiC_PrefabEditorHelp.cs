using System;
using UnityEngine.Scripting;

// Token: 0x02000FA6 RID: 4006
[Preserve]
public class XUiC_PrefabEditorHelp : XUiController
{
	// Token: 0x06007E8B RID: 32395 RVA: 0x00301706 File Offset: 0x002FF906
	public override void Init()
	{
		base.Init();
		XUiC_PrefabEditorHelp.ID = base.WindowGroup.Id;
		base.GetChildById("outclick").OnPress += this.Close_OnPress;
	}

	// Token: 0x06007E8C RID: 32396 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void Close_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x04005DD6 RID: 24022
	public static string ID = "";
}
