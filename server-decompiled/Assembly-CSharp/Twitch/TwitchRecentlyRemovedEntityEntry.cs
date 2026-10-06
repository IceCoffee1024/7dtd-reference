using System;

namespace Twitch
{
	// Token: 0x0200187E RID: 6270
	public class TwitchRecentlyRemovedEntityEntry
	{
		// Token: 0x0600C1D7 RID: 49623 RVA: 0x004789C8 File Offset: 0x00476BC8
		public TwitchRecentlyRemovedEntityEntry(TwitchSpawnedEntityEntry entry)
		{
			this.SpawnedEntity = entry.SpawnedEntity;
			this.SpawnedEntityID = entry.SpawnedEntityID;
			this.Action = entry.Action;
			this.Event = entry.Event;
			this.Vote = entry.Vote;
			this.TimeRemaining = 60f;
		}

		// Token: 0x040091CD RID: 37325
		public Entity SpawnedEntity;

		// Token: 0x040091CE RID: 37326
		public int SpawnedEntityID = -1;

		// Token: 0x040091CF RID: 37327
		public TwitchActionEntry Action;

		// Token: 0x040091D0 RID: 37328
		public TwitchEventActionEntry Event;

		// Token: 0x040091D1 RID: 37329
		public TwitchVoteEntry Vote;

		// Token: 0x040091D2 RID: 37330
		public float TimeRemaining;
	}
}
