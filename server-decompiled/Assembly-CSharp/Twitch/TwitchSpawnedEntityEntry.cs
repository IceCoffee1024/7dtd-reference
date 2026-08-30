using System;

namespace Twitch
{
	// Token: 0x02001850 RID: 6224
	public class TwitchSpawnedEntityEntry
	{
		// Token: 0x040090F9 RID: 37113
		public Entity SpawnedEntity;

		// Token: 0x040090FA RID: 37114
		public int SpawnedEntityID = -1;

		// Token: 0x040090FB RID: 37115
		public TwitchActionEntry Action;

		// Token: 0x040090FC RID: 37116
		public TwitchEventActionEntry Event;

		// Token: 0x040090FD RID: 37117
		public TwitchVoteEntry Vote;

		// Token: 0x040090FE RID: 37118
		public TwitchRespawnEntry RespawnEntry;
	}
}
