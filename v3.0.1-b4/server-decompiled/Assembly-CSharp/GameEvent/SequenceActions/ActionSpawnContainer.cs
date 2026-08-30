using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A0 RID: 6560
	[Preserve]
	public class ActionSpawnContainer : ActionBaseSpawn
	{
		// Token: 0x0600C827 RID: 51239 RVA: 0x00499A90 File Offset: 0x00497C90
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

		// Token: 0x0600C828 RID: 51240 RVA: 0x00499B02 File Offset: 0x00497D02
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSpawnContainer.PropOverrideLootList, ref this.overrideLootList);
			properties.ParseString(ActionSpawnContainer.PropOverrideName, ref this.overrideName);
		}

		// Token: 0x0600C829 RID: 51241 RVA: 0x00499B30 File Offset: 0x00497D30
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

		// Token: 0x040097CC RID: 38860
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideLootList = "";

		// Token: 0x040097CD RID: 38861
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideName = "";

		// Token: 0x040097CE RID: 38862
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideLootList = "override_loot_list";

		// Token: 0x040097CF RID: 38863
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideName = "override_name";
	}
}
