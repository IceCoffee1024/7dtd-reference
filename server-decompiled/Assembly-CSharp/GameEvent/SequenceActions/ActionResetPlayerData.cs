using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AD RID: 6573
	[Preserve]
	public class ActionResetPlayerData : ActionBaseClientAction
	{
		// Token: 0x0600C93E RID: 51518 RVA: 0x0049DB18 File Offset: 0x0049BD18
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

		// Token: 0x0600C93F RID: 51519 RVA: 0x0049DDF4 File Offset: 0x0049BFF4
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

		// Token: 0x0600C940 RID: 51520 RVA: 0x0049E0D8 File Offset: 0x0049C2D8
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

		// Token: 0x0600C941 RID: 51521 RVA: 0x0049E198 File Offset: 0x0049C398
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

		// Token: 0x04009873 RID: 39027
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetLevels;

		// Token: 0x04009874 RID: 39028
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeLandclaims;

		// Token: 0x04009875 RID: 39029
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeSleepingBag;

		// Token: 0x04009876 RID: 39030
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetSkills;

		// Token: 0x04009877 RID: 39031
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeBooks;

		// Token: 0x04009878 RID: 39032
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeCrafting;

		// Token: 0x04009879 RID: 39033
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeQuests;

		// Token: 0x0400987A RID: 39034
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeChallenges;

		// Token: 0x0400987B RID: 39035
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool removeBackpack;

		// Token: 0x0400987C RID: 39036
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool resetStats;

		// Token: 0x0400987D RID: 39037
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetLevels = "reset_levels";

		// Token: 0x0400987E RID: 39038
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetSkills = "reset_skills";

		// Token: 0x0400987F RID: 39039
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveLandClaims = "remove_landclaims";

		// Token: 0x04009880 RID: 39040
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveSleepingBag = "remove_bedroll";

		// Token: 0x04009881 RID: 39041
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveBooks = "reset_books";

		// Token: 0x04009882 RID: 39042
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveCrafting = "reset_crafting";

		// Token: 0x04009883 RID: 39043
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveQuests = "remove_quests";

		// Token: 0x04009884 RID: 39044
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveChallenges = "remove_challenges";

		// Token: 0x04009885 RID: 39045
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemoveBackpack = "remove_backpack";

		// Token: 0x04009886 RID: 39046
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropResetStats = "reset_stats";
	}
}
