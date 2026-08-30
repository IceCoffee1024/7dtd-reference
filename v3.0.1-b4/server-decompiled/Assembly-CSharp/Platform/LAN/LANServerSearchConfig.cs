using System;
using System.Net;

namespace Platform.LAN
{
	// Token: 0x02001CAF RID: 7343
	public static class LANServerSearchConfig
	{
		// Token: 0x0400A472 RID: 42098
		public static readonly IPAddress MulticastGroupIp = IPAddress.Parse("239.192.0.1");

		// Token: 0x0400A473 RID: 42099
		public const int DefaultPort = 11000;
	}
}
