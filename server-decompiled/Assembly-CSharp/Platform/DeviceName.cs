using System;

namespace Platform
{
	// Token: 0x02001B43 RID: 6979
	public static class DeviceName
	{
		// Token: 0x0600D12D RID: 53549 RVA: 0x004C5288 File Offset: 0x004C3488
		public static string GetDeviceName(this DeviceFlag _deviceId)
		{
			if (_deviceId <= DeviceFlag.XBoxSeriesS)
			{
				switch (_deviceId)
				{
				case DeviceFlag.StandaloneWindows:
					return "Windows";
				case DeviceFlag.StandaloneLinux:
					return "Linux";
				case DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux:
					break;
				case DeviceFlag.StandaloneOSX:
					return "OSX";
				default:
					if (_deviceId == DeviceFlag.XBoxSeriesS)
					{
						return "XBoxSeriesS";
					}
					break;
				}
			}
			else
			{
				if (_deviceId == DeviceFlag.XBoxSeriesX)
				{
					return "XBoxSeriesX";
				}
				if (_deviceId == DeviceFlag.PS5)
				{
					return "PS5";
				}
			}
			Log.Warning(string.Format("Device name for flag '{0}' is unknown", _deviceId));
			return string.Empty;
		}

		// Token: 0x0400A01F RID: 40991
		public const string StandaloneWindows = "Windows";

		// Token: 0x0400A020 RID: 40992
		public const string StandaloneLinux = "Linux";

		// Token: 0x0400A021 RID: 40993
		public const string StandaloneOSX = "OSX";

		// Token: 0x0400A022 RID: 40994
		public const string PS5 = "PS5";

		// Token: 0x0400A023 RID: 40995
		public const string XBoxSeriesS = "XBoxSeriesS";

		// Token: 0x0400A024 RID: 40996
		public const string XBoxSeriesX = "XBoxSeriesX";
	}
}
