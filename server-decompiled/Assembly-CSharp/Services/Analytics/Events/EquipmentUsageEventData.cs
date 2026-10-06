using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016A9 RID: 5801
	[EventTitle("Equipment Usage")]
	public class EquipmentUsageEventData : BaseEventData
	{
		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x0600B574 RID: 46452 RVA: 0x0043B55A File Offset: 0x0043975A
		public override string EventType
		{
			get
			{
				return "equipment_usage";
			}
		}

		// Token: 0x040087E3 RID: 34787
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId;

		// Token: 0x040087E4 RID: 34788
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId;

		// Token: 0x040087E5 RID: 34789
		[JsonProperty(PropertyName = "equipment_change_ts")]
		[Description("The moment when the player equipped the item")]
		[DateTimeFormat]
		public string EquipmentChangeTimestamp;

		// Token: 0x040087E6 RID: 34790
		[JsonProperty(PropertyName = "player_level")]
		[Description("The current level of the player at the time of equipping the item")]
		public int PlayerLevel;

		// Token: 0x040087E7 RID: 34791
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("The current personal  game Stage of the player at the time of equipping the item")]
		public int PersonalGameStage;

		// Token: 0x040087E8 RID: 34792
		[JsonProperty(PropertyName = "source")]
		[Description("The source from which the item came")]
		public string Source;

		// Token: 0x040087E9 RID: 34793
		[JsonProperty(PropertyName = "equipment_name")]
		[Description("The name of the item (truncated to the first 50 characters)")]
		[JsonConverter(typeof(TruncateStringSerializerConverter))]
		public string EquipmentName;

		// Token: 0x040087EA RID: 34794
		[JsonProperty(PropertyName = "equipment_slot")]
		[Description("The slot where the equipment goes")]
		public string EquipmentSlot;

		// Token: 0x040087EB RID: 34795
		[JsonProperty(PropertyName = "equipment_tags")]
		[Description("The list of tags on the item")]
		public object EquipmentTags;

		// Token: 0x040087EC RID: 34796
		[JsonProperty(PropertyName = "quality")]
		[Description("The quality of the Item")]
		public int? Quality;

		// Token: 0x040087ED RID: 34797
		[JsonProperty(PropertyName = "mod_slots")]
		[Description("Name and quality of the equipped mods")]
		public IEnumerable<EquipmentUsageEventData.ModificationData> ModSlots;

		// Token: 0x040087EE RID: 34798
		[JsonProperty(PropertyName = "passive_effects")]
		[Description("An object containing of the passive effects on the item")]
		public object PassiveEffects;

		// Token: 0x040087EF RID: 34799
		[JsonProperty(PropertyName = "stats")]
		[Description("List of boosted stats on the item and their values")]
		public object Stats;

		// Token: 0x040087F0 RID: 34800
		[JsonProperty(PropertyName = "current_degradation")]
		[Description("Current level of degradation on the item")]
		public float? CurrentDegradation;

		// Token: 0x040087F1 RID: 34801
		[JsonProperty(PropertyName = "previous_equipment")]
		[Description("The name of the item that replaced by the new piece (truncated to the first 50 characters). NULL if previously no equipment")]
		[JsonConverter(typeof(TruncateStringSerializerConverter))]
		public string PreviousEquipment;

		// Token: 0x020016AA RID: 5802
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public readonly struct ModificationData
		{
			// Token: 0x0600B576 RID: 46454 RVA: 0x0043B561 File Offset: 0x00439761
			public ModificationData(string name, int? quality)
			{
				this.Name = name;
				this.Quality = quality;
			}

			// Token: 0x040087F2 RID: 34802
			[JsonConverter(typeof(TruncateStringSerializerConverter))]
			public readonly string Name;

			// Token: 0x040087F3 RID: 34803
			public readonly int? Quality;
		}
	}
}
