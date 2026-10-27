using System;

namespace Twitch
{
	// Token: 0x020018AF RID: 6319
	public class TwitchRecentlyRemovedEntityEntry
	{
		// Token: 0x0600C2EB RID: 49899 RVA: 0x0047C374 File Offset: 0x0047A574
		public TwitchRecentlyRemovedEntityEntry(TwitchSpawnedEntityEntry entry)
		{
			this.SpawnedEntity = entry.SpawnedEntity;
			this.SpawnedEntityID = entry.SpawnedEntityID;
			this.Action = entry.Action;
			this.Event = entry.Event;
			this.Vote = entry.Vote;
			this.TimeRemaining = 60f;
		}

		// Token: 0x040092B6 RID: 37558
		public Entity SpawnedEntity;

		// Token: 0x040092B7 RID: 37559
		public int SpawnedEntityID = -1;

		// Token: 0x040092B8 RID: 37560
		public TwitchActionEntry Action;

		// Token: 0x040092B9 RID: 37561
		public TwitchEventActionEntry Event;

		// Token: 0x040092BA RID: 37562
		public TwitchVoteEntry Vote;

		// Token: 0x040092BB RID: 37563
		public float TimeRemaining;
	}
}
