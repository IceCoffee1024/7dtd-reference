using System;

namespace Platform
{
	// Token: 0x02001B44 RID: 6980
	public static class DeviceCapabilities
	{
		// Token: 0x0600D12E RID: 53550 RVA: 0x004C5301 File Offset: 0x004C3501
		public static bool CanUserAccessFilesystem()
		{
			return !Submission.Enabled || (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent();
		}
	}
}
