using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x02001685 RID: 5765
	[EventTitle("Challenge Completed")]
	public class ChallengeCompletedEventData : BaseEventData
	{
		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x0600B4B0 RID: 46256 RVA: 0x004376FF File Offset: 0x004358FF
		public override string EventType
		{
			get
			{
				return "challenge_completed";
			}
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x0600B4B1 RID: 46257 RVA: 0x00437706 File Offset: 0x00435906
		// (set) Token: 0x0600B4B2 RID: 46258 RVA: 0x0043770E File Offset: 0x0043590E
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x0600B4B3 RID: 46259 RVA: 0x00437717 File Offset: 0x00435917
		// (set) Token: 0x0600B4B4 RID: 46260 RVA: 0x0043771F File Offset: 0x0043591F
		[JsonProperty(PropertyName = "save_id")]
		public string SaveId { get; set; }

		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x0600B4B5 RID: 46261 RVA: 0x00437728 File Offset: 0x00435928
		// (set) Token: 0x0600B4B6 RID: 46262 RVA: 0x00437730 File Offset: 0x00435930
		[JsonProperty(PropertyName = "challenge_completed_ts")]
		[Description("Timestamp of when the challenged was completed")]
		[DateTimeFormat]
		public string ChallengeCompletedTimeStamp { get; set; }

		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x0600B4B7 RID: 46263 RVA: 0x00437739 File Offset: 0x00435939
		// (set) Token: 0x0600B4B8 RID: 46264 RVA: 0x00437741 File Offset: 0x00435941
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time the challenge was completed")]
		public uint ElapsedTime { get; set; }

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x0600B4B9 RID: 46265 RVA: 0x0043774A File Offset: 0x0043594A
		// (set) Token: 0x0600B4BA RID: 46266 RVA: 0x00437752 File Offset: 0x00435952
		[JsonProperty(PropertyName = "player_level")]
		[Description("Level of the player when challenge completed")]
		public int PlayerLevel { get; set; }

		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x0600B4BB RID: 46267 RVA: 0x0043775B File Offset: 0x0043595B
		// (set) Token: 0x0600B4BC RID: 46268 RVA: 0x00437763 File Offset: 0x00435963
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("Personal Game Stage of the player when challenge completed")]
		public int PersonalGameStage { get; set; }

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x0600B4BD RID: 46269 RVA: 0x0043776C File Offset: 0x0043596C
		// (set) Token: 0x0600B4BE RID: 46270 RVA: 0x00437774 File Offset: 0x00435974
		[JsonProperty(PropertyName = "challenge_name")]
		[Description("Name of the challenge completed")]
		public string ChallengeName { get; set; }

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x0600B4BF RID: 46271 RVA: 0x0043777D File Offset: 0x0043597D
		// (set) Token: 0x0600B4C0 RID: 46272 RVA: 0x00437785 File Offset: 0x00435985
		[JsonProperty(PropertyName = "challenge_group")]
		[Description("Group of the challenge completed")]
		public string ChallengeGroup { get; set; }

		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x0600B4C1 RID: 46273 RVA: 0x0043778E File Offset: 0x0043598E
		// (set) Token: 0x0600B4C2 RID: 46274 RVA: 0x00437796 File Offset: 0x00435996
		[JsonProperty(PropertyName = "challenge_category")]
		[Description("Category of the challenge completed")]
		public string ChallengeCategory { get; set; }

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x0600B4C3 RID: 46275 RVA: 0x0043779F File Offset: 0x0043599F
		// (set) Token: 0x0600B4C4 RID: 46276 RVA: 0x004377A7 File Offset: 0x004359A7
		[JsonProperty(PropertyName = "is_forced_completion")]
		[Description("If the challenge was force completed")]
		public bool IsForcedCompletion { get; set; }
	}
}
