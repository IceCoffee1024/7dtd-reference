using System;

namespace Twitch
{
	// Token: 0x02001868 RID: 6248
	public class TwitchVotePreset
	{
		// Token: 0x040091C8 RID: 37320
		public string Name;

		// Token: 0x040091C9 RID: 37321
		public bool IsDefault;

		// Token: 0x040091CA RID: 37322
		public bool IsEmpty;

		// Token: 0x040091CB RID: 37323
		public string Title;

		// Token: 0x040091CC RID: 37324
		public string Description;

		// Token: 0x040091CD RID: 37325
		public TwitchVotingManager.BossVoteSettings BossVoteSetting = TwitchVotingManager.BossVoteSettings.Standard;
	}
}
