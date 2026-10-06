using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198F RID: 6543
	[Preserve]
	public class ActionAddSpawnedEntitiesToGroup : BaseAction
	{
		// Token: 0x0600C94F RID: 51535 RVA: 0x0049988C File Offset: 0x00497A8C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.Parse(this.tag);
			List<Entity> list = new List<Entity>();
			for (int i = 0; i < GameEventManager.Current.spawnEntries.Count; i++)
			{
				GameEventManager.SpawnEntry spawnEntry = GameEventManager.Current.spawnEntries[i];
				if (spawnEntry.SpawnedEntity.HasAnyTags(fastTags) && (!this.targetOnly || spawnEntry.SpawnedEntity.spawnById == base.Owner.Target.entityId) && (this.excludeBuff == "" || !spawnEntry.SpawnedEntity.Buffs.HasBuff(this.excludeBuff)))
				{
					list.Add(spawnEntry.SpawnedEntity);
				}
			}
			base.Owner.AddEntitiesToGroup(this.groupName, list, false);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C950 RID: 51536 RVA: 0x00499958 File Offset: 0x00497B58
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropTag, ref this.tag);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropExcludeBuff, ref this.excludeBuff);
			properties.ParseBool(ActionAddSpawnedEntitiesToGroup.PropTargetOnly, ref this.targetOnly);
		}

		// Token: 0x0600C951 RID: 51537 RVA: 0x004999B0 File Offset: 0x00497BB0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddSpawnedEntitiesToGroup
			{
				tag = this.tag,
				groupName = this.groupName,
				targetOnly = this.targetOnly,
				excludeBuff = this.excludeBuff
			};
		}

		// Token: 0x04009784 RID: 38788
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009785 RID: 38789
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x04009786 RID: 38790
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x04009787 RID: 38791
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeBuff = "";

		// Token: 0x04009788 RID: 38792
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009789 RID: 38793
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x0400978A RID: 38794
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x0400978B RID: 38795
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeBuff = "exclude_buff";
	}
}
