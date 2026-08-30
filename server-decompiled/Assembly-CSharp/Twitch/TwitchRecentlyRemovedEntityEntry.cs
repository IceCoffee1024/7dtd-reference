using System;

namespace Twitch
{
	// Token: 0x0200182C RID: 6188
	public class TwitchRecentlyRemovedEntityEntry
	{
		// Token: 0x0600BEFA RID: 48890 RVA: 0x0046E6F0 File Offset: 0x0046C8F0
		public TwitchRecentlyRemovedEntityEntry(TwitchSpawnedEntityEntry entry)
		{
			this.SpawnedEntity = entry.SpawnedEntity;
			this.SpawnedEntityID = entry.SpawnedEntityID;
			this.Action = entry.Action;
			this.Event = entry.Event;
			this.Vote = entry.Vote;
			this.TimeRemaining = 60f;
		}

		// Token: 0x04008FEB RID: 36843
		public Entity SpawnedEntity;

		// Token: 0x04008FEC RID: 36844
		public int SpawnedEntityID = -1;

		// Token: 0x04008FED RID: 36845
		public TwitchActionEntry Action;

		// Token: 0x04008FEE RID: 36846
		public TwitchEventActionEntry Event;

		// Token: 0x04008FEF RID: 36847
		public TwitchVoteEntry Vote;

		// Token: 0x04008FF0 RID: 36848
		public float TimeRemaining;
	}
}
