using System;
using UnityEngine.Scripting;

// Token: 0x02000E14 RID: 3604
[Preserve]
public class XUiC_EditingTools : XUiController
{
	// Token: 0x06006DE9 RID: 28137 RVA: 0x002ADF54 File Offset: 0x002AC154
	[XuiBindEvent("WindowSelected", "parentSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void onWindowSelected(XUiC_WindowSelector _sender, string _windowId)
	{
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (!(_windowId == "rwgPreviewer"))
		{
			if (_windowId == "poiEditor")
			{
				windowManager.Open(XUiC_EditingToolsPoiEditor.ID, true);
				return;
			}
			if (!(_windowId == "worldEditor"))
			{
				return;
			}
			windowManager.Open(XUiC_WorldEditor.ID, true);
		}
		else if (!XUiC_WorldGenerationWindow.IsWindowOpen(this.xui))
		{
			XUiC_WorldGenerationWindow.Open(this.xui, XUiC_MainMenu.ID);
			return;
		}
	}

	// Token: 0x06006DEA RID: 28138 RVA: 0x002ADFD1 File Offset: 0x002AC1D1
	public override void Init()
	{
		base.Init();
		XUiC_EditingTools.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006DEB RID: 28139 RVA: 0x002ADFE9 File Offset: 0x002AC1E9
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x06006DEC RID: 28140 RVA: 0x002ADFF8 File Offset: 0x002AC1F8
	public void SetWindow(string _windowName)
	{
		this.parentSelector.SetSelected(_windowName);
	}

	// Token: 0x040053E0 RID: 21472
	public static string ID = "";

	// Token: 0x040053E1 RID: 21473
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
