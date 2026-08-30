using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DAA RID: 3498
[Preserve]
public class XUiC_OptionsVideoBase : XUiC_OptionsDialogBase
{
	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x06006B15 RID: 27413 RVA: 0x0029E313 File Offset: 0x0029C513
	[XuiXmlBinding("vsync_count_pref")]
	public EnumGamePrefs VSyncCountPref
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return PlatformApplicationManager.Application.VSyncCountPref;
		}
	}

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x06006B16 RID: 27414 RVA: 0x0029E320 File Offset: 0x0029C520
	[XuiXmlBinding("ui_size_limited")]
	public bool UiSizeLimited
	{
		get
		{
			float num = (float)GameOptionsManager.GetUiSizeLimit();
			return GamePrefs.GetFloat(EnumGamePrefs.OptionsHudSize) > num;
		}
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06006B17 RID: 27415 RVA: 0x0029E341 File Offset: 0x0029C541
	[XuiXmlBinding("ui_size_limit")]
	public float UiSizeLimit
	{
		get
		{
			return (float)GameOptionsManager.GetUiSizeLimit();
		}
	}

	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06006B18 RID: 27416 RVA: 0x0029E349 File Offset: 0x0029C549
	[XuiXmlBinding("fov_min")]
	public int FovMin
	{
		get
		{
			return Constants.cMinCameraFieldOfView;
		}
	}

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x06006B19 RID: 27417 RVA: 0x0029E350 File Offset: 0x0029C550
	[XuiXmlBinding("fov_max")]
	public int FovMax
	{
		get
		{
			return Constants.cMaxCameraFieldOfView;
		}
	}

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06006B1A RID: 27418 RVA: 0x0029E357 File Offset: 0x0029C557
	[XuiXmlBinding("ui_opacity_background_min")]
	public float UiOpacityBackgroundMin
	{
		get
		{
			return Constants.cMinGlobalBackgroundOpacity;
		}
	}

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06006B1B RID: 27419 RVA: 0x0029E35E File Offset: 0x0029C55E
	[XuiXmlBinding("ui_opacity_foreground_min")]
	public float UiOpacityForegroundMin
	{
		get
		{
			return Constants.cMinGlobalForegroundOpacity;
		}
	}
}
