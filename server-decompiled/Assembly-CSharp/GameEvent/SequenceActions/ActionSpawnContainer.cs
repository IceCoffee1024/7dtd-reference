using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A23 RID: 6691
	[Preserve]
	public class ActionSpawnContainer : ActionBaseSpawn
	{
		// Token: 0x0600CC1A RID: 52250 RVA: 0x004A7D88 File Offset: 0x004A5F88
		public override void AddPropertiesToSpawnedEntity(Entity entity)
		{
			base.AddPropertiesToSpawnedEntity(entity);
			entity.spawnByAllowShare = base.Owner.CrateShare;
			EntityLootContainer entityLootContainer = entity as EntityLootContainer;
			if (entityLootContainer != null)
			{
				if (this.overrideLootList != "")
				{
					string[] array = this.overrideLootList.Split(',', StringSplitOptions.None);
					entityLootContainer.OverrideLootList = array[entity.rand.RandomRange(array.Length)];
				}
				entityLootContainer.OverrideName = this.overrideName;
			}
		}

		// Token: 0x0600CC1B RID: 52251 RVA: 0x004A7DFA File Offset: 0x004A5FFA
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSpawnContainer.PropOverrideLootList, ref this.overrideLootList);
			properties.ParseString(ActionSpawnContainer.PropOverrideName, ref this.overrideName);
		}

		// Token: 0x0600CC1C RID: 52252 RVA: 0x004A7E28 File Offset: 0x004A6028
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSpawnContainer
			{
				count = this.count,
				currentCount = this.currentCount,
				entityNames = this.entityNames,
				maxDistance = this.maxDistance,
				minDistance = this.minDistance,
				safeSpawn = this.safeSpawn,
				airSpawn = this.airSpawn,
				singleChoice = this.singleChoice,
				targetGroup = this.targetGroup,
				partyAdditionText = this.partyAdditionText,
				AddToGroup = this.AddToGroup,
				AddToGroups = this.AddToGroups,
				spawnType = this.spawnType,
				clearPositionOnComplete = this.clearPositionOnComplete,
				yOffset = this.yOffset,
				useEntityGroup = this.useEntityGroup,
				ignoreMultiplier = this.ignoreMultiplier,
				raycastOffset = this.raycastOffset,
				isAggressive = false,
				spawnSound = this.spawnSound,
				overrideLootList = this.overrideLootList,
				overrideName = this.overrideName
			};
		}

		// Token: 0x04009AA4 RID: 39588
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideLootList = "";

		// Token: 0x04009AA5 RID: 39589
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideName = "";

		// Token: 0x04009AA6 RID: 39590
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideLootList = "override_loot_list";

		// Token: 0x04009AA7 RID: 39591
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideName = "override_name";
	}
}
