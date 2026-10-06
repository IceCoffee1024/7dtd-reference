using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DCD RID: 3533
[Preserve]
public class XUiC_OptionsVideoBase : XUiC_OptionsDialogBase
{
	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x06006BA8 RID: 27560 RVA: 0x002A4803 File Offset: 0x002A2A03
	[XuiXmlBinding("vsync_count_pref")]
	public EnumGamePrefs VSyncCountPref
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformApplicationManager.Application.VSyncCountPref;
		}
	}

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x06006BA9 RID: 27561 RVA: 0x002A4810 File Offset: 0x002A2A10
	[XuiXmlBinding("ui_size_limited")]
	public bool UiSizeLimited
	{
		get
		{
			float num = (float)GameOptionsManager.GetUiSizeLimit();
			return GamePrefs.GetFloat(EnumGamePrefs.OptionsHudSize) > num;
		}
	}

	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x06006BAA RID: 27562 RVA: 0x002A4831 File Offset: 0x002A2A31
	[XuiXmlBinding("ui_size_limit")]
	public float UiSizeLimit
	{
		get
		{
			return (float)GameOptionsManager.GetUiSizeLimit();
		}
	}

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x06006BAB RID: 27563 RVA: 0x002A4839 File Offset: 0x002A2A39
	[XuiXmlBinding("fov_min")]
	public int FovMin
	{
		get
		{
			return Constants.cMinCameraFieldOfView;
		}
	}

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x06006BAC RID: 27564 RVA: 0x002A4840 File Offset: 0x002A2A40
	[XuiXmlBinding("fov_max")]
	public int FovMax
	{
		get
		{
			return Constants.cMaxCameraFieldOfView;
		}
	}

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x06006BAD RID: 27565 RVA: 0x002A4847 File Offset: 0x002A2A47
	[XuiXmlBinding("ui_opacity_background_min")]
	public float UiOpacityBackgroundMin
	{
		get
		{
			return Constants.cMinGlobalBackgroundOpacity;
		}
	}

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x06006BAE RID: 27566 RVA: 0x002A484E File Offset: 0x002A2A4E
	[XuiXmlBinding("ui_opacity_foreground_min")]
	public float UiOpacityForegroundMin
	{
		get
		{
			return Constants.cMinGlobalForegroundOpacity;
		}
	}
}
