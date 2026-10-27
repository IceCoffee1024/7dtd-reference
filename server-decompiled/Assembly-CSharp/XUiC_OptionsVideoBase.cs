using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DD8 RID: 3544
[Preserve]
public class XUiC_OptionsVideoBase : XUiC_OptionsDialogBase
{
	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x06006BCD RID: 27597 RVA: 0x002A5D2B File Offset: 0x002A3F2B
	[XuiXmlBinding("vsync_count_pref")]
	public EnumGamePrefs VSyncCountPref
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformApplicationManager.Application.VSyncCountPref;
		}
	}

	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x06006BCE RID: 27598 RVA: 0x002A5D38 File Offset: 0x002A3F38
	[XuiXmlBinding("ui_size_limited")]
	public bool UiSizeLimited
	{
		get
		{
			float num = (float)GameOptionsManager.GetUiSizeLimit();
			return GamePrefs.GetFloat(EnumGamePrefs.OptionsHudSize) > num;
		}
	}

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x06006BCF RID: 27599 RVA: 0x002A5D59 File Offset: 0x002A3F59
	[XuiXmlBinding("ui_size_limit")]
	public float UiSizeLimit
	{
		get
		{
			return (float)GameOptionsManager.GetUiSizeLimit();
		}
	}

	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x002A5D61 File Offset: 0x002A3F61
	[XuiXmlBinding("fov_min")]
	public int FovMin
	{
		get
		{
			return Constants.cMinCameraFieldOfView;
		}
	}

	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x06006BD1 RID: 27601 RVA: 0x002A5D68 File Offset: 0x002A3F68
	[XuiXmlBinding("fov_max")]
	public int FovMax
	{
		get
		{
			return Constants.cMaxCameraFieldOfView;
		}
	}

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x06006BD2 RID: 27602 RVA: 0x002A5D6F File Offset: 0x002A3F6F
	[XuiXmlBinding("ui_opacity_background_min")]
	public float UiOpacityBackgroundMin
	{
		get
		{
			return Constants.cMinGlobalBackgroundOpacity;
		}
	}

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x06006BD3 RID: 27603 RVA: 0x002A5D76 File Offset: 0x002A3F76
	[XuiXmlBinding("ui_opacity_foreground_min")]
	public float UiOpacityForegroundMin
	{
		get
		{
			return Constants.cMinGlobalForegroundOpacity;
		}
	}
}
