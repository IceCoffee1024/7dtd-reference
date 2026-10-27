using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0A RID: 6666
	[Preserve]
	public class ActionResetPlayerData : ActionBaseClientAction
	{
		// Token: 0x0600CBAA RID: 52138 RVA: 0x004A5B08 File Offset: 0x004A3D08
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				GameManager instance = GameManager.Instance;
				if (this.removeQuests)
				{
					entityPlayerLocal.Buffs.RemoveCustomVar("IntroComplete");
					entityPlayerLocal.QuestJournal.Clear();
				}
				if (this.removeBackpack)
				{
					entityPlayerLocal.SetDroppedBackpackPositions(null);
					if (entityPlayerLocal.persistentPlayerData != null)
					{
						entityPlayerLocal.persistentPlayerData.ClearDroppedBackpacks();
					}
				}
				entityPlayerLocal.Progression.ResetProgression(this.resetLevels || this.resetSkills, this.removeBooks, this.removeCrafting);
				if (this.resetLevels)
				{
					entityPlayerLocal.Progression.Level = 1;
					entityPlayerLocal.Progression.ExpToNextLevel = entityPlayerLocal.Progression.GetExpForNextLevel();
					entityPlayerLocal.Progression.SkillPoints = entityPlayerLocal.QuestJournal.GetRewardedSkillPoints();
					entityPlayerLocal.Progression.ExpDeficit = 0;
					entityPlayerLocal.Buffs.SetCustomVar("$PlayerLevelBonus", 0f, true, CVarOperation.set, false);
					entityPlayerLocal.Buffs.SetCustomVar("$LastPlayerLevel", 1f, true, CVarOperation.set, false);
				}
				if (this.resetStats)
				{
					entityPlayerLocal.KilledZombies = 0;
					entityPlayerLocal.KilledPlayers = 0;
					entityPlayerLocal.Died = 0;
					entityPlayerLocal.distanceWalked = 0f;
					entityPlayerLocal.totalItemsCrafted = 0U;
					entityPlayerLocal.longestLife = 0f;
					entityPlayerLocal.currentLife = 0f;
				}
				if (this.removeCrafting)
				{
					List<Recipe> recipes = CraftingManager.GetRecipes();
					for (int i = 0; i < recipes.Count; i++)
					{
						if (recipes[i].IsLearnable)
						{
							entityPlayerLocal.Buffs.RemoveCustomVar(recipes[i].GetName());
						}
					}
					List<string> list = null;
					foreach (KeyValuePair<string, float> keyValuePair in entityPlayerLocal.Buffs.EnumerateCustomVars("_craftCount_", true))
					{
						if (list == null)
						{
							list = new List<string>();
						}
						list.Add(keyValuePair.Key);
					}
					if (list != null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							entityPlayerLocal.Buffs.RemoveCustomVar(list[j]);
						}
					}
				}
				if (this.removeLandclaims)
				{
					PersistentPlayerData playerDataFromEntityID = instance.persistentPlayers.GetPlayerDataFromEntityID(target.entityId);
					if (playerDataFromEntityID.LPBlocks != null)
					{
						for (int k = 0; k < playerDataFromEntityID.LPBlocks.Count; k++)
						{
							instance.persistentPlayers.m_lpBlockMap.Remove(playerDataFromEntityID.LPBlocks[k]);
						}
						playerDataFromEntityID.LPBlocks.Clear();
					}
					NavObjectManager.Instance.UnRegisterNavObjectByOwnerEntity(entityPlayerLocal, "land_claim");
				}
				if (this.removeSleepingBag)
				{
					PersistentPlayerData playerDataFromEntityID2 = instance.persistentPlayers.GetPlayerDataFromEntityID(target.entityId);
					entityPlayerLocal.RemoveSpawnPoints(false);
					playerDataFromEntityID2.ClearBedroll();
				}
				if (this.removeChallenges)
				{
					entityPlayerLocal.challengeJournal.ResetChallenges();
				}
			}
		}

		// Token: 0x0600CBAB RID: 52139 RVA: 0x004A5DE4 File Offset: 0x004A3FE4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnServerPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				GameManager instance = GameManager.Instance;
				PersistentPlayerData playerDataFromEntityID = instance.persistentPlayers.GetPlayerDataFromEntityID(target.entityId);
				if (this.removeBackpack)
				{
					List<Entity> list = instance.World.Entities.list;
					for (int i = 0; i < list.Count; i++)
					{
						EntityBackpack entityBackpack = list[i] as EntityBackpack;
						if (entityBackpack != null && entityBackpack.RefPlayerId == entityPlayer.entityId)
						{
							entityBackpack.RefPlayerId = -1;
						}
					}
					entityPlayer.ClearDroppedBackpackPositions();
					if (playerDataFromEntityID != null)
					{
						playerDataFromEntityID.ClearDroppedBackpacks();
					}
				}
				entityPlayer.Progression.ResetProgression(this.resetLevels || this.resetSkills, this.removeBooks, this.removeCrafting);
				if (this.resetLevels)
				{
					entityPlayer.Progression.Level = 1;
					entityPlayer.Progression.ExpToNextLevel = entityPlayer.Progression.GetExpForNextLevel();
					entityPlayer.Progression.SkillPoints = entityPlayer.QuestJournal.GetRewardedSkillPoints();
					entityPlayer.Progression.ExpDeficit = 0;
					entityPlayer.Buffs.SetCustomVar("$PlayerLevelBonus", 0f, true, CVarOperation.set, false);
					entityPlayer.Buffs.SetCustomVar("$LastPlayerLevel", 1f, true, CVarOperation.set, false);
				}
				if (this.resetStats)
				{
					entityPlayer.KilledZombies = 0;
					entityPlayer.KilledPlayers = 0;
					entityPlayer.Died = 0;
					entityPlayer.distanceWalked = 0f;
					entityPlayer.totalItemsCrafted = 0U;
					entityPlayer.longestLife = 0f;
					entityPlayer.currentLife = 0f;
				}
				if (this.removeCrafting)
				{
					List<Recipe> recipes = CraftingManager.GetRecipes();
					for (int j = 0; j < recipes.Count; j++)
					{
						if (recipes[j].IsLearnable)
						{
							entityPlayer.Buffs.RemoveCustomVar(recipes[j].GetName());
						}
					}
					List<string> list2 = null;
					foreach (KeyValuePair<string, float> keyValuePair in entityPlayer.Buffs.EnumerateCustomVars("_craftCount_", true))
					{
						if (list2 == null)
						{
							list2 = new List<string>();
						}
						list2.Add(keyValuePair.Key);
					}
					if (list2 != null)
					{
						for (int k = 0; k < list2.Count; k++)
						{
							entityPlayer.Buffs.RemoveCustomVar(list2[k]);
						}
					}
				}
				if (this.removeLandclaims && playerDataFromEntityID.LPBlocks != null)
				{
					for (int l = 0; l < playerDataFromEntityID.LPBlocks.Count; l++)
					{
						instance.persistentPlayers.m_lpBlockMap.Remove(playerDataFromEntityID.LPBlocks[l]);
					}
					playerDataFromEntityID.LPBlocks.Clear();
				}
				if (this.removeSleepingBag)
				{
					playerDataFromEntityID.ClearBedroll();
				}
				if (this.removeChallenges && entityPlayer is EntityPlayerLocal)
				{
					entityPlayer.challengeJournal.ResetChallenges();
				}
			}
		}

		// Token: 0x0600CBAC RID: 52140 RVA: 0x004A60C8 File Offset: 0x004A42C8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionResetPlayerData.PropResetLevels, ref this.resetLevels);
			properties.ParseBool(ActionResetPlayerData.PropResetSkills, ref this.resetSkills);
			properties.ParseBool(ActionResetPlayerData.PropRemoveLandClaims, ref this.removeLandclaims);
			properties.ParseBool(ActionResetPlayerData.PropRemoveSleepingBag, ref this.removeSleepingBag);
			properties.ParseBool(ActionResetPlayerData.PropRemoveBooks, ref this.removeBooks);
			properties.ParseBool(ActionResetPlayerData.PropRemoveCrafting, ref this.removeCrafting);
			properties.ParseBool(ActionResetPlayerData.PropRemoveQuests, ref this.removeQuests);
			properties.ParseBool(ActionResetPlayerData.PropRemoveChallenges, ref this.removeChallenges);
			properties.ParseBool(ActionResetPlayerData.PropRemoveBackpack, ref this.removeBackpack);
			properties.ParseBool(ActionResetPlayerData.PropResetStats, ref this.resetStats);
		}

		// Token: 0x0600CBAD RID: 52141 RVA: 0x004A6188 File Offset: 0x004A4388
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetPlayerData
			{
				resetLevels = this.resetLevels,
				resetSkills = this.resetSkills,
				removeLandclaims = this.removeLandclaims,
				targetGroup = this.targetGroup,
				removeSleepingBag = this.removeSleepingBag,
				removeBooks = this.removeBooks,
				removeCrafting = this.removeCrafting,
				removeQuests = this.removeQuests,
				removeChallenges = this.removeChallenges,
				removeBackpack = this.removeBackpack,
				resetStats = this.resetStats
			};
		}

		// Token: 0x04009A22 RID: 39458
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetLevels;

		// Token: 0x04009A23 RID: 39459
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeLandclaims;

		// Token: 0x04009A24 RID: 39460
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeSleepingBag;

		// Token: 0x04009A25 RID: 39461
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetSkills;

		// Token: 0x04009A26 RID: 39462
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeBooks;

		// Token: 0x04009A27 RID: 39463
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeCrafting;

		// Token: 0x04009A28 RID: 39464
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeQuests;

		// Token: 0x04009A29 RID: 39465
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeChallenges;

		// Token: 0x04009A2A RID: 39466
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeBackpack;

		// Token: 0x04009A2B RID: 39467
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetStats;

		// Token: 0x04009A2C RID: 39468
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetLevels = "reset_levels";

		// Token: 0x04009A2D RID: 39469
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetSkills = "reset_skills";

		// Token: 0x04009A2E RID: 39470
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveLandClaims = "remove_landclaims";

		// Token: 0x04009A2F RID: 39471
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveSleepingBag = "remove_bedroll";

		// Token: 0x04009A30 RID: 39472
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveBooks = "reset_books";

		// Token: 0x04009A31 RID: 39473
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveCrafting = "reset_crafting";

		// Token: 0x04009A32 RID: 39474
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveQuests = "remove_quests";

		// Token: 0x04009A33 RID: 39475
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveChallenges = "remove_challenges";

		// Token: 0x04009A34 RID: 39476
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveBackpack = "remove_backpack";

		// Token: 0x04009A35 RID: 39477
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetStats = "reset_stats";
	}
}
