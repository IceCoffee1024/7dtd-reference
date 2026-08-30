using System;

namespace Platform
{
	// Token: 0x02001B1D RID: 6941
	public static class DeviceCapabilities
	{
		// Token: 0x0600CFA5 RID: 53157 RVA: 0x004BEF0D File Offset: 0x004BD10D
		public static bool CanUserAccessFilesystem()
		{
			return !Submission.Enabled || (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent();
		}
	}
}
