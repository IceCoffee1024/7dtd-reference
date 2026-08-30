using System;
using UnityEngine.Scripting;

// Token: 0x02000DF2 RID: 3570
[Preserve]
public class XUiC_EditingToolsPoiEditor : XUiC_EditingToolsDialogBase
{
	// Token: 0x06006D5E RID: 27998 RVA: 0x002A7878 File Offset: 0x002A5A78
	[XuiBindEvent("OnPress", "btnStart")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnStart_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameUtils.StartSinglePrefabEditingWithMessage(null);
	}

	// Token: 0x06006D5F RID: 27999 RVA: 0x002A789C File Offset: 0x002A5A9C
	public override void Init()
	{
		base.Init();
		XUiC_EditingToolsPoiEditor.ID = this.windowGroup.Id;
	}

	// Token: 0x06006D60 RID: 28000 RVA: 0x002A78B4 File Offset: 0x002A5AB4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnStart_OnPressed(null, 0);
		}
	}

	// Token: 0x04005309 RID: 21257
	public static string ID = "";

	// Token: 0x0400530A RID: 21258
	[XuiBindComponent("btnStart", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStart;
}
