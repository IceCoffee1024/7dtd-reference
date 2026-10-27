using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020018AE RID: 6318
	public class TwitchSpawnedBlocksEntry
	{
		// Token: 0x0600C2E7 RID: 49895 RVA: 0x0047C1FC File Offset: 0x0047A3FC
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

		// Token: 0x0600C2E8 RID: 49896 RVA: 0x0047C26C File Offset: 0x0047A46C
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

		// Token: 0x0600C2E9 RID: 49897 RVA: 0x0047C2EC File Offset: 0x0047A4EC
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

		// Token: 0x040092AE RID: 37550
		public List<Vector3i> blocks;

		// Token: 0x040092AF RID: 37551
		public List<Vector3i> recentlyRemoved;

		// Token: 0x040092B0 RID: 37552
		public TwitchActionEntry Action;

		// Token: 0x040092B1 RID: 37553
		public TwitchEventActionEntry Event;

		// Token: 0x040092B2 RID: 37554
		public TwitchVoteEntry Vote;

		// Token: 0x040092B3 RID: 37555
		public int BlockGroupID = -1;

		// Token: 0x040092B4 RID: 37556
		public float TimeRemaining = -1f;

		// Token: 0x040092B5 RID: 37557
		public TwitchRespawnEntry RespawnEntry;
	}
}
