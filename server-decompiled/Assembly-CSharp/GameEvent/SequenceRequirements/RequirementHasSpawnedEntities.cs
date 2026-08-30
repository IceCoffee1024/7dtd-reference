using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001932 RID: 6450
	[Preserve]
	public class RequirementHasSpawnedEntities : BaseRequirement
	{
		// Token: 0x0600C70C RID: 50956 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C70D RID: 50957 RVA: 0x00491170 File Offset: 0x0048F370
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

		// Token: 0x0600C70E RID: 50958 RVA: 0x004911E0 File Offset: 0x0048F3E0
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

		// Token: 0x0600C70F RID: 50959 RVA: 0x0049124C File Offset: 0x0048F44C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasSpawnedEntities
			{
				tag = this.tag,
				targetOnly = this.targetOnly
			};
		}

		// Token: 0x040095EA RID: 38378
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x040095EB RID: 38379
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x040095EC RID: 38380
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x040095ED RID: 38381
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";
	}
}
