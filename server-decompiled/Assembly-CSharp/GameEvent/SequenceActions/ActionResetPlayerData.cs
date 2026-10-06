using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D9 RID: 6617
	[Preserve]
	public class ActionResetPlayerData : ActionBaseClientAction
	{
		// Token: 0x0600CA96 RID: 51862 RVA: 0x004A2008 File Offset: 0x004A0208
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

		// Token: 0x0600CA97 RID: 51863 RVA: 0x004A22E4 File Offset: 0x004A04E4
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

		// Token: 0x0600CA98 RID: 51864 RVA: 0x004A25C8 File Offset: 0x004A07C8
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

		// Token: 0x0600CA99 RID: 51865 RVA: 0x004A2688 File Offset: 0x004A0888
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

		// Token: 0x04009939 RID: 39225
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetLevels;

		// Token: 0x0400993A RID: 39226
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeLandclaims;

		// Token: 0x0400993B RID: 39227
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeSleepingBag;

		// Token: 0x0400993C RID: 39228
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetSkills;

		// Token: 0x0400993D RID: 39229
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeBooks;

		// Token: 0x0400993E RID: 39230
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeCrafting;

		// Token: 0x0400993F RID: 39231
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeQuests;

		// Token: 0x04009940 RID: 39232
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeChallenges;

		// Token: 0x04009941 RID: 39233
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeBackpack;

		// Token: 0x04009942 RID: 39234
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetStats;

		// Token: 0x04009943 RID: 39235
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetLevels = "reset_levels";

		// Token: 0x04009944 RID: 39236
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetSkills = "reset_skills";

		// Token: 0x04009945 RID: 39237
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveLandClaims = "remove_landclaims";

		// Token: 0x04009946 RID: 39238
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveSleepingBag = "remove_bedroll";

		// Token: 0x04009947 RID: 39239
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveBooks = "reset_books";

		// Token: 0x04009948 RID: 39240
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveCrafting = "reset_crafting";

		// Token: 0x04009949 RID: 39241
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveQuests = "remove_quests";

		// Token: 0x0400994A RID: 39242
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveChallenges = "remove_challenges";

		// Token: 0x0400994B RID: 39243
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveBackpack = "remove_backpack";

		// Token: 0x0400994C RID: 39244
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetStats = "reset_stats";
	}
}
