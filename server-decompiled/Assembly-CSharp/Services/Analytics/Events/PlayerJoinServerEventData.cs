using System;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x0200168C RID: 5772
	[EventTitle("Player Join Server")]
	public class PlayerJoinServerEventData : BaseEventData
	{
		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x0600B4FB RID: 46331 RVA: 0x00437A10 File Offset: 0x00435C10
		public override string EventType
		{
			get
			{
				return "player_join_server";
			}
		}

		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x0600B4FC RID: 46332 RVA: 0x00437A17 File Offset: 0x00435C17
		// (set) Token: 0x0600B4FD RID: 46333 RVA: 0x00437A1F File Offset: 0x00435C1F
		[JsonProperty(PropertyName = "server_id")]
		public string ServerId { get; set; }

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x0600B4FE RID: 46334 RVA: 0x00437A28 File Offset: 0x00435C28
		// (set) Token: 0x0600B4FF RID: 46335 RVA: 0x00437A30 File Offset: 0x00435C30
		[JsonProperty(PropertyName = "save_id")]
		public string SaveId { get; set; }

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x0600B500 RID: 46336 RVA: 0x00437A39 File Offset: 0x00435C39
		// (set) Token: 0x0600B501 RID: 46337 RVA: 0x00437A41 File Offset: 0x00435C41
		[JsonProperty(PropertyName = "server_join_ts")]
		[DateTimeFormat]
		public string ServerJoinTimestamp { get; set; }

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x0600B502 RID: 46338 RVA: 0x00437A4A File Offset: 0x00435C4A
		// (set) Token: 0x0600B503 RID: 46339 RVA: 0x00437A52 File Offset: 0x00435C52
		[JsonProperty(PropertyName = "online_players")]
		public int? OnlinePlayers { get; set; }

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x0600B504 RID: 46340 RVA: 0x00437A5B File Offset: 0x00435C5B
		// (set) Token: 0x0600B505 RID: 46341 RVA: 0x00437A63 File Offset: 0x00435C63
		[JsonProperty(PropertyName = "server_join_source")]
		public string ServerJoinSource { get; set; }

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600B506 RID: 46342 RVA: 0x00437A6C File Offset: 0x00435C6C
		// (set) Token: 0x0600B507 RID: 46343 RVA: 0x00437A74 File Offset: 0x00435C74
		[JsonProperty(PropertyName = "local_mods")]
		public object LocalMods { get; set; }

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x0600B508 RID: 46344 RVA: 0x00437A7D File Offset: 0x00435C7D
		// (set) Token: 0x0600B509 RID: 46345 RVA: 0x00437A85 File Offset: 0x00435C85
		[JsonProperty(PropertyName = "has_modified_xml_files")]
		public bool? HasModifiedXML { get; set; }

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x0600B50A RID: 46346 RVA: 0x00437A8E File Offset: 0x00435C8E
		// (set) Token: 0x0600B50B RID: 46347 RVA: 0x00437A96 File Offset: 0x00435C96
		[JsonProperty(PropertyName = "elapsed_player_time")]
		public uint? ElapsedPlayerTime { get; set; }

		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x0600B50C RID: 46348 RVA: 0x00437A9F File Offset: 0x00435C9F
		// (set) Token: 0x0600B50D RID: 46349 RVA: 0x00437AA7 File Offset: 0x00435CA7
		[JsonProperty(PropertyName = "elapsed_world_save_time")]
		public ulong? ElapsedWorldSaveTime { get; set; }

		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x0600B50E RID: 46350 RVA: 0x00437AB0 File Offset: 0x00435CB0
		// (set) Token: 0x0600B50F RID: 46351 RVA: 0x00437AB8 File Offset: 0x00435CB8
		[JsonProperty(PropertyName = "in_game_days")]
		public int? InGameDays { get; set; }

		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x0600B510 RID: 46352 RVA: 0x00437AC1 File Offset: 0x00435CC1
		// (set) Token: 0x0600B511 RID: 46353 RVA: 0x00437AC9 File Offset: 0x00435CC9
		[JsonProperty(PropertyName = "character_level")]
		public int? CharacterLevel { get; set; }

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x0600B512 RID: 46354 RVA: 0x00437AD2 File Offset: 0x00435CD2
		// (set) Token: 0x0600B513 RID: 46355 RVA: 0x00437ADA File Offset: 0x00435CDA
		[JsonProperty(PropertyName = "total_deaths")]
		public int? TotalDeaths { get; set; }

		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x0600B514 RID: 46356 RVA: 0x00437AE3 File Offset: 0x00435CE3
		// (set) Token: 0x0600B515 RID: 46357 RVA: 0x00437AEB File Offset: 0x00435CEB
		[JsonProperty(PropertyName = "personal_game_stage_modified")]
		public float? PersonalGameStageModified { get; set; }

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x0600B516 RID: 46358 RVA: 0x00437AF4 File Offset: 0x00435CF4
		// (set) Token: 0x0600B517 RID: 46359 RVA: 0x00437AFC File Offset: 0x00435CFC
		[JsonProperty(PropertyName = "personal_game_stage_unmodified")]
		public float? PersonalGameStageUnmodified { get; set; }

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x0600B518 RID: 46360 RVA: 0x00437B05 File Offset: 0x00435D05
		// (set) Token: 0x0600B519 RID: 46361 RVA: 0x00437B0D File Offset: 0x00435D0D
		[JsonProperty(PropertyName = "game_stages_json")]
		public object GameStagesJson { get; set; }

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x0600B51A RID: 46362 RVA: 0x00437B16 File Offset: 0x00435D16
		// (set) Token: 0x0600B51B RID: 46363 RVA: 0x00437B1E File Offset: 0x00435D1E
		[JsonProperty(PropertyName = "total_mods")]
		public int? TotalMods { get; set; }
	}
}
