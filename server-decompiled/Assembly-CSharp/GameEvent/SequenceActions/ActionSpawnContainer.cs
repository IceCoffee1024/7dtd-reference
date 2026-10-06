using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F2 RID: 6642
	[Preserve]
	public class ActionSpawnContainer : ActionBaseSpawn
	{
		// Token: 0x0600CB06 RID: 51974 RVA: 0x004A428C File Offset: 0x004A248C
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

		// Token: 0x0600CB07 RID: 51975 RVA: 0x004A42FE File Offset: 0x004A24FE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSpawnContainer.PropOverrideLootList, ref this.overrideLootList);
			properties.ParseString(ActionSpawnContainer.PropOverrideName, ref this.overrideName);
		}

		// Token: 0x0600CB08 RID: 51976 RVA: 0x004A432C File Offset: 0x004A252C
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

		// Token: 0x040099BB RID: 39355
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideLootList = "";

		// Token: 0x040099BC RID: 39356
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideName = "";

		// Token: 0x040099BD RID: 39357
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideLootList = "override_loot_list";

		// Token: 0x040099BE RID: 39358
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideName = "override_name";
	}
}
