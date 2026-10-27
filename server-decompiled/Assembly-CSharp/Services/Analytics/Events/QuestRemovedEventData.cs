using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016D2 RID: 5842
	[EventTitle("QuestRemoved")]
	public class QuestRemovedEventData : BaseEventData
	{
		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x0600B725 RID: 46885 RVA: 0x0043E7AE File Offset: 0x0043C9AE
		public override string EventType
		{
			get
			{
				return "quest_removed";
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x0600B726 RID: 46886 RVA: 0x0043E7B5 File Offset: 0x0043C9B5
		// (set) Token: 0x0600B727 RID: 46887 RVA: 0x0043E7BD File Offset: 0x0043C9BD
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x0600B728 RID: 46888 RVA: 0x0043E7C6 File Offset: 0x0043C9C6
		// (set) Token: 0x0600B729 RID: 46889 RVA: 0x0043E7CE File Offset: 0x0043C9CE
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x0600B72A RID: 46890 RVA: 0x0043E7D7 File Offset: 0x0043C9D7
		// (set) Token: 0x0600B72B RID: 46891 RVA: 0x0043E7DF File Offset: 0x0043C9DF
		[JsonProperty(PropertyName = "quest_removed_ts")]
		[DateTimeFormat]
		public string QuestRemovedTimestamp { get; set; }

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x0600B72C RID: 46892 RVA: 0x0043E7E8 File Offset: 0x0043C9E8
		// (set) Token: 0x0600B72D RID: 46893 RVA: 0x0043E7F0 File Offset: 0x0043C9F0
		[JsonProperty(PropertyName = "remove_reason")]
		public string RemoveReason { get; set; }

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x0600B72E RID: 46894 RVA: 0x0043E7F9 File Offset: 0x0043C9F9
		// (set) Token: 0x0600B72F RID: 46895 RVA: 0x0043E801 File Offset: 0x0043CA01
		[JsonProperty(PropertyName = "quest_instance_id")]
		[Description("Primary join key — unique per accepted instance; identical across quest_started → quest_progression → quest_reward.")]
		public string QuestInstanceId { get; set; }

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x0600B730 RID: 46896 RVA: 0x0043E80A File Offset: 0x0043CA0A
		// (set) Token: 0x0600B731 RID: 46897 RVA: 0x0043E812 File Offset: 0x0043CA12
		[JsonProperty(PropertyName = "quest_id")]
		public string QuestId { get; set; }

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x0600B732 RID: 46898 RVA: 0x0043E81B File Offset: 0x0043CA1B
		// (set) Token: 0x0600B733 RID: 46899 RVA: 0x0043E823 File Offset: 0x0043CA23
		[JsonProperty(PropertyName = "quest_name")]
		[Description("English localization of the quest name.")]
		public string QuestName { get; set; }

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x0600B734 RID: 46900 RVA: 0x0043E82C File Offset: 0x0043CA2C
		// (set) Token: 0x0600B735 RID: 46901 RVA: 0x0043E834 File Offset: 0x0043CA34
		[JsonProperty(PropertyName = "quest_group")]
		public string QuestGroup { get; set; }

		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x0600B736 RID: 46902 RVA: 0x0043E83D File Offset: 0x0043CA3D
		// (set) Token: 0x0600B737 RID: 46903 RVA: 0x0043E845 File Offset: 0x0043CA45
		[JsonProperty(PropertyName = "quest_category")]
		[Description("The category of the quest.")]
		public string QuestCategory { get; set; }

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x0600B738 RID: 46904 RVA: 0x0043E84E File Offset: 0x0043CA4E
		// (set) Token: 0x0600B739 RID: 46905 RVA: 0x0043E856 File Offset: 0x0043CA56
		[JsonProperty(PropertyName = "quest_tier")]
		[Description("Quest difficulty tier.")]
		public int? QuestTier { get; set; }

		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x0600B73A RID: 46906 RVA: 0x0043E85F File Offset: 0x0043CA5F
		// (set) Token: 0x0600B73B RID: 46907 RVA: 0x0043E867 File Offset: 0x0043CA67
		[JsonProperty(PropertyName = "quest_owner_provider_id")]
		[KrakenIdTransform("$.quest_owner_provider_id")]
		[Description("data_provider_user_id of the player who acquired the quest")]
		public string QuestOwnerProviderId { get; set; }
	}
}
