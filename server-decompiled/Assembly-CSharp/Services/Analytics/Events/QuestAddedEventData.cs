using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016CF RID: 5839
	[EventTitle("QuestAdded")]
	public class QuestAddedEventData : BaseEventData
	{
		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x0600B6D9 RID: 46809 RVA: 0x0043E34B File Offset: 0x0043C54B
		public override string EventType
		{
			get
			{
				return "quest_added";
			}
		}

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x0600B6DA RID: 46810 RVA: 0x0043E352 File Offset: 0x0043C552
		// (set) Token: 0x0600B6DB RID: 46811 RVA: 0x0043E35A File Offset: 0x0043C55A
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x0600B6DC RID: 46812 RVA: 0x0043E363 File Offset: 0x0043C563
		// (set) Token: 0x0600B6DD RID: 46813 RVA: 0x0043E36B File Offset: 0x0043C56B
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x0600B6DE RID: 46814 RVA: 0x0043E374 File Offset: 0x0043C574
		// (set) Token: 0x0600B6DF RID: 46815 RVA: 0x0043E37C File Offset: 0x0043C57C
		[JsonProperty(PropertyName = "add_source")]
		[Description("How the player acquired this new quest.")]
		public string AddSource { get; set; }

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x0600B6E0 RID: 46816 RVA: 0x0043E385 File Offset: 0x0043C585
		// (set) Token: 0x0600B6E1 RID: 46817 RVA: 0x0043E38D File Offset: 0x0043C58D
		[JsonProperty(PropertyName = "quest_instance_id")]
		[Description("Primary join key — unique per accepted instance; identical across quest_started → quest_progression → quest_reward.")]
		public string QuestInstanceId { get; set; }

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x0600B6E2 RID: 46818 RVA: 0x0043E396 File Offset: 0x0043C596
		// (set) Token: 0x0600B6E3 RID: 46819 RVA: 0x0043E39E File Offset: 0x0043C59E
		[JsonProperty(PropertyName = "quest_id")]
		public string QuestId { get; set; }

		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x0600B6E4 RID: 46820 RVA: 0x0043E3A7 File Offset: 0x0043C5A7
		// (set) Token: 0x0600B6E5 RID: 46821 RVA: 0x0043E3AF File Offset: 0x0043C5AF
		[JsonProperty(PropertyName = "quest_name")]
		[Description("English localization of the quest name.")]
		public string QuestName { get; set; }

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x0600B6E6 RID: 46822 RVA: 0x0043E3B8 File Offset: 0x0043C5B8
		// (set) Token: 0x0600B6E7 RID: 46823 RVA: 0x0043E3C0 File Offset: 0x0043C5C0
		[JsonProperty(PropertyName = "quest_group")]
		public string QuestGroup { get; set; }

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x0600B6E8 RID: 46824 RVA: 0x0043E3C9 File Offset: 0x0043C5C9
		// (set) Token: 0x0600B6E9 RID: 46825 RVA: 0x0043E3D1 File Offset: 0x0043C5D1
		[JsonProperty(PropertyName = "quest_category")]
		[Description("The category of the quest.")]
		public string QuestCategory { get; set; }

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x0600B6EA RID: 46826 RVA: 0x0043E3DA File Offset: 0x0043C5DA
		// (set) Token: 0x0600B6EB RID: 46827 RVA: 0x0043E3E2 File Offset: 0x0043C5E2
		[JsonProperty(PropertyName = "quest_tier")]
		[Description("Quest difficulty tier.")]
		public int? QuestTier { get; set; }

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x0600B6EC RID: 46828 RVA: 0x0043E3EB File Offset: 0x0043C5EB
		// (set) Token: 0x0600B6ED RID: 46829 RVA: 0x0043E3F3 File Offset: 0x0043C5F3
		[JsonProperty(PropertyName = "quest_tags")]
		[Description("Quest difficulty tier.")]
		public IEnumerable<string> QuestTags { get; set; }

		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x0600B6EE RID: 46830 RVA: 0x0043E3FC File Offset: 0x0043C5FC
		// (set) Token: 0x0600B6EF RID: 46831 RVA: 0x0043E404 File Offset: 0x0043C604
		[JsonProperty(PropertyName = "is_shareable")]
		public bool IsShareable { get; set; }

		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x0600B6F0 RID: 46832 RVA: 0x0043E40D File Offset: 0x0043C60D
		// (set) Token: 0x0600B6F1 RID: 46833 RVA: 0x0043E415 File Offset: 0x0043C615
		[JsonProperty(PropertyName = "completion_type")]
		public string CompletionType { get; set; }

		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x0600B6F2 RID: 46834 RVA: 0x0043E41E File Offset: 0x0043C61E
		// (set) Token: 0x0600B6F3 RID: 46835 RVA: 0x0043E426 File Offset: 0x0043C626
		[JsonProperty(PropertyName = "quest_giver_entity_id")]
		[Description("ID of the trader giving the quest\n\nNULL for challenges and treasure.")]
		public string QuestGiverEntityId { get; set; }

		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x0600B6F4 RID: 46836 RVA: 0x0043E42F File Offset: 0x0043C62F
		// (set) Token: 0x0600B6F5 RID: 46837 RVA: 0x0043E437 File Offset: 0x0043C637
		[JsonProperty(PropertyName = "quest_owner_provider_id")]
		[KrakenIdTransform("$.quest_owner_provider_id")]
		[Description("data_provider_user_id of the player who acquired the quest")]
		public string QuestOwnerProviderId { get; set; }

		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x0600B6F6 RID: 46838 RVA: 0x0043E440 File Offset: 0x0043C640
		// (set) Token: 0x0600B6F7 RID: 46839 RVA: 0x0043E448 File Offset: 0x0043C648
		[JsonProperty(PropertyName = "trader_quest_tier")]
		[Description("The current tier level of that trader. NULL for non trader quests.")]
		public int? TraderQuestTier { get; set; }

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x0600B6F8 RID: 46840 RVA: 0x0043E451 File Offset: 0x0043C651
		// (set) Token: 0x0600B6F9 RID: 46841 RVA: 0x0043E459 File Offset: 0x0043C659
		[JsonProperty(PropertyName = "offered_quests")]
		[Description("Trader's quest list at the moment of selection, their tier and their distance")]
		public IEnumerable<QuestAddedEventData.OfferedQuest> OfferedQuests { get; set; }

		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x0600B6FA RID: 46842 RVA: 0x0043E462 File Offset: 0x0043C662
		// (set) Token: 0x0600B6FB RID: 46843 RVA: 0x0043E46A File Offset: 0x0043C66A
		[JsonProperty(PropertyName = "quest_distance")]
		[Description("the distance of the quest at the time of adding it in the journal in meters. Mainly for trader quests.")]
		public float? QuestDistance { get; set; }

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x0600B6FC RID: 46844 RVA: 0x0043E473 File Offset: 0x0043C673
		// (set) Token: 0x0600B6FD RID: 46845 RVA: 0x0043E47B File Offset: 0x0043C67B
		[JsonProperty(PropertyName = "poi_assigned")]
		[Description("the name of the associated POI")]
		public string PoiAssigned { get; set; }

		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x0600B6FE RID: 46846 RVA: 0x0043E484 File Offset: 0x0043C684
		// (set) Token: 0x0600B6FF RID: 46847 RVA: 0x0043E48C File Offset: 0x0043C68C
		[JsonProperty(PropertyName = "objective_count")]
		[Description("The number of objectives within the quest.")]
		public int ObjectiveCount { get; set; }

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x0600B700 RID: 46848 RVA: 0x0043E495 File Offset: 0x0043C695
		// (set) Token: 0x0600B701 RID: 46849 RVA: 0x0043E49D File Offset: 0x0043C69D
		[JsonProperty(PropertyName = "active_quest_count")]
		[Description("The number of quests currently in the player's journal.")]
		public int ActiveQuestCount { get; set; }

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x0600B702 RID: 46850 RVA: 0x0043E4A6 File Offset: 0x0043C6A6
		// (set) Token: 0x0600B703 RID: 46851 RVA: 0x0043E4AE File Offset: 0x0043C6AE
		[JsonProperty(PropertyName = "total_completed_quest_count")]
		[Description("The number of quests currently in the player's journal.")]
		public int TotalCompletedQuestCount { get; set; }

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x0600B704 RID: 46852 RVA: 0x0043E4B7 File Offset: 0x0043C6B7
		// (set) Token: 0x0600B705 RID: 46853 RVA: 0x0043E4BF File Offset: 0x0043C6BF
		[JsonProperty(PropertyName = "total_quest_count")]
		[Description("The number of quests currently in the player's journal.")]
		public int TotalQuestCount { get; set; }

		// Token: 0x0600B706 RID: 46854 RVA: 0x0043E4C8 File Offset: 0x0043C6C8
		public string QuestSourceToString(Quest.QuestSource source)
		{
			string text;
			switch (source)
			{
			case Quest.QuestSource.Debug:
				text = "debug";
				break;
			case Quest.QuestSource.ChallengeSystem:
				text = "challenge_system";
				break;
			case Quest.QuestSource.QuestSystem:
				text = "quest_system";
				break;
			case Quest.QuestSource.TreasureMap:
				text = "treasure_map_item";
				break;
			case Quest.QuestSource.Trader:
				text = "trader_list";
				break;
			case Quest.QuestSource.PartyShare:
				text = "party_share";
				break;
			case Quest.QuestSource.Load:
				text = "load";
				break;
			default:
				throw new ArgumentOutOfRangeException("source", source, null);
			}
			return text;
		}

		// Token: 0x040088CB RID: 35019
		[JsonProperty(PropertyName = "quest_added_ts")]
		[DateTimeFormat]
		public string QuestAddedTimestamp;

		// Token: 0x020016D0 RID: 5840
		public struct OfferedQuest
		{
			// Token: 0x040088E0 RID: 35040
			public string quest_id;

			// Token: 0x040088E1 RID: 35041
			public int quest_tier;

			// Token: 0x040088E2 RID: 35042
			public float distance;
		}
	}
}
