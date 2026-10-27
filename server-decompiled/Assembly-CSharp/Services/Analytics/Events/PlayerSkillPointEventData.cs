using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016CB RID: 5835
	[EventTitle("SkillPointEvent")]
	public class PlayerSkillPointEventData : BaseEventData
	{
		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x0600B6A9 RID: 46761 RVA: 0x0043E1C7 File Offset: 0x0043C3C7
		public override string EventType
		{
			get
			{
				return "player_skill_points";
			}
		}

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x0600B6AA RID: 46762 RVA: 0x0043E1CE File Offset: 0x0043C3CE
		// (set) Token: 0x0600B6AB RID: 46763 RVA: 0x0043E1D6 File Offset: 0x0043C3D6
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server")]
		public string ServerId { get; set; }

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x0600B6AC RID: 46764 RVA: 0x0043E1DF File Offset: 0x0043C3DF
		// (set) Token: 0x0600B6AD RID: 46765 RVA: 0x0043E1E7 File Offset: 0x0043C3E7
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x0600B6AE RID: 46766 RVA: 0x0043E1F0 File Offset: 0x0043C3F0
		// (set) Token: 0x0600B6AF RID: 46767 RVA: 0x0043E1F8 File Offset: 0x0043C3F8
		[JsonProperty(PropertyName = "transaction_ts")]
		[DateTimeFormat]
		[Description("Whenever the player uses or gain the server")]
		public string TransactionTimestamp { get; set; }

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x0600B6B0 RID: 46768 RVA: 0x0043E201 File Offset: 0x0043C401
		// (set) Token: 0x0600B6B1 RID: 46769 RVA: 0x0043E209 File Offset: 0x0043C409
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time of trigger")]
		public int ElapsedTime { get; set; }

		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x0600B6B2 RID: 46770 RVA: 0x0043E212 File Offset: 0x0043C412
		// (set) Token: 0x0600B6B3 RID: 46771 RVA: 0x0043E21A File Offset: 0x0043C41A
		[JsonProperty(PropertyName = "in_game_days")]
		[Description("The current in-game days of the world at the time of the transaction")]
		public int InGameDays { get; set; }

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x0600B6B4 RID: 46772 RVA: 0x0043E223 File Offset: 0x0043C423
		// (set) Token: 0x0600B6B5 RID: 46773 RVA: 0x0043E22B File Offset: 0x0043C42B
		[JsonProperty(PropertyName = "transaction_type")]
		[Description("The type of the transaction")]
		public string TransactionType { get; set; }

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x0600B6B6 RID: 46774 RVA: 0x0043E234 File Offset: 0x0043C434
		// (set) Token: 0x0600B6B7 RID: 46775 RVA: 0x0043E23C File Offset: 0x0043C43C
		[JsonProperty(PropertyName = "amount")]
		[Description("The amount of skill points involved in this transaction")]
		public int Amount { get; set; }

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x0600B6B8 RID: 46776 RVA: 0x0043E245 File Offset: 0x0043C445
		// (set) Token: 0x0600B6B9 RID: 46777 RVA: 0x0043E24D File Offset: 0x0043C44D
		[JsonProperty(PropertyName = "balance_before")]
		[Description("the balance before the transaction")]
		public int BalanceBefore { get; set; }

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x0600B6BA RID: 46778 RVA: 0x0043E256 File Offset: 0x0043C456
		// (set) Token: 0x0600B6BB RID: 46779 RVA: 0x0043E25E File Offset: 0x0043C45E
		[JsonProperty(PropertyName = "balance_after")]
		[Description("the balance after the transaction")]
		public int BalanceAfter { get; set; }

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x0600B6BC RID: 46780 RVA: 0x0043E267 File Offset: 0x0043C467
		// (set) Token: 0x0600B6BD RID: 46781 RVA: 0x0043E26F File Offset: 0x0043C46F
		[JsonProperty(PropertyName = "total_spent")]
		[Description("The total skill points spent on the character after the transaction")]
		public int TotalSpent { get; set; }

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x0600B6BE RID: 46782 RVA: 0x0043E278 File Offset: 0x0043C478
		// (set) Token: 0x0600B6BF RID: 46783 RVA: 0x0043E280 File Offset: 0x0043C480
		[JsonProperty(PropertyName = "sp_source")]
		[Description("What moved the skill points")]
		public string SPSource { get; set; }

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x0600B6C0 RID: 46784 RVA: 0x0043E289 File Offset: 0x0043C489
		// (set) Token: 0x0600B6C1 RID: 46785 RVA: 0x0043E291 File Offset: 0x0043C491
		[JsonProperty(PropertyName = "sp_target")]
		[Description("Where the player spent skill points")]
		public string SPTarget { get; set; }

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x0600B6C2 RID: 46786 RVA: 0x0043E29A File Offset: 0x0043C49A
		// (set) Token: 0x0600B6C3 RID: 46787 RVA: 0x0043E2A2 File Offset: 0x0043C4A2
		[JsonProperty(PropertyName = "sp_target_detail")]
		[Description("The specific cause of this skill point change")]
		public string SPTargetDetail { get; set; }

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x0600B6C4 RID: 46788 RVA: 0x0043E2AB File Offset: 0x0043C4AB
		// (set) Token: 0x0600B6C5 RID: 46789 RVA: 0x0043E2B3 File Offset: 0x0043C4B3
		[JsonProperty(PropertyName = "sp_target_level")]
		[Description("The level of the source after spending the points.")]
		public int? SPTargerLevel { get; set; }

		// Token: 0x040088A9 RID: 34985
		[JsonProperty(PropertyName = "player_level")]
		[Description("The current level of the player after transaction")]
		public int PlayerLevel;
	}
}
