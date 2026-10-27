using System;
using System.Net;

namespace Platform.LAN
{
	// Token: 0x02001D39 RID: 7481
	public static class LANServerSearchConfig
	{
		// Token: 0x0400A763 RID: 42851
		public static readonly IPAddress MulticastGroupIp = IPAddress.Parse("239.192.0.1");

		// Token: 0x0400A764 RID: 42852
		public const int DefaultPort = 11000;
	}
}
