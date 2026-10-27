using System;

namespace Twitch
{
	// Token: 0x020018AD RID: 6317
	public class TwitchSpawnedEntityEntry
	{
		// Token: 0x040092A8 RID: 37544
		public Entity SpawnedEntity;

		// Token: 0x040092A9 RID: 37545
		public int SpawnedEntityID = -1;

		// Token: 0x040092AA RID: 37546
		public TwitchActionEntry Action;

		// Token: 0x040092AB RID: 37547
		public TwitchEventActionEntry Event;

		// Token: 0x040092AC RID: 37548
		public TwitchVoteEntry Vote;

		// Token: 0x040092AD RID: 37549
		public TwitchRespawnEntry RespawnEntry;
	}
}
