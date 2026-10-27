using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016CC RID: 5836
	[EventTitle("Player State Event")]
	public class PlayerStateEventData : BaseEventData
	{
		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x0600B6C7 RID: 46791 RVA: 0x0043E2BC File Offset: 0x0043C4BC
		public override string EventType
		{
			get
			{
				return "player_state";
			}
		}

		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x0600B6C8 RID: 46792 RVA: 0x0043E2C3 File Offset: 0x0043C4C3
		// (set) Token: 0x0600B6C9 RID: 46793 RVA: 0x0043E2CB File Offset: 0x0043C4CB
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server")]
		public string ServerId { get; set; }

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x0600B6CA RID: 46794 RVA: 0x0043E2D4 File Offset: 0x0043C4D4
		// (set) Token: 0x0600B6CB RID: 46795 RVA: 0x0043E2DC File Offset: 0x0043C4DC
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x040088B6 RID: 34998
		[JsonProperty(PropertyName = "state_ts")]
		[Description("time of evetn trigger")]
		[DateTimeFormat]
		public string StateTimestamp;

		// Token: 0x040088B7 RID: 34999
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("cumulative number of seconds online on this server at the time of trigger")]
		public int ElapsedTime;

		// Token: 0x040088B8 RID: 35000
		[JsonProperty(PropertyName = "trigger_reason")]
		[Description("Reason for trigger change")]
		public string TriggerReason;

		// Token: 0x040088B9 RID: 35001
		[JsonProperty(PropertyName = "trigger_source")]
		[Description("What is the source of the condition")]
		public string TriggerSource;

		// Token: 0x040088BA RID: 35002
		[JsonProperty(PropertyName = "conditions_added")]
		[Description("usualy the one condition added from a Conditon Acquired trigger")]
		public object ConditionsAdded;

		// Token: 0x040088BB RID: 35003
		[JsonProperty(PropertyName = "conditions_removed")]
		[Description("usualy the one conditions added from a Condition Acquired trigger")]
		public object ConditionsRemoved;

		// Token: 0x040088BC RID: 35004
		[JsonProperty(PropertyName = "active_conditions")]
		[Description("A tally of all conditions currently active. (included those from conditions_added) ")]
		public IEnumerable<PlayerStateEventData.ActiveConditionData> ActiveConditions;

		// Token: 0x020016CD RID: 5837
		public enum triggerReasons
		{
			// Token: 0x040088BE RID: 35006
			ServerStartState,
			// Token: 0x040088BF RID: 35007
			ConditionAquired,
			// Token: 0x040088C0 RID: 35008
			ConditionRemoved,
			// Token: 0x040088C1 RID: 35009
			ConditionReset,
			// Token: 0x040088C2 RID: 35010
			PlayerDeath
		}

		// Token: 0x020016CE RID: 5838
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public struct ActiveConditionData
		{
			// Token: 0x170016A7 RID: 5799
			// (get) Token: 0x0600B6CD RID: 46797 RVA: 0x0043E2E5 File Offset: 0x0043C4E5
			// (set) Token: 0x0600B6CE RID: 46798 RVA: 0x0043E2ED File Offset: 0x0043C4ED
			[JsonProperty(PropertyName = "condition_name")]
			public string ConditionName { readonly get; set; }

			// Token: 0x170016A8 RID: 5800
			// (get) Token: 0x0600B6CF RID: 46799 RVA: 0x0043E2F6 File Offset: 0x0043C4F6
			// (set) Token: 0x0600B6D0 RID: 46800 RVA: 0x0043E2FE File Offset: 0x0043C4FE
			[JsonProperty(PropertyName = "max_duration")]
			[Description("Max duration in seconds")]
			public float MaxDuration { readonly get; set; }

			// Token: 0x170016A9 RID: 5801
			// (get) Token: 0x0600B6D1 RID: 46801 RVA: 0x0043E307 File Offset: 0x0043C507
			// (set) Token: 0x0600B6D2 RID: 46802 RVA: 0x0043E30F File Offset: 0x0043C50F
			[JsonProperty(PropertyName = "current_duration")]
			[Description("Current duration in seconds")]
			public float CurrentDuration { readonly get; set; }

			// Token: 0x170016AA RID: 5802
			// (get) Token: 0x0600B6D3 RID: 46803 RVA: 0x0043E318 File Offset: 0x0043C518
			// (set) Token: 0x0600B6D4 RID: 46804 RVA: 0x0043E320 File Offset: 0x0043C520
			[JsonProperty(PropertyName = "damage_type")]
			public string DamageType { readonly get; set; }

			// Token: 0x170016AB RID: 5803
			// (get) Token: 0x0600B6D5 RID: 46805 RVA: 0x0043E329 File Offset: 0x0043C529
			// (set) Token: 0x0600B6D6 RID: 46806 RVA: 0x0043E331 File Offset: 0x0043C531
			[JsonProperty(PropertyName = "stack_type")]
			public string StackType { readonly get; set; }

			// Token: 0x170016AC RID: 5804
			// (get) Token: 0x0600B6D7 RID: 46807 RVA: 0x0043E33A File Offset: 0x0043C53A
			// (set) Token: 0x0600B6D8 RID: 46808 RVA: 0x0043E342 File Offset: 0x0043C542
			[JsonProperty(PropertyName = "update_rate")]
			[Description("Update rate in ticks")]
			public int UpdateRate { readonly get; set; }
		}
	}
}
