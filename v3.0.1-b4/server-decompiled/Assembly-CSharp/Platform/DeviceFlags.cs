using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B1B RID: 6939
	public static class DeviceFlags
	{
		// Token: 0x0600CFA3 RID: 53155 RVA: 0x0003CAB7 File Offset: 0x0003ACB7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCurrent(this DeviceFlag flags)
		{
			return (flags & DeviceFlag.StandaloneWindows) > DeviceFlag.None;
		}

		// Token: 0x04009EE7 RID: 40679
		public const DeviceFlag Standalone = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;

		// Token: 0x04009EE8 RID: 40680
		public const DeviceFlag XBoxSeries = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX;

		// Token: 0x04009EE9 RID: 40681
		public const DeviceFlag PS5 = DeviceFlag.PS5;

		// Token: 0x04009EEA RID: 40682
		public const DeviceFlag Console = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x04009EEB RID: 40683
		public const DeviceFlag All = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x04009EEC RID: 40684
		public const DeviceFlag None = DeviceFlag.None;

		// Token: 0x04009EED RID: 40685
		public const DeviceFlag Current = DeviceFlag.StandaloneWindows;
	}
}
