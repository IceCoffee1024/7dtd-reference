using System;
using System.Net;

namespace Platform.LAN
{
	// Token: 0x02001CD6 RID: 7382
	public static class LANServerSearchConfig
	{
		// Token: 0x0400A5A5 RID: 42405
		public static readonly IPAddress MulticastGroupIp = IPAddress.Parse("239.192.0.1");

		// Token: 0x0400A5A6 RID: 42406
		public const int DefaultPort = 11000;
	}
}
