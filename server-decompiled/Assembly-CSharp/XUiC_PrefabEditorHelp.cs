using System;
using UnityEngine.Scripting;

// Token: 0x02000FDE RID: 4062
[Preserve]
public class XUiC_PrefabEditorHelp : XUiController
{
	// Token: 0x06007FDB RID: 32731 RVA: 0x0030A9CE File Offset: 0x00308BCE
	public override void Init()
	{
		base.Init();
		XUiC_PrefabEditorHelp.ID = base.WindowGroup.Id;
		base.GetChildById("outclick").OnPress += this.Close_OnPress;
	}

	// Token: 0x06007FDC RID: 32732 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Private)]
	public void Close_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x04005F01 RID: 24321
	public static string ID = "";
}
