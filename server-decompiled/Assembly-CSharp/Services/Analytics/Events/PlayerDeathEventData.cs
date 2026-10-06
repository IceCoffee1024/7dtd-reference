using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016AE RID: 5806
	[EventTitle("QuestAdded")]
	public class PlayerDeathEventData : BaseEventData
	{
		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x0600B598 RID: 46488 RVA: 0x0043B738 File Offset: 0x00439938
		public override string EventType
		{
			get
			{
				return "player_death";
			}
		}

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x0600B599 RID: 46489 RVA: 0x0043B73F File Offset: 0x0043993F
		// (set) Token: 0x0600B59A RID: 46490 RVA: 0x0043B747 File Offset: 0x00439947
		[JsonProperty(PropertyName = "server_id")]
		[Description("Server identity")]
		public string ServerId { get; set; }

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x0600B59B RID: 46491 RVA: 0x0043B750 File Offset: 0x00439950
		// (set) Token: 0x0600B59C RID: 46492 RVA: 0x0043B758 File Offset: 0x00439958
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId { get; set; }

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x0600B59D RID: 46493 RVA: 0x0043B761 File Offset: 0x00439961
		// (set) Token: 0x0600B59E RID: 46494 RVA: 0x0043B769 File Offset: 0x00439969
		[JsonProperty(PropertyName = "death_ts")]
		[Description("Time of event trigger")]
		[DateTimeFormat]
		public string DeathTimestamp { get; set; }

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x0600B59F RID: 46495 RVA: 0x0043B772 File Offset: 0x00439972
		// (set) Token: 0x0600B5A0 RID: 46496 RVA: 0x0043B77A File Offset: 0x0043997A
		[JsonProperty(PropertyName = "elapsed_time")]
		public uint ElapsedTime { get; set; }

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x0600B5A1 RID: 46497 RVA: 0x0043B783 File Offset: 0x00439983
		// (set) Token: 0x0600B5A2 RID: 46498 RVA: 0x0043B78B File Offset: 0x0043998B
		[JsonProperty(PropertyName = "cause_of_death")]
		public string CauseOfDeath { get; set; }

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x0600B5A3 RID: 46499 RVA: 0x0043B794 File Offset: 0x00439994
		// (set) Token: 0x0600B5A4 RID: 46500 RVA: 0x0043B79C File Offset: 0x0043999C
		[JsonProperty(PropertyName = "cause_of_death_provider_id")]
		[KrakenIdTransform("$.cause_of_death_provider_id")]
		[Description("If cause of death is another player, this is the player's provider id, otherwise this field is null")]
		public string CauseOfDeathProviderId { get; set; }

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x0600B5A5 RID: 46501 RVA: 0x0043B7A5 File Offset: 0x004399A5
		// (set) Token: 0x0600B5A6 RID: 46502 RVA: 0x0043B7AD File Offset: 0x004399AD
		[JsonProperty(PropertyName = "damage_taken")]
		[Description("Damage value of ‘last hit'")]
		public int DamageTaken { get; set; }

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x0600B5A7 RID: 46503 RVA: 0x0043B7B6 File Offset: 0x004399B6
		// (set) Token: 0x0600B5A8 RID: 46504 RVA: 0x0043B7BE File Offset: 0x004399BE
		[JsonProperty(PropertyName = "count_foes_engaged")]
		public int CountFoesEngaged { get; set; }

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x0600B5A9 RID: 46505 RVA: 0x0043B7C7 File Offset: 0x004399C7
		// (set) Token: 0x0600B5AA RID: 46506 RVA: 0x0043B7CF File Offset: 0x004399CF
		[JsonProperty(PropertyName = "foes_engaged")]
		[Description("List of enemies currently in combat with the player at the time of death")]
		public object FoesEngaged { get; set; }

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x0600B5AB RID: 46507 RVA: 0x0043B7D8 File Offset: 0x004399D8
		// (set) Token: 0x0600B5AC RID: 46508 RVA: 0x0043B7E0 File Offset: 0x004399E0
		[JsonProperty(PropertyName = "active_conditions")]
		public object ActiveConditions { get; set; }

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x0600B5AD RID: 46509 RVA: 0x0043B7E9 File Offset: 0x004399E9
		// (set) Token: 0x0600B5AE RID: 46510 RVA: 0x0043B7F1 File Offset: 0x004399F1
		[JsonProperty(PropertyName = "player_state")]
		public object PlayerState { get; set; }

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x0600B5AF RID: 46511 RVA: 0x0043B7FA File Offset: 0x004399FA
		// (set) Token: 0x0600B5B0 RID: 46512 RVA: 0x0043B802 File Offset: 0x00439A02
		[JsonProperty(PropertyName = "player_loc")]
		[Description("Coordinates of the player' location at the time of death.")]
		public string PlayerLocation { get; set; }

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x0600B5B1 RID: 46513 RVA: 0x0043B80B File Offset: 0x00439A0B
		// (set) Token: 0x0600B5B2 RID: 46514 RVA: 0x0043B813 File Offset: 0x00439A13
		[JsonProperty(PropertyName = "biome_id")]
		[Description("Id of the biome where they died")]
		public string BiomeId { get; set; }

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x0600B5B3 RID: 46515 RVA: 0x0043B81C File Offset: 0x00439A1C
		// (set) Token: 0x0600B5B4 RID: 46516 RVA: 0x0043B824 File Offset: 0x00439A24
		[JsonProperty(PropertyName = "biome_name")]
		[Description("name of the biome where they died")]
		public string BiomeName { get; set; }

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x0600B5B5 RID: 46517 RVA: 0x0043B82D File Offset: 0x00439A2D
		// (set) Token: 0x0600B5B6 RID: 46518 RVA: 0x0043B835 File Offset: 0x00439A35
		[JsonProperty(PropertyName = "poi_id")]
		[Description("POI ID if player died in a POI otherwise use biome location as shown in the HUD.")]
		public string PoiId { get; set; }

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x0600B5B7 RID: 46519 RVA: 0x0043B83E File Offset: 0x00439A3E
		// (set) Token: 0x0600B5B8 RID: 46520 RVA: 0x0043B846 File Offset: 0x00439A46
		[JsonProperty(PropertyName = "poi_name")]
		[Description("POI name if player died in a POI otherwise use biome location as shown in the HUD.")]
		public string PoiName { get; set; }

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x0600B5B9 RID: 46521 RVA: 0x0043B84F File Offset: 0x00439A4F
		// (set) Token: 0x0600B5BA RID: 46522 RVA: 0x0043B857 File Offset: 0x00439A57
		[JsonProperty(PropertyName = "party_loc")]
		[Description("Coordinates of the player's teammates at the time of their death.")]
		public object PartyLoc { get; set; }

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x0600B5BB RID: 46523 RVA: 0x0043B860 File Offset: 0x00439A60
		// (set) Token: 0x0600B5BC RID: 46524 RVA: 0x0043B868 File Offset: 0x00439A68
		[JsonProperty(PropertyName = "player_inventory")]
		[Description("What the player was holding in their backback at the time of death.")]
		public object PlayerInventory { get; set; }

		// Token: 0x04008802 RID: 34818
		public const float EngagedFoeRange = 10f;

		// Token: 0x020016AF RID: 5807
		public struct EngagedEnemyData
		{
			// Token: 0x17001641 RID: 5697
			// (get) Token: 0x0600B5BE RID: 46526 RVA: 0x0043B871 File Offset: 0x00439A71
			// (set) Token: 0x0600B5BF RID: 46527 RVA: 0x0043B879 File Offset: 0x00439A79
			[JsonProperty(PropertyName = "entity_id")]
			public string EntityId { readonly get; set; }

			// Token: 0x17001642 RID: 5698
			// (get) Token: 0x0600B5C0 RID: 46528 RVA: 0x0043B882 File Offset: 0x00439A82
			// (set) Token: 0x0600B5C1 RID: 46529 RVA: 0x0043B88A File Offset: 0x00439A8A
			[JsonProperty(PropertyName = "count")]
			public int Count { readonly get; set; }
		}

		// Token: 0x020016B0 RID: 5808
		public struct ActiveConditionData
		{
			// Token: 0x17001643 RID: 5699
			// (get) Token: 0x0600B5C2 RID: 46530 RVA: 0x0043B893 File Offset: 0x00439A93
			// (set) Token: 0x0600B5C3 RID: 46531 RVA: 0x0043B89B File Offset: 0x00439A9B
			[JsonProperty(PropertyName = "condition_name")]
			public string ConditionName { readonly get; set; }

			// Token: 0x17001644 RID: 5700
			// (get) Token: 0x0600B5C4 RID: 46532 RVA: 0x0043B8A4 File Offset: 0x00439AA4
			// (set) Token: 0x0600B5C5 RID: 46533 RVA: 0x0043B8AC File Offset: 0x00439AAC
			[JsonProperty(PropertyName = "max_duration")]
			[Description("Max duration in seconds")]
			public float MaxDuration { readonly get; set; }

			// Token: 0x17001645 RID: 5701
			// (get) Token: 0x0600B5C6 RID: 46534 RVA: 0x0043B8B5 File Offset: 0x00439AB5
			// (set) Token: 0x0600B5C7 RID: 46535 RVA: 0x0043B8BD File Offset: 0x00439ABD
			[JsonProperty(PropertyName = "current_duration")]
			[Description("Current duration in seconds")]
			public float CurrentDuration { readonly get; set; }

			// Token: 0x17001646 RID: 5702
			// (get) Token: 0x0600B5C8 RID: 46536 RVA: 0x0043B8C6 File Offset: 0x00439AC6
			// (set) Token: 0x0600B5C9 RID: 46537 RVA: 0x0043B8CE File Offset: 0x00439ACE
			[JsonProperty(PropertyName = "damage_type")]
			public string DamageType { readonly get; set; }

			// Token: 0x17001647 RID: 5703
			// (get) Token: 0x0600B5CA RID: 46538 RVA: 0x0043B8D7 File Offset: 0x00439AD7
			// (set) Token: 0x0600B5CB RID: 46539 RVA: 0x0043B8DF File Offset: 0x00439ADF
			[JsonProperty(PropertyName = "stack_type")]
			public string StackType { readonly get; set; }

			// Token: 0x17001648 RID: 5704
			// (get) Token: 0x0600B5CC RID: 46540 RVA: 0x0043B8E8 File Offset: 0x00439AE8
			// (set) Token: 0x0600B5CD RID: 46541 RVA: 0x0043B8F0 File Offset: 0x00439AF0
			[JsonProperty(PropertyName = "update_rate")]
			[Description("Update rate in ticks")]
			public int UpdateRate { readonly get; set; }
		}

		// Token: 0x020016B1 RID: 5809
		public struct PlayerStateData
		{
			// Token: 0x17001649 RID: 5705
			// (get) Token: 0x0600B5CE RID: 46542 RVA: 0x0043B8F9 File Offset: 0x00439AF9
			// (set) Token: 0x0600B5CF RID: 46543 RVA: 0x0043B901 File Offset: 0x00439B01
			[JsonProperty(PropertyName = "max_stamina")]
			public float MaxStamina { readonly get; set; }

			// Token: 0x1700164A RID: 5706
			// (get) Token: 0x0600B5D0 RID: 46544 RVA: 0x0043B90A File Offset: 0x00439B0A
			// (set) Token: 0x0600B5D1 RID: 46545 RVA: 0x0043B912 File Offset: 0x00439B12
			[JsonProperty(PropertyName = "current_stamina")]
			public float CurrentStamina { readonly get; set; }

			// Token: 0x1700164B RID: 5707
			// (get) Token: 0x0600B5D2 RID: 46546 RVA: 0x0043B91B File Offset: 0x00439B1B
			// (set) Token: 0x0600B5D3 RID: 46547 RVA: 0x0043B923 File Offset: 0x00439B23
			[JsonProperty(PropertyName = "max_health")]
			public float MaxHealth { readonly get; set; }

			// Token: 0x1700164C RID: 5708
			// (get) Token: 0x0600B5D4 RID: 46548 RVA: 0x0043B92C File Offset: 0x00439B2C
			// (set) Token: 0x0600B5D5 RID: 46549 RVA: 0x0043B934 File Offset: 0x00439B34
			[JsonProperty(PropertyName = "current_health")]
			public float CurrentHealth { readonly get; set; }

			// Token: 0x1700164D RID: 5709
			// (get) Token: 0x0600B5D6 RID: 46550 RVA: 0x0043B93D File Offset: 0x00439B3D
			// (set) Token: 0x0600B5D7 RID: 46551 RVA: 0x0043B945 File Offset: 0x00439B45
			[JsonProperty(PropertyName = "max_water")]
			public float MaxWater { readonly get; set; }

			// Token: 0x1700164E RID: 5710
			// (get) Token: 0x0600B5D8 RID: 46552 RVA: 0x0043B94E File Offset: 0x00439B4E
			// (set) Token: 0x0600B5D9 RID: 46553 RVA: 0x0043B956 File Offset: 0x00439B56
			[JsonProperty(PropertyName = "current_water")]
			public float CurrentWater { readonly get; set; }

			// Token: 0x1700164F RID: 5711
			// (get) Token: 0x0600B5DA RID: 46554 RVA: 0x0043B95F File Offset: 0x00439B5F
			// (set) Token: 0x0600B5DB RID: 46555 RVA: 0x0043B967 File Offset: 0x00439B67
			[JsonProperty(PropertyName = "max_food")]
			public float MaxFood { readonly get; set; }

			// Token: 0x17001650 RID: 5712
			// (get) Token: 0x0600B5DC RID: 46556 RVA: 0x0043B970 File Offset: 0x00439B70
			// (set) Token: 0x0600B5DD RID: 46557 RVA: 0x0043B978 File Offset: 0x00439B78
			[JsonProperty(PropertyName = "current_food")]
			public float CurrentFood { readonly get; set; }

			// Token: 0x17001651 RID: 5713
			// (get) Token: 0x0600B5DE RID: 46558 RVA: 0x0043B981 File Offset: 0x00439B81
			// (set) Token: 0x0600B5DF RID: 46559 RVA: 0x0043B989 File Offset: 0x00439B89
			[JsonProperty(PropertyName = "temperature")]
			public float Temperature { readonly get; set; }
		}

		// Token: 0x020016B2 RID: 5810
		public struct PlayerInventoryData
		{
			// Token: 0x17001652 RID: 5714
			// (get) Token: 0x0600B5E0 RID: 46560 RVA: 0x0043B992 File Offset: 0x00439B92
			// (set) Token: 0x0600B5E1 RID: 46561 RVA: 0x0043B99A File Offset: 0x00439B9A
			public object Inventory { readonly get; set; }

			// Token: 0x17001653 RID: 5715
			// (get) Token: 0x0600B5E2 RID: 46562 RVA: 0x0043B9A3 File Offset: 0x00439BA3
			// (set) Token: 0x0600B5E3 RID: 46563 RVA: 0x0043B9AB File Offset: 0x00439BAB
			public object Toolbelt { readonly get; set; }

			// Token: 0x17001654 RID: 5716
			// (get) Token: 0x0600B5E4 RID: 46564 RVA: 0x0043B9B4 File Offset: 0x00439BB4
			// (set) Token: 0x0600B5E5 RID: 46565 RVA: 0x0043B9BC File Offset: 0x00439BBC
			public object Gear { readonly get; set; }
		}

		// Token: 0x020016B3 RID: 5811
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public struct InventoryItemData
		{
			// Token: 0x17001655 RID: 5717
			// (get) Token: 0x0600B5E6 RID: 46566 RVA: 0x0043B9C5 File Offset: 0x00439BC5
			// (set) Token: 0x0600B5E7 RID: 46567 RVA: 0x0043B9CD File Offset: 0x00439BCD
			public int Slot { readonly get; set; }

			// Token: 0x17001656 RID: 5718
			// (get) Token: 0x0600B5E8 RID: 46568 RVA: 0x0043B9D6 File Offset: 0x00439BD6
			// (set) Token: 0x0600B5E9 RID: 46569 RVA: 0x0043B9DE File Offset: 0x00439BDE
			public bool? IsEncumberedSlot { readonly get; set; }

			// Token: 0x17001657 RID: 5719
			// (get) Token: 0x0600B5EA RID: 46570 RVA: 0x0043B9E7 File Offset: 0x00439BE7
			// (set) Token: 0x0600B5EB RID: 46571 RVA: 0x0043B9EF File Offset: 0x00439BEF
			public string ObjectName { readonly get; set; }

			// Token: 0x17001658 RID: 5720
			// (get) Token: 0x0600B5EC RID: 46572 RVA: 0x0043B9F8 File Offset: 0x00439BF8
			// (set) Token: 0x0600B5ED RID: 46573 RVA: 0x0043BA00 File Offset: 0x00439C00
			public int Quantity { readonly get; set; }

			// Token: 0x17001659 RID: 5721
			// (get) Token: 0x0600B5EE RID: 46574 RVA: 0x0043BA09 File Offset: 0x00439C09
			// (set) Token: 0x0600B5EF RID: 46575 RVA: 0x0043BA11 File Offset: 0x00439C11
			public float Durability { readonly get; set; }
		}
	}
}
