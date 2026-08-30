using System;

namespace Platform.EOS
{
	// Token: 0x02001CF7 RID: 7415
	public static class NetworkCommonEos
	{
		// Token: 0x0400A646 RID: 42566
		public const int MaxUsedPacketSize = 1120;

		// Token: 0x02001CF8 RID: 7416
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A648 RID: 42568
			NetpackageChannel0,
			// Token: 0x0400A649 RID: 42569
			NetpackageChannel1,
			// Token: 0x0400A64A RID: 42570
			Authentication = 50,
			// Token: 0x0400A64B RID: 42571
			Ping = 60
		}

		// Token: 0x02001CF9 RID: 7417
		public readonly struct SendInfo
		{
			// Token: 0x0600DC02 RID: 56322 RVA: 0x004EA632 File Offset: 0x004E8832
			public SendInfo(ClientInfo _clientInfo, ArrayListMP<byte> _data)
			{
				this.Recipient = _clientInfo;
				this.Data = _data;
			}

			// Token: 0x0400A64C RID: 42572
			public readonly ClientInfo Recipient;

			// Token: 0x0400A64D RID: 42573
			public readonly ArrayListMP<byte> Data;
		}
	}
}
