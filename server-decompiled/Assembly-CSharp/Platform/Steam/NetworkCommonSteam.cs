using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C99 RID: 7321
	public static class NetworkCommonSteam
	{
		// Token: 0x02001C9A RID: 7322
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A4BA RID: 42170
			NetpackageChannel0,
			// Token: 0x0400A4BB RID: 42171
			NetpackageChannel1,
			// Token: 0x0400A4BC RID: 42172
			Authentication = 50,
			// Token: 0x0400A4BD RID: 42173
			Ping = 60
		}

		// Token: 0x02001C9B RID: 7323
		public readonly struct SendInfo
		{
			// Token: 0x0600D92B RID: 55595 RVA: 0x004DF50E File Offset: 0x004DD70E
			public SendInfo(CSteamID _recipient, ArrayListMP<byte> _data)
			{
				this.Recipient = _recipient;
				this.Data = _data;
			}

			// Token: 0x0400A4BE RID: 42174
			public readonly CSteamID Recipient;

			// Token: 0x0400A4BF RID: 42175
			public readonly ArrayListMP<byte> Data;
		}
	}
}
