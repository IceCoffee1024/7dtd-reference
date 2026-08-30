using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x0200168B RID: 5771
	[EventTitle("Login")]
	public class LoginEventData : BaseEventData
	{
		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x0600B4EE RID: 46318 RVA: 0x004379AC File Offset: 0x00435BAC
		public override string EventType
		{
			get
			{
				return "login";
			}
		}

		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x0600B4EF RID: 46319 RVA: 0x004379B3 File Offset: 0x00435BB3
		// (set) Token: 0x0600B4F0 RID: 46320 RVA: 0x004379BB File Offset: 0x00435BBB
		[JsonProperty(PropertyName = "session_start_ts")]
		[DateTimeFormat]
		public string SessionStartTimeStamp { get; set; }

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x0600B4F1 RID: 46321 RVA: 0x004379C4 File Offset: 0x00435BC4
		[JsonProperty(PropertyName = "country_code", NullValueHandling = NullValueHandling.Include)]
		[Description("The country that the game session was initiated from. Converted From IP Address")]
		public string CountryCode { get; }

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x0600B4F2 RID: 46322 RVA: 0x004379CC File Offset: 0x00435BCC
		// (set) Token: 0x0600B4F3 RID: 46323 RVA: 0x004379D4 File Offset: 0x00435BD4
		[JsonProperty(PropertyName = "ip")]
		[IpToCountryTransform("$.ip")]
		[DropTransform("$.ip")]
		[EventAdditionalProperty]
		public string IP { get; set; }

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x0600B4F4 RID: 46324 RVA: 0x004379DD File Offset: 0x00435BDD
		// (set) Token: 0x0600B4F5 RID: 46325 RVA: 0x004379E5 File Offset: 0x00435BE5
		[JsonProperty(PropertyName = "crossplay_enabled")]
		[Description("Was crossplay enabled when the player launched the game")]
		public bool CrossplayEnabled { get; set; }

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x0600B4F6 RID: 46326 RVA: 0x004379EE File Offset: 0x00435BEE
		// (set) Token: 0x0600B4F7 RID: 46327 RVA: 0x004379F6 File Offset: 0x00435BF6
		[JsonProperty(PropertyName = "is_first_launch_eos")]
		[Description("A flag to identify first session based on EOS")]
		public bool? IsFirstLaunchEos { get; set; }

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x0600B4F8 RID: 46328 RVA: 0x004379FF File Offset: 0x00435BFF
		// (set) Token: 0x0600B4F9 RID: 46329 RVA: 0x00437A07 File Offset: 0x00435C07
		[JsonProperty(PropertyName = "is_new_login")]
		[Description("Is false if the session became stale")]
		public bool IsNewLogin { get; set; }
	}
}
