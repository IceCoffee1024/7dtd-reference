using System;

namespace Audio
{
	// Token: 0x02001B3B RID: 6971
	public class StrAudioClip
	{
		// Token: 0x04009EB9 RID: 40633
		public const string ItemCollected = "item_pickup";

		// Token: 0x04009EBA RID: 40634
		public const string ItemPlantCollected = "item_plant_pickup";

		// Token: 0x04009EBB RID: 40635
		public const string EntityHitsGround = "entityhitsground";

		// Token: 0x04009EBC RID: 40636
		public const string ItemDropped = "itemdropped";

		// Token: 0x04009EBD RID: 40637
		public const string OpenDoor = "open_door_wood";

		// Token: 0x04009EBE RID: 40638
		public const string CloseDoor = "close_door_wood";

		// Token: 0x04009EBF RID: 40639
		public const string FallingIntoWater = "waterfallinginto";

		// Token: 0x04009EC0 RID: 40640
		public const string BuildingCompleted = "placeblock";

		// Token: 0x04009EC1 RID: 40641
		public const string RotateBlock = "rotateblock";

		// Token: 0x04009EC2 RID: 40642
		public const string TrunkBreaks = "trunkbreak";

		// Token: 0x04009EC3 RID: 40643
		public const string TrunkFallImpact = "treefallimpact";

		// Token: 0x04009EC4 RID: 40644
		public const string FlashlightToggle = "flashlight_toggle";

		// Token: 0x04009EC5 RID: 40645
		public const string GenericHolster = "generic_holster";

		// Token: 0x04009EC6 RID: 40646
		public const string GenericUnholster = "generic_unholster";

		// Token: 0x04009EC7 RID: 40647
		public const string MissingItemToRepair = "missingitemtorepair";

		// Token: 0x04009EC8 RID: 40648
		public const string CraftingClick = "craft_click_craft";

		// Token: 0x04009EC9 RID: 40649
		public const string RecipeUnlocked = "recipe_unlocked";

		// Token: 0x04009ECA RID: 40650
		public const string OpenInventory = "open_inventory";

		// Token: 0x04009ECB RID: 40651
		public const string CloseInventory = "close_inventory";

		// Token: 0x04009ECC RID: 40652
		public const string CampfireOpen = "campfire_open";

		// Token: 0x04009ECD RID: 40653
		public const string CampfireClose = "campfire_close";

		// Token: 0x04009ECE RID: 40654
		public const string CampfireCookClick = "campfire_cook_click";

		// Token: 0x04009ECF RID: 40655
		public const string ForgeOpen = "forge_open";

		// Token: 0x04009ED0 RID: 40656
		public const string ForgeClose = "forge_close";

		// Token: 0x04009ED1 RID: 40657
		public const string ForgeSmeltClick = "forge_smelt_click";

		// Token: 0x04009ED2 RID: 40658
		public const string ForgeBurn = "forge_burn_fuel";

		// Token: 0x04009ED3 RID: 40659
		public const string ForgeFireDie = "forge_fire_die";

		// Token: 0x04009ED4 RID: 40660
		public const string CementMixerOpen = "cement_mixer_open";

		// Token: 0x04009ED5 RID: 40661
		public const string CementMixerClose = "cement_mixer_close";

		// Token: 0x04009ED6 RID: 40662
		public const string CementMixerClick = "cement_mixer_start_click";

		// Token: 0x04009ED7 RID: 40663
		public const string CraftComplete = "craft_complete_item";

		// Token: 0x04009ED8 RID: 40664
		public const string CampfireComplete = "campfire_complete_item";

		// Token: 0x04009ED9 RID: 40665
		public const string ForgeComplete = "forge_item_complete";

		// Token: 0x04009EDA RID: 40666
		public const string CementMixerComplete = "cement_mixer_complete";

		// Token: 0x04009EDB RID: 40667
		public const string ChemStationOpen = "chem_station_open";

		// Token: 0x04009EDC RID: 40668
		public const string ChemStationClose = "chem_station_close";

		// Token: 0x04009EDD RID: 40669
		public const string ChemStationClick = "chem_station_mix_click";

		// Token: 0x04009EDE RID: 40670
		public const string ChemStationComplete = "chem_station_complete_item";

		// Token: 0x04009EDF RID: 40671
		public const string QuestNoteOffered = "quest_note_offer";

		// Token: 0x04009EE0 RID: 40672
		public const string QuestNoteDeclined = "quest_note_decline";

		// Token: 0x04009EE1 RID: 40673
		public const string QuestStarted = "quest_started";

		// Token: 0x04009EE2 RID: 40674
		public const string QuestFailed = "quest_failed";

		// Token: 0x04009EE3 RID: 40675
		public const string QuestCompleted = "quest_subtask_complete";

		// Token: 0x04009EE4 RID: 40676
		public const string QuestObjective = "quest_objective_complete";

		// Token: 0x04009EE5 RID: 40677
		public const string QuestChainCompleted = "quest_master_complete";

		// Token: 0x04009EE6 RID: 40678
		public const string SkillPurchase = "ui_skill_purchase";

		// Token: 0x04009EE7 RID: 40679
		public const string TraderPurchase = "ui_trader_purchase";

		// Token: 0x04009EE8 RID: 40680
		public const string VendingPurchase = "ui_vending_purchase";

		// Token: 0x04009EE9 RID: 40681
		public const string VendingOpen = "open_vending";

		// Token: 0x04009EEA RID: 40682
		public const string VendingClose = "close_vending";

		// Token: 0x04009EEB RID: 40683
		public const string UITab = "ui_tab";

		// Token: 0x04009EEC RID: 40684
		public const string UIHover = "ui_hover";

		// Token: 0x04009EED RID: 40685
		public const string UIDenied = "ui_denied";

