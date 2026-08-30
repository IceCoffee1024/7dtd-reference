using System;

namespace Audio
{
	// Token: 0x02001B0F RID: 6927
	public class StrAudioClip
	{
		// Token: 0x04009DF3 RID: 40435
		public const string ItemCollected = "item_pickup";

		// Token: 0x04009DF4 RID: 40436
		public const string ItemPlantCollected = "item_plant_pickup";

		// Token: 0x04009DF5 RID: 40437
		public const string EntityHitsGround = "entityhitsground";

		// Token: 0x04009DF6 RID: 40438
		public const string ItemDropped = "itemdropped";

		// Token: 0x04009DF7 RID: 40439
		public const string OpenDoor = "open_door_wood";

		// Token: 0x04009DF8 RID: 40440
		public const string CloseDoor = "close_door_wood";

		// Token: 0x04009DF9 RID: 40441
		public const string FallingIntoWater = "waterfallinginto";

		// Token: 0x04009DFA RID: 40442
		public const string BuildingCompleted = "placeblock";

		// Token: 0x04009DFB RID: 40443
		public const string RotateBlock = "rotateblock";

		// Token: 0x04009DFC RID: 40444
		public const string TrunkBreaks = "trunkbreak";

		// Token: 0x04009DFD RID: 40445
		public const string TrunkFallImpact = "treefallimpact";

		// Token: 0x04009DFE RID: 40446
		public const string FlashlightToggle = "flashlight_toggle";

		// Token: 0x04009DFF RID: 40447
		public const string GenericHolster = "generic_holster";

		// Token: 0x04009E00 RID: 40448
		public const string GenericUnholster = "generic_unholster";

		// Token: 0x04009E01 RID: 40449
		public const string MissingItemToRepair = "missingitemtorepair";

		// Token: 0x04009E02 RID: 40450
		public const string CraftingClick = "craft_click_craft";

		// Token: 0x04009E03 RID: 40451
		public const string RecipeUnlocked = "recipe_unlocked";

		// Token: 0x04009E04 RID: 40452
		public const string OpenInventory = "open_inventory";

		// Token: 0x04009E05 RID: 40453
		public const string CloseInventory = "close_inventory";

		// Token: 0x04009E06 RID: 40454
		public const string CampfireOpen = "campfire_open";

		// Token: 0x04009E07 RID: 40455
		public const string CampfireClose = "campfire_close";

		// Token: 0x04009E08 RID: 40456
		public const string CampfireCookClick = "campfire_cook_click";

		// Token: 0x04009E09 RID: 40457
		public const string ForgeOpen = "forge_open";

		// Token: 0x04009E0A RID: 40458
		public const string ForgeClose = "forge_close";

		// Token: 0x04009E0B RID: 40459
		public const string ForgeSmeltClick = "forge_smelt_click";

		// Token: 0x04009E0C RID: 40460
		public const string ForgeBurn = "forge_burn_fuel";

		// Token: 0x04009E0D RID: 40461
		public const string ForgeFireDie = "forge_fire_die";

		// Token: 0x04009E0E RID: 40462
		public const string CementMixerOpen = "cement_mixer_open";

		// Token: 0x04009E0F RID: 40463
		public const string CementMixerClose = "cement_mixer_close";

		// Token: 0x04009E10 RID: 40464
		public const string CementMixerClick = "cement_mixer_start_click";

		// Token: 0x04009E11 RID: 40465
		public const string CraftComplete = "craft_complete_item";

		// Token: 0x04009E12 RID: 40466
		public const string CampfireComplete = "campfire_complete_item";

		// Token: 0x04009E13 RID: 40467
		public const string ForgeComplete = "forge_item_complete";

		// Token: 0x04009E14 RID: 40468
		public const string CementMixerComplete = "cement_mixer_complete";

		// Token: 0x04009E15 RID: 40469
		public const string ChemStationOpen = "chem_station_open";

		// Token: 0x04009E16 RID: 40470
		public const string ChemStationClose = "chem_station_close";

		// Token: 0x04009E17 RID: 40471
		public const string ChemStationClick = "chem_station_mix_click";

		// Token: 0x04009E18 RID: 40472
		public const string ChemStationComplete = "chem_station_complete_item";

		// Token: 0x04009E19 RID: 40473
		public const string QuestNoteOffered = "quest_note_offer";

		// Token: 0x04009E1A RID: 40474
		public const string QuestNoteDeclined = "quest_note_decline";

		// Token: 0x04009E1B RID: 40475
		public const string QuestStarted = "quest_started";

		// Token: 0x04009E1C RID: 40476
		public const string QuestFailed = "quest_failed";

		// Token: 0x04009E1D RID: 40477
		public const string QuestCompleted = "quest_subtask_complete";

		// Token: 0x04009E1E RID: 40478
		public const string QuestObjective = "quest_objective_complete";

		// Token: 0x04009E1F RID: 40479
		public const string QuestChainCompleted = "quest_master_complete";

		// Token: 0x04009E20 RID: 40480
		public const string SkillPurchase = "ui_skill_purchase";

		// Token: 0x04009E21 RID: 40481
		public const string TraderPurchase = "ui_trader_purchase";

		// Token: 0x04009E22 RID: 40482
		public const string VendingPurchase = "ui_vending_purchase";

		// Token: 0x04009E23 RID: 40483
		public const string VendingOpen = "open_vending";

		// Token: 0x04009E24 RID: 40484
		public const string VendingClose = "close_vending";

		// Token: 0x04009E25 RID: 40485
		public const string UITab = "ui_tab";

		// Token: 0x04009E26 RID: 40486
		public const string UIHover = "ui_hover";

		// Token: 0x04009E27 RID: 40487
		public const string UIDenied = "ui_denied";

