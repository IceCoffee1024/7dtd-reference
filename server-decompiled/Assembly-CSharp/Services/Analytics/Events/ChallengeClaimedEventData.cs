using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016A4 RID: 5796
	[EventTitle("Challenge Claimed")]
	public class ChallengeClaimedEventData : BaseEventData
	{
		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x0600B532 RID: 46386 RVA: 0x0043B349 File Offset: 0x00439549
		public override string EventType
		{
			get
			{
				return "challenge_claimed";
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x0600B533 RID: 46387 RVA: 0x0043B350 File Offset: 0x00439550
		// (set) Token: 0x0600B534 RID: 46388 RVA: 0x0043B358 File Offset: 0x00439558
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x0600B535 RID: 46389 RVA: 0x0043B361 File Offset: 0x00439561
		// (set) Token: 0x0600B536 RID: 46390 RVA: 0x0043B369 File Offset: 0x00439569
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x0600B537 RID: 46391 RVA: 0x0043B372 File Offset: 0x00439572
		// (set) Token: 0x0600B538 RID: 46392 RVA: 0x0043B37A File Offset: 0x0043957A
		[JsonProperty(PropertyName = "challenge_claimed_ts")]
		[Description("Timestamp of when the challenged was claimed")]
		[DateTimeFormat]
		public string ChallengeClaimedTimeStamp { get; set; }

		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x0600B539 RID: 46393 RVA: 0x0043B383 File Offset: 0x00439583
		// (set) Token: 0x0600B53A RID: 46394 RVA: 0x0043B38B File Offset: 0x0043958B
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time the challenge was claimed")]
		public uint ElapsedTime { get; set; }

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x0600B53B RID: 46395 RVA: 0x0043B394 File Offset: 0x00439594
		// (set) Token: 0x0600B53C RID: 46396 RVA: 0x0043B39C File Offset: 0x0043959C
		[JsonProperty(PropertyName = "player_level")]
		[Description("Level of the player when challenge claimed")]
		public int PlayerLevel { get; set; }

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x0600B53D RID: 46397 RVA: 0x0043B3A5 File Offset: 0x004395A5
		// (set) Token: 0x0600B53E RID: 46398 RVA: 0x0043B3AD File Offset: 0x004395AD
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("Personal Game Stage of the player when challenge claimed")]
		public int PersonalGameStage { get; set; }

		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x0600B53F RID: 46399 RVA: 0x0043B3B6 File Offset: 0x004395B6
		// (set) Token: 0x0600B540 RID: 46400 RVA: 0x0043B3BE File Offset: 0x004395BE
		[JsonProperty(PropertyName = "challenge_name")]
		[Description("Name of the challenge claimed")]
		public string ChallengeName { get; set; }

		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x0600B541 RID: 46401 RVA: 0x0043B3C7 File Offset: 0x004395C7
		// (set) Token: 0x0600B542 RID: 46402 RVA: 0x0043B3CF File Offset: 0x004395CF
		[JsonProperty(PropertyName = "challenge_group")]
		[Description("Group of the challenge claimed")]
		public string ChallengeGroup { get; set; }

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x0600B543 RID: 46403 RVA: 0x0043B3D8 File Offset: 0x004395D8
		// (set) Token: 0x0600B544 RID: 46404 RVA: 0x0043B3E0 File Offset: 0x004395E0
		[JsonProperty(PropertyName = "is_challenge_group_complete")]
		[Description("Group of the challenge claimed")]
		public bool IsChallengeGroupComplete { get; set; }

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x0600B545 RID: 46405 RVA: 0x0043B3E9 File Offset: 0x004395E9
		// (set) Token: 0x0600B546 RID: 46406 RVA: 0x0043B3F1 File Offset: 0x004395F1
		[JsonProperty(PropertyName = "challenge_category")]
		[Description("Category of the challenge claimed")]
		public string ChallengeCategory { get; set; }

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x0600B547 RID: 46407 RVA: 0x0043B3FA File Offset: 0x004395FA
		// (set) Token: 0x0600B548 RID: 46408 RVA: 0x0043B402 File Offset: 0x00439602
		[JsonProperty(PropertyName = "reward_event")]
		[Description("Reward Event of the challenge claimed")]
		public string RewardEvent { get; set; }
	}
}
