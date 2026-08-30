using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001866 RID: 6246
	public class TwitchVoteEntry
	{
		// Token: 0x0600C0F2 RID: 49394 RVA: 0x00476FB1 File Offset: 0x004751B1
		public TwitchVoteEntry(string voteCommand, TwitchVote voteClass)
		{
			this.VoteCommand = voteCommand;
			this.VoteClass = voteClass;
		}

		// Token: 0x040091BB RID: 37307
		public TwitchVote VoteClass;

		// Token: 0x040091BC RID: 37308
		public string VoteCommand;

		// Token: 0x040091BD RID: 37309
		public TwitchVotingManager Owner;

		// Token: 0x040091BE RID: 37310
		public int VoteCount;

		// Token: 0x040091BF RID: 37311
		public int Index = -1;

		// Token: 0x040091C0 RID: 37312
		public bool UIDirty = true;

		// Token: 0x040091C1 RID: 37313
		public bool Complete;

		// Token: 0x040091C2 RID: 37314
		public List<string> VoterNames = new List<string>();

		// Token: 0x040091C3 RID: 37315
		public List<int> ActiveSpawns = new List<int>();
	}
}
