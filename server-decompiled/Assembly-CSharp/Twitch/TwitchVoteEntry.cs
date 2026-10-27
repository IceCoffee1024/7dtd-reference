using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020018C3 RID: 6339
	public class TwitchVoteEntry
	{
		// Token: 0x0600C35E RID: 50014 RVA: 0x0047EE99 File Offset: 0x0047D099
		public TwitchVoteEntry(string voteCommand, TwitchVote voteClass)
		{
			this.VoteCommand = voteCommand;
			this.VoteClass = voteClass;
		}

		// Token: 0x0400936A RID: 37738
		public TwitchVote VoteClass;

		// Token: 0x0400936B RID: 37739
		public string VoteCommand;

		// Token: 0x0400936C RID: 37740
		public TwitchVotingManager Owner;

		// Token: 0x0400936D RID: 37741
		public int VoteCount;

		// Token: 0x0400936E RID: 37742
		public int Index = -1;

		// Token: 0x0400936F RID: 37743
		public bool UIDirty = true;

		// Token: 0x04009370 RID: 37744
		public bool Complete;

		// Token: 0x04009371 RID: 37745
		public List<string> VoterNames = new List<string>();

		// Token: 0x04009372 RID: 37746
		public List<int> ActiveSpawns = new List<int>();
	}
}
