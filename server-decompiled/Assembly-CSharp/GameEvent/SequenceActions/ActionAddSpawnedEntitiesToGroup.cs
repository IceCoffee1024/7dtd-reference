using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001963 RID: 6499
	[Preserve]
	public class ActionAddSpawnedEntitiesToGroup : BaseAction
	{
		// Token: 0x0600C7F7 RID: 51191 RVA: 0x0049539C File Offset: 0x0049359C
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

		// Token: 0x0600C7F8 RID: 51192 RVA: 0x00495468 File Offset: 0x00493668
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropTag, ref this.tag);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropExcludeBuff, ref this.excludeBuff);
			properties.ParseBool(ActionAddSpawnedEntitiesToGroup.PropTargetOnly, ref this.targetOnly);
		}

		// Token: 0x0600C7F9 RID: 51193 RVA: 0x004954C0 File Offset: 0x004936C0
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

		// Token: 0x040096BE RID: 38590
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x040096BF RID: 38591
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x040096C0 RID: 38592
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x040096C1 RID: 38593
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeBuff = "";

		// Token: 0x040096C2 RID: 38594
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x040096C3 RID: 38595
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x040096C4 RID: 38596
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x040096C5 RID: 38597
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeBuff = "exclude_buff";
	}
}
