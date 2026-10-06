using System;

namespace Platform
{
	// Token: 0x02001B76 RID: 7030
	public static class DeviceCapabilities
	{
		// Token: 0x0600D2A5 RID: 53925 RVA: 0x004C9DE9 File Offset: 0x004C7FE9
		public static bool CanUserAccessFilesystem()
		{
			return !Submission.Enabled || (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent();
		}
	}
}
