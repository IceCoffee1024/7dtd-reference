using System;
using UnityEngine.Scripting;

// Token: 0x0200102E RID: 4142
[Preserve]
public class XUiC_ServerJoinRulesDialog : XUiController
{
	// Token: 0x06008287 RID: 33415 RVA: 0x0031B258 File Offset: 0x00319458
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

	// Token: 0x06008288 RID: 33416 RVA: 0x0031B2E9 File Offset: 0x003194E9
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		base.GetChildById("btnLeave").SelectCursorElement(false, false);
	}

	// Token: 0x06008289 RID: 33417 RVA: 0x0031B31A File Offset: 0x0031951A
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLeave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
	}

	// Token: 0x0600828A RID: 33418 RVA: 0x0031B342 File Offset: 0x00319542
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSpawn_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameManager.Instance.DoSpawn();
	}

	// Token: 0x0600828B RID: 33419 RVA: 0x0031B36C File Offset: 0x0031956C
	public static void Show(LocalPlayerUI _playerUi, string _confirmationText)
	{
		XUiC_ServerJoinRulesDialog childByType = _playerUi.xui.FindWindowGroupByName(XUiC_ServerJoinRulesDialog.ID).GetChildByType<XUiC_ServerJoinRulesDialog>();
		if (childByType.labelConfirmationText != null)
		{
			childByType.labelConfirmationText.Text = _confirmationText.Replace("\\n", "\n");
		}
		_playerUi.windowManager.Open(XUiC_ServerJoinRulesDialog.ID, true, true);
	}

	// Token: 0x040060C6 RID: 24774
	public static string ID = "";

	// Token: 0x040060C7 RID: 24775
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelConfirmationText;
}
