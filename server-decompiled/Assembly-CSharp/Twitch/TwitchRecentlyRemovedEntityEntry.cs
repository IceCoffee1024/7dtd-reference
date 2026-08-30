using System;

namespace Twitch
{
	// Token: 0x02001852 RID: 6226
	public class TwitchRecentlyRemovedEntityEntry
	{
		// Token: 0x0600C07F RID: 49279 RVA: 0x0047448C File Offset: 0x0047268C
		public TwitchRecentlyRemovedEntityEntry(TwitchSpawnedEntityEntry entry)
		{
			this.SpawnedEntity = entry.SpawnedEntity;
			this.SpawnedEntityID = entry.SpawnedEntityID;
			this.Action = entry.Action;
			this.Event = entry.Event;
			this.Vote = entry.Vote;
			this.TimeRemaining = 60f;
		}

		// Token: 0x04009107 RID: 37127
		public Entity SpawnedEntity;

		// Token: 0x04009108 RID: 37128
		public int SpawnedEntityID = -1;

		// Token: 0x04009109 RID: 37129
		public TwitchActionEntry Action;

		// Token: 0x0400910A RID: 37130
		public TwitchEventActionEntry Event;

		// Token: 0x0400910B RID: 37131
		public TwitchVoteEntry Vote;

		// Token: 0x0400910C RID: 37132
		public float TimeRemaining;
	}
}
