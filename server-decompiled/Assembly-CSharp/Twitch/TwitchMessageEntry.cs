using System;

namespace Twitch
{
	// Token: 0x020018B2 RID: 6322
	public class TwitchMessageEntry
	{
		// Token: 0x0600C2EF RID: 49903 RVA: 0x0047C40F File Offset: 0x0047A60F
		public TwitchMessageEntry(string msg, string sound)
		{
			this.Message = msg;
			this.Sound = sound;
		}

		// Token: 0x040092C6 RID: 37574
		public string Message = "";

		// Token: 0x040092C7 RID: 37575
		public string Sound = "";
	}
}
