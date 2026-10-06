using System;

namespace Twitch
{
	// Token: 0x02001894 RID: 6292
	public class TwitchVotePreset
	{
		// Token: 0x0400928E RID: 37518
		public string Name;

		// Token: 0x0400928F RID: 37519
		public bool IsDefault;

		// Token: 0x04009290 RID: 37520
		public bool IsEmpty;

		// Token: 0x04009291 RID: 37521
		public string Title;

		// Token: 0x04009292 RID: 37522
		public string Description;

		// Token: 0x04009293 RID: 37523
		public TwitchVotingManager.BossVoteSettings BossVoteSetting = TwitchVotingManager.BossVoteSettings.Standard;
	}
}
