using System;

namespace Twitch
{
	// Token: 0x0200182A RID: 6186
	public class TwitchSpawnedEntityEntry
	{
		// Token: 0x04008FDD RID: 36829
		public Entity SpawnedEntity;

		// Token: 0x04008FDE RID: 36830
		public int SpawnedEntityID = -1;

		// Token: 0x04008FDF RID: 36831
		public TwitchActionEntry Action;

		// Token: 0x04008FE0 RID: 36832
		public TwitchEventActionEntry Event;

		// Token: 0x04008FE1 RID: 36833
		public TwitchVoteEntry Vote;

		// Token: 0x04008FE2 RID: 36834
		public TwitchRespawnEntry RespawnEntry;
	}
}
