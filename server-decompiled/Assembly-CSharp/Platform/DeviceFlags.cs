using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B42 RID: 6978
	public static class DeviceFlags
	{
		// Token: 0x0600D12C RID: 53548 RVA: 0x0003CD17 File Offset: 0x0003AF17
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCurrent(this DeviceFlag flags)
		{
			return (flags & DeviceFlag.StandaloneWindows) > DeviceFlag.None;
		}

		// Token: 0x0400A018 RID: 40984
		public const DeviceFlag Standalone = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX;

		// Token: 0x0400A019 RID: 40985
		public const DeviceFlag XBoxSeries = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX;

		// Token: 0x0400A01A RID: 40986
		public const DeviceFlag PS5 = DeviceFlag.PS5;

		// Token: 0x0400A01B RID: 40987
		public const DeviceFlag Console = DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x0400A01C RID: 40988
		public const DeviceFlag All = DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;

		// Token: 0x0400A01D RID: 40989
		public const DeviceFlag None = DeviceFlag.None;

		// Token: 0x0400A01E RID: 40990
		public const DeviceFlag Current = DeviceFlag.StandaloneWindows;
	}
}
