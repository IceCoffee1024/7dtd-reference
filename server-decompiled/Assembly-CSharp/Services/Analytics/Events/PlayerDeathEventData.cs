using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016C2 RID: 5826
	[EventTitle("QuestAdded")]
	public class PlayerDeathEventData : BaseEventData
	{
		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x0600B619 RID: 46617 RVA: 0x0043DD1D File Offset: 0x0043BF1D
		public override string EventType
		{
			get
			{
				return "player_death";
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x0600B61A RID: 46618 RVA: 0x0043DD24 File Offset: 0x0043BF24
		// (set) Token: 0x0600B61B RID: 46619 RVA: 0x0043DD2C File Offset: 0x0043BF2C
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x0600B61C RID: 46620 RVA: 0x0043DD35 File Offset: 0x0043BF35
		// (set) Token: 0x0600B61D RID: 46621 RVA: 0x0043DD3D File Offset: 0x0043BF3D
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x0600B61E RID: 46622 RVA: 0x0043DD46 File Offset: 0x0043BF46
		// (set) Token: 0x0600B61F RID: 46623 RVA: 0x0043DD4E File Offset: 0x0043BF4E
		[JsonProperty(PropertyName = "death_ts")]
		[Description("Time of event trigger")]
		[DateTimeFormat]
		public string DeathTimestamp { get; set; }

		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x0600B620 RID: 46624 RVA: 0x0043DD57 File Offset: 0x0043BF57
		// (set) Token: 0x0600B621 RID: 46625 RVA: 0x0043DD5F File Offset: 0x0043BF5F
		[JsonProperty(PropertyName = "elapsed_time")]
		public uint ElapsedTime { get; set; }

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x0600B622 RID: 46626 RVA: 0x0043DD68 File Offset: 0x0043BF68
		// (set) Token: 0x0600B623 RID: 46627 RVA: 0x0043DD70 File Offset: 0x0043BF70
		[JsonProperty(PropertyName = "cause_of_death")]
		public string CauseOfDeath { get; set; }

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x0600B624 RID: 46628 RVA: 0x0043DD79 File Offset: 0x0043BF79
		// (set) Token: 0x0600B625 RID: 46629 RVA: 0x0043DD81 File Offset: 0x0043BF81
		[JsonProperty(PropertyName = "cause_of_death_provider_id")]
		[KrakenIdTransform("$.cause_of_death_provider_id")]
		[Description("If cause of death is another player, this is the player's provider id, otherwise this field is null")]
		public string CauseOfDeathProviderId { get; set; }

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x0600B626 RID: 46630 RVA: 0x0043DD8A File Offset: 0x0043BF8A
		// (set) Token: 0x0600B627 RID: 46631 RVA: 0x0043DD92 File Offset: 0x0043BF92
		[JsonProperty(PropertyName = "damage_taken")]
		[Description("Damage value of ‘last hit'")]
		public int DamageTaken { get; set; }

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x0600B628 RID: 46632 RVA: 0x0043DD9B File Offset: 0x0043BF9B
		// (set) Token: 0x0600B629 RID: 46633 RVA: 0x0043DDA3 File Offset: 0x0043BFA3
		[JsonProperty(PropertyName = "count_foes_engaged")]
		public int CountFoesEngaged { get; set; }

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x0600B62A RID: 46634 RVA: 0x0043DDAC File Offset: 0x0043BFAC
		// (set) Token: 0x0600B62B RID: 46635 RVA: 0x0043DDB4 File Offset: 0x0043BFB4
		[JsonProperty(PropertyName = "foes_engaged")]
		[Description("List of enemies currently in combat with the player at the time of death")]
		public object FoesEngaged { get; set; }

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x0600B62C RID: 46636 RVA: 0x0043DDBD File Offset: 0x0043BFBD
		// (set) Token: 0x0600B62D RID: 46637 RVA: 0x0043DDC5 File Offset: 0x0043BFC5
		[JsonProperty(PropertyName = "active_conditions")]
		public object ActiveConditions { get; set; }

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x0600B62E RID: 46638 RVA: 0x0043DDCE File Offset: 0x0043BFCE
		// (set) Token: 0x0600B62F RID: 46639 RVA: 0x0043DDD6 File Offset: 0x0043BFD6
		[JsonProperty(PropertyName = "player_state")]
		public object PlayerState { get; set; }

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x0600B630 RID: 46640 RVA: 0x0043DDDF File Offset: 0x0043BFDF
		// (set) Token: 0x0600B631 RID: 46641 RVA: 0x0043DDE7 File Offset: 0x0043BFE7
		[JsonProperty(PropertyName = "player_loc")]
		[Description("Coordinates of the player' location at the time of death.")]
		public string PlayerLocation { get; set; }

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x0600B632 RID: 46642 RVA: 0x0043DDF0 File Offset: 0x0043BFF0
		// (set) Token: 0x0600B633 RID: 46643 RVA: 0x0043DDF8 File Offset: 0x0043BFF8
		[JsonProperty(PropertyName = "biome_id")]
		[Description("Id of the biome where they died")]
		public string BiomeId { get; set; }

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x0600B634 RID: 46644 RVA: 0x0043DE01 File Offset: 0x0043C001
		// (set) Token: 0x0600B635 RID: 46645 RVA: 0x0043DE09 File Offset: 0x0043C009
		[JsonProperty(PropertyName = "biome_name")]
		[Description("name of the biome where they died")]
		public string BiomeName { get; set; }

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x0600B636 RID: 46646 RVA: 0x0043DE12 File Offset: 0x0043C012
		// (set) Token: 0x0600B637 RID: 46647 RVA: 0x0043DE1A File Offset: 0x0043C01A
		[JsonProperty(PropertyName = "poi_id")]
		[Description("POI ID if player died in a POI otherwise use biome location as shown in the HUD.")]
		public string PoiId { get; set; }

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x0600B638 RID: 46648 RVA: 0x0043DE23 File Offset: 0x0043C023
		// (set) Token: 0x0600B639 RID: 46649 RVA: 0x0043DE2B File Offset: 0x0043C02B
		[JsonProperty(PropertyName = "poi_name")]
		[Description("POI name if player died in a POI otherwise use biome location as shown in the HUD.")]
		public string PoiName { get; set; }

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x0600B63A RID: 46650 RVA: 0x0043DE34 File Offset: 0x0043C034
		// (set) Token: 0x0600B63B RID: 46651 RVA: 0x0043DE3C File Offset: 0x0043C03C
		[JsonProperty(PropertyName = "party_loc")]
		[Description("Coordinates of the player's teammates at the time of their death.")]
		public object PartyLoc { get; set; }

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x0600B63C RID: 46652 RVA: 0x0043DE45 File Offset: 0x0043C045
		// (set) Token: 0x0600B63D RID: 46653 RVA: 0x0043DE4D File Offset: 0x0043C04D
		[JsonProperty(PropertyName = "player_inventory")]
		[Description("What the player was holding in their backback at the time of death.")]
		public object PlayerInventory { get; set; }

		// Token: 0x0400885B RID: 34907
		public const float EngagedFoeRange = 10f;

		// Token: 0x0400886E RID: 34926
		[JsonProperty(PropertyName = "death_penalty")]
		[Description("Death penalty in settings")]
		public string DeathPenalty;

		// Token: 0x020016C3 RID: 5827
		public struct EngagedEnemyData
		{
			// Token: 0x17001660 RID: 5728
			// (get) Token: 0x0600B63F RID: 46655 RVA: 0x0043DE56 File Offset: 0x0043C056
			// (set) Token: 0x0600B640 RID: 46656 RVA: 0x0043DE5E File Offset: 0x0043C05E
			[JsonProperty(PropertyName = "entity_id")]
			public string EntityId { readonly get; set; }

			// Token: 0x17001661 RID: 5729
			// (get) Token: 0x0600B641 RID: 46657 RVA: 0x0043DE67 File Offset: 0x0043C067
			// (set) Token: 0x0600B642 RID: 46658 RVA: 0x0043DE6F File Offset: 0x0043C06F
			[JsonProperty(PropertyName = "count")]
			public int Count { readonly get; set; }
		}

		// Token: 0x020016C4 RID: 5828
		public struct ActiveConditionData
		{
			// Token: 0x17001662 RID: 5730
			// (get) Token: 0x0600B643 RID: 46659 RVA: 0x0043DE78 File Offset: 0x0043C078
			// (set) Token: 0x0600B644 RID: 46660 RVA: 0x0043DE80 File Offset: 0x0043C080
			[JsonProperty(PropertyName = "condition_name")]
			public string ConditionName { readonly get; set; }

			// Token: 0x17001663 RID: 5731
			// (get) Token: 0x0600B645 RID: 46661 RVA: 0x0043DE89 File Offset: 0x0043C089
			// (set) Token: 0x0600B646 RID: 46662 RVA: 0x0043DE91 File Offset: 0x0043C091
			[JsonProperty(PropertyName = "max_duration")]
			[Description("Max duration in seconds")]
			public float MaxDuration { readonly get; set; }

			// Token: 0x17001664 RID: 5732
			// (get) Token: 0x0600B647 RID: 46663 RVA: 0x0043DE9A File Offset: 0x0043C09A
			// (set) Token: 0x0600B648 RID: 46664 RVA: 0x0043DEA2 File Offset: 0x0043C0A2
			[JsonProperty(PropertyName = "current_duration")]
			[Description("Current duration in seconds")]
			public float CurrentDuration { readonly get; set; }

			// Token: 0x17001665 RID: 5733
			// (get) Token: 0x0600B649 RID: 46665 RVA: 0x0043DEAB File Offset: 0x0043C0AB
			// (set) Token: 0x0600B64A RID: 46666 RVA: 0x0043DEB3 File Offset: 0x0043C0B3
			[JsonProperty(PropertyName = "damage_type")]
			public string DamageType { readonly get; set; }

			// Token: 0x17001666 RID: 5734
			// (get) Token: 0x0600B64B RID: 46667 RVA: 0x0043DEBC File Offset: 0x0043C0BC
			// (set) Token: 0x0600B64C RID: 46668 RVA: 0x0043DEC4 File Offset: 0x0043C0C4
			[JsonProperty(PropertyName = "stack_type")]
			public string StackType { readonly get; set; }

			// Token: 0x17001667 RID: 5735
			// (get) Token: 0x0600B64D RID: 46669 RVA: 0x0043DECD File Offset: 0x0043C0CD
			// (set) Token: 0x0600B64E RID: 46670 RVA: 0x0043DED5 File Offset: 0x0043C0D5
			[JsonProperty(PropertyName = "update_rate")]
			[Description("Update rate in ticks")]
			public int UpdateRate { readonly get; set; }
		}

		// Token: 0x020016C5 RID: 5829
		public struct PlayerStateData
		{
			// Token: 0x17001668 RID: 5736
			// (get) Token: 0x0600B64F RID: 46671 RVA: 0x0043DEDE File Offset: 0x0043C0DE
			// (set) Token: 0x0600B650 RID: 46672 RVA: 0x0043DEE6 File Offset: 0x0043C0E6
			[JsonProperty(PropertyName = "max_stamina")]
			public float MaxStamina { readonly get; set; }

			// Token: 0x17001669 RID: 5737
			// (get) Token: 0x0600B651 RID: 46673 RVA: 0x0043DEEF File Offset: 0x0043C0EF
			// (set) Token: 0x0600B652 RID: 46674 RVA: 0x0043DEF7 File Offset: 0x0043C0F7
			[JsonProperty(PropertyName = "current_stamina")]
			public float CurrentStamina { readonly get; set; }

			// Token: 0x1700166A RID: 5738
			// (get) Token: 0x0600B653 RID: 46675 RVA: 0x0043DF00 File Offset: 0x0043C100
			// (set) Token: 0x0600B654 RID: 46676 RVA: 0x0043DF08 File Offset: 0x0043C108
			[JsonProperty(PropertyName = "max_health")]
			public float MaxHealth { readonly get; set; }

			// Token: 0x1700166B RID: 5739
			// (get) Token: 0x0600B655 RID: 46677 RVA: 0x0043DF11 File Offset: 0x0043C111
			// (set) Token: 0x0600B656 RID: 46678 RVA: 0x0043DF19 File Offset: 0x0043C119
			[JsonProperty(PropertyName = "current_health")]
			public float CurrentHealth { readonly get; set; }

			// Token: 0x1700166C RID: 5740
			// (get) Token: 0x0600B657 RID: 46679 RVA: 0x0043DF22 File Offset: 0x0043C122
			// (set) Token: 0x0600B658 RID: 46680 RVA: 0x0043DF2A File Offset: 0x0043C12A
			[JsonProperty(PropertyName = "max_water")]
			public float MaxWater { readonly get; set; }

			// Token: 0x1700166D RID: 5741
			// (get) Token: 0x0600B659 RID: 46681 RVA: 0x0043DF33 File Offset: 0x0043C133
			// (set) Token: 0x0600B65A RID: 46682 RVA: 0x0043DF3B File Offset: 0x0043C13B
			[JsonProperty(PropertyName = "current_water")]
			public float CurrentWater { readonly get; set; }

			// Token: 0x1700166E RID: 5742
			// (get) Token: 0x0600B65B RID: 46683 RVA: 0x0043DF44 File Offset: 0x0043C144
			// (set) Token: 0x0600B65C RID: 46684 RVA: 0x0043DF4C File Offset: 0x0043C14C
			[JsonProperty(PropertyName = "max_food")]
			public float MaxFood { readonly get; set; }

			// Token: 0x1700166F RID: 5743
			// (get) Token: 0x0600B65D RID: 46685 RVA: 0x0043DF55 File Offset: 0x0043C155
			// (set) Token: 0x0600B65E RID: 46686 RVA: 0x0043DF5D File Offset: 0x0043C15D
			[JsonProperty(PropertyName = "current_food")]
			public float CurrentFood { readonly get; set; }

			// Token: 0x17001670 RID: 5744
			// (get) Token: 0x0600B65F RID: 46687 RVA: 0x0043DF66 File Offset: 0x0043C166
			// (set) Token: 0x0600B660 RID: 46688 RVA: 0x0043DF6E File Offset: 0x0043C16E
			[JsonProperty(PropertyName = "temperature")]
			public float Temperature { readonly get; set; }
		}

		// Token: 0x020016C6 RID: 5830
		public struct PlayerInventoryData
		{
			// Token: 0x17001671 RID: 5745
			// (get) Token: 0x0600B661 RID: 46689 RVA: 0x0043DF77 File Offset: 0x0043C177
			// (set) Token: 0x0600B662 RID: 46690 RVA: 0x0043DF7F File Offset: 0x0043C17F
			public object Inventory { readonly get; set; }

			// Token: 0x17001672 RID: 5746
			// (get) Token: 0x0600B663 RID: 46691 RVA: 0x0043DF88 File Offset: 0x0043C188
			// (set) Token: 0x0600B664 RID: 46692 RVA: 0x0043DF90 File Offset: 0x0043C190
			public object Toolbelt { readonly get; set; }

			// Token: 0x17001673 RID: 5747
			// (get) Token: 0x0600B665 RID: 46693 RVA: 0x0043DF99 File Offset: 0x0043C199
			// (set) Token: 0x0600B666 RID: 46694 RVA: 0x0043DFA1 File Offset: 0x0043C1A1
			public object Gear { readonly get; set; }
		}

		// Token: 0x020016C7 RID: 5831
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public struct InventoryItemData
		{
			// Token: 0x17001674 RID: 5748
			// (get) Token: 0x0600B667 RID: 46695 RVA: 0x0043DFAA File Offset: 0x0043C1AA
			// (set) Token: 0x0600B668 RID: 46696 RVA: 0x0043DFB2 File Offset: 0x0043C1B2
			public int Slot { readonly get; set; }

			// Token: 0x17001675 RID: 5749
			// (get) Token: 0x0600B669 RID: 46697 RVA: 0x0043DFBB File Offset: 0x0043C1BB
			// (set) Token: 0x0600B66A RID: 46698 RVA: 0x0043DFC3 File Offset: 0x0043C1C3
			public bool? IsEncumberedSlot { readonly get; set; }

			// Token: 0x17001676 RID: 5750
			// (get) Token: 0x0600B66B RID: 46699 RVA: 0x0043DFCC File Offset: 0x0043C1CC
			// (set) Token: 0x0600B66C RID: 46700 RVA: 0x0043DFD4 File Offset: 0x0043C1D4
			public string ObjectName { readonly get; set; }

			// Token: 0x17001677 RID: 5751
			// (get) Token: 0x0600B66D RID: 46701 RVA: 0x0043DFDD File Offset: 0x0043C1DD
			// (set) Token: 0x0600B66E RID: 46702 RVA: 0x0043DFE5 File Offset: 0x0043C1E5
			public int Quantity { readonly get; set; }

			// Token: 0x17001678 RID: 5752
			// (get) Token: 0x0600B66F RID: 46703 RVA: 0x0043DFEE File Offset: 0x0043C1EE
			// (set) Token: 0x0600B670 RID: 46704 RVA: 0x0043DFF6 File Offset: 0x0043C1F6
			public float Durability { readonly get; set; }
		}
	}
}
