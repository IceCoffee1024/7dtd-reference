using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C72 RID: 7282
	public static class NetworkCommonSteam
	{
		// Token: 0x02001C73 RID: 7283
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A388 RID: 41864
			NetpackageChannel0,
			// Token: 0x0400A389 RID: 41865
			NetpackageChannel1,
			// Token: 0x0400A38A RID: 41866
			Authentication = 50,
			// Token: 0x0400A38B RID: 41867
			Ping = 60
		}

		// Token: 0x02001C74 RID: 7284
		public readonly struct SendInfo
		{
			// Token: 0x0600D799 RID: 55193 RVA: 0x004D8FF2 File Offset: 0x004D71F2
			public SendInfo(CSteamID _recipient, ArrayListMP<byte> _data)
			{
				this.Recipient = _recipient;
				this.Data = _data;
			}

			// Token: 0x0400A38C RID: 41868
			public readonly CSteamID Recipient;

			// Token: 0x0400A38D RID: 41869
			public readonly ArrayListMP<byte> Data;
		}
	}
}
