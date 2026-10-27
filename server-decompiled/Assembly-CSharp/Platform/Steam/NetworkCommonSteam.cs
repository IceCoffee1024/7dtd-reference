using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CFC RID: 7420
	public static class NetworkCommonSteam
	{
		// Token: 0x02001CFD RID: 7421
		public enum ESteamNetChannels : byte
		{
			// Token: 0x0400A678 RID: 42616
			NetpackageChannel0,
			// Token: 0x0400A679 RID: 42617
			NetpackageChannel1,
			// Token: 0x0400A67A RID: 42618
			Authentication = 50,
			// Token: 0x0400A67B RID: 42619
			Ping = 60
		}

		// Token: 0x02001CFE RID: 7422
		public readonly struct SendInfo
		{
			// Token: 0x0600DBB9 RID: 56249 RVA: 0x004E7BF6 File Offset: 0x004E5DF6
			public SendInfo(CSteamID _recipient, ArrayListMP<byte> _data)
			{
				this.Recipient = _recipient;
				this.Data = _data;
			}

			// Token: 0x0400A67C RID: 42620
			public readonly CSteamID Recipient;

			// Token: 0x0400A67D RID: 42621
			public readonly ArrayListMP<byte> Data;
		}
	}
}
