using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DBE RID: 3518
[Preserve]
public class XUiC_OptionsVideoBase : XUiC_OptionsDialogBase
{
	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x06006BB0 RID: 27568 RVA: 0x002A1473 File Offset: 0x0029F673
	[XuiXmlBinding("vsync_count_pref")]
	public EnumGamePrefs VSyncCountPref
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformApplicationManager.Application.VSyncCountPref;
		}
	}

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x06006BB1 RID: 27569 RVA: 0x002A1480 File Offset: 0x0029F680
	[XuiXmlBinding("ui_size_limited")]
	public bool UiSizeLimited
	{
		get
		{
			float num = (float)GameOptionsManager.GetUiSizeLimit();
			return GamePrefs.GetFloat(EnumGamePrefs.OptionsHudSize) > num;
		}
	}

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x06006BB2 RID: 27570 RVA: 0x002A14A1 File Offset: 0x0029F6A1
	[XuiXmlBinding("ui_size_limit")]
	public float UiSizeLimit
	{
		get
		{
			return (float)GameOptionsManager.GetUiSizeLimit();
		}
	}

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x06006BB3 RID: 27571 RVA: 0x002A14A9 File Offset: 0x0029F6A9
	[XuiXmlBinding("fov_min")]
	public int FovMin
	{
		get
		{
			return Constants.cMinCameraFieldOfView;
		}
	}

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x06006BB4 RID: 27572 RVA: 0x002A14B0 File Offset: 0x0029F6B0
	[XuiXmlBinding("fov_max")]
	public int FovMax
	{
		get
		{
			return Constants.cMaxCameraFieldOfView;
		}
	}

	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x06006BB5 RID: 27573 RVA: 0x002A14B7 File Offset: 0x0029F6B7
	[XuiXmlBinding("ui_opacity_background_min")]
	public float UiOpacityBackgroundMin
	{
		get
		{
			return Constants.cMinGlobalBackgroundOpacity;
		}
	}

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x06006BB6 RID: 27574 RVA: 0x002A14BE File Offset: 0x0029F6BE
	[XuiXmlBinding("ui_opacity_foreground_min")]
	public float UiOpacityForegroundMin
	{
		get
		{
			return Constants.cMinGlobalForegroundOpacity;
		}
	}
}
