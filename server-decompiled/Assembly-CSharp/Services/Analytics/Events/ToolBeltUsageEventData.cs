using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016D6 RID: 5846
	[EventTitle("ToolBeltUsageEvent")]
	public class ToolBeltUsageEventData : BaseEventData
	{
		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x0600B756 RID: 46934 RVA: 0x0043EB4D File Offset: 0x0043CD4D
		public override string EventType
		{
			get
			{
				return "toolbelt_usage";
			}
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x0600B757 RID: 46935 RVA: 0x0043EB54 File Offset: 0x0043CD54
		// (set) Token: 0x0600B758 RID: 46936 RVA: 0x0043EB5C File Offset: 0x0043CD5C
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server")]
		public string ServerId { get; set; }

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x0600B759 RID: 46937 RVA: 0x0043EB65 File Offset: 0x0043CD65
		// (set) Token: 0x0600B75A RID: 46938 RVA: 0x0043EB6D File Offset: 0x0043CD6D
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x04008917 RID: 35095
		[JsonProperty(PropertyName = "toolbelt_change_ts")]
		[Description("The moment when the player equipped or removed the item.")]
		[DateTimeFormat]
		public string ToolbeltChangeTimestamp;

		// Token: 0x04008918 RID: 35096
		[JsonProperty(PropertyName = "player_level")]
		[Description("The current level of the player at the time of equipping the item.")]
		public int PlayerLevel;

		// Token: 0x04008919 RID: 35097
		[JsonProperty(PropertyName = "personal_game_stage")]
		[Description("The current personal game Stage of the player at the time of equipping the item.")]
		public int PersonalGameStage;

		// Token: 0x0400891A RID: 35098
		[JsonProperty(PropertyName = "toolbelt_slot")]
		[Description("the index number of the slot")]
		public int ToolbeltSlot;

		// Token: 0x0400891B RID: 35099
		[JsonProperty(PropertyName = "item_name")]
		[Description("the name of the item being equipped (watch out of localization)")]
		public string ItemName;

		// Token: 0x0400891C RID: 35100
		[JsonProperty(PropertyName = "stats")]
		[Description("List of stats on the item")]
		public object Stats;

		// Token: 0x0400891D RID: 35101
		[JsonProperty(PropertyName = "passive_effects")]
		[Description("An object containing of the passive effects on the item")]
		public object PassiveEffects;

		// Token: 0x0400891E RID: 35102
		[JsonProperty(PropertyName = "quality")]
		[Description("the quality of the item. Can be null")]
		public int? Quality;

		// Token: 0x0400891F RID: 35103
		[JsonProperty(PropertyName = "mod_slots")]
		[Description("")]
		public IEnumerable<ToolBeltUsageEventData.ModificationData> ModSlots;

		// Token: 0x04008920 RID: 35104
		[JsonProperty(PropertyName = "tags")]
		[Description("the list of tags on the item (Not localized value)")]
		public object Tags;

		// Token: 0x04008921 RID: 35105
		[JsonProperty(PropertyName = "current_degradation")]
		[Description("The current level of degradation on the item")]
		public float? CurrentDegradation;

		// Token: 0x04008922 RID: 35106
		[JsonProperty(PropertyName = "item_type")]
		[Description("The item type")]
		public string ItemType;

		// Token: 0x04008923 RID: 35107
		[JsonProperty(PropertyName = "stack_size")]
		[Description("The stack size")]
		public int? StackSize;

		// Token: 0x04008924 RID: 35108
		[JsonProperty(PropertyName = "previous_item_name")]
		[Description("The name of the item that was previously there ?")]
		public string PreviousItemName;

		// Token: 0x04008925 RID: 35109
		[JsonProperty(PropertyName = "groups")]
		[Description("what is the item group")]
		public object Groups;

		// Token: 0x04008926 RID: 35110
		[JsonProperty(PropertyName = "properties")]
		[Description("all property on the item")]
		public object Properties;

		// Token: 0x020016D7 RID: 5847
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public readonly struct ModificationData
		{
			// Token: 0x0600B75C RID: 46940 RVA: 0x0043EB76 File Offset: 0x0043CD76
			public ModificationData(string name, int? quality)
			{
				this.Name = name;
				this.Quality = quality;
			}

			// Token: 0x04008927 RID: 35111
			[JsonConverter(typeof(TruncateStringSerializerConverter))]
			public readonly string Name;

			// Token: 0x04008928 RID: 35112
			public readonly int? Quality;
		}

		// Token: 0x020016D8 RID: 5848
		public readonly struct StatsData
		{
			// Token: 0x0600B75D RID: 46941 RVA: 0x0043EB86 File Offset: 0x0043CD86
			public StatsData(string name, bool isBoosted, short value)
			{
				this.PassiveEffects = name;
				this.IsBoosted = isBoosted;
				this.Value = value;
			}

			// Token: 0x04008929 RID: 35113
			[JsonConverter(typeof(TruncateStringSerializerConverter))]
			public readonly string PassiveEffects;

			// Token: 0x0400892A RID: 35114
			public readonly bool IsBoosted;

			// Token: 0x0400892B RID: 35115
			public readonly short Value;
		}
	}
}
