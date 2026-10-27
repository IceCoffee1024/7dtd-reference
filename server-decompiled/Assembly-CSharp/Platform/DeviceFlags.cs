using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001BA5 RID: 7077
	public static class DeviceFlags
	{
		// Token: 0x0600D3B7 RID: 54199 RVA: 0x0003CDC3 File Offset: 0x0003AFC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCurrent(this DeviceFlag flags)
		{
			return (flags & DeviceFlag.StandaloneWindows) > DeviceFlag.None;
		}

		// Token: 0x0400A1D6 RID: 41430
		public const DeviceFlag Standalone = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;

		// Token: 0x0400A1D7 RID: 41431
		public const DeviceFlag XBoxSeries = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX;

		// Token: 0x0400A1D8 RID: 41432
		public const DeviceFlag PS5 = DeviceFlag.PS5;

		// Token: 0x0400A1D9 RID: 41433
		public const DeviceFlag Console = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x0400A1DA RID: 41434
		public const DeviceFlag All = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x0400A1DB RID: 41435
		public const DeviceFlag None = DeviceFlag.None;

		// Token: 0x0400A1DC RID: 41436
		public const DeviceFlag Current = DeviceFlag.StandaloneWindows;
	}
}
