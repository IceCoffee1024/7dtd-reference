using System;

namespace Twitch
{
	// Token: 0x0200182F RID: 6191
	public class TwitchMessageEntry
	{
		// Token: 0x0600BEFE RID: 48894 RVA: 0x0046E78B File Offset: 0x0046C98B
		public TwitchMessageEntry(string msg, string sound)
		{
			this.Message = msg;
			this.Sound = sound;
		}

		// Token: 0x04008FFB RID: 36859
		public string Message = "";

		// Token: 0x04008FFC RID: 36860
		public string Sound = "";
	}
}
