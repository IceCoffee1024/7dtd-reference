using System;
using Platform;

// Token: 0x02001118 RID: 4376
public static class UIOptions
{
	// Token: 0x1700100F RID: 4111
	// (get) Token: 0x06008A9A RID: 35482 RVA: 0x0034C388 File Offset: 0x0034A588
	// (set) Token: 0x06008A9B RID: 35483 RVA: 0x0034C38F File Offset: 0x0034A58F
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

	// Token: 0x140000E3 RID: 227
	// (add) Token: 0x06008A9C RID: 35484 RVA: 0x0034C3A8 File Offset: 0x0034A5A8
	// (remove) Token: 0x06008A9D RID: 35485 RVA: 0x0034C3DC File Offset: 0x0034A5DC
	public static event Action<OptionsVideoWindowMode> OnOptionsVideoWindowChanged;

	// Token: 0x06008A9E RID: 35486 RVA: 0x0034C40F File Offset: 0x0034A60F
	public static void Init()
	{
		UIOptions.optionsVideoWindow = ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() ? OptionsVideoWindowMode.Simplified : OptionsVideoWindowMode.Detailed);
	}

	// Token: 0x040066DB RID: 26331
	[PublicizedFrom(EAccessModifier.Private)]
	public static OptionsVideoWindowMode optionsVideoWindow;
}
