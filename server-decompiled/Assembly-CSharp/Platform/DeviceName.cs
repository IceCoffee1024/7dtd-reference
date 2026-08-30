using System;

namespace Platform
{
	// Token: 0x02001B1C RID: 6940
	public static class DeviceName
	{
		// Token: 0x0600CFA4 RID: 53156 RVA: 0x004BEE94 File Offset: 0x004BD094
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

		// Token: 0x04009EEE RID: 40686
		public const string StandaloneWindows = "Windows";

		// Token: 0x04009EEF RID: 40687
		public const string StandaloneLinux = "Linux";

		// Token: 0x04009EF0 RID: 40688
		public const string StandaloneOSX = "OSX";

		// Token: 0x04009EF1 RID: 40689
		public const string PS5 = "PS5";

		// Token: 0x04009EF2 RID: 40690
		public const string XBoxSeriesS = "XBoxSeriesS";

		// Token: 0x04009EF3 RID: 40691
		public const string XBoxSeriesX = "XBoxSeriesX";
	}
}
