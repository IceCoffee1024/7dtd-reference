using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E0F RID: 3599
[Preserve]
public class XUiC_NewsScreen : XUiController
{
	// Token: 0x06006E95 RID: 28309 RVA: 0x002ACAD8 File Offset: 0x002AACD8
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

	// Token: 0x06006E96 RID: 28310 RVA: 0x002ACB65 File Offset: 0x002AAD65
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		base.RefreshBindings();
		this.UpdateContinueLabel(_newStyle);
	}

	// Token: 0x06006E97 RID: 28311 RVA: 0x002ACB7C File Offset: 0x002AAD7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateContinueLabel(PlayerInputManager.InputStyle _style)
	{
		if (_style != PlayerInputManager.InputStyle.Keyboard)
		{
			string text;
			if (PlatformManager.NativePlatform.Input.CurrentControllerInputStyle == PlayerInputManager.InputStyle.PS4)
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

	// Token: 0x06006E98 RID: 28312 RVA: 0x002ACBD3 File Offset: 0x002AADD3
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnContinue_OnPress(XUiController _sender, int _mousebutton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		XUiC_MainMenu.Open(this.xui);
	}

	// Token: 0x06006E99 RID: 28313 RVA: 0x002ACBFC File Offset: 0x002AADFC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x06006E9A RID: 28314 RVA: 0x002ACC0C File Offset: 0x002AAE0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if ((guiactions.Apply.WasReleased || guiactions.Cancel.WasReleased) && XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			this.BtnContinue_OnPress(this, -1);
		}
	}

	// Token: 0x06006E9B RID: 28315 RVA: 0x002ACC5F File Offset: 0x002AAE5F
	public static void Open(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Open(XUiC_NewsScreen.ID, true, true);
	}

	// Token: 0x06006E9C RID: 28316 RVA: 0x002ACC78 File Offset: 0x002AAE78
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MessageBoxWindowGroup.Close(this.xui);
	}

	// Token: 0x040053AE RID: 21422
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x040053AF RID: 21423
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label controllerContinueLabel;
}
