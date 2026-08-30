using System;
using UnityEngine.Scripting;

// Token: 0x0200101F RID: 4127
[Preserve]
public class XUiC_ServerJoinRulesDialog : XUiController
{
	// Token: 0x06008278 RID: 33400 RVA: 0x00318214 File Offset: 0x00316414
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

	// Token: 0x06008279 RID: 33401 RVA: 0x003182A5 File Offset: 0x003164A5
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		base.GetChildById("btnLeave").SelectCursorElement(false, false);
	}

	// Token: 0x0600827A RID: 33402 RVA: 0x003182D6 File Offset: 0x003164D6
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLeave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
	}

	// Token: 0x0600827B RID: 33403 RVA: 0x003182FE File Offset: 0x003164FE
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSpawn_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameManager.Instance.DoSpawn();
	}

	// Token: 0x0600827C RID: 33404 RVA: 0x00318328 File Offset: 0x00316528
	public static void Show(LocalPlayerUI _playerUi, string _confirmationText)
	{
		XUiC_ServerJoinRulesDialog childByType = _playerUi.xui.FindWindowGroupByName(XUiC_ServerJoinRulesDialog.ID).GetChildByType<XUiC_ServerJoinRulesDialog>();
		if (childByType.labelConfirmationText != null)
		{
			childByType.labelConfirmationText.Text = _confirmationText.Replace("\\n", "\n");
		}
		_playerUi.windowManager.Open(XUiC_ServerJoinRulesDialog.ID, true, true);
	}

	// Token: 0x0400609D RID: 24733
	public static string ID = "";

	// Token: 0x0400609E RID: 24734
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelConfirmationText;
}
