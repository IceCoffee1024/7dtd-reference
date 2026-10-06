using System;

namespace Twitch
{
	// Token: 0x0200187C RID: 6268
	public class TwitchSpawnedEntityEntry
	{
		// Token: 0x040091BF RID: 37311
		public Entity SpawnedEntity;

		// Token: 0x040091C0 RID: 37312
		public int SpawnedEntityID = -1;

		// Token: 0x040091C1 RID: 37313
		public TwitchActionEntry Action;

		// Token: 0x040091C2 RID: 37314
		public TwitchEventActionEntry Event;

		// Token: 0x040091C3 RID: 37315
		public TwitchVoteEntry Vote;

		// Token: 0x040091C4 RID: 37316
		public TwitchRespawnEntry RespawnEntry;
	}
}
