using System;

namespace Twitch
{
	// Token: 0x0200188F RID: 6287
	public class GiftSubEntry
	{
		// Token: 0x0600C23D RID: 49725 RVA: 0x0047AC14 File Offset: 0x00478E14
		public GiftSubEntry(string userName, int userID, TwitchSubEventEntry.SubTierTypes tier, int subCount)
		{
			this.UserName = userName;
			this.UserID = userID;
			this.TimeRemaining = 1f;
			this.SubCount = subCount;
			this.Tier = tier;
		}

		// Token: 0x0600C23E RID: 49726 RVA: 0x0047AC73 File Offset: 0x00478E73
		public void AddSub()
		{
			this.SubCount++;
			this.TimeRemaining = 1f;
		}

		// Token: 0x0600C23F RID: 49727 RVA: 0x0047AC8E File Offset: 0x00478E8E
		public bool Update(float deltaTime)
		{
			this.TimeRemaining -= deltaTime;
			return this.TimeRemaining <= 0f;
		}

		// Token: 0x0400923E RID: 37438
		public float TimeRemaining = 1f;

		// Token: 0x0400923F RID: 37439
		public string UserName = "";

		// Token: 0x04009240 RID: 37440
		public int UserID = -1;

		// Token: 0x04009241 RID: 37441
		public int SubCount;

		// Token: 0x04009242 RID: 37442
		public TwitchSubEventEntry.SubTierTypes Tier = TwitchSubEventEntry.SubTierTypes.Tier1;
	}
}
