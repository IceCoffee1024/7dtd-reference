using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016C0 RID: 5824
	[EventTitle("Login")]
	public class LoginEventData : BaseEventData
	{
		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x0600B5F6 RID: 46582 RVA: 0x0043DC08 File Offset: 0x0043BE08
		public override string EventType
		{
			get
			{
				return "login";
			}
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x0600B5F7 RID: 46583 RVA: 0x0043DC0F File Offset: 0x0043BE0F
		// (set) Token: 0x0600B5F8 RID: 46584 RVA: 0x0043DC17 File Offset: 0x0043BE17
		[JsonProperty(PropertyName = "session_start_ts")]
		[DateTimeFormat]
		[Description("the timestamp in UTC when the session was launched")]
		public string SessionStartTimeStamp { get; set; }

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x0600B5F9 RID: 46585 RVA: 0x0043DC20 File Offset: 0x0043BE20
		[JsonProperty(PropertyName = "country_code", NullValueHandling = NullValueHandling.Include)]
		[Description("The country that the game session was initiated from. Converted From IP Address")]
		public string CountryCode { get; }

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x0600B5FA RID: 46586 RVA: 0x0043DC28 File Offset: 0x0043BE28
		// (set) Token: 0x0600B5FB RID: 46587 RVA: 0x0043DC30 File Offset: 0x0043BE30
		[JsonProperty(PropertyName = "ip")]
		[IpToCountryTransform("$.ip")]
		[DropTransform("$.ip")]
		[EventAdditionalProperty]
		public string IP { get; set; }

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x0600B5FC RID: 46588 RVA: 0x0043DC39 File Offset: 0x0043BE39
		// (set) Token: 0x0600B5FD RID: 46589 RVA: 0x0043DC41 File Offset: 0x0043BE41
		[JsonProperty(PropertyName = "crossplay_enabled")]
		[Description("Was crossplay enabled when the player launched the game")]
		public bool CrossplayEnabled { get; set; }

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x0600B5FE RID: 46590 RVA: 0x0043DC4A File Offset: 0x0043BE4A
		// (set) Token: 0x0600B5FF RID: 46591 RVA: 0x0043DC52 File Offset: 0x0043BE52
		[JsonProperty(PropertyName = "is_first_launch_eos")]
		[Description("A flag to identify first session based on EOS")]
		public bool? IsFirstLaunchEos { get; set; }

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x0600B600 RID: 46592 RVA: 0x0043DC5B File Offset: 0x0043BE5B
		// (set) Token: 0x0600B601 RID: 46593 RVA: 0x0043DC63 File Offset: 0x0043BE63
		[JsonProperty(PropertyName = "is_new_login")]
		[Description("Is false if the session became stale")]
		public bool IsNewLogin { get; set; }
	}
}
