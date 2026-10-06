using System;

namespace Twitch
{
	// Token: 0x02001881 RID: 6273
	public class TwitchMessageEntry
	{
		// Token: 0x0600C1DB RID: 49627 RVA: 0x00478A63 File Offset: 0x00476C63
		public TwitchMessageEntry(string msg, string sound)
		{
			this.Message = msg;
			this.Sound = sound;
		}

		// Token: 0x040091DD RID: 37341
		public string Message = "";

		// Token: 0x040091DE RID: 37342
		public string Sound = "";
	}
}
