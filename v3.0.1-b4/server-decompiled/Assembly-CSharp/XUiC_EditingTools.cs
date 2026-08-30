using System;
using UnityEngine.Scripting;

// Token: 0x02000DF0 RID: 3568
[Preserve]
public class XUiC_EditingTools : XUiController
{
	// Token: 0x06006D52 RID: 27986 RVA: 0x002A76E8 File Offset: 0x002A58E8
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

	// Token: 0x06006D53 RID: 27987 RVA: 0x002A7765 File Offset: 0x002A5965
	public override void Init()
	{
		base.Init();
		XUiC_EditingTools.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006D54 RID: 27988 RVA: 0x002A777D File Offset: 0x002A597D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.RefreshBindings();
	}

	// Token: 0x04005306 RID: 21254
	public static string ID = "";

	// Token: 0x04005307 RID: 21255
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WindowSelector parentSelector;
}
