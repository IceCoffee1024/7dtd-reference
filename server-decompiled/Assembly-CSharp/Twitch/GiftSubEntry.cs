using System;

namespace Twitch
{
	// Token: 0x02001863 RID: 6243
	public class GiftSubEntry
	{
		// Token: 0x0600C0E5 RID: 49381 RVA: 0x004766D8 File Offset: 0x004748D8
		public GiftSubEntry(string userName, int userID, TwitchSubEventEntry.SubTierTypes tier, int subCount)
		{
			this.UserName = userName;
			this.UserID = userID;
			this.TimeRemaining = 1f;
			this.SubCount = subCount;
			this.Tier = tier;
		}

		// Token: 0x0600C0E6 RID: 49382 RVA: 0x00476737 File Offset: 0x00474937
		public void AddSub()
		{
			this.SubCount++;
			this.TimeRemaining = 1f;
		}

		// Token: 0x0600C0E7 RID: 49383 RVA: 0x00476752 File Offset: 0x00474952
		public bool Update(float deltaTime)
		{
			this.TimeRemaining -= deltaTime;
			return this.TimeRemaining <= 0f;
		}

		// Token: 0x04009178 RID: 37240
		public float TimeRemaining = 1f;

		// Token: 0x04009179 RID: 37241
		public string UserName = "";

		// Token: 0x0400917A RID: 37242
		public int UserID = -1;

		// Token: 0x0400917B RID: 37243
		public int SubCount;

		// Token: 0x0400917C RID: 37244
		public TwitchSubEventEntry.SubTierTypes Tier = TwitchSubEventEntry.SubTierTypes.Tier1;
	}
}
