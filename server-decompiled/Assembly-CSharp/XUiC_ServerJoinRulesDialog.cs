using System;
using UnityEngine.Scripting;

// Token: 0x02001006 RID: 4102
[Preserve]
public class XUiC_ServerJoinRulesDialog : XUiController
{
	// Token: 0x06008186 RID: 33158 RVA: 0x003139EC File Offset: 0x00311BEC
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

	// Token: 0x06008187 RID: 33159 RVA: 0x00313A7D File Offset: 0x00311C7D
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		base.GetChildById("btnLeave").SelectCursorElement(false, false);
	}

	// Token: 0x06008188 RID: 33160 RVA: 0x00313AAE File Offset: 0x00311CAE
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLeave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
	}

	// Token: 0x06008189 RID: 33161 RVA: 0x00313AD6 File Offset: 0x00311CD6
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSpawn_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		GameManager.Instance.DoSpawn();
	}

	// Token: 0x0600818A RID: 33162 RVA: 0x00313B00 File Offset: 0x00311D00
	public static void Show(LocalPlayerUI _playerUi, string _confirmationText)
	{
		XUiC_ServerJoinRulesDialog childByType = _playerUi.xui.FindWindowGroupByName(XUiC_ServerJoinRulesDialog.ID).GetChildByType<XUiC_ServerJoinRulesDialog>();
		if (childByType.labelConfirmationText != null)
		{
			childByType.labelConfirmationText.Text = _confirmationText.Replace("\\n", "\n");
		}
		_playerUi.windowManager.Open(XUiC_ServerJoinRulesDialog.ID, true, true);
	}

	// Token: 0x04005FD6 RID: 24534
	public static string ID = "";

	// Token: 0x04005FD7 RID: 24535
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelConfirmationText;
}
