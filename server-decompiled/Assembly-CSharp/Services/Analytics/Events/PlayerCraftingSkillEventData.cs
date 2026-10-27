using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016C1 RID: 5825
	[EventTitle("CraftingSkillEvent")]
	public class PlayerCraftingSkillEventData : BaseEventData
	{
		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x0600B603 RID: 46595 RVA: 0x0043DC6C File Offset: 0x0043BE6C
		public override string EventType
		{
			get
			{
				return "player_crafting_skill_progression";
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x0600B604 RID: 46596 RVA: 0x0043DC73 File Offset: 0x0043BE73
		// (set) Token: 0x0600B605 RID: 46597 RVA: 0x0043DC7B File Offset: 0x0043BE7B
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server")]
		public string ServerId { get; set; }

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x0600B606 RID: 46598 RVA: 0x0043DC84 File Offset: 0x0043BE84
		// (set) Token: 0x0600B607 RID: 46599 RVA: 0x0043DC8C File Offset: 0x0043BE8C
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x0600B608 RID: 46600 RVA: 0x0043DC95 File Offset: 0x0043BE95
		// (set) Token: 0x0600B609 RID: 46601 RVA: 0x0043DC9D File Offset: 0x0043BE9D
		[JsonProperty(PropertyName = "transaction_ts")]
		[DateTimeFormat]
		[Description("Whenever the player uses or gain the server")]
		public string TransactionTimestamp { get; set; }

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x0600B60A RID: 46602 RVA: 0x0043DCA6 File Offset: 0x0043BEA6
		// (set) Token: 0x0600B60B RID: 46603 RVA: 0x0043DCAE File Offset: 0x0043BEAE
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time of trigger")]
		public int ElapsedTime { get; set; }

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x0600B60C RID: 46604 RVA: 0x0043DCB7 File Offset: 0x0043BEB7
		// (set) Token: 0x0600B60D RID: 46605 RVA: 0x0043DCBF File Offset: 0x0043BEBF
		[JsonProperty(PropertyName = "player_level")]
		[Description("The current level of the player after transaction")]
		public int PlayerLevel { get; set; }

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x0600B60E RID: 46606 RVA: 0x0043DCC8 File Offset: 0x0043BEC8
		// (set) Token: 0x0600B60F RID: 46607 RVA: 0x0043DCD0 File Offset: 0x0043BED0
		[JsonProperty(PropertyName = "in_game_days")]
		[Description("The current in-game days of the world at the time of the transaction")]
		public int InGameDays { get; set; }

		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x0600B610 RID: 46608 RVA: 0x0043DCD9 File Offset: 0x0043BED9
		// (set) Token: 0x0600B611 RID: 46609 RVA: 0x0043DCE1 File Offset: 0x0043BEE1
		[JsonProperty(PropertyName = "crafting_skill")]
		[Description("The crafting skill track being levelled up")]
		public string CraftingSkill { get; set; }

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x0600B612 RID: 46610 RVA: 0x0043DCEA File Offset: 0x0043BEEA
		// (set) Token: 0x0600B613 RID: 46611 RVA: 0x0043DCF2 File Offset: 0x0043BEF2
		[JsonProperty(PropertyName = "source_type")]
		[Description("The crafting skill track being levelled up")]
		public string SourceType { get; set; }

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x0600B614 RID: 46612 RVA: 0x0043DCFB File Offset: 0x0043BEFB
		// (set) Token: 0x0600B615 RID: 46613 RVA: 0x0043DD03 File Offset: 0x0043BF03
		[JsonProperty(PropertyName = "skill_level_before")]
		[Description("The current in-game days of the world at the time of the transaction")]
		public int SkillLevelBefore { get; set; }

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x0600B616 RID: 46614 RVA: 0x0043DD0C File Offset: 0x0043BF0C
		// (set) Token: 0x0600B617 RID: 46615 RVA: 0x0043DD14 File Offset: 0x0043BF14
		[JsonProperty(PropertyName = "skill_level_after")]
		[Description("The current in-game days of the world at the time of the transaction")]
		public int SkillLevelAfter { get; set; }
	}
}
