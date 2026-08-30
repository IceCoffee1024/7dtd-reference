using System;

namespace Platform.EOS
{
	// Token: 0x02001CD0 RID: 7376
	public static class NetworkCommonEos
	{
		// Token: 0x0400A513 RID: 42259
		public const int MaxUsedPacketSize = 1120;

		// Token: 0x02001CD1 RID: 7377
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A515 RID: 42261
			NetpackageChannel0,
			// Token: 0x0400A516 RID: 42262
			NetpackageChannel1,
			// Token: 0x0400A517 RID: 42263
			Authentication = 50,
			// Token: 0x0400A518 RID: 42264
			Ping = 60
		}

		// Token: 0x02001CD2 RID: 7378
		public readonly struct SendInfo
		{
			// Token: 0x0600DA67 RID: 55911 RVA: 0x004E401E File Offset: 0x004E221E
			public SendInfo(ClientInfo _clientInfo, ArrayListMP<byte> _data)
			{
				this.Recipient = _clientInfo;
				this.Data = _data;
			}

			// Token: 0x0400A519 RID: 42265
			public readonly ClientInfo Recipient;

			// Token: 0x0400A51A RID: 42266
			public readonly ArrayListMP<byte> Data;
		}
	}
}
