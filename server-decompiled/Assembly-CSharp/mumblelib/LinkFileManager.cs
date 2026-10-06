using System;

namespace mumblelib
{
	// Token: 0x020016CA RID: 5834
	public static class LinkFileManager
	{
		// Token: 0x0600B6E8 RID: 46824 RVA: 0x0043D151 File Offset: 0x0043B351
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
