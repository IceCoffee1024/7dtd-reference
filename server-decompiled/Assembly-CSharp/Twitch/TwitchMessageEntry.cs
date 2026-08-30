using System;

namespace Twitch
{
	// Token: 0x02001855 RID: 6229
	public class TwitchMessageEntry
	{
		// Token: 0x0600C083 RID: 49283 RVA: 0x00474527 File Offset: 0x00472727
		public TwitchMessageEntry(string msg, string sound)
		{
			this.Message = msg;
			this.Sound = sound;
		}

		// Token: 0x04009117 RID: 37143
		public string Message = "";

		// Token: 0x04009118 RID: 37144
		public string Sound = "";
	}
}
