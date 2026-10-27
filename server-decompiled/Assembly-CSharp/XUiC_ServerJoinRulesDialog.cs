using System;
using UnityEngine.Scripting;

// Token: 0x0200103E RID: 4158
[Preserve]
public class XUiC_ServerJoinRulesDialog : XUiController
{
	// Token: 0x060082D9 RID: 33497 RVA: 0x0031CF5C File Offset: 0x0031B15C
	public override void Init()
	{
		base.Init();
		XUiC_ServerJoinRulesDialog.ID = base.WindowGroup.Id;
		XUiController childById = base.GetChildById("labelConfirmationText");
		this.labelConfirmationText = (XUiV_Label)((childById != null) ? childById.ViewComponent : null);
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnSpawn") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnSpawn_OnPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnLeave") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += this.BtnLeave_OnPressed;
		}
	}

	// Token: 0x060082DA RID: 33498 RVA: 0x0031CFED File Offset: 0x0031B1ED
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		base.GetChildById("btnLeave").SelectCursorElement(false, false);
	}

	// Token: 0x060082DB RID: 33499 RVA: 0x0031D01E File Offset: 0x0031B21E
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLeave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
	}

	// Token: 0x060082DC RID: 33500 RVA: 0x0031D046 File Offset: 0x0031B246
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSpawn_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameManager.Instance.DoSpawn();
	}

	// Token: 0x060082DD RID: 33501 RVA: 0x0031D070 File Offset: 0x0031B270
	public static void Show(LocalPlayerUI _playerUi, string _confirmationText)
	{
		XUiC_ServerJoinRulesDialog childByType = _playerUi.xui.FindWindowGroupByName(XUiC_ServerJoinRulesDialog.ID).GetChildByType<XUiC_ServerJoinRulesDialog>();
		if (childByType.labelConfirmationText != null)
		{
			childByType.labelConfirmationText.Text = _confirmationText.Replace("\\n", "\n");
		}
		_playerUi.windowManager.Open(XUiC_ServerJoinRulesDialog.ID, true, true);
	}

	// Token: 0x04006104 RID: 24836
	public static string ID = "";

	// Token: 0x04006105 RID: 24837
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelConfirmationText;
}
