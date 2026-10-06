using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200187D RID: 6269
	public class TwitchSpawnedBlocksEntry
	{
		// Token: 0x0600C1D3 RID: 49619 RVA: 0x00478850 File Offset: 0x00476A50
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

		// Token: 0x0600C1D4 RID: 49620 RVA: 0x004788C0 File Offset: 0x00476AC0
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

		// Token: 0x0600C1D5 RID: 49621 RVA: 0x00478940 File Offset: 0x00476B40
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

		// Token: 0x040091C5 RID: 37317
		public List<Vector3i> blocks;

		// Token: 0x040091C6 RID: 37318
		public List<Vector3i> recentlyRemoved;

		// Token: 0x040091C7 RID: 37319
		public TwitchActionEntry Action;

		// Token: 0x040091C8 RID: 37320
		public TwitchEventActionEntry Event;

		// Token: 0x040091C9 RID: 37321
		public TwitchVoteEntry Vote;

		// Token: 0x040091CA RID: 37322
		public int BlockGroupID = -1;

		// Token: 0x040091CB RID: 37323
		public float TimeRemaining = -1f;

		// Token: 0x040091CC RID: 37324
		public TwitchRespawnEntry RespawnEntry;
	}
}
