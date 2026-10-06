using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B5 RID: 5813
	[EventTitle("QuestAdded")]
	public class QuestAddedEventData : BaseEventData
	{
		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x0600B610 RID: 46608 RVA: 0x0043BB20 File Offset: 0x00439D20
		public override string EventType
		{
			get
			{
				return "quest_added";
			}
		}

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x0600B611 RID: 46609 RVA: 0x0043BB27 File Offset: 0x00439D27
		// (set) Token: 0x0600B612 RID: 46610 RVA: 0x0043BB2F File Offset: 0x00439D2F
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x0600B613 RID: 46611 RVA: 0x0043BB38 File Offset: 0x00439D38
		// (set) Token: 0x0600B614 RID: 46612 RVA: 0x0043BB40 File Offset: 0x00439D40
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x0600B615 RID: 46613 RVA: 0x0043BB49 File Offset: 0x00439D49
		// (set) Token: 0x0600B616 RID: 46614 RVA: 0x0043BB51 File Offset: 0x00439D51
		[JsonProperty(PropertyName = "add_source")]
		[Description("How the player acquired this new quest.")]
		public string AddSource { get; set; }

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x0600B617 RID: 46615 RVA: 0x0043BB5A File Offset: 0x00439D5A
		// (set) Token: 0x0600B618 RID: 46616 RVA: 0x0043BB62 File Offset: 0x00439D62
		[JsonProperty(PropertyName = "quest_instance_id")]
		[Description("Primary join key — unique per accepted instance; identical across quest_started → quest_progression → quest_reward.")]
		public string QuestInstanceId { get; set; }

		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x0600B619 RID: 46617 RVA: 0x0043BB6B File Offset: 0x00439D6B
		// (set) Token: 0x0600B61A RID: 46618 RVA: 0x0043BB73 File Offset: 0x00439D73
		[JsonProperty(PropertyName = "quest_id")]
		public string QuestId { get; set; }

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x0600B61B RID: 46619 RVA: 0x0043BB7C File Offset: 0x00439D7C
		// (set) Token: 0x0600B61C RID: 46620 RVA: 0x0043BB84 File Offset: 0x00439D84
		[JsonProperty(PropertyName = "quest_name")]
		[Description("English localization of the quest name.")]
		public string QuestName { get; set; }

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x0600B61D RID: 46621 RVA: 0x0043BB8D File Offset: 0x00439D8D
		// (set) Token: 0x0600B61E RID: 46622 RVA: 0x0043BB95 File Offset: 0x00439D95
		[JsonProperty(PropertyName = "quest_group")]
		public string QuestGroup { get; set; }

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x0600B61F RID: 46623 RVA: 0x0043BB9E File Offset: 0x00439D9E
		// (set) Token: 0x0600B620 RID: 46624 RVA: 0x0043BBA6 File Offset: 0x00439DA6
		[JsonProperty(PropertyName = "quest_category")]
		[Description("The category of the quest.")]
		public string QuestCategory { get; set; }

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x0600B621 RID: 46625 RVA: 0x0043BBAF File Offset: 0x00439DAF
		// (set) Token: 0x0600B622 RID: 46626 RVA: 0x0043BBB7 File Offset: 0x00439DB7
		[JsonProperty(PropertyName = "quest_tier")]
		[Description("Quest difficulty tier.")]
		public int? QuestTier { get; set; }

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x0600B623 RID: 46627 RVA: 0x0043BBC0 File Offset: 0x00439DC0
		// (set) Token: 0x0600B624 RID: 46628 RVA: 0x0043BBC8 File Offset: 0x00439DC8
		[JsonProperty(PropertyName = "quest_tags")]
		[Description("Quest difficulty tier.")]
		public IEnumerable<string> QuestTags { get; set; }

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x0600B625 RID: 46629 RVA: 0x0043BBD1 File Offset: 0x00439DD1
		// (set) Token: 0x0600B626 RID: 46630 RVA: 0x0043BBD9 File Offset: 0x00439DD9
		[JsonProperty(PropertyName = "is_shareable")]
		public bool IsShareable { get; set; }

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x0600B627 RID: 46631 RVA: 0x0043BBE2 File Offset: 0x00439DE2
		// (set) Token: 0x0600B628 RID: 46632 RVA: 0x0043BBEA File Offset: 0x00439DEA
		[JsonProperty(PropertyName = "completion_type")]
		public string CompletionType { get; set; }

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x0600B629 RID: 46633 RVA: 0x0043BBF3 File Offset: 0x00439DF3
		// (set) Token: 0x0600B62A RID: 46634 RVA: 0x0043BBFB File Offset: 0x00439DFB
		[JsonProperty(PropertyName = "quest_giver_entity_id")]
		[Description("ID of the trader giving the quest\n\nNULL for challenges and treasure.")]
		public string QuestGiverEntityId { get; set; }

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x0600B62B RID: 46635 RVA: 0x0043BC04 File Offset: 0x00439E04
		// (set) Token: 0x0600B62C RID: 46636 RVA: 0x0043BC0C File Offset: 0x00439E0C
		[JsonProperty(PropertyName = "quest_owner_provider_id")]
		[KrakenIdTransform("$.quest_owner_provider_id")]
		[Description("data_provider_user_id of the player who acquired the quest")]
		public string QuestOwnerProviderId { get; set; }

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x0600B62D RID: 46637 RVA: 0x0043BC15 File Offset: 0x00439E15
		// (set) Token: 0x0600B62E RID: 46638 RVA: 0x0043BC1D File Offset: 0x00439E1D
		[JsonProperty(PropertyName = "trader_quest_tier")]
		[Description("The current tier level of that trader. NULL for non trader quests.")]
		public int? TraderQuestTier { get; set; }

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x0600B62F RID: 46639 RVA: 0x0043BC26 File Offset: 0x00439E26
		// (set) Token: 0x0600B630 RID: 46640 RVA: 0x0043BC2E File Offset: 0x00439E2E
		[JsonProperty(PropertyName = "offered_quests")]
		[Description("Trader's quest list at the moment of selection, their tier and their distance")]
		public IEnumerable<QuestAddedEventData.OfferedQuest> OfferedQuests { get; set; }

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x0600B631 RID: 46641 RVA: 0x0043BC37 File Offset: 0x00439E37
		// (set) Token: 0x0600B632 RID: 46642 RVA: 0x0043BC3F File Offset: 0x00439E3F
		[JsonProperty(PropertyName = "quest_distance")]
		[Description("the distance of the quest at the time of adding it in the journal in meters. Mainly for trader quests.")]
		public float? QuestDistance { get; set; }

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x0600B633 RID: 46643 RVA: 0x0043BC48 File Offset: 0x00439E48
		// (set) Token: 0x0600B634 RID: 46644 RVA: 0x0043BC50 File Offset: 0x00439E50
		[JsonProperty(PropertyName = "poi_assigned")]
		[Description("the name of the associated POI")]
		public string PoiAssigned { get; set; }

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x0600B635 RID: 46645 RVA: 0x0043BC59 File Offset: 0x00439E59
		// (set) Token: 0x0600B636 RID: 46646 RVA: 0x0043BC61 File Offset: 0x00439E61
		[JsonProperty(PropertyName = "objective_count")]
		[Description("The number of objectives within the quest.")]
		public int ObjectiveCount { get; set; }

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x0600B637 RID: 46647 RVA: 0x0043BC6A File Offset: 0x00439E6A
		// (set) Token: 0x0600B638 RID: 46648 RVA: 0x0043BC72 File Offset: 0x00439E72
		[JsonProperty(PropertyName = "active_quest_count")]
		[Description("The number of quests currently in the player's journal.")]
		public int ActiveQuestCount { get; set; }

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x0600B639 RID: 46649 RVA: 0x0043BC7B File Offset: 0x00439E7B
		// (set) Token: 0x0600B63A RID: 46650 RVA: 0x0043BC83 File Offset: 0x00439E83
		[JsonProperty(PropertyName = "total_completed_quest_count")]
		[Description("The number of quests currently in the player's journal.")]
		public int TotalCompletedQuestCount { get; set; }

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x0600B63B RID: 46651 RVA: 0x0043BC8C File Offset: 0x00439E8C
		// (set) Token: 0x0600B63C RID: 46652 RVA: 0x0043BC94 File Offset: 0x00439E94
		[JsonProperty(PropertyName = "total_quest_count")]
		[Description("The number of quests currently in the player's journal.")]
		public int TotalQuestCount { get; set; }

		// Token: 0x0600B63D RID: 46653 RVA: 0x0043BCA0 File Offset: 0x00439EA0
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

		// Token: 0x0400883F RID: 34879
		[JsonProperty(PropertyName = "quest_added_ts")]
		[DateTimeFormat]
		public string QuestAddedTimestamp;

		// Token: 0x020016B6 RID: 5814
		public struct OfferedQuest
		{
			// Token: 0x04008854 RID: 34900
			public string quest_id;

			// Token: 0x04008855 RID: 34901
			public int quest_tier;

			// Token: 0x04008856 RID: 34902
			public float distance;
		}
	}
}
