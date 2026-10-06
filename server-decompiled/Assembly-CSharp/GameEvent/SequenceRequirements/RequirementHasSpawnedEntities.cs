using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200195E RID: 6494
	[Preserve]
	public class RequirementHasSpawnedEntities : BaseRequirement
	{
		// Token: 0x0600C864 RID: 51300 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C865 RID: 51301 RVA: 0x0049565C File Offset: 0x0049385C
		public override bool CanPerform(Entity target)
		{
			FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.Parse(this.tag);
			for (int i = 0; i < GameEventManager.Current.spawnEntries.Count; i++)
			{
				GameEventManager.SpawnEntry spawnEntry = GameEventManager.Current.spawnEntries[i];
				if (spawnEntry.SpawnedEntity.HasAnyTags(fastTags) && (!this.targetOnly || spawnEntry.SpawnedEntity.spawnById == target.entityId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C866 RID: 51302 RVA: 0x004956CC File Offset: 0x004938CC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementHasSpawnedEntities.PropTag))
			{
				this.tag = properties.Values[RequirementHasSpawnedEntities.PropTag];
			}
			if (properties.Values.ContainsKey(RequirementHasSpawnedEntities.PropTargetOnly))
			{
				this.targetOnly = StringParsers.ParseBool(properties.Values[RequirementHasSpawnedEntities.PropTargetOnly], 0, -1, true);
			}
		}

		// Token: 0x0600C867 RID: 51303 RVA: 0x00495738 File Offset: 0x00493938
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasSpawnedEntities
			{
				tag = this.tag,
				targetOnly = this.targetOnly
			};
		}

		// Token: 0x040096B0 RID: 38576
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x040096B1 RID: 38577
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x040096B2 RID: 38578
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x040096B3 RID: 38579
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";
	}
}
