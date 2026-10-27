using System;

namespace Platform.EOS
{
	// Token: 0x02001D5A RID: 7514
	public static class NetworkCommonEos
	{
		// Token: 0x0400A804 RID: 43012
		public const int MaxUsedPacketSize = 1120;

		// Token: 0x02001D5B RID: 7515
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A806 RID: 43014
			NetpackageChannel0,
			// Token: 0x0400A807 RID: 43015
			NetpackageChannel1,
			// Token: 0x0400A808 RID: 43016
			Authentication = 50,
			// Token: 0x0400A809 RID: 43017
			Ping = 60
		}

		// Token: 0x02001D5C RID: 7516
		public readonly struct SendInfo
		{
			// Token: 0x0600DE91 RID: 56977 RVA: 0x004F2D1A File Offset: 0x004F0F1A
			public SendInfo(ClientInfo _clientInfo, ArrayListMP<byte> _data)
			{
				this.Recipient = _clientInfo;
				this.Data = _data;
			}

			// Token: 0x0400A80A RID: 43018
			public readonly ClientInfo Recipient;

			// Token: 0x0400A80B RID: 43019
			public readonly ArrayListMP<byte> Data;
		}
	}
}
