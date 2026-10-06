using System;
using System.Net;

namespace Platform.LAN
{
	// Token: 0x02001D08 RID: 7432
	public static class LANServerSearchConfig
	{
		// Token: 0x0400A67A RID: 42618
		public static readonly IPAddress MulticastGroupIp = IPAddress.Parse("239.192.0.1");

		// Token: 0x0400A67B RID: 42619
		public const int DefaultPort = 11000;
	}
}
