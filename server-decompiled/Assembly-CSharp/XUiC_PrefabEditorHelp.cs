using System;
using UnityEngine.Scripting;

// Token: 0x02000FCE RID: 4046
[Preserve]
public class XUiC_PrefabEditorHelp : XUiController
{
	// Token: 0x06007F8C RID: 32652 RVA: 0x00308EB2 File Offset: 0x003070B2
	public override void Init()
	{
		base.Init();
		XUiC_PrefabEditorHelp.ID = base.WindowGroup.Id;
		base.GetChildById("outclick").OnPress += this.Close_OnPress;
	}

	// Token: 0x06007F8D RID: 32653 RVA: 0x00089715 File Offset: 0x00087915
	[PublicizedFrom(EAccessModifier.Private)]
	public void Close_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x04005EC6 RID: 24262
	public static string ID = "";
}
