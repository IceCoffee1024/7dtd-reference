using System;

namespace Platform.EOS
{
	// Token: 0x02001D29 RID: 7465
	public static class NetworkCommonEos
	{
		// Token: 0x0400A71B RID: 42779
		public const int MaxUsedPacketSize = 1120;

		// Token: 0x02001D2A RID: 7466
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A71D RID: 42781
			NetpackageChannel0,
			// Token: 0x0400A71E RID: 42782
			NetpackageChannel1,
			// Token: 0x0400A71F RID: 42783
			Authentication = 50,
			// Token: 0x0400A720 RID: 42784
			Ping = 60
		}

		// Token: 0x02001D2B RID: 7467
		public readonly struct SendInfo
		{
			// Token: 0x0600DD7D RID: 56701 RVA: 0x004EF17A File Offset: 0x004ED37A
			public SendInfo(ClientInfo _clientInfo, ArrayListMP<byte> _data)
			{
				this.Recipient = _clientInfo;
				this.Data = _data;
			}

			// Token: 0x0400A721 RID: 42785
			public readonly ClientInfo Recipient;

			// Token: 0x0400A722 RID: 42786
			public readonly ArrayListMP<byte> Data;
		}
	}
}
