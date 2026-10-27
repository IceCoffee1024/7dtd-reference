using System;

namespace Audio
{
	// Token: 0x02001B6C RID: 7020
	public class StrAudioClip
	{
		// Token: 0x04009FA2 RID: 40866
		public const string ItemCollected = "item_pickup";

		// Token: 0x04009FA3 RID: 40867
		public const string ItemPlantCollected = "item_plant_pickup";

		// Token: 0x04009FA4 RID: 40868
		public const string EntityHitsGround = "entityhitsground";

		// Token: 0x04009FA5 RID: 40869
		public const string ItemDropped = "itemdropped";

		// Token: 0x04009FA6 RID: 40870
		public const string OpenDoor = "open_door_wood";

		// Token: 0x04009FA7 RID: 40871
		public const string CloseDoor = "close_door_wood";

		// Token: 0x04009FA8 RID: 40872
		public const string FallingIntoWater = "waterfallinginto";

		// Token: 0x04009FA9 RID: 40873
		public const string BuildingCompleted = "placeblock";

		// Token: 0x04009FAA RID: 40874
		public const string RotateBlock = "rotateblock";

		// Token: 0x04009FAB RID: 40875
		public const string TrunkBreaks = "trunkbreak";

		// Token: 0x04009FAC RID: 40876
		public const string TrunkFallImpact = "treefallimpact";

		// Token: 0x04009FAD RID: 40877
		public const string FlashlightToggle = "flashlight_toggle";

		// Token: 0x04009FAE RID: 40878
		public const string GenericHolster = "generic_holster";

		// Token: 0x04009FAF RID: 40879
		public const string GenericUnholster = "generic_unholster";

		// Token: 0x04009FB0 RID: 40880
		public const string MissingItemToRepair = "missingitemtorepair";

		// Token: 0x04009FB1 RID: 40881
		public const string CraftingClick = "craft_click_craft";

		// Token: 0x04009FB2 RID: 40882
		public const string RecipeUnlocked = "recipe_unlocked";

		// Token: 0x04009FB3 RID: 40883
		public const string OpenInventory = "open_inventory";

		// Token: 0x04009FB4 RID: 40884
		public const string CloseInventory = "close_inventory";

		// Token: 0x04009FB5 RID: 40885
		public const string CampfireOpen = "campfire_open";

		// Token: 0x04009FB6 RID: 40886
		public const string CampfireClose = "campfire_close";

		// Token: 0x04009FB7 RID: 40887
		public const string CampfireCookClick = "campfire_cook_click";

		// Token: 0x04009FB8 RID: 40888
		public const string ForgeOpen = "forge_open";

		// Token: 0x04009FB9 RID: 40889
		public const string ForgeClose = "forge_close";

		// Token: 0x04009FBA RID: 40890
		public const string ForgeSmeltClick = "forge_smelt_click";

		// Token: 0x04009FBB RID: 40891
		public const string ForgeBurn = "forge_burn_fuel";

		// Token: 0x04009FBC RID: 40892
		public const string ForgeFireDie = "forge_fire_die";

		// Token: 0x04009FBD RID: 40893
		public const string CementMixerOpen = "cement_mixer_open";

		// Token: 0x04009FBE RID: 40894
		public const string CementMixerClose = "cement_mixer_close";

		// Token: 0x04009FBF RID: 40895
		public const string CementMixerClick = "cement_mixer_start_click";

		// Token: 0x04009FC0 RID: 40896
		public const string CraftComplete = "craft_complete_item";

		// Token: 0x04009FC1 RID: 40897
		public const string CampfireComplete = "campfire_complete_item";

		// Token: 0x04009FC2 RID: 40898
		public const string ForgeComplete = "forge_item_complete";

		// Token: 0x04009FC3 RID: 40899
		public const string CementMixerComplete = "cement_mixer_complete";

		// Token: 0x04009FC4 RID: 40900
		public const string ChemStationOpen = "chem_station_open";

