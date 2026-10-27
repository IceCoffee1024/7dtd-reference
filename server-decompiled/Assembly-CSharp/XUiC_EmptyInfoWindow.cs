using System;
using InControl;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F4E RID: 3918
[UnityEngine.Scripting.Preserve]
public class XUiC_EmptyInfoWindow : XUiC_InfoWindow
{
	// Token: 0x06007A44 RID: 31300 RVA: 0x002E40ED File Offset: 0x002E22ED
	public override void Init()
	{
		base.Init();
		this.descriptionText = base.GetChildById("descriptionText").ViewComponent as XUiV_Label;
		base.registerForInputStyleChanges();
	}

	// Token: 0x06007A45 RID: 31301 RVA: 0x002E4116 File Offset: 0x002E2316
	public override void OnOpen()
	{
		base.OnOpen();
		this.UpdateDescriptionText();
	}

	// Token: 0x06007A46 RID: 31302 RVA: 0x002E4124 File Offset: 0x002E2324
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		base.inputStyleChanged(_oldStyle, _newStyle);
		this.UpdateDescriptionText();
	}

	// Token: 0x06007A47 RID: 31303 RVA: 0x002E4134 File Offset: 0x002E2334
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

	// Token: 0x04005AA2 RID: 23202
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label descriptionText;
}
