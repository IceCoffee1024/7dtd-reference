using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016AD RID: 5805
	[EventTitle("Login")]
	public class LoginEventData : BaseEventData
	{
		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x0600B58B RID: 46475 RVA: 0x0043B6D4 File Offset: 0x004398D4
		public override string EventType
		{
			get
			{
				return "login";
			}
		}

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x0600B58C RID: 46476 RVA: 0x0043B6DB File Offset: 0x004398DB
		// (set) Token: 0x0600B58D RID: 46477 RVA: 0x0043B6E3 File Offset: 0x004398E3
		[JsonProperty(PropertyName = "session_start_ts")]
		[DateTimeFormat]
		[Description("the timestamp in UTC when the session was launched")]
		public string SessionStartTimeStamp { get; set; }

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x0600B58E RID: 46478 RVA: 0x0043B6EC File Offset: 0x004398EC
		[JsonProperty(PropertyName = "country_code", NullValueHandling = NullValueHandling.Include)]
		[Description("The country that the game session was initiated from. Converted From IP Address")]
		public string CountryCode { get; }

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x0600B58F RID: 46479 RVA: 0x0043B6F4 File Offset: 0x004398F4
		// (set) Token: 0x0600B590 RID: 46480 RVA: 0x0043B6FC File Offset: 0x004398FC
		[JsonProperty(PropertyName = "ip")]
		[IpToCountryTransform("$.ip")]
		[DropTransform("$.ip")]
		[EventAdditionalProperty]
		public string IP { get; set; }

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x0600B591 RID: 46481 RVA: 0x0043B705 File Offset: 0x00439905
		// (set) Token: 0x0600B592 RID: 46482 RVA: 0x0043B70D File Offset: 0x0043990D
		[JsonProperty(PropertyName = "crossplay_enabled")]
		[Description("Was crossplay enabled when the player launched the game")]
		public bool CrossplayEnabled { get; set; }

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x0600B593 RID: 46483 RVA: 0x0043B716 File Offset: 0x00439916
		// (set) Token: 0x0600B594 RID: 46484 RVA: 0x0043B71E File Offset: 0x0043991E
		[JsonProperty(PropertyName = "is_first_launch_eos")]
		[Description("A flag to identify first session based on EOS")]
		public bool? IsFirstLaunchEos { get; set; }

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x0600B595 RID: 46485 RVA: 0x0043B727 File Offset: 0x00439927
		// (set) Token: 0x0600B596 RID: 46486 RVA: 0x0043B72F File Offset: 0x0043992F
		[JsonProperty(PropertyName = "is_new_login")]
		[Description("Is false if the session became stale")]
		public bool IsNewLogin { get; set; }
	}
}
