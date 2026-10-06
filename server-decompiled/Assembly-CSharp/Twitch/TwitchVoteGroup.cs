using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001893 RID: 6291
	public class TwitchVoteGroup
	{
		// Token: 0x0600C24B RID: 49739 RVA: 0x0047B527 File Offset: 0x00479727
		public TwitchVoteGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600C24C RID: 49740 RVA: 0x0047B54C File Offset: 0x0047974C
		public TwitchVoteType GetNextVoteType()
		{
			this.index++;
			if (this.index >= this.VoteTypes.Count)
			{
				this.index = 0;
			}
			return this.VoteTypes[this.index];
		}

		// Token: 0x0600C24D RID: 49741 RVA: 0x0047B588 File Offset: 0x00479788
		public void ShuffleVoteTypes()
		{
			for (int i = 0; i <= this.VoteTypes.Count * this.VoteTypes.Count; i++)
			{
				int num = global::UnityEngine.Random.Range(0, this.VoteTypes.Count);
				int num2 = global::UnityEngine.Random.Range(0, this.VoteTypes.Count);
				if (num != num2)
				{
					TwitchVoteType twitchVoteType = this.VoteTypes[num];
					this.VoteTypes[num] = this.VoteTypes[num2];
					this.VoteTypes[num2] = twitchVoteType;
				}
			}
		}

		// Token: 0x0400928A RID: 37514
		public string Name = "";

		// Token: 0x0400928B RID: 37515
		public List<TwitchVoteType> VoteTypes = new List<TwitchVoteType>();

		// Token: 0x0400928C RID: 37516
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x0400928D RID: 37517
		public bool SkippedThisVote;
	}
}