		// Token: 0x04009FC5 RID: 40901
		public const string ChemStationClose = "chem_station_close";

		// Token: 0x04009FC6 RID: 40902
		public const string ChemStationClick = "chem_station_mix_click";

		// Token: 0x04009FC7 RID: 40903
		public const string ChemStationComplete = "chem_station_complete_item";

		// Token: 0x04009FC8 RID: 40904
		public const string QuestNoteOffered = "quest_note_offer";

		// Token: 0x04009FC9 RID: 40905
		public const string QuestNoteDeclined = "quest_note_decline";

		// Token: 0x04009FCA RID: 40906
		public const string QuestStarted = "quest_started";

		// Token: 0x04009FCB RID: 40907
		public const string QuestFailed = "quest_failed";

		// Token: 0x04009FCC RID: 40908
		public const string QuestCompleted = "quest_subtask_complete";

		// Token: 0x04009FCD RID: 40909
		public const string QuestObjective = "quest_objective_complete";

		// Token: 0x04009FCE RID: 40910
		public const string QuestChainCompleted = "quest_master_complete";

		// Token: 0x04009FCF RID: 40911
		public const string SkillPurchase = "ui_skill_purchase";

		// Token: 0x04009FD0 RID: 40912
		public const string TraderPurchase = "ui_trader_purchase";

		// Token: 0x04009FD1 RID: 40913
		public const string VendingPurchase = "ui_vending_purchase";

		// Token: 0x04009FD2 RID: 40914
		public const string VendingOpen = "open_vending";

		// Token: 0x04009FD3 RID: 40915
		public const string VendingClose = "close_vending";

		// Token: 0x04009FD4 RID: 40916
		public const string UITab = "ui_tab";

		// Token: 0x04009FD5 RID: 40917
		public const string UIHover = "ui_hover";

		// Token: 0x04009FD6 RID: 40918
		public const string UIDenied = "ui_denied";

		// Token: 0x04009FD7 RID: 40919
		public const string MapZoomIn = "map_zoom_in";

		// Token: 0x04009FD8 RID: 40920
		public const string MapZoomOut = "map_zoom_out";

		// Token: 0x04009FD9 RID: 40921
		public const string WaypointAdd = "ui_waypoint_add";

		// Token: 0x04009FDA RID: 40922
		public const string WaypointDelete = "ui_waypoint_delete";

		// Token: 0x04009FDB RID: 40923
		public const string PickupItem = "craft_take_item";

		// Token: 0x04009FDC RID: 40924
		public const string PlaceItem = "craft_place_item";

		// Token: 0x04009FDD RID: 40925
		public const string SignOpen = "open_sign";

		// Token: 0x04009FDE RID: 40926
		public const string SignClose = "close_sign";

		// Token: 0x04009FDF RID: 40927
		public const string BatteryBankStart = "batterybank_start";

		// Token: 0x04009FE0 RID: 40928
		public const string BatteryBankStop = "batterybank_stop";

		// Token: 0x04009FE1 RID: 40929
		public const string GeneratorStart = "generator_start";

		// Token: 0x04009FE2 RID: 40930
		public const string GeneratorStop = "generator_stop";

		// Token: 0x04009FE3 RID: 40931
		public const string SolarPanelStart = "solarpanel_on";

		// Token: 0x04009FE4 RID: 40932
		public const string SolarPanelStop = "solarpanel_off";

		// Token: 0x04009FE5 RID: 40933
		public const string SwitchOn = "switch_up";

		// Token: 0x04009FE6 RID: 40934
		public const string SwitchOff = "switch_down";

		// Token: 0x04009FE7 RID: 40935
		public const string WireConnectLive = "wire_live_connect";

		// Token: 0x04009FE8 RID: 40936
		public const string WireConnectDead = "wire_dead_connect";

		// Token: 0x04009FE9 RID: 40937
		public const string WireBreakLive = "wire_live_break";

