using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200193D RID: 6461
	[Preserve]
	public class ActionAddSpawnedEntitiesToGroup : BaseAction
	{
		// Token: 0x0600C670 RID: 50800 RVA: 0x0048F050 File Offset: 0x0048D250
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

		// Token: 0x0600C671 RID: 50801 RVA: 0x0048F11C File Offset: 0x0048D31C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropTag, ref this.tag);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropExcludeBuff, ref this.excludeBuff);
			properties.ParseBool(ActionAddSpawnedEntitiesToGroup.PropTargetOnly, ref this.targetOnly);
		}

		// Token: 0x0600C672 RID: 50802 RVA: 0x0048F174 File Offset: 0x0048D374
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

		// Token: 0x04009595 RID: 38293
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009596 RID: 38294
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x04009597 RID: 38295
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x04009598 RID: 38296
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeBuff = "";

		// Token: 0x04009599 RID: 38297
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400959A RID: 38298
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x0400959B RID: 38299
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x0400959C RID: 38300
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeBuff = "exclude_buff";
	}
}
