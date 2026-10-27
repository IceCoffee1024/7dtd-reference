using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016C9 RID: 5833
	[EventTitle("PerkProgressionEvent")]
	public class PlayerPerkProgressionEventData : BaseEventData
	{
		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x0600B691 RID: 46737 RVA: 0x0043E105 File Offset: 0x0043C305
		public override string EventType
		{
			get
			{
				return "player_perk_progression";
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x0600B692 RID: 46738 RVA: 0x0043E10C File Offset: 0x0043C30C
		// (set) Token: 0x0600B693 RID: 46739 RVA: 0x0043E114 File Offset: 0x0043C314
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server")]
		public string ServerId { get; set; }

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x0600B694 RID: 46740 RVA: 0x0043E11D File Offset: 0x0043C31D
		// (set) Token: 0x0600B695 RID: 46741 RVA: 0x0043E125 File Offset: 0x0043C325
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x0600B696 RID: 46742 RVA: 0x0043E12E File Offset: 0x0043C32E
		// (set) Token: 0x0600B697 RID: 46743 RVA: 0x0043E136 File Offset: 0x0043C336
		[JsonProperty(PropertyName = "progression_event_ts")]
		[DateTimeFormat]
		[Description("Whenever the player uses or gain the server")]
		public string ProgressionEventTS { get; set; }

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x0600B698 RID: 46744 RVA: 0x0043E13F File Offset: 0x0043C33F
		// (set) Token: 0x0600B699 RID: 46745 RVA: 0x0043E147 File Offset: 0x0043C347
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time of trigger")]
		public int ElapsedTime { get; set; }

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x0600B69A RID: 46746 RVA: 0x0043E150 File Offset: 0x0043C350
		// (set) Token: 0x0600B69B RID: 46747 RVA: 0x0043E158 File Offset: 0x0043C358
		[JsonProperty(PropertyName = "player_level")]
		[Description("The current level of the player after transaction")]
		public int PlayerLevel { get; set; }

		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x0600B69C RID: 46748 RVA: 0x0043E161 File Offset: 0x0043C361
		// (set) Token: 0x0600B69D RID: 46749 RVA: 0x0043E169 File Offset: 0x0043C369
		[JsonProperty(PropertyName = "perk_section")]
		[Description("Highest Grouping. Refer to the tabs of the UI: Attribute name or General for the gear icon or Book for the book perks.")]
		public string PerkSection { get; set; }

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x0600B69E RID: 46750 RVA: 0x0043E172 File Offset: 0x0043C372
		// (set) Token: 0x0600B69F RID: 46751 RVA: 0x0043E17A File Offset: 0x0043C37A
		[JsonProperty(PropertyName = "perk_sub_section")]
		[Description("The second grouping if available. Set name for books, the section separator for skill perks")]
		public string PerkSubSection { get; set; }

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x0600B6A0 RID: 46752 RVA: 0x0043E183 File Offset: 0x0043C383
		// (set) Token: 0x0600B6A1 RID: 46753 RVA: 0x0043E18B File Offset: 0x0043C38B
		[JsonProperty(PropertyName = "perk_parent")]
		[Description("Sub category of the perk track")]
		public string PerkParent { get; set; }

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x0600B6A2 RID: 46754 RVA: 0x0043E194 File Offset: 0x0043C394
		// (set) Token: 0x0600B6A3 RID: 46755 RVA: 0x0043E19C File Offset: 0x0043C39C
		[JsonProperty(PropertyName = "perk_name")]
		[Description("Specific ID of the perk")]
		public string PerkName { get; set; }

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x0600B6A4 RID: 46756 RVA: 0x0043E1A5 File Offset: 0x0043C3A5
		// (set) Token: 0x0600B6A5 RID: 46757 RVA: 0x0043E1AD File Offset: 0x0043C3AD
		[JsonProperty(PropertyName = "change_type")]
		[Description("The change happening to that perk")]
		public string ChangeType { get; set; }

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x0600B6A6 RID: 46758 RVA: 0x0043E1B6 File Offset: 0x0043C3B6
		// (set) Token: 0x0600B6A7 RID: 46759 RVA: 0x0043E1BE File Offset: 0x0043C3BE
		[JsonProperty(PropertyName = "change_source")]
		[Description("What caused the change.")]
		public string ChangeSource { get; set; }

		// Token: 0x020016CA RID: 5834
		public enum changeTypes
		{
			// Token: 0x040088A3 RID: 34979
			Learn,
			// Token: 0x040088A4 RID: 34980
			Unlearn
		}
	}
}
