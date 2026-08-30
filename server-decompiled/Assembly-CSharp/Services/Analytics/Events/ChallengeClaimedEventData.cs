using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x02001684 RID: 5764
	[EventTitle("Challenge Claimed")]
	public class ChallengeClaimedEventData : BaseEventData
	{
		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x0600B498 RID: 46232 RVA: 0x00437635 File Offset: 0x00435835
		public override string EventType
		{
			get
			{
				return "challenge_claimed";
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x0600B499 RID: 46233 RVA: 0x0043763C File Offset: 0x0043583C
		// (set) Token: 0x0600B49A RID: 46234 RVA: 0x00437644 File Offset: 0x00435844
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x0600B49B RID: 46235 RVA: 0x0043764D File Offset: 0x0043584D
		// (set) Token: 0x0600B49C RID: 46236 RVA: 0x00437655 File Offset: 0x00435855
		[JsonProperty(PropertyName = "save_id")]
		public string SaveId { get; set; }

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x0600B49D RID: 46237 RVA: 0x0043765E File Offset: 0x0043585E
		// (set) Token: 0x0600B49E RID: 46238 RVA: 0x00437666 File Offset: 0x00435866
		[JsonProperty(PropertyName = "challenge_claimed_ts")]
		[Description("Timestamp of when the challenged was claimed")]
		[DateTimeFormat]
		public string ChallengeClaimedTimeStamp { get; set; }

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x0600B49F RID: 46239 RVA: 0x0043766F File Offset: 0x0043586F
		// (set) Token: 0x0600B4A0 RID: 46240 RVA: 0x00437677 File Offset: 0x00435877
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time the challenge was claimed")]
		public uint ElapsedTime { get; set; }

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x0600B4A1 RID: 46241 RVA: 0x00437680 File Offset: 0x00435880
		// (set) Token: 0x0600B4A2 RID: 46242 RVA: 0x00437688 File Offset: 0x00435888
		[JsonProperty(PropertyName = "player_level")]
		[Description("Level of the player when challenge claimed")]
		public int PlayerLevel { get; set; }

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x0600B4A3 RID: 46243 RVA: 0x00437691 File Offset: 0x00435891
		// (set) Token: 0x0600B4A4 RID: 46244 RVA: 0x00437699 File Offset: 0x00435899
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("Personal Game Stage of the player when challenge claimed")]
		public int PersonalGameStage { get; set; }

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x0600B4A5 RID: 46245 RVA: 0x004376A2 File Offset: 0x004358A2
		// (set) Token: 0x0600B4A6 RID: 46246 RVA: 0x004376AA File Offset: 0x004358AA
		[JsonProperty(PropertyName = "challenge_name")]
		[Description("Name of the challenge claimed")]
		public string ChallengeName { get; set; }

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x0600B4A7 RID: 46247 RVA: 0x004376B3 File Offset: 0x004358B3
		// (set) Token: 0x0600B4A8 RID: 46248 RVA: 0x004376BB File Offset: 0x004358BB
		[JsonProperty(PropertyName = "challenge_group")]
		[Description("Group of the challenge claimed")]
		public string ChallengeGroup { get; set; }

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x0600B4A9 RID: 46249 RVA: 0x004376C4 File Offset: 0x004358C4
		// (set) Token: 0x0600B4AA RID: 46250 RVA: 0x004376CC File Offset: 0x004358CC
		[JsonProperty(PropertyName = "is_challenge_group_complete")]
		[Description("Group of the challenge claimed")]
		public bool IsChallengeGroupComplete { get; set; }

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x0600B4AB RID: 46251 RVA: 0x004376D5 File Offset: 0x004358D5
		// (set) Token: 0x0600B4AC RID: 46252 RVA: 0x004376DD File Offset: 0x004358DD
		[JsonProperty(PropertyName = "challenge_category")]
		[Description("Category of the challenge claimed")]
		public string ChallengeCategory { get; set; }

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x0600B4AD RID: 46253 RVA: 0x004376E6 File Offset: 0x004358E6
		// (set) Token: 0x0600B4AE RID: 46254 RVA: 0x004376EE File Offset: 0x004358EE
		[JsonProperty(PropertyName = "reward_event")]
		[Description("Reward Event of the challenge claimed")]
		public string RewardEvent { get; set; }
	}
}
