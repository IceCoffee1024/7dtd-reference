using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B74 RID: 7028
	public static class DeviceFlags
	{
		// Token: 0x0600D2A3 RID: 53923 RVA: 0x0003CE03 File Offset: 0x0003B003
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCurrent(this DeviceFlag flags)
		{
			return (flags & DeviceFlag.StandaloneWindows) > DeviceFlag.None;
		}

		// Token: 0x0400A0ED RID: 41197
		public const DeviceFlag Standalone = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;

		// Token: 0x0400A0EE RID: 41198
		public const DeviceFlag XBoxSeries = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX;

		// Token: 0x0400A0EF RID: 41199
		public const DeviceFlag PS5 = DeviceFlag.PS5;

		// Token: 0x0400A0F0 RID: 41200
		public const DeviceFlag Console = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x0400A0F1 RID: 41201
		public const DeviceFlag All = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x0400A0F2 RID: 41202
		public const DeviceFlag None = DeviceFlag.None;

		// Token: 0x0400A0F3 RID: 41203
		public const DeviceFlag Current = DeviceFlag.StandaloneWindows;
	}
}
