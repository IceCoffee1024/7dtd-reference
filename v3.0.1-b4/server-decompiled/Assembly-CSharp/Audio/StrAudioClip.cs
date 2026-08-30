using System;

namespace Audio
{
	// Token: 0x02001AE8 RID: 6888
	public class StrAudioClip
	{
		// Token: 0x04009CC2 RID: 40130
		public const string ItemCollected = "item_pickup";

		// Token: 0x04009CC3 RID: 40131
		public const string ItemPlantCollected = "item_plant_pickup";

		// Token: 0x04009CC4 RID: 40132
		public const string EntityHitsGround = "entityhitsground";

		// Token: 0x04009CC5 RID: 40133
		public const string ItemDropped = "itemdropped";

		// Token: 0x04009CC6 RID: 40134
		public const string OpenDoor = "open_door_wood";

		// Token: 0x04009CC7 RID: 40135
		public const string CloseDoor = "close_door_wood";

		// Token: 0x04009CC8 RID: 40136
		public const string FallingIntoWater = "waterfallinginto";

		// Token: 0x04009CC9 RID: 40137
		public const string BuildingCompleted = "placeblock";

		// Token: 0x04009CCA RID: 40138
		public const string RotateBlock = "rotateblock";

		// Token: 0x04009CCB RID: 40139
		public const string TrunkBreaks = "trunkbreak";

		// Token: 0x04009CCC RID: 40140
		public const string TrunkFallImpact = "treefallimpact";

		// Token: 0x04009CCD RID: 40141
		public const string FlashlightToggle = "flashlight_toggle";

		// Token: 0x04009CCE RID: 40142
		public const string GenericHolster = "generic_holster";

		// Token: 0x04009CCF RID: 40143
		public const string GenericUnholster = "generic_unholster";

		// Token: 0x04009CD0 RID: 40144
		public const string MissingItemToRepair = "missingitemtorepair";

		// Token: 0x04009CD1 RID: 40145
		public const string CraftingClick = "craft_click_craft";

		// Token: 0x04009CD2 RID: 40146
		public const string RecipeUnlocked = "recipe_unlocked";

		// Token: 0x04009CD3 RID: 40147
		public const string OpenInventory = "open_inventory";

		// Token: 0x04009CD4 RID: 40148
		public const string CloseInventory = "close_inventory";

		// Token: 0x04009CD5 RID: 40149
		public const string CampfireOpen = "campfire_open";

		// Token: 0x04009CD6 RID: 40150
		public const string CampfireClose = "campfire_close";

		// Token: 0x04009CD7 RID: 40151
		public const string CampfireCookClick = "campfire_cook_click";

		// Token: 0x04009CD8 RID: 40152
		public const string ForgeOpen = "forge_open";

		// Token: 0x04009CD9 RID: 40153
		public const string ForgeClose = "forge_close";

		// Token: 0x04009CDA RID: 40154
		public const string ForgeSmeltClick = "forge_smelt_click";

		// Token: 0x04009CDB RID: 40155
		public const string ForgeBurn = "forge_burn_fuel";

		// Token: 0x04009CDC RID: 40156
		public const string ForgeFireDie = "forge_fire_die";

		// Token: 0x04009CDD RID: 40157
		public const string CementMixerOpen = "cement_mixer_open";

		// Token: 0x04009CDE RID: 40158
		public const string CementMixerClose = "cement_mixer_close";

		// Token: 0x04009CDF RID: 40159
		public const string CementMixerClick = "cement_mixer_start_click";

		// Token: 0x04009CE0 RID: 40160
		public const string CraftComplete = "craft_complete_item";

		// Token: 0x04009CE1 RID: 40161
		public const string CampfireComplete = "campfire_complete_item";

		// Token: 0x04009CE2 RID: 40162
		public const string ForgeComplete = "forge_item_complete";

		// Token: 0x04009CE3 RID: 40163
		public const string CementMixerComplete = "cement_mixer_complete";

