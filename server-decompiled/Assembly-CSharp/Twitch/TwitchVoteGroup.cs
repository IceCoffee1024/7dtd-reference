using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x020018C4 RID: 6340
	public class TwitchVoteGroup
	{
		// Token: 0x0600C35F RID: 50015 RVA: 0x0047EED3 File Offset: 0x0047D0D3
		public TwitchVoteGroup(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600C360 RID: 50016 RVA: 0x0047EEF8 File Offset: 0x0047D0F8
		public TwitchVoteType GetNextVoteType()
		{
			this.index++;
			if (this.index >= this.VoteTypes.Count)
			{
				this.index = 0;
			}
			return this.VoteTypes[this.index];
		}

		// Token: 0x0600C361 RID: 50017 RVA: 0x0047EF34 File Offset: 0x0047D134
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

		// Token: 0x04009373 RID: 37747
		public string Name = "";

		// Token: 0x04009374 RID: 37748
		public List<TwitchVoteType> VoteTypes = new List<TwitchVoteType>();

		// Token: 0x04009375 RID: 37749
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x04009376 RID: 37750
		public bool SkippedThisVote;
	}
}
