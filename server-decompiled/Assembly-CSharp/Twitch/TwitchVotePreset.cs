using System;

namespace Twitch
{
	// Token: 0x020018C5 RID: 6341
	public class TwitchVotePreset
	{
		// Token: 0x04009377 RID: 37751
		public string Name;

		// Token: 0x04009378 RID: 37752
		public bool IsDefault;

		// Token: 0x04009379 RID: 37753
		public bool IsEmpty;

		// Token: 0x0400937A RID: 37754
		public string Title;

		// Token: 0x0400937B RID: 37755
		public string Description;

		// Token: 0x0400937C RID: 37756
		public TwitchVotingManager.BossVoteSettings BossVoteSetting = TwitchVotingManager.BossVoteSettings.Standard;
	}
}
