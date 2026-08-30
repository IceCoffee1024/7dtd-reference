using System;

namespace mumblelib
{
	// Token: 0x0200169E RID: 5790
	public static class LinkFileManager
	{
		// Token: 0x0600B591 RID: 46481 RVA: 0x00438C31 File Offset: 0x00436E31
		public static ILinkFile Open()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				Log.Out("[MumbleLF] Loading Windows Mumble Link");
				return new WindowsLinkFile();
			}
			Log.Out("[MumbleLF] Loading Unix Mumble Link");
			return new UnixLinkFile();
		}
	}
}
