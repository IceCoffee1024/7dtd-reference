using System;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F30 RID: 3888
[UnityEngine.Scripting.Preserve]
public class XUiC_EmptyInfoWindow : XUiC_InfoWindow
{
	// Token: 0x06007A04 RID: 31236 RVA: 0x002DF491 File Offset: 0x002DD691
	public override void Init()
	{
		base.Init();
		this.descriptionText = base.GetChildById("descriptionText").ViewComponent as XUiV_Label;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06007A05 RID: 31237 RVA: 0x002DF4BA File Offset: 0x002DD6BA
	public override void OnOpen()
	{
		base.OnOpen();
		this.UpdateDescriptionText();
	}

	// Token: 0x06007A06 RID: 31238 RVA: 0x002DF4C8 File Offset: 0x002DD6C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateDescriptionText();
	}

	// Token: 0x06007A07 RID: 31239 RVA: 0x002DF4D8 File Offset: 0x002DD6D8
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

	// Token: 0x04005A37 RID: 23095
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label descriptionText;
}
