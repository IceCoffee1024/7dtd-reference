using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001841 RID: 6209
	public class TwitchVoteGroup
	{
		// Token: 0x0600BF6E RID: 49006 RVA: 0x0047124F File Offset: 0x0046F44F
		public TwitchVoteGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600BF6F RID: 49007 RVA: 0x00471274 File Offset: 0x0046F474
		public TwitchVoteType GetNextVoteType()
		{
			this.index++;
			if (this.index >= this.VoteTypes.Count)
			{
				this.index = 0;
			}
			return this.VoteTypes[this.index];
		}

		// Token: 0x0600BF70 RID: 49008 RVA: 0x004712B0 File Offset: 0x0046F4B0
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

		// Token: 0x040090A8 RID: 37032
		public string Name = "";

		// Token: 0x040090A9 RID: 37033
		public List<TwitchVoteType> VoteTypes = new List<TwitchVoteType>();

		// Token: 0x040090AA RID: 37034
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x040090AB RID: 37035
		public bool SkippedThisVote;
	}
}
