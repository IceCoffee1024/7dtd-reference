using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E24 RID: 3620
[Preserve]
public class XUiC_NewsScreen : XUiController
{
	// Token: 0x06006F35 RID: 28469 RVA: 0x002AFFF4 File Offset: 0x002AE1F4
	public override void Init()
	{
		base.Init();
		XUiC_NewsScreen.ID = base.WindowGroup.Id;
		XUiController childById = base.GetChildById("btnContinue");
		if (childById != null && childById.ViewComponent is XUiV_Button)
		{
			childById.OnPress += this.BtnContinue_OnPress;
		}
		this.controllerContinueLabel = base.GetChildById("continueButtonLabelController").ViewComponent as XUiV_Label;
		if (DeviceFlag.PS5.IsCurrent())
		{
			this.UpdateContinueLabel(PlayerInputManager.InputStyle.PS4);
			return;
		}
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent())
		{
			this.UpdateContinueLabel(PlayerInputManager.InputStyle.XB1);
		}
	}

	// Token: 0x06006F36 RID: 28470 RVA: 0x002B0081 File Offset: 0x002AE281
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		base.RefreshBindings();
		this.UpdateContinueLabel(_newStyle);
	}

	// Token: 0x06006F37 RID: 28471 RVA: 0x002B0098 File Offset: 0x002AE298
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateContinueLabel(PlayerInputManager.InputStyle _style)
	{
		if (_style != PlayerInputManager.InputStyle.Keyboard)
		{
			string text;
			if (PlayerInputManager.InputStyleFromSelectedIconStyle() == PlayerInputManager.InputStyle.PS4)
			{
				text = "[sp=PS5_Button_Options]";
			}
			else
			{
				text = "[sp=XB_Button_Menu]";
			}
			string text2 = Localization.Get("xuiNewsContinueController", false, null).ToUpper();
			text2 = string.Format(text2, text);
			this.controllerContinueLabel.Text = text2;
		}
	}

	// Token: 0x06006F38 RID: 28472 RVA: 0x002B00E5 File Offset: 0x002AE2E5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnContinue_OnPress(XUiController _sender, int _mousebutton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		XUiC_MainMenu.Open(this.xui);
	}

	// Token: 0x06006F39 RID: 28473 RVA: 0x002B010E File Offset: 0x002AE30E
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x06006F3A RID: 28474 RVA: 0x002B0120 File Offset: 0x002AE320
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if ((guiactions.Apply.WasReleased || guiactions.Cancel.WasReleased) && XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			this.BtnContinue_OnPress(this, -1);
		}
	}

	// Token: 0x06006F3B RID: 28475 RVA: 0x002B0173 File Offset: 0x002AE373
	public static void Open(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Open(XUiC_NewsScreen.ID, true, true);
	}

	// Token: 0x06006F3C RID: 28476 RVA: 0x002B018C File Offset: 0x002AE38C
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MessageBoxWindowGroup.Close(this.xui);
	}

	// Token: 0x04005452 RID: 21586
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005453 RID: 21587
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label controllerContinueLabel;
}
