using System;

namespace Twitch
{
	// Token: 0x020018C0 RID: 6336
	public class GiftSubEntry
	{
		// Token: 0x0600C351 RID: 50001 RVA: 0x0047E5C0 File Offset: 0x0047C7C0
		public GiftSubEntry(string userName, int userID, TwitchSubEventEntry.SubTierTypes tier, int subCount)
		{
			this.UserName = userName;
			this.UserID = userID;
			this.TimeRemaining = 1f;
			this.SubCount = subCount;
			this.Tier = tier;
		}

		// Token: 0x0600C352 RID: 50002 RVA: 0x0047E61F File Offset: 0x0047C81F
		public void AddSub()
		{
			this.SubCount++;
			this.TimeRemaining = 1f;
		}

		// Token: 0x0600C353 RID: 50003 RVA: 0x0047E63A File Offset: 0x0047C83A
		public bool Update(float deltaTime)
		{
			this.TimeRemaining -= deltaTime;
			return this.TimeRemaining <= 0f;
		}

		// Token: 0x04009327 RID: 37671
		public float TimeRemaining = 1f;

		// Token: 0x04009328 RID: 37672
		public string UserName = "";

		// Token: 0x04009329 RID: 37673
		public int UserID = -1;

		// Token: 0x0400932A RID: 37674
		public int SubCount;

		// Token: 0x0400932B RID: 37675
		public TwitchSubEventEntry.SubTierTypes Tier = TwitchSubEventEntry.SubTierTypes.Tier1;
	}
}
