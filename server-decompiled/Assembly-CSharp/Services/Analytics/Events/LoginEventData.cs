using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x02001666 RID: 5734
	public class LoginEventData : BaseEventData
	{
		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x0600B38D RID: 45965 RVA: 0x00431D4A File Offset: 0x0042FF4A
		public override string EventType
		{
			get
			{
				return "login";
			}
		}

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x0600B38E RID: 45966 RVA: 0x00431D51 File Offset: 0x0042FF51
		// (set) Token: 0x0600B38F RID: 45967 RVA: 0x00431D59 File Offset: 0x0042FF59
		[JsonProperty(PropertyName = "session_start_ts")]
		public string SessionStartTimeStamp { get; set; }

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x0600B390 RID: 45968 RVA: 0x00431D62 File Offset: 0x0042FF62
		// (set) Token: 0x0600B391 RID: 45969 RVA: 0x00431D6A File Offset: 0x0042FF6A
		[JsonProperty(PropertyName = "platform")]
		[Description("The platform that the game session was initiated on.")]
		public string Platform { get; set; }

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x0600B392 RID: 45970 RVA: 0x00431D73 File Offset: 0x0042FF73
		// (set) Token: 0x0600B393 RID: 45971 RVA: 0x00431D7B File Offset: 0x0042FF7B
		[JsonProperty(PropertyName = "provider")]
		[Description("Provider Used to launch the game (Steam, XBL, PSN)")]
		public string Provider { get; set; }

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x0600B394 RID: 45972 RVA: 0x00431D84 File Offset: 0x0042FF84
		[JsonProperty(PropertyName = "country_code")]
		[Description("The country that the game session was initiated from. Converted From IP Address")]
		public string CountryCode { get; }

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x0600B395 RID: 45973 RVA: 0x00431D8C File Offset: 0x0042FF8C
		// (set) Token: 0x0600B396 RID: 45974 RVA: 0x00431D94 File Offset: 0x0042FF94
		[JsonProperty(PropertyName = "ip")]
		public string IP { get; set; }

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x0600B397 RID: 45975 RVA: 0x00431D9D File Offset: 0x0042FF9D
		// (set) Token: 0x0600B398 RID: 45976 RVA: 0x00431DA5 File Offset: 0x0042FFA5
		[JsonProperty(PropertyName = "crossplay_enabled")]
		[Description("Was crossplay enabled when the player launched the game")]
		public bool CrossplayEnabled { get; set; }

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x0600B399 RID: 45977 RVA: 0x00431DAE File Offset: 0x0042FFAE
		// (set) Token: 0x0600B39A RID: 45978 RVA: 0x00431DB6 File Offset: 0x0042FFB6
		[JsonProperty(PropertyName = "is_first_launch_eos")]
		[Description("A flag to identify first session based on EOS")]
		public bool? IsFirstLaunchEos { get; set; }
	}
}
