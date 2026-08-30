using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001851 RID: 6225
	public class TwitchSpawnedBlocksEntry
	{
		// Token: 0x0600C07B RID: 49275 RVA: 0x00474314 File Offset: 0x00472514
		public bool CheckPos(Vector3i pos)
		{
			for (int i = 0; i < this.blocks.Count; i++)
			{
				if (this.blocks[i] == pos)
				{
					return true;
				}
			}
			if (this.recentlyRemoved != null)
			{
				for (int j = 0; j < this.recentlyRemoved.Count; j++)
				{
					if (this.recentlyRemoved[j] == pos)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C07C RID: 49276 RVA: 0x00474384 File Offset: 0x00472584
		[PublicizedFrom(EAccessModifier.Internal)]
		public bool RemoveBlock(Vector3i blockRemoved)
		{
			for (int i = this.blocks.Count - 1; i >= 0; i--)
			{
				if (this.blocks[i] == blockRemoved)
				{
					if (this.recentlyRemoved == null)
					{
						this.recentlyRemoved = new List<Vector3i>();
					}
					this.recentlyRemoved.Add(this.blocks[i]);
					this.blocks.RemoveAt(i);
				}
			}
			return this.blocks.Count == 0;
		}

		// Token: 0x0600C07D RID: 49277 RVA: 0x00474404 File Offset: 0x00472604
		[PublicizedFrom(EAccessModifier.Internal)]
		public bool RemoveBlocks(List<Vector3i> blocksRemoved)
		{
			for (int i = this.blocks.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < blocksRemoved.Count; j++)
				{
					if (this.blocks[i] == blocksRemoved[j])
					{
						this.blocks.RemoveAt(i);
						break;
					}
				}
			}
			return this.blocks.Count == 0;
		}

		// Token: 0x040090FF RID: 37119
		public List<Vector3i> blocks;

		// Token: 0x04009100 RID: 37120
		public List<Vector3i> recentlyRemoved;

		// Token: 0x04009101 RID: 37121
		public TwitchActionEntry Action;

		// Token: 0x04009102 RID: 37122
		public TwitchEventActionEntry Event;

		// Token: 0x04009103 RID: 37123
		public TwitchVoteEntry Vote;

		// Token: 0x04009104 RID: 37124
		public int BlockGroupID = -1;

		// Token: 0x04009105 RID: 37125
		public float TimeRemaining = -1f;

		// Token: 0x04009106 RID: 37126
		public TwitchRespawnEntry RespawnEntry;
	}
}
