using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B6 RID: 5814
	[EventTitle("CameraToggle")]
	public class CameraToggleEventData : BaseEventData
	{
		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x0600B58D RID: 46477 RVA: 0x0043D7FD File Offset: 0x0043B9FD
		public override string EventType
		{
			get
			{
				return "camera_toggle";
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x0600B58E RID: 46478 RVA: 0x0043D804 File Offset: 0x0043BA04
		// (set) Token: 0x0600B58F RID: 46479 RVA: 0x0043D80C File Offset: 0x0043BA0C
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x0600B590 RID: 46480 RVA: 0x0043D815 File Offset: 0x0043BA15
		// (set) Token: 0x0600B591 RID: 46481 RVA: 0x0043D81D File Offset: 0x0043BA1D
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x0600B592 RID: 46482 RVA: 0x0043D826 File Offset: 0x0043BA26
		// (set) Token: 0x0600B593 RID: 46483 RVA: 0x0043D82E File Offset: 0x0043BA2E
		[JsonProperty(PropertyName = "elapsed_time")]
		[Description("Cumulative number of seconds online on this server at the time of trigger")]
		public uint ElapsedTime { get; set; }

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x0600B594 RID: 46484 RVA: 0x0043D837 File Offset: 0x0043BA37
		// (set) Token: 0x0600B595 RID: 46485 RVA: 0x0043D83F File Offset: 0x0043BA3F
		[JsonProperty(PropertyName = "player_loc")]
		[Description("The coordinates of the player' location at the time of changing cam type.")]
		public string PlayerLocation { get; set; }

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x0600B596 RID: 46486 RVA: 0x0043D848 File Offset: 0x0043BA48
		// (set) Token: 0x0600B597 RID: 46487 RVA: 0x0043D850 File Offset: 0x0043BA50
		[JsonProperty(PropertyName = "biome_id")]
		[Description("Id of the biome where they are")]
		public string BiomeId { get; set; }

		// Token: 0x0400880B RID: 34827
		[JsonProperty(PropertyName = "biome_name")]
		[Description("Name of the biome where they are")]
		public string BiomeName;

		// Token: 0x0400880C RID: 34828
		[JsonProperty(PropertyName = "poi_id")]
		[Description("POI ID, NULL if not in a POI")]
		public string PoiId;

		// Token: 0x0400880D RID: 34829
		[JsonProperty(PropertyName = "poi_name")]
		[Description("POI name if in a poi, NULL otherwise. Use the loc key to avoid issues caused by Mods.")]
		public string PoiName;

		// Token: 0x0400880E RID: 34830
		[JsonProperty(PropertyName = "camera_change_ts")]
		[Description("The timestamp when they changed the camera type.")]
		[DateTimeFormat]
		public string CameraChangeTimestamp;

		// Token: 0x0400880F RID: 34831
		[JsonProperty(PropertyName = "camera_type")]
		[Description("The camera type being enabled")]
		public string CameraType;
	}
}
