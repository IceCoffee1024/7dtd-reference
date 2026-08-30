using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200182B RID: 6187
	public class TwitchSpawnedBlocksEntry
	{
		// Token: 0x0600BEF6 RID: 48886 RVA: 0x0046E578 File Offset: 0x0046C778
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

		// Token: 0x0600BEF7 RID: 48887 RVA: 0x0046E5E8 File Offset: 0x0046C7E8
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

		// Token: 0x0600BEF8 RID: 48888 RVA: 0x0046E668 File Offset: 0x0046C868
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

		// Token: 0x04008FE3 RID: 36835
		public List<Vector3i> blocks;

		// Token: 0x04008FE4 RID: 36836
		public List<Vector3i> recentlyRemoved;

		// Token: 0x04008FE5 RID: 36837
		public TwitchActionEntry Action;

		// Token: 0x04008FE6 RID: 36838
		public TwitchEventActionEntry Event;

		// Token: 0x04008FE7 RID: 36839
		public TwitchVoteEntry Vote;

		// Token: 0x04008FE8 RID: 36840
		public int BlockGroupID = -1;

		// Token: 0x04008FE9 RID: 36841
		public float TimeRemaining = -1f;

		// Token: 0x04008FEA RID: 36842
		public TwitchRespawnEntry RespawnEntry;
	}
}
