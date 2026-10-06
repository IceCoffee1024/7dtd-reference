using System;
using UnityEngine.Scripting;

// Token: 0x02000E16 RID: 3606
[Preserve]
public class XUiC_EditingToolsPoiEditor : XUiC_EditingToolsDialogBase
{
	// Token: 0x06006DF6 RID: 28150 RVA: 0x002AE134 File Offset: 0x002AC334
	[XuiBindEvent("OnPress", "btnStart")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnStart_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameUtils.StartSinglePrefabEditingWithMessage(null);
	}

	// Token: 0x06006DF7 RID: 28151 RVA: 0x002AE158 File Offset: 0x002AC358
	public override void Init()
	{
		base.Init();
		XUiC_EditingToolsPoiEditor.ID = this.windowGroup.Id;
	}

	// Token: 0x06006DF8 RID: 28152 RVA: 0x002AE170 File Offset: 0x002AC370
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnStart_OnPressed(null, 0);
		}
	}

	// Token: 0x040053E3 RID: 21475
	public static string ID = "";

	// Token: 0x040053E4 RID: 21476
	[XuiBindComponent("btnStart", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStart;
}
