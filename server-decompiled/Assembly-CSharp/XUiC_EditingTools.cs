using System;
using UnityEngine.Scripting;

// Token: 0x02000E1F RID: 3615
[Preserve]
public class XUiC_EditingTools : XUiController
{
	// Token: 0x06006E0E RID: 28174 RVA: 0x002AF4C0 File Offset: 0x002AD6C0
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

	// Token: 0x06006E0F RID: 28175 RVA: 0x002AF53D File Offset: 0x002AD73D
	public override void Init()
	{
		base.Init();
		XUiC_EditingTools.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006E10 RID: 28176 RVA: 0x002AF555 File Offset: 0x002AD755
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x06006E11 RID: 28177 RVA: 0x002AF564 File Offset: 0x002AD764
	public void SetWindow(string _windowName)
	{
		this.parentSelector.SetSelected(_windowName);
	}

	// Token: 0x04005405 RID: 21509
	public static string ID = "";

	// Token: 0x04005406 RID: 21510
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
