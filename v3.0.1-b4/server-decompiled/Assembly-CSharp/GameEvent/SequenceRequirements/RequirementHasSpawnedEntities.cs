using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200190C RID: 6412
	[Preserve]
	public class RequirementHasSpawnedEntities : BaseRequirement
	{
		// Token: 0x0600C585 RID: 50565 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C586 RID: 50566 RVA: 0x0048AE24 File Offset: 0x00489024
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

		// Token: 0x0600C587 RID: 50567 RVA: 0x0048AE94 File Offset: 0x00489094
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

		// Token: 0x0600C588 RID: 50568 RVA: 0x0048AF00 File Offset: 0x00489100
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasSpawnedEntities
			{
				tag = this.tag,
				targetOnly = this.targetOnly
			};
		}

		// Token: 0x040094C1 RID: 38081
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x040094C2 RID: 38082
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x040094C3 RID: 38083
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x040094C4 RID: 38084
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";
	}
}
