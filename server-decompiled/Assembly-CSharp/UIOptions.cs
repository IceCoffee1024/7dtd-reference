using System;
using Platform;

// Token: 0x02001130 RID: 4400
public static class UIOptions
{
	// Token: 0x17001044 RID: 4164
	// (get) Token: 0x06008B06 RID: 35590 RVA: 0x00350834 File Offset: 0x0034EA34
	// (set) Token: 0x06008B07 RID: 35591 RVA: 0x0035083B File Offset: 0x0034EA3B
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
	// (add) Token: 0x06008B08 RID: 35592 RVA: 0x00350854 File Offset: 0x0034EA54
	// (remove) Token: 0x06008B09 RID: 35593 RVA: 0x00350888 File Offset: 0x0034EA88
	public static event Action<OptionsVideoWindowMode> OnOptionsVideoWindowChanged;

	// Token: 0x06008B0A RID: 35594 RVA: 0x003508BB File Offset: 0x0034EABB
	public static void Init()
	{
		UIOptions.optionsVideoWindow = ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() ? OptionsVideoWindowMode.Simplified : OptionsVideoWindowMode.Detailed);
	}

	// Token: 0x04006730 RID: 26416
	[PublicizedFrom(EAccessModifier.Private)]
	public static OptionsVideoWindowMode optionsVideoWindow;
}
