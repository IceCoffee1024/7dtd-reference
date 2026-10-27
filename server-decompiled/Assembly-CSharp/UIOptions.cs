using System;
using Platform;

// Token: 0x02001140 RID: 4416
public static class UIOptions
{
	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x06008B5A RID: 35674 RVA: 0x003528E4 File Offset: 0x00350AE4
	// (set) Token: 0x06008B5B RID: 35675 RVA: 0x003528EB File Offset: 0x00350AEB
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
	// (add) Token: 0x06008B5C RID: 35676 RVA: 0x00352904 File Offset: 0x00350B04
	// (remove) Token: 0x06008B5D RID: 35677 RVA: 0x00352938 File Offset: 0x00350B38
	public static event Action<OptionsVideoWindowMode> OnOptionsVideoWindowChanged;

	// Token: 0x06008B5E RID: 35678 RVA: 0x0035296B File Offset: 0x00350B6B
	public static void Init()
	{
		UIOptions.optionsVideoWindow = ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() ? OptionsVideoWindowMode.Simplified : OptionsVideoWindowMode.Detailed);
	}

	// Token: 0x04006770 RID: 26480
	[PublicizedFrom(EAccessModifier.Private)]
	public static OptionsVideoWindowMode optionsVideoWindow;
}
