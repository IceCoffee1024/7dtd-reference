using System;
using UnityEngine.Scripting;

// Token: 0x02000E05 RID: 3589
[Preserve]
public class XUiC_EditingTools : XUiController
{
	// Token: 0x06006DF1 RID: 28145 RVA: 0x002AABAC File Offset: 0x002A8DAC
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

	// Token: 0x06006DF2 RID: 28146 RVA: 0x002AAC29 File Offset: 0x002A8E29
	public override void Init()
	{
		base.Init();
		XUiC_EditingTools.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006DF3 RID: 28147 RVA: 0x002AAC41 File Offset: 0x002A8E41
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x06006DF4 RID: 28148 RVA: 0x002AAC50 File Offset: 0x002A8E50
	public void SetWindow(string _windowName)
	{
		this.parentSelector.SetSelected(_windowName);
	}

	// Token: 0x040053A9 RID: 21417
	public static string ID = "";

	// Token: 0x040053AA RID: 21418
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
