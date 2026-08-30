using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CA0 RID: 7328
	public class RichPresence : IRichPresence
	{
		// Token: 0x0600D954 RID: 55636 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D955 RID: 55637 RVA: 0x004E02EC File Offset: 0x004DE4EC
		public void UpdateRichPresence(IRichPresence.PresenceStates state)
		{
			World world = GameManager.Instance.World;
			switch (state)
			{
			case IRichPresence.PresenceStates.Menu:
				SteamFriends.ClearRichPresence();
				SteamFriends.SetRichPresence("steam_display", "#Status_AtMainMenu");
				this.localPlayer = null;
				return;
			case IRichPresence.PresenceStates.Loading:
				SteamFriends.ClearRichPresence();
				SteamFriends.SetRichPresence("steam_display", "#Status_LoadingGame");
				this.localPlayer = null;
				return;
			case IRichPresence.PresenceStates.Connecting:
				SteamFriends.ClearRichPresence();
				SteamFriends.SetRichPresence("steam_display", "#Status_ConnectingToServer");
				this.localPlayer = null;
				return;
			case IRichPresence.PresenceStates.InGame:
			{
				if (this.localPlayer == null)
				{
					this.localPlayer = world.GetPrimaryPlayer();
				}
				if (this.localPlayer == null)
				{
					return;
				}
				GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
				SteamFriends.SetRichPresence("steam_player_group", gameServerInfo.GetValue(GameInfoString.UniqueId));
				SteamFriends.SetRichPresence("steam_player_group_size", world.Players.Count.ToString());
				SteamFriends.SetRichPresence("day", string.Format("{0:0}", GameUtils.WorldTimeToDays(world.worldTime)));
				float num = SkyManager.TimeOfDay();
				string text;
				if (num < (float)world.DawnHour || num >= (float)world.DuskHour)
				{
					text = "Night";
				}
				else if (num >= (float)world.DawnHour && num < 12f)
				{
					text = "Morning";
				}
				else
				{
					text = "Afternoon";
				}
				if (this.localPlayer.biomeStandingOn == null)
				{
					SteamFriends.ClearRichPresence();
					return;
				}
				EntityVehicle entityVehicle = this.localPlayer.AttachedToEntity as EntityVehicle;
				bool flag = this.localPlayer.Stats.AmountEnclosed > 0f;
				Quest activeQuest = this.localPlayer.QuestJournal.ActiveQuest;
				bool flag2 = GameManager.Instance.World.IsWorldEvent(World.WorldEvent.BloodMoon);
				if (this.localPlayer.biomeStandingOn != this.currentBiome || this.localPlayer.prefab != this.currentPrefab || text != this.currentTimeOfDay || flag != this.currentIndoors || entityVehicle != this.currentDriving || activeQuest != this.currentQuest)
				{
					this.currentTimeOfDay = text;
					this.currentBiome = this.localPlayer.biomeStandingOn;
					this.currentPrefab = this.localPlayer.prefab;
					this.currentIndoors = flag;
					this.currentDriving = entityVehicle;
					this.currentQuest = activeQuest;
					this.currentBloodmoon = flag2;
					SteamFriends.SetRichPresence("timeofday", this.currentTimeOfDay);
					bool flag3 = false;
					if (this.currentBloodmoon)
					{
						SteamFriends.SetRichPresence("description", "Surviving the Bloodmoon in the " + this.currentBiome.LocalizedName);
						flag3 = true;
					}
					if (!flag3 && this.currentQuest != null)
					{
						flag3 = true;
						if (this.currentQuest.QuestTags.Test_AnySet(QuestEventManager.restorePowerTag))
						{
							SteamFriends.SetRichPresence("description", "Restoring power in the " + this.currentBiome.LocalizedName);
						}
						else if (this.currentQuest.QuestTags.Test_AnySet(QuestEventManager.infestedTag))
						{
							SteamFriends.SetRichPresence("description", "Clearing Infestation in the " + this.currentBiome.LocalizedName);
						}
						else if (this.currentQuest.QuestTags.Test_AnySet(QuestEventManager.fetchTag))
						{
							SteamFriends.SetRichPresence("description", "Recovering Supplies in the " + this.currentBiome.LocalizedName);
						}
						else if (this.currentQuest.QuestTags.Test_AnySet(QuestEventManager.treasureTag))
						{
							SteamFriends.SetRichPresence("description", "Digging up Supplies in the " + this.currentBiome.LocalizedName);
						}
						else if (this.currentQuest.QuestTags.Test_AnySet(QuestEventManager.clearTag))
						{
							SteamFriends.SetRichPresence("description", "Clearing Zombies in the " + this.currentBiome.LocalizedName);
						}
						else
						{
							flag3 = false;
						}
					}
					if (!flag3)
					{
						if (this.currentDriving)
						{
							if (this.currentDriving is EntityBicycle)
							{
								SteamFriends.SetRichPresence("description", "Pedaling through the " + this.currentBiome.LocalizedName);
							}
							else if (this.currentDriving is EntityVGyroCopter)
							{
								SteamFriends.SetRichPresence("description", "Flying through the " + this.currentBiome.LocalizedName);
							}
							else
							{
								SteamFriends.SetRichPresence("description", "Cruising through the " + this.currentBiome.LocalizedName);
							}
						}
						else
						{
							if (this.currentPrefab != null)
							{
								flag3 = true;
								if (this.currentPrefab.prefab.bTraderArea)
								{
									SteamFriends.SetRichPresence("description", "Visiting " + this.currentPrefab.prefab.LocalizedName + " in the " + this.currentBiome.LocalizedName);
								}
								else if (this.currentIndoors)
								{
									switch (world.GetGameRandom().RandomRange(3))
									{
									case 0:
										SteamFriends.SetRichPresence("description", "Scavenging in the " + this.currentBiome.LocalizedName);
										break;
									case 1:
										SteamFriends.SetRichPresence("description", "Looting Supplies in the " + this.currentBiome.LocalizedName);
										break;
									case 2:
										SteamFriends.SetRichPresence("description", "Finding Supplies in the " + this.currentBiome.LocalizedName);
										break;
									}
								}
								else
								{
									flag3 = false;
								}
							}
							if (!flag3)
							{
								switch (world.GetGameRandom().RandomRange(4))
								{
								case 0:
									SteamFriends.SetRichPresence("description", "Exploring the " + this.currentBiome.LocalizedName);
									break;
								case 1:
									SteamFriends.SetRichPresence("description", "Wandering the " + this.currentBiome.LocalizedName);
									break;
								case 2:
									SteamFriends.SetRichPresence("description", "Roaming the " + this.currentBiome.LocalizedName);
									break;
								case 3:
									SteamFriends.SetRichPresence("description", "Navigating the " + this.currentBiome.LocalizedName);
									break;
								}
							}
						}
					}
					SteamFriends.SetRichPresence("steam_display", "#Status_InGame");
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0400A4E4 RID: 42212
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal localPlayer;

		// Token: 0x0400A4E5 RID: 42213
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentTimeOfDay = "";

		// Token: 0x0400A4E6 RID: 42214
		[PublicizedFrom(EAccessModifier.Private)]
		public BiomeDefinition currentBiome;

		// Token: 0x0400A4E7 RID: 42215
		[PublicizedFrom(EAccessModifier.Private)]
		public PrefabInstance currentPrefab;

		// Token: 0x0400A4E8 RID: 42216
		[PublicizedFrom(EAccessModifier.Private)]
		public bool currentIndoors;

		// Token: 0x0400A4E9 RID: 42217
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityVehicle currentDriving;

		// Token: 0x0400A4EA RID: 42218
		[PublicizedFrom(EAccessModifier.Private)]
		public Quest currentQuest;

		// Token: 0x0400A4EB RID: 42219
		[PublicizedFrom(EAccessModifier.Private)]
		public bool currentBloodmoon;
	}
}