		// Token: 0x04009FEA RID: 40938
		public const string WireBreakDead = "wire_dead_break";

		// Token: 0x04009FEB RID: 40939
		public const string PressurePlateDown = "pressureplate_down";

		// Token: 0x04009FEC RID: 40940
		public const string PressurePlateUp = "pressureplate_up";

		// Token: 0x04009FED RID: 40941
		public const string MotionSensorTrigger = "motion_sensor_trigger";

		// Token: 0x04009FEE RID: 40942
		public const string TripWireTrigger = "trip_wire_trigger";

		// Token: 0x04009FEF RID: 40943
		public const string LightOn = "light_on";

		// Token: 0x04009FF0 RID: 40944
		public const string LightOff = "light_off";

		// Token: 0x04009FF1 RID: 40945
		public const string TimerRelayStart = "timer_start";

		// Token: 0x04009FF2 RID: 40946
		public const string TimerRelayStop = "timer_stop";

		// Token: 0x04009FF3 RID: 40947
		public const string ItemBreak = "itembreak";

		// Token: 0x04009FF4 RID: 40948
		public const string TwitchNoAttack = "twitch_no_attack";

		// Token: 0x04009FF5 RID: 40949
		public const string TwitchVoteAdded = "twitch_vote_received";

		// Token: 0x04009FF6 RID: 40950
		public const string TwitchVoteStarted = "twitch_vote_started";

		// Token: 0x04009FF7 RID: 40951
		public const string TwitchVoteEnded = "twitch_vote_ended";

		// Token: 0x04009FF8 RID: 40952
		public const string TwitchInflate = "twitch_bighead_inflate";

		// Token: 0x04009FF9 RID: 40953
		public const string TwitchDeflate = "twitch_bighead_deflate";

		// Token: 0x04009FFA RID: 40954
		public const string TwitchCelebrate = "twitch_celebrate";

		// Token: 0x04009FFB RID: 40955
		public const string TwitchBalloonPop = "twitch_baseball_balloon_pop";

		// Token: 0x04009FFC RID: 40956
		public const string TwitchBalloonSpawn = "twitch_balloon_spawn";

		// Token: 0x04009FFD RID: 40957
		public const string TwitchBalloonDespawn = "twitch_balloon_despawn";

		// Token: 0x04009FFE RID: 40958
		public const string TwitchRefund = "twitch_refund";

		// Token: 0x04009FFF RID: 40959
		public const string TwitchPausedActions = "twitch_pause";

		// Token: 0x0400A000 RID: 40960
		public const string TwitchUnPausedActions = "twitch_unpause";

		// Token: 0x0400A001 RID: 40961
		public const string PartyInviteRecieve = "party_invite_receive";

		// Token: 0x0400A002 RID: 40962
		public const string PartyJoin = "party_join";

		// Token: 0x0400A003 RID: 40963
		public const string PartyLeave = "party_leave";

		// Token: 0x0400A004 RID: 40964
		public const string PartyMemberJoin = "party_member_join";

		// Token: 0x0400A005 RID: 40965
		public const string PartyMemberLeave = "party_member_leave";

		// Token: 0x0400A006 RID: 40966
		public const string ChallengeTrack = "ui_challenge_track";

		// Token: 0x0400A007 RID: 40967
		public const string ChallengeRedeem = "ui_challenge_redeem";

		// Token: 0x0400A008 RID: 40968
		public const string ChallengeComplete = "ui_challenge_complete";

		// Token: 0x0400A009 RID: 40969
		public const string ChallengeCompleteRow = "ui_challenge_complete_row";

		// Token: 0x0400A00A RID: 40970
		public const string ChallengeUnhideRow = "ui_challenge_unhide_row";

		// Token: 0x0400A00B RID: 40971
		public const string ChallengeObjectiveComplete = "ui_challenge_objective_complete";
	}
}
