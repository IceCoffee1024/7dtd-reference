using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B7 RID: 5815
	[EventTitle("Challenge Claimed")]
	public class ChallengeClaimedEventData : BaseEventData
	{
		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x0600B599 RID: 46489 RVA: 0x0043D861 File Offset: 0x0043BA61
		public override string EventType
		{
			get
			{
				return "challenge_claimed";
			}
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x0600B59A RID: 46490 RVA: 0x0043D868 File Offset: 0x0043BA68
		// (set) Token: 0x0600B59B RID: 46491 RVA: 0x0043D870 File Offset: 0x0043BA70
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x0600B59C RID: 46492 RVA: 0x0043D879 File Offset: 0x0043BA79
		// (set) Token: 0x0600B59D RID: 46493 RVA: 0x0043D881 File Offset: 0x0043BA81
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x0600B59E RID: 46494 RVA: 0x0043D88A File Offset: 0x0043BA8A
		// (set) Token: 0x0600B59F RID: 46495 RVA: 0x0043D892 File Offset: 0x0043BA92
		[JsonProperty(PropertyName = "challenge_claimed_ts")]
		[Description("Timestamp of when the challenged was claimed")]
		[DateTimeFormat]
		public string ChallengeClaimedTimeStamp { get; set; }

		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x0600B5A0 RID: 46496 RVA: 0x0043D89B File Offset: 0x0043BA9B
		// (set) Token: 0x0600B5A1 RID: 46497 RVA: 0x0043D8A3 File Offset: 0x0043BAA3
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time the challenge was claimed")]
		public uint ElapsedTime { get; set; }

		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x0600B5A2 RID: 46498 RVA: 0x0043D8AC File Offset: 0x0043BAAC
		// (set) Token: 0x0600B5A3 RID: 46499 RVA: 0x0043D8B4 File Offset: 0x0043BAB4
		[JsonProperty(PropertyName = "player_level")]
		[Description("Level of the player when challenge claimed")]
		public int PlayerLevel { get; set; }

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x0600B5A4 RID: 46500 RVA: 0x0043D8BD File Offset: 0x0043BABD
		// (set) Token: 0x0600B5A5 RID: 46501 RVA: 0x0043D8C5 File Offset: 0x0043BAC5
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("Personal Game Stage of the player when challenge claimed")]
		public int PersonalGameStage { get; set; }

		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x0600B5A6 RID: 46502 RVA: 0x0043D8CE File Offset: 0x0043BACE
		// (set) Token: 0x0600B5A7 RID: 46503 RVA: 0x0043D8D6 File Offset: 0x0043BAD6
		[JsonProperty(PropertyName = "challenge_name")]
		[Description("Name of the challenge claimed")]
		public string ChallengeName { get; set; }

		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x0600B5A8 RID: 46504 RVA: 0x0043D8DF File Offset: 0x0043BADF
		// (set) Token: 0x0600B5A9 RID: 46505 RVA: 0x0043D8E7 File Offset: 0x0043BAE7
		[JsonProperty(PropertyName = "challenge_group")]
		[Description("Group of the challenge claimed")]
		public string ChallengeGroup { get; set; }

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x0600B5AA RID: 46506 RVA: 0x0043D8F0 File Offset: 0x0043BAF0
		// (set) Token: 0x0600B5AB RID: 46507 RVA: 0x0043D8F8 File Offset: 0x0043BAF8
		[JsonProperty(PropertyName = "is_challenge_group_complete")]
		[Description("Group of the challenge claimed")]
		public bool IsChallengeGroupComplete { get; set; }

		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x0600B5AC RID: 46508 RVA: 0x0043D901 File Offset: 0x0043BB01
		// (set) Token: 0x0600B5AD RID: 46509 RVA: 0x0043D909 File Offset: 0x0043BB09
		[JsonProperty(PropertyName = "challenge_category")]
		[Description("Category of the challenge claimed")]
		public string ChallengeCategory { get; set; }

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x0600B5AE RID: 46510 RVA: 0x0043D912 File Offset: 0x0043BB12
		// (set) Token: 0x0600B5AF RID: 46511 RVA: 0x0043D91A File Offset: 0x0043BB1A
		[JsonProperty(PropertyName = "reward_event")]
		[Description("Reward Event of the challenge claimed")]
		public string RewardEvent { get; set; }

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x0600B5B0 RID: 46512 RVA: 0x0043D923 File Offset: 0x0043BB23
		// (set) Token: 0x0600B5B1 RID: 46513 RVA: 0x0043D92B File Offset: 0x0043BB2B
		[JsonProperty(PropertyName = "player_location")]
		[Description("Location of the player at the time of completion")]
		public string PlayerLocation { get; set; }
	}
}
