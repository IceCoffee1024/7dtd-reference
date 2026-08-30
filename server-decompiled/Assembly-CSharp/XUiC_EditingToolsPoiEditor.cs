using System;
using UnityEngine.Scripting;

// Token: 0x02000E07 RID: 3591
[Preserve]
public class XUiC_EditingToolsPoiEditor : XUiC_EditingToolsDialogBase
{
	// Token: 0x06006DFE RID: 28158 RVA: 0x002AAD8C File Offset: 0x002A8F8C
	[XuiBindEvent("OnPress", "btnStart")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnStart_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameUtils.StartSinglePrefabEditingWithMessage(null);
	}

	// Token: 0x06006DFF RID: 28159 RVA: 0x002AADB0 File Offset: 0x002A8FB0
	public override void Init()
	{
		base.Init();
		XUiC_EditingToolsPoiEditor.ID = this.windowGroup.Id;
	}

	// Token: 0x06006E00 RID: 28160 RVA: 0x002AADC8 File Offset: 0x002A8FC8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnStart_OnPressed(null, 0);
		}
	}

	// Token: 0x040053AC RID: 21420
	public static string ID = "";

	// Token: 0x040053AD RID: 21421
	[XuiBindComponent("btnStart", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStart;
}
