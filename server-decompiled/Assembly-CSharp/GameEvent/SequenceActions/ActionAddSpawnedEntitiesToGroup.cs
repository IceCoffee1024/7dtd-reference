using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C0 RID: 6592
	[Preserve]
	public class ActionAddSpawnedEntitiesToGroup : BaseAction
	{
		// Token: 0x0600CA63 RID: 51811 RVA: 0x0049D38C File Offset: 0x0049B58C
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

		// Token: 0x0600CA64 RID: 51812 RVA: 0x0049D458 File Offset: 0x0049B658
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropTag, ref this.tag);
			properties.ParseString(ActionAddSpawnedEntitiesToGroup.PropExcludeBuff, ref this.excludeBuff);
			properties.ParseBool(ActionAddSpawnedEntitiesToGroup.PropTargetOnly, ref this.targetOnly);
		}

		// Token: 0x0600CA65 RID: 51813 RVA: 0x0049D4B0 File Offset: 0x0049B6B0
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

		// Token: 0x0400986D RID: 39021
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400986E RID: 39022
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x0400986F RID: 39023
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetOnly;

		// Token: 0x04009870 RID: 39024
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeBuff = "";

		// Token: 0x04009871 RID: 39025
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009872 RID: 39026
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x04009873 RID: 39027
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetOnly = "target_only";

		// Token: 0x04009874 RID: 39028
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeBuff = "exclude_buff";
	}
}