		// Token: 0x04009EEE RID: 40686
		public const string MapZoomIn = "map_zoom_in";

		// Token: 0x04009EEF RID: 40687
		public const string MapZoomOut = "map_zoom_out";

		// Token: 0x04009EF0 RID: 40688
		public const string WaypointAdd = "ui_waypoint_add";

		// Token: 0x04009EF1 RID: 40689
		public const string WaypointDelete = "ui_waypoint_delete";

		// Token: 0x04009EF2 RID: 40690
		public const string PickupItem = "craft_take_item";

		// Token: 0x04009EF3 RID: 40691
		public const string PlaceItem = "craft_place_item";

		// Token: 0x04009EF4 RID: 40692
		public const string SignOpen = "open_sign";

		// Token: 0x04009EF5 RID: 40693
		public const string SignClose = "close_sign";

		// Token: 0x04009EF6 RID: 40694
		public const string BatteryBankStart = "batterybank_start";

		// Token: 0x04009EF7 RID: 40695
		public const string BatteryBankStop = "batterybank_stop";

		// Token: 0x04009EF8 RID: 40696
		public const string GeneratorStart = "generator_start";

		// Token: 0x04009EF9 RID: 40697
		public const string GeneratorStop = "generator_stop";

		// Token: 0x04009EFA RID: 40698
		public const string SolarPanelStart = "solarpanel_on";

		// Token: 0x04009EFB RID: 40699
		public const string SolarPanelStop = "solarpanel_off";

		// Token: 0x04009EFC RID: 40700
		public const string SwitchOn = "switch_up";

		// Token: 0x04009EFD RID: 40701
		public const string SwitchOff = "switch_down";

		// Token: 0x04009EFE RID: 40702
		public const string WireConnectLive = "wire_live_connect";

		// Token: 0x04009EFF RID: 40703
		public const string WireConnectDead = "wire_dead_connect";

		// Token: 0x04009F00 RID: 40704
		public const string WireBreakLive = "wire_live_break";

		// Token: 0x04009F01 RID: 40705
		public const string WireBreakDead = "wire_dead_break";

		// Token: 0x04009F02 RID: 40706
		public const string PressurePlateDown = "pressureplate_down";

		// Token: 0x04009F03 RID: 40707
		public const string PressurePlateUp = "pressureplate_up";

		// Token: 0x04009F04 RID: 40708
		public const string MotionSensorTrigger = "motion_sensor_trigger";

		// Token: 0x04009F05 RID: 40709
		public const string TripWireTrigger = "trip_wire_trigger";

		// Token: 0x04009F06 RID: 40710
		public const string LightOn = "light_on";

		// Token: 0x04009F07 RID: 40711
		public const string LightOff = "light_off";

		// Token: 0x04009F08 RID: 40712
		public const string TimerRelayStart = "timer_start";

		// Token: 0x04009F09 RID: 40713
		public const string TimerRelayStop = "timer_stop";

		// Token: 0x04009F0A RID: 40714
		public const string ItemBreak = "itembreak";

		// Token: 0x04009F0B RID: 40715
		public const string TwitchNoAttack = "twitch_no_attack";

		// Token: 0x04009F0C RID: 40716
		public const string TwitchVoteAdded = "twitch_vote_received";

		// Token: 0x04009F0D RID: 40717
		public const string TwitchVoteStarted = "twitch_vote_started";

		// Token: 0x04009F0E RID: 40718
		public const string TwitchVoteEnded = "twitch_vote_ended";

		// Token: 0x04009F0F RID: 40719
		public const string TwitchInflate = "twitch_bighead_inflate";

		// Token: 0x04009F10 RID: 40720
		public const string TwitchDeflate = "twitch_bighead_deflate";

		// Token: 0x04009F11 RID: 40721
		public const string TwitchCelebrate = "twitch_celebrate";

		// Token: 0x04009F12 RID: 40722
		public const string TwitchBalloonPop = "twitch_baseball_balloon_pop";

		// Token: 0x04009F13 RID: 40723
		public const string TwitchBalloonSpawn = "twitch_balloon_spawn";

		// Token: 0x04009F14 RID: 40724
		public const string TwitchBalloonDespawn = "twitch_balloon_despawn";

		// Token: 0x04009F15 RID: 40725
		public const string TwitchRefund = "twitch_refund";

		// Token: 0x04009F16 RID: 40726
		public const string TwitchPausedActions = "twitch_pause";

		// Token: 0x04009F17 RID: 40727
		public const string TwitchUnPausedActions = "twitch_unpause";

		// Token: 0x04009F18 RID: 40728
		public const string PartyInviteRecieve = "party_invite_receive";

		// Token: 0x04009F19 RID: 40729
		public const string PartyJoin = "party_join";

		// Token: 0x04009F1A RID: 40730
		public const string PartyLeave = "party_leave";

		// Token: 0x04009F1B RID: 40731
		public const string PartyMemberJoin = "party_member_join";

		// Token: 0x04009F1C RID: 40732
		public const string PartyMemberLeave = "party_member_leave";

		// Token: 0x04009F1D RID: 40733
		public const string ChallengeTrack = "ui_challenge_track";

		// Token: 0x04009F1E RID: 40734
		public const string ChallengeRedeem = "ui_challenge_redeem";

		// Token: 0x04009F1F RID: 40735
		public const string ChallengeComplete = "ui_challenge_complete";

		// Token: 0x04009F20 RID: 40736
		public const string ChallengeCompleteRow = "ui_challenge_complete_row";

		// Token: 0x04009F21 RID: 40737
		public const string ChallengeUnhideRow = "ui_challenge_unhide_row";

		// Token: 0x04009F22 RID: 40738
		public const string ChallengeObjectiveComplete = "ui_challenge_objective_complete";
	}
}