		// Token: 0x04009CE4 RID: 40164
		public const string ChemStationOpen = "chem_station_open";

		// Token: 0x04009CE5 RID: 40165
		public const string ChemStationClose = "chem_station_close";

		// Token: 0x04009CE6 RID: 40166
		public const string ChemStationClick = "chem_station_mix_click";

		// Token: 0x04009CE7 RID: 40167
		public const string ChemStationComplete = "chem_station_complete_item";

		// Token: 0x04009CE8 RID: 40168
		public const string QuestNoteOffered = "quest_note_offer";

		// Token: 0x04009CE9 RID: 40169
		public const string QuestNoteDeclined = "quest_note_decline";

		// Token: 0x04009CEA RID: 40170
		public const string QuestStarted = "quest_started";

		// Token: 0x04009CEB RID: 40171
		public const string QuestFailed = "quest_failed";

		// Token: 0x04009CEC RID: 40172
		public const string QuestCompleted = "quest_subtask_complete";

		// Token: 0x04009CED RID: 40173
		public const string QuestObjective = "quest_objective_complete";

		// Token: 0x04009CEE RID: 40174
		public const string QuestChainCompleted = "quest_master_complete";

		// Token: 0x04009CEF RID: 40175
		public const string SkillPurchase = "ui_skill_purchase";

		// Token: 0x04009CF0 RID: 40176
		public const string TraderPurchase = "ui_trader_purchase";

		// Token: 0x04009CF1 RID: 40177
		public const string VendingPurchase = "ui_vending_purchase";

		// Token: 0x04009CF2 RID: 40178
		public const string VendingOpen = "open_vending";

		// Token: 0x04009CF3 RID: 40179
		public const string VendingClose = "close_vending";

		// Token: 0x04009CF4 RID: 40180
		public const string UITab = "ui_tab";

		// Token: 0x04009CF5 RID: 40181
		public const string UIHover = "ui_hover";

		// Token: 0x04009CF6 RID: 40182
		public const string UIDenied = "ui_denied";

		// Token: 0x04009CF7 RID: 40183
		public const string MapZoomIn = "map_zoom_in";

		// Token: 0x04009CF8 RID: 40184
		public const string MapZoomOut = "map_zoom_out";

		// Token: 0x04009CF9 RID: 40185
		public const string WaypointAdd = "ui_waypoint_add";

		// Token: 0x04009CFA RID: 40186
		public const string WaypointDelete = "ui_waypoint_delete";

		// Token: 0x04009CFB RID: 40187
		public const string PickupItem = "craft_take_item";

		// Token: 0x04009CFC RID: 40188
		public const string PlaceItem = "craft_place_item";

		// Token: 0x04009CFD RID: 40189
		public const string SignOpen = "open_sign";

		// Token: 0x04009CFE RID: 40190
		public const string SignClose = "close_sign";

		// Token: 0x04009CFF RID: 40191
		public const string BatteryBankStart = "batterybank_start";

		// Token: 0x04009D00 RID: 40192
		public const string BatteryBankStop = "batterybank_stop";

		// Token: 0x04009D01 RID: 40193
		public const string GeneratorStart = "generator_start";

		// Token: 0x04009D02 RID: 40194
		public const string GeneratorStop = "generator_stop";

		// Token: 0x04009D03 RID: 40195
		public const string SolarPanelStart = "solarpanel_on";

		// Token: 0x04009D04 RID: 40196
		public const string SolarPanelStop = "solarpanel_off";

		// Token: 0x04009D05 RID: 40197
		public const string SwitchOn = "switch_up";

		// Token: 0x04009D06 RID: 40198
		public const string SwitchOff = "switch_down";

		// Token: 0x04009D07 RID: 40199
		public const string WireConnectLive = "wire_live_connect";

		// Token: 0x04009D08 RID: 40200
		public const string WireConnectDead = "wire_dead_connect";

