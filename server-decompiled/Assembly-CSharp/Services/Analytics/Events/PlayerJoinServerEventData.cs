using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B4 RID: 5812
	[EventTitle("Player Join Server")]
	public class PlayerJoinServerEventData : BaseEventData
	{
		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x0600B5F0 RID: 46576 RVA: 0x0043BA1A File Offset: 0x00439C1A
		public override string EventType
		{
			get
			{
				return "player_join_server";
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x0600B5F1 RID: 46577 RVA: 0x0043BA21 File Offset: 0x00439C21
		// (set) Token: 0x0600B5F2 RID: 46578 RVA: 0x0043BA29 File Offset: 0x00439C29
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server")]
		public string ServerId { get; set; }

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x0600B5F3 RID: 46579 RVA: 0x0043BA32 File Offset: 0x00439C32
		// (set) Token: 0x0600B5F4 RID: 46580 RVA: 0x0043BA3A File Offset: 0x00439C3A
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x0600B5F5 RID: 46581 RVA: 0x0043BA43 File Offset: 0x00439C43
		// (set) Token: 0x0600B5F6 RID: 46582 RVA: 0x0043BA4B File Offset: 0x00439C4B
		[JsonProperty(PropertyName = "server_join_ts")]
		[DateTimeFormat]
		[Description("Whenever the player joins the server")]
		public string ServerJoinTimestamp { get; set; }

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x0600B5F7 RID: 46583 RVA: 0x0043BA54 File Offset: 0x00439C54
		// (set) Token: 0x0600B5F8 RID: 46584 RVA: 0x0043BA5C File Offset: 0x00439C5C
		[JsonProperty(PropertyName = "online_players")]
		[Description("Number of players on the server (inclusive)")]
		public int? OnlinePlayers { get; set; }

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x0600B5F9 RID: 46585 RVA: 0x0043BA65 File Offset: 0x00439C65
		// (set) Token: 0x0600B5FA RID: 46586 RVA: 0x0043BA6D File Offset: 0x00439C6D
		[JsonProperty(PropertyName = "server_join_source")]
		[Description("How the player joined the server")]
		public string ServerJoinSource { get; set; }

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x0600B5FB RID: 46587 RVA: 0x0043BA76 File Offset: 0x00439C76
		// (set) Token: 0x0600B5FC RID: 46588 RVA: 0x0043BA7E File Offset: 0x00439C7E
		[JsonProperty(PropertyName = "local_mods")]
		[Description("List of active mods with name, display name and description. Limited to the first 100")]
		public object LocalMods { get; set; }

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x0600B5FD RID: 46589 RVA: 0x0043BA87 File Offset: 0x00439C87
		// (set) Token: 0x0600B5FE RID: 46590 RVA: 0x0043BA8F File Offset: 0x00439C8F
		[JsonProperty(PropertyName = "has_modified_xml_files")]
		[Description("Are the hashes different than the stock file hashes?")]
		public bool? HasModifiedXML { get; set; }

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x0600B5FF RID: 46591 RVA: 0x0043BA98 File Offset: 0x00439C98
		// (set) Token: 0x0600B600 RID: 46592 RVA: 0x0043BAA0 File Offset: 0x00439CA0
		[JsonProperty(PropertyName = "elapsed_player_time")]
		[Description("Cumulative seconds that the player has previously played on the server")]
		public uint? ElapsedPlayerTime { get; set; }

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x0600B601 RID: 46593 RVA: 0x0043BAA9 File Offset: 0x00439CA9
		// (set) Token: 0x0600B602 RID: 46594 RVA: 0x0043BAB1 File Offset: 0x00439CB1
		[JsonProperty(PropertyName = "in_game_days")]
		[Description("Seconds with at least one player online on this specific save")]
		public int? InGameDays { get; set; }

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x0600B603 RID: 46595 RVA: 0x0043BABA File Offset: 0x00439CBA
		// (set) Token: 0x0600B604 RID: 46596 RVA: 0x0043BAC2 File Offset: 0x00439CC2
		[JsonProperty(PropertyName = "character_level")]
		[Description("Current level of the character")]
		public int? CharacterLevel { get; set; }

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x0600B605 RID: 46597 RVA: 0x0043BACB File Offset: 0x00439CCB
		// (set) Token: 0x0600B606 RID: 46598 RVA: 0x0043BAD3 File Offset: 0x00439CD3
		[JsonProperty(PropertyName = "total_deaths")]
		[Description("Total number of deaths this player suffered")]
		public int? TotalDeaths { get; set; }

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x0600B607 RID: 46599 RVA: 0x0043BADC File Offset: 0x00439CDC
		// (set) Token: 0x0600B608 RID: 46600 RVA: 0x0043BAE4 File Offset: 0x00439CE4
		[JsonProperty(PropertyName = "personal_game_stage_modified")]
		[Description("Current gamestage of the player taking into account biomes and quest modifiers")]
		public float? PersonalGameStageModified { get; set; }

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x0600B609 RID: 46601 RVA: 0x0043BAED File Offset: 0x00439CED
		// (set) Token: 0x0600B60A RID: 46602 RVA: 0x0043BAF5 File Offset: 0x00439CF5
		[JsonProperty(PropertyName = "personal_game_stage_unmodified")]
		[Description("Current gamestage of the player")]
		public float? PersonalGameStageUnmodified { get; set; }

		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x0600B60B RID: 46603 RVA: 0x0043BAFE File Offset: 0x00439CFE
		// (set) Token: 0x0600B60C RID: 46604 RVA: 0x0043BB06 File Offset: 0x00439D06
		[JsonProperty(PropertyName = "game_stages_json")]
		[Description("Breakdown of the variables used in the gamestage modified calculation")]
		public object GameStagesJson { get; set; }

		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x0600B60D RID: 46605 RVA: 0x0043BB0F File Offset: 0x00439D0F
		// (set) Token: 0x0600B60E RID: 46606 RVA: 0x0043BB17 File Offset: 0x00439D17
		[JsonProperty(PropertyName = "total_mods")]
		[Description("Total number of active mods")]
		public int? TotalMods { get; set; }
	}
}
