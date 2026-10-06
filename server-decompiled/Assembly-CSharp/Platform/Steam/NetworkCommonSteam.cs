using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CCB RID: 7371
	public static class NetworkCommonSteam
	{
		// Token: 0x02001CCC RID: 7372
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A58F RID: 42383
			NetpackageChannel0,
			// Token: 0x0400A590 RID: 42384
			NetpackageChannel1,
			// Token: 0x0400A591 RID: 42385
			Authentication = 50,
			// Token: 0x0400A592 RID: 42386
			Ping = 60
		}

		// Token: 0x02001CCD RID: 7373
		public readonly struct SendInfo
		{
			// Token: 0x0600DAA5 RID: 55973 RVA: 0x004E4056 File Offset: 0x004E2256
			public SendInfo(CSteamID _recipient, ArrayListMP<byte> _data)
			{
				this.Recipient = _recipient;
				this.Data = _data;
			}

			// Token: 0x0400A593 RID: 42387
			public readonly CSteamID Recipient;

			// Token: 0x0400A594 RID: 42388
			public readonly ArrayListMP<byte> Data;
		}
	}
}
