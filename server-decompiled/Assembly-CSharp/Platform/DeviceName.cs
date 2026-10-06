using System;

namespace Platform
{
	// Token: 0x02001B75 RID: 7029
	public static class DeviceName
	{
		// Token: 0x0600D2A4 RID: 53924 RVA: 0x004C9D70 File Offset: 0x004C7F70
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

		// Token: 0x0400A0F4 RID: 41204
		public const string StandaloneWindows = "Windows";

		// Token: 0x0400A0F5 RID: 41205
		public const string StandaloneLinux = "Linux";

		// Token: 0x0400A0F6 RID: 41206
		public const string StandaloneOSX = "OSX";

		// Token: 0x0400A0F7 RID: 41207
		public const string PS5 = "PS5";

		// Token: 0x0400A0F8 RID: 41208
		public const string XBoxSeriesS = "XBoxSeriesS";

		// Token: 0x0400A0F9 RID: 41209
		public const string XBoxSeriesX = "XBoxSeriesX";
	}
}
