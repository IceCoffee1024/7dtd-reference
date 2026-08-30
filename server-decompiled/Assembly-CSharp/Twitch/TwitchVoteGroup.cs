using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001867 RID: 6247
	public class TwitchVoteGroup
	{
		// Token: 0x0600C0F3 RID: 49395 RVA: 0x00476FEB File Offset: 0x004751EB
		public TwitchVoteGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600C0F4 RID: 49396 RVA: 0x00477010 File Offset: 0x00475210
		public TwitchVoteType GetNextVoteType()
		{
			this.index++;
			if (this.index >= this.VoteTypes.Count)
			{
				this.index = 0;
			}
			return this.VoteTypes[this.index];
		}

		// Token: 0x0600C0F5 RID: 49397 RVA: 0x0047704C File Offset: 0x0047524C
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

		// Token: 0x040091C4 RID: 37316
		public string Name = "";

		// Token: 0x040091C5 RID: 37317
		public List<TwitchVoteType> VoteTypes = new List<TwitchVoteType>();

		// Token: 0x040091C6 RID: 37318
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x040091C7 RID: 37319
		public bool SkippedThisVote;
	}
}
