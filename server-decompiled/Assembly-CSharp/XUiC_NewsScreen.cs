using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E3E RID: 3646
[Preserve]
public class XUiC_NewsScreen : XUiController
{
	// Token: 0x06006F52 RID: 28498 RVA: 0x002B4908 File Offset: 0x002B2B08
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

	// Token: 0x06006F53 RID: 28499 RVA: 0x002B4995 File Offset: 0x002B2B95
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		base.RefreshBindings();
		this.UpdateContinueLabel(_newStyle);
	}

	// Token: 0x06006F54 RID: 28500 RVA: 0x002B49AC File Offset: 0x002B2BAC
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

	// Token: 0x06006F55 RID: 28501 RVA: 0x002B49F9 File Offset: 0x002B2BF9
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnContinue_OnPress(XUiController _sender, int _mousebutton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		XUiC_MainMenu.Open(this.xui);
	}

	// Token: 0x06006F56 RID: 28502 RVA: 0x002B4A22 File Offset: 0x002B2C22
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x06006F57 RID: 28503 RVA: 0x002B4A34 File Offset: 0x002B2C34
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if ((guiactions.Apply.WasReleased || guiactions.Cancel.WasReleased) && XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			this.BtnContinue_OnPress(this, -1);
		}
	}

	// Token: 0x06006F58 RID: 28504 RVA: 0x002B4A87 File Offset: 0x002B2C87
	public static void Open(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Open(XUiC_NewsScreen.ID, true, true);
	}

	// Token: 0x06006F59 RID: 28505 RVA: 0x002B4AA0 File Offset: 0x002B2CA0
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MessageBoxWindowGroup.Close(this.xui);
	}

	// Token: 0x040054AE RID: 21678
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x040054AF RID: 21679
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label controllerContinueLabel;
}
