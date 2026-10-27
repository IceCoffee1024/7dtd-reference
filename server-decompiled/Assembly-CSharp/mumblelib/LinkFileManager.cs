using System;

namespace mumblelib
{
	// Token: 0x020016E9 RID: 5865
	public static class LinkFileManager
	{
		// Token: 0x0600B7BC RID: 47036 RVA: 0x0043FBF5 File Offset: 0x0043DDF5
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
