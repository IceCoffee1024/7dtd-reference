using System;

namespace Platform
{
	// Token: 0x02001BA7 RID: 7079
	public static class DeviceCapabilities
	{
		// Token: 0x0600D3B9 RID: 54201 RVA: 0x004CD911 File Offset: 0x004CBB11
		public static bool CanUserAccessFilesystem()
		{
			return !Submission.Enabled || (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent();
		}
	}
}
