using System;

namespace Twitch
{
	// Token: 0x0200183D RID: 6205
	public class GiftSubEntry
	{
		// Token: 0x0600BF60 RID: 48992 RVA: 0x0047093C File Offset: 0x0046EB3C
		public GiftSubEntry(string userName, int userID, TwitchSubEventEntry.SubTierTypes tier, int subCount)
		{
			this.UserName = userName;
			this.UserID = userID;
			this.TimeRemaining = 1f;
			this.SubCount = subCount;
			this.Tier = tier;
		}

		// Token: 0x0600BF61 RID: 48993 RVA: 0x0047099B File Offset: 0x0046EB9B
		public void AddSub()
		{
			this.SubCount++;
			this.TimeRemaining = 1f;
		}

		// Token: 0x0600BF62 RID: 48994 RVA: 0x004709B6 File Offset: 0x0046EBB6
		public bool Update(float deltaTime)
		{
			this.TimeRemaining -= deltaTime;
			return this.TimeRemaining <= 0f;
		}

		// Token: 0x0400905C RID: 36956
		public float TimeRemaining = 1f;

		// Token: 0x0400905D RID: 36957
		public string UserName = "";

		// Token: 0x0400905E RID: 36958
		public int UserID = -1;

		// Token: 0x0400905F RID: 36959
		public int SubCount;

		// Token: 0x04009060 RID: 36960
		public TwitchSubEventEntry.SubTierTypes Tier = TwitchSubEventEntry.SubTierTypes.Tier1;
	}
}