		// Token: 0x04009D09 RID: 40201
		public const string WireBreakLive = "wire_live_break";

		// Token: 0x04009D0A RID: 40202
		public const string WireBreakDead = "wire_dead_break";

		// Token: 0x04009D0B RID: 40203
		public const string PressurePlateDown = "pressureplate_down";

		// Token: 0x04009D0C RID: 40204
		public const string PressurePlateUp = "pressureplate_up";

		// Token: 0x04009D0D RID: 40205
		public const string MotionSensorTrigger = "motion_sensor_trigger";

		// Token: 0x04009D0E RID: 40206
		public const string TripWireTrigger = "trip_wire_trigger";

		// Token: 0x04009D0F RID: 40207
		public const string LightOn = "light_on";

		// Token: 0x04009D10 RID: 40208
		public const string LightOff = "light_off";

		// Token: 0x04009D11 RID: 40209
		public const string TimerRelayStart = "timer_start";

		// Token: 0x04009D12 RID: 40210
		public const string TimerRelayStop = "timer_stop";

		// Token: 0x04009D13 RID: 40211
		public const string ItemBreak = "itembreak";

		// Token: 0x04009D14 RID: 40212
		public const string TwitchNoAttack = "twitch_no_attack";

		// Token: 0x04009D15 RID: 40213
		public const string TwitchVoteAdded = "twitch_vote_received";

		// Token: 0x04009D16 RID: 40214
		public const string TwitchVoteStarted = "twitch_vote_started";

		// Token: 0x04009D17 RID: 40215
		public const string TwitchVoteEnded = "twitch_vote_ended";

		// Token: 0x04009D18 RID: 40216
		public const string TwitchInflate = "twitch_bighead_inflate";

		// Token: 0x04009D19 RID: 40217
		public const string TwitchDeflate = "twitch_bighead_deflate";

		// Token: 0x04009D1A RID: 40218
		public const string TwitchCelebrate = "twitch_celebrate";

		// Token: 0x04009D1B RID: 40219
		public const string TwitchBalloonPop = "twitch_baseball_balloon_pop";

		// Token: 0x04009D1C RID: 40220
		public const string TwitchBalloonSpawn = "twitch_balloon_spawn";

		// Token: 0x04009D1D RID: 40221
		public const string TwitchBalloonDespawn = "twitch_balloon_despawn";

		// Token: 0x04009D1E RID: 40222
		public const string TwitchRefund = "twitch_refund";

		// Token: 0x04009D1F RID: 40223
		public const string TwitchPausedActions = "twitch_pause";

		// Token: 0x04009D20 RID: 40224
		public const string TwitchUnPausedActions = "twitch_unpause";

		// Token: 0x04009D21 RID: 40225
		public const string PartyInviteRecieve = "party_invite_receive";

		// Token: 0x04009D22 RID: 40226
		public const string PartyJoin = "party_join";

		// Token: 0x04009D23 RID: 40227
		public const string PartyLeave = "party_leave";

		// Token: 0x04009D24 RID: 40228
		public const string PartyMemberJoin = "party_member_join";

		// Token: 0x04009D25 RID: 40229
		public const string PartyMemberLeave = "party_member_leave";

		// Token: 0x04009D26 RID: 40230
		public const string ChallengeTrack = "ui_challenge_track";

		// Token: 0x04009D27 RID: 40231
		public const string ChallengeRedeem = "ui_challenge_redeem";

		// Token: 0x04009D28 RID: 40232
		public const string ChallengeComplete = "ui_challenge_complete";

		// Token: 0x04009D29 RID: 40233
		public const string ChallengeCompleteRow = "ui_challenge_complete_row";

		// Token: 0x04009D2A RID: 40234
		public const string ChallengeUnhideRow = "ui_challenge_unhide_row";

		// Token: 0x04009D2B RID: 40235
		public const string ChallengeObjectiveComplete = "ui_challenge_objective_complete";
	}
}
