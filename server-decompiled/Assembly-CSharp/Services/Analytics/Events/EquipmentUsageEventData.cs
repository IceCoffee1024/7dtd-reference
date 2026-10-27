using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016BC RID: 5820
	[EventTitle("Equipment Usage")]
	public class EquipmentUsageEventData : BaseEventData
	{
		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x0600B5DF RID: 46559 RVA: 0x0043DA8C File Offset: 0x0043BC8C
		public override string EventType
		{
			get
			{
				return "equipment_usage";
			}
		}

		// Token: 0x04008832 RID: 34866
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId;

		// Token: 0x04008833 RID: 34867
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId;

		// Token: 0x04008834 RID: 34868
		[JsonProperty(PropertyName = "equipment_change_ts")]
		[Description("The moment when the player equipped the item")]
		[DateTimeFormat]
		public string EquipmentChangeTimestamp;

		// Token: 0x04008835 RID: 34869
		[JsonProperty(PropertyName = "player_level")]
		[Description("The current level of the player at the time of equipping the item")]
		public int PlayerLevel;

		// Token: 0x04008836 RID: 34870
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("The current personal  game Stage of the player at the time of equipping the item")]
		public int PersonalGameStage;

		// Token: 0x04008837 RID: 34871
		[JsonProperty(PropertyName = "source")]
		[Description("The source from which the item came")]
		public string Source;

		// Token: 0x04008838 RID: 34872
		[JsonProperty(PropertyName = "equipment_name")]
		[Description("The name of the item (truncated to the first 50 characters)")]
		[JsonConverter(typeof(TruncateStringSerializerConverter))]
		public string EquipmentName;

		// Token: 0x04008839 RID: 34873
		[JsonProperty(PropertyName = "equipment_slot")]
		[Description("The slot where the equipment goes")]
		public string EquipmentSlot;

		// Token: 0x0400883A RID: 34874
		[JsonProperty(PropertyName = "equipment_tags")]
		[Description("The list of tags on the item")]
		public object EquipmentTags;

		// Token: 0x0400883B RID: 34875
		[JsonProperty(PropertyName = "quality")]
		[Description("The quality of the Item")]
		public int? Quality;

		// Token: 0x0400883C RID: 34876
		[JsonProperty(PropertyName = "mod_slots")]
		[Description("Name and quality of the equipped mods")]
		public IEnumerable<EquipmentUsageEventData.ModificationData> ModSlots;

		// Token: 0x0400883D RID: 34877
		[JsonProperty(PropertyName = "passive_effects")]
		[Description("An object containing of the passive effects on the item")]
		public object PassiveEffects;

		// Token: 0x0400883E RID: 34878
		[JsonProperty(PropertyName = "stats")]
		[Description("List of boosted stats on the item and their values")]
		public object Stats;

		// Token: 0x0400883F RID: 34879
		[JsonProperty(PropertyName = "current_degradation")]
		[Description("Current level of degradation on the item")]
		public float? CurrentDegradation;

		// Token: 0x04008840 RID: 34880
		[JsonProperty(PropertyName = "previous_equipment")]
		[Description("The name of the item that replaced by the new piece (truncated to the first 50 characters). NULL if previously no equipment")]
		[JsonConverter(typeof(TruncateStringSerializerConverter))]
		public string PreviousEquipment;

		// Token: 0x020016BD RID: 5821
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public readonly struct ModificationData
		{
			// Token: 0x0600B5E1 RID: 46561 RVA: 0x0043DA93 File Offset: 0x0043BC93
			public ModificationData(string name, int? quality)
			{
				this.Name = name;
				this.Quality = quality;
			}

			// Token: 0x04008841 RID: 34881
			[JsonConverter(typeof(TruncateStringSerializerConverter))]
			public readonly string Name;

			// Token: 0x04008842 RID: 34882
			public readonly int? Quality;
		}
	}
}
