using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B8 RID: 5816
	[EventTitle("Challenge Completed")]
	public class ChallengeCompletedEventData : BaseEventData
	{
		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x0600B5B3 RID: 46515 RVA: 0x0043D934 File Offset: 0x0043BB34
		public override string EventType
		{
			get
			{
				return "challenge_completed";
			}
		}

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x0600B5B4 RID: 46516 RVA: 0x0043D93B File Offset: 0x0043BB3B
		// (set) Token: 0x0600B5B5 RID: 46517 RVA: 0x0043D943 File Offset: 0x0043BB43
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x0600B5B6 RID: 46518 RVA: 0x0043D94C File Offset: 0x0043BB4C
		// (set) Token: 0x0600B5B7 RID: 46519 RVA: 0x0043D954 File Offset: 0x0043BB54
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x0600B5B8 RID: 46520 RVA: 0x0043D95D File Offset: 0x0043BB5D
		// (set) Token: 0x0600B5B9 RID: 46521 RVA: 0x0043D965 File Offset: 0x0043BB65
		[JsonProperty(PropertyName = "challenge_completed_ts")]
		[Description("Timestamp of when the challenged was completed")]
		[DateTimeFormat]
		public string ChallengeCompletedTimeStamp { get; set; }

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x0600B5BA RID: 46522 RVA: 0x0043D96E File Offset: 0x0043BB6E
		// (set) Token: 0x0600B5BB RID: 46523 RVA: 0x0043D976 File Offset: 0x0043BB76
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time the challenge was completed")]
		public uint ElapsedTime { get; set; }

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x0600B5BC RID: 46524 RVA: 0x0043D97F File Offset: 0x0043BB7F
		// (set) Token: 0x0600B5BD RID: 46525 RVA: 0x0043D987 File Offset: 0x0043BB87
		[JsonProperty(PropertyName = "player_level")]
		[Description("Level of the player when challenge completed")]
		public int PlayerLevel { get; set; }

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x0600B5BE RID: 46526 RVA: 0x0043D990 File Offset: 0x0043BB90
		// (set) Token: 0x0600B5BF RID: 46527 RVA: 0x0043D998 File Offset: 0x0043BB98
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("Personal Game Stage of the player when challenge completed")]
		public int PersonalGameStage { get; set; }

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x0600B5C0 RID: 46528 RVA: 0x0043D9A1 File Offset: 0x0043BBA1
		// (set) Token: 0x0600B5C1 RID: 46529 RVA: 0x0043D9A9 File Offset: 0x0043BBA9
		[JsonProperty(PropertyName = "challenge_name")]
		[Description("Name of the challenge completed")]
		public string ChallengeName { get; set; }

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x0600B5C2 RID: 46530 RVA: 0x0043D9B2 File Offset: 0x0043BBB2
		// (set) Token: 0x0600B5C3 RID: 46531 RVA: 0x0043D9BA File Offset: 0x0043BBBA
		[JsonProperty(PropertyName = "challenge_group")]
		[Description("Group of the challenge completed")]
		public string ChallengeGroup { get; set; }

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x0600B5C4 RID: 46532 RVA: 0x0043D9C3 File Offset: 0x0043BBC3
		// (set) Token: 0x0600B5C5 RID: 46533 RVA: 0x0043D9CB File Offset: 0x0043BBCB
		[JsonProperty(PropertyName = "challenge_category")]
		[Description("Category of the challenge completed")]
		public string ChallengeCategory { get; set; }

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x0600B5C6 RID: 46534 RVA: 0x0043D9D4 File Offset: 0x0043BBD4
		// (set) Token: 0x0600B5C7 RID: 46535 RVA: 0x0043D9DC File Offset: 0x0043BBDC
		[JsonProperty(PropertyName = "is_forced_completion")]
		[Description("If the challenge was force completed")]
		public bool IsForcedCompletion { get; set; }

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x0600B5C8 RID: 46536 RVA: 0x0043D9E5 File Offset: 0x0043BBE5
		// (set) Token: 0x0600B5C9 RID: 46537 RVA: 0x0043D9ED File Offset: 0x0043BBED
		[JsonProperty(PropertyName = "player_location")]
		[Description("Location of the player at the time of completion")]
		public string PlayerLocation { get; set; }
	}
}
