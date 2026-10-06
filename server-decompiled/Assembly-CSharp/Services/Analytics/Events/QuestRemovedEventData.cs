using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B8 RID: 5816
	[EventTitle("QuestRemoved")]
	public class QuestRemovedEventData : BaseEventData
	{
		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x0600B65C RID: 46684 RVA: 0x0043BF86 File Offset: 0x0043A186
		public override string EventType
		{
			get
			{
				return "quest_removed";
			}
		}

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x0600B65D RID: 46685 RVA: 0x0043BF8D File Offset: 0x0043A18D
		// (set) Token: 0x0600B65E RID: 46686 RVA: 0x0043BF95 File Offset: 0x0043A195
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x0600B65F RID: 46687 RVA: 0x0043BF9E File Offset: 0x0043A19E
		// (set) Token: 0x0600B660 RID: 46688 RVA: 0x0043BFA6 File Offset: 0x0043A1A6
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x0600B661 RID: 46689 RVA: 0x0043BFAF File Offset: 0x0043A1AF
		// (set) Token: 0x0600B662 RID: 46690 RVA: 0x0043BFB7 File Offset: 0x0043A1B7
		[JsonProperty(PropertyName = "quest_removed_ts")]
		[DateTimeFormat]
		public string QuestRemovedTimestamp { get; set; }

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x0600B663 RID: 46691 RVA: 0x0043BFC0 File Offset: 0x0043A1C0
		// (set) Token: 0x0600B664 RID: 46692 RVA: 0x0043BFC8 File Offset: 0x0043A1C8
		[JsonProperty(PropertyName = "remove_reason")]
		public string RemoveReason { get; set; }

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x0600B665 RID: 46693 RVA: 0x0043BFD1 File Offset: 0x0043A1D1
		// (set) Token: 0x0600B666 RID: 46694 RVA: 0x0043BFD9 File Offset: 0x0043A1D9
		[JsonProperty(PropertyName = "quest_instance_id")]
		[Description("Primary join key — unique per accepted instance; identical across quest_started → quest_progression → quest_reward.")]
		public string QuestInstanceId { get; set; }

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x0600B667 RID: 46695 RVA: 0x0043BFE2 File Offset: 0x0043A1E2
		// (set) Token: 0x0600B668 RID: 46696 RVA: 0x0043BFEA File Offset: 0x0043A1EA
		[JsonProperty(PropertyName = "quest_id")]
		public string QuestId { get; set; }

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x0600B669 RID: 46697 RVA: 0x0043BFF3 File Offset: 0x0043A1F3
		// (set) Token: 0x0600B66A RID: 46698 RVA: 0x0043BFFB File Offset: 0x0043A1FB
		[JsonProperty(PropertyName = "quest_name")]
		[Description("English localization of the quest name.")]
		public string QuestName { get; set; }

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x0600B66B RID: 46699 RVA: 0x0043C004 File Offset: 0x0043A204
		// (set) Token: 0x0600B66C RID: 46700 RVA: 0x0043C00C File Offset: 0x0043A20C
		[JsonProperty(PropertyName = "quest_group")]
		public string QuestGroup { get; set; }

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x0600B66D RID: 46701 RVA: 0x0043C015 File Offset: 0x0043A215
		// (set) Token: 0x0600B66E RID: 46702 RVA: 0x0043C01D File Offset: 0x0043A21D
		[JsonProperty(PropertyName = "quest_category")]
		[Description("The category of the quest.")]
		public string QuestCategory { get; set; }

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x0600B66F RID: 46703 RVA: 0x0043C026 File Offset: 0x0043A226
		// (set) Token: 0x0600B670 RID: 46704 RVA: 0x0043C02E File Offset: 0x0043A22E
		[JsonProperty(PropertyName = "quest_tier")]
		[Description("Quest difficulty tier.")]
		public int? QuestTier { get; set; }

		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x0600B671 RID: 46705 RVA: 0x0043C037 File Offset: 0x0043A237
		// (set) Token: 0x0600B672 RID: 46706 RVA: 0x0043C03F File Offset: 0x0043A23F
		[JsonProperty(PropertyName = "quest_owner_provider_id")]
		[KrakenIdTransform("$.quest_owner_provider_id")]
		[Description("data_provider_user_id of the player who acquired the quest")]
		public string QuestOwnerProviderId { get; set; }
	}
}