		// Token: 0x04009E28 RID: 40488
		public const string MapZoomIn = "map_zoom_in";

		// Token: 0x04009E29 RID: 40489
		public const string MapZoomOut = "map_zoom_out";

		// Token: 0x04009E2A RID: 40490
		public const string WaypointAdd = "ui_waypoint_add";

		// Token: 0x04009E2B RID: 40491
		public const string WaypointDelete = "ui_waypoint_delete";

		// Token: 0x04009E2C RID: 40492
		public const string PickupItem = "craft_take_item";

		// Token: 0x04009E2D RID: 40493
		public const string PlaceItem = "craft_place_item";

		// Token: 0x04009E2E RID: 40494
		public const string SignOpen = "open_sign";

		// Token: 0x04009E2F RID: 40495
		public const string SignClose = "close_sign";

		// Token: 0x04009E30 RID: 40496
		public const string BatteryBankStart = "batterybank_start";

		// Token: 0x04009E31 RID: 40497
		public const string BatteryBankStop = "batterybank_stop";

		// Token: 0x04009E32 RID: 40498
		public const string GeneratorStart = "generator_start";

		// Token: 0x04009E33 RID: 40499
		public const string GeneratorStop = "generator_stop";

		// Token: 0x04009E34 RID: 40500
		public const string SolarPanelStart = "solarpanel_on";

		// Token: 0x04009E35 RID: 40501
		public const string SolarPanelStop = "solarpanel_off";

		// Token: 0x04009E36 RID: 40502
		public const string SwitchOn = "switch_up";

		// Token: 0x04009E37 RID: 40503
		public const string SwitchOff = "switch_down";

		// Token: 0x04009E38 RID: 40504
		public const string WireConnectLive = "wire_live_connect";

		// Token: 0x04009E39 RID: 40505
		public const string WireConnectDead = "wire_dead_connect";

		// Token: 0x04009E3A RID: 40506
		public const string WireBreakLive = "wire_live_break";

		// Token: 0x04009E3B RID: 40507
		public const string WireBreakDead = "wire_dead_break";

		// Token: 0x04009E3C RID: 40508
		public const string PressurePlateDown = "pressureplate_down";

		// Token: 0x04009E3D RID: 40509
		public const string PressurePlateUp = "pressureplate_up";

		// Token: 0x04009E3E RID: 40510
		public const string MotionSensorTrigger = "motion_sensor_trigger";

		// Token: 0x04009E3F RID: 40511
		public const string TripWireTrigger = "trip_wire_trigger";

		// Token: 0x04009E40 RID: 40512
		public const string LightOn = "light_on";

		// Token: 0x04009E41 RID: 40513
		public const string LightOff = "light_off";

		// Token: 0x04009E42 RID: 40514
		public const string TimerRelayStart = "timer_start";

		// Token: 0x04009E43 RID: 40515
		public const string TimerRelayStop = "timer_stop";

		// Token: 0x04009E44 RID: 40516
		public const string ItemBreak = "itembreak";

		// Token: 0x04009E45 RID: 40517
		public const string TwitchNoAttack = "twitch_no_attack";

		// Token: 0x04009E46 RID: 40518
		public const string TwitchVoteAdded = "twitch_vote_received";

		// Token: 0x04009E47 RID: 40519
		public const string TwitchVoteStarted = "twitch_vote_started";

		// Token: 0x04009E48 RID: 40520
		public const string TwitchVoteEnded = "twitch_vote_ended";

		// Token: 0x04009E49 RID: 40521
		public const string TwitchInflate = "twitch_bighead_inflate";

		// Token: 0x04009E4A RID: 40522
		public const string TwitchDeflate = "twitch_bighead_deflate";

		// Token: 0x04009E4B RID: 40523
		public const string TwitchCelebrate = "twitch_celebrate";

		// Token: 0x04009E4C RID: 40524
		public const string TwitchBalloonPop = "twitch_baseball_balloon_pop";

		// Token: 0x04009E4D RID: 40525
		public const string TwitchBalloonSpawn = "twitch_balloon_spawn";

		// Token: 0x04009E4E RID: 40526
		public const string TwitchBalloonDespawn = "twitch_balloon_despawn";

		// Token: 0x04009E4F RID: 40527
		public const string TwitchRefund = "twitch_refund";

		// Token: 0x04009E50 RID: 40528
		public const string TwitchPausedActions = "twitch_pause";

		// Token: 0x04009E51 RID: 40529
		public const string TwitchUnPausedActions = "twitch_unpause";

		// Token: 0x04009E52 RID: 40530
		public const string PartyInviteRecieve = "party_invite_receive";

		// Token: 0x04009E53 RID: 40531
		public const string PartyJoin = "party_join";

		// Token: 0x04009E54 RID: 40532
		public const string PartyLeave = "party_leave";

		// Token: 0x04009E55 RID: 40533
		public const string PartyMemberJoin = "party_member_join";

		// Token: 0x04009E56 RID: 40534
		public const string PartyMemberLeave = "party_member_leave";

		// Token: 0x04009E57 RID: 40535
		public const string ChallengeTrack = "ui_challenge_track";

		// Token: 0x04009E58 RID: 40536
		public const string ChallengeRedeem = "ui_challenge_redeem";

		// Token: 0x04009E59 RID: 40537
		public const string ChallengeComplete = "ui_challenge_complete";

		// Token: 0x04009E5A RID: 40538
		public const string ChallengeCompleteRow = "ui_challenge_complete_row";

		// Token: 0x04009E5B RID: 40539
		public const string ChallengeUnhideRow = "ui_challenge_unhide_row";

		// Token: 0x04009E5C RID: 40540
		public const string ChallengeObjectiveComplete = "ui_challenge_objective_complete";
	}
}
