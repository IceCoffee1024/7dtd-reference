using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016C8 RID: 5832
	[EventTitle("Player Join Server")]
	public class PlayerJoinServerEventData : BaseEventData
	{
		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x0600B671 RID: 46705 RVA: 0x0043DFFF File Offset: 0x0043C1FF
		public override string EventType
		{
			get
			{
				return "player_join_server";
			}
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x0600B672 RID: 46706 RVA: 0x0043E006 File Offset: 0x0043C206
		// (set) Token: 0x0600B673 RID: 46707 RVA: 0x0043E00E File Offset: 0x0043C20E
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server")]
		public string ServerId { get; set; }

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x0600B674 RID: 46708 RVA: 0x0043E017 File Offset: 0x0043C217
		// (set) Token: 0x0600B675 RID: 46709 RVA: 0x0043E01F File Offset: 0x0043C21F
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x0600B676 RID: 46710 RVA: 0x0043E028 File Offset: 0x0043C228
		// (set) Token: 0x0600B677 RID: 46711 RVA: 0x0043E030 File Offset: 0x0043C230
		[JsonProperty(PropertyName = "server_join_ts")]
		[DateTimeFormat]
		[Description("Whenever the player joins the server")]
		public string ServerJoinTimestamp { get; set; }

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x0600B678 RID: 46712 RVA: 0x0043E039 File Offset: 0x0043C239
		// (set) Token: 0x0600B679 RID: 46713 RVA: 0x0043E041 File Offset: 0x0043C241
		[JsonProperty(PropertyName = "online_players")]
		[Description("Number of players on the server (inclusive)")]
		public int? OnlinePlayers { get; set; }

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x0600B67A RID: 46714 RVA: 0x0043E04A File Offset: 0x0043C24A
		// (set) Token: 0x0600B67B RID: 46715 RVA: 0x0043E052 File Offset: 0x0043C252
		[JsonProperty(PropertyName = "server_join_source")]
		[Description("How the player joined the server")]
		public string ServerJoinSource { get; set; }

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x0600B67C RID: 46716 RVA: 0x0043E05B File Offset: 0x0043C25B
		// (set) Token: 0x0600B67D RID: 46717 RVA: 0x0043E063 File Offset: 0x0043C263
		[JsonProperty(PropertyName = "local_mods")]
		[Description("List of active mods with name, display name and description. Limited to the first 100")]
		public object LocalMods { get; set; }

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x0600B67E RID: 46718 RVA: 0x0043E06C File Offset: 0x0043C26C
		// (set) Token: 0x0600B67F RID: 46719 RVA: 0x0043E074 File Offset: 0x0043C274
		[JsonProperty(PropertyName = "has_modified_xml_files")]
		[Description("Are the hashes different than the stock file hashes?")]
		public bool? HasModifiedXML { get; set; }

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x0600B680 RID: 46720 RVA: 0x0043E07D File Offset: 0x0043C27D
		// (set) Token: 0x0600B681 RID: 46721 RVA: 0x0043E085 File Offset: 0x0043C285
		[JsonProperty(PropertyName = "elapsed_player_time")]
		[Description("Cumulative seconds that the player has previously played on the server")]
		public uint? ElapsedPlayerTime { get; set; }

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x0600B682 RID: 46722 RVA: 0x0043E08E File Offset: 0x0043C28E
		// (set) Token: 0x0600B683 RID: 46723 RVA: 0x0043E096 File Offset: 0x0043C296
		[JsonProperty(PropertyName = "in_game_days")]
		[Description("Seconds with at least one player online on this specific save")]
		public int? InGameDays { get; set; }

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x0600B684 RID: 46724 RVA: 0x0043E09F File Offset: 0x0043C29F
		// (set) Token: 0x0600B685 RID: 46725 RVA: 0x0043E0A7 File Offset: 0x0043C2A7
		[JsonProperty(PropertyName = "character_level")]
		[Description("Current level of the character")]
		public int? CharacterLevel { get; set; }

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x0600B686 RID: 46726 RVA: 0x0043E0B0 File Offset: 0x0043C2B0
		// (set) Token: 0x0600B687 RID: 46727 RVA: 0x0043E0B8 File Offset: 0x0043C2B8
		[JsonProperty(PropertyName = "total_deaths")]
		[Description("Total number of deaths this player suffered")]
		public int? TotalDeaths { get; set; }

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x0600B688 RID: 46728 RVA: 0x0043E0C1 File Offset: 0x0043C2C1
		// (set) Token: 0x0600B689 RID: 46729 RVA: 0x0043E0C9 File Offset: 0x0043C2C9
		[JsonProperty(PropertyName = "personal_game_stage_modified")]
		[Description("Current gamestage of the player taking into account biomes and quest modifiers")]
		public float? PersonalGameStageModified { get; set; }

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x0600B68A RID: 46730 RVA: 0x0043E0D2 File Offset: 0x0043C2D2
		// (set) Token: 0x0600B68B RID: 46731 RVA: 0x0043E0DA File Offset: 0x0043C2DA
		[JsonProperty(PropertyName = "personal_game_stage_unmodified")]
		[Description("Current gamestage of the player")]
		public float? PersonalGameStageUnmodified { get; set; }

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x0600B68C RID: 46732 RVA: 0x0043E0E3 File Offset: 0x0043C2E3
		// (set) Token: 0x0600B68D RID: 46733 RVA: 0x0043E0EB File Offset: 0x0043C2EB
		[JsonProperty(PropertyName = "game_stages_json")]
		[Description("Breakdown of the variables used in the gamestage modified calculation")]
		public object GameStagesJson { get; set; }

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x0600B68E RID: 46734 RVA: 0x0043E0F4 File Offset: 0x0043C2F4
		// (set) Token: 0x0600B68F RID: 46735 RVA: 0x0043E0FC File Offset: 0x0043C2FC
		[JsonProperty(PropertyName = "total_mods")]
		[Description("Total number of active mods")]
		public int? TotalMods { get; set; }
	}
}
