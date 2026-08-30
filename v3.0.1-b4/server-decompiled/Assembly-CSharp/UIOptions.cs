using System;
using Platform;

// Token: 0x020010FF RID: 4351
public static class UIOptions
{
	// Token: 0x17000FD6 RID: 4054
	// (get) Token: 0x060089A8 RID: 35240 RVA: 0x00347B64 File Offset: 0x00345D64
	// (set) Token: 0x060089A9 RID: 35241 RVA: 0x00347B6B File Offset: 0x00345D6B
	public static OptionsVideoWindowMode OptionsVideoWindow
	{
		get
		{
			return UIOptions.optionsVideoWindow;
		}
		set
		{
			UIOptions.optionsVideoWindow = value;
			Action<OptionsVideoWindowMode> onOptionsVideoWindowChanged = UIOptions.OnOptionsVideoWindowChanged;
			if (onOptionsVideoWindowChanged == null)
			{
				return;
			}
			onOptionsVideoWindowChanged(value);
		}
	}

	// Token: 0x140000E2 RID: 226
	// (add) Token: 0x060089AA RID: 35242 RVA: 0x00347B84 File Offset: 0x00345D84
	// (remove) Token: 0x060089AB RID: 35243 RVA: 0x00347BB8 File Offset: 0x00345DB8
	public static event Action<OptionsVideoWindowMode> OnOptionsVideoWindowChanged;

	// Token: 0x060089AC RID: 35244 RVA: 0x00347BEB File Offset: 0x00345DEB
	public static void Init()
	{
		UIOptions.optionsVideoWindow = ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() ? OptionsVideoWindowMode.Simplified : OptionsVideoWindowMode.Detailed);
	}

	// Token: 0x04006614 RID: 26132
	[PublicizedFrom(EAccessModifier.Private)]
	public static OptionsVideoWindowMode optionsVideoWindow;
}
