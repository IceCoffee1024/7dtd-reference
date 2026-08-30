using System;

namespace mumblelib
{
	// Token: 0x02001678 RID: 5752
	public static class LinkFileManager
	{
		// Token: 0x0600B40E RID: 46094 RVA: 0x00432EB9 File Offset: 0x004310B9
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
