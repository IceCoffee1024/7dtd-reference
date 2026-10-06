using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E33 RID: 3635
[Preserve]
public class XUiC_NewsScreen : XUiController
{
	// Token: 0x06006F2D RID: 28461 RVA: 0x002B339C File Offset: 0x002B159C
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

	// Token: 0x06006F2E RID: 28462 RVA: 0x002B3429 File Offset: 0x002B1629
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		base.RefreshBindings();
		this.UpdateContinueLabel(_newStyle);
	}

	// Token: 0x06006F2F RID: 28463 RVA: 0x002B3440 File Offset: 0x002B1640
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

	// Token: 0x06006F30 RID: 28464 RVA: 0x002B348D File Offset: 0x002B168D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnContinue_OnPress(XUiController _sender, int _mousebutton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		XUiC_MainMenu.Open(this.xui);
	}

	// Token: 0x06006F31 RID: 28465 RVA: 0x002B34B6 File Offset: 0x002B16B6
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x06006F32 RID: 28466 RVA: 0x002B34C8 File Offset: 0x002B16C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		if ((guiactions.Apply.WasReleased || guiactions.Cancel.WasReleased) && XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			this.BtnContinue_OnPress(this, -1);
		}
	}

	// Token: 0x06006F33 RID: 28467 RVA: 0x002B351B File Offset: 0x002B171B
	public static void Open(XUi _xuiInstance)
	{
		_xuiInstance.playerUI.windowManager.Open(XUiC_NewsScreen.ID, true, true);
	}

	// Token: 0x06006F34 RID: 28468 RVA: 0x002B3534 File Offset: 0x002B1734
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MessageBoxWindowGroup.Close(this.xui);
	}

	// Token: 0x04005489 RID: 21641
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x0400548A RID: 21642
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label controllerContinueLabel;
}
