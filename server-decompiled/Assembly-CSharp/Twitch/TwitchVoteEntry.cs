using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001840 RID: 6208
	public class TwitchVoteEntry
	{
		// Token: 0x0600BF6D RID: 49005 RVA: 0x00471215 File Offset: 0x0046F415
		public TwitchVoteEntry(string voteCommand, TwitchVote voteClass)
		{
			this.VoteCommand = voteCommand;
			this.VoteClass = voteClass;
		}

		// Token: 0x0400909F RID: 37023
		public TwitchVote VoteClass;

		// Token: 0x040090A0 RID: 37024
		public string VoteCommand;

		// Token: 0x040090A1 RID: 37025
		public TwitchVotingManager Owner;

		// Token: 0x040090A2 RID: 37026
		public int VoteCount;

		// Token: 0x040090A3 RID: 37027
		public int Index = -1;

		// Token: 0x040090A4 RID: 37028
		public bool UIDirty = true;

		// Token: 0x040090A5 RID: 37029
		public bool Complete;

		// Token: 0x040090A6 RID: 37030
		public List<string> VoterNames = new List<string>();

		// Token: 0x040090A7 RID: 37031
		public List<int> ActiveSpawns = new List<int>();
	}
}
