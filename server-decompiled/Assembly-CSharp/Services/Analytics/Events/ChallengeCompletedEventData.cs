using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016A5 RID: 5797
	[EventTitle("Challenge Completed")]
	public class ChallengeCompletedEventData : BaseEventData
	{
		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x0600B54A RID: 46410 RVA: 0x0043B413 File Offset: 0x00439613
		public override string EventType
		{
			get
			{
				return "challenge_completed";
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x0600B54B RID: 46411 RVA: 0x0043B41A File Offset: 0x0043961A
		// (set) Token: 0x0600B54C RID: 46412 RVA: 0x0043B422 File Offset: 0x00439622
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x0600B54D RID: 46413 RVA: 0x0043B42B File Offset: 0x0043962B
		// (set) Token: 0x0600B54E RID: 46414 RVA: 0x0043B433 File Offset: 0x00439633
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x0600B54F RID: 46415 RVA: 0x0043B43C File Offset: 0x0043963C
		// (set) Token: 0x0600B550 RID: 46416 RVA: 0x0043B444 File Offset: 0x00439644
		[JsonProperty(PropertyName = "challenge_completed_ts")]
		[Description("Timestamp of when the challenged was completed")]
		[DateTimeFormat]
		public string ChallengeCompletedTimeStamp { get; set; }

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x0600B551 RID: 46417 RVA: 0x0043B44D File Offset: 0x0043964D
		// (set) Token: 0x0600B552 RID: 46418 RVA: 0x0043B455 File Offset: 0x00439655
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time the challenge was completed")]
		public uint ElapsedTime { get; set; }

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x0600B553 RID: 46419 RVA: 0x0043B45E File Offset: 0x0043965E
		// (set) Token: 0x0600B554 RID: 46420 RVA: 0x0043B466 File Offset: 0x00439666
		[JsonProperty(PropertyName = "player_level")]
		[Description("Level of the player when challenge completed")]
		public int PlayerLevel { get; set; }

		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x0600B555 RID: 46421 RVA: 0x0043B46F File Offset: 0x0043966F
		// (set) Token: 0x0600B556 RID: 46422 RVA: 0x0043B477 File Offset: 0x00439677
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("Personal Game Stage of the player when challenge completed")]
		public int PersonalGameStage { get; set; }

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x0600B557 RID: 46423 RVA: 0x0043B480 File Offset: 0x00439680
		// (set) Token: 0x0600B558 RID: 46424 RVA: 0x0043B488 File Offset: 0x00439688
		[JsonProperty(PropertyName = "challenge_name")]
		[Description("Name of the challenge completed")]
		public string ChallengeName { get; set; }

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x0600B559 RID: 46425 RVA: 0x0043B491 File Offset: 0x00439691
		// (set) Token: 0x0600B55A RID: 46426 RVA: 0x0043B499 File Offset: 0x00439699
		[JsonProperty(PropertyName = "challenge_group")]
		[Description("Group of the challenge completed")]
		public string ChallengeGroup { get; set; }

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x0600B55B RID: 46427 RVA: 0x0043B4A2 File Offset: 0x004396A2
		// (set) Token: 0x0600B55C RID: 46428 RVA: 0x0043B4AA File Offset: 0x004396AA
		[JsonProperty(PropertyName = "challenge_category")]
		[Description("Category of the challenge completed")]
		public string ChallengeCategory { get; set; }

		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x0600B55D RID: 46429 RVA: 0x0043B4B3 File Offset: 0x004396B3
		// (set) Token: 0x0600B55E RID: 46430 RVA: 0x0043B4BB File Offset: 0x004396BB
		[JsonProperty(PropertyName = "is_forced_completion")]
		[Description("If the challenge was force completed")]
		public bool IsForcedCompletion { get; set; }
	}
}
