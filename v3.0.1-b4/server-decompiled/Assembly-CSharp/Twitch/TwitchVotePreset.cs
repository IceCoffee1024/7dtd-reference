using System;

namespace Twitch
{
	// Token: 0x02001842 RID: 6210
	public class TwitchVotePreset
	{
		// Token: 0x040090AC RID: 37036
		public string Name;

		// Token: 0x040090AD RID: 37037
		public bool IsDefault;

		// Token: 0x040090AE RID: 37038
		public bool IsEmpty;

		// Token: 0x040090AF RID: 37039
		public string Title;

		// Token: 0x040090B0 RID: 37040
		public string Description;

		// Token: 0x040090B1 RID: 37041
		public TwitchVotingManager.BossVoteSettings BossVoteSetting = TwitchVotingManager.BossVoteSettings.Standard;
	}
}
