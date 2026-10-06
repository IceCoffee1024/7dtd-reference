using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001892 RID: 6290
	public class TwitchVoteEntry
	{
		// Token: 0x0600C24A RID: 49738 RVA: 0x0047B4ED File Offset: 0x004796ED
		public TwitchVoteEntry(string voteCommand, TwitchVote voteClass)
		{
			this.VoteCommand = voteCommand;
			this.VoteClass = voteClass;
		}

		// Token: 0x04009281 RID: 37505
		public TwitchVote VoteClass;

		// Token: 0x04009282 RID: 37506
		public string VoteCommand;

		// Token: 0x04009283 RID: 37507
		public TwitchVotingManager Owner;

		// Token: 0x04009284 RID: 37508
		public int VoteCount;

		// Token: 0x04009285 RID: 37509
		public int Index = -1;

		// Token: 0x04009286 RID: 37510
		public bool UIDirty = true;

		// Token: 0x04009287 RID: 37511
		public bool Complete;

		// Token: 0x04009288 RID: 37512
		public List<string> VoterNames = new List<string>();

		// Token: 0x04009289 RID: 37513
		public List<int> ActiveSpawns = new List<int>();
	}
}
