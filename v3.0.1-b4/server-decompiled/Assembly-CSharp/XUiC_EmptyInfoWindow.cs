using System;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F19 RID: 3865
[UnityEngine.Scripting.Preserve]
public class XUiC_EmptyInfoWindow : XUiC_InfoWindow
{
	// Token: 0x0600794B RID: 31051 RVA: 0x002DB4DD File Offset: 0x002D96DD
	public override void Init()
	{
		base.Init();
		this.descriptionText = base.GetChildById("descriptionText").ViewComponent as XUiV_Label;
		base.registerForInputStyleChanges();
	}

	// Token: 0x0600794C RID: 31052 RVA: 0x002DB506 File Offset: 0x002D9706
	public override void OnOpen()
	{
		base.OnOpen();
		this.UpdateDescriptionText();
	}

	// Token: 0x0600794D RID: 31053 RVA: 0x002DB514 File Offset: 0x002D9714
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateDescriptionText();
	}

	// Token: 0x0600794E RID: 31054 RVA: 0x002DB524 File Offset: 0x002D9724
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateDescriptionText()
	{
		if (!(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() && PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
		{
			this.descriptionText.Text = Localization.Get("xuiEmptyInfoPanelText", false, null);
			return;
		}
		PlayerInputManager.InputStyleFromSelectedIconStyle();
		string text = string.Format(Localization.Get("xuiEmptyInfoPanelTextController", false, null), new object[]
		{
			InControlExtensions.GetGamepadSourceString(InputControlType.Action1),
			InControlExtensions.GetGamepadSourceString(InputControlType.Action3),
			InControlExtensions.GetGamepadSourceString(InputControlType.Action3),
			InControlExtensions.GetGamepadSourceString(InputControlType.Action4),
			InControlExtensions.GetGamepadSourceString(InputControlType.Action4),
			InControlExtensions.GetGamepadSourceString(InputControlType.DPadUp),
			InControlExtensions.GetGamepadSourceString(InputControlType.DPadDown),
			InControlExtensions.GetGamepadSourceString(InputControlType.DPadLeft),
			InControlExtensions.GetGamepadSourceString(InputControlType.DPadRight),
			InControlExtensions.GetGamepadSourceString(InputControlType.RightStickButton),
			InControlExtensions.GetGamepadSourceString(InputControlType.LeftStickButton),
			InControlExtensions.GetGamepadSourceString(InputControlType.Back)
		});
		this.descriptionText.Text = text;
	}

	// Token: 0x04005984 RID: 22916
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label descriptionText;
}
