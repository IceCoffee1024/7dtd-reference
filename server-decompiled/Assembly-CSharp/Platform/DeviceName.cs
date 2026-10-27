using System;

namespace Platform
{
	// Token: 0x02001BA6 RID: 7078
	public static class DeviceName
	{
		// Token: 0x0600D3B8 RID: 54200 RVA: 0x004CD898 File Offset: 0x004CBA98
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

		// Token: 0x0400A1DD RID: 41437
		public const string StandaloneWindows = "Windows";

		// Token: 0x0400A1DE RID: 41438
		public const string StandaloneLinux = "Linux";

		// Token: 0x0400A1DF RID: 41439
		public const string StandaloneOSX = "OSX";

		// Token: 0x0400A1E0 RID: 41440
		public const string PS5 = "PS5";

		// Token: 0x0400A1E1 RID: 41441
		public const string XBoxSeriesS = "XBoxSeriesS";

		// Token: 0x0400A1E2 RID: 41442
		public const string XBoxSeriesX = "XBoxSeriesX";
	}
}
