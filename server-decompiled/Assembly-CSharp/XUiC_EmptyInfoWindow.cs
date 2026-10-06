using System;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F3F RID: 3903
[UnityEngine.Scripting.Preserve]
public class XUiC_EmptyInfoWindow : XUiC_InfoWindow
{
	// Token: 0x06007A01 RID: 31233 RVA: 0x002E27FD File Offset: 0x002E09FD
	public override void Init()
	{
		base.Init();
		this.descriptionText = base.GetChildById("descriptionText").ViewComponent as XUiV_Label;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06007A02 RID: 31234 RVA: 0x002E2826 File Offset: 0x002E0A26
	public override void OnOpen()
	{
		base.OnOpen();
		this.UpdateDescriptionText();
	}

	// Token: 0x06007A03 RID: 31235 RVA: 0x002E2834 File Offset: 0x002E0A34
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateDescriptionText();
	}

	// Token: 0x06007A04 RID: 31236 RVA: 0x002E2844 File Offset: 0x002E0A44
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

	// Token: 0x04005A6F RID: 23151
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label descriptionText;
}
