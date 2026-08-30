using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C6 RID: 6598
	[Preserve]
	public class ActionSpawnContainer : ActionBaseSpawn
	{
		// Token: 0x0600C9AE RID: 51630 RVA: 0x0049FDE8 File Offset: 0x0049DFE8
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

		// Token: 0x0600C9AF RID: 51631 RVA: 0x0049FE5A File Offset: 0x0049E05A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSpawnContainer.PropOverrideLootList, ref this.overrideLootList);
			properties.ParseString(ActionSpawnContainer.PropOverrideName, ref this.overrideName);
		}

		// Token: 0x0600C9B0 RID: 51632 RVA: 0x0049FE88 File Offset: 0x0049E088
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

		// Token: 0x040098F5 RID: 39157
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideLootList = "";

		// Token: 0x040098F6 RID: 39158
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideName = "";

		// Token: 0x040098F7 RID: 39159
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideLootList = "override_loot_list";

		// Token: 0x040098F8 RID: 39160
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideName = "override_name";
	}
}
