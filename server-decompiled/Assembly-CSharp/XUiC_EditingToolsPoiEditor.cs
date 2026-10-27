using System;
using UnityEngine.Scripting;

// Token: 0x02000E21 RID: 3617
[Preserve]
public class XUiC_EditingToolsPoiEditor : XUiC_EditingToolsDialogBase
{
	// Token: 0x06006E1B RID: 28187 RVA: 0x002AF6A0 File Offset: 0x002AD8A0
	[XuiBindEvent("OnPress", "btnStart")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnStart_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameUtils.StartSinglePrefabEditingWithMessage(null);
	}

	// Token: 0x06006E1C RID: 28188 RVA: 0x002AF6C4 File Offset: 0x002AD8C4
	public override void Init()
	{
		base.Init();
		XUiC_EditingToolsPoiEditor.ID = this.windowGroup.Id;
	}

	// Token: 0x06006E1D RID: 28189 RVA: 0x002AF6DC File Offset: 0x002AD8DC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnStart_OnPressed(null, 0);
		}
	}

	// Token: 0x04005408 RID: 21512
	public static string ID = "";

	// Token: 0x04005409 RID: 21513
	[XuiBindComponent("btnStart", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStart;
}
